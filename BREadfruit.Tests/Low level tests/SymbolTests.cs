using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BREadfruit.Tests.Low_level_tests
{
    [TestFixture]
    public class SymbolTests
    {

        [TestCase ( "test", 0, true )]
        [TestCase ( "test", 1, true )]
        public void SymbolShouldConstructCorrectly ( string token, int indentlevel, bool isTerminal )
        {
            var s = new Symbol ( token, indentlevel );
            Assert.That ( s.IndentLevel == indentlevel );
            Assert.That ( s.IsTerminal == isTerminal );
            Assert.That ( s.Token == token );
            Assert.That ( s.Children.Count () == 0 );
            Assert.That ( s.ValidChildTokens ().Count() == 0 );
        }


    }
}
