﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using BREadfruit.Clauses;
using BREadfruit.Conditions;
using BREadfruit.Exceptions;
using BREadfruit.Helpers;

namespace BREadfruit
{
    public class Parser
    {

        private LineParser _lineParser = new LineParser ();
        private List<LineInfo> _parsedLines = new List<LineInfo> ();

        private IList<Entity> _entities = new List<Entity> ();


        /// <summary>
        /// Returns a collection of all parsed line in the form
        /// of a LineInfo list.
        /// </summary>
        public IEnumerable<LineInfo> ParsedLines
        {
            get
            { return this._parsedLines; }
        }

        /// <summary>
        /// Main object graph, where all parsed entities and all their data
        /// (defaults, rules, triggers, actions and constraints) are to be found.
        /// </summary>
        internal IEnumerable<Entity> Entities
        {
            get
            {
                return this._entities;
            }
        }


        // ---------------------------------------------------------------------------------


        /// <summary>
        /// Main entry point for parsing a file.
        /// Reads a file containing rules and store every line in array
        /// in order to pass the read lines to the parsing engine.
        /// </summary>
        /// <param name="filePath">File containing rules to be parsed.</param>
        /// <returns>A collection of entities.</returns>
        public IEnumerable<Entity> ParseRuleSet ( string filePath )
        {
            this.CheckFileExists ( filePath );

            var line = String.Empty;
            List<string> lines = new List<string> ();
            using ( var sr = new StreamReader ( filePath ) )
            {
                while ( line != null )
                {
                    line = sr.ReadLine ();
                    lines.Add ( line );
                }
            }

            return ParseRuleSet ( lines );
        }


        // ---------------------------------------------------------------------------------


