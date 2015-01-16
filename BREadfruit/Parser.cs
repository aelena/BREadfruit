#define DISABLECONSTRAINTS
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using BREadfruit.Clauses;
using BREadfruit.Conditions;
using BREadfruit.Exceptions;
using BREadfruit.Helpers;

namespace BREadfruit
{
	public class Parser
	{

		private LineParser _lineParser = new LineParser ();
		private List<LineInfo> _parsedLines = new List<LineInfo> ();

		private IList<Entity> _entities = new List<Entity> ();


		/// <summary>
		/// Returns a collection of all parsed line in the form
		/// of a LineInfo list.
		/// </summary>
		public IEnumerable<LineInfo> ParsedLines
		{
			get
			{ return this._parsedLines; }
		}

		/// <summary>
		/// Main object graph, where all parsed entities and all their data
		/// (defaults, rules, triggers, actions and constraints) are to be found.
		/// </summary>
		internal IEnumerable<Entity> Entities
		{
			get
			{
				return this._entities;
			}
		}


		// ---------------------------------------------------------------------------------


		/// <summary>
		/// Main entry point for parsing a file.
		/// Reads a file containing rules and store every line in array
		/// in order to pass the read lines to the parsing engine.
		/// </summary>
		/// <param name="filePath">File containing rules to be parsed.</param>
		/// <returns>A collection of entities.</returns>
		public IEnumerable<Entity> ParseRuleSet ( string filePath )
		{
			this.CheckFileExists ( filePath );

			var line = String.Empty;
			List<string> lines = new List<string> ();
			using ( var sr = new StreamReader ( filePath ) )
			{
				while ( line != null )
				{
					line = sr.ReadLine ();
					lines.Add ( line );
				}
			}

			return ParseRuleSet ( lines );
		}


		// ---------------------------------------------------------------------------------

		public IEnumerable<Entity> ParseRuleSetAsString ( string ruleSet )
		{
			return ParseRuleSet ( ruleSet.Split ( new [] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries ) );
		}


		// ---------------------------------------------------------------------------------

