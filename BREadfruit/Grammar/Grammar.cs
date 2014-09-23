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
        private static List<Operator> _unaryOperators = new List<Operator> ();


        private static List<DefaultClause> _defaultsTokens = new List<DefaultClause> ();


        private static List<LogicalOperatorSymbol> _logicalOperators = new List<LogicalOperatorSymbol> ();
        private static List<Symbol> _constraintSymbols = new List<Symbol> ();



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

        public static IEnumerable<Operator> UnaryOperators
        {
            get
            {
                return _unaryOperators;
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


        public static IEnumerable<Symbol> ConstraintSymbols
        {
            get
            {
                return _constraintSymbols;
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
        public const string SetValueRegex = "^((\"|')(.*?)(\"|')|[A-Za-z\".\"\"_\"0-9]*)$";
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
        public const string LabelDefaultValueRegex = "^((\"|')(.*?)(\"|')|[A-Za-z\".\"\"_\"0-9]*)$";

        /// <summary>
        /// Validates an entire load data from line
        /// </summary>
        public const string LoadDataFromLineRegex = @"^load data from (WEBSERVICE|DATASOURCE)?\.[A-Z]*(\.?[A-Z_0-9]+)+$";
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

        private static string _constraintLineRegex = @"^\t{0,2}(###)$";
        public static string ConstraintLineRegex
        {
            get { return _constraintLineRegex; }
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
        //public const string ConstraintValueRegex = "^()$";



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

        public static Symbol EmptyStringSymbol = new Symbol ( "\"\"", 2, true );
        public static Symbol NullValueSymbol = new Symbol ( "null", 2, true );


        public static Symbol WithArgumentsSymbol = new Symbol ( "with_args", 2, false, new [] { "with args", "with arguments" } );

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
        public static UnaryAction LoadDataUnaryActionSymbol = new UnaryAction ( "load_data_from", 2, true, new [] { "load data from" } );

        public static ResultAction SetValueActionSymbol = new ResultAction ( "set_value", 2, true, new [] { "set value" } );
       


        #endregion

        #region " --- event symbols ( for use in triggers block ) --- "

        public static Symbol ChangedEventSymbol = new Symbol ( "changed", 2, true, new List<string> { "on changed", "on change", "changes" } );
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

        public static Symbol LoadDataSymbol = new Symbol ( "load_data_from", 2, new List<string>() {"load data from"},
            new List<Symbol> () { DataSourceSymbol, 
                WebServiceSymbol, 
                FileObjectSymbol
            } );

        #endregion


        // ---------------------------------------------------------------------------------


        #region " --- constraint symbols --- "

        public static Symbol OnlyAsciiConstraintSymbol = new Symbol ( "only_ascii", 2, true, new List<string> { "only ascii", "ascii only" } );
        public static Symbol OnlyNumbersConstraintSymbol = new Symbol ( "only_numbers", 2, true, new List<string> { "only numbers", "numbers only" } );
        public static Symbol OnlyLettersConstraintSymbol = new Symbol ( "only_letters", 2, true, new List<string> { "only letters", "letters only" } );

        #endregion

        // ---------------------------------------------------------------------------------


        #endregion


        // ---------------------------------------------------------------------------------


        #region " --- operators --- "


        public static Operator IsOperator = new Operator ( "is", 2, false );
        public static Operator IsNotOperator = new Operator ( "is_not", 2, false, new [] { "is not", "isn't" } );
        public static Operator IsEmptyOperator = new Operator ( "is_empty", 2, false, new [] { "is empty" } );
        public static Operator IsNotEmptyOperator = new Operator ( "is_not_empty", 2, false, new [] { "is not empty" } );
        public static Operator IsNullOperator = new Operator ( "is_null", 2, false, new [] { "is null" } );
        public static Operator IsNotNullOperator = new Operator ( "is_not_null", 2, false, new [] { "is not null" } );
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
        public static Operator NotInOperator = new Operator ( "not_in", 2, false, new [] { "not in" } );
        public static Operator EqualityOperator = new Operator ( "==", 2, false );
        public static Operator NonEqualityOperator = new Operator ( "!=", 2, false );



        #endregion


        public static DefaultClause MaxlengthDefaultClause = new DefaultClause ( "max_length", PositiveIntegerValueRegex, new List<String> () { "maximum length", "max length" } );
        public static DefaultClause MinLengthDefaultClause = new DefaultClause ( "min_length", PositiveIntegerValueRegex, new List<String> () { "min length", "minimum length" } );
        public static DefaultClause MandatoryDefaultClause = new DefaultClause ( "mandatory", BooleanValueRegex );
        public static DefaultClause EnabledDefaultClause = new DefaultClause ( "enable", BooleanValueRegex );
        public static DefaultClause VisibleDefaultClause = new DefaultClause ( "visible", BooleanValueRegex );
        public static DefaultClause ValueDefaultClause = new DefaultClause ( "value", SetValueRegex, new List<string> () { "set value" } );
        public static DefaultClause LabelDefaultClause = new DefaultClause ( "label", LabelDefaultValueRegex );
        public static DefaultClause LoadDataDefaultClause = new DefaultClause ( "load_data_from", LoadDataFromValueRegex, new List<string> () { "load data from" } );


        // ---------------------------------------------------------------------------------


        static Grammar ()
        {

            // add valid entity types
            PopulateEntityTypes ();
            PopulateDefaultTokens ();
            PopulateConstraintSymbols ();
            AddSymbols ();
            AddOperators ();

            ComposeEntityLineRegex ();
            ComposeConstraintLineRegex ();

        }


        // ---------------------------------------------------------------------------------


        private static void ComposeEntityLineRegex ()
        {
            _entityLineRegex = _entityLineRegex.Replace ( "###", String.Format ( "({0})",
                String.Join ( "|", Grammar.EntityTypes.Select ( x => x.Token ) ) ) ).ToUpperInvariant ();
        }


        // ---------------------------------------------------------------------------------


        private static void ComposeConstraintLineRegex ()
        {
            _constraintLineRegex = _constraintLineRegex.Replace ( "###",
                String.Join ( "|", Grammar.ConstraintSymbols.Select ( x => x.Token ) ) );
        }


        // ---------------------------------------------------------------------------------



        #region " --- utility methods --- "

        // TODO: Probably this is obsolete

        /// <summary>
        /// Performs a search of default tokens
        /// </summary>
        /// <param name="token"></param>
        /// <param name="strictComparison"></param>
        /// <returns></returns>
        public static DefaultClause GetDefaultClauseByToken ( string token, bool strictComparison = true )
        {
            if ( String.IsNullOrWhiteSpace ( token ) )
                throw new ArgumentNullException ( token );

            IEnumerable<DefaultClause> _s;

            if ( strictComparison )
            {
                _s = from s in Grammar.DefaultTokens
                     where s.Token == token
                     || s.Aliases.Contains ( token )
                     select s;
            }
            else
            {
                _s = from s in Grammar.DefaultTokens
                     where s.Token.Trim ().ToUpperInvariant () == token.Trim ().ToUpperInvariant ()
                     || s.Aliases.Contains ( token )
                     select s;
            }
            if ( _s != null && _s.Count () > 0 )
                return _s.First ();

            return null;

        }


        // ---------------------------------------------------------------------------------


        /// <summary>
        /// Searches any symbol declared in the Grammar whose main token
        /// or aliases are the same as the token specified as the first parameter.
        /// </summary>
        /// <param name="token">String with the value for the Symbol.</param>
        /// <param name="strictComparison">Sets a value to indicate if the comparison has to be strict or not
        /// regarding case sensitivity</param>
        /// <returns></returns>
        public static Symbol GetSymbolByToken ( string token, bool strictComparison = true )
        {
            if ( String.IsNullOrWhiteSpace ( token ) )
                throw new ArgumentNullException ( token );

            IEnumerable<Symbol> _s;

            if ( strictComparison )
            {
                _s = from s in Symbols
                     where s.Token == token
                     || s.Aliases.Contains ( token ) // TODO: NOT HAPPY ABOUT THIS SHIT HERE
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


        /// <summary>
        /// Searches any symbol declared in the Grammar whose main token
        /// or aliases are the same as the token specified as the first parameter.
        /// This overload allows the caller a specific Type instance
        /// that will act as filter to the query against the Grammar.
        /// </summary>
        /// <param name="token">String with the value for the Symbol.</param>
        /// <param name="filterType">Type instance that will act as filter to the query against the Grammar</param>
        /// <param name="strictComparison">Sets a value to indicate if the comparison has to be strict or not
        /// regarding case sensitivity</param>
        /// <returns></returns>
        public static Symbol GetSymbolByToken ( string token, Type filterType, bool strictComparison = true )
        {
            return GetSymbolByToken ( token, new List<Type> { filterType }, strictComparison );
        }


        // ---------------------------------------------------------------------------------


        /// <summary>
        /// Searches any symbol declared in the Grammar whose main token
        /// or aliases are the same as the token specified as the first parameter.
        /// This overload allows the caller to pass a collection of Type instances
        /// that will act as filter to the query against the Grammar.
        /// </summary>
        /// <param name="token">String with the value for the Symbol.</param>
        /// <param name="filteredTypes">Collection of Type instances that will act as filter to the query against the Grammar</param>
        /// <param name="strictComparison">Sets a value to indicate if the comparison has to be strict or not
        /// regarding case sensitivity</param>
        /// <returns></returns>
        public static Symbol GetSymbolByToken ( string token, IEnumerable<Type> filteredTypes, bool strictComparison = true )
        {
            if ( String.IsNullOrWhiteSpace ( token ) )
                throw new ArgumentNullException ( token );

            IEnumerable<Symbol> _s;

            if ( strictComparison )
            {
                _s = from s in Symbols
                     where filteredTypes.Contains ( s.GetType() )
                     && s.Token == token
                     || s.Aliases.Contains ( token ) // TODO: NOT HAPPY ABOUT THIS SHIT HERE
                     select s;
            }
            else
            {
                _s = from s in Symbols
                     where filteredTypes.Contains ( s.GetType () )
                     && s.Token.Trim ().ToUpperInvariant () == token.Trim ().ToUpperInvariant ()
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


        private static void PopulateConstraintSymbols ()
        {
            Grammar._constraintSymbols.Add ( OnlyAsciiConstraintSymbol );
            Grammar._constraintSymbols.Add ( OnlyNumbersConstraintSymbol );
            Grammar._constraintSymbols.Add ( OnlyLettersConstraintSymbol );
        }


        // ---------------------------------------------------------------------------------


        private static void AddOperators ()
        {
            //IsOperator.Aliases.ToList ().ForEach ( x => Grammar._operators.Add ( new Operator ( x ) ) );
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
            Grammar._operators.Add ( InOperator );
            Grammar._operators.Add ( NotInOperator );
            Grammar._operators.Add ( IsEmptyOperator );
            Grammar._operators.Add ( IsNotEmptyOperator );
            Grammar._operators.Add ( IsNullOperator );
            Grammar._operators.Add ( IsNotNullOperator );
            Grammar._operators.Add ( EqualityOperator );
            Grammar._operators.Add ( NonEqualityOperator );


            Grammar._unaryOperators.Add ( IsEmptyOperator );
            Grammar._unaryOperators.Add ( IsNotEmptyOperator );
            Grammar._unaryOperators.Add ( IsNullOperator );
            Grammar._unaryOperators.Add ( IsNotNullOperator );
            Grammar._unaryOperators.Add ( EqualityOperator );
            Grammar._unaryOperators.Add ( NonEqualityOperator );

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
            Grammar._symbols.Add ( WithArgumentsSymbol );
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
            Grammar._symbols.Add ( InOperator );
            Grammar._symbols.Add ( NotInOperator );
            Grammar._symbols.Add ( IsEmptyOperator );
            Grammar._symbols.Add ( IsNotEmptyOperator );
            Grammar._symbols.Add ( IsNullOperator );
            Grammar._symbols.Add ( IsNotNullOperator );
            Grammar._symbols.Add ( EqualityOperator );
            Grammar._symbols.Add ( NonEqualityOperator );
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
            Grammar._symbols.Add ( LoadDataUnaryActionSymbol );
            
            // result actions
            Grammar._symbols.Add ( SetValueActionSymbol );

            Grammar._symbols.Add ( ANDSymbol );
            Grammar._symbols.Add ( ORSymbol );

            Grammar._symbols.Add ( ChangedEventSymbol );
            Grammar._symbols.Add ( FocusEventSymbol );
            Grammar._symbols.Add ( BlurredEventSymbol );

            Grammar._symbols.Add ( OnlyAsciiConstraintSymbol );
            Grammar._symbols.Add ( OnlyNumbersConstraintSymbol );
            Grammar._symbols.Add ( OnlyLettersConstraintSymbol );
        }


        // ---------------------------------------------------------------------------------


        /// <summary>
        /// THese are the symbols used in the 'with defaults' section of the entity block 
        /// in the intermediate format
        /// </summary>
        private static void PopulateDefaultTokens ()
        {
          
            _defaultsTokens.Add ( MaxlengthDefaultClause );
            _defaultsTokens.Add ( MinLengthDefaultClause );
            _defaultsTokens.Add ( MandatoryDefaultClause );
            _defaultsTokens.Add ( EnabledDefaultClause );
            _defaultsTokens.Add ( VisibleDefaultClause );
            _defaultsTokens.Add ( ValueDefaultClause );
            _defaultsTokens.Add ( LabelDefaultClause );
            _defaultsTokens.Add ( LoadDataDefaultClause );

            Grammar._symbols.Add ( MaxlengthDefaultClause );
            Grammar._symbols.Add ( MinLengthDefaultClause );
            Grammar._symbols.Add ( MandatoryDefaultClause );
            Grammar._symbols.Add ( EnabledDefaultClause );
            Grammar._symbols.Add ( VisibleDefaultClause );
            Grammar._symbols.Add ( ValueDefaultClause );
            Grammar._symbols.Add ( LabelDefaultClause );
            Grammar._symbols.Add ( LoadDataDefaultClause );
        }


        // ---------------------------------------------------------------------------------


        #endregion


        // ---------------------------------------------------------------------------------

    }
}
