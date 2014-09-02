using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using BREadfruit.Helpers;

namespace BREadfruit.Tests.Low_level_tests
{
    [TestFixture]
    public class LineParserTests
    {

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
            var _t = LineParser.ExtractTokens ( line );
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

            var _t = LineParser.ExtractTokens ( line ).ToList ();
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
            var _t = LineParser.ExtractTokens ( line ).ToList ();
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
            var _t = LineParser.ExtractTokens ( line ).ToList ();
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
            var li = LineParser.ParseLine ( line );
            var resolved = LineParser.TokenizeMultiplePartOperators ( li );
            return resolved;
        }


        // ---------------------------------------------------------------------------------


        [TestCase ( "role is 'ENQ' then enabled", "role is 'ENQ'", Result = 1 )]
        [TestCase ( "role is {UPM,CPM} then not enabled", "role is {UPM,CPM}", Result = 1 )]
        public int ExtractConditionsTests_1 ( string line, string expected )
        {
            var lineInfo = LineParser.ParseLine ( line );
            lineInfo = LineParser.ParseLine ( LineParser.TokenizeMultiplePartOperators ( lineInfo ) );
            var _conds = LineParser.ExtractConditions ( lineInfo );
            Assert.That ( _conds.First ().ToString ().Equals ( expected ) );
            Assert.That ( _conds.First ().SuffixLogicalOperator == null );
            return _conds.Count ();
        }

        [TestCase ( "role is 'ENQ' and role is {UPM,CPM} then enabled", "role is 'ENQ'", "role is {UPM,CPM}", Result = 2 )]
        [TestCase ( "VendorCountry is ES and TaxCode1 starts with {P,Q,S} then", "VendorCountry is ES", "TaxCode1 starts_with {P,Q,S}", Result = 2 )]
        public int ExtractConditionsTests_2 ( string line, string expected1, string expected2 )
        {
            var lineInfo = LineParser.ParseLine ( line );
            lineInfo = LineParser.ParseLine ( LineParser.TokenizeMultiplePartOperators ( lineInfo ) );
            var _conds = LineParser.ExtractConditions ( lineInfo );
            Assert.That ( _conds.First ().ToString ().Equals ( expected1 ) );
            Assert.That ( _conds.First ().SuffixLogicalOperator == "and" );
            Assert.That ( _conds.Last ().ToString ().Equals ( expected2 ) );
            Assert.That ( _conds.Last ().SuffixLogicalOperator == null );

            return _conds.Count ();
        }

        [TestCase ( "role is 'ENQ' and role is {UPM,CPM} or field is disabled then enabled",
            "role is 'ENQ'", "role is {UPM,CPM}", "field is disable", Result = 3 )]
        [TestCase ( "VendorCountry is ES and TaxCode1 starts with {P,Q,S} or he is hungry then",
            "VendorCountry is ES", "TaxCode1 starts_with {P,Q,S}", "he is hungry", Result = 3 )]
        public int ExtractConditionsTests_3 ( string line, string expected1, string expected2, string expected3 )
        {
            var lineInfo = LineParser.ParseLine ( line );
            lineInfo = LineParser.ParseLine ( LineParser.TokenizeMultiplePartOperators ( lineInfo ) );
            var _conds = LineParser.ExtractConditions ( lineInfo );
            Assert.That ( _conds.First ().ToString ().Equals ( expected1 ) );
            Assert.That ( _conds.First ().SuffixLogicalOperator == "and" );
            Assert.That ( _conds.ElementAt ( 1 ).ToString ().Equals ( expected2 ) );
            Assert.That ( _conds.ElementAt ( 1 ).SuffixLogicalOperator == "or" );
            Assert.That ( _conds.Last ().ToString ().Equals ( expected3 ) );
            Assert.That ( _conds.Last ().SuffixLogicalOperator == null );
            return _conds.Count ();
        }
    }
}
