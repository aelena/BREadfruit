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


    }
}
