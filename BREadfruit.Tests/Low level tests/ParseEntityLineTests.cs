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
            var p = new LineParser ();
            var li = p.ParseLine ( line );
            Assert.That ( li.IndentLevel == indentCount );
        }

        [TestCase ( "", 0 )]
        [TestCase ( "Entity 'ABC'", 2 )]
        [TestCase ( "\tEntity 'ABC' is TextBox", 4 )]
        [TestCase ( "\t\t", 0 )]
        public void ShouldFindCorrectTokenCount ( string line, int tokenCount )
        {
            var p = new LineParser ();
            var li = p.ParseLine ( line );
            Assert.That ( li.Tokens.Count () == tokenCount );
        }

        [Test]
        public void ShouldValidateCorrectLineInfoInstances ()
        {
            var p = new LineParser ();
            var lineInfo = p.ParseLine ( "Entity ABC Is TextBoxt" );
            Assert.That ( p.IsAValidSentence ( lineInfo ) == true );
        }
    }
}
