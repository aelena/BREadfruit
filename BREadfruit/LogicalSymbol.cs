using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BREadfruit
{
    public class LogicalOperatorSymbol : Symbol
    {

        public LogicalOperatorSymbol ( string symbol, int indentLevel )
            : base ( symbol, indentLevel )
        { }

        public LogicalOperatorSymbol ( string symbol, int indentLevel, bool isTerminal )
            : base ( symbol, indentLevel, isTerminal )
        { }

        public LogicalOperatorSymbol ( string symbol, int indentLevel, IList<Symbol> validChildren )
            : base ( symbol, indentLevel, validChildren )
        { }
    }

}
