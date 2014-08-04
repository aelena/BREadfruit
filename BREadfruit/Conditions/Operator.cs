using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BREadfruit.Conditions
{
    public class Operator : AliasedSymbol
    {


        public Operator ( string identifier, int indentLevel, bool isTerminal, IEnumerable<string> aliases = null )
            : base ( identifier, indentLevel, isTerminal )
        {
            if ( String.IsNullOrWhiteSpace ( identifier ) )
                throw new ArgumentNullException ( "identifier", "the identifier for an Operator instance cannot be null" );

            this.Token = identifier;
            this._aliases = new List<string> ();
            this._aliases.Add ( identifier );

            if ( aliases != null )
                this._aliases.AddRange ( aliases );

        }


        // ---------------------------------------------------------------------------------



    }
}
