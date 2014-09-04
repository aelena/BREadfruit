using System;
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
                // current line counter
                var _currLine = 0;
                // current line in the text file
                var line = String.Empty;

                while ( line != null )
                {

                    line = sr.ReadLine ();
                    if ( !( String.IsNullOrWhiteSpace ( line ) ) )
                    {

                        var lineInfo = ParseLine ( line );
                        if ( lineInfo.Tokens.Count () == 0 )
                            continue;

                        // we need to check the indent level to make sure we set the right scope
                        // this is for conditions with several action lines
                        if ( _currentScope == CurrentScope.CONDITION_ACTIONS_BLOCK && lineInfo.IndentLevel == 2 )
                            _currentScope = CurrentScope.RULES_BLOCK;

                        #region " --- entity block --- "
                        if ( lineInfo.Tokens.First ().Token.Equals ( Grammar.EntitySymbol.Token, StringComparison.InvariantCultureIgnoreCase ) )
                        {
                            this._currentScope = CurrentScope.NEW_ENTITY;
                            this._entities.Add ( new Entity ( lineInfo.Tokens.ElementAt ( 1 ).Token,
                                lineInfo.Tokens.ElementAt ( 3 ).Token ) );
                        }
                        #endregion

                        #region " --- with block --- "
                        if ( lineInfo.Tokens.First ().Token.Equals ( Grammar.WithSymbol.Token, StringComparison.InvariantCultureIgnoreCase ) )
                        {
                            // Clear the scope state
                            this._currentScope = CurrentScope.NO_SCOPE;
                            if ( this._lineParser.IsAValidSentence ( lineInfo ) )
                            {
                                // check the scope of the with clause and change the state accordingly                            
                                var _withScope = lineInfo.Tokens.Last ().Token;
                                if ( _withScope.Equals ( Grammar.DefaultsSymbol.Token ) )
                                    this._currentScope = CurrentScope.DEFAULTS_BLOCK;
                                if ( _withScope.Equals ( Grammar.ActionsSymbol.Token ) )
                                    this._currentScope = CurrentScope.ACTIONS_BLOCK;
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
                            this._entities.Last ().AddDefaultClause ( this.ConfigureDefaultClause ( lineInfo ) );
                        }
                        #endregion

                        #region " --- rules block --- "
                        if ( this._currentScope == CurrentScope.RULES_BLOCK )
                        {

                            lineInfo = ParseLine ( this._lineParser.TokenizeMultiplePartOperators ( lineInfo ) );

                            var _rule = new Rule ();
                            var _conds = this._lineParser.ExtractConditions ( lineInfo );
                            foreach ( var c in _conds )
                                _rule.AddCondition ( c );

                            // check presence of token 'then'
                            if ( lineInfo.Tokens.Contains ( Grammar.ThenSymbol ) )
                            {
                                // if then is the last token, then there are actions
                                if ( lineInfo.Tokens.Last ().Equals ( Grammar.ThenSymbol ) )
                                {
                                    _currentScope = CurrentScope.CONDITION_ACTIONS_BLOCK;
                                    continue;
                                }
                                else
                                {
                                    // if it's not the last then we have the action right after the token
                                    // check if it's a valid action
                                    if ( lineInfo.Tokens.Penultimate () == Grammar.ThenSymbol )
                                    {
                                        var _unaryAction = new UnaryAction ( Grammar.GetSymbolByToken ( lineInfo.Tokens.Last ().Token ) );
                                        _rule.Conditions.Last ().AddUnaryAction ( _unaryAction );
                                    }
                                    // then it's a long result action line
                                    if ( lineInfo.Tokens.ElementAtFromLast ( 3 ) == Grammar.ThenSymbol )
                                    {
                                        var _unaryAction = new UnaryAction ( Grammar.GetSymbolByToken ( lineInfo.Tokens.Penultimate ().Token ),
                                            lineInfo.Tokens.Last ().Token );
                                        _rule.Conditions.Last ().AddUnaryAction ( _unaryAction );
                                    }
                                    if ( lineInfo.Tokens.ElementAtFromLast ( 5 ) == Grammar.ThenSymbol )
                                    {
                                        if ( lineInfo.Tokens.Penultimate () != Grammar.InSymbol )
                                            throw new Exception ( String.Format ( "Line {0} seems to be missing token 'in'", line ) );
                                        var thenClause = lineInfo.GetSymbolsAfterThen ();
                                        var _ra = new ResultAction ( Grammar.GetSymbolByToken ( thenClause.First ().Token ),
                                            thenClause.ElementAt ( 1 ).Token, thenClause.Last ().Token );
                                        _rule.Conditions.Last ().AddResultAction ( _ra );
                                    }

                                }

                                this._entities.Last ().AddRule ( _rule );
                            }
                            else
                                throw new Exception (
                                    String.Format ( "Found Invalid Condition Syntax - symbol 'then' missing in line '{0}'",
                                        lineInfo.Representation ) );

                        }
                        #endregion

                        #region " --- condition-less actions block --- "
                        if ( this._currentScope == CurrentScope.ACTIONS_BLOCK )
                        {
                            lineInfo = ParseLine ( this._lineParser.TokenizeMultiplePartOperators ( lineInfo ) );
                            // see if this is resultaction
                            if ( lineInfo.Tokens.Count () == 4 )
                            {
                                var _ra = new ResultAction ( Grammar.GetSymbolByToken ( lineInfo.Tokens.First ().Token ),
                                            lineInfo.Tokens.ElementAt ( 1 ).Token, lineInfo.Tokens.Last ().Token );
                                this._entities.Last ().AddResultAction ( _ra );
                            }
                            // or unary action
                            if ( lineInfo.Tokens.Count () == 2 )
                            {
                                var _ua = new UnaryAction ( Grammar.GetSymbolByToken ( lineInfo.Tokens.First ().Token ),
                                    lineInfo.Tokens.ElementAt ( 1 ).Token );
                                this._entities.Last ().AddUnaryAction ( _ua );
                            }
                        }
                        #endregion

                        #region " --- condition  actions block --- "
                        if ( this._currentScope == CurrentScope.CONDITION_ACTIONS_BLOCK )
                        {
                            lineInfo = ParseLine ( this._lineParser.TokenizeMultiplePartOperators ( lineInfo ) );
                            if ( lineInfo.Tokens.Count () == 4 && lineInfo.Tokens.Contains ( Grammar.InSymbol ) )
                            {
                                var _ra = new ResultAction ( Grammar.GetSymbolByToken ( lineInfo.Tokens.First ().Token ),
                                            lineInfo.Tokens.ElementAt ( 1 ).Token, lineInfo.Tokens.Last ().Token );
                                this._entities.Last ().Rules.Last ().Conditions.Last ().AddResultAction ( _ra );
                            }
                        }
                        #endregion

                        if ( this._currentScope == CurrentScope.TRIGGERS_BLOCK )
                        {
                            lineInfo = ParseLine ( this._lineParser.TokenizeMultiplePartOperators ( lineInfo ) );
                            {
                                if ( lineInfo.Tokens.Count () == 2 )
                                {
                                    var _firstToken = lineInfo.Tokens.First ().Token;
                                    if ( lineInfo.Tokens.First ().Token.Trim ().StartsWith ( "this." ) )
                                        _firstToken = _firstToken.Replace ( "this", this._entities.Last ().Name );
                                    var _trigger = new Trigger ( _firstToken, lineInfo.Tokens.ElementAt ( 1 ).Token );
                                    this._entities.Last ().AddTrigger ( _trigger );
                                }
                            }
                        }

                        if ( this._currentScope == CurrentScope.CONSTRAINTS_BLOCK )
                        {

                        }


                    }

                    _currLine++;

                }   // close of while loop

            }


        }


        // ---------------------------------------------------------------------------------


        private DefaultClause ConfigureDefaultClause ( LineInfo lineInfo )
        {

            lineInfo = ParseLine ( this._lineParser.TokenizeMultiplePartOperators ( lineInfo ) );
            if ( this._lineParser.LineInfoContainsArgumentkeyValuePairs ( lineInfo ) )
            {
                lineInfo = this._lineParser.TokenizeArgumentArgumentkeyValuePairs ( lineInfo );
            }

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
                {
                    clause.SetValue ( lineInfo.Tokens.ElementAt ( 1 ).Token );
                    if ( lineInfo.HasSymbol ( Grammar.WithArgumentsSymbol ) )
                    {
                        clause.AddArgumentsFromString ( lineInfo.Tokens.ElementAt ( lineInfo.IndexOfSymbol ( Grammar.WithArgumentsSymbol ) + 1 ).Token );
                    }
                }


            }
            return clause;
        }


        // ---------------------------------------------------------------------------------


        protected internal LineInfo ParseLine ( string line )
        {
            var lineInfo = this._lineParser.ParseLine ( line );
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
        /// Signals we're in the defaults block for a business rule definition
        /// </summary>
        DEFAULTS_BLOCK,
        /// <summary>
        /// Signals we're in the constraints block for a business rule definition
        /// </summary>
        CONSTRAINTS_BLOCK,
        /// <summary>
        /// Indicates we are in the constraints block of a business rule definition
        /// </summary>
        TRIGGERS_BLOCK,
        /// <summary>
        /// Indicates we are in the rules block of a business rule definition
        /// </summary>
        RULES_BLOCK,
        /// <summary>
        /// Indicates we are in the block that indicates condition-less actions
        /// </summary>
        ACTIONS_BLOCK,
        /// <summary>
        /// Indicates we are in the actions block of a business rule definition
        /// </summary>
        CONDITION_ACTIONS_BLOCK
    }

}
