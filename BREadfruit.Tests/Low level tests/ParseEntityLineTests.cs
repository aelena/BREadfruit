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
        public void ShouldFindIndentLevelCorrectly(string line, int indentCount)
        {
            var p = new LineParser ();
            var li = p.ParseLine ( line );
            Assert.That ( li.IndentLevel == indentCount );
        }
    }
}
