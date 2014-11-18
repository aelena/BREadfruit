using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using BREadfruit.Clauses;
using BREadfruit.Conditions;
using BREadfruit.Helpers;

namespace BREadfruit
{
	/// <summary>
	/// Main class that takes care of parsing lines from the 
	/// business rule file.
	/// </summary>
	internal class LineParser
	{

		/// <summary>
		/// Parses a string representation of a line as read from the file.
		/// </summary>
		/// <param name="line">String containing all the line information.</param>
		/// <returns>Returns a configured instance of LineInfo.</returns>
		public LineInfo ParseLine ( string line )
		{
			// check the indentation leven in the string representation
			var indentLevel = GetIndentCount ( line );
			// split the line in spaces; replace any tabs just in case the user
			// put tabs in between tokens
			var _tokens = ExtractTokens ( line );
			// create the LineInfo instance, passing the original string represnetation, the tokens found
			// and preserving the indentation level
			var li = new LineInfo ( line, indentLevel, _tokens );

			return li;
		}


		// ---------------------------------------------------------------------------------


		/// <summary>
		/// Returns the number of tabs in the string passed as argument.
		/// </summary>
		/// <param name="line"></param>
		/// <returns></returns>
		protected internal int GetIndentCount ( string line )
		{
			if ( line != null )
				return line.Where ( x => x == '\t' ).Count ();
			return 0;
		}


		// ---------------------------------------------------------------------------------

		protected internal IEnumerable<Symbol> ExtractTokens ( string line )
		{
			if ( line != null )
			{
				// this poses the details of the last token being really the nontermoinal or not....
				// maybe this need some more careful thinking
				var _tokens = from t in line.Replace ( "\t", " " ).Split ( new [] { " " },
								 StringSplitOptions.RemoveEmptyEntries )
							  let grammarToken = Grammar.GetSymbolByToken ( t, false )
							  select
								 grammarToken == null ?
									  new Symbol ( t, line.ToCharArray ().Where ( x => x == '\t' ).Count (), false )
									  : grammarToken;

				// parse comments out
				// there are no multiline or inline comments but there can be trailing comments at the end of the line
				_tokens = _tokens.TakeWhile ( z => !z.Token.StartsWith ( ";" ) );

				// now we need to detect and unify any tokens in between single or double quotes as one token
				// so first let's just see if there are any sort of quotes in the line itself
				// no quote should actually be fount at 0...
				if ( line.IndexOfAny ( new char [] { '"', '\'' } ) >= 0 )
				{
					var _t1 = Enumerable.Range ( 0, _tokens.Count () ).Where ( x => _tokens.ElementAt ( x ).Token.StartsWith ( "'" ) || _tokens.ElementAt ( x ).Token.StartsWith ( "\"" ) );
					var _t2 = Enumerable.Range ( 0, _tokens.Count () ).Where ( x => _tokens.ElementAt ( x ).Token.EndsWith ( "'" ) || _tokens.ElementAt ( x ).Token.StartsWith ( "\"" ) );

					// count should be the same
					if ( _t1.Count () == _t2.Count () )
					{
						var _fusedSymbols = new List<Symbol> ();
						// now "fuse" the symbols that are within quotes
						// TODO: as this is valid for strings (user strings) any keywords here are considered just string
						for ( var i = 0; i < _t1.Count (); i++ )
							_fusedSymbols.Add ( _tokens.ToList ().GetRange ( _t1.ElementAt ( i ), ( _t2.ElementAt ( i ) - _t1.ElementAt ( i ) ) + 1 ).JoinTogether () );

						List<Symbol> replacedLineInfo = new List<Symbol> ();
						for ( int i = 0, j = 0; i < _tokens.Count (); i++ )
						{
							if ( _t1.Contains ( i ) )
							{
								replacedLineInfo.Add ( _fusedSymbols.ElementAt ( j ) );
								i = _t2.ElementAt ( j );    // move the index
								j++;
							}
							else
								replacedLineInfo.Add ( _tokens.ElementAt ( i ) );
						}

						return replacedLineInfo;
					}
					else
					{
						throw new Exception ( String.Format ( "There seems to be a mismatch in the usage of quotes in line {0}", line ) );
					}
				}

				return _tokens;
			}
			return null;
		}