		/// <summary>
		/// Main entry point for parsing a file.
		/// Allows a caller to send in a list of strings (standard text lines)
		/// containing all the entities and rules to be parsed by the engine.
		/// </summary>
		/// <param name="filePath">Array of lines to be parsed.</param>
		public IEnumerable<Entity> ParseRuleSet ( IEnumerable<string> lines )
		{


			// current line counter
			var _currLine = 0;
			var _currentScope = CurrentScope.NO_SCOPE;

			foreach ( var line in lines )
			{
				_currLine++;
				if ( !( String.IsNullOrWhiteSpace ( line ) ) )
				{

					this.CheckForTestGenInstruction ( line );
					this.AddLineToCurrentEntityRepresentation ( line );

					var lineInfo = ParseLine ( line );


					// if this is a full comment line, skip it
					if ( lineInfo.Tokens.Count () == 0 )
						continue;
					lineInfo = ParseLine ( this._lineParser.TokenizeMultiplePartOperators ( lineInfo ) );

					if ( lineInfo.Tokens.First () != Grammar.ValidationRegexDefaultClause )
						lineInfo = this._lineParser.TokenizeBracketExpression ( lineInfo );

					// we need to check the indent level to make sure we set the right scope
					// this is for conditions with several action lines
					if ( ( _currentScope == CurrentScope.CONDITION_ACTIONS_BLOCK || _currentScope == CurrentScope.CONDITION_ACTIONS_ELSE_BLOCK ) && lineInfo.IndentLevel == 2 )
						_currentScope = CurrentScope.RULES_BLOCK;


					if ( lineInfo.Tokens.Contains ( Grammar.ReturnSymbol ) && _currentScope.In ( new [] { CurrentScope.RULES_BLOCK, CurrentScope.CONDITION_ACTIONS_BLOCK, CurrentScope.CONDITION_ACTIONS_ELSE_BLOCK } ) )
						this._entities.Last ().Rules.Last ().IsFinalRule = true;

					#region " --- entity block --- "
					if ( lineInfo.Tokens.First ().Token.Equals ( Grammar.EntitySymbol.Token, StringComparison.InvariantCultureIgnoreCase ) )
					{
						this.CheckIfVisibleAndEnabledDefaultsAreSet ();
						_currentScope = ProcessEntityBlock ( _currLine, _currentScope, line, lineInfo );
						this._entities.Last ().TextRepresentation += __lastTestGenCommandFound;
						this._entities.Last ().TextRepresentation += Environment.NewLine;
						this._entities.Last ().TextRepresentation += line;
						this._entities.Last ().TextRepresentation += Environment.NewLine;
						__lastTestGenCommandFound = "";
						continue;
					}
					#endregion


					#region " --- with block --- "
					if ( lineInfo.Tokens.First ().Token.Equals ( Grammar.WithSymbol.Token, StringComparison.InvariantCultureIgnoreCase ) )
					{
						_currentScope = this.ProcessWithClauseStatement ( _currLine, _currentScope, line, lineInfo );
						if ( _currentScope != CurrentScope.NO_SCOPE )
							continue;   // ugly... but for the moment...
					}
					#endregion


					#region " --- default block --- "
					// now, if we're in the scope of the defaults block
					if ( _currentScope == CurrentScope.DEFAULTS_BLOCK )
					{
						try
						{
							if ( lineInfo.Tokens.First ().In ( new [] { Grammar.ToolTipDefaultClause, Grammar.DataFieldDefaultClause, Grammar.ValueFieldDefaultClause } ) )
							{
								CheckForStringTokens ( line, lineInfo );
							}
							// then try and parse a default clause
							if ( lineInfo.Tokens.First () == Grammar.DefineColumnDefaultClause && this._entities.Last ().TypeDescription != Grammar.GridSymbol.Token )
								throw new UnexpectedClauseException ( Grammar.UnexpectedDefaultClauseExceptionDefaultMessage + "\r\n" + "Cannot define a default column in an Entity that is not a grid." );
							this._entities.Last ().AddDefaultClause ( this.ConfigureDefaultClause ( lineInfo ) );
						}
						catch ( Exception ex )
						{
							throw new Exception ( String.Format ( "Line {2} {0} caused exception : {1}", lineInfo.Representation, ex.Message, _currLine ) );
						}
					}
					#endregion


					#region " --- check for elses --- "

					if ( lineInfo.Tokens.First ().Token.Equals ( Grammar.ElseSymbol.Token, StringComparison.InvariantCultureIgnoreCase ) )
					{
						// check we are in the current scope for else to appear
						if ( _currentScope != CurrentScope.RULES_BLOCK )
							throw new InvalidElseStatementClauseException (
								String.Format ( Grammar.InvalidElseStatementClauseExceptionDefaultTemplate, _currLine, line ) );
						// we keep in the same scope, that of RULES, so change the scope and go on
						_currentScope = CurrentScope.CONDITION_ACTIONS_ELSE_BLOCK;
						// if the else is in a separate line
						if ( lineInfo.Tokens.Count () == 1 )
							continue;
						else
						{
							// otherwise fuck our brains inside out 
							lineInfo.RemoveTokens ( 1 );
						}
					}

					#endregion


					#region " --- rules block --- "
					if ( _currentScope == CurrentScope.RULES_BLOCK )
					{

						var _rule = new Rule ();
						var _conds = this._lineParser.ExtractConditions ( lineInfo, this._entities.Last ().Name );
						foreach ( var c in _conds )
							_rule.AddCondition ( c );

						this._entities.Last ().AddRule ( _rule );

						// check presence of token 'then'
						if ( lineInfo.Tokens.Contains ( Grammar.ThenSymbol ) )
						{
							// if then is the last token, then there are actions
							if ( lineInfo.Tokens.Last ().Equals ( Grammar.ThenSymbol ) )
							{
								_currentScope = CurrentScope.CONDITION_ACTIONS_BLOCK;
								continue;
							}
							else
							{
								// if it's not the last then we have the action right after the token
								// check if it's a valid action
								if ( lineInfo.Tokens.Penultimate () == Grammar.ThenSymbol )
								{
									// CAVEAT here, a rule like this fits here
									// DDLVDCountry.value in {FR,GF,GP,RE,MQ,YT,NC,PF,PM,WF,MC,TF} then return
									if ( lineInfo.Tokens.Last () == Grammar.ReturnSymbol )
									{
										this._entities.Last ().Rules.Last ().IsFinalRule = true;
										continue;
									}
									var _unaryAction = new UnaryAction ( Grammar.GetSymbolByToken ( lineInfo.Tokens.Last ().Token ),
										this.Entities.Last ().Name );
									_rule.Conditions.Last ().AddUnaryAction ( _unaryAction );
									continue;
								}
								// then it's a long result action line
								if ( lineInfo.Tokens.ElementAtFromLast ( 3 ) == Grammar.ThenSymbol )
								{
									var _resClause = GetClauseAfterThen ( lineInfo );
									if ( _resClause.First () == Grammar.VisibleDefaultClause )
									{
										var _ra = new ResultAction ( _resClause.First (), _resClause.ElementAt ( 1 ), this.Entities.Last ().Name );
										this._entities.Last ().Rules.Last ().Conditions.Last ().AddResultAction ( _ra );
										continue;
									}
									if ( _resClause.First () == Grammar.ThisSymbol )
									{
										var _ra = new ResultAction ( _resClause.ElementAt ( 1 ), _resClause.ElementAt ( 2 ), this.Entities.Last ().Name );
										this._entities.Last ().Rules.Last ().Conditions.Last ().AddResultAction ( _ra );
										continue;
									}

									var _penultimate = Grammar.GetSymbolByToken ( lineInfo.Tokens.Penultimate ().Token );
									UnaryAction _unaryAction = null;
									if ( _penultimate != null )
										_unaryAction = new UnaryAction ( Grammar.GetSymbolByToken ( lineInfo.Tokens.Penultimate ().Token ),
										   lineInfo.Tokens.Last ().Token );
									else
										_unaryAction = new UnaryAction ( Grammar.GetSymbolByToken ( lineInfo.Tokens.Last ().Token ),
											lineInfo.Tokens.Penultimate ().Token );
									_rule.Conditions.Last ().AddUnaryAction ( _unaryAction );
									continue;

								}
								if ( lineInfo.Tokens.ElementAtFromLast ( 4 ) == Grammar.ThenSymbol )
								{
									ResultAction _ra = null;
									var _resClause = GetClauseAfterThen ( lineInfo );
									if ( _resClause.First () == Grammar.ThisSymbol )
									{
										_ra = new ResultAction ( _resClause.ElementAt ( 1 ), _resClause.ElementAt ( 2 ), this.Entities.Last ().Name );
										this._entities.Last ().Rules.Last ().Conditions.Last ().AddResultAction ( _ra );
										continue;
									}

									var thenClause = lineInfo.GetSymbolsAfterThen ();
									if ( thenClause.Any ( x => x == Grammar.ThisSymbol ) )
									{
										// this should be the value
										var _index = thenClause.Where ( x => !Grammar.Symbols.Contains ( x ) ).First ();
										var _symbol = thenClause.Where ( x => Grammar.Symbols.Contains ( x ) ).First ();
										_ra = new ResultAction ( _symbol, _index, this._entities.Last ().Name );
									}
									else
									{
										_ra = new ResultAction ( Grammar.GetSymbolByToken ( thenClause.First ().Token ),
										   thenClause.ElementAt ( 1 ).Token, thenClause.Last ().Token );
									}
									_rule.Conditions.Last ().AddResultAction ( _ra );

									continue;
								}
								if ( lineInfo.Tokens.ElementAtFromLast ( 5 ) == Grammar.ThenSymbol )
								{
									if ( lineInfo.Tokens.Penultimate () != Grammar.InSymbol )
										throw new MissingInClauseException ( String.Format ( Grammar.MissingInClauseExceptionMessageTemplate, _currLine, line ) );
									var thenClause = lineInfo.GetSymbolsAfterThen ();
									var _ra = new ResultAction ( Grammar.GetSymbolByToken ( thenClause.First ().Token ),
										thenClause.ElementAt ( 1 ).Token, thenClause.Last ().Token );
									_rule.Conditions.Last ().AddResultAction ( _ra );
									continue;
								}

								//// last case here
								//// detect then clauses that have commans in their actions in order to parse those...
								//var thenClause2 = lineInfo.GetSymbolsAfterThen ();
								//if ( thenClause2.JoinTogether ().Token.IndexOf ( "," ) > 0 )
								//{
								//}

								// tentatively, if we make it here, we haven't parsed something...
								throw new InvalidLineFoundException ( String.Format ( Grammar.InvalidLineFoundExceptionDefaultTemplate, _currLine, line ) );

							}

							//this._entities.Last ().AddRule ( _rule );
						}
						else
							throw new Exception (
								String.Format ( Grammar.MissingThenClauseExceptionMessageTemplate, _currLine,
									lineInfo.Representation ) );

					}
					#endregion


					#region " --- condition-less actions block --- "
					if ( _currentScope == CurrentScope.ACTIONS_BLOCK )
					{
						CheckForStringTokens ( line, lineInfo );

						lineInfo = CheckForWithClauses ( lineInfo );

						if ( lineInfo.Tokens.Count () >= 6 )
						{
							if ( lineInfo.HasSymbol ( Grammar.WithArgumentsSymbol ) && lineInfo.HasSymbol ( Grammar.InSymbol ) )
							{
								if ( lineInfo.HasSymbol ( Grammar.WithQuerySymbol ) )
								{
									var _ra = ConfigureQueriedResultActionWithArguments ( lineInfo );
									this._entities.Last ().AddResultAction ( _ra );
								}
								else
								{
									var _ra = ConfigureResultActionWithArguments ( lineInfo );
									this._entities.Last ().AddResultAction ( _ra );
								}
							}
							if ( lineInfo.HasSymbol ( Grammar.WithArgumentsSymbol ) && lineInfo.HasSymbol ( Grammar.WithOutputArgumentsSymbol ) )
							{
								if ( lineInfo.HasSymbol ( Grammar.WithQuerySymbol ) )
								{
									var _ra = ConfigureQueriedResultActionWithArguments ( lineInfo );
									// output arguments, if they are there, should be last token in representation
									var _args = lineInfo.Tokens.Last ().Token.Split ( new char [] { ',' }, StringSplitOptions.RemoveEmptyEntries );
									foreach ( var _a in _args )
									{
										var _argPair = _a.Split ( new char [] { ':' }, StringSplitOptions.RemoveEmptyEntries );
										_ra.AddOutputArgument ( _argPair.Last ().Trim ().MultipleReplace ( Grammar.ValidQuoteSymbols ), _argPair.First ().Trim ().ReplaceFirstAndLastOnly ( "\"" ) );
									}
									this._entities.Last ().AddResultAction ( _ra );
								}
								else
								{
									var _ra = ConfigureResultActionWithArguments ( lineInfo );
									// output arguments, if they are there, should be last token in representation
									var _args = lineInfo.Tokens.Last ().Token.Split ( new char [] { ',' }, StringSplitOptions.RemoveEmptyEntries );
									foreach ( var _a in _args )
									{
										var _argPair = _a.Split ( new char [] { ':' }, StringSplitOptions.RemoveEmptyEntries );
										_ra.AddOutputArgument ( _argPair.Last ().Trim ().MultipleReplace ( Grammar.ValidQuoteSymbols ), _argPair.First ().Trim ().ReplaceFirstAndLastOnly ( "\"" ) );
									}
									this._entities.Last ().AddResultAction ( _ra );
								}
							}
						}
						// see if this is resultaction
						if ( lineInfo.Tokens.Count () == 4 )
						{
							if ( lineInfo.Tokens.Contains ( Grammar.WithArgumentsSymbol ) )
							{
								var _r = new ParameterizedResultAction ( Grammar.GetSymbolByToken ( lineInfo.Tokens.First ().Token ),
									   lineInfo.Tokens.ElementAt ( 1 ).Token, null, lineInfo );
								var _args = lineInfo.Tokens.ElementAt ( 3 ).Token.Split ( new char [] { ',' }, StringSplitOptions.RemoveEmptyEntries );
								foreach ( var _a in _args )
								{
									var _argPair = _a.Split ( new char [] { ':' }, StringSplitOptions.RemoveEmptyEntries );
									_r.AddArgument ( _argPair.First ().Trim ().MultipleReplace ( Grammar.ValidQuoteSymbols ), _argPair.Last ().Trim ().ReplaceFirstAndLastOnly ( "\"" ) );
								}
								this._entities.Last ().AddResultAction ( _r );
							}
							else
							{
								var _ra = new ResultAction ( Grammar.GetSymbolByToken ( lineInfo.Tokens.First ().Token ),
											lineInfo.Tokens.ElementAt ( 1 ).Token, lineInfo.Tokens.Last ().Token );
								this._entities.Last ().AddResultAction ( _ra );
							}
						}

						if ( lineInfo.Tokens.Count () == 3 )
						{
							if ( CheckForThreePartUnaryAction ( lineInfo, Grammar.MandatoryDefaultClause,
																		  Grammar.MakeMandatoryUnaryActionSymbol,
																		  Grammar.MakeNonMandatoryUnaryActionSymbol,
																		  PropertyType.MANDATORY ) )
								continue;
							if ( CheckForThreePartUnaryAction ( lineInfo, Grammar.VisibleDefaultClause,
																		  Grammar.VisibleUnaryActionSymbol,
																		  Grammar.HideUnaryActionSymbol,
																		  PropertyType.VISIBLE ) )
								continue;
							if ( CheckForThreePartUnaryAction ( lineInfo, Grammar.EnabledDefaultClause,
																		  Grammar.EnableUnaryActionSymbol,
																		  Grammar.DisableUnaryActionSymbol,
																		  PropertyType.ENABLED ) )
								continue;

						}
						// or unary action
						if ( lineInfo.Tokens.Count () == 2 )
						{
							if ( this._lineParser.IsAValidSentence ( lineInfo ) )
							{
								if ( !Grammar.Symbols.Contains ( lineInfo.Tokens.First ().Token ) &&
									 Grammar.Symbols.Contains ( lineInfo.Tokens.Last ().Token ) )
								{
									var _ua = new UnaryAction ( Grammar.GetSymbolByToken ( lineInfo.Tokens.Last ().Token ),
										lineInfo.Tokens.First ().Token );
									_ua.Property = MapToPropertyType ( Grammar.GetSymbolByToken ( lineInfo.Tokens.Last ().Token ) );
									this._entities.Last ().AddUnaryAction ( _ua );
									continue;
								}
								if ( Grammar.Symbols.Contains ( lineInfo.Tokens.First ().Token ) &&
									 !Grammar.Symbols.Contains ( lineInfo.Tokens.Last ().Token ) )
								{
									var _ua = new UnaryAction ( Grammar.GetSymbolByToken ( lineInfo.Tokens.First ().Token ),
										lineInfo.Tokens.Last ().Token );
									_ua.Property = MapToPropertyType ( Grammar.GetSymbolByToken ( lineInfo.Tokens.Last ().Token ) );
									this._entities.Last ().AddUnaryAction ( _ua );
									continue;
								}
								throw new InvalidLineFoundException ( String.Format ( Grammar.InvalidLineFoundExceptionDefaultTemplate, _currLine, line ) );
							}
							else
								throw new InvalidLineFoundException ( String.Format ( Grammar.InvalidLineFoundExceptionDefaultTemplate, _currLine, line ) );
						}
					}
					#endregion


					#region " --- condition  actions block --- "
					if ( _currentScope == CurrentScope.CONDITION_ACTIONS_BLOCK || _currentScope == CurrentScope.CONDITION_ACTIONS_ELSE_BLOCK )
					{

						if ( lineInfo.Tokens.First () == Grammar.LabelDefaultClause )
						{
							var _ra = new ResultAction ( Grammar.LabelDefaultClause, lineInfo.Tokens.Skip ( 1 ).JoinTogether ().Token.Replace ( "\"", "" ),
								 this.Entities.Last ().Name );
							this._entities.Last ().Rules.Last ().HasElseClause = _currentScope == CurrentScope.CONDITION_ACTIONS_ELSE_BLOCK;
							AddResultAction ( _ra, _currentScope == CurrentScope.CONDITION_ACTIONS_ELSE_BLOCK );
							continue;
						}
						if ( lineInfo.Tokens.First () == Grammar.ToolTipDefaultClause )
						{
							var _ra = new ResultAction ( Grammar.ToolTipDefaultClause, lineInfo.Tokens.Skip ( 1 ).JoinTogether ().Token.Replace ( "\"", "" ),
								 this.Entities.Last ().Name );
							this._entities.Last ().Rules.Last ().HasElseClause = _currentScope == CurrentScope.CONDITION_ACTIONS_ELSE_BLOCK;
							AddResultAction ( _ra, _currentScope == CurrentScope.CONDITION_ACTIONS_ELSE_BLOCK );
							continue;
						}

						if ( lineInfo.Tokens.Count () == 1 )
						{
							if ( lineInfo.Tokens.Contains ( Grammar.MandatoryDefaultClause ) )
							{
								AddMandatoryUnaryAction ( _currentScope );
								continue;
							}
						}
						// TODO: REFACTOR THIS ENTIRE SECTION AND MAKE MORE HOMOGENEOUS -------------------
						// unary action
						#region " --- 2 tokens --- "
						if ( lineInfo.Tokens.Count () == 2 )
						{
							if ( lineInfo.Tokens.First () == Grammar.ShowElementUnaryActionSymbol )
							{
								if ( Regex.IsMatch ( lineInfo.Representation.Trim (), Grammar.ShowElementLineRegex, RegexOptions.IgnoreCase ) )
								{
									var _ua = new UnaryAction ( lineInfo.Tokens.First (),
										lineInfo.Tokens.Last ().Token == "this" ? this.Entities.Last ().Name : lineInfo.Tokens.Last ().Token );
									AddUnaryActionToCurrentRule ( _currentScope, _ua );
								}
								else
									throw new InvalidShowStatementClauseException (
										String.Format ( Grammar.InvalidShowElementExceptionMessageTemplate, _currLine + 1, line.Trim () ) );
							}
							if ( lineInfo.Tokens.First () == Grammar.HideUnaryActionSymbol )
							{
								if ( Regex.IsMatch ( lineInfo.Representation.Trim (), Grammar.HideElementLineRegex, RegexOptions.IgnoreCase ) )
								{
									var _ua = new UnaryAction ( lineInfo.Tokens.First (),
										lineInfo.Tokens.Last ().Token == "this" ? this.Entities.Last ().Name : lineInfo.Tokens.Last ().Token );
									AddUnaryActionToCurrentRule ( _currentScope, _ua );
								}
								else
									throw new InvalidHideStatementClauseException (
										String.Format ( Grammar.InvalidHideElementExceptionMessageTemplate, _currLine + 1, line.Trim () ) );
							}

							if ( lineInfo.Tokens.Contains ( Grammar.MakeMandatoryUnaryActionSymbol ) )
							{
								ProcessUnaryAction ( lineInfo, Grammar.MakeMandatoryUnaryActionSymbol, _currentScope == CurrentScope.CONDITION_ACTIONS_ELSE_BLOCK );
								this._entities.Last ().Rules.Last ().HasElseClause = _currentScope == CurrentScope.CONDITION_ACTIONS_ELSE_BLOCK;
								continue;
							}

							if ( lineInfo.Tokens.Contains ( Grammar.MandatoryDefaultClause ) )
							{
								var _ua = new UnaryAction ( Grammar.MandatoryDefaultClause, lineInfo.Tokens.First () == Grammar.ThisSymbol ? this.Entities.Last ().Name : lineInfo.Tokens.First ().Token );
								AddUnaryActionToCurrentRule ( _currentScope, _ua );
								continue;
							}

							if ( lineInfo.Tokens.Contains ( Grammar.MakeNonMandatoryUnaryActionSymbol ) )
							{
								var _ua = new UnaryAction ( Grammar.MakeNonMandatoryUnaryActionSymbol,
									lineInfo.Tokens.First ().Token == "this" ? this.Entities.Last ().Name : lineInfo.Tokens.First ().Token );
								AddUnaryActionToCurrentRule ( _currentScope, _ua );
								continue;
							}

							if ( lineInfo.Tokens.Contains ( Grammar.NotVisibleUnaryActionSymbol ) )
							{
								ProcessUnaryAction ( lineInfo, Grammar.NotVisibleUnaryActionSymbol, _currentScope == CurrentScope.CONDITION_ACTIONS_ELSE_BLOCK );
								this._entities.Last ().Rules.Last ().HasElseClause = _currentScope == CurrentScope.CONDITION_ACTIONS_ELSE_BLOCK;
								continue;
							}

							if ( lineInfo.Tokens.First () == ( Grammar.MaxlengthDefaultClause ) )
							{
								var _ra = new ResultAction ( Grammar.GetSymbolByToken ( lineInfo.Tokens.First ().Token ), lineInfo.Tokens.Last ().Token,
									this.Entities.Last ().Name );
								AddResultActionToCurrentRule ( _currentScope, _ra );
								continue;
							}

							if ( lineInfo.Tokens.First () == ( Grammar.SetValidationRegexUnaryActionSymbol ) ||
								 lineInfo.Tokens.First () == ( Grammar.ValidationRegexDefaultClause ) )
							{
								var _ra = new ResultAction ( Grammar.GetSymbolByToken ( lineInfo.Tokens.First ().Token ), lineInfo.Tokens.Last ().Token.Replace ( "\"", "" ),
									this.Entities.Last ().Name );
								AddResultActionToCurrentRule ( _currentScope, _ra );
								continue;
							}

							if ( lineInfo.Tokens.First () == Grammar.EnabledDefaultClause )
							{
								// ProcessUnaryAction ( lineInfo, Grammar.EnabledDefaultClause );
								var _ra = new ResultAction ( Grammar.GetSymbolByToken ( lineInfo.Tokens.First ().Token ), lineInfo.Tokens.Last ().Token.Replace ( "\"", "" ),
								   this.Entities.Last ().Name );
								AddResultActionToCurrentRule ( _currentScope, _ra );
								continue;
							}
							if ( lineInfo.Tokens.First () == Grammar.VisibleDefaultClause )
							{
								//ProcessUnaryAction ( lineInfo, Grammar.VisibleDefaultClause );
								var _ra = new ResultAction ( Grammar.GetSymbolByToken ( lineInfo.Tokens.First ().Token ), lineInfo.Tokens.Last ().Token.Replace ( "\"", "" ),
								   this.Entities.Last ().Name );
								AddResultActionToCurrentRule ( _currentScope, _ra );
								continue;
							}
							if ( lineInfo.Tokens.Last () == Grammar.VisibleDefaultClause )
							{
								var _ra = new ResultAction ( Grammar.VisibleDefaultClause, true, GetEntityName ( lineInfo ) );
								AddResultActionToCurrentRule ( _currentScope, _ra );
								continue;
							}
							if ( lineInfo.Tokens.Last () == Grammar.MandatoryDefaultClause )
							{
								var _ra = new ResultAction ( Grammar.MandatoryDefaultClause, true, GetEntityName ( lineInfo ) );
								AddResultActionToCurrentRule ( _currentScope, _ra );
								continue;
							}
							if ( lineInfo.Tokens.Last () == Grammar.DisableUnaryActionSymbol || lineInfo.Tokens.First () == Grammar.DisableUnaryActionSymbol )
							{
								var _ua = new UnaryAction ( Grammar.DisableUnaryActionSymbol,
									lineInfo.Tokens.First () == Grammar.DisableUnaryActionSymbol ?
									lineInfo.Tokens.ElementAt ( 1 ).Token == "this" ? this.Entities.Last ().Name : lineInfo.Tokens.ElementAt ( 1 ).Token
									: lineInfo.Tokens.ElementAt ( 0 ).Token == "this" ? this.Entities.Last ().Name : lineInfo.Tokens.ElementAt ( 0 ).Token );
								AddUnaryActionToCurrentRule ( _currentScope, _ua );
								continue;

							}

							if ( lineInfo.Tokens.First () == Grammar.SetValueActionSymbol || lineInfo.Tokens.First () == Grammar.LoadDataDefaultClause )
							{
								var _ra = new ResultAction ( Grammar.GetSymbolByToken ( lineInfo.Tokens.First ().Token ),
										lineInfo.Tokens.ElementAt ( 1 ).Token, this.Entities.Last ().Name );
								AddResultActionToCurrentRule ( _currentScope, _ra );
								continue;
							}

						}
						#endregion

						#region " --- 3 tokens --- "
						if ( lineInfo.Tokens.Count () == 3 )
						{
							if ( lineInfo.Tokens.First () == Grammar.EnabledDefaultClause )
							{
								var _ra = new ResultAction ( Grammar.GetSymbolByToken ( lineInfo.Tokens.First ().Token ), lineInfo.Tokens.Last ().Token.Replace ( "\"", "" ),
								   lineInfo.Tokens.ElementAt ( 1 ).Token == "this" ? this.Entities.Last ().Name : lineInfo.Tokens.ElementAt ( 1 ).Token );
								AddResultActionToCurrentRule ( _currentScope, _ra );
								continue;
							}
							if ( lineInfo.Tokens.First () == Grammar.VisibleDefaultClause )
							{
								var _ra = new ResultAction ( Grammar.GetSymbolByToken ( lineInfo.Tokens.First ().Token ), lineInfo.Tokens.Last ().Token.Replace ( "\"", "" ),
								   lineInfo.Tokens.ElementAt ( 1 ).Token == "this" ? this.Entities.Last ().Name : lineInfo.Tokens.ElementAt ( 1 ).Token );
								AddResultActionToCurrentRule ( _currentScope, _ra );
								continue;
							}
							if ( lineInfo.Tokens.Contains ( Grammar.MandatoryDefaultClause ) )
							{
								//ResultAction _ra = null;
								//var _booleanValue = lineInfo.Tokens.ContainsAny2 ( new List<Symbol> () { Grammar.TrueSymbol, Grammar.FalseSymbol } );
								//var _indexOfThis = lineInfo.Tokens.ToList ().IndexOf ( Grammar.ThisSymbol );
								//if ( _indexOfThis > 0 )
								//    _ra = new ResultAction ( Grammar.MandatoryDefaultClause, _booleanValue.Item2,
								//               lineInfo.Tokens.ElementAt ( lineInfo.Tokens.ToList ().IndexOf ( Grammar.ThisSymbol ) ).Token == "this" ?
								//                    this.Entities.Last ().Name : lineInfo.Tokens.ElementAt ( lineInfo.Tokens.ToList ().IndexOf ( Grammar.ThisSymbol ) ).Token );
								//else
								//    _ra = new ResultAction ( Grammar.MandatoryDefaultClause, _booleanValue.Item2, lineInfo.Tokens.First ().Token );

								//this._entities.Last ().Rules.Last ().Conditions.Last ().AddResultAction ( _ra );

								this._entities.Last ().Rules.Last ().HasElseClause = _currentScope == CurrentScope.CONDITION_ACTIONS_ELSE_BLOCK;
								AddTrueFalseAction ( lineInfo, Grammar.MakeMandatoryUnaryActionSymbol, Grammar.MakeNonMandatoryUnaryActionSymbol, _currentScope == CurrentScope.CONDITION_ACTIONS_ELSE_BLOCK );
								continue;
							}
							if ( lineInfo.Tokens.Contains ( Grammar.EnabledDefaultClause ) )
							{
								this._entities.Last ().Rules.Last ().HasElseClause = _currentScope == CurrentScope.CONDITION_ACTIONS_ELSE_BLOCK;
								AddTrueFalseAction ( lineInfo, Grammar.EnableUnaryActionSymbol, Grammar.DisableUnaryActionSymbol, _currentScope == CurrentScope.CONDITION_ACTIONS_ELSE_BLOCK );
								continue;
							}
							if ( lineInfo.Tokens.Contains ( Grammar.VisibleDefaultClause ) )
							{
								this._entities.Last ().Rules.Last ().HasElseClause = _currentScope == CurrentScope.CONDITION_ACTIONS_ELSE_BLOCK;
								AddTrueFalseAction ( lineInfo, Grammar.VisibleUnaryActionSymbol, Grammar.NotVisibleUnaryActionSymbol, _currentScope == CurrentScope.CONDITION_ACTIONS_ELSE_BLOCK );
								continue;
							}


						}
						#endregion

						#region " --- 4 tokens --- "

						lineInfo = CheckForWithClauses ( lineInfo );

						if ( lineInfo.Tokens.Count () == 4 && lineInfo.Tokens.Contains ( Grammar.InSymbol ) )
						{
							var _ra = new ResultAction ( Grammar.GetSymbolByToken ( lineInfo.Tokens.First ().Token ),
										lineInfo.Tokens.ElementAt ( 1 ).Token, lineInfo.Tokens.Last ().Token );

							AddResultActionToCurrentRule ( _currentScope, _ra );
							continue;
						}

						if ( lineInfo.Tokens.Count () >= 4 )
						{

							if ( !lineInfo.HasSymbol ( Grammar.WithArgumentsSymbol ) )
							{
								if ( lineInfo.HasSymbol ( Grammar.WithOutputArgumentsSymbol ) )
								{
									var _ra = ConfigureResultActionWithArguments ( lineInfo, false );
									// output arguments, if they are there, should be last token in representation
									ExtractAndAddOutputArguments ( lineInfo, _ra );
									AddResultActionToCurrentRule ( _currentScope, _ra );
								}
								continue;
							}

							if ( lineInfo.HasSymbol ( Grammar.WithArgumentsSymbol ) && lineInfo.HasSymbol ( Grammar.InSymbol ) )
							{
								if ( lineInfo.HasSymbol ( Grammar.WithQuerySymbol ) )
								{
									var _ra = ConfigureQueriedResultActionWithArguments ( lineInfo );
									AddResultActionToCurrentRule ( _currentScope, _ra );
								}
								else
								{
									var _ra = ConfigureResultActionWithArguments ( lineInfo );
									AddResultActionToCurrentRule ( _currentScope, _ra );
								}
								continue;
							}
							if ( lineInfo.HasSymbol ( Grammar.WithArgumentsSymbol ) && lineInfo.HasSymbol ( Grammar.WithOutputArgumentsSymbol ) )
							{
								if ( lineInfo.HasSymbol ( Grammar.WithQuerySymbol ) )
								{
									var _ra = ConfigureQueriedResultActionWithArguments ( lineInfo );
									// output arguments, if they are there, should be last token in representation
									ExtractAndAddOutputArguments ( lineInfo, _ra );
									AddResultActionToCurrentRule ( _currentScope, _ra );
								}
								else
								{
									var _ra = ConfigureResultActionWithArguments ( lineInfo );
									// output arguments, if they are there, should be last token in representation
									ExtractAndAddOutputArguments ( lineInfo, _ra );
									AddResultActionToCurrentRule ( _currentScope, _ra );
								}
								continue;

							}

							// last case
							if ( lineInfo.HasSymbol ( Grammar.WithArgumentsSymbol ) )
							{
								var _ra = ConfigureResultActionWithArguments ( lineInfo, false );
								// output arguments, if they are there, should be last token in representation
								ExtractAndAddArguments ( lineInfo, _ra );
								AddResultActionToCurrentRule ( _currentScope, _ra );
							}
						}
						#endregion

					}
					#endregion


					#region " --- trigger block --- "
					if ( _currentScope == CurrentScope.TRIGGERS_BLOCK )
					{
						var _x = from op in Grammar.TriggerSymbols
								 where op.Aliases.Count () > 0
								 && op.Aliases.Contains ( lineInfo.Tokens.Skip ( 1 ).JoinTogether ().Token )
								 select op;

						Trigger _trigger = null;

						if ( lineInfo.Tokens.Count () == 2 && lineInfo.Tokens.First () == Grammar.ThisSymbol )
						{
							var _firstToken = lineInfo.Tokens.First ().Token;
							if ( lineInfo.Tokens.First ().Token.Trim ().StartsWith ( "this" ) )
								_firstToken = _firstToken.Replace ( "this", this._entities.Last ().Name );

							if ( _x != null && _x.Count () > 0 )
								_trigger = new Trigger ( _firstToken, _x.First ().Token );
							else
								_trigger = new Trigger ( _firstToken, lineInfo.Tokens.ElementAt ( 1 ).Token );

						}
						else
						{
							if ( lineInfo.Tokens.Count () == 2 )
							{
								var _firstToken = lineInfo.Tokens.First ().Token;
								if ( lineInfo.Tokens.First ().Token.Trim ().StartsWith ( "this" ) )
									_firstToken = _firstToken.Replace ( "this", this._entities.Last ().Name );

								if ( _x.Count () > 0 )
									_trigger = new Trigger ( _firstToken, _x.First ().Token );
								else
									_trigger = new Trigger ( _firstToken, lineInfo.Tokens.ElementAt ( 1 ).Token );

							}
							if ( lineInfo.Tokens.Count () == 1 )
							{
								if ( lineInfo.Tokens.First ().In ( new List<Symbol> { Grammar.RowDeletedEventSymbol, Grammar.RowInsertedEventSymbol, Grammar.RowUpdatedEventSymbol } )
									&& this._entities.Last ().TypeDescription != Grammar.GridSymbol.Token )
									throw new UnexpectedClauseException ( Grammar.UnexpectedDefaultClauseExceptionDefaultMessage + "\r\n" + "Cannot define a row trigger in an Entity that is not a grid." );
								_trigger = new Trigger ( this._entities.Last ().Name, lineInfo.Tokens.First ().Token );
							}
						}

						this._entities.Last ().AddTrigger ( _trigger );


					}
					#endregion


					#region " --- constraints block --- "
#if !DISABLECONSTRAINTS
					if ( _currentScope == CurrentScope.CONSTRAINTS_BLOCK )
					{
						// there should be only one token
						this._entities.Last ().AddConstraint ( new Constraint ( lineInfo.Tokens.First ().Token ) );
					}
#endif
					#endregion


				}


			}



			this.CheckIfVisibleAndEnabledDefaultsAreSet ();

			return this.Entities;


		}

