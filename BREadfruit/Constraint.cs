using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BREadfruit
{
    public class Constraint
    {
        private readonly string _constraintName;
        /// <summary>
        /// Gets an naming descriptor that indicates the kind 
        /// of constraint this represents
        /// </summary>
        public string Name
        {
            get { return _constraintName; }
        }


        public Constraint ( string name)
        {
            this._constraintName = name;
        }

        // ---------------------------------------------------------------------------------


        public override string ToString ()
        {
            return this.Name;
        }


        // ---------------------------------------------------------------------------------


        public static bool operator == ( Constraint constraint, Symbol y )
        {
            var t = Grammar.GetSymbolByToken ( constraint.Name);
            return t == y;
        }


        // ---------------------------------------------------------------------------------


        public static bool operator != ( Constraint constraint, Symbol y )
        {
            var t = Grammar.GetSymbolByToken ( constraint.Name );
            return t != y;
        }


        // ---------------------------------------------------------------------------------

    }
}
