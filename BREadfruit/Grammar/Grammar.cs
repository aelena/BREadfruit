using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BREadfruit.Clauses;
using BREadfruit.Conditions;

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
        private static List<Symbol> _entityTypes = new List<Symbol> ();


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
        private static List<Operator> _operators = new List<Operator> ();


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

        public static IEnumerable<Symbol> EntityTypes
        {
            get
            {
                return _entityTypes;
            }
        }

        public static IEnumerable<Operator> Operators
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
        public const string WithLineRegex = "^\tWITH (DEFAULTS|TRIGGERS|CONSTRAINTS|RULES){1}[' ']*$";
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


        /* 
         * when adding a new Symbol here, do not forget
         * to update the WithSymbol definition below 
         */
        public static Symbol DefaultsSymbol = new Symbol ( "defaults", 1, true );
        public static Symbol ConstraintsSymbol = new Symbol ( "constraints", 1, true );
        public static Symbol TriggersSymbol = new Symbol ( "triggers", 1, true );
        public static Symbol RulesSymbol = new Symbol ( "rules", 1, true );

        public static Symbol TextBoxSymbol = new Symbol ( "TextBox", 0, true );
        public static Symbol CheckBoxSymbol = new Symbol ( "CheckBox", 0, true );
        public static Symbol DropDownListSymbol = new Symbol ( "DropDownList", 0, true );
        public static Symbol RadioButtonSymbol = new Symbol ( "RadioButton", 0, true );
        public static Symbol LabelSymbol = new Symbol ( "Label", 0, true );
        public static Symbol ButtonSymbol = new Symbol ( "Button", 0, true );
        public static Symbol DivSymbol = new Symbol ( "Div", 0, true );
        public static Symbol MultilineSymbol = new Symbol ( "Multiline", 0, true );
        public static Symbol ObjectSymbol = new Symbol ( "Object", 0, true );
        public static Symbol DynamicSymbol = new Symbol ( "Dynamic", 0, true );
        public static Symbol ThenSymbol = new Symbol ( "Then", 2, false );

        #endregion


        #region " --- nonterminals --- "


        public static Symbol EntitySymbol = new Symbol ( "Entity", 0, false );
        public static Symbol WithSymbol = new Symbol ( "With", 1,
            new List<Symbol> () { DefaultsSymbol, 
                ConstraintsSymbol, 
                TriggersSymbol,
                RulesSymbol
            } );



        #endregion


        // ---------------------------------------------------------------------------------


        #endregion


        // ---------------------------------------------------------------------------------


        #region " --- operators --- "


        public static Operator IsOperator = new Operator ( "is" );
        public static Operator IsNotOperator = new Operator ( "is_not", new [] { "is not", "isn't" } );
        public static Operator IsEmptyOperator = new Operator ( "is_empty", new [] { "is empty" } );
        public static Operator IsNotEmptyOperator = new Operator ( "is_not_empty", new [] { "is not empty" } );
        public static Operator IsMandatoryOperator = new Operator ( "is_mandatory", new [] { "is mandatory" } );
        public static Operator IsNotMandatoryOperator = new Operator ( "is_not_mandatory", new [] { "is not mandatory" } );
        public static Operator IsVisibleOperator = new Operator ( "is_visible", new [] { "is visible" } );
        public static Operator IsNotVisibleOperator = new Operator ( "is_not_visible", new [] { "is not visible" } );
        public static Operator StartsWithOperator = new Operator ( "starts_with", new [] { "starts with", "starts" } );
        public static Operator NotStartsWithOperator = new Operator ( "does_not_start_with", new [] { "not starts with", "not starts", "does not start with" } );
        public static Operator EndsWithOperator = new Operator ( "ends_with", new [] { "ends", "ends with" } );
        public static Operator NotEndsWithOperator = new Operator ( "does_not_end_with", new [] { "not ends", "not ends with", "does not end with" } );
        public static Operator ContainsOperator = new Operator ( "contains" );
        public static Operator NotContainsOperator = new Operator ( "does_not_contain", new [] { "does not contain", "not contains" } );


        #endregion


        // ---------------------------------------------------------------------------------


        static Grammar ()
        {

            // add valid entity types
            PopulateEntityTypes ();
            PopulateDefaultTokens ();
            AddSymbols ();
            AddOperators ();

            _entityLineRegex = _entityLineRegex.Replace ( "###", String.Format ( "({0})",
                String.Join ( "|", Grammar.EntityTypes.Select ( x => x.Token ) ) ) ).ToUpperInvariant ();


        }


        // ---------------------------------------------------------------------------------


        #region " --- utility methods --- "

        public static DefaultClause GetDefaultClauseByToken ( string token, bool strictComparison = true )
        {
            if ( String.IsNullOrWhiteSpace ( token ) )
                throw new ArgumentNullException ( token );

            IEnumerable<DefaultClause> _s;

            if ( strictComparison )
            {
                _s = from s in Grammar.DefaultTokens
                     where s.Token == token
                     select s;
            }
            else
            {
                _s = from s in Grammar.DefaultTokens
                     where s.Token.Trim ().ToUpperInvariant () == token.Trim ().ToUpperInvariant ()
                     select s;
            }
            if ( _s != null && _s.Count () > 0 )
                return _s.First ();

            return null;

        }


        // ---------------------------------------------------------------------------------


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


        public static Operator GetOperator ( string token )
        {
            if ( String.IsNullOrWhiteSpace ( token ) )
                throw new ArgumentNullException ( token );

            IEnumerable<Operator> _s;
            _s = from s in Operators
                 where s.MatchesToken ( token )
                 select s;
            
            if ( _s != null && _s.Count () > 0 )
                return _s.First ();

            return null;
        }

        #endregion


        // ---------------------------------------------------------------------------------


        #region " --- private methods --- "


        private static void PopulateEntityTypes ()
        {
            Grammar._entityTypes.Add ( TextBoxSymbol );
            Grammar._entityTypes.Add ( CheckBoxSymbol );
            Grammar._entityTypes.Add ( DropDownListSymbol );
            Grammar._entityTypes.Add ( RadioButtonSymbol );
            Grammar._entityTypes.Add ( LabelSymbol );
            Grammar._entityTypes.Add ( ButtonSymbol );
            Grammar._entityTypes.Add ( DivSymbol );
            Grammar._entityTypes.Add ( MultilineSymbol );
            Grammar._entityTypes.Add ( ObjectSymbol );
            Grammar._entityTypes.Add ( DynamicSymbol );
        }


        // ---------------------------------------------------------------------------------


        private static void AddOperators ()
        {
            Grammar._operators.Add ( IsOperator );
            Grammar._operators.Add ( IsNotOperator );
            Grammar._operators.Add ( IsEmptyOperator );
            Grammar._operators.Add ( IsNotEmptyOperator );
            Grammar._operators.Add ( IsMandatoryOperator );
            Grammar._operators.Add ( IsNotMandatoryOperator );
            Grammar._operators.Add ( IsVisibleOperator );
            Grammar._operators.Add ( IsNotVisibleOperator );
            Grammar._operators.Add ( StartsWithOperator );
            Grammar._operators.Add ( NotStartsWithOperator );
            Grammar._operators.Add ( EndsWithOperator );
            Grammar._operators.Add ( NotEndsWithOperator );
            Grammar._operators.Add ( ContainsOperator );
            Grammar._operators.Add ( NotContainsOperator );
        }


        // ---------------------------------------------------------------------------------


        private static void AddSymbols ()
        {
            Grammar._symbols.Add ( EntitySymbol );
            Grammar._symbols.Add ( WithSymbol );
            Grammar._symbols.Add ( DefaultsSymbol );
            Grammar._symbols.Add ( ConstraintsSymbol );
            Grammar._symbols.Add ( TriggersSymbol );
            Grammar._symbols.Add ( RulesSymbol );
            Grammar._symbols.Add ( TextBoxSymbol );
            Grammar._symbols.Add ( CheckBoxSymbol );
            Grammar._symbols.Add ( DropDownListSymbol );
            Grammar._symbols.Add ( RadioButtonSymbol );
            Grammar._symbols.Add ( LabelSymbol );
            Grammar._symbols.Add ( ButtonSymbol );
            Grammar._symbols.Add ( DivSymbol );
            Grammar._symbols.Add ( MultilineSymbol );
            Grammar._symbols.Add ( ObjectSymbol );
            Grammar._symbols.Add ( DynamicSymbol );
            Grammar._symbols.Add ( ThenSymbol );
        }


        // ---------------------------------------------------------------------------------


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


        // ---------------------------------------------------------------------------------


        #endregion


        // ---------------------------------------------------------------------------------

    }
}