		private static void ExtractAndAddArguments ( LineInfo lineInfo, ParameterizedResultAction _ra )
		{
			var _args = lineInfo.Tokens.Last ().Token.Split ( new char [] { ',' }, StringSplitOptions.RemoveEmptyEntries );
			foreach ( var _a in _args )
			{
				var _argPair = _a.Split ( new char [] { ':' }, StringSplitOptions.RemoveEmptyEntries );
				_ra.AddArgument ( _argPair.Last ().Trim ().MultipleReplace ( Grammar.ValidQuoteSymbols ), _argPair.First ().Trim ().ReplaceFirstAndLastOnly ( "\"" ) );
			}
		}

		private static void ExtractAndAddOutputArguments ( LineInfo lineInfo, ParameterizedResultAction _ra )
		{
			var _args = lineInfo.Tokens.Last ().Token.Split ( new char [] { ',' }, StringSplitOptions.RemoveEmptyEntries );
			foreach ( var _a in _args )
			{
				var _argPair = _a.Split ( new char [] { ':' }, StringSplitOptions.RemoveEmptyEntries );
				_ra.AddOutputArgument ( _argPair.Last ().Trim ().MultipleReplace ( Grammar.ValidQuoteSymbols ), _argPair.First ().Trim ().ReplaceFirstAndLastOnly ( "\"" ) );
			}
		}

