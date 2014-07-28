using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace BREadfruit.Tests.Low_level_tests
{
    [TestFixture]
    public class ParseEntityLineTests
    {
        [TestCase ( "", 0 )]
        [TestCase ( "Entity", 0 )]
        [TestCase ( "\tEntity", 1 )]
        [TestCase ( "\t\tEntity", 2 )]
        [TestCase ( "\t\t\tEntity", 3 )]
        public void ShouldFindIndentLevelCorrectly ( string line, int indentCount )
        {
            //var p = new LineParser ();
            var li = LineParser.ParseLine ( line );
            Assert.That ( li.IndentLevel == indentCount );
        }

        [TestCase ( "", 0 )]
        [TestCase ( "Entity 'ABC'", 2 )]
        [TestCase ( "\tEntity 'ABC' is TextBox", 4 )]
        [TestCase ( "\t\t", 0 )]
        public void ShouldFindCorrectTokenCount ( string line, int tokenCount )
        {
            //var p = new LineParser ();
            var li = LineParser.ParseLine ( line );
            Assert.That ( li.Tokens.Count () == tokenCount );
        }


        [TestCase ( "Entity ABC IS TextBoxt", true )]
        [TestCase ( "Entity ABC Is TextBoxt", true )]
        [TestCase ( "Entity ABC", false )]
        [TestCase ( "Entity ABC Are TextBoxt", false )]
        [TestCase ( "Entity ABC Are TextBoxt A", false )]
        public void ShouldDetectValidAndInvalidLineInfoSentences(string lineInfo, bool result)
        {
            //var p = new LineParser ();
            var _li = new LineInfo ( lineInfo );
            Assert.That ( LineParser.IsAValidSentence ( _li ) == result );

        }
    }
}
