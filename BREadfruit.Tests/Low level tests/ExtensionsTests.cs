﻿using System;
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


        // ---------------------------------------------------------------------------------


        [Test]
        public void ContainsAny2Tests_1 ()
        {
            var _l = new List<string> () { "a", "b", "c", "d", "e", "f" };
            var _s = new List<string> () { "aa", "r" };
            Assert.That ( !_l.ContainsAny2 ( _s ).Item1 );
            Assert.That ( _l.ContainsAny2 ( _s ).Item2 == null );
        }


        // ---------------------------------------------------------------------------------


        [TestCase ( null, "A", 1, Result = "A" )]
        [TestCase ( "A", null, 1, Result = "A" )]
        [TestCase ( "A", null, 3, Result = "A" )]
        [TestCase ( "", "A", 1, Result = "A" )]
        [TestCase ( "", "A", 0, Result = "" )]
        [TestCase ( "", "A", 4, Result = "AAAA" )]
        [TestCase ( null, "A", 4, Result = "AAAA" )]
        [TestCase ( "test string", "\t", 3, Result = "\t\t\ttest string" )]
        [TestCase ( "test string", "\t", -13, Result = "\ttest string" )]
        [TestCase ( "test string", "\t", 1, Result = "\ttest string" )]
        [TestCase ( "test string", "\t", 0, Result = "test string" )]
        public string PrependTests ( string s, string prepended, int reps )
        {
            return s.Prepend ( prepended, reps );
        }


        // ---------------------------------------------------------------------------------


        [Test]
        public void PenultimateTests ()
        {
            var strings = new List<String> () { "cd", "ef", "po", "tu", "tt" };
            Assert.That ( strings.Penultimate () == "tu" );
        }


        // ---------------------------------------------------------------------------------


        [Test]
        public void ElementAtFromLastTests ()
        {
            var strings = new List<String> () { "cd", "ef", "po", "tu", "tt" };
            Assert.That ( strings.ElementAtFromLast ( 2 ) == "tu" );
        }


        // ---------------------------------------------------------------------------------


        [Test]
        public void ListIsNullOrEmptyTests_1 ()
        {
            List<string> l = null;
            Assert.That ( l.IsNullOrEmpty () );
        }


        // ---------------------------------------------------------------------------------


        [Test]
        public void ListIsNullOrEmptyTests_2 ()
        {
            List<string> l = new List<string> ();
            Assert.That ( l.IsNullOrEmpty () );
        }


        // ---------------------------------------------------------------------------------


        [Test]
        public void ListIsNullOrEmptyTests_3 ()
        {
            List<string> l = new List<string> () { "a" };
            Assert.That ( !l.IsNullOrEmpty () );
        }


        // ---------------------------------------------------------------------------------


        [TestCase ( null, 0, false, Result = "" )]
        [TestCase ( null, 0, true, Result = "" )]
        [TestCase ( null, 3, false, Result = "" )]
        [TestCase ( null, 3, true, Result = "" )]
        [TestCase ( "", 0, false, Result = "" )]
        [TestCase ( "", 0, true, Result = "" )]
        [TestCase ( "", 2, false, Result = "" )]
        [TestCase ( "", 2, true, Result = "" )]
        [TestCase ( "abcd", 2, true, Result = "ab" )]
        [TestCase ( "abcd", 2, false, Result = "ab" )]
        [TestCase ( "abcd ", 2, true, Result = "ab" )]
        [TestCase ( "abcd ", 2, false, Result = "abc" )]
        [TestCase ( "abcd       ", 2, true, Result = "ab" )]
        [TestCase ( "abcd    ", 2, false, Result = "abcd  " )]
        [TestCase ( "abcd ", 12, true, Result = "" )]
        [TestCase ( "abcd ", 12, false, Result = "" )]
        [TestCase ( "abcd", 3, true, Result = "a" )]
        [TestCase ( "abcd", 3, false, Result = "a" )]
        [TestCase ( "abcd", 4, true, Result = "" )]
        [TestCase ( "abcd", 4, false, Result = "" )]
        [TestCase ( "abcd", 5, true, Result = "" )]
        [TestCase ( "abcd", 5, false, Result = "" )]
        public string ShouldRemoveFromEnd ( string value, int charCount, bool trimEndFirst )
        {
            return value.RemoveFromEnd ( charCount, trimEndFirst );
        }


        // ---------------------------------------------------------------------------------


        [TestCase ( null, "", true, Result = "" )]
        [TestCase ( null, "", false, Result = "" )]
        [TestCase ( "", "", true, Result = "" )]
        [TestCase ( "", "", false, Result = "" )]
        [TestCase ( "", "a", true, Result = "" )]
        [TestCase ( "", "a", false, Result = "" )]
        [TestCase ( "the quick brown fox jumps over the lazy dog", "fox", false, Result = " jumps over the lazy dog" )]
        [TestCase ( "the quick brown fox jumps over the lazy dog", "fox", true, Result = "jumps over the lazy dog" )]
        [TestCase ( "the quick brown fox jumps over the lazy dog", "fox ", true, Result = "jumps over the lazy dog" )]
        [TestCase ( "the quick brown fox jumps over the lazy dog", "fox ", false, Result = "jumps over the lazy dog" )]
        [TestCase ( "the quick brown fox jumps over the lazy dog", "dog", true, Result = "" )]
        [TestCase ( "the quick brown fox jumps over the lazy dog", "dog", false, Result = "" )]
        [TestCase ( "the quick brown fox jumps over the lazy dog", "bull ", false, Result = "the quick brown fox jumps over the lazy dog" )]
        [TestCase ( "the quick brown fox jumps over the lazy dog", "bull ", true, Result = "the quick brown fox jumps over the lazy dog" )]
        [TestCase ( " the quick brown fox jumps over the lazy dog", "bull ", false, Result = " the quick brown fox jumps over the lazy dog" )]
        [TestCase ( " the quick brown fox jumps over the lazy dog", "bull ", true, Result = " the quick brown fox jumps over the lazy dog" )]
        [TestCase ( "the quick brown fox jumps over the lazy brown dog", "the", false, Result = " quick brown fox jumps over the lazy brown dog" )]
        [TestCase ( "the quick brown fox jumps over the lazy brown dog", "the", true, Result = "quick brown fox jumps over the lazy brown dog" )]
        [TestCase ( "the quick brown fox jumps over the lazy brown dog", "the ", false, Result = "quick brown fox jumps over the lazy brown dog" )]
        [TestCase ( "the quick brown fox jumps over the lazy brown dog", "the ", true, Result = "quick brown fox jumps over the lazy brown dog" )]
        public string SubStringAfterTests ( string value, string marker, bool trimResults )
        {

            return value.SubStringAfter ( marker, StringComparison.CurrentCulture, trimResults );

        }


        // ---------------------------------------------------------------------------------


        [TestCase ( null, "", true, Result = "" )]
        [TestCase ( null, "", false, Result = "" )]
        [TestCase ( "", "", true, Result = "" )]
        [TestCase ( "", "", false, Result = "" )]
        [TestCase ( "", "a", true, Result = "" )]
        [TestCase ( "", "a", false, Result = "" )]
        [TestCase ( "the quick brown fox jumps over the lazy dog", "fox", false, Result = " jumps over the lazy dog" )]
        [TestCase ( "the quick brown fox jumps over the lazy dog", "fox", true, Result = "jumps over the lazy dog" )]
        [TestCase ( "the quick brown fox jumps over the lazy dog", "fox ", true, Result = "jumps over the lazy dog" )]
        [TestCase ( "the quick brown fox jumps over the lazy dog", "fox ", false, Result = "jumps over the lazy dog" )]
        [TestCase ( "the quick brown fox jumps over the lazy dog", "dog", true, Result = "" )]
        [TestCase ( "the quick brown fox jumps over the lazy dog", "dog", false, Result = "" )]
        [TestCase ( "the quick brown fox jumps over the lazy dog", "bull ", false, Result = "the quick brown fox jumps over the lazy dog" )]
        [TestCase ( "the quick brown fox jumps over the lazy dog", "bull ", true, Result = "the quick brown fox jumps over the lazy dog" )]
        [TestCase ( " the quick brown fox jumps over the lazy dog", "bull ", false, Result = " the quick brown fox jumps over the lazy dog" )]
        [TestCase ( " the quick brown fox jumps over the lazy dog", "bull ", true, Result = " the quick brown fox jumps over the lazy dog" )]
        [TestCase ( "the quick brown fox jumps over the lazy brown dog", "brown", false, Result = " dog" )]
        [TestCase ( "the quick brown fox jumps over the lazy brown dog", "brown", true, Result = "dog" )]
        [TestCase ( "the quick brown fox jumps over the lazy brown dog", "the", false, Result = " lazy brown dog" )]
        [TestCase ( "the quick brown fox jumps over the lazy brown dog", "the", true, Result = "lazy brown dog" )]
        [TestCase ( "the quick brown fox jumps over the lazy brown dog", "the ", false, Result = "lazy brown dog" )]
        [TestCase ( "the quick brown fox jumps over the lazy brown dog", "the ", true, Result = "lazy brown dog" )]
        public string SubStringAfterLastTests ( string value, string marker, bool trimResults )
        {

            return value.SubStringAfterLast ( marker, StringComparison.CurrentCulture, trimResults );

        }


        // ---------------------------------------------------------------------------------


    }
}