		private string GetEntityName ( LineInfo lineInfo )
		{
			var _ename = lineInfo.Tokens.First ().Token;
			if ( lineInfo.Tokens.First () == Grammar.ThisSymbol )
				_ename = this.Entities.Last ().Name;
			return _ename;
		}

		private void AddUnaryActionToCurrentRule ( CurrentScope _currentScope, UnaryAction _ua )
		{
			this._entities.Last ().Rules.Last ().HasElseClause = _currentScope == CurrentScope.CONDITION_ACTIONS_ELSE_BLOCK;
			this._entities.Last ().Rules.Last ().Conditions.Last ().AddUnaryAction ( _ua, _currentScope == CurrentScope.CONDITION_ACTIONS_ELSE_BLOCK );

		}

		private void AddResultActionToCurrentRule ( CurrentScope _currentScope, ResultAction _ra )
		{
			this._entities.Last ().Rules.Last ().HasElseClause = _currentScope == CurrentScope.CONDITION_ACTIONS_ELSE_BLOCK;
			this._entities.Last ().Rules.Last ().Conditions.Last ().AddResultAction ( _ra, _currentScope == CurrentScope.CONDITION_ACTIONS_ELSE_BLOCK );
		}

		private LineInfo CheckForWithClauses ( LineInfo lineInfo )
		{
			if ( this._lineParser.LineInfoContainsArgumentkeyValuePairs ( lineInfo ) )
			{
				lineInfo = this._lineParser.TokenizeArgumentKeyValuePairs ( lineInfo );
			}
			if ( this._lineParser.LineInfoContainsOutputkeyValuePairs ( lineInfo ) )
			{
				lineInfo = this._lineParser.TokenizeOutputKeyValuePairs ( lineInfo );
			}
			if ( lineInfo.Tokens.Contains ( Grammar.WithQuerySymbol ) )
			{
				lineInfo = this._lineParser.TokenizeQuotedText ( lineInfo, Grammar.WithQuerySymbol );
			}
			return lineInfo;
		}


