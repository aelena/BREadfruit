using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BREadfruit
{
    public class Symbol
    {

        public string Token { get; private set; }
        public bool IsTerminal { get; private set; }


        private IList<Symbol> _validChildren = new List<Symbol> ();
        public IEnumerable<Symbol> ValidChildren { get; private set; }

        public Symbol ( string symbol )
        {
            this.Token = symbol;
            this.IsTerminal = true;
        }

        public Symbol ( string symbol, bool isTerminal)
        {
            this.Token = symbol;
            this.IsTerminal = isTerminal;
        }

        public Symbol ( string symbol, IList<Symbol> validChildren)
        {
            this.Token = symbol;
            this.IsTerminal = false;
            this._validChildren = validChildren;
        }

        public void AddValidChild ( Symbol s )
        {
            if ( this.IsTerminal )
                throw new InvalidOperationException ( "This is a terminal symbol. No symbols can be added." );

            if ( this._validChildren != null )
                this._validChildren.Add ( s );

            // if any sort of child Symbol is added, then this cannot be a nonterminal
            this.IsTerminal = false;
        }

      
    }
}
