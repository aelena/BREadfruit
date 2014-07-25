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

        public void ParseRuleFile ( string filePath )
        {

            if ( String.IsNullOrWhiteSpace ( filePath ) )
                throw new ArgumentNullException ( filePath );

            if ( !File.Exists ( filePath ) )
                throw new FileNotFoundException ( "rule file not found" );



            using ( var sr = new StreamReader(filePath) )
            {
                var line = String.Empty;
                while ( line != null)
                {

                    line = sr.ReadLine ();
                    if ( !( String.IsNullOrWhiteSpace ( line ) ) )
                    {

                        if ( line.StartsWith ( "Entity"))
                        {
                            ParseEntityLine ( line );
                        }

                    }
                }

            }



        }

        protected internal void ParseEntityLine ( string line )
        {
            throw new NotImplementedException ();
        }
    }
}