		// ---------------------------------------------------------------------------------


		private PropertyType MapToPropertyType ( Symbol s )
		{
			if ( s.In ( new Symbol [] { Grammar.MandatoryDefaultClause, Grammar.MakeMandatoryUnaryActionSymbol, Grammar.MakeNonMandatoryUnaryActionSymbol } ) )
				return PropertyType.MANDATORY;
			if ( s.In ( new Symbol [] { Grammar.VisibleUnaryActionSymbol, Grammar.NotVisibleUnaryActionSymbol, Grammar.VisibleDefaultClause } ) )
				return PropertyType.VISIBLE;
			if ( s.In ( new Symbol [] { Grammar.EnableUnaryActionSymbol, Grammar.EnabledDefaultClause } ) )
				return PropertyType.ENABLED;

			// TODO: add the rest....

			// as default
			return PropertyType.NOT_SET;
		}


		// ---------------------------------------------------------------------------------


		private bool CheckForThreePartUnaryAction ( LineInfo lineInfo, Symbol symbolToCheck, Symbol ifTrueSymbol, Symbol ifFalseSymbol, PropertyType propertyType )
		{
			UnaryAction _ua = null;
			if ( lineInfo.Tokens.ElementAt ( 1 ) == symbolToCheck )
			{
				if ( lineInfo.Tokens.Last () == Grammar.TrueSymbol )
					_ua = new UnaryAction ( ifTrueSymbol, lineInfo.Tokens.ElementAt ( 0 ).Token );

				if ( lineInfo.Tokens.Last () == Grammar.FalseSymbol )
					_ua = new UnaryAction ( ifFalseSymbol, lineInfo.Tokens.ElementAt ( 0 ).Token );

				_ua.Property = propertyType;
				this._entities.Last ().AddUnaryAction ( _ua );
				return true;
			}
			return false;
		}


		// ---------------------------------------------------------------------------------


		private void AddMandatoryUnaryAction ( CurrentScope _currentScope )
		{
			var _ua = new UnaryAction ( Grammar.MandatoryDefaultClause, this.Entities.Last ().Name );
			this._entities.Last ().Rules.Last ().HasElseClause = _currentScope == CurrentScope.CONDITION_ACTIONS_ELSE_BLOCK;
			AddUnaryAction ( _ua, _currentScope == CurrentScope.CONDITION_ACTIONS_ELSE_BLOCK );
		}


