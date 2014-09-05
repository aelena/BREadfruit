﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using BREadfruit.Clauses;
using BREadfruit.Conditions;
using BREadfruit.Helpers;

namespace BREadfruit
{
    /// <summary>
    /// Main class that takes care of parsing lines from the 
    /// business rule file.
    /// </summary>
    internal class LineParser
    {

        /// <summary>
        /// Parses a string representation of a line as read from the file.
        /// </summary>
        /// <param name="line">String containing all the line information.</param>
        /// <returns>Returns a configured instance of LineInfo.</returns>
        public LineInfo ParseLine ( string line )
        {
            // check the indentation leven in the string representation
            var indentLevel = GetIndentCount ( line );
            // split the line in spaces; replace any tabs just in case the user
            // put tabs in between tokens
            var _tokens = ExtractTokens ( line );
            // create the LineInfo instance, passing the original string represnetation, the tokens found
            // and preserving the indentation level
            var li = new LineInfo ( line, indentLevel, _tokens );
           
            return li;
        }


        // ---------------------------------------------------------------------------------


        /// <summary>
        /// Returns the number of tabs in the string passed as argument.
        /// </summary>
        /// <param name="line"></param>
        /// <returns></returns>
        protected internal int GetIndentCount ( string line )
        {
            if ( line != null )
                return line.Where ( x => x == '\t' ).Count ();
            return 0;
        }


        // ---------------------------------------------------------------------------------


        /// <summary>
        /// get tokens from string 
        /// if the token exists in the grammar, then it adds the grammar-declared token
        /// otherwise, as in the case of literals or other symbols,
        /// instantiates a new Symbol and adds it to the collection.
        /// </summary>
        /// <param name="line"></param>
        /// <returns></returns>
        protected internal IEnumerable<Symbol> ExtractTokens ( string line )
        {
            if ( line != null )
            {
                // this poses the details of the last token being really the nontermoinal or not....
                // maybe this need some more careful thinking
                var _tokens = from t in line.Replace ( "\t", " " ).Split ( new [] { " " },
                                 StringSplitOptions.RemoveEmptyEntries )
                              let grammarToken = Grammar.GetSymbolByToken ( t, false )
                              select
                                 grammarToken == null ?
                                      new Symbol ( t, line.ToCharArray ().Where ( x => x == '\t' ).Count (), false )
                                      : grammarToken;

                // parse comments out
                // there are no multiline or inline comments but there can be trailing comments at the end of the line
                _tokens = _tokens.TakeWhile ( z => !z.Token.StartsWith ( ";" ) );

                // now we need to detect and unify any tokens in between single or double quotes as one token
                // so first let's just see if there are any sort of quotes in the line itself
                // no quote should actually be fount at 0...
                if ( line.IndexOfAny ( new char [] { '"', '\'' } ) >= 0 )
                {
                    var _t1 = Enumerable.Range ( 0, _tokens.Count () ).Where ( x => _tokens.ElementAt ( x ).Token.StartsWith ( "'" ) || _tokens.ElementAt ( x ).Token.StartsWith ( "\"" ) );
                    var _t2 = Enumerable.Range ( 0, _tokens.Count () ).Where ( x => _tokens.ElementAt ( x ).Token.EndsWith ( "'" ) || _tokens.ElementAt ( x ).Token.StartsWith ( "\"" ) );

                    // count should be the same
                    if ( _t1.Count () == _t2.Count () )
                    {
                        var _fusedSymbols = new List<Symbol> ();
                        // now "fuse" the symbols that are within quotes
                        // TODO: as this is valid for strings (user strings) any keywords here are considered just string
                        for ( var i = 0; i < _t1.Count (); i++ )
                            _fusedSymbols.Add ( _tokens.ToList ().GetRange ( _t1.ElementAt ( i ), ( _t2.ElementAt ( i ) - _t1.ElementAt ( i ) ) + 1 ).JoinTogether () );

                        List<Symbol> replacedLineInfo = new List<Symbol> ();
                        for ( int i = 0, j = 0; i < _tokens.Count (); i++ )
                        {
                            if ( _t1.Contains ( i ) )
                            {
                                replacedLineInfo.Add ( _fusedSymbols.ElementAt ( j ) );
                                i = _t2.ElementAt ( j );    // move the index
                                j++;
                            }
                            else
                                replacedLineInfo.Add ( _tokens.ElementAt ( i ) );
                        }

                        return replacedLineInfo;
                    }
                    else
                    {
                        throw new Exception ( String.Format ( "There seems to be a mismatch in the usage of quotes in line {0}", line ) );
                    }
                }

                return _tokens;
            }
            return null;
        }


        // ---------------------------------------------------------------------------------


        /// <summary>
        /// Takes care of validating lines found in the file for business rules.
        /// </summary>
        /// <param name="line"></param>
        /// <returns></returns>
        public bool IsAValidSentence ( LineInfo line )
        {
            if ( line.Tokens != null && line.Tokens.Count () > 0 )
            {
                // considering the opening token of a line info
                switch ( line.Tokens.First ().Token.ToUpperInvariant () )
                {
                    case "ENTITY":
                        return this.ValidateEntityStatement ( line );
                    case "WITH":
                        return this.ValidateWithStatement ( line );

                }
            }

            return false;
        }


        // ---------------------------------------------------------------------------------