        /// <summary>
        /// Main entry point for parsing a file.
        /// Allows a caller to send in a list of strings (standard text lines)
        /// containing all the entities and rules to be parsed by the engine.
        /// </summary>
        /// <param name="filePath">Array of lines to be parsed.</param>
        public IEnumerable<Entity> ParseRuleSet ( IEnumerable<string> lines )
        {


            // current line counter
            var _currLine = 0;
            var _currentScope = CurrentScope.NO_SCOPE;

            foreach ( var line in lines )
            {
                _currLine++;
                if ( !( String.IsNullOrWhiteSpace ( line ) ) )
                {

                    var lineInfo = ParseLine ( line );


                    // if this is a full comment line, skip it
                    if ( lineInfo.Tokens.Count () == 0 )
                        continue;
                    lineInfo = ParseLine ( this._lineParser.TokenizeMultiplePartOperators ( lineInfo ) );
                    // we need to check the indent level to make sure we set the right scope
                    // this is for conditions with several action lines
                    if ( _currentScope == CurrentScope.CONDITION_ACTIONS_BLOCK && lineInfo.IndentLevel == 2 )
                        _currentScope = CurrentScope.RULES_BLOCK;


                    #region " --- entity block --- "
                    if ( lineInfo.Tokens.First ().Token.Equals ( Grammar.EntitySymbol.Token, StringComparison.InvariantCultureIgnoreCase ) )
                    {
                        this.CheckIfVisibleAndEnabledDefaultsAreSet ();
                        _currentScope = ProcessEntityBlock ( _currLine, _currentScope, line, lineInfo );
                    }
                    #endregion


                    #region " --- with block --- "
                    if ( lineInfo.Tokens.First ().Token.Equals ( Grammar.WithSymbol.Token, StringComparison.InvariantCultureIgnoreCase ) )
                    {
                        _currentScope = this.ProcessWithClauseStatement ( _currLine, _currentScope, line, lineInfo );
                        if ( _currentScope != CurrentScope.NO_SCOPE )
                            continue;   // ugly... but for the moment...
                    }
                    #endregion


                    #region " --- default block --- "
                    // now, if we're in the scope of the defaults block
                    if ( _currentScope == CurrentScope.DEFAULTS_BLOCK )
                    {
                        // then try and parse a default clause
                        this._entities.Last ().AddDefaultClause ( this.ConfigureDefaultClause ( lineInfo ) );
                    }
                    #endregion


                    #region " --- rules block --- "
                    if ( _currentScope == CurrentScope.RULES_BLOCK )
                    {

                        var _rule = new Rule ();
                        var _conds = this._lineParser.ExtractConditions ( lineInfo, this._entities.Last ().Name );
                        foreach ( var c in _conds )
                            _rule.AddCondition ( c );

                        this._entities.Last ().AddRule ( _rule );

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
                                    var _unaryAction = new UnaryAction ( Grammar.GetSymbolByToken ( lineInfo.Tokens.Last ().Token ),
                                        this.Entities.Last ().Name );
                                    _rule.Conditions.Last ().AddUnaryAction ( _unaryAction );
                                    continue;
                                }
                                // then it's a long result action line
                                if ( lineInfo.Tokens.ElementAtFromLast ( 3 ) == Grammar.ThenSymbol )
                                {
                                    var _penultimate = Grammar.GetSymbolByToken ( lineInfo.Tokens.Penultimate ().Token );
                                    UnaryAction _unaryAction = null;
                                    if ( _penultimate != null )
                                        _unaryAction = new UnaryAction ( Grammar.GetSymbolByToken ( lineInfo.Tokens.Penultimate ().Token ),
                                           lineInfo.Tokens.Last ().Token );
                                    else
                                        _unaryAction = new UnaryAction ( Grammar.GetSymbolByToken ( lineInfo.Tokens.Last ().Token ),
                                            lineInfo.Tokens.Penultimate ().Token );
                                    _rule.Conditions.Last ().AddUnaryAction ( _unaryAction );
                                    continue;
                                }
                                if ( lineInfo.Tokens.ElementAtFromLast ( 4 ) == Grammar.ThenSymbol )
                                {
                                    ResultAction _ra = null;
                                    var thenClause = lineInfo.GetSymbolsAfterThen ();
                                    if ( thenClause.Any ( x => x == Grammar.ThisSymbol ) )
                                    {
                                        // this should be the value
                                        var _index = thenClause.Where ( x => !Grammar.Symbols.Contains ( x ) ).First();
                                        var _symbol = thenClause.Where ( x => Grammar.Symbols.Contains ( x ) ).First();
                                        _ra = new ResultAction ( _symbol , _index, this._entities.Last().Name);
                                    }
                                    else
                                    {
                                        _ra = new ResultAction ( Grammar.GetSymbolByToken ( thenClause.First ().Token ),
                                           thenClause.ElementAt ( 1 ).Token, thenClause.Last ().Token );
                                    }
                                    _rule.Conditions.Last ().AddResultAction ( _ra );
                                    continue;
                                }
                                if ( lineInfo.Tokens.ElementAtFromLast ( 5 ) == Grammar.ThenSymbol )
                                {
                                    if ( lineInfo.Tokens.Penultimate () != Grammar.InSymbol )
                                        throw new MissingInClauseException ( String.Format ( Grammar.MissingInClauseExceptionMessageTemplate, _currLine, line ) );
                                    var thenClause = lineInfo.GetSymbolsAfterThen ();
                                    var _ra = new ResultAction ( Grammar.GetSymbolByToken ( thenClause.First ().Token ),
                                        thenClause.ElementAt ( 1 ).Token, thenClause.Last ().Token );
                                    _rule.Conditions.Last ().AddResultAction ( _ra );
                                    continue;
                                }

                                //// last case here
                                //// detect then clauses that have commans in their actions in order to parse those...
                                //var thenClause2 = lineInfo.GetSymbolsAfterThen ();
                                //if ( thenClause2.JoinTogether ().Token.IndexOf ( "," ) > 0 )
                                //{
                                //}

                                // tentatively, if we make it here, we haven't parsed something...
                                throw new InvalidLineFoundException ( String.Format ( Grammar.InvalidLineFoundExceptionDefaultTemplate, _currLine, line ) );

                            }

                            //this._entities.Last ().AddRule ( _rule );
                        }
                        else
                            throw new Exception (
                                String.Format ( Grammar.MissingThenClauseExceptionMessageTemplate, _currLine,
                                    lineInfo.Representation ) );

                    }
                    #endregion


