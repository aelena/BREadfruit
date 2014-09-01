using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;


namespace BREadfruit.Tests
{
    [TestFixture]
    public class Class1
    {

        [Test]
        public void ShouldFindEntities ()
        {
            var parser = new Parser ();
            parser.ParseRuleFile ( @"..\..\sample files\vendor-rules.txt" );
        }

    }
}