		/// <summary>
		/// get tokens from string 
		/// if the token exists in the grammar, then it adds the grammar-declared token
		/// otherwise, as in the case of literals or other symbols,
		/// instantiates a new Symbol and adds it to the collection.
		/// </summary>
		/// <param name="line"></param>
		/// <returns></returns>
		//protected internal IEnumerable<Symbol> ExtractTokens ( string line )
		//{
		//    if ( line != null )
		//    {
		//        // this poses the details of the last token being really the nontermoinal or not....
		//        // maybe this need some more careful thinking
		//        var _tokens = from t in line.Replace ( "\t", " " ).Split ( new [] { " " },
		//                         StringSplitOptions.RemoveEmptyEntries )
		//                      let grammarToken = Grammar.GetSymbolByToken ( t, false )
		//                      select
		//                         grammarToken == null ?
		//                              new Symbol ( t, line.ToCharArray ().Where ( x => x == '\t' ).Count (), false )
		//                              : grammarToken;

		//        // parse comments out
		//        // there are no multiline or inline comments but there can be trailing comments at the end of the line
		//        _tokens = _tokens.TakeWhile ( z => !z.Token.StartsWith ( ";" ) );

		//        // now we need to detect and unify any tokens in between single or double quotes as one token
		//        // so first let's just see if there are any sort of quotes in the line itself
		//        // no quote should actually be fount at 0...
		//        if ( _tokens.Count () > 0 && line.IndexOfAny ( new char [] { '"', '\'' } ) >= 0 )
		//        {


		//            if ( Grammar.LineWithQuotedStringAndNoOutsideBrackets.IsMatch ( line ) )
		//            {
		//                var first = _tokens.First ( u => u.Token.StartsWith ( "\"" ) || u.Token.StartsWith ( "'" ) );
		//                var last = _tokens.Last ( u => u.Token.EndsWith ( "\"" ) || u.Token.EndsWith ( "'" ) );

		//                var _tokenList = _tokens.ToList ();
		//                var _fused = _tokens.JoinTogetherBetween ( _tokenList.IndexOf ( first ), _tokenList.IndexOf ( last ) );

		//                List<Symbol> replacedLineInfo = new List<Symbol> ();
		//                replacedLineInfo.AddRange ( _tokens.Take ( _tokenList.IndexOf ( first ) ) );
		//                replacedLineInfo.Add ( _fused );
		//                if ( ( _tokenList.IndexOf ( last ) + 1 ) < _tokens.Count () )
		//                    replacedLineInfo.AddRange ( _tokens.Skip ( _tokenList.IndexOf ( last ) + 1 ) );

		//                return replacedLineInfo;
		//            }

		//            //var _t1 = Enumerable.Range ( 0, _tokens.Count () ).Where ( x => _tokens.ElementAt ( x ).Token.StartsWith ( "'" ) || _tokens.ElementAt ( x ).Token.StartsWith ( "\"" ) );
		//            //var _t2 = Enumerable.Range ( 0, _tokens.Count () ).Where ( x => _tokens.ElementAt ( x ).Token.EndsWith ( "'" ) || _tokens.ElementAt ( x ).Token.StartsWith ( "\"" ) );

		//            //if ( _t1.Count () == _t2.Count () )
		//            //{

		//            //}
		//            // count should be the same
		//            //if ( _t1.Count () == _t2.Count () )
		//            //{
		//            //    var _fusedSymbols = new List<Symbol> ();
		//            //    // now "fuse" the symbols that are within quotes
		//            //    // TODO: as this is valid for strings (user strings) any keywords here are considered just string
		//            //    for ( var i = 0; i < _t1.Count (); i++ )
		//            //        _fusedSymbols.Add ( _tokens.ToList ().GetRange ( _t1.ElementAt ( i ), ( _t2.ElementAt ( i ) + 1 - _t1.ElementAt ( i ) ) + 1 ).JoinTogether () );

		//            //    List<Symbol> replacedLineInfo = new List<Symbol> ();
		//            //    for ( int i = 0, j = 0; i < _tokens.Count (); i++ )
		//            //    {
		//            //        if ( _t1.Contains ( i ) )
		//            //        {
		//            //            replacedLineInfo.Add ( _fusedSymbols.ElementAt ( j ) );
		//            //            i = _t2.ElementAt ( j );    // move the index
		//            //            j++;
		//            //        }
		//            //        else
		//            //            replacedLineInfo.Add ( _tokens.ElementAt ( i ) );
		//            //    }