                    #region " --- condition-less actions block --- "
                    if ( _currentScope == CurrentScope.ACTIONS_BLOCK )
                    {

                        if ( this._lineParser.LineInfoContainsArgumentkeyValuePairs ( lineInfo ) )
                        {
                            lineInfo = this._lineParser.TokenizeArgumentKeyValuePairs ( lineInfo );
                        }
                        if ( lineInfo.Tokens.Count () == 6 )
                        {
                            if ( lineInfo.HasSymbol ( Grammar.WithArgumentsSymbol ) && lineInfo.HasSymbol ( Grammar.InSymbol ) )
                            {
                                // then it must be this sort of rule
                                // load data from DATASOURCE.ACTIVE_CCs with arguments {"Country" : this.value} in DDLCDCompany
                                var _ra = new ResultAction ( Grammar.GetSymbolByToken ( lineInfo.Tokens.First ().Token ),
                                        lineInfo.Tokens.ElementAt ( 1 ).Token, lineInfo.Tokens.Last ().Token );

                                // quite brittle this one here...
                                // replace with extension such as ElementAfterToken(xxx) or something like that.
                                var _args = lineInfo.Tokens.ElementAt ( 3 ).Token.Split ( new char [] { ',' }, StringSplitOptions.RemoveEmptyEntries );
                                foreach ( var _a in _args )
                                {
                                    var _argPair = _a.Split ( new char [] { ':' }, StringSplitOptions.RemoveEmptyEntries );
                                    _ra.AddArgument ( _argPair.First ().Trim (), _argPair.Last ().Trim () );
                                }

                                this._entities.Last ().AddResultAction ( _ra );
                            }
                        }
                        // see if this is resultaction
                        if ( lineInfo.Tokens.Count () == 4 )
                        {
                            if ( lineInfo.Tokens.Contains ( Grammar.WithArgumentsSymbol ) )
                            {
                                var _r = new ResultAction ( Grammar.GetSymbolByToken ( lineInfo.Tokens.First ().Token ),
                                       lineInfo.Tokens.ElementAt ( 1 ).Token );
                                var _args = lineInfo.Tokens.ElementAt ( 3 ).Token.Split ( new char [] { ',' }, StringSplitOptions.RemoveEmptyEntries );
                                foreach ( var _a in _args )
                                {
                                    var _argPair = _a.Split ( new char [] { ':' }, StringSplitOptions.RemoveEmptyEntries );
                                    _r.AddArgument ( _argPair.First ().Trim (), _argPair.Last ().Trim () );
                                }
                                this._entities.Last ().AddResultAction ( _r );
                            }
                            else
                            {
                                var _ra = new ResultAction ( Grammar.GetSymbolByToken ( lineInfo.Tokens.First ().Token ),
                                            lineInfo.Tokens.ElementAt ( 1 ).Token, lineInfo.Tokens.Last ().Token );
                                this._entities.Last ().AddResultAction ( _ra );
                            }
                        }
                        // or unary action
                        if ( lineInfo.Tokens.Count () == 2 )
                        {
                            if ( this._lineParser.IsAValidSentence ( lineInfo ) )
                            {
                                var _ua = new UnaryAction ( Grammar.GetSymbolByToken ( lineInfo.Tokens.First ().Token ),
                                    lineInfo.Tokens.ElementAt ( 1 ).Token );
                                this._entities.Last ().AddUnaryAction ( _ua );
                            }
                            else
                                throw new InvalidLineFoundException ( String.Format ( Grammar.InvalidLineFoundExceptionDefaultTemplate, _currLine, line ) );
                        }
                    }
                    #endregion


