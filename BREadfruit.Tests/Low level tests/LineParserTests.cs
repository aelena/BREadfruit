﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using BREadfruit.Helpers;
using BREadfruit.Conditions;
using BREadfruit.Clauses;

namespace BREadfruit.Tests.Low_level_tests
{
	[TestFixture]
	public class LineParserTests
	{

		LineParser lineParser = new LineParser ();


		/// <summary>
		/// Performs a series of tests to ensure that the tokens
		/// in different topologies of lines in the document format
		/// are parsed and extracted correctly, including
		/// 
		/// - detecting comments and ignoring them
		/// - detecting items in single or double quotes that are to be interpreted
		///   as single tokens, not as separate tokens 
		/// 
		/// </summary>
		/// <param name="line"></param>
		/// <param name="correctCount"></param>
		/// <param name="lastToken"></param>
		/// <returns></returns>
		[TestCase ( "max_length 10", 2, "10", Result = true )]
		[TestCase ( "max_length 11", 2, "10", Result = false )]
		[TestCase ( "max_length 11 0", 2, "10", Result = false )]
		[TestCase ( "max_length 10 ; this is a comment", 2, "10", Result = true )]
		[TestCase ( "max_length 12 ; this is a comment", 2, "10", Result = false )]
		[TestCase ( "mandatory true", 2, "true", Result = true )]
		[TestCase ( "mandatory", 1, "mandatory", Result = true )]
		[TestCase ( "mandatory ; this is a comment", 1, "mandatory", Result = true )]
		[TestCase ( "Entity XYZ is TextBox", 4, "TextBox", Result = true )]
		[TestCase ( "Entity XYZ is TextBox ; and this is a comment", 4, "TextBox", Result = true )]
		public bool ShouldExtractTokensCorrectly ( string line, int correctCount, string lastToken )
		{
			var _t = lineParser.ExtractTokens ( line );
			return ( _t.Count () == correctCount ) && ( _t.Last ().Token == lastToken );
		}


		// ---------------------------------------------------------------------------------


		[TestCase ( "", "'", 0, "", false, Result = true )]
		[TestCase ( "say 'hello'", "'", 2, "'hello'", true, Result = true )]
		[TestCase ( "say 'hello there'", "'", 2, "'hello there'", true, Result = true )]
		[TestCase ( "say 'hello there' boyo!", "'", 2, "'hello there'", true, Result = true )]
		[TestCase ( "say \"hello world\"", "\"", 2, "\"hello world\"", true, Result = true )]
		[TestCase ( "say \"hello there world\"", "\"", 2, "\"hello there world\"", true, Result = true )]
		[TestCase ( "say \"hello there, it's a big world\"", "\"", 2, "\"hello there, it's a big world\"", true, Result = true )]
		[TestCase ( "say \"hello there, it's a big world\" isn't it?", "\"", 2, "\"hello there, it's a big world\"", true, Result = true )]
		public bool ShouldConcatenateMultipleTokens ( string line, string matchingChar,
			int finalTokenCount, string fusedToken, bool shouldThereBeFusedTokens )
		{

			var _t = lineParser.ExtractTokens ( line ).ToList ();
			var _t1 = Enumerable.Range ( 0, _t.Count ).Where ( x => _t [ x ].Token.StartsWith ( matchingChar ) );
			var _t2 = Enumerable.Range ( 0, _t.Count ).Where ( x => _t [ x ].Token.EndsWith ( matchingChar ) );
			var _fusedSymbols = new List<Symbol> ();

			for ( var i = 0; i < _t1.Count (); i++ )
				_fusedSymbols.Add ( _t.GetRange ( _t1.ElementAt ( i ), ( _t2.ElementAt ( i ) - _t1.ElementAt ( i ) ) + 1 ).JoinTogether () );

			Assert.That ( _fusedSymbols.Count () > 0 == shouldThereBeFusedTokens );
			if ( shouldThereBeFusedTokens )
				Assert.That ( _fusedSymbols.First ().Token == fusedToken );

			return _t1.Count () == _t2.Count ();
		}


		// ---------------------------------------------------------------------------------