		//            //    return replacedLineInfo;
		//            //}
		//            //else
		//            //{
		//            //    throw new Exception ( String.Format ( "There seems to be a mismatch in the usage of quotes in line {0}", line ) );
		//            //}
		//        }

		//        return _tokens;
		//    }
		//    return null;
		//}


		// ---------------------------------------------------------------------------------


		/// <summary>
		/// Takes care of validating lines found in the file for business rules.
		/// </summary>
		/// <param name="line"></param>
		/// <returns></returns>
		public bool IsAValidSentence ( LineInfo line )
		{
			if ( line.Tokens != null && line.Tokens.Count () > 0 )
			{

				// extract this hardcoded identifier
				if ( line.Tokens.First ().Token.ToUpperInvariant ().Equals ( "ENTITY" ) )
				{
					return this.ValidateEntityStatement ( line );
				}
				if ( line.Tokens.First ().Token.ToUpperInvariant ().Equals ( "WITH" ) )
				{
					return this.ValidateWithStatement ( line );
				}
				if ( line.Tokens.First ().Token.ToUpperInvariant ().Equals ( Grammar.ChangeFormUnaryActionSymbol.Token.ToUpperInvariant () ) )
				{
					if ( !line.HasSymbol ( Grammar.WithArgumentsSymbol ) )
						return Regex.IsMatch ( line.Representation.Trim (), Grammar.ChangeFormNoArgumentsLineRegex, RegexOptions.IgnoreCase );
					else
					{
						// parse separately the beginning and the with arguments clause
						var _firstPart = new LineInfo ( line.TakeUntil ( Grammar.WithArgumentsSymbol ).JoinTogether ().Token );
						var _secondPart = new LineInfo ( line.TakeFrom ( Grammar.WithArgumentsSymbol ).JoinTogether ().Token );

						return Regex.IsMatch ( line.TakeUntil ( Grammar.WithArgumentsSymbol, false ).JoinTogether ().Token, Grammar.ChangeFormNoArgumentsLineRegex, RegexOptions.IgnoreCase ) &&
							   Regex.IsMatch ( line.TakeFrom ( Grammar.WithArgumentsSymbol, true ).JoinTogether ().Token, Grammar.WithArgumentsClauseLineRegex, RegexOptions.IgnoreCase );

					}
				}

				if ( line.Tokens.First ().Token.ToUpperInvariant ().Equals ( Grammar.HideUnaryActionSymbol.Token.ToUpperInvariant () ) )
				{
					return Regex.IsMatch ( line.Representation.Trim (), Grammar.HideElementLineRegex, RegexOptions.IgnoreCase );
				}

				if ( line.Tokens.First ().Token.ToUpperInvariant ().Equals ( Grammar.LoadDataSymbol.Token.ToUpperInvariant () ) )
				{
					return Regex.IsMatch ( line.Representation.Trim (), Grammar.LoadDataFromLineRegex, RegexOptions.IgnoreCase );
				}

				if ( line.Tokens.First ().Token.ToUpperInvariant ().Equals ( Grammar.ClearValueUnaryActionSymbol.Token.ToUpperInvariant () ) )
				{
					return Regex.IsMatch ( line.Representation.Trim (), Grammar.ClearValueLineRegex, RegexOptions.IgnoreCase );
				}
			}

			return false;
		}


		// ---------------------------------------------------------------------------------


		/// <summary>
		/// This function detects keywords or operators that have multi-word tokens or aliases
		/// (such as "load data from" which is actually a single instruction)
		/// and performs a substitution to convert them into single-word tokens 
		/// according to the Grammar specs. This means that in effect, the number of token
		/// is generally reduced here or remains the same if no multi-word tokens or aliases
		/// are found in the LineInfo instance passed as argument.
		/// </summary>
		/// <param name="li"></param>
		/// <returns></returns>
		public string TokenizeMultiplePartOperators ( LineInfo li /*IEnumerable<Symbol> tokens, string line*/ )
		{

			CheckForValueListsInCondition ( li );

			// take a repreeentation of the entire list of tokens as a simple string
			var _fused = li.Tokens.JoinTogether ().Token;
			var _replacedString = _fused;


			// now check for occurrence
			var _x = from op in Grammar.Symbols
					 where op is Symbol
					 || op is ResultAction
					 || op is DefaultClause
					 || op is Operator
					 || op is UnaryAction
					 && op.Aliases.Count () > 0
					 let t = _fused.ContainsAny2 ( op.Aliases, true )
					 where t.Item1
					 orderby op.Token.Length descending
					 select new
					 {
						 symbol = op,
						 Ocurrence = t.Item2,
						 Index = _fused.IndexOf ( t.Item2 ),
						 Length = t.Item2.Length
					 };


			if ( _x != null && _x.Count () > 0 )
				_x.ToList ().ForEach ( x => _replacedString = _replacedString.ReplaceWord ( x.Ocurrence, x.symbol.Token ) );

			_fused = _replacedString;


			_replacedString = _replacedString.Prepend ( "\t", li.IndentLevel );

			li = ParseLine ( _replacedString.RemoveBetween ( " ", "{", "}" ) );

			var _ = li.Representation;
			return li.Representation;
		}


