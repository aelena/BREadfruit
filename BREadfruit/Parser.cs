using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

                        if ( lineInfo.Tokens.First ().Token.Equals ( Grammar.EntitySymbol.Token, StringComparison.InvariantCultureIgnoreCase ) )
                        {
                            this._currentScope = CurrentScope.NEW_ENTITY;
                            this._entities.Add ( new Entity ( lineInfo.Tokens.ElementAt ( 1 ).Token,
                                lineInfo.Tokens.ElementAt ( 3 ).Token ) );
                        }
                        if ( lineInfo.Tokens.First ().Token.Equals ( Grammar.WithSymbol.Token, StringComparison.InvariantCultureIgnoreCase ) )
                        {
                            if ( LineParser.IsAValidSentence ( lineInfo ) )
                            {

                            }
                            else
                                throw new ArgumentException ( String.Format (
                                    "Invalid with clause found in line {0} in rule file {1}",
                                    _currLine, filePath ) );
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
        NEW_ENTITY,
        DEFAULTS_BLOCK,
        CONSTRAINTS_BLOCK,
        TRIGGERS_BLOCK,
        RULES_BLOCK
    }

}
