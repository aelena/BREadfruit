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

        protected internal static int GetIndentCount ( string line )
        {
            if ( line != null )
                return line.ToCharArray ().Where ( x => x == '\t' ).Count ();
            return 0;
        }

        protected internal static IEnumerable<Symbol> ExtractTokens ( string line )
        {
            if ( line != null )
            {
                var _tokens = from t in line.Replace ( "\t", " " ).Split ( new [] { " " },
                              StringSplitOptions.RemoveEmptyEntries )
                              select
                              new Symbol ( t );
                return _tokens;
            }
            return null;
        }

        public bool IsAValidSentence ( LineInfo line )
        {
            // considering the opening token of a line info
            switch ( line.Tokens.First ().Token.ToUpperInvariant () )
            {
                case "ENTITY":
                    return this.ValidateEntityLine ( line );

            }

            return false;
        }



        private bool ValidateEntityLine ( LineInfo line )
        {
            var _entityLineRegex = new Regex ( Grammar.EntityLineRegex );
            return _entityLineRegex.IsMatch ( line.Representation.ToUpperInvariant () );
        }

    }
}