		// ---------------------------------------------------------------------------------


		private static void CheckForValueListsInCondition ( LineInfo li )
		{
			var _conditionValueList = li.TokenizeValueListInCondition ();
			if ( _conditionValueList != null )
			{
				li.RemoveTokensFromTo ( _conditionValueList.Item2, _conditionValueList.Item3 + 1 );
				li.InsertTokenAt ( _conditionValueList.Item2, new Symbol ( _conditionValueList.Item1, 2 ) );
			}
		}

		// ---------------------------------------------------------------------------------


		#region " --- specific line validations --- "


		// ---------------------------------------------------------------------------------


		internal bool ValidateEntityStatement ( LineInfo line )
		{
			return this.ValidateEntityStatement ( line.Representation.Trim ().ToUpperInvariant () );
		}


		// ---------------------------------------------------------------------------------


		internal bool ValidateEntityStatement ( string line )
		{
			var _entityLineRegex = new Regex ( Grammar.EntityLineRegex );
			return _entityLineRegex.IsMatch ( line.ToUpperInvariant () );
		}


		// ---------------------------------------------------------------------------------

		private bool ValidateWithStatement ( LineInfo line )
		{
			var r = new Regex ( Grammar.WithLineRegex, RegexOptions.IgnoreCase );
			if ( !r.IsMatch ( line.Representation ) )
				return false;
			if ( line.IndentLevel != Grammar.WithSymbol.IndentLevel )
				return false;
			// verify the only child token in a 'with' statement is correct
			if ( !Grammar.WithSymbol.Children.Contains ( line.Tokens.ElementAt ( 1 ) ) )
				return false;

			return true;
		}


		// ---------------------------------------------------------------------------------


		#endregion


		// ---------------------------------------------------------------------------------


		public IEnumerable<Condition> ExtractConditions ( LineInfo lineInfo, string entityName = null )
		{
			var _conds = new List<Condition> ();
			int __ands = 0, __ors = 0;
			if ( lineInfo.Tokens.Contains ( Grammar.ANDSymbol ) )
				__ands = lineInfo.Tokens.Count ( x => x == Grammar.ANDSymbol );
			if ( lineInfo.Tokens.Contains ( Grammar.ORSymbol ) )
				__ors = lineInfo.Tokens.Count ( x => x == Grammar.ORSymbol );
			__ands += __ors;

			if ( __ands >= 1 )
				__ands++;

			for ( int i = 0; i <= 3 * __ands; )
			{
				if ( i >= lineInfo.Tokens.Count() )
					break;
				var _operandToken = lineInfo.Tokens.ElementAt ( i ).Token;
				if ( _operandToken.Trim ().ToLowerInvariant ().StartsWith ( "this." ) && entityName != null )
					_operandToken = _operandToken.Replace ( "this", entityName );

				var _c = new Condition ( _operandToken,
										 Grammar.GetOperator ( lineInfo.Tokens.ElementAt ( ++i ).Token ),
										 lineInfo.Tokens.ElementAt ( ++i ).Token );

				// need to purge in the case of unary operator conditions
				if ( _c.Operator.In ( new [] { Grammar.IsEmptyOperator, Grammar.IsMandatoryOperator, Grammar.IsMandatoryOperator, Grammar.IsNotMandatoryOperator } ) )
					_c.ChangeValue ( "" );

				if ( _c.Operator.In ( Grammar.UnaryOperators ) )
					i -= 2;

				if ( _c.Operator == Grammar.IsEmptyOperator )
					_c = new Condition ( lineInfo.Tokens.ElementAt ( i ).Token,
						Grammar.EqualityOperator, Grammar.EmptyStringSymbol.Token );

				if ( _c.Operator == Grammar.IsNotEmptyOperator )
					_c = new Condition ( lineInfo.Tokens.ElementAt ( i ).Token,
						Grammar.NonEqualityOperator, Grammar.EmptyStringSymbol.Token );

				if ( _c.Operator == Grammar.IsNullOperator )
					_c = new Condition ( lineInfo.Tokens.ElementAt ( i ).Token,
						Grammar.EqualityOperator, Grammar.NullValueSymbol.Token );

				if ( _c.Operator == Grammar.IsNotNullOperator )
					_c = new Condition ( lineInfo.Tokens.ElementAt ( i ).Token,
						Grammar.NonEqualityOperator, Grammar.NullValueSymbol.Token );

				if ( _c.Operator.In ( Grammar.UnaryOperators ) )
					i += 1;

				_conds.Add ( _c );

				var _logical = lineInfo.Tokens.ElementAtOrDefault ( ++i );
				if ( _logical != null )
				{
					if ( new List<string> () { Grammar.ANDSymbol.Token, Grammar.ORSymbol.Token }.Contains ( _logical.Token ) )
					{
						_conds.Last ().SetLogicalOperator ( _logical.Token );
					}
				}
				i += 1;
			}


			return _conds;
		}


