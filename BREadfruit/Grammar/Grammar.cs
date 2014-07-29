﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BREadfruit.Clauses;

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

        /// <summary>
        /// This string list contains all the valid types for entities,
        /// meaning that in a statement such as
        /// 
        /// Entity XYZ is TYPE
        /// 
        /// this list contains all the valid values for TYPE.
        /// 
        /// THese values will usually be values such as DOM elements or object references, 
        /// for example:
        /// 
        /// TextBox
        /// DropDownLists
        /// Object
        /// Dynamic
        /// MyOwnCustomerClass
        /// 
        /// So, any custom required classes can be explicitly detailed here.
        /// This is list is used later on to create a dynamic RegEx expression to validate lines.
        /// 
        /// CAVEAT: the values for TYPE are Case Insenstive, so TEXTBOX and TextBox are
        /// considered to be the same value.
        /// 
        /// </summary>
        private static List<String> _entityTypes = new List<string> ();


        // ---------------------------------------------------------------------------------


        /// <summary>
        /// This string list contains all the valid operators for conditions,
        /// meaning that in a statement such as
        /// 
        /// text OPERATOR 0 then put 'Hello WOrld' in 'lblErrorMessages'
        /// 
        /// this list contains all the valid values for OPERATOR, usually things like
        /// 
        /// starts_with
        /// ends_with
        /// contains
        /// does_not_contain
        /// is (meaning, equals)
        /// is_not (meaning != )
        ///
        /// and so on. Again, any specific comparison operator, can be added here
        /// if it is not in the default collection of operators.
        /// This is list is used later on to create a dynamic RegEx expression to validate lines.
        /// 
        /// CAVEAT: the values for OPERATOR are Case Insenstive, so Ends_With and ENDS_WITH are
        /// considered to denote the same operator.
        /// 
        /// </summary>
        private static List<String> _operators = new List<string> ();


        private static List<DefaultClause> _defaultsTokens = new List<DefaultClause> ();


        // ---------------------------------------------------------------------------------

        #endregion


        // ---------------------------------------------------------------------------------



        #region " --- public members --- "

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

        public static IEnumerable<string> EntityTypes
        {
            get
            {
                return _entityTypes;
            }
        }

        public static IEnumerable<string> Operators
        {
            get
            {
                return _operators;
            }
        }

        public static IEnumerable<DefaultClause> DefaultTokens
        {
            get
            {
                return _defaultsTokens;
            }
        }

        #endregion


        // ---------------------------------------------------------------------------------


        #region " --- regexs for line validation --- "

        public const string PositiveIntegerRegex = "^[0-9]*$";
        public const string IntegerRegex = "^[-]?[0-9]*$";
        public const string FloatRegex = "^[-]?[0-9]*[(.|,)]?[0-9]*$";
        public const string BooleanRegex = "^(true|false){1}$";


        /// <summary>
        /// Regular expression that validates the correct format of an 'entity' line.
        /// </summary>
        private static string _entityLineRegex = "^ENTITY [A-Za-z0-9_]* (IS|is|Is|iS)? (###){1}$";
        public static string EntityLineRegex
        {
            get { return _entityLineRegex; }
        }


        // ---------------------------------------------------------------------------------


        /// <summary>
        /// Regular expression that validates the correct format of a with statement
        /// within the document format.
        /// </summary>
        public const string WithLineRegex = "^\tWITH (DEFAULTS|TRIGGERS|CONSTRAINTS)?[' ']*$";
        public const string MaxLengthLineRegex = "^MAX_LENGTH [0-9]*[' ']*$";
        public const string MinLengthLineRegex = "^MIN_LENGTH [0-9]*[' ']*$";
        public const string MandatoryLineRegex = @"^MANDATORY[' ']*((TRUE)?|FALSE)?[' ']*$";
        public const string EnabledLineRegex = @"^ENABLED[' ']*((TRUE)?|FALSE)?[' ']*$";
        public const string VisibleLineRegex = @"^VISIBLE[' ']*((TRUE)?|FALSE)?[' ']*$";


        // ---------------------------------------------------------------------------------


        #endregion


        // ---------------------------------------------------------------------------------


        #region " --- symbols --- "

        #region " --- terminals --- "


        public static Symbol DefaultsSymbol = new Symbol ( "defaults", 1, true );
        public static Symbol ConstraintsSymbol = new Symbol ( "constraints", 1, true );
        public static Symbol TriggersSymbol = new Symbol ( "triggers", 1, true );

        #endregion


        #region " --- nonterminals --- "

        public static Symbol EntitySymbol = new Symbol ( "Entity", 0, false );
        public static Symbol WithSymbol = new Symbol ( "With", 1, new List<Symbol> () { DefaultsSymbol, ConstraintsSymbol, TriggersSymbol } );



        #endregion


        // ---------------------------------------------------------------------------------


        #endregion


        // ---------------------------------------------------------------------------------


        static Grammar ()
        {

            // add valid entity types
            PopulateEntityTypes ();
      
            //_operators.Add ( "starts_with" );
            //_operators.Add ( "does_not_start_with" );
            //_operators.Add ( "ends_with" );
            //_operators.Add ( "does_not_end_with" );
            //_operators.Add ( "contains" );
            //_operators.Add ( "does_not_contain" );
            //_operators.Add ( "equals" );
            //_operators.Add ( "does_not_equal" );

            PopulateDefaultTokens ();

            Grammar._symbols.Add ( EntitySymbol );
            Grammar._symbols.Add ( WithSymbol );
            Grammar._symbols.Add ( DefaultsSymbol );
            Grammar._symbols.Add ( ConstraintsSymbol );
            Grammar._symbols.Add ( TriggersSymbol );


            _entityLineRegex = _entityLineRegex.Replace ( "###", String.Format ( "({0})",
                String.Join ( "|", Grammar.EntityTypes ) ) ).ToUpperInvariant ();


        }





        // ---------------------------------------------------------------------------------


        #region " --- utility methods --- "

        public static Symbol GetSymbolByToken ( string token, bool strictComparison = true )
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
                     where s.Token.Trim ().ToUpperInvariant () == token.Trim ().ToUpperInvariant ()
                     select s;
            }
            if ( _s != null && _s.Count () > 0 )
                return _s.First ();

            return null;
        }

        // ---------------------------------------------------------------------------------


        #endregion


        // ---------------------------------------------------------------------------------


        #region " --- private methods --- "


        private static void PopulateEntityTypes ()
        {
            Grammar._entityTypes.Add ( "Textbox" );
            Grammar._entityTypes.Add ( "Checkbox" );
            Grammar._entityTypes.Add ( "DropDownList" );
            Grammar._entityTypes.Add ( "RadioButton" );
            Grammar._entityTypes.Add ( "Label" );
            Grammar._entityTypes.Add ( "Button" );
            Grammar._entityTypes.Add ( "Div" );
            Grammar._entityTypes.Add ( "Multiline" );
            Grammar._entityTypes.Add ( "Object" );
            Grammar._entityTypes.Add ( "Dynamic" );
        }



        private static void PopulateDefaultTokens ()
        {
            var maxlengthdefault = new DefaultClause ( "max_length", PositiveIntegerRegex,
                new List<String> () { "max", "length", "maximum_length" } );
            var minlengthdefault = new DefaultClause ( "min_length", PositiveIntegerRegex,
                new List<String> () { "min", "minimum_length" } );
            var mandatorydefault = new DefaultClause ( "mandatory", BooleanRegex );
            var enableddefault = new DefaultClause ( "enabled", BooleanRegex );
            var visibledefault = new DefaultClause ( "visible", BooleanRegex );
            _defaultsTokens.Add ( maxlengthdefault );
            _defaultsTokens.Add ( minlengthdefault );
            _defaultsTokens.Add ( mandatorydefault );
            _defaultsTokens.Add ( enableddefault );
            _defaultsTokens.Add ( visibledefault );
        }

        #endregion


        // ---------------------------------------------------------------------------------

    }
}
