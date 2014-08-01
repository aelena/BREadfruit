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
    public class ExtensionsTests
    {
        [Test]
        public void NewTakeExtensionShouldWorkOnACorrectListOfString ()
        {
            IEnumerable<string> _data = new List<string> () { "a", "kl", "do", "d", "cm", "y", "u", "··", "·t" };
            var _sub = _data.Take ( 2, 4 );
            Assert.That ( _sub.First () == "do" ); 
            Assert.That ( _sub.Last () == "cm" );
            Assert.That ( _sub.Count () == 3 );
        }

        [Test]
        public void NewTakeExtensionShouldWorkOnACorrectListOfInt ()
        {
            var _data = new List<Int32> () { 6, 3, 6, 76, 544, 54, 2, 72, 445, 23, 55, 91 };
            var _sub = _data.Take ( 3, 6 );
            Assert.That ( _sub.First () == 76 );
            Assert.That ( _sub.Last () == 2 );
            Assert.That ( _sub.Count () == 4 );
        }

        [Test]
        public void NewToFromExtensionShouldWorkOnACorrectListOfString ()
        {
            IEnumerable<string> _data = new List<string> () { "a", "kl", "do", "d", "cm", "y", "u", "··", "·t" };
            var _sub = _data.From ( 2 ).To ( 2 );
            Assert.That ( _sub.First () == "do" );
            Assert.That ( _sub.Last () == "cm" );
            Assert.That ( _sub.Count () == 3 );
        }

        [Test]
        public void NewToFromExtensionsShouldWorkOnACorrectListOfInt ()
        {
            var _data = new List<Int32> () { 6, 3, 6, 76, 544, 54, 2, 72, 445, 23, 55, 91 };
            var _sub = _data.From ( 3 ).To ( 3 );
            Assert.That ( _sub.First () == 76 );
            Assert.That ( _sub.Last () == 2 );
            Assert.That ( _sub.Count () == 4 );
        }
    }
}
