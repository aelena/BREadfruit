﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BREadfruit.Clauses;
using BREadfruit.Conditions;
using BREadfruit.Helpers;

namespace BREadfruit
{
    public class Parser
    {

        private LineParser _lineParser = new LineParser ();
        private List<LineInfo> _parsedLines = new List<LineInfo> ();

        private IList<Entity> _entities = new List<Entity> ();
        internal CurrentScope _currentScope { get; private set; }

        public IEnumerable<LineInfo> ParsedLines
        {
            get
            { return this._parsedLines; }
        }

        public IEnumerable<Entity> Entities
        {
            get
            {
                return this._entities;
            }
        }


        // ---------------------------------------------------------------------------------


        public void ParseRuleFile ( string filePath )
        {

            if ( String.IsNullOrWhiteSpace ( filePath ) )
                throw new ArgumentNullException ( filePath );

            if ( !File.Exists ( filePath ) )
                throw new FileNotFoundException ( "rule file not found" );



            using ( var sr = new StreamReader ( filePath ) )
            {
                var _currLine = 0;
                var line = String.Empty;
                while ( line != null )
                {

                    line = sr.ReadLine ();
                    if ( !( String.IsNullOrWhiteSpace ( line ) ) )
                    {

                        var lineInfo = ParseLine ( line );
                        if ( lineInfo.Tokens.Count () == 0 )
                            continue;

                        if ( lineInfo.Tokens.First ().Token.Equals ( Grammar.EntitySymbol.Token, StringComparison.InvariantCultureIgnoreCase ) )
                        {
                            this._currentScope = CurrentScope.NEW_ENTITY;
                            this._entities.Add ( new Entity ( lineInfo.Tokens.ElementAt ( 1 ).Token,
                                lineInfo.Tokens.ElementAt ( 3 ).Token ) );
                        }
                        if ( lineInfo.Tokens.First ().Token.Equals ( Grammar.WithSymbol.Token, StringComparison.InvariantCultureIgnoreCase ) )
                        {
                            // Clear the scope state
                            this._currentScope = CurrentScope.NO_SCOPE;
                            if ( LineParser.IsAValidSentence ( lineInfo ) )
                            {
                                // check the scope of the with clause and change the state accordingly                            
                                var _withScope = lineInfo.Tokens.Last ().Token;
                                if ( _withScope.Equals ( Grammar.DefaultsSymbol.Token ) )
                                    this._currentScope = CurrentScope.DEFAULTS_BLOCK;
                                if ( _withScope.Equals ( Grammar.RulesSymbol.Token ) )
                                    this._currentScope = CurrentScope.RULES_BLOCK;
                                if ( _withScope.Equals ( Grammar.ConstraintsSymbol.Token ) )
                                    this._currentScope = CurrentScope.CONSTRAINTS_BLOCK;
                                if ( _withScope.Equals ( Grammar.TriggersSymbol.Token ) )
                                    this._currentScope = CurrentScope.TRIGGERS_BLOCK;
                            }
                            else
                                throw new ArgumentException ( String.Format (
                                    "Invalid with clause found in line {0} in rule file {1}",
                                    _currLine, filePath ) );

                            if ( this._currentScope != CurrentScope.NO_SCOPE )
                                continue;   // ugly... but for the moment...
                        }
                        // now, if we're in the scope of the defaults block
                        if ( this._currentScope == CurrentScope.DEFAULTS_BLOCK )
                        {
                            // then try and parse a default clause
                            var clause = Grammar.GetDefaultClauseByToken ( lineInfo.Tokens.First ().Token, false );
                            if ( clause != null )
                            {
                                // if we only have one token, it means we are dealing with a single-element instruction such as 'enabled'
                                // in that case we assumed the instruction to always be true
                                // TODO: further validation to be added here to ensure we accept only boolean values here
                                if ( lineInfo.Tokens.Count () == 1 )
                                    clause.SetValue ( "true" );
                                // if there are two tokens, it means there are no comments, and we're dealing with a default instruction+
                                // such as width 100 or enabled true, and then we assume the last token is the value
                                if ( lineInfo.Tokens.Count () == 2 )
                                    clause.SetValue ( lineInfo.Tokens.Last ().Token );
                                // if there are more tokens, it's comments most likely, but let's keep this separate for the time being
                                // altough it could be the same for this and the previous case just by using ElementAt(1)
                                if ( lineInfo.Tokens.Count () > 2 )
                                    clause.SetValue ( lineInfo.Tokens.ElementAt ( 1 ).Token );


                                this._entities.Last ().AddDefaultClause ( clause );
                            }
                        }
                        if ( this._currentScope == CurrentScope.RULES_BLOCK )
                        {
                            lineInfo = ParseLine ( LineParser.TokenizeMultiplePartOperators ( lineInfo ) );

                            var _rule = new Rule ();
                            var _cond = new Condition ( lineInfo.Tokens.First ().Token,
                                                        Grammar.GetOperator ( lineInfo.Tokens.ElementAt ( 1 ).Token ),
                                                        lineInfo.Tokens.ElementAt ( 2 ).Token );
                            _rule.AddCondition ( _cond );
                            // check presence of token 'then'
                            if ( lineInfo.Tokens.Contains ( Grammar.ThenSymbol ) )
                            {
                                // and check too if after then, tehre is some action or more nested conditions
                                if ( lineInfo.Tokens.Last ().Equals ( Grammar.ThenSymbol ) )
                                {
                                    // then there are nested conditions, so
                                    continue;
                                }
                                else
                                {
                                    // if it's not the last then we have the action right after the token
                                    // check if it's a valid action
                                    if ( lineInfo.Tokens.Penultimate () == Grammar.ThenSymbol )
                                    {
                                        var _unaryAction = new UnaryAction ( Grammar.GetSymbolByToken ( lineInfo.Tokens.Last ().Token ) );
                                        _rule.AddUnaryAction ( _unaryAction );
                                    }
                                    // then it's a long result action line
                                    if ( lineInfo.Tokens.ElementAtFromLast ( 3 ) == Grammar.ThenSymbol )
                                    {
                                        var _unaryAction = new UnaryAction ( Grammar.GetSymbolByToken ( lineInfo.Tokens.Penultimate ().Token ),
                                            lineInfo.Tokens.Last ().Token );
                                        _rule.AddUnaryAction ( _unaryAction );
                                    }
                                    if ( lineInfo.Tokens.ElementAtFromLast ( 5 ) == Grammar.ThenSymbol )
                                    {
                                        if ( lineInfo.Tokens.Penultimate () != Grammar.InSymbol )
                                            throw new Exception ( String.Format ( "Line {0} seems to be missing token 'in'", line ) );
                                        var thenClause = LineInfo.AfterThen ( lineInfo );
                                        var _ra = new ResultAction ( Grammar.GetSymbolByToken ( thenClause.First ().Token ),
                                            thenClause.ElementAt ( 1 ).Token, thenClause.Last ().Token );
                                        _rule.AddResultAction ( _ra );


                                    }

                                }

                                this._entities.Last ().AddRule ( _rule );
                            }
                            else
                                throw new Exception (
                                    String.Format ( "Found Invalid Condition Syntax - symbol 'then' missing in line '{0}'",
                                        lineInfo.Representation ) );

                        }

                    }

                    _currLine++;

                }   // close of while loop

            }

        }


        // ---------------------------------------------------------------------------------


        protected internal LineInfo ParseLine ( string line )
        {
            var lineInfo = LineParser.ParseLine ( line );
            _parsedLines.Add ( lineInfo );
            return lineInfo;
        }


        // ---------------------------------------------------------------------------------



    }



    enum CurrentScope
    {
        /// <summary>
        /// Empty scope
        /// </summary>
        NO_SCOPE,
        /// <summary>
        /// ENtity node
        /// </summary>
        NEW_ENTITY,
        /// <summary>
        /// Signals we're in the defaults block for a business rule
        /// </summary>
        DEFAULTS_BLOCK,
        /// <summary>
        /// Signals we're in the constraints block for a business rule
        /// </summary>
        CONSTRAINTS_BLOCK,
        TRIGGERS_BLOCK,
        RULES_BLOCK
    }

}
