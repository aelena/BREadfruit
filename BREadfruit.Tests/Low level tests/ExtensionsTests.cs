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


        [Test]
        public void ContainsAnyTests ()
        {
            var _l = new List<string> () { "a", "b", "c", "d", "e", "f" };
            var _s = new List<string> () { "c", "r" };
            Assert.That ( _l.ContainsAny ( _s ) );
        }

        [Test]
        public void ContainsAnyTests_1 ()
        {
            var _l = new List<string> () { "a", "b", "c", "d", "e", "f" };
            var _s = new List<string> () { "aa", "r" };
            Assert.That ( !_l.ContainsAny ( _s ) );
        }

        [Test]
        public void ContainsAnyTests_2 ()
        {
            var _l = new List<Int32> () { 34, 45, 3, 425, 63, 3, 52, 354, 23, 4 };
            var _s = new List<Int32> () { 57, 128 };
            Assert.That ( !_l.ContainsAny ( _s ) );
        }

        [Test]
        public void ContainsAnyTests_3 ()
        {
            var _l = new List<Int32> () { 34, 45, 3, 425, 63, 3, 52, 354, 23, 4 };
            var _s = new List<Int32> () { 128, 99, 23 };
            Assert.That ( _l.ContainsAny ( _s ) );
        }

        [TestCase ( "this is my string yeah", Result = true )]
        [TestCase ( "that is my string yeah", Result = false )]
        [TestCase ( "well, yeah, about that...", Result = true )]
        [TestCase ( "", Result = false )]
        public bool ContainsAnyForStringTests ( string teststring )
        {
            return teststring.ContainsAny ( new List<String> () { "no", "I", "don't", "know", "about", "this" } );
        }


        [TestCase ( "this is my string yeah", "this", Result = true )]
        [TestCase ( "that is my string yeah", "", Result = false )]
        [TestCase ( "well, yeah, about that...", "about", Result = true )]
        [TestCase ( "", "", Result = false )]
        public bool ContainsAny2ForStringTests ( string teststring, string item )
        {
            var t = teststring.ContainsAny2 ( new List<String> () { "no", "I", "don't", "know", "about", "this" } );
            Assert.That ( t.Item2 == item );
            return t.Item1;
        }

        [Test]
        public void ContainsAny2Tests ()
        {
            var _l = new List<string> () { "a", "b", "c", "d", "e", "f" };
            var _s = new List<string> () { "c", "r" };
            Assert.That ( _l.ContainsAny2 ( _s ).Item1 );
            Assert.That ( _l.ContainsAny2 ( _s ).Item2.ToString () == "c" );
        }

        [Test]
        public void ContainsAny2Tests_1 ()
        {
            var _l = new List<string> () { "a", "b", "c", "d", "e", "f" };
            var _s = new List<string> () { "aa", "r" };
            Assert.That ( !_l.ContainsAny2 ( _s ).Item1 );
            Assert.That ( _l.ContainsAny2 ( _s ).Item2 == null );

        }

    }
}