		[Test]
		public void ShouldConcatenateMultipleTokens_2 ()
		{
			var line = "when you 'see me' then say 'hello there, boy'";
			string matchingChar = "'";
			var _t = lineParser.ExtractTokens ( line ).ToList ();
			var _t1 = Enumerable.Range ( 0, _t.Count ).Where ( x => _t [ x ].Token.StartsWith ( matchingChar ) );
			var _t2 = Enumerable.Range ( 0, _t.Count ).Where ( x => _t [ x ].Token.EndsWith ( matchingChar ) );
			var _fusedSymbols = new List<Symbol> ();

			for ( var i = 0; i < _t1.Count (); i++ )
				_fusedSymbols.Add ( _t.GetRange ( _t1.ElementAt ( i ), ( _t2.ElementAt ( i ) - _t1.ElementAt ( i ) ) + 1 ).JoinTogether () );

			Assert.That ( _fusedSymbols.Count () == 2 );
			Assert.That ( _fusedSymbols.First ().Token == "'see me'" );
			Assert.That ( _fusedSymbols.Last ().Token == "'hello there, boy'" );

		}


		// ---------------------------------------------------------------------------------


		[Test]
		public void ShouldConcatenateMultipleTokens_3 ()
		{
			var line = "when you \"see me\" then say \"hello there, boy\" or risk being ostracised at \"The Mad Hatter\" pub tonight";
			string matchingChar = "\"";
			var _t = lineParser.ExtractTokens ( line ).ToList ();
			var _t1 = Enumerable.Range ( 0, _t.Count ).Where ( x => _t [ x ].Token.StartsWith ( matchingChar ) );
			var _t2 = Enumerable.Range ( 0, _t.Count ).Where ( x => _t [ x ].Token.EndsWith ( matchingChar ) );
			var _fusedSymbols = new List<Symbol> ();

			for ( var i = 0; i < _t1.Count (); i++ )
				_fusedSymbols.Add ( _t.GetRange ( _t1.ElementAt ( i ), ( _t2.ElementAt ( i ) - _t1.ElementAt ( i ) ) + 1 ).JoinTogether () );

			Assert.That ( _fusedSymbols.Count () == 3 );
			Assert.That ( _fusedSymbols.First ().Token == "\"see me\"" );
			Assert.That ( _fusedSymbols.ElementAt ( 1 ).Token == "\"hello there, boy\"" );
			Assert.That ( _fusedSymbols.ElementAt ( 2 ).Token == "\"The Mad Hatter\"" );

			List<Symbol> replacedLineInfo = new List<Symbol> ();
			for ( int i = 0, j = 0; i < _t.Count (); i++ )
			{
				if ( _t1.Contains ( i ) )
				{
					replacedLineInfo.Add ( _fusedSymbols.ElementAt ( j ) );
					i = _t2.ElementAt ( j );    // move the index
					j++;
				}
				else
					replacedLineInfo.Add ( _t [ i ] );

			}

			Assert.That ( replacedLineInfo.Count () == 14 );

		}

		// ---------------------------------------------------------------------------------


		[TestCase ( "this starts with 0 then disabled", Result = "this starts_with 0 then disable" )]
		[TestCase ( "this starts with 0 and that ends with 0 then disabled", Result = "this starts_with 0 and that ends_with 0 then disable" )]
		[TestCase ( "enabled true", Result = "enable true" )]
		[TestCase ( "max_length 10", Result = "max_length 10" )]
		[TestCase ( "this starts with 0 then set visible", Result = "this starts_with 0 then visible" )]
		[TestCase ( "load data from DATASOURCE.TABLE_11", Result = "load_data_from DATASOURCE.TABLE_11" )]
		[TestCase ( "max length 10", Result = "max_length 10" )]
		//[TestCase ( "max 10", Result = "max_length 10" )]
		[TestCase ( "maximum length 10", Result = "max_length 10" )]
		//[TestCase ( "length 10", Result = "max_length 10" )]
		//[TestCase ( "min length 10", Result = "min_length 10" )]
		//[TestCase ( "min 10", Result = "min_length 10" )]
		[TestCase ( "minimum length 10", Result = "min_length 10" )]
		public string TokenizeMultiplePartOperatorTests ( string line )
		{
			var li = lineParser.ParseLine ( line );
			var resolved = lineParser.TokenizeMultiplePartOperators ( li );
			return resolved;
		}


		// ---------------------------------------------------------------------------------