                    #region " --- condition  actions block --- "
                    if ( _currentScope == CurrentScope.CONDITION_ACTIONS_BLOCK )
                    {

                        // TODO: REFACTOR THIS ENTIRE SECTION AND MAKE MORE HOMOGENEOUS -------------------
                        // unary action
                        if ( lineInfo.Tokens.Count () == 2 )
                        {
                            if ( lineInfo.Tokens.First () == Grammar.ShowElementUnaryActionSymbol )
                            {
                                if ( Regex.IsMatch ( lineInfo.Representation.Trim (), Grammar.ShowElementLineRegex, RegexOptions.IgnoreCase ) )
                                {
                                    var _ua = new UnaryAction ( lineInfo.Tokens.First (),
                                        lineInfo.Tokens.Last ().Token == "this" ? this.Entities.Last ().Name : lineInfo.Tokens.Last ().Token );
                                    this._entities.Last ().Rules.Last ().Conditions.Last ().AddUnaryAction ( _ua );
                                }
                                else
                                    throw new InvalidShowStatementClauseException (
                                        String.Format ( Grammar.InvalidShowElementExceptionMessageTemplate, _currLine + 1, line.Trim () ) );
                            }
                            if ( lineInfo.Tokens.First () == Grammar.HideUnaryActionSymbol )
                            {
                                if ( Regex.IsMatch ( lineInfo.Representation.Trim (), Grammar.HideElementLineRegex, RegexOptions.IgnoreCase ) )
                                {
                                    var _ua = new UnaryAction ( lineInfo.Tokens.First (),
                                        lineInfo.Tokens.Last ().Token == "this" ? this.Entities.Last ().Name : lineInfo.Tokens.Last ().Token );
                                    this._entities.Last ().Rules.Last ().Conditions.Last ().AddUnaryAction ( _ua );
                                }
                                else
                                    throw new InvalidHideStatementClauseException (
                                        String.Format ( Grammar.InvalidHideElementExceptionMessageTemplate, _currLine + 1, line.Trim () ) );
                            }

                            if ( lineInfo.Tokens.Contains ( Grammar.MakeMandatoryUnaryActionSymbol ) )
                            {
                                ProcessUnaryAction ( lineInfo, Grammar.MakeMandatoryUnaryActionSymbol );
                                continue;
                            }

                            if ( lineInfo.Tokens.Contains ( Grammar.MandatoryDefaultClause ) )
                            {
                                ProcessUnaryAction ( lineInfo, Grammar.MandatoryDefaultClause );
                                continue;
                            }

                            if ( lineInfo.Tokens.Contains ( Grammar.MakeNonMandatoryUnaryActionSymbol ) )
                            {
                                ProcessUnaryAction ( lineInfo, Grammar.MakeNonMandatoryUnaryActionSymbol );
                                continue;
                            }

                            if ( lineInfo.Tokens.Contains ( Grammar.NotVisibleUnaryActionSymbol ) )
                            {
                                ProcessUnaryAction ( lineInfo, Grammar.NotVisibleUnaryActionSymbol );
                                continue;
                            }

                            if ( lineInfo.Tokens.First () == ( Grammar.MaxlengthDefaultClause ) )
                            {
                                var _ra = new ResultAction ( Grammar.GetSymbolByToken ( lineInfo.Tokens.First ().Token ), lineInfo.Tokens.Last ().Token,
                                    this.Entities.Last ().Name );
                                this._entities.Last ().Rules.Last ().Conditions.Last ().AddResultAction ( _ra );
                                continue;
                            }

                            if ( lineInfo.Tokens.First () == ( Grammar.SetValidationRegexUnaryActionSymbol ) ||
                                 lineInfo.Tokens.First () == ( Grammar.ValidationRegexDefaultClause ) )
                            {
                                var _ra = new ResultAction ( Grammar.GetSymbolByToken ( lineInfo.Tokens.First ().Token ), lineInfo.Tokens.Last ().Token.Replace ( "\"", "" ),
                                    this.Entities.Last ().Name );
                                this._entities.Last ().Rules.Last ().Conditions.Last ().AddResultAction ( _ra );
                                continue;
                            }

                        }

                        if ( lineInfo.Tokens.Count () == 4 && lineInfo.Tokens.Contains ( Grammar.InSymbol ) )
                        {
                            var _ra = new ResultAction ( Grammar.GetSymbolByToken ( lineInfo.Tokens.First ().Token ),
                                        lineInfo.Tokens.ElementAt ( 1 ).Token, lineInfo.Tokens.Last ().Token );
                            this._entities.Last ().Rules.Last ().Conditions.Last ().AddResultAction ( _ra );
                        }
                    }
                    #endregion


