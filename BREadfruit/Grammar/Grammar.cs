using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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

		private static List<String> _defaultClausesValidationRegexs = new List<string> ();

		private static List<Symbol> _triggerSymbols = new List<Symbol> ();

		private static List<String> _validQuoteSymbols = new List<String> () { "\"", "'" };


		// ---------------------------------------------------------------------------------


		#endregion


		// ---------------------------------------------------------------------------------


		#region " --- public members --- "

		/// <summary>
		/// Returns a collection of the symbols currently declared in the grammar
		/// </summary>
		public static IEnumerable<Symbol> Symbols
		{
			get
			{
				return _symbols;
			}
		}

		/// <summary>
		/// Returns a collection of the entity types currently declared in the grammar
		/// </summary>
		public static IEnumerable<Symbol> EntityTypes
		{
			get
			{
				return _entityTypes;
			}
		}

		/// <summary>
		/// Returns a collection of the operators currently declared in the grammar
		/// </summary>
		public static IEnumerable<Operator> Operators
		{
			get
			{
				return _operators;
			}
		}

		/// <summary>
		/// Returns a collection of the unary operators currently declared in the grammar
		/// </summary>
		public static IEnumerable<Operator> UnaryOperators
		{
			get
			{
				return _unaryOperators;
			}
		}

		/// <summary>
		/// Returns a collection of the default clauses currently declared in the grammar
		/// </summary>
		public static IEnumerable<DefaultClause> DefaultTokens
		{
			get
			{
				return _defaultsTokens;
			}
		}

		/// <summary>
		/// Returns a collection of the logical operators currently declared in the grammar
		/// </summary>
		public static IEnumerable<LogicalOperatorSymbol> LogicalOperators
		{
			get
			{
				return _logicalOperators;
			}
		}

		/// <summary>
		/// Returns a collection of the types of constraints currently declared in the grammar
		/// </summary>
		public static IEnumerable<Symbol> TriggerSymbols
		{
			get
			{
				return _triggerSymbols;
			}
		}


		/// <summary>
		/// Returns a collection of the types of constraints currently declared in the grammar
		/// </summary>
		public static IEnumerable<Symbol> ConstraintSymbols
		{
			get
			{
				return _constraintSymbols;
			}
		}

		public static IEnumerable<String> DefaultClausesValidationRegexs
		{
			get
			{
				return Grammar._defaultClausesValidationRegexs;
			}
		}


		public static IEnumerable<String> ValidQuoteSymbols
		{
			get
			{
				return Grammar._validQuoteSymbols;
			}
		}
		#endregion


		// ---------------------------------------------------------------------------------


		#region " --- regexs for line validation --- "

		public const string PositiveIntegerValueRegex = "^[0-9]*$";
		public const string IntegerValueRegex = "^[-]?[0-9]*$";
		public const string FloatValueRegex = "^[-]?[0-9]*[(.|,)]?[0-9]*$";
		public const string BooleanValueRegex = "^(True|true|false|False|TRUE|FALSE){1}$";
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
		public const string LoadDataFromLineRegex = "^load_data_from[\t\\s]+((JAVASCRIPT|DATASOURCE|WEBSERVICE)\\.[A-Za-z0-9_\\.-]+){1}([\t\\s]+with_args[\t\\s]*{[\t\\s]*(\"|'){1}[A-Za-z0-9-_\\.]*(\"|'){1}[\t\\s]*:[\t\\s]*(((\"|'){1}.*(\"|'){1})|[A-Za-z09_\\.-]+)}[\t\\s]*)?([\t\\s]*in[\t\\s]*[A-Za-z0-9_\\.]+[\t\\s]*)?";
		//public const string LoadDataFromLineRegex = @"^load_data_from[\t\s]+(WEBSERVICE|DATASOURCE)?\.[A-Z]*(\.?[A-Z_0-9]+)+[\t\s]*$";
		/// <summary>
		/// Validates the argument part of a load data from line 
		/// (that is, all that comes after the load data from instruction)
		/// </summary>
		public const string LoadDataFromValueRegex = @"^(JAVASCRIPT|WEBSERVICE|DATASOURCE){1}\.[A-Za-z\d_]+(\.?[A-Za-z\d_]+)*$";
		/// <summary>
		/// Validates the argument part of a load data from line 
		/// (that is, all that comes after the load data from instruction)
		/// </summary>
		public const string ValidationRegexLineRegex = @"^validation_regex[\t\s]+(.)*[\t\s]*";
		public const string ValidationRegexValueRegex = @"^(.)+$";

		public const string AddRowToLineRegex = "^[\t\\s]*ADD_ROW[\t\\s]+(TO[\t\\s]+)?[A-Za-z0-9_-]+[\t\\s]+WITH_ARGS[\t\\s]+{[\t\\s]*(\"|'){1}[A-Za-z0-9-_\\.]*(\"|'){1}[\t\\s]*:[\t\\s]*(((\"|'){1}.*(\"|'){1})|[A-Za-z09_\\.-]+)[\t\\s]*}[\t\\s]*$";

		public const string ClearValueLineRegex = @"^clear_element[\t\s]+([A-Za-z0-9'.'_])+[\t\s]*$";
		public const string ToggleMandatoryLineRegex = @"^([A-Za-z0-9'.'_])+[\t\s]+MANDATORY(([\t\s]+TRUE)?(|[\t\s]*FALSE|[\t\s]*YES|[\t\s]*NO))?[\t\s]*$";
		public const string ToggleEnableLineRegex = @"^([A-Za-z0-9'.'_])+[\t\s]+(ENABLE|ENABLED)(([\t\s]+TRUE)?(|[\t\s]*FALSE|[\t\s]*YES|[\t\s]*NO))?[\t\s]*$";
		public const string ToggleVisibleLineRegex = @"^([A-Za-z0-9'.'_])+[\t\s]+VISIBLE(([\t\s]+TRUE)?(|[\t\s]*FALSE|[\t\s]*YES|[\t\s]*NO))?[\t\s]*$";

		public const string DataOrValueFieldLineRegex = "^(DATAFIELD|VALUEFIELD){1}[\\s]+(\"|')?[A-Za-z_\\-0-9\\.]+(\"|')?[\\s]*$";
		public const string DataOrValueFieldValueRegex = "^((\"|'){1}[A-Za-z_\\-0-9\\.\\s]+(\"|'){1}[\\s]*|[A-Za-z_\\-0-9\\.]+)$";
		public const string HiddenFieldLineRegex = "^(HIDDEN[\\s]*FIELD)([\\s]+(TRUE|true|yes))?[\\s]*";
		public const string HiddenFieldValueRegex = "^(TRUE|true|yes)*";
		/// <summary>
		/// Validates a line that instructs to change the current form shown
		/// This is when there are no arguments to pass to the second form
		///  
		/// </summary>
		public const string ChangeFormNoArgumentsLineRegex = "^CHANGE_TO[\t\\s]+(((\"|')?(([A-Za-z0-9'.'_])+(\"|')?)){1}|((\"|'){1}(([A-Za-z0-9'.'_\\s])+(\"|'){1})){1})[\t\\s]*$";

		/// <summary>
		/// Regular expression to validate a substring expression.
		/// These are valid examples:
		/// 
		/// substring ( 'any value here including &(/&(/&=)?=", 4 )
		/// 
		/// or identifiers with no quotes
		/// 
		/// substring ( IDENTIFIER.MY_NAME, 9)
		/// 
		/// in which case, only numbers, letters, _ and . are allowed 
		/// 
		/// </summary>
		public const string SubStringExpressionRegex = "^substring[\t\\s]*[(][\t\\s]*(([A-Za-z0-9'.'_])|(\"|'){1}(.)+(\"|'))+[\t\\s]*,[\t\\s]*[0-9]+[\t\\s]*[)][\t\\s]*$";
		public const string ToUpperExpressionRegex = "^toupper[\t\\s]*[(][\t\\s]*(([A-Za-z0-9'.'_])|(\"|'){1}(.)+(\"|'))+[\t\\s]*[)][\t\\s]*$";
		public const string ToLowerExpressionRegex = "^tolower[\t\\s]*[(][\t\\s]*(([A-Za-z0-9'.'_])|(\"|'){1}(.)+(\"|'))+[\t\\s]*[)][\t\\s]*$";

		public const string WithArgumentsClauseLineRegex = "^WITH_ARGS[\t\\s]+{([\t\\s]*((\"|')?([A-Za-z0-9'.'_])+(\"|'))[\t\\s]*:[\t\\s]*((\"|')?(.)+(\"|'))[\t\\s]*),?}[\t\\s]*$";
		/// <summary>
		/// Regular expression that validates the correct format of an 'entity' line.
		/// </summary>
		private static string _entityLineRegex = "^ENTITY[\t\\s]+[A-Za-z0-9_]*[\t\\s]+(IS|is|Is|iS)?[\t\\s]+(###){1}[\t\\s]+(IN|in|In|iN)?[\t\\s]+(((\"|')?([A-Za-z0-9'.'_])+(\"|')?)){1}[\t\\s]*$";
		// prev value : "^ENTITY [A-Za-z0-9_]* (IS|is|Is|iS)? (###){1}[\t' ']*$";
		// ^ENTITY[\t\s]+[A-Za-z0-9_]*[\t\s]+(IS|is|Is|iS)?[\t\s]+(###){1}[\t\s]+in[\t\s]+(((\"|')([A-Za-z0-9'.'_])+(\"|'))){1}[\t\s]*$



		public static Regex SaveDataToFullLineRegex = new Regex ( "^(save data to|save_data_to){1}[\t\\s]+(JAVASCRIPT|DATASOURCE|WEBSERVICE)(\\.){1}[A-Za-z0-9_-]+[\t\\s]+with arguments[\t\\s]*{{1}[\t\\s]*((\"|'){1}[A-Za-z0-9_-]+(\"|'){1}){1}[\t\\s]*:[\t\\s]*((\"|'){1}.+(\"|'){1}){1}[\t\\s]*}{1}[\t\\s]*$", RegexOptions.IgnoreCase );
		public static Regex LoadDataFromFullLineRegex = new Regex ( "^(load_data_from|load data from){1}[\t\\s]+(JAVASCRIPT|DATASOURCE|WEBSERVICE)(\\.){1}[A-Za-z0-9_-]+[\t\\s]+with arguments[\t\\s]*{{1}[\t\\s]*((\"|'){1}[A-Za-z0-9_-]+(\"|'){1}){1}[\t\\s]*:[\t\\s]*((\"|'){1}.+(\"|'){1}){1}[\t\\s]*}{1}[\t\\s]*$", RegexOptions.IgnoreCase );
		public static Regex LineWithQuotedStringAndNoOutsideBrackets = new Regex ( "^[^{}]*(\"|'){1}.*(\"|'){1}[^{}]*$", RegexOptions.IgnoreCase );
		public static Regex LineWithOutsideBracketsAndNoOutsideQuotes = new Regex ( "^[^\"']*{{1}.*}{1}[^\"']*$", RegexOptions.IgnoreCase );

		public static Regex EntityFormLineRegex = new Regex ( @"^ENTITY[\s]+[A-Za-z0-9-_\.]+[\s]+IS[\s]+FORM[\s]*$", RegexOptions.IgnoreCase );

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
		public const string WithLineRegex = "^\tWITH (DEFAULTS|TRIGGERS|CONSTRAINTS|RULES|ACTIONS){1}[\t' ']*$";

		public const string MaxLengthLineRegex = @"^MAX_LENGTH[\t\s]+[0-9]+[\t\s]*$";
		public const string MinLengthLineRegex = @"^MIN_LENGTH[\t\s]+[0-9]+[\t\s]*$";
		public const string MandatoryLineRegex = @"^MANDATORY[\t' ']*((TRUE)?|FALSE|YES|NO)?[\t' ']*$";
		public const string MandatoryOrNotLineRegex = @"^([A-Za-z0-9'.'_])+[\t\s]+(MANDATORY|(NOT[\t\s]+|NOT_)MANDATORY){1}[\t\s]*$";
		public const string VisibleOrNotLineRegex = @"^([A-Za-z0-9'.'_])+[\t\s]+(VISIBLE|(NOT[\t\s]+|NOT_)VISIBLE){1}[\t\s]*$";
		public const string EnabledLineRegex = @"^ENABLED[\t' ']*((TRUE)?|FALSE|YES|NO)?[\t' ']*$";
		public const string VisibleLineRegex = @"^VISIBLE[\t' ']*((TRUE)?|FALSE|YES|NO)?[\t' ']*$";
		public const string FreeValueLineRegex = "^VALUE[\t\\s]+(((\"|')(.*?)(\"|'))?|([A-Za-z0-9'.'_]+)?)[\t\\s]*$"; // ([\"'])(?:(?=(\\?))\2.)*?\1";
		public const string LabelDefaultLineRegex = "^LABEL[\t\\s]+(((\"|')(.*?)(\"|'))?|([A-Za-z0-9'.'_]+)?)[\t\\s]*$";
		public const string LoadDataDefaultLineRegex = @"^LOAD_DATA_FROM[\t\s]+(JAVASCRIPT|WEBSERVICE|DATASOURCE)?\.[A-Z]*(\.?[A-Z_0-9]+)+[\t\s]*$";
		public const string AddRowDefaultLineRegex = @"^ADD_ROW[\t\s]+[A-Za-z_0-9]+[\t\s]*$";

		/// <summary>
		/// Regex for lines such as 
		/// show this
		/// show DIV_ADDITIONAL_INFO_NATURAL_PERSON	
		/// See tests in ShouldDiscriminateShowElementsLineCorrectly
		/// </summary>
		public const string ShowElementLineRegex = @"^show[\t\s]+(this|([A-Za-z0-9_])+){1}[\t\s]*$";
		/// <summary>
		/// Regex for lines such as 
		/// hide this
		/// hide DIV_ADDITIONAL_INFO_NATURAL_PERSON	
		/// See tests in ShouldDiscriminateHideElementsLineCorrectly
		/// </summary>
		public const string HideElementLineRegex = @"^hide[\t\s]+(this|([A-Za-z0-9_\.])+){1}[\t\s]*$";
		public const string CloseElementLineRegex = @"^close[\t\s]+(this|([A-Za-z0-9_\.])+){1}[\t\s]*$";
		



		// ---------------------------------------------------------------------------------

		public static IEnumerable<String> EmptyStringMarkers = new List<string> { "''", "\"\"" };

		#endregion


		// ---------------------------------------------------------------------------------


		#region " --- symbols --- "

		#region " --- terminals --- "

		public static Symbol OpeningCurlyBracket = new Symbol ( "{", 2, true );
		public static Symbol ClosingCurlyBracket = new Symbol ( "}", 2, true );
		public static Symbol OpeningSquareBracket = new Symbol ( "[", 2, true );
		public static Symbol ClosingSquareBracket = new Symbol ( "]", 2, true );



		public static readonly Symbol TrueSymbol = new Symbol ( "true", 2, true );
		public static readonly Symbol FalseSymbol = new Symbol ( "false", 2, true );

		public static readonly Symbol ReturnSymbol = new Symbol ( "return", 3, true, new [] { "exit", "break" } );

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
		public static Symbol NumericBoxSymbol = new Symbol ( "NumericBox", 0, true );
		public static Symbol CheckBoxSymbol = new Symbol ( "CheckBox", 0, true );
		public static Symbol DropDownListSymbol = new Symbol ( "DropDownList", 0, true );
		public static Symbol RadioButtonSymbol = new Symbol ( "RadioButton", 0, true );
		public static Symbol LabelSymbol = new Symbol ( "Label", 0, true );
		public static Symbol ButtonSymbol = new Symbol ( "Button", 0, true );
		public static Symbol DivSymbol = new Symbol ( "Div", 0, true );
		public static Symbol MultilineSymbol = new Symbol ( "Multiline", 0, true );
		public static Symbol GridSymbol = new Symbol ( "Grid", 0, true );
		public static Symbol HyperLinkSymbol = new Symbol ( "HyperLink", 0, true );
		public static Symbol ListBoxSymbol = new Symbol ( "ListBox", 0, true );
		public static Symbol CalendarSymbol = new Symbol ( "Calendar", 0, true );
		public static Symbol AttachmentManagerSymbol = new Symbol ( "AttachmentManager", 0, true );
		public static Symbol FormSymbol = new Symbol ( "Form", 0, true );
		public static Symbol ObjectSymbol = new Symbol ( "Object", 0, true );
		//public static Symbol DynamicSymbol = new Symbol ( "Dynamic", 0, true );

		#endregion

		public static Symbol EmptySymbol = new Symbol ( "", 2, true );

		public static Symbol ThenSymbol = new Symbol ( "then", 2, false );
		public static Symbol ElseSymbol = new Symbol ( "else", 2, false );
		public static Symbol ThisSymbol = new Symbol ( "this", 2, false );
		public static Symbol InSymbol = new Symbol ( "in", 2, false, new [] { "is in", "is one of", "is any of" } );

		public static Symbol EmptyStringSymbol = new Symbol ( "\"\"", 2, true );
		public static Symbol NullValueSymbol = new Symbol ( "null", 2, true );

		public static Symbol VisibleSymbol = new Symbol ( "Visible", 2, true );
		public static Symbol EnabledSymbol = new Symbol ( "Enabled", 2, true );

		public static Symbol WithArgumentsSymbol = new Symbol ( "with_args", 2, false, new [] { "with args", "with arguments" } );
		public static Symbol WithOutputArgumentsSymbol = new Symbol ( "with_output", 2, false, new [] { "with output", "with output arguments" } );
		public static Symbol WithQuerySymbol = new Symbol ( "with_query", 2, false, new [] { "with query" } );


		public static Symbol DataSourceSymbol = new Symbol ( "DATASOURCE", 2, false );
		public static Symbol WebServiceSymbol = new Symbol ( "WEBSERVICE", 2, false );
		public static Symbol JavascriptSymbol = new Symbol ( "JAVASCRIPT", 2, false );
		public static Symbol FileObjectSymbol = new Symbol ( "FILE", 2, false );


		public static LogicalOperatorSymbol ANDSymbol = new LogicalOperatorSymbol ( "and", 2, false );
		public static LogicalOperatorSymbol ORSymbol = new LogicalOperatorSymbol ( "or", 2, false );

		public static Symbol ValueSymbol = new Symbol ( "value", 1, false );

		public static UnaryAction EnableUnaryActionSymbol = new UnaryAction ( "enable", 2, true, new [] { "set enabled", "make enabled", "enabled" } );
		public static UnaryAction DisableUnaryActionSymbol = new UnaryAction ( "disable", 2, true, new [] { "set disabled", "make disabled", "not enabled", "not enable", "disabled" } );
		public static UnaryAction VisibleUnaryActionSymbol = new UnaryAction ( "visible", 2, true, new [] { "set visible", "make visible" } );
		public static UnaryAction NotVisibleUnaryActionSymbol = new UnaryAction ( "not_visible", 2, true, new [] { "set not visible", "make not visible", "not visible" } );
		public static UnaryAction HideUnaryActionSymbol = new UnaryAction ( "hide", 2, true, new [] { "set hidden", "make hidden", "hidden" } );
		public static UnaryAction CloseUnaryActionSymbol = new UnaryAction ( "close", 2, true, null );
		//public static UnaryAction MakeMandatoryUnaryActionSymbol = new UnaryAction ( "mandatory", 2, true, new [] { "make mandatory", "set mandatory", "mandatory true" } );
		public static UnaryAction MakeMandatoryUnaryActionSymbol = new UnaryAction ( "mandatory", 2, true, new [] { "make mandatory", "set mandatory" } );
		public static UnaryAction MakeNonMandatoryUnaryActionSymbol = new UnaryAction ( "not_mandatory", 2, true, new [] { "make not mandatory", "set not mandatory", "not mandatory" } );
		public static UnaryAction ShowElementUnaryActionSymbol = new UnaryAction ( "show", 2, true, new [] { "show element" } );
		public static UnaryAction ClearValueUnaryActionSymbol = new UnaryAction ( "clear_element", 2, true, new [] { "clear element", "clear" } );
		public static UnaryAction LoadDataUnaryActionSymbol = new UnaryAction ( "load_data_from", 2, true, new [] { "load data from" } );
		public static UnaryAction SaveDataUnaryActionSymbol = new UnaryAction ( "save_data_to", 2, true, new [] { "save data to" } );
		public static UnaryAction ChangeFormUnaryActionSymbol = new UnaryAction ( "change_to", 2, true, new [] { "change to" } );
		public static UnaryAction SetValidationRegexUnaryActionSymbol = new UnaryAction ( "validation_regex", 2, true, new [] { "validation regex", "validation" } );
		public static UnaryAction SetMaxLengthActionSymbol = new UnaryAction ( "max_length", 2, true, new [] { "max length", "set length", "length" } );

		public static UnaryAction AddRowUnaryActionSymbol = new UnaryAction ( "add_row", 2, true, new [] { "add row to", "add row" } );


		public static ResultAction SetValueActionSymbol = new ResultAction ( "set_value", 2, true, new [] { "set value" } );
		public static ResultAction AddValueActionSymbol = new ResultAction ( "add_value", 2, true, new [] { "add value" } );

		public static ResultAction SetIndexActionSymbol = new ResultAction ( "set_index", 2, true, new [] { "set index", "select index" } );
		public static ResultAction RemoveValueActionSymbol = new ResultAction ( "remove_value", 2, true, new [] { "remove", "remove value" } );

		public static ResultAction SetLabelActionSymbol = new ResultAction ( "set_label", 2, true, new [] { "set label" } );


		#endregion

		#region " --- event symbols ( for use in triggers block ) --- "

		public static Symbol ChangedEventSymbol = new Symbol ( "changed", 2, true, new List<string> { "on changed", "on change", "changes" } );
		public static Symbol FocusEventSymbol = new Symbol ( "entered", 2, true, new List<string> { "on entered", "on enter", "enters", "on focus", "focus" } );
		public static Symbol BlurredEventSymbol = new Symbol ( "exited", 2, true, new List<string> { "exit", "exits", "on exit", "on blur", "blur" } );
		public static Symbol ClickedEventSymbol = new Symbol ( "clicked", 2, true, new List<string> { "on click", "onclick", "on_click", "on clicked", "on_clicked", "onclicked", "clicks", "click" } );
		public static Symbol LoadedEventSymbol = new Symbol ( "loaded", 2, true, new List<string> { "on loads", "on load", "loads" } );

		public static Symbol RowInsertedEventSymbol = new Symbol ( "row_inserted", 2, true, new List<string> { "on row inserted", "row inserted" } );
		public static Symbol RowDeletedEventSymbol = new Symbol ( "row_deleted", 2, true, new List<string> { "on row deleted", "row deleted" } );
		public static Symbol RowUpdatedEventSymbol = new Symbol ( "row_updated", 2, true, new List<string> { "on row updated", "row updated" } );


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

		public static Symbol LoadDataSymbol = new Symbol ( "load_data_from", 2, new List<string> () { "load data from" },
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


		#region " --- operators --- Sub


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
		public static Operator InOperator = new Operator ( "in", 2, false, new [] { "is in", "is one of", "is any of" } );
		public static Operator NotInOperator = new Operator ( "not_in", 2, false, new [] { "not in", "not one of", "is not in", "is not any of", "is not one of" } );
		public static Operator EqualityOperator = new Operator ( "==", 2, false );
		public static Operator NonEqualityOperator = new Operator ( "!=", 2, false );
		public static Operator InRangeOperator = new Operator ( "in_range", 2, false, new [] { "in range" } );
		public static Operator NotInRangeOperator = new Operator ( "not_in_range", 2, false, new [] { "not in range" } );


		public static Operator IsNumberOperator = new Operator ( "is_number", 2, false, new [] { "is number", "is digit", "is integer" } );
		public static Operator IsNotNumberOperator = new Operator ( "is_not_number", 2, false, new [] { "is not number", "is not a number", "is not a digit", "is not integer", "is not an integer" } );
		public static Operator IsDecimalOperator = new Operator ( "is_decimal", 2, false, new [] { "is decimal" } );
		public static Operator IsNotDecimalOperator = new Operator ( "is_not_decimal", 2, false, new [] { "is not decimal" } );
		public static Operator IsAplhaOperator = new Operator ( "is_alpha", 2, false, new [] { "is alphanumeric", "is alpha" } );

		public static Operator SubStringOperator = new Operator ( "substring", 2, false );

		public static Operator GreaterThanOperator = new Operator ( ">", 2, false, new [] { "greater than", "bigger than" } );
		public static Operator LowerThanOperator = new Operator ( "<", 2, false, new [] { "less than", "smaller than", "lower than" } );
		public static Operator GreaterEqualThanOperator = new Operator ( ">=", 2, false, new [] { "greater or equal than", "bigger or equal than" } );
		public static Operator LowerEqualThanOperator = new Operator ( "<=", 2, false, new [] { "less or equal than", "smaller or equal than", "lower or equal than" } );

		#endregion


		public static DefaultClause MaxlengthDefaultClause = new DefaultClause ( "max_length", PositiveIntegerValueRegex, new List<String> () { "maximum length", "max length" } );
		public static DefaultClause MinLengthDefaultClause = new DefaultClause ( "min_length", PositiveIntegerValueRegex, new List<String> () { "min length", "minimum length" } );
		public static DefaultClause MandatoryDefaultClause = new DefaultClause ( "mandatory", BooleanValueRegex );
		public static DefaultClause EnabledDefaultClause = new DefaultClause ( "enable", BooleanValueRegex );
		public static DefaultClause VisibleDefaultClause = new DefaultClause ( "visible", BooleanValueRegex );
		public static DefaultClause ValueDefaultClause = new DefaultClause ( "value", SetValueRegex, new List<string> () { "set value" } );
		public static DefaultClause LabelDefaultClause = new DefaultClause ( "label", LabelDefaultValueRegex );
		public static DefaultClause LoadDataDefaultClause = new DefaultClause ( "load_data_from", LoadDataFromValueRegex, new List<string> () { "load data from" } );
		public static DefaultClause ValidationRegexDefaultClause = new DefaultClause ( "validation_regex", ValidationRegexValueRegex, new [] { "validation regex", "set validation", "validation" } );
		public static DefaultClause ToolTipDefaultClause = new DefaultClause ( "tooltip", ValidationRegexValueRegex, new [] { "set tooltip" } );
		public static DefaultClause DefineColumnDefaultClause = new DefaultClause ( "define_column", ".", new List<string> () { "define column" } );

		public static DefaultClause DataFieldDefaultClause = new DefaultClause ( "DATAFIELD", Grammar.DataOrValueFieldValueRegex, new List<string> () { "data field" } );
		public static DefaultClause ValueFieldDefaultClause = new DefaultClause ( "VALUEFIELD", Grammar.DataOrValueFieldValueRegex, new List<string> () { "value field" } );
		public static DefaultClause HiddenFieldDefaultClause = new DefaultClause ( "HIDDENFIELD", Grammar.HiddenFieldValueRegex, new List<string> () { "hidden field" } );

		

		// ---------------------------------------------------------------------------------


		static Grammar ()
		{

			// add valid entity types
			PopulateEntityTypes ();
			PopulateDefaultTokens ();
			PopulateConstraintSymbols ();
			AddSymbols ();
			AddOperators ();
			AddTriggerSymbols ();

			AddDefaultClauseLineValidationRegexs ();

			ComposeEntityLineRegex ();
			ComposeConstraintLineRegex ();

		}


		// ---------------------------------------------------------------------------------


		private static void AddDefaultClauseLineValidationRegexs ()
		{
			Grammar._defaultClausesValidationRegexs.Add ( MaxLengthLineRegex );
			Grammar._defaultClausesValidationRegexs.Add ( MinLengthLineRegex );
			Grammar._defaultClausesValidationRegexs.Add ( MandatoryLineRegex );
			Grammar._defaultClausesValidationRegexs.Add ( EnabledLineRegex );
			Grammar._defaultClausesValidationRegexs.Add ( VisibleLineRegex );
			Grammar._defaultClausesValidationRegexs.Add ( FreeValueLineRegex );
			Grammar._defaultClausesValidationRegexs.Add ( LabelDefaultLineRegex );
			Grammar._defaultClausesValidationRegexs.Add ( LoadDataDefaultLineRegex );
			Grammar._defaultClausesValidationRegexs.Add ( AddRowToLineRegex );
		}


		// ---------------------------------------------------------------------------------


		private static void ComposeEntityLineRegex ()
		{
			_entityLineRegex = _entityLineRegex.Replace ( "###", String.Format ( "({0})",
				String.Join ( "|", Grammar.EntityTypes.Select ( x => x.Token.ToUpperInvariant () ) ) ) );
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
			{
				return _s.First ().Clone ();
			}
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
					 where filteredTypes.Contains ( s.GetType () )
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


		/// <summary>
		/// Receives a given token (string) and returns a boolean value
		/// that indicates if that token is an alias (not a main token)
		/// of any of the symbols in the grammar.
		/// </summary>
		/// <param name="token">Token to be searched in all Symbols
		/// in the grammar</param>
		/// <returns>boolean value
		/// that indicates if that token is an alias (not a main token)
		/// of any of the symbols in the grammar.</returns>
		public static bool TokenIsAlias ( string token )
		{
			if ( String.IsNullOrWhiteSpace ( token ) )
				return false;

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
			Grammar._entityTypes.Add ( NumericBoxSymbol );
			Grammar._entityTypes.Add ( CheckBoxSymbol );
			Grammar._entityTypes.Add ( DropDownListSymbol );
			Grammar._entityTypes.Add ( RadioButtonSymbol );
			Grammar._entityTypes.Add ( LabelSymbol );
			Grammar._entityTypes.Add ( ButtonSymbol );
			Grammar._entityTypes.Add ( GridSymbol );
			Grammar._entityTypes.Add ( DivSymbol );
			Grammar._entityTypes.Add ( MultilineSymbol );
			Grammar._entityTypes.Add ( HyperLinkSymbol );
			Grammar._entityTypes.Add ( ListBoxSymbol );
			Grammar._entityTypes.Add ( CalendarSymbol );
			Grammar._entityTypes.Add ( AttachmentManagerSymbol );
			Grammar._entityTypes.Add ( FormSymbol );
			Grammar._entityTypes.Add ( ObjectSymbol );
		
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
			Grammar._operators.Add ( IsNotEmptyOperator );
			Grammar._operators.Add ( IsNullOperator );
			Grammar._operators.Add ( IsNotNullOperator );
			Grammar._operators.Add ( EqualityOperator );
			Grammar._operators.Add ( NonEqualityOperator );
			Grammar._operators.Add ( SubStringOperator );

			Grammar._operators.Add ( IsOperator );
			Grammar._operators.Add ( IsNotOperator );

			Grammar._operators.Add ( InRangeOperator );
			Grammar._operators.Add ( NotInRangeOperator );

			Grammar._operators.Add ( InRangeOperator );
			Grammar._operators.Add ( NotInRangeOperator );
			Grammar._operators.Add ( IsNumberOperator );
			Grammar._operators.Add ( IsNotNumberOperator );
			Grammar._operators.Add ( IsDecimalOperator );
			Grammar._operators.Add ( IsNotDecimalOperator );
			Grammar._operators.Add ( IsAplhaOperator );

			Grammar._operators.Add ( InRangeOperator );
			Grammar._operators.Add ( NotInRangeOperator );

			Grammar._operators.Add ( GreaterThanOperator );
			Grammar._operators.Add ( LowerThanOperator );
			Grammar._operators.Add ( GreaterEqualThanOperator );
			Grammar._operators.Add ( LowerEqualThanOperator );

			Grammar._unaryOperators.Add ( IsEmptyOperator );
			Grammar._unaryOperators.Add ( IsNotEmptyOperator );
			Grammar._unaryOperators.Add ( IsNullOperator );
			Grammar._unaryOperators.Add ( IsNotNullOperator );
			Grammar._unaryOperators.Add ( EqualityOperator );
			Grammar._unaryOperators.Add ( NonEqualityOperator );
			Grammar._unaryOperators.Add ( IsVisibleOperator );
			Grammar._unaryOperators.Add ( IsNotVisibleOperator );

			Grammar._unaryOperators.Add ( IsNumberOperator );
			Grammar._unaryOperators.Add ( IsNotNumberOperator );
			Grammar._unaryOperators.Add ( IsDecimalOperator );
			Grammar._unaryOperators.Add ( IsNotDecimalOperator );
			Grammar._unaryOperators.Add ( IsAplhaOperator );

		}


		// ---------------------------------------------------------------------------------


		private static void AddTriggerSymbols ()
		{
			Grammar._triggerSymbols.Add ( ChangedEventSymbol );
			Grammar._triggerSymbols.Add ( FocusEventSymbol );
			Grammar._triggerSymbols.Add ( BlurredEventSymbol );
			Grammar._triggerSymbols.Add ( ClickedEventSymbol );
			Grammar._triggerSymbols.Add ( LoadedEventSymbol );
			Grammar._triggerSymbols.Add ( RowInsertedEventSymbol );
			Grammar._triggerSymbols.Add ( RowDeletedEventSymbol );
			Grammar._triggerSymbols.Add ( RowUpdatedEventSymbol );
		}


		// ---------------------------------------------------------------------------------


		private static void AddSymbols ()
		{

			Grammar._symbols.Add ( TrueSymbol );
			Grammar._symbols.Add ( FalseSymbol );
			Grammar._symbols.Add ( ReturnSymbol );

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
			Grammar._symbols.Add ( NumericBoxSymbol );
			Grammar._symbols.Add ( CheckBoxSymbol );
			Grammar._symbols.Add ( DropDownListSymbol );
			Grammar._symbols.Add ( RadioButtonSymbol );
			Grammar._symbols.Add ( LabelSymbol );
			Grammar._symbols.Add ( ButtonSymbol );
			Grammar._symbols.Add ( DivSymbol );
			Grammar._symbols.Add ( MultilineSymbol );
			Grammar._symbols.Add ( HyperLinkSymbol );
			Grammar._symbols.Add ( ListBoxSymbol );
			Grammar._symbols.Add ( CalendarSymbol );
			Grammar._symbols.Add ( AttachmentManagerSymbol );
			Grammar._symbols.Add ( FormSymbol );
			Grammar._symbols.Add ( ObjectSymbol );
			
			Grammar._symbols.Add ( ElseSymbol );
			Grammar._symbols.Add ( ThenSymbol );
			Grammar._symbols.Add ( ThisSymbol );
			Grammar._symbols.Add ( InSymbol );
			Grammar._symbols.Add ( WithArgumentsSymbol );
			Grammar._symbols.Add ( WithOutputArgumentsSymbol );
			Grammar._symbols.Add ( WithQuerySymbol );


			// add default symbols
			Grammar._symbols.Add ( ValueSymbol );
			// add operator symbols
			//Grammar._symbols.Add ( IsOperator );
			//Grammar._symbols.Add ( IsNotOperator );
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
			Grammar._symbols.Add ( IsNotEmptyOperator );
			Grammar._symbols.Add ( IsNullOperator );
			Grammar._symbols.Add ( IsNotNullOperator );
			Grammar._symbols.Add ( EqualityOperator );
			Grammar._symbols.Add ( NonEqualityOperator );

			Grammar._symbols.Add ( IsOperator );
			Grammar._symbols.Add ( IsNotOperator );

			Grammar._symbols.Add ( IsAplhaOperator );
			Grammar._symbols.Add ( IsDecimalOperator );
			Grammar._symbols.Add ( IsNotDecimalOperator );
			Grammar._symbols.Add ( IsNumberOperator );
			Grammar._symbols.Add ( IsNotNumberOperator );

			Grammar._symbols.Add ( InRangeOperator );
			Grammar._symbols.Add ( NotInRangeOperator );

			Grammar._symbols.Add ( GreaterThanOperator );
			Grammar._symbols.Add ( LowerThanOperator );
			Grammar._symbols.Add ( GreaterEqualThanOperator );
			Grammar._symbols.Add ( LowerEqualThanOperator );

			// add unary actions
			Grammar._symbols.Add ( EnableUnaryActionSymbol );
			Grammar._symbols.Add ( DisableUnaryActionSymbol );
			Grammar._symbols.Add ( VisibleUnaryActionSymbol );
			Grammar._symbols.Add ( HideUnaryActionSymbol );
			Grammar._symbols.Add ( CloseUnaryActionSymbol );
			Grammar._symbols.Add ( MakeMandatoryUnaryActionSymbol );
			Grammar._symbols.Add ( MakeNonMandatoryUnaryActionSymbol );
			Grammar._symbols.Add ( ShowElementUnaryActionSymbol );
			Grammar._symbols.Add ( NotVisibleUnaryActionSymbol );
			//Grammar._symbols.Add ( HideElementUnaryActionSymbol );
			Grammar._symbols.Add ( ClearValueUnaryActionSymbol );
			Grammar._symbols.Add ( LoadDataUnaryActionSymbol );
			Grammar._symbols.Add ( SaveDataUnaryActionSymbol );
			Grammar._symbols.Add ( ChangeFormUnaryActionSymbol );
			Grammar._symbols.Add ( SetValidationRegexUnaryActionSymbol );
			Grammar._symbols.Add ( AddRowUnaryActionSymbol );

			// result actions
			Grammar._symbols.Add ( SetValueActionSymbol );
			Grammar._symbols.Add ( AddValueActionSymbol );

			Grammar._symbols.Add ( SetIndexActionSymbol );
			Grammar._symbols.Add ( RemoveValueActionSymbol );
			Grammar._symbols.Add ( SetLabelActionSymbol );

			Grammar._symbols.Add ( ANDSymbol );
			Grammar._symbols.Add ( ORSymbol );

			Grammar._symbols.Add ( ChangedEventSymbol );
			Grammar._symbols.Add ( FocusEventSymbol );
			Grammar._symbols.Add ( BlurredEventSymbol );
			Grammar._symbols.Add ( ClickedEventSymbol );
			Grammar._symbols.Add ( LoadedEventSymbol );

			Grammar._symbols.Add ( RowInsertedEventSymbol );
			Grammar._symbols.Add ( RowDeletedEventSymbol );
			Grammar._symbols.Add ( RowUpdatedEventSymbol );

			Grammar._symbols.Add ( OnlyAsciiConstraintSymbol );
			Grammar._symbols.Add ( OnlyNumbersConstraintSymbol );
			Grammar._symbols.Add ( OnlyLettersConstraintSymbol );

			Grammar._symbols.Add ( OpeningCurlyBracket );
			Grammar._symbols.Add ( ClosingCurlyBracket );
			Grammar._symbols.Add ( OpeningSquareBracket );
			Grammar._symbols.Add ( ClosingSquareBracket );


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
			_defaultsTokens.Add ( ValidationRegexDefaultClause );
			_defaultsTokens.Add ( ToolTipDefaultClause );
			_defaultsTokens.Add ( DefineColumnDefaultClause );
			_defaultsTokens.Add ( DataFieldDefaultClause );
			_defaultsTokens.Add ( ValueFieldDefaultClause );
			_defaultsTokens.Add ( HiddenFieldDefaultClause );

			Grammar._symbols.Add ( MaxlengthDefaultClause );
			Grammar._symbols.Add ( MinLengthDefaultClause );
			Grammar._symbols.Add ( MandatoryDefaultClause );
			Grammar._symbols.Add ( EnabledDefaultClause );
			Grammar._symbols.Add ( VisibleDefaultClause );
			Grammar._symbols.Add ( ValueDefaultClause );
			Grammar._symbols.Add ( LabelDefaultClause );
			Grammar._symbols.Add ( LoadDataDefaultClause );
			Grammar._symbols.Add ( ValidationRegexDefaultClause );
			Grammar._symbols.Add ( ToolTipDefaultClause );
			Grammar._symbols.Add ( DefineColumnDefaultClause );
			Grammar._symbols.Add ( DataFieldDefaultClause );
			Grammar._symbols.Add ( ValueFieldDefaultClause );
			Grammar._symbols.Add ( HiddenFieldDefaultClause );

		}


		// ---------------------------------------------------------------------------------


		#endregion


		// ---------------------------------------------------------------------------------


		/// <summary>
		/// Template for the custom exception thrown when an invalid entity is found 
		/// parsing the file.
		/// The template is "Invalid Entity declaration found in line {0} - '{1}'",
		/// where {0} should be the line number, and {1} the line itself for easy identification
		/// </summary>
		public static readonly string InvalidEntityDeclarationExceptionMessageTemplate = "Invalid Entity declaration found in line {0} - '{1}'";
		/// <summary>
		/// Default unadorned message for the custom exception thrown when an invalid entity is found 
		/// parsing the file.
		/// The template is "Invalid Entity declaration found.".
		/// </summary>
		public static readonly string InvalidEntityDeclarationExceptionDefaultMessage = "Invalid Entity declaration found.";
		/// <summary>
		/// Template for the custom exception thrown when an invalid with clause is found 
		/// parsing the file.
		/// The template is "Invalid Entity declaration found in line {0} - '{1}'",
		/// where {0} should be the line number, and {1} the line itself for easy identification
		/// </summary>
		public static readonly string InvalidWithClauseExceptionMessageTemplate = "Invalid With clause found in line {0} - '{1}'";
		/// <summary>
		/// Default unadorned message for the custom exception thrown when an invalid with clause is found 
		/// parsing the file.
		/// The template is "Invalid With clause found.".
		/// </summary>
		public static readonly string InvalidWithClauseExceptionDefaultMessage = "Invalid With clause found.";
		public static readonly string InvalidShowElementExceptionDefaultMessage = "Invalid Show Element clause found";
		public static readonly string InvalidShowElementExceptionMessageTemplate = "Invalid Show Element clause found in line {0} - '{1}'";
		public static readonly string InvalidHideElementExceptionDefaultMessage = "Invalid Hide Element clause found";
		public static readonly string InvalidHideElementExceptionMessageTemplate = "Invalid Hide Element clause found in line {0} - '{1}'";
		public static readonly string MissingInClauseExceptionDefaultMessage = "Line seems to be missing token 'in'";
		public static readonly string MissingInClauseExceptionMessageTemplate = "Line {0} seems to be missing token 'in' - '{1}'";
		public static readonly string MissingThenClauseExceptionDefaultMessage = "Found Invalid Condition Syntax - symbol 'then' missing in line";
		public static readonly string MissingThenClauseExceptionMessageTemplate = "Found Invalid Condition Syntax - symbol 'then' missing in line {0} - '{1}'";
		public static readonly string InvalidLineFoundExceptionDefaultMessage = "A line that seems invalid or was not parsed was found.";
		public static readonly string InvalidLineFoundExceptionDefaultTemplate = "Line {0} ('{1}') seems invalid or was not parsed correctly.";
		public static readonly string TokenNotFoundExceptionDefaultMessage = "Token not found";
		public static readonly string TokenNotFoundExceptionDefaultTemplate = "Token '{0}' was not found in line {1}";
		public static readonly string DuplicateEntityFoundExceptionDefaultMessage = "Duplicate entity name found";
		public static readonly string DuplicateEntityFoundExceptionDefaultTemplate = "A duplicate declaration was found for Entity '{0}' in line {1}";
		public static readonly string UnexpectedDefaultClauseExceptionDefaultMessage = "An unexpected clause was found";
		public static readonly string UnexpectedDefaultClauseExceptionDefaultTemplate = "An unexpected clause clause was found. Entity of type '{0}' does not accept {1}";
		public static readonly string InvalidElseStatementClauseExceptionDefaultMessage = "An out-of-scope else statement was found";
		public static readonly string InvalidElseStatementClauseExceptionDefaultTemplate = "An out-of-scope else statement was found in line {0} - '{1}' - Check else statements are in rules blocks";


		// ---------------------------------------------------------------------------------


	}
}