		// ---------------------------------------------------------------------------------


		private static ParameterizedResultAction ConfigureResultActionWithArguments ( LineInfo lineInfo, bool parseArguments = true )
		{
			// then it must be this sort of rule
			// load data from DATASOURCE.ACTIVE_CCs with arguments {"Country" : this.value} in DDLCDCompany
			var _ra = new ParameterizedResultAction ( Grammar.GetSymbolByToken ( lineInfo.Tokens.First ().Token ),
					lineInfo.Tokens.ElementAt ( 1 ).Token, lineInfo.Tokens.Last ().Token );

			if ( parseArguments )
			{
				// quite brittle this one here...
				// replace with extension such as ElementAfterToken(xxx) or something like that.
				var _args = lineInfo.Tokens.ElementAt ( 3 ).Token.Split ( new char [] { ',' }, StringSplitOptions.RemoveEmptyEntries );
				foreach ( var _a in _args )
				{
					var _argPair = _a.Split ( new char [] { ':' }, StringSplitOptions.RemoveEmptyEntries );
					_ra.AddArgument ( _argPair.First ().Trim ().MultipleReplace ( Grammar.ValidQuoteSymbols ), _argPair.Last ().Trim ().ReplaceFirstAndLastOnly ( "\"" ) );
				}
			}
			return _ra;
		}


		// ---------------------------------------------------------------------------------


		private static ParameterizedResultAction ConfigureQueriedResultActionWithArguments ( LineInfo lineInfo )
		{
			var _ = lineInfo.Tokens.IndexOf ( x => x == Grammar.WithQuerySymbol );

			// then it must be this sort of rule
			// load data from DATASOURCE.ACTIVE_CCs with arguments {"Country" : this.value} in DDLCDCompany
			var _ra = new QueryResultAction ( lineInfo.Tokens.ElementAt ( ++_ ).Token.ReplaceFirstAndLastOnly ( "\"" ), Grammar.GetSymbolByToken ( lineInfo.Tokens.First ().Token ),
					lineInfo.Tokens.ElementAt ( 1 ).Token, lineInfo.Tokens.Last ().Token );

			// quite brittle this one here...
			// replace with extension such as ElementAfterToken(xxx) or something like that.
			var _args = lineInfo.Tokens.ElementAt ( 5 ).Token.Split ( new char [] { ',' }, StringSplitOptions.RemoveEmptyEntries );
			foreach ( var _a in _args )
			{
				var _argPair = _a.Split ( new char [] { ':' }, StringSplitOptions.RemoveEmptyEntries );
				_ra.AddArgument ( _argPair.First ().Trim ().MultipleReplace ( Grammar.ValidQuoteSymbols ), _argPair.Last ().Trim ().ReplaceFirstAndLastOnly ( "\"" ) );
			}
			return _ra;
		}


		// ---------------------------------------------------------------------------------

		private string __lastTestGenCommandFound = "";

		public void CheckForTestGenInstruction ( string line )
		{
			if ( !string.IsNullOrWhiteSpace ( line ) && line.ToUpperInvariant ().Matches ( new Regex ( "^[\t\\s]*;[\t\\s]*TESTGEN" ) ) )
			{
				__lastTestGenCommandFound = line;
			}
		}


		// ---------------------------------------------------------------------------------


		public void AddLineToCurrentEntityRepresentation ( string line )
		{
			if ( !string.IsNullOrWhiteSpace ( line ) &&
				 this._entities.Count () > 0 &&
				 !line.ToUpperInvariant ().Matches ( new Regex ( "^[\t\\s]*;[\t\\s]*TESTGEN" ) ) &&
				 !line.Trim ().StartsWith ( Grammar.EntitySymbol.Token, StringComparison.InvariantCultureIgnoreCase ) )
			{
				this._entities.Last ().TextRepresentation += line;
				this._entities.Last ().TextRepresentation += Environment.NewLine;
			}
		}

		// ---------------------------------------------------------------------------------


		private void AddUnaryAction ( UnaryAction _ua, bool addAsElse = false )
		{
			if ( !addAsElse )
				this._entities.Last ().Rules.Last ().Conditions.Last ().AddUnaryAction ( _ua );
			else
			{
				this._entities.Last ().Rules.Last ().HasElseClause = true;
				this._entities.Last ().Rules.Last ().Conditions.Last ().AddUnaryAction ( _ua, true );
			}
		}


		// ---------------------------------------------------------------------------------


		private void AddResultAction ( ResultAction _ra, bool addAsElse = false )
		{
			if ( !addAsElse )
				this._entities.Last ().Rules.Last ().Conditions.Last ().AddResultAction ( _ra );
			else
			{
				this._entities.Last ().Rules.Last ().HasElseClause = true;
				this._entities.Last ().Rules.Last ().Conditions.Last ().AddResultAction ( _ra, true );
			}

		}


		// ---------------------------------------------------------------------------------


		private static IEnumerable<Symbol> GetClauseAfterThen ( LineInfo lineInfo )
		{
			var _resClause = lineInfo.Tokens.TakeAfter ( x => x == Grammar.ThenSymbol );
			return _resClause;
		}


		// ---------------------------------------------------------------------------------


		private void AddTrueFalseAction ( LineInfo lineInfo, Symbol trueSymbol, Symbol falseSymbol, bool addToElseBranch = false )
		{
			var _booleanValue = lineInfo.Tokens.ContainsAny2 ( new List<Symbol> () { Grammar.TrueSymbol, Grammar.FalseSymbol } );
			var _indexOfThis = lineInfo.Tokens.ToList ().IndexOf ( Grammar.ThisSymbol );
			UnaryAction _ua = null;
			if ( _booleanValue.Item2 == Grammar.TrueSymbol )
				_ua = new UnaryAction ( trueSymbol,
					_indexOfThis >= 0 ? this.Entities.Last ().Name : lineInfo.Tokens.First ().Token );
			else
				_ua = new UnaryAction ( falseSymbol,
				_indexOfThis >= 0 ? this.Entities.Last ().Name : lineInfo.Tokens.First ().Token );

			this._entities.Last ().Rules.Last ().Conditions.Last ().AddUnaryAction ( _ua, addToElseBranch );
		}


		// ---------------------------------------------------------------------------------


		private static void CheckForStringTokens ( string line, LineInfo lineInfo )
		{
			if ( Grammar.LineWithQuotedStringAndNoOutsideBrackets.IsMatch ( line ) )
			{
				var first = lineInfo.Tokens.First ( u => u.Token.StartsWith ( "\"" ) || u.Token.StartsWith ( "'" ) );
				var last = lineInfo.Tokens.Last ( u => u.Token.EndsWith ( "\"" ) || u.Token.EndsWith ( "'" ) );

				var _tokenList = lineInfo.Tokens.ToList ();
				var _fused = lineInfo.Tokens.JoinTogetherBetween ( _tokenList.IndexOf ( first ), _tokenList.IndexOf ( last ) );

				List<Symbol> replacedLineInfo = new List<Symbol> ();
				replacedLineInfo.AddRange ( lineInfo.Tokens.Take ( _tokenList.IndexOf ( first ) ) );
				replacedLineInfo.Add ( _fused );
				if ( ( _tokenList.IndexOf ( last ) + 1 ) < lineInfo.Tokens.Count () )
					replacedLineInfo.AddRange ( lineInfo.Tokens.Skip ( _tokenList.IndexOf ( last ) + 1 ) );

				lineInfo.RemoveTokensFromIndex ( 0 );
				lineInfo.AddTokens ( replacedLineInfo );
			}
		}


		// ---------------------------------------------------------------------------------


		private void CheckIfVisibleAndEnabledDefaultsAreSet ()
		{
			if ( this._entities.Count > 0 )
			{
				// CheckIfVisibleAndEnabledDefaultsAreSet ();
				if ( !this._entities.Last ().Defaults.Any ( x => x.Token == Grammar.VisibleDefaultClause.Token ) )
				{
					var clause = Grammar.GetDefaultClauseByToken ( Grammar.VisibleDefaultClause.Token, false );
					clause.SetValue ( true );
					this._entities.Last ().AddDefaultClause ( clause );
				}
				if ( !this._entities.Last ().Defaults.Any ( x => x.Token == Grammar.EnabledDefaultClause.Token ) )
				{
					var clause = Grammar.GetDefaultClauseByToken ( Grammar.EnabledDefaultClause.Token, false );
					clause.SetValue ( true );
					this._entities.Last ().AddDefaultClause ( clause );
				}
			}
		}


		// ---------------------------------------------------------------------------------