                    #region " --- trigger block --- "
                    if ( _currentScope == CurrentScope.TRIGGERS_BLOCK )
                    {
                        var _x = from op in Grammar.TriggerSymbols
                                 where op.Aliases.Count () > 0
                                 && op.Aliases.Contains ( lineInfo.Tokens.Skip ( 1 ).JoinTogether ().Token )
                                 select op;

                        //if ( lineInfo.Tokens.Count () == 2 )
                        //{
                        var _firstToken = lineInfo.Tokens.First ().Token;
                        if ( lineInfo.Tokens.First ().Token.Trim ().StartsWith ( "this" ) )
                            _firstToken = _firstToken.Replace ( "this", this._entities.Last ().Name );
                        Trigger _trigger;
                        if ( _x != null && _x.Count () > 0 )
                            _trigger = new Trigger ( _firstToken, _x.First ().Token );
                        else
                            _trigger = new Trigger ( _firstToken, lineInfo.Tokens.ElementAt ( 1 ).Token );
                        this._entities.Last ().AddTrigger ( _trigger );
                        //}
                    }
                    #endregion


                    #region " --- constraints block --- "

                    if ( _currentScope == CurrentScope.CONSTRAINTS_BLOCK )
                    {
                        // there should be only one token
                        this._entities.Last ().AddConstraint ( new Constraint ( lineInfo.Tokens.First ().Token ) );
                    }

