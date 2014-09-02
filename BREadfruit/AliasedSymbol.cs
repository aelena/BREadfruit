using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BREadfruit
{
    public class AliasedSymbol : Symbol
    {

        

        public AliasedSymbol ( string symbol, int indentLevel )
            : base ( symbol, indentLevel )
        {
            this._aliases = new List<string> ();
        }

        // ---------------------------------------------------------------------------------

        public AliasedSymbol ( string symbol, int indentLevel, bool isTerminal )
            : base ( symbol, indentLevel, isTerminal )
        {
            this._aliases = new List<string> ();
        }

        // ---------------------------------------------------------------------------------

        public AliasedSymbol ( string symbol, int indentLevel, bool isTerminal, IEnumerable<string> aliases )
            : base ( symbol, indentLevel, isTerminal )
        {
            this._aliases = aliases.ToList();
        }
    }
}