		// ---------------------------------------------------------------------------------


		/// <summary>
		/// This function parses a lineInfo to detect argument key value pairs 
		/// and make a single token of all the individual tokens.
		/// This token is to be parsed later on for extracting the individual key valuye pairs.
		/// </summary>
		/// <param name="lineInfo"></param>
		/// <returns></returns>
		protected internal LineInfo TokenizeArgumentKeyValuePairs ( LineInfo lineInfo )
		{
			var _argsAsString = lineInfo.GetArgumentsAsString ();
			// check for tokens that might be after the token ending with }
			var _trailingTokensToBePreserved = lineInfo.Tokens.TakeAfter ( x => x.Token.EndsWith ( "}" ) );
			if ( !String.IsNullOrWhiteSpace ( _argsAsString ) )
			{
				lineInfo.RemoveTokensAfterSymbol ( Grammar.WithArgumentsSymbol, true );
				lineInfo.AddToken ( new Symbol ( _argsAsString, 2, true ) );
			}
			// if there were such tokens, preserve them again in the new instance
			if ( _trailingTokensToBePreserved != null )
				lineInfo.AddTokens ( _trailingTokensToBePreserved );
			return lineInfo;
		}


		// ---------------------------------------------------------------------------------


		protected internal LineInfo TokenizeBracketExpression ( LineInfo lineInfo )
		{

			if ( lineInfo.Representation.Contains ( new [] { Grammar.OpeningSquareBracket.Token, Grammar.ClosingSquareBracket.Token } ) )
			{
				var _argsAsString = lineInfo.Representation.TakeBetween ( Grammar.OpeningSquareBracket.Token, Grammar.ClosingSquareBracket.Token, trimResults: true );
				// check for tokens that might be after the token ending with }
				var _trailingTokensToBePreserved = lineInfo.Tokens.TakeAfter ( x => x.Token.EndsWith ( Grammar.ClosingSquareBracket.Token ) );
				if ( !String.IsNullOrWhiteSpace ( _argsAsString ) )
				{
					lineInfo.RemoveTokensFrom ( x => x.Token.StartsWith ( Grammar.OpeningSquareBracket.Token ) );
					lineInfo.AddToken ( new Symbol ( _argsAsString, 2, true ) );
				}
				// if there were such tokens, preserve them again in the new instance
				if ( _trailingTokensToBePreserved != null )
					lineInfo.AddTokens ( _trailingTokensToBePreserved );
			}
			return lineInfo;
		}


		// ---------------------------------------------------------------------------------


		/// <summary>
		/// Returns a value that indicates if the passed LineInfo instance contains
		/// a "with arguments" clause.
		/// </summary>
		/// <param name="lineInfo">LineInfo instance to inspect.</param>
		/// <returns></returns>
		protected internal bool LineInfoContainsArgumentkeyValuePairs ( LineInfo lineInfo )
		{
			return lineInfo.HasSymbol ( Grammar.WithArgumentsSymbol );
		}


		// ---------------------------------------------------------------------------------

	}
}