                    #endregion


                }


            }



            this.CheckIfVisibleAndEnabledDefaultsAreSet ();

            return this.Entities;


        }


        // ---------------------------------------------------------------------------------


        private void CheckIfVisibleAndEnabledDefaultsAreSet ()
        {
            if ( this._entities.Count > 0 )
            {
                // CheckIfVisibleAndEnabledDefaultsAreSet ();
                if ( !this._entities.Last ().Defaults.Any ( x => x.Token == Grammar.VisibleDefaultClause.Token ) )
                {
                    var clause = Grammar.GetDefaultClauseByToken ( Grammar.VisibleDefaultClause.Token, false );
                    clause.SetValue ( true );
                    this._entities.Last ().AddDefaultClause ( clause );
                }
                if ( !this._entities.Last ().Defaults.Any ( x => x.Token == Grammar.EnabledDefaultClause.Token ) )
                {
                    var clause = Grammar.GetDefaultClauseByToken ( Grammar.EnabledDefaultClause.Token, false );
                    clause.SetValue ( true );
                    this._entities.Last ().AddDefaultClause ( clause );
                }
            }
        }


        // ---------------------------------------------------------------------------------


        private void ProcessUnaryAction ( LineInfo lineInfo, Symbol symbol )
        {
            if ( lineInfo.Tokens.Contains ( symbol ) )
            {
                UnaryAction _ua = null;
                if ( lineInfo.Tokens.First () == symbol )
                    _ua = new UnaryAction ( lineInfo.Tokens.First (),
                           lineInfo.Tokens.Last ().Token == "this" ? this.Entities.Last ().Name : lineInfo.Tokens.Last ().Token );
                else
                    _ua = new UnaryAction ( lineInfo.Tokens.Last (),
                           lineInfo.Tokens.First ().Token == "this" ? this.Entities.First ().Name : lineInfo.Tokens.First ().Token );
                this._entities.Last ().Rules.Last ().Conditions.Last ().AddUnaryAction ( _ua );
            }
        }



        // ---------------------------------------------------------------------------------


        /// <summary>
        /// This method processes entity declarations found at the beginning
        /// of each entity block.
        /// Not meant to be used or leaked outside.
        /// </summary>
        /// <param name="_currLine">current global counter</param>
        /// <param name="_currentScope">marker for the current scope in parsing, which
        /// indicates what block the parser is parsing right now.</param>
        /// <param name="line">Current text line being parsed.</param>
        /// <param name="lineInfo">Object contained the tokens parsed</param>
        /// <returns></returns>
        private CurrentScope ProcessEntityBlock ( int _currLine, CurrentScope _currentScope, string line, LineInfo lineInfo )
        {
            if ( this._lineParser.IsAValidSentence ( lineInfo ) )
            {
                _currentScope = CurrentScope.NEW_ENTITY;
                this._entities.Add ( new Entity (
                    lineInfo.Tokens.ElementAt ( 1 ).Token,
                    lineInfo.Tokens.ElementAt ( 3 ).Token,
                    lineInfo.Tokens.Last ().Token.Replace ( "\"", "" ).Replace ( "'", "" ) ) );               // last token in form's name where the entity belongs
            }
            else
                // throw an exception if the regex validation fails 
                throw new InvalidEntityDeclarationException ( String.Format ( Grammar.InvalidEntityDeclarationExceptionMessageTemplate, _currLine + 1, line.Trim () ) );

            return _currentScope;
        }


        // ---------------------------------------------------------------------------------


        /// <summary>
        /// This method processes with clauses marking the beginning of each block
        /// of each entity block.
        /// Not meant to be used or leaked outside.
        /// </summary>
        /// <param name="_currLine">current global counter</param>
        /// <param name="_currentScope">marker for the current scope in parsing, which
        /// indicates what block the parser is parsing right now.</param>
        /// <param name="line">Current text line being parsed.</param>
        /// <param name="lineInfo">Object contained the tokens parsed</param>
        /// <returns></returns>
        private CurrentScope ProcessWithClauseStatement ( int _currLine, CurrentScope _currentScope, string line, LineInfo lineInfo )
        {
            // Clear the scope state
            _currentScope = CurrentScope.NO_SCOPE;
            if ( this._lineParser.IsAValidSentence ( lineInfo ) )
            {
                // check the scope of the with clause and change the state accordingly                            
                _currentScope = SetCurrentScope ( _currentScope, lineInfo );
            }
            else
                throw new InvalidWithClauseException ( String.Format
                    ( Grammar.InvalidWithClauseExceptionMessageTemplate, _currLine + 1, line.Trim () ) );
            return _currentScope;
        }


        // ---------------------------------------------------------------------------------


        /// <summary>
        /// Sets up the current scope according to the with clause found.
        /// </summary>
        /// <param name="_currentScope"></param>
        /// <param name="lineInfo"></param>
        /// <returns></returns>
        private CurrentScope SetCurrentScope ( CurrentScope _currentScope, LineInfo lineInfo )
        {
            var _withScope = lineInfo.Tokens.Last ().Token;
            if ( _withScope.Equals ( Grammar.DefaultsSymbol.Token ) )
                _currentScope = CurrentScope.DEFAULTS_BLOCK;
            if ( _withScope.Equals ( Grammar.ActionsSymbol.Token ) )
                _currentScope = CurrentScope.ACTIONS_BLOCK;
            if ( _withScope.Equals ( Grammar.RulesSymbol.Token ) )
                _currentScope = CurrentScope.RULES_BLOCK;
            if ( _withScope.Equals ( Grammar.ConstraintsSymbol.Token ) )
                _currentScope = CurrentScope.CONSTRAINTS_BLOCK;
            if ( _withScope.Equals ( Grammar.TriggersSymbol.Token ) )
                _currentScope = CurrentScope.TRIGGERS_BLOCK;
            return _currentScope;
        }


        // ---------------------------------------------------------------------------------


        private void CheckFileExists ( string filePath )
        {
            if ( String.IsNullOrWhiteSpace ( filePath ) )
                throw new ArgumentNullException ( filePath );

            if ( !File.Exists ( filePath ) )
                throw new FileNotFoundException ( "rule file not found" );
        }


        // ---------------------------------------------------------------------------------


        private DefaultClause ConfigureDefaultClause ( LineInfo lineInfo )
        {

            lineInfo = ParseLine ( this._lineParser.TokenizeMultiplePartOperators ( lineInfo ) );
            if ( this._lineParser.LineInfoContainsArgumentkeyValuePairs ( lineInfo ) )
            {
                lineInfo = this._lineParser.TokenizeArgumentKeyValuePairs ( lineInfo );
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