		[TestCase ( "role is 'ENQ' then enabled", "role is 'ENQ'", Result = 1 )]
		[TestCase ( "role is {UPM,CPM} then not enabled", "role is {UPM,CPM}", Result = 1 )]
		public int ExtractConditionsTests_1 ( string line, string expected )
		{
			var lineInfo = lineParser.ParseLine ( line );
			lineInfo = lineParser.ParseLine ( lineParser.TokenizeMultiplePartOperators ( lineInfo ) );
			var _conds = lineParser.ExtractConditions ( lineInfo, "" );
			Assert.That ( _conds.First ().ToString ().Equals ( expected ) );
			Assert.That ( _conds.First ().SuffixLogicalOperator == null );
			return _conds.Count ();
		}


		// ---------------------------------------------------------------------------------


		[TestCase ( "role is 'ENQ' and role is {UPM,CPM} then enabled", "role is 'ENQ'", "role is {UPM,CPM}", Result = 2 )]
		[TestCase ( "VendorCountry is ES and TaxCode1 starts with {P,Q,S} then", "VendorCountry is ES", "TaxCode1 starts_with {P,Q,S}", Result = 2 )]
		public int ExtractConditionsTests_2 ( string line, string expected1, string expected2 )
		{
			var lineInfo = lineParser.ParseLine ( line );
			lineInfo = lineParser.ParseLine ( lineParser.TokenizeMultiplePartOperators ( lineInfo ) );
			var _conds = lineParser.ExtractConditions ( lineInfo );
			Assert.That ( _conds.First ().ToString ().Equals ( expected1 ) );
			Assert.That ( _conds.First ().SuffixLogicalOperator == "and" );
			Assert.That ( _conds.Last ().ToString ().Equals ( expected2 ) );
			Assert.That ( _conds.Last ().SuffixLogicalOperator == null );

			return _conds.Count ();
		}


		// ---------------------------------------------------------------------------------


		[TestCase ( "role is 'ENQ' and role is {UPM,CPM} or field is disabled then enabled",
			"role is 'ENQ'", "role is {UPM,CPM}", "field is disable", Result = 3 )]
		[TestCase ( "VendorCountry is ES and TaxCode1 starts with {P,Q,S} or he is hungry then",
			"VendorCountry is ES", "TaxCode1 starts_with {P,Q,S}", "he is hungry", Result = 3 )]
		public int ExtractConditionsTests_3 ( string line, string expected1, string expected2, string expected3 )
		{
			var lineInfo = lineParser.ParseLine ( line );
			lineInfo = lineParser.ParseLine ( lineParser.TokenizeMultiplePartOperators ( lineInfo ) );
			var _conds = lineParser.ExtractConditions ( lineInfo, "" );
			Assert.That ( _conds.First ().ToString ().Equals ( expected1 ) );
			Assert.That ( _conds.First ().SuffixLogicalOperator == "and" );
			Assert.That ( _conds.ElementAt ( 1 ).ToString ().Equals ( expected2 ) );
			Assert.That ( _conds.ElementAt ( 1 ).SuffixLogicalOperator == "or" );
			Assert.That ( _conds.Last ().ToString ().Equals ( expected3 ) );
			Assert.That ( _conds.Last ().SuffixLogicalOperator == null );
			return _conds.Count ();
		}


		// ---------------------------------------------------------------------------------


		[TestCase ( "DDLVDCountry.value not in {\"PT\",\"ES\",\"IT\",\"FR\",\"GF\",\"GP\",\"RE\",\"MQ\",\"YT\",\"NC\",\"PF\",\"PM\",\"WF\",\"MC\"} then" )]
		public void ShouldFindSymbol ( string _fused )
		{
			var _x = from op in Grammar.Symbols
					 where op is Symbol
					 || op is ResultAction
					 || op is DefaultClause
					 || op is Operator
					  && op.Aliases.Count () > 0
					 let t = _fused.ContainsAny2 ( op.Aliases )
					 where t.Item1
					 select new
					 {
						 symbol = op,
						 Ocurrence = t.Item2,
						 Index = _fused.IndexOf ( t.Item2 ),
						 Length = t.Item2.Length
					 };

			Assert.That ( _x.Where ( x => x.symbol.Token == "not_in" ).Count () == 1 );
		}


		// ---------------------------------------------------------------------------------


