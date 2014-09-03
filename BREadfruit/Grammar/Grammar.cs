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


        private static List<LogicalOperatorSymbol> _logicalOperators = new List<LogicalOperatorSymbol> ();



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

        public static IEnumerable<LogicalOperatorSymbol> LogicalOperators
        {
            get
            {
                return _logicalOperators;
            }
        }


        #endregion


        // ---------------------------------------------------------------------------------


        #region " --- regexs for line validation --- "

        public const string PositiveIntegerValueRegex = "^[0-9]*$";
        public const string IntegerValueRegex = "^[-]?[0-9]*$";
        public const string FloatValueRegex = "^[-]?[0-9]*[(.|,)]?[0-9]*$";
        public const string BooleanValueRegex = "^(true|false){1}$";
        /// <summary>
        /// This regex allows either setting ad hoc strings with single or double quotes, such as
        ///
        /// 'My String' or "My String"
        /// 
        /// as well as making reference to constants that are defined somewhere else
        /// using the all capitals and "." and "_" conventions, so that references such as
        /// 
        /// CONSTANTS.HELLOMESSAGE
        /// ERROR_MESSAGES.NO_SESSION (underscores can be used too)
        /// 
        /// are valid
        /// </summary>
        public const string SetValueRegex = "^((\"|')(.*?)(\"|')|[A-Z\".\"\"_\"]*)$";
        /// <summary>
        /// This regex allows either setting ad hoc strings with single or double quotes for labels, such as
        ///
        /// 'My String' or "My String"
        /// 
        /// as well as making reference to constants that are defined somewhere else
        /// using the all capitals and "." and "_" conventions, so that references such as
        /// 
        /// CONSTANTS.HELLOMESSAGE
        /// ERROR_MESSAGES.NO_SESSION (underscores can be used too)
        /// 
        /// are valid
        /// </summary>
        public const string LabelDefaultValueRegex = "^((\"|')(.*?)(\"|')|[A-Z\".\"\"_\"]*)$";

        /// <summary>
        /// Validates an entire load data from line
        /// </summary>
        public const string LoadDataFromLineRegex = @"^load data from (WEBSERVICE|DATASOURCE)?\.[A-Z]*(\.?[A-Z_]+)+$";
        /// <summary>
        /// Validates the argument part of a load data from line 
        /// (that is, all that comes after the load data from instruction)
        /// </summary>
        public const string LoadDataFromValueRegex = @"^(WEBSERVICE|DATASOURCE)?\.[A-Z]*(\.?[A-Z_]+)+$";

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
        public const string WithLineRegex = "^\tWITH (DEFAULTS|TRIGGERS|CONSTRAINTS|RULES|ACTIONS){1}[' ']*$";
        public const string MaxLengthLineRegex = "^MAX_LENGTH [0-9]*[' ']*$";
        public const string MinLengthLineRegex = "^MIN_LENGTH [0-9]*[' ']*$";
        public const string MandatoryLineRegex = @"^MANDATORY[' ']*((TRUE)?|FALSE)?[' ']*$";
        public const string EnabledLineRegex = @"^ENABLED[' ']*((TRUE)?|FALSE)?[' ']*$";
        public const string VisibleLineRegex = @"^VISIBLE[' ']*((TRUE)?|FALSE)?[' ']*$";
        public const string FreeValueLineRegex = "^value\\s*(\"|')(.*?)(\"|')$"; // ([\"'])(?:(?=(\\?))\2.)*?\1";
        public const string LabelDefaultLineRegex = "^label\\s*((\"|')(.*?)(\"|')|[A-Z\".\"]*)$";


        // ---------------------------------------------------------------------------------

        public static IEnumerable<String> EmptyStringMarkers = new List<string> { "''", "\"\"" };

        #endregion


        // ---------------------------------------------------------------------------------


        #region " --- symbols --- "

        #region " --- terminals --- "


        /* 
         * when adding a new Symbol here, do not forget
         * to update the WithSymbol definition below 
         */

        #region " --- symbols for scopes --- "
        public static Symbol DefaultsSymbol = new Symbol ( "defaults", 1, true );
        public static Symbol ConstraintsSymbol = new Symbol ( "constraints", 1, true );
        public static Symbol TriggersSymbol = new Symbol ( "triggers", 1, true );
        public static Symbol RulesSymbol = new Symbol ( "rules", 1, true );
        public static Symbol ActionsSymbol = new Symbol ( "actions", 1, true );
        #endregion

        #region " --- symbol for entities --- "

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

        #endregion

        public static Symbol ThenSymbol = new Symbol ( "then", 2, false );
        public static Symbol ThisSymbol = new Symbol ( "this", 2, false );
        public static Symbol InSymbol = new Symbol ( "in", 2, false );

        public static Symbol DataSourceSymbol = new Symbol ( "DATASOURCE", 2, false );
        public static Symbol WebServiceSymbol = new Symbol ( "WEBSERVICE", 2, false );
        public static Symbol FileObjectSymbol = new Symbol ( "FILE", 2, false );


        public static LogicalOperatorSymbol ANDSymbol = new LogicalOperatorSymbol ( "and", 2, false );
        public static LogicalOperatorSymbol ORSymbol = new LogicalOperatorSymbol ( "or", 2, false );

        public static Symbol ValueSymbol = new Symbol ( "value", 1, false );

        public static UnaryAction EnableUnaryActionSymbol = new UnaryAction ( "enable", 2, true, new [] { "set enabled", "make enabled", "enabled" } );
        public static UnaryAction DisableUnaryActionSymbol = new UnaryAction ( "disable", 2, true, new [] { "set disabled", "make disabled", "not enabled", "not enable", "disabled" } );
        public static UnaryAction VisibleUnaryActionSymbol = new UnaryAction ( "visible", 2, true, new [] { "set visible", "make visible" } );
        public static UnaryAction HideUnaryActionSymbol = new UnaryAction ( "hide", 2, true, new [] { "set hidden", "make hidden", "hidden" } );
        public static UnaryAction MakeMandatoryUnaryActionSymbol = new UnaryAction ( "mandatory", 2, true, new [] { "make mandatory", "set mandatory" } );
        public static UnaryAction MakeNonMandatoryUnaryActionSymbol = new UnaryAction ( "not mandatory", 2, true, new [] { "make not mandatory", "set not mandatory", "not mandatory" } );
        public static UnaryAction ShowElementUnaryActionSymbol = new UnaryAction ( "show_element", 2, true, new [] { "show element", "show" } );
        //public static UnaryAction HideElementUnaryActionSymbol = new UnaryAction ( "hide_element", 2, true, new [] { "hide element", "hide" } );
        public static UnaryAction ClearValueUnaryActionSymbol = new UnaryAction ( "clear_element", 2, true, new [] { "clear element", "clear" } );

        public static ResultAction SetValueActionSymbol = new ResultAction ( "set_value", 2, true, new [] { "set value" } );



        #endregion

        #region " --- event symbols ( for use in triggers block ) --- "

        public static Symbol ChangedEventSymbol = new Symbol ( "changed", 2, true, new List<string> { "on changed", "on change", "changes", "change" } );
        public static Symbol FocusEventSymbol = new Symbol ( "entered", 2, true, new List<string> { "on entered", "on enter", "enters", "on focus", "focus" } );
        public static Symbol BlurredEventSymbol = new Symbol ( "exited", 2, true, new List<string> { "exit", "exits", "on exit", "on blur", "blur" } );

        #endregion


        #region " --- nonterminals --- "


        public static Symbol EntitySymbol = new Symbol ( "Entity", 0, false );

        /// <summary>
        /// Add valid children to the "with" symbol
        /// </summary>
        public static Symbol WithSymbol = new Symbol ( "with", 1,
            new List<Symbol> () { DefaultsSymbol, 
                ConstraintsSymbol, 
                TriggersSymbol,
                RulesSymbol,
                ActionsSymbol
            } );

        public static Symbol LoadDataSymbol = new Symbol ( "load data from", 2,
            new List<Symbol> () { DataSourceSymbol, 
                WebServiceSymbol, 
                FileObjectSymbol
            } );

        #endregion


        // ---------------------------------------------------------------------------------


        #endregion


        // ---------------------------------------------------------------------------------


        #region " --- operators --- "


        public static Operator IsOperator = new Operator ( "is", 2, false );
        public static Operator IsNotOperator = new Operator ( "is_not", 2, false, new [] { "is not", "isn't" } );
        public static Operator IsEmptyOperator = new Operator ( "is_empty", 2, false, new [] { "is empty" } );
        public static Operator IsNotEmptyOperator = new Operator ( "is_not_empty", 2, false, new [] { "is not empty" } );
        public static Operator IsMandatoryOperator = new Operator ( "is_mandatory", 2, false, new [] { "is mandatory" } );
        public static Operator IsNotMandatoryOperator = new Operator ( "is_not_mandatory", 2, false, new [] { "is not mandatory" } );
        public static Operator IsVisibleOperator = new Operator ( "is_visible", 2, false, new [] { "is visible" } );
        public static Operator IsNotVisibleOperator = new Operator ( "is_not_visible", 2, false, new [] { "is not visible" } );
        public static Operator StartsWithOperator = new Operator ( "starts_with", 2, false, new [] { "starts with", "starts" } );
        // in cases like this one, put longer identifier first
        public static Operator NotStartsWithOperator = new Operator ( "does_not_start_with", 2, false, new [] { "does not start with", "not starts with", "not starts" } );
        public static Operator EndsWithOperator = new Operator ( "ends_with", 2, false, new [] { "ends with", "ends" } );
        // in cases like this one, put longer identifier first
        public static Operator NotEndsWithOperator = new Operator ( "does_not_end_with", 2, false, new [] { "does not end with", "not ends with", "not ends" } );
        public static Operator ContainsOperator = new Operator ( "contains", 2, false );
        public static Operator NotContainsOperator = new Operator ( "does_not_contain", 2, false, new [] { "does not contain", "not contains" } );
        public static Operator InOperator = new Operator ( "in", 2, false );
        public static Operator NotInOperator = new Operator ( "not in", 2, false );



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


        public static bool TokenIsAlias ( string token )
        {
            var _ = from s in Grammar.Symbols
                    where s is Symbol
                    where s.Aliases.Contains ( token )
                    select s;

            return ( _ != null && _.Count () > 0 );

        }

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
            //IsOperator.Aliases.ToList ().ForEach ( x => Grammar._operators.Add ( new Operator ( x ) ) );
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
            Grammar._operators.Add ( InOperator );
            Grammar._operators.Add ( NotInOperator );
        }


        // ---------------------------------------------------------------------------------


        private static void AddSymbols ()
        {
            // add scope symbols
            Grammar._symbols.Add ( EntitySymbol );
            Grammar._symbols.Add ( WithSymbol );
            Grammar._symbols.Add ( DefaultsSymbol );
            Grammar._symbols.Add ( ConstraintsSymbol );
            Grammar._symbols.Add ( TriggersSymbol );
            Grammar._symbols.Add ( RulesSymbol );
            Grammar._symbols.Add ( ActionsSymbol );
            // add entity types symbols
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
            Grammar._symbols.Add ( ThisSymbol );
            Grammar._symbols.Add ( InSymbol );
            // add default symbols
            Grammar._symbols.Add ( ValueSymbol );
            // add operator symbols
            Grammar._symbols.Add ( IsOperator );
            Grammar._symbols.Add ( IsNotOperator );
            Grammar._symbols.Add ( IsEmptyOperator );
            Grammar._symbols.Add ( IsNotEmptyOperator );
            Grammar._symbols.Add ( IsMandatoryOperator );
            Grammar._symbols.Add ( IsNotMandatoryOperator );
            Grammar._symbols.Add ( IsVisibleOperator );
            Grammar._symbols.Add ( IsNotVisibleOperator );
            Grammar._symbols.Add ( StartsWithOperator );
            Grammar._symbols.Add ( NotStartsWithOperator );
            Grammar._symbols.Add ( EndsWithOperator );
            Grammar._symbols.Add ( NotEndsWithOperator );
            Grammar._symbols.Add ( ContainsOperator );
            Grammar._symbols.Add ( NotContainsOperator );
            // add unary actions
            Grammar._symbols.Add ( EnableUnaryActionSymbol );
            Grammar._symbols.Add ( DisableUnaryActionSymbol );
            Grammar._symbols.Add ( VisibleUnaryActionSymbol );
            Grammar._symbols.Add ( HideUnaryActionSymbol );
            Grammar._symbols.Add ( MakeMandatoryUnaryActionSymbol );
            Grammar._symbols.Add ( MakeNonMandatoryUnaryActionSymbol );
            Grammar._symbols.Add ( ShowElementUnaryActionSymbol );
            //Grammar._symbols.Add ( HideElementUnaryActionSymbol );
            Grammar._symbols.Add ( ClearValueUnaryActionSymbol );
            // result actions
            Grammar._symbols.Add ( SetValueActionSymbol );

            Grammar._symbols.Add ( ANDSymbol );
            Grammar._symbols.Add ( ORSymbol );

            Grammar._symbols.Add ( ChangedEventSymbol );
            Grammar._symbols.Add ( FocusEventSymbol );
            Grammar._symbols.Add ( BlurredEventSymbol );
        }


        // ---------------------------------------------------------------------------------


        /// <summary>
        /// THese are the symbols used in the 'with defaults' section of the entity block 
        /// in the intermediate format
        /// </summary>
        private static void PopulateDefaultTokens ()
        {
            var maxlengthdefault = new DefaultClause ( "max_length", PositiveIntegerValueRegex,
                new List<String> () { "maximum length", "max length"} );
            var minlengthdefault = new DefaultClause ( "min_length", PositiveIntegerValueRegex,
                new List<String> () { "minimum length" } );
            var mandatorydefault = new DefaultClause ( "mandatory", BooleanValueRegex );
            var enableddefault = new DefaultClause ( "enabled", BooleanValueRegex );
            var visibledefault = new DefaultClause ( "visible", BooleanValueRegex );
            var valuedefault = new DefaultClause ( "value", SetValueRegex, new List<string> () { "set value" } );
            var labeldefault = new DefaultClause ( "label", LabelDefaultValueRegex );
            var loaddatadefault = new DefaultClause ( "load_data_from", LoadDataFromValueRegex, new List<string> () { "load data from" } );

            _defaultsTokens.Add ( maxlengthdefault );
            _defaultsTokens.Add ( minlengthdefault );
            _defaultsTokens.Add ( mandatorydefault );
            _defaultsTokens.Add ( enableddefault );
            _defaultsTokens.Add ( visibledefault );
            _defaultsTokens.Add ( valuedefault );
            _defaultsTokens.Add ( labeldefault );
            _defaultsTokens.Add ( loaddatadefault );

            Grammar._symbols.Add ( maxlengthdefault );
            Grammar._symbols.Add ( minlengthdefault );
            Grammar._symbols.Add ( mandatorydefault );
            Grammar._symbols.Add ( enableddefault );
            Grammar._symbols.Add ( visibledefault );
            Grammar._symbols.Add ( valuedefault );
            Grammar._symbols.Add ( labeldefault );
            Grammar._symbols.Add ( loaddatadefault );
        }


        // ---------------------------------------------------------------------------------


        #endregion


        // ---------------------------------------------------------------------------------

    }
}
