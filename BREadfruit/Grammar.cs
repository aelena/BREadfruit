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


        #region " --- private members --- "

        /// <summary>
        /// Private list of declared symbols.
        /// </summary>
        private static List<Symbol> _symbols = new List<Symbol> ();

        #endregion


        #region " --- private members --- "

        /// <summary>
        /// Returns a collection of the symbols currently known to the grammar
        /// </summary>
        public static IEnumerable<Symbol> Symbols
        {
            get
            {
                return _symbols;
            }
        }

        #endregion


        #region " --- regexs for line validation --- "

        /// <summary>
        /// Regular expression that validates the correct format of an 'entity' line.
        /// </summary>
        public const string EntityLineRegex = "ENTITY [A-Za-z0-9_]* (IS|is|Is|iS)? [A-Za-z]*";
        /// <summary>
        /// Regular expression that validates the correct format of a with statement
        /// within the document format.
        /// </summary>
        public const string WithLineRegex = "^WITH (Defaults|Triggers|Constraints)?";


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

        static Grammar ()
        {
            Grammar._symbols.Add ( EntitySymbol );
            Grammar._symbols.Add ( WithSymbol );
            Grammar._symbols.Add ( DefaultsSymbol );
            Grammar._symbols.Add ( ConstraintsSymbol );
            Grammar._symbols.Add ( TriggersSymbol );
        }

        #region " --- utility methods --- "

        public static Symbol GetSymbolByToken (string token, bool strictComparison = true)
        {
            if ( String.IsNullOrWhiteSpace ( token ) )
                throw new ArgumentNullException ( token );


            IEnumerable<Symbol> _s;

            if ( strictComparison )
            {
                _s = from s in Symbols
                         where s.Token == token
                         select s;
            }
            else
            {
                _s = from s in Symbols
                         where s.Token.Trim().ToUpperInvariant() == token.Trim().ToUpperInvariant()
                         select s;
            }
            if ( _s != null && _s.Count() > 0 )
                return _s.First ();

            return null;
        }


        #endregion




    }
}
