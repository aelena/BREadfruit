using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BREadfruit
{

    /// <summary>
    /// This class or data structure, contains all the tokens
    /// that are allowed in the document format.
    /// </summary>
    public static class Grammar
    {

        #region " --- regexs for line validation --- "
        public const string EntityLineRegex = "ENTITY [A-Za-z0-9_]* (IS|is|Is|iS)? [A-Za-z]*";


        #endregion


        #region " --- symbols --- "

        #region " --- terminals --- "


        public static Symbol DefaultsSymbol = new Symbol ( "defaults", true );
        public static Symbol ConstraintsSymbol = new Symbol ( "constraints", true );
        public static Symbol TriggersSymbol = new Symbol ( "triggers", true );

        #endregion


        #region " --- nonterminals --- "

        public static Symbol EntitySymbol = new Symbol ( "Entity", false );
        public static Symbol WithSymbol = new Symbol ( "With", new List<Symbol> () { DefaultsSymbol, ConstraintsSymbol, TriggersSymbol } );

        #endregion


        #endregion



    }
}
