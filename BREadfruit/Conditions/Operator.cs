using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BREadfruit.Conditions
{
    public class Operator : AliasedToken
    {


        public Operator ( string identifier, IEnumerable<string> aliases = null )
        {
            if ( String.IsNullOrWhiteSpace ( identifier ) )
                throw new ArgumentNullException ( "identifier", "the identifier for an Operator instance cannot be null" );

            this._name = identifier;
            this._aliases = new List<string> ();
            this._aliases.Add ( identifier );

            if ( aliases != null )
                this._aliases.AddRange ( aliases );
        }


        // ---------------------------------------------------------------------------------


       
    }
}
