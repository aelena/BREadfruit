using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BREadfruit.Conditions
{
    public class ResultAction : AliasedSymbol
    {

        public ResultAction ( string identifier, int indentLevel, bool isTerminal, IEnumerable<string> aliases = null )
            : base ( identifier, indentLevel, isTerminal )
        {
            if ( String.IsNullOrWhiteSpace ( identifier ) )
                throw new ArgumentNullException ( "identifier", "The identifier for a ResultAction instance cannot be null" );

            this.Token = identifier;
            this._aliases = new List<string> ();
            this._aliases.Add ( identifier );

            if ( aliases != null )
                this._aliases.AddRange ( aliases );

           
        }


        // ---------------------------------------------------------------------------------

    }
}
