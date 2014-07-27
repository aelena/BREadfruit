using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BREadfruit
{
    public class Symbol
    {
        public string Token { get; private set; }

        public Symbol(string Symbol)
        {
            this.Token = Symbol;
        }
    }
}