		private void ProcessUnaryAction ( LineInfo lineInfo, Symbol symbol, bool addToElseBranch = false, bool addToConditionlessActions = false )
		{
			if ( lineInfo.Tokens.Contains ( symbol ) )
			{
				UnaryAction _ua = null;
				if ( lineInfo.Tokens.First () == symbol )
					_ua = new UnaryAction ( lineInfo.Tokens.First (),
						   lineInfo.Tokens.Last ().Token == "this" ? this.Entities.Last ().Name : lineInfo.Tokens.Last ().Token );
				else
					_ua = new UnaryAction ( lineInfo.Tokens.Last (),
						   lineInfo.Tokens.First ().Token == "this" ? this.Entities.First ().Name : lineInfo.Tokens.First ().Token );
				if ( !addToConditionlessActions )
					this._entities.Last ().Rules.Last ().Conditions.Last ().AddUnaryAction ( _ua, addToElseBranch );
				else
					this._entities.Last ().AddUnaryAction ( _ua );
			}
		}



		// ---------------------------------------------------------------------------------


		/// <summary>
		/// This method processes entity declarations found at the beginning
		/// of each entity block.
		/// Not meant to be used or leaked outside.
		/// </summary>
		/// <param name="_currLine">current global counter</param>
		/// <param name="_currentScope">marker for the current scope in parsing, which
		/// indicates what block the parser is parsing right now.</param>
		/// <param name="line">Current text line being parsed.</param>
		/// <param name="lineInfo">Object contained the tokens parsed</param>
		/// <returns></returns>
		private CurrentScope ProcessEntityBlock ( int _currLine, CurrentScope _currentScope, string line, LineInfo lineInfo )
		{
			if ( this._lineParser.IsAValidSentence ( lineInfo ) )
			{
				_currentScope = CurrentScope.NEW_ENTITY;

				if ( this._entities.Any ( x => x.Name == lineInfo.Tokens.ElementAt ( 1 ).Token ) )
					throw new DuplicateEntityFoundException ( String.Format ( Grammar.DuplicateEntityFoundExceptionDefaultTemplate,
						lineInfo.Tokens.ElementAt ( 1 ).Token, _currLine ) );

				if ( !( line.ToUpperInvariant ().Trim ().EndsWith ( "IS FORM" ) ) )
					this._entities.Add ( new Entity (
						lineInfo.Tokens.ElementAt ( 1 ).Token,
						lineInfo.Tokens.ElementAt ( 3 ).Token,
						lineInfo.Tokens.Last ().Token.Replace ( "\"", "" ).Replace ( "'", "" ) ) );               // last token in form's name where the entity belongs
				else
					this._entities.Add ( new Entity (
						lineInfo.Tokens.ElementAt ( 1 ).Token,
						lineInfo.Tokens.ElementAt ( 3 ).Token, "" ) );               // last token in form's name where the entity belongs

			}
			else
				// throw an exception if the regex validation fails 
				throw new InvalidEntityDeclarationException ( String.Format ( Grammar.InvalidEntityDeclarationExceptionMessageTemplate, _currLine + 1, line.Trim () ) );

			return _currentScope;
		}


		// ---------------------------------------------------------------------------------


		/// <summary>
		/// This method processes with clauses marking the beginning of each block
		/// of each entity block.
		/// Not meant to be used or leaked outside.
		/// </summary>
		/// <param name="_currLine">current global counter</param>
		/// <param name="_currentScope">marker for the current scope in parsing, which
		/// indicates what block the parser is parsing right now.</param>
		/// <param name="line">Current text line being parsed.</param>
		/// <param name="lineInfo">Object contained the tokens parsed</param>
		/// <returns></returns>
		private CurrentScope ProcessWithClauseStatement ( int _currLine, CurrentScope _currentScope, string line, LineInfo lineInfo )
		{
			// Clear the scope state
			_currentScope = CurrentScope.NO_SCOPE;
			if ( this._lineParser.IsAValidSentence ( lineInfo ) )
			{
				// check the scope of the with clause and change the state accordingly                            
				_currentScope = SetCurrentScope ( _currentScope, lineInfo );
			}
			else
				throw new InvalidWithClauseException ( String.Format
					( Grammar.InvalidWithClauseExceptionMessageTemplate, _currLine + 1, line.Trim () ) );
			return _currentScope;
		}


		// ---------------------------------------------------------------------------------


		/// <summary>
		/// Sets up the current scope according to the with clause found.
		/// </summary>
		/// <param name="_currentScope"></param>
		/// <param name="lineInfo"></param>
		/// <returns></returns>
		private CurrentScope SetCurrentScope ( CurrentScope _currentScope, LineInfo lineInfo )
		{
			var _withScope = lineInfo.Tokens.Last ().Token;
			if ( _withScope.Equals ( Grammar.DefaultsSymbol.Token ) )
				_currentScope = CurrentScope.DEFAULTS_BLOCK;
			if ( _withScope.Equals ( Grammar.ActionsSymbol.Token ) )
				_currentScope = CurrentScope.ACTIONS_BLOCK;
			if ( _withScope.Equals ( Grammar.RulesSymbol.Token ) )
				_currentScope = CurrentScope.RULES_BLOCK;
			if ( _withScope.Equals ( Grammar.ConstraintsSymbol.Token ) )
				_currentScope = CurrentScope.CONSTRAINTS_BLOCK;
			if ( _withScope.Equals ( Grammar.TriggersSymbol.Token ) )
				_currentScope = CurrentScope.TRIGGERS_BLOCK;
			return _currentScope;
		}


		// ---------------------------------------------------------------------------------


		private void CheckFileExists ( string filePath )
		{
			if ( String.IsNullOrWhiteSpace ( filePath ) )
				throw new ArgumentNullException ( filePath );

			if ( !File.Exists ( filePath ) )
				throw new FileNotFoundException ( "rule file not found" );
		}


		// ---------------------------------------------------------------------------------


		private DefaultClause ConfigureDefaultClause ( LineInfo lineInfo )
		{

			//lineInfo = ParseLine ( this._lineParser.TokenizeMultiplePartOperators ( lineInfo ) );
			//if ( this._lineParser.LineInfoContainsArgumentkeyValuePairs ( lineInfo ) )
			//{
			//    lineInfo = this._lineParser.TokenizeArgumentKeyValuePairs ( lineInfo );
			//}

			var clause = Grammar.GetDefaultClauseByToken ( lineInfo.Tokens.First ().Token, false );
			if ( clause != null )
			{
				// if we only have one token, it means we are dealing with a single-element instruction such as 'enabled'
				// in that case we assumed the instruction to always be true
				// TODO: further validation to be added here to ensure we accept only boolean values here
				if ( lineInfo.Tokens.Count () == 1 )
					clause.SetValue ( "true" );
				// if there are two tokens, it means there are no comments, and we're dealing with a default instruction+
				// such as width 100 or enabled true, and then we assume the last token is the value
				if ( lineInfo.Tokens.Count () == 2 )
					clause.SetValue ( lineInfo.Tokens.Last ().Token );
				// if there are more tokens, it's comments most likely, but let's keep this separate for the time being
				// altough it could be the same for this and the previous case just by using ElementAt(1)
				if ( lineInfo.Tokens.Count () > 2 )
				{
					clause.SetValue ( lineInfo.Tokens.ElementAt ( 1 ).Token );
					if ( lineInfo.HasSymbol ( Grammar.WithArgumentsSymbol ) )
					{
						clause.AddArgumentsFromString ( lineInfo.Tokens.ElementAt ( lineInfo.IndexOfSymbol ( Grammar.WithArgumentsSymbol ) + 1 ).Token );
					}

					if ( lineInfo.HasSymbol ( Grammar.WithOutputArgumentsSymbol ) )
					{
						clause.AddOutputArgumentsFromString ( lineInfo.Tokens.ElementAt ( lineInfo.IndexOfSymbol ( Grammar.WithOutputArgumentsSymbol ) + 1 ).Token );
					}
				}
			}
			return clause;
		}


		// ---------------------------------------------------------------------------------


		protected internal LineInfo ParseLine ( string line )
		{
			var lineInfo = this._lineParser.ParseLine ( line );
			_parsedLines.Add ( lineInfo );
			return lineInfo;
		}


		// ---------------------------------------------------------------------------------