        /// <summary>
        /// This function detects keywords or operators that have multi-word tokens or aliases
        /// (such as "load data from" which is actually a single instruction)
        /// and performs a substitution to convert them into single-word tokens 
        /// according to the Grammar specs. This means that in effect, the number of token
        /// is generally reduced here or remains the same if no multi-word tokens or aliases
        /// are found in the LineInfo instance passed as argument.
        /// </summary>
        /// <param name="li"></param>
        /// <returns></returns>
        public string TokenizeMultiplePartOperators ( LineInfo li /*IEnumerable<Symbol> tokens, string line*/ )
        {

            var _conditionValueList = li.TokenizeValueListInCondition ();
            if (  _conditionValueList != null )
            {
                li.RemoveTokensFromTo ( _conditionValueList.Item2, _conditionValueList.Item3 + 1 );
                li.InsertTokenAt ( _conditionValueList.Item2, new Symbol ( _conditionValueList.Item1, 2 ) );
            }

            // take a repreeentation of the entire list of tokens as a simple string
            var _fused = li.Tokens.JoinTogether ().Token;
            var _replacedString = _fused;

            // now check for occurrence
            var _x = from op in Grammar.Symbols
                     where op is Symbol
                     || op is ResultAction
                     || op is DefaultClause
                     || op is Operator
                     && op.Aliases.Count () > 0
                     let t = _fused.ContainsAny2 ( op.Aliases )
                     where t.Item1
                     select new
                     {
                         symbol = op,
                         Ocurrence = t.Item2,
                         Index = _fused.IndexOf ( t.Item2 ),
                         Length = t.Item2.Length
                     };


            if ( _x != null && _x.Count () > 0 )
            {
                _x.ToList ().ForEach ( x => _replacedString = _replacedString.Replace ( x.Ocurrence, x.symbol.Token ) );
            }
            _fused = _replacedString;

           
            _replacedString = _replacedString.Prepend ( "\t", li.IndentLevel );
            return _replacedString;
        }

        // ---------------------------------------------------------------------------------


        #region " --- specific line validations --- "


        // ---------------------------------------------------------------------------------


        private bool ValidateEntityStatement ( LineInfo line )
        {
            var _entityLineRegex = new Regex ( Grammar.EntityLineRegex );
            return _entityLineRegex.IsMatch ( line.Representation.ToUpperInvariant () );
        }


        // ---------------------------------------------------------------------------------


        private bool ValidateWithStatement ( LineInfo line )
        {
            var r = new Regex ( Grammar.WithLineRegex, RegexOptions.IgnoreCase );
            if ( !r.IsMatch ( line.Representation ) )
                return false;
            if ( line.IndentLevel != Grammar.WithSymbol.IndentLevel )
                return false;
            // verify the only child token in a 'with' statement is correct
            if ( !Grammar.WithSymbol.Children.Contains ( line.Tokens.ElementAt ( 1 ) ) )
                return false;

            return true;
        }


        // ---------------------------------------------------------------------------------


        #endregion


        // ---------------------------------------------------------------------------------


        public IEnumerable<Condition> ExtractConditions ( LineInfo lineInfo )
        {
            var _conds = new List<Condition> ();
            int __ands = 0, __ors = 0;
            if ( lineInfo.Tokens.Contains ( Grammar.ANDSymbol ) )
                __ands = lineInfo.Tokens.Count ( x => x == Grammar.ANDSymbol );
            if ( lineInfo.Tokens.Contains ( Grammar.ORSymbol ) )
                __ors = lineInfo.Tokens.Count ( x => x == Grammar.ORSymbol );
            __ands += __ors;
            for ( int i = 0; i < 3 * ( __ands + 1 ); )
            {
                var _c = new Condition ( lineInfo.Tokens.ElementAt ( i ).Token,
                                                   Grammar.GetOperator ( lineInfo.Tokens.ElementAt ( ++i ).Token ),
                                                   lineInfo.Tokens.ElementAt ( ++i ).Token );
                _conds.Add ( _c );

                var _logical = lineInfo.Tokens.ElementAtOrDefault ( ++i );
                if ( _logical != null )
                {
                    if ( new List<string> () { Grammar.ANDSymbol.Token, Grammar.ORSymbol.Token }.Contains ( _logical.Token ) )
                    {
                        _conds.Last ().SetLogicalOperator ( _logical.Token );
                    }
                }
                i += 1;
            }


            return _conds;
        }


        // ---------------------------------------------------------------------------------


        /// <summary>
        /// This function parses a lineInfo to detect argument key value pairs 
        /// and make a single token of all the individual tokens.
        /// This token is to be parsed later on for extracting the individual key valuye pairs.
        /// </summary>
        /// <param name="lineInfo"></param>
        /// <returns></returns>
        protected internal LineInfo TokenizeArgumentArgumentkeyValuePairs ( LineInfo lineInfo )
        {
            var _argsAsString = lineInfo.GetArgumentsAsString ();
            if ( !String.IsNullOrWhiteSpace ( _argsAsString ) )
            {
                lineInfo.RemoveTokensAfterSymbol ( Grammar.WithArgumentsSymbol, true );
                lineInfo.AddToken ( new Symbol ( _argsAsString, 2, true ) );
            }
            return lineInfo;
        }


        // ---------------------------------------------------------------------------------


        /// <summary>
        /// Returns a value that indicates if the passed LineInfo instance contains
        /// a "with arguments" clause.
        /// </summary>
        /// <param name="lineInfo">LineInfo instance to inspect.</param>
        /// <returns></returns>
        protected internal bool LineInfoContainsArgumentkeyValuePairs ( LineInfo lineInfo )
        {
            return lineInfo.HasSymbol ( Grammar.WithArgumentsSymbol );
        }


        // ---------------------------------------------------------------------------------

    }
}
