using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

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
                return line.ToCharArray ().Where ( x => x == '\t' ).Count ();
            return 0;
        }


        // ---------------------------------------------------------------------------------


        protected internal static IEnumerable<Symbol> ExtractTokens ( string line )
        {
            if ( line != null )
            {
                var _tokens = from t in line.Replace ( "\t", " " ).Split ( new [] { " " },
                              StringSplitOptions.RemoveEmptyEntries )
                              select
                              new Symbol ( t, line.ToCharArray ().Where ( x => x == '\t' ).Count () );
                return _tokens;
            }
            return null;
        }


        // ---------------------------------------------------------------------------------

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

    }
}