		[TestCase ( "load data from DATASOURCE.ENTERPRISE.WORLD_COUNTRIES with arguments {\"Country\":\"ES\",\"Active\":true, \"Age\":30, \"Title\":\"No reason to fear this\" }", Result = true )]
		[TestCase ( "load data from DATASOURCE.ENTERPRISE.WORLD_COUNTRIES", Result = false )]
		[TestCase ( "load data from DATASOURCE.ENTERPRISE.WORLD_COUNTRIES with arguments", Result = true )]
		public bool LineInfoContainsArgumentkeyValuePairsTests ( string line )
		{
			return lineParser.LineInfoContainsArgumentkeyValuePairs ( new LineInfo ( line ) );
		}


		// ---------------------------------------------------------------------------------


		[TestCase ( "load data from DATASOURCE.ENTERPRISE.WORLD_COUNTRIES with arguments {\"Country\":\"ES\",\"Active\":true, \"Age\":30, \"Title\":\"No reason to fear this\" }", Result = 4 )]
		[TestCase ( "load data from DATASOURCE.ENTERPRISE.WORLD_COUNTRIES with arguments {\"Country\":\"ES\"}", Result = 4 )]
		[TestCase ( "load data from DATASOURCE.ENTERPRISE.WORLD_COUNTRIES with arguments ", Result = 3 )]
		public int TokenizeArgumentArgumentkeyValuePairsTests ( string line )
		{
			var lineInfo = lineParser.ParseLine ( lineParser.TokenizeMultiplePartOperators ( new LineInfo ( line ) ) );
			var newLine = lineParser.TokenizeArgumentKeyValuePairs ( lineInfo );
			return newLine.Tokens.Count ();
		}


		// ---------------------------------------------------------------------------------


		[TestCase ( "load data from DATASOURCE.ENTERPRISE.WORLD_COUNTRIES with arguments [\"Country\":\"ES\",\"Active\":true, \"Age\":30, \"Title\":\"No reason to fear this\" ]", Result = 4 )]
		[TestCase ( "load data from DATASOURCE.ENTERPRISE.WORLD_COUNTRIES with arguments [\"Country\":\"ES\"]", Result = 4 )]
		[TestCase ( "load data from DATASOURCE.ENTERPRISE.WORLD_COUNTRIES with arguments ", Result = 3 )]
		[TestCase ( "			set value [LU + GD_Ctr_PIVA.Value] in GD_Ctr_VATRegistrationNumber", Result = 4 )]
		[TestCase ( "set value [LU + GD_Ctr_PIVA.Value] in GD_Ctr_VATRegistrationNumber", Result = 4 )]
		public int TokenizeBracketExpressionTests ( string line )
		{
			var lineInfo = lineParser.ParseLine ( lineParser.TokenizeMultiplePartOperators ( new LineInfo ( line ) ) );
			var newLine = lineParser.TokenizeBracketExpression ( lineInfo );
			return newLine.Tokens.Count ();
		}


		// ---------------------------------------------------------------------------------


		[TestCase ( "REQUEST.STATE in {STATE.LOCKED, STATE.CLOSED} then", "{STATE.LOCKED, STATE.CLOSED}", null, null, Result = 4 )]
		[TestCase ( "REQUEST.STATE in {STATE.LOCKED, STATE.CLOSED} or REQUEST.REQUEST_TYPE is CM_BLK  then", "{STATE.LOCKED, STATE.CLOSED}", null, null, Result = 8 )]
		[TestCase ( "REQUEST.STATE in {STATE.LOCKED, STATE.CLOSED} or REQUEST.REQUEST_TYPE is CM_BLK or REQUEST.CURRENT_ROLE in {ROLES.SUS, ROLES.RCO}  then",
			"{STATE.LOCKED, STATE.CLOSED}", "{ROLES.SUS, ROLES.RCO}", null, Result = 12 )]
		public int CheckForValueListsInConditionTests ( string sut, string fused1, string fused2, string fused3 )
		{
			var parser = new Parser ();
			var _lineParser = new LineParser ();
			var lineInfo = parser.ParseLine ( sut );
			parser.ParseLine ( _lineParser.TokenizeMultiplePartOperators ( lineInfo ) );
			LineParser.CheckForValueListsInCondition ( lineInfo );

			Assert.IsTrue ( lineInfo.Tokens.Any ( x => x.Token == fused1 ) );

			if ( fused2 != null )
				Assert.IsTrue ( lineInfo.Tokens.Any ( x => x.Token == fused2 ) );

			if ( fused3 != null )
				Assert.IsTrue ( lineInfo.Tokens.Any ( x => x.Token == fused3 ) );

			return lineInfo.Tokens.Count ();
		}
	}
}
