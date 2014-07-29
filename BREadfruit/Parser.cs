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
                var line = String.Empty;
                while ( line != null )
                {

                    line = sr.ReadLine ();
                    if ( !( String.IsNullOrWhiteSpace ( line ) ) )
                    {

                        ParseLine ( line );

                    }
                }

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
}
