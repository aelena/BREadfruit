﻿using System;
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


        public IEnumerable<LineInfo> ParsedLines
        {
            get
            { return this._parsedLines; }
        }



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

        protected internal void ParseLine ( string line )
        {
            _parsedLines.Add ( LineParser.ParseLine ( line ) );
        }
    }
}