		/// <summary>
		/// Main entry point for parsing a file.
		/// Allows a caller to send in a list of strings (standard text lines)
		/// containing all the entities and rules to be parsed by the engine.
		/// </summary>
		/// <param name="filePath">Array of lines to be parsed.</param>
		protected internal string CreateTestForEntities ( string filePath, string uniqueID )
		{
			var _fucktardos = this.ParseRuleSet ( filePath );
			var __localCopy = new Entity [ 10000 ];
			_fucktardos.ToList ().CopyTo ( __localCopy );
			this._entities.Clear ();
			var __tests = new List<string> ();
			foreach ( var _f in __localCopy.Where ( x => x != null ) )
				__tests.Add ( this.CreateTestForEntity ( _f.TextRepresentation ) );


			var __sb = new StringBuilder ();

			__sb.Append ( "using System;						".Trim () ).Append ( Environment.NewLine );
			__sb.Append ( "using System.Collections.Generic;	".Trim () ).Append ( Environment.NewLine );
			__sb.Append ( "using System.IO;					".Trim () ).Append ( Environment.NewLine );
			__sb.Append ( "using System.Linq;				".Trim () ).Append ( Environment.NewLine );
			__sb.Append ( "using System.Text;				".Trim () ).Append ( Environment.NewLine );
			__sb.Append ( "using System.Threading.Tasks;		".Trim () ).Append ( Environment.NewLine );
			__sb.Append ( "using BREadfruit.Exceptions;		".Trim () ).Append ( Environment.NewLine );
			__sb.Append ( "using NUnit.Framework;			".Trim () ).Append ( Environment.NewLine );
			__sb.Append ( "using BREadfruit.Helpers;			".Trim () ).Append ( Environment.NewLine );
			__sb.Append ( "using BREadfruit.Conditions;		".Trim () ).Append ( Environment.NewLine );

			__sb.Append ( "/* ************************************************************" ).Append ( Environment.NewLine );
			__sb.Append ( " *                                                            *" ).Append ( Environment.NewLine );
			__sb.AppendFormat ( " * AUTOGENERATED ON : {0}	", DateTime.UtcNow.ToString () ).Append ( Environment.NewLine );
			__sb.Append ( " *                                                            *" ).Append ( Environment.NewLine );
			__sb.Append ( "************************************************************* */" ).Append ( Environment.NewLine );

			__sb.Append ( "namespace BREadfruit.Tests.Autogenerated" ).Append ( Environment.NewLine );
			__sb.Append ( "{" ).Append ( Environment.NewLine );
			__sb.Append ( "\t[TestFixture]" ).Append ( Environment.NewLine );
			__sb.Append ( "\tpublic class " ).Append ( uniqueID ).Append ( Environment.NewLine );
			__sb.Append ( "\t{" ).Append ( Environment.NewLine );


			__sb.AppendAll ( __tests ).Append ( "\t}" ).Append ( Environment.NewLine ).Append ( "}" );

			return __sb.ToString ();


		}


		// ---------------------------------------------------------------------------------


		protected internal string CreateTestForEntity ( string entity )
		{
			var lines = entity.Split ( new [] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries );

			// first line should be TESTGEN declaration as a comment
			// but we search for it
			string __testgenLine = "";
			var __testgenLines = lines.FindAllMatching ( new Regex ( TestGenParameters.TestGenLine1Regex ) );
			if ( __testgenLines.HasItems () )
				__testgenLine = __testgenLines.First ().ToUpperInvariant ();
			else
				return __testgenLine;

			var e = this.ParseRuleSet ( lines ).Last ();

			var __testBody = new StringBuilder ();
			__testBody.Append ( "[Test]", 2 ).Append ( Environment.NewLine );
			__testBody.AppendFormat ( 2, "public void TestEntity_{0} ()", e.Name ).Append ( Environment.NewLine );
			__testBody.Append ( "{", 2 ).Append ( Environment.NewLine );
			__testBody.Append ( "var parser = new Parser ();", 3 ).Append ( Environment.NewLine );
			__testBody.AppendFormat ( 3, "parser.ParseRuleSetAsString ( @\"{0}\" ", entity.EscapeAllDoubleQuotes () ).Append ( Environment.NewLine );
			__testBody.Append ( ");", 3 ).Append ( Environment.NewLine );
			__testBody.Append ( "Assert.That ( parser.Entities.Count () == 1 );", 3 ).Append ( Environment.NewLine );
			__testBody.Append ( "var e = parser.Entities.First ();", 3 ).Append ( Environment.NewLine );
			__testBody.AppendFormat ( 3, "Assert.That ( e.Form == \"{0}\", \"Entity form should be '{0}' but is \" + e.Form );", e.Form ).Append ( Environment.NewLine );
			__testBody.AppendFormat ( 3, "Assert.That ( e.Name == \"{0}\", \"Entity name should be '{0}' but is \" + e.Name );", e.Name ).Append ( Environment.NewLine );

			string __numOfDefaults = "0";
			string __numOfRules = "0";
			string __numOfActions = "0";
			string __numOfTriggers = "0";
			string __numOfConstraints = "0";

			var _1 = __testgenLine.FindAllMatching ( new Regex ( "DEFAULTS[\t\\s]*=[\t\\s]*[0-9]+" ) );
			if ( _1.HasItems () )
				__numOfDefaults = _1.First ().Split ( new [] { '=' }, StringSplitOptions.RemoveEmptyEntries ) [ 1 ].Trim ();

			var _2 = __testgenLine.FindAllMatching ( new Regex ( "RULES[\t\\s]*=[\t\\s]*[0-9]+" ) );
			if ( _2.HasItems () )
				__numOfRules = _2.First ().Split ( new [] { '=' }, StringSplitOptions.RemoveEmptyEntries ) [ 1 ].Trim ();

			var _3 = __testgenLine.FindAllMatching ( new Regex ( "ACTIONS[\t\\s]*=[\t\\s]*[0-9]+" ) );
			if ( _3.HasItems () )
				__numOfActions = _3.First ().Split ( new [] { '=' }, StringSplitOptions.RemoveEmptyEntries ) [ 1 ].Trim ();

			var _4 = __testgenLine.FindAllMatching ( new Regex ( "TRIGGERS[\t\\s]*=[\t\\s]*[0-9]+" ) );
			if ( _4.HasItems () )
				__numOfTriggers = _4.First ().Split ( new [] { '=' }, StringSplitOptions.RemoveEmptyEntries ) [ 1 ].Trim ();
#if !DISABLECONSTRAINTS
			var _5 = __testgenLine.FindAllMatching ( new Regex ( "CONSTRAINTS[\t\\s]*=[\t\\s]*[0-9]+" ) );
			if ( _5.HasItems () )
				__numOfConstraints = _5.First ().Split ( new [] { '=' }, StringSplitOptions.RemoveEmptyEntries ) [ 1 ].Trim ();
#endif
			__testBody.AppendFormat ( 3, "Assert.That ( e.Defaults.Count() == {0}, \"Should have '{0}' default clauses but has \" + e.Defaults.Count());", __numOfDefaults ).Append ( Environment.NewLine );
			__testBody.AppendFormat ( 3, "Assert.That ( e.ConditionlessActions.Count() == {0}, \"Should have '{0}' Conditionless Actions but has \" + e.ConditionlessActions.Count());", __numOfActions ).Append ( Environment.NewLine );
			__testBody.AppendFormat ( 3, "Assert.That ( e.Rules.Count() == {0}, \"Should have '{0}' Rules but has \" + e.Rules.Count());", __numOfRules ).Append ( Environment.NewLine );
			__testBody.AppendFormat ( 3, "Assert.That ( e.Triggers.Count() == {0}, \"Should have '{0}' Triggers but has \" + e.Triggers.Count());", __numOfTriggers ).Append ( Environment.NewLine );
#if !DISABLECONSTRAINTS
			__testBody.AppendFormat ( 3, "Assert.That ( e.Constraints.Count() == {0}, \"Should have '{0}' Constraints but has \" + e.Constraints.Count());", __numOfConstraints ).Append ( Environment.NewLine );
#endif

			__testBody.Append ( "}", 2 ).Append ( Environment.NewLine );
			__testBody.Append ( Environment.NewLine ).Append ( Environment.NewLine );
			__testBody.Append ( "// ---------------------------------------------------------------------------------", 2 );
			__testBody.Append ( Environment.NewLine ).Append ( Environment.NewLine );

			return __testBody.ToString ();
		}


		// ---------------------------------------------------------------------------------

	}



	enum CurrentScope
	{
		/// <summary>
		/// Empty scope
		/// </summary>
		NO_SCOPE,
		/// <summary>
		/// ENtity node
		/// </summary>
		NEW_ENTITY,
		/// <summary>
		/// Signals we're in the defaults block for a business rule definition
		/// </summary>
		DEFAULTS_BLOCK,
		/// <summary>
		/// Signals we're in the constraints block for a business rule definition
		/// </summary>
		CONSTRAINTS_BLOCK,
		/// <summary>
		/// Indicates we are in the constraints block of a business rule definition
		/// </summary>
		TRIGGERS_BLOCK,
		/// <summary>
		/// Indicates we are in the rules block of a business rule definition
		/// </summary>
		RULES_BLOCK,
		/// <summary>
		/// Indicates we are in the rules' else block of a business rule definition
		/// </summary>
		CONDITION_ACTIONS_ELSE_BLOCK,
		/// <summary>
		/// Indicates we are in the block that indicates condition-less actions
		/// </summary>
		ACTIONS_BLOCK,
		/// <summary>
		/// Indicates we are in the actions block of a business rule definition
		/// </summary>
		CONDITION_ACTIONS_BLOCK
	}

}
