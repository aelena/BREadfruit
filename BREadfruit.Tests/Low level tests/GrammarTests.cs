using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BREadfruit.Tests.Low_level_tests
{
    [TestFixture]
    public class GrammarTests
    {

        [TestCase ( " WItH ", false, 3, false )]
        [TestCase ( "With", false, 3, true )]
        public void ShouldFindSymbol ( string token, bool isTerminal, int childCount, bool strictMatch )
        {
            var s = Grammar.GetSymbolByToken ( token, strictMatch );
            Assert.That ( s.IsTerminal == isTerminal );
            Assert.That ( s.Children.Count () == childCount );
        }


    }
}
