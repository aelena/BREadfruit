using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using BREadfruit.Conditions;
using BREadfruit.Helpers;

namespace BREadfruit
{
    internal class LineParser
    {

        public static LineInfo ParseLine ( string line )
        {

            var indentLevel = GetIndentCount ( line );
            // split the line in spaces; replace any tabs just in case the user
            // put tabs in between tokens
            var _tokens = ExtractTokens ( line );
            return new LineInfo ( line, indentLevel, _tokens );

        }


        // ---------------------------------------------------------------------------------


        protected internal static int GetIndentCount ( string line )
        {
            if ( line != null )
                return line.Where ( x => x == '\t' ).Count ();
            return 0;
        }


        // ---------------------------------------------------------------------------------


        protected internal static IEnumerable<Symbol> ExtractTokens ( string line )
        {
            if ( line != null )
            {
                // get tokens from string 
                // if the token exists in the grammar, then add the grammar-declared token
                // otherwise, as in teh case of literals or other symbols
                // instantiate a new Symbol and add it to the collection
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
                _tokens = _tokens.TakeWhile ( z => !z.Token.StartsWith ( ";" ) );

                // now we need to detect and unify any tokens in between single or double quotes as one token
                // so first let's just see if there are any sort of quotes in the line itself
                // no quote should actually be fount at 0...
                if ( line.IndexOfAny ( new char [] { '"', '\'' } ) >= 0 )
                {
                    // if so, then find all the opening quotes
                    //var _t1 = _tokens.Where ( x => x.Token.StartsWith ( "'" ) || x.Token.StartsWith ( "\"" ) );
                    //// there could be several quoted items in the entire line
                    //// find all the closing
                    //var _t2 = _tokens.Where ( x => x.Token.EndsWith ( "'" ) || x.Token.EndsWith ( "\"" ) );

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
                        //
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
        public static bool IsAValidSentence ( LineInfo line )
        {
            if ( line.Tokens != null && line.Tokens.Count () > 0 )
            {
                // considering the opening token of a line info
                switch ( line.Tokens.First ().Token.ToUpperInvariant () )
                {
                    case "ENTITY":
                        return LineParser.ValidateEntityStatement ( line );
                    case "WITH":
                        return LineParser.ValidateWithStatement ( line );

                }
            }

            return false;
        }


        // ---------------------------------------------------------------------------------


        public static string TokenizeMultiplePartOperators ( LineInfo li /*IEnumerable<Symbol> tokens, string line*/ )
        {

            // take a repreeentation of the entire list of tokens as a simple string
            var _fused = li.Tokens.JoinTogether ().Token;
            var _replacedString = _fused;
            foreach ( var i in new List<int> { 1, 2 } )
            {

                // now check for occurrence
                var _x = from op in Grammar.Symbols
                         where op is AliasedSymbol
                         || op is ResultAction
                         let t = _fused.ContainsAny2 ( ( ( AliasedSymbol ) op ).Aliases )
                         where t.Item1
                         select new
                         {
                             symbol = op,
                             Ocurrence = t.Item2,
                             Index = _fused.IndexOf ( op.Token )
                         };

                if ( _x != null && _x.Count () > 0 )
                    _x.ToList ().ForEach ( x => _replacedString = _replacedString.Replace ( x.Ocurrence, x.symbol.Token ) );
                _fused = _replacedString;
            }
            // restore original tabs
            _replacedString = _replacedString.Prepend ( "\t", li.IndentLevel );
            return _replacedString;
        }

        // ---------------------------------------------------------------------------------




        #region " --- specific line validations --- "


        // ---------------------------------------------------------------------------------


        private static bool ValidateEntityStatement ( LineInfo line )
        {
            var _entityLineRegex = new Regex ( Grammar.EntityLineRegex );
            return _entityLineRegex.IsMatch ( line.Representation.ToUpperInvariant () );
        }


        // ---------------------------------------------------------------------------------


        private static bool ValidateWithStatement ( LineInfo line )
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


        public static IEnumerable<Condition> ExtractConditions ( LineInfo lineInfo )
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
                        _conds.Last().SetLogicalOperator ( _logical.Token );
                    }
                }
                i += 1;
            }


            return _conds;
        }

        // ---------------------------------------------------------------------------------

    }
}
