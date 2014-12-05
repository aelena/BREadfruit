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

		[TestCase ( "this is my string yeah", false, Result = true )]
		[TestCase ( "that is my string yeah", false, Result = false )]
		[TestCase ( "well, yeah, about that...", false, Result = true )]
		[TestCase ( "usually, island are inhabited by islanders", true, Result = true )]
		[TestCase ( "usually, island are inhabited by islanders", false, Result = true )]
		[TestCase ( "usually, islands are inhabited by islanders", true, Result = false )]
		[TestCase ( "usually, islands are inhabited by islanders", false, Result = true )]
		[TestCase ( "the islanders were crazy", true, Result = false )]
		[TestCase ( "the islanders were crazy", false, Result = true )]
		[TestCase ( "", false, Result = false )]
		[TestCase ( "", true, Result = false )]
		public bool ContainsAnyForStringTests ( string teststring, bool asWord )
		{
			var _ = teststring.ContainsAny ( new List<String> () { "no", "I", "don't", "know", "about", "this", "island" }, asWord );
			return _;
		}


		[TestCase ( "this is my string yeah", "this", false, Result = true )]
		[TestCase ( "that is my string yeah", "", false, Result = false )]
		[TestCase ( "well, yeah, about that...", "about", false, Result = true )]
		[TestCase ( "usually, an island is inhabited by islanders", "island", true, Result = true )]
		[TestCase ( "the islanders were crazy", "", true, Result = false )]
		[TestCase ( "the islanders were crazy", "island", false, Result = true )]
		[TestCase ( "usually, island are inhabited by islanders", "island", true, Result = true )]
		[TestCase ( "usually, island are inhabited by islanders", "island", false, Result = true )]
		[TestCase ( "usually, islands are inhabited by islanders", "", true, Result = false )]
		[TestCase ( "usually, islands are inhabited by islanders", "island", false, Result = true )]
		[TestCase ( "", "", false, Result = false )]
		[TestCase ( "", "", true, Result = false )]
		public bool ContainsAny2ForStringTests ( string teststring, string item, bool asWord )
		{
			var t = teststring.ContainsAny2 ( new List<String> () { "no", "I", "don't", "know", "about", "this", "island" }, asWord );
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



		[TestCase ( "{}", "{", "}", Result = "" )]
		[TestCase ( "{amduscia}", "{", "}", Result = "amduscia" )]
		[TestCase ( " a band like {amduscia}", "{", "}", Result = "amduscia" )]
		[TestCase ( "the quick brown fox jumps over the lazy brown dog", "quick", "lazy", Result = " brown fox jumps over the " )]
		[TestCase ( "{\"Country\":\"ES\"}", "{", "}", Result = "\"Country\":\"ES\"" )]
		[TestCase ( " {\"Country\":\"ES\",\"Active\":true, \"Age\":30, \"Title\":\"No reason to fear this\" }", "{", "}", Result = "\"Country\":\"ES\",\"Active\":true, \"Age\":30, \"Title\":\"No reason to fear this\" " )]
		public string TakeBetweenTests ( string original, string s1, string s2 )
		{
			return original.TakeBetween ( s1, s2 );
		}


		// ---------------------------------------------------------------------------------


		[TestCase ( "this is the message \"the email is used generally for all the automatic communication with the supplier - e.g. payment advice\" that was said to me",
			"\"", "\"", Result = "the email is used generally for all the automatic communication with the supplier - e.g. payment advice" )]
		[TestCase ( "this is the message \"the email is used generally for all the \"automatic\" communication with the supplier - e.g. payment advice\" that was said to me",
			 "\"", "\"", Result = "the email is used generally for all the \"automatic\" communication with the supplier - e.g. payment advice" )]
		[TestCase ( "and then, what about this text with no coincidences?",
			"\"", "\"", Result = "" )]
		public string TakeBetweenTests2 ( string original, string s1, string s2 )
		{
			return original.TakeBetween ( s1, s2 );
		}


		// ---------------------------------------------------------------------------------



		[TestCase ( "{'P', 'Q'}", " ", "{", "}", Result = "{'P','Q'}" )]
		[TestCase ( "{'P',     'Q'}", " ", "{", "}", Result = "{'P','Q'}" )]
		[TestCase ( "{'P', 'Q', '2', 'birds'}", " ", "{", "}", Result = "{'P','Q','2','birds'}" )]
		[TestCase ( "{'P',   'Q', '2',    'birds'}", " ", "{", "}", Result = "{'P','Q','2','birds'}" )]

		[TestCase ( "hello boys and girls {'P', 'Q'} what was that?", " ", "{", "}", Result = "hello boys and girls {'P','Q'} what was that?" )]
		[TestCase ( "hello boys and girls {'P',     'Q'} what was that?", " ", "{", "}", Result = "hello boys and girls {'P','Q'} what was that?" )]
		[TestCase ( "hello boys and girls {'P', 'Q', '2', 'birds'} what was that?", " ", "{", "}", Result = "hello boys and girls {'P','Q','2','birds'} what was that?" )]
		[TestCase ( "hello boys and girls {'P',   'Q', '2',    'birds'} what was that?", " ", "{", "}", Result = "hello boys and girls {'P','Q','2','birds'} what was that?" )]

		[TestCase ( "dog{'P', 'Q'}god", " ", "dog{", "}god", Result = "dog{'P','Q'}god" )]
		[TestCase ( "dog{'P',     'Q'}god", " ", "dog{", "}god", Result = "dog{'P','Q'}god" )]
		[TestCase ( "dog{'P', 'Q', '2', 'birds'}god", " ", "dog{", "}god", Result = "dog{'P','Q','2','birds'}god" )]
		[TestCase ( "dog{'P',   'Q', '2',    'birds'}god", " ", "dog{", "}god", Result = "dog{'P','Q','2','birds'}god" )]

		[TestCase ( "random stirng before dog{'P', 'Q'}god", " ", "dog{", "}god", Result = "random stirng before dog{'P','Q'}god" )]
		[TestCase ( "random stirng before dog{'P',     'Q'}god", " ", "dog{", "}god", Result = "random stirng before dog{'P','Q'}god" )]
		[TestCase ( "random stirng before dog{'P', 'Q', '2', 'birds'}god", " ", "dog{", "}god", Result = "random stirng before dog{'P','Q','2','birds'}god" )]
		[TestCase ( "random stirng before dog{'P',   'Q', '2',    'birds'}god", " ", "dog{", "}god", Result = "random stirng before dog{'P','Q','2','birds'}god" )]
		public string ReplaceBetweenTests_01 ( string original, string removee, string s1, string s2 )
		{

			// TODO: ADD FOR SEVERAL CHARS, like \t etc 
			// TODO: or even an overload that takes a set of key value pairs for pieces to remove and pieces to replace
			return original.RemoveBetween ( removee, s1, s2 );
		}


		[TestCase ( "{'P I X I E L A N D', 'Q U O'}", " ", "{", "}", Result = "{'P I X I E L A N D','Q U O'}" )]
		[TestCase ( "{  'P I X I E L A N D',    'Q U O'}", " ", "{", "}", Result = "{'P I X I E L A N D','Q U O'}" )]
		[TestCase ( "{  'P I X I E L A N D',    'Q U O'  }", " ", "{", "}", Result = "{'P I X I E L A N D','Q U O'}" )]
		[TestCase ( "my dreams are of {  'P I X I E L A N D',    'Q U O'  } quandaries", " ", "{", "}",
			Result = "my dreams are of {'P I X I E L A N D','Q U O'} quandaries" )]

		public string ReplaceBetweenTests_02 ( string original, string removee, string s1, string s2 )
		{

			// TODO: ADD FOR SEVERAL CHARS, like \t etc 
			// TODO: or even an overload that takes a set of key value pairs for pieces to remove and pieces to replace
			var rep = original.RemoveBetween ( removee, s1, s2, ( x, y ) => x.IsBetween ( original, "'", y ) );
			return rep;
		}


		// ---------------------------------------------------------------------------------


		[TestCase ( "xxx { \"yay ouch\" : \"i am weasel\" } xx", " ", "{", "}", Result = "xxx {\"yay ouch\":\"i am weasel\"} xx" )]
		public string RemoveBetweenTests_02 ( string original, string removee, string s1, string s2 )
		{
			return original.RemoveBetween ( removee, s1, s2, ( x, y ) => x.IsBetween ( original, "\"", y ) );
		}


		// ---------------------------------------------------------------------------------


		[TestCase ( "{'P', 'Q'}", " ", "{", "}", Result = "{'P','Q'}" )]
		[TestCase ( "{'P',  'Q'}", " ", "{", "}", Result = "{'P','Q'}" )]
		[TestCase ( "{'P',          'Q', '2', 'birds'}", " ", "{", "}", Result = "{'P','Q','2','birds'}" )]
		[TestCase ( "{'P',      'Q',     '2',    'birds'}", " ", "{", "}", Result = "{'P','Q','2','birds'}" )]
		public string RemoveBetweenTests2 ( string original, string removee, string s1, string s2 )
		{
			return original.RemoveBetween ( new List<String> { " ", "\t" }, s1, s2 );
		}


		// ---------------------------------------------------------------------------------


		[TestCase ( "{'P', - 'Q'}", " ", "{", "}", Result = "{'P','Q'}" )]
		[TestCase ( "{'P',   -- 'Q'}", " ", "{", "}", Result = "{'P','Q'}" )]
		[TestCase ( "{'P',      -  -    -   'Q', '2', 'birds'}", " ", "{", "}", Result = "{'P','Q','2','birds'}" )]
		[TestCase ( "{'P',  - - #    'Q',     '2',    'birds'}", " ", "{", "}", Result = "{'P','Q','2','birds'}" )]
		public string RemoveBetweenTests3 ( string original, string removee, string s1, string s2 )
		{
			return original.RemoveBetween ( new List<String> { " ", "\t", "-", "#" }, s1, s2 );
		}


		// ---------------------------------------------------------------------------------


		[TestCase ( null, " ", "{", "}", Result = "{'P','Q'}", ExpectedException = typeof ( ArgumentException ), ExpectedMessage = "String cannot be null" )]
		[TestCase ( "}'P',   -- 'Q'{", " ", "{", "}", Result = "{'P','Q'}", ExpectedException = typeof ( Exception ), ExpectedMessage = "End string cannot appear earlier than beginning string" )]
		public string RemoveBetweenTestsExceptions ( string original, string removee, string s1, string s2 )
		{
			return original.RemoveBetween ( new List<String> { " ", "\t", "-", "#" }, s1, s2 );
		}


		// ---------------------------------------------------------------------------------


		// this one should not change as island is not contained as a word inside the subject sentence
		[TestCase ( "the islanders were insane", "island", "villag", Result = "the islanders were insane" )]
		[TestCase ( "the islands were quite beautiful in summer", "islands", "villages", Result = "the villages were quite beautiful in summer" )]
		[TestCase ( "some islands were quite beautiful in summer, other islands were better in winter", "islands", "villages",
			Result = "some villages were quite beautiful in summer, other villages were better in winter" )]
		public string ReplaceWordTests ( string teststring, string newstring, string oldstring )
		{
			return teststring.ReplaceWord ( newstring, oldstring );
		}


		// ---------------------------------------------------------------------------------


		[TestCase ( "let sleeping dogs lie", Result = "t sping dogs i" )]
		public string ImprovedRemoveTests_1 ( string line )
		{
			return line.MultipleReplace ( new List<string> () { "l", "e" } );
		}

		// ---------------------------------------------------------------------------------


		[TestCase ( "'All the things she did' was a track performed by \"The Whatevers\"", Result = "All the things she did was a track performed by The Whatevers" )]
		public string ImprovedRemoveTests_2 ( string line )
		{
			return line.MultipleReplace ( new List<string> () { "\"", "'" } );
		}


		// ---------------------------------------------------------------------------------


		[TestCase ( "let sleeping dogs lie", "l", "", Result = "et sleeping dogs lie" )]
		[TestCase ( "let sleeping dogs lie", "t", "", Result = "le sleeping dogs lie" )]
		[TestCase ( "let sleeping dogs lie", "p", "", Result = "let sleeing dogs lie" )]

		[TestCase ( "let sleeping dogs lie", "l", "@", Result = "@et sleeping dogs lie" )]
		[TestCase ( "let sleeping dogs lie", "t", "KUZ", Result = "leKUZ sleeping dogs lie" )]
		[TestCase ( "let sleeping dogs lie", "p", "1234567890", Result = "let slee1234567890ing dogs lie" )]
		public string ReplaceFirstTests ( string line, string ocurrence, string replacement )
		{
			if ( String.IsNullOrEmpty ( replacement ) )
				return line.ReplaceFirst ( ocurrence );
			return line.ReplaceFirst ( ocurrence, replacement );
		}


		// ---------------------------------------------------------------------------------


		[TestCase ( "let sleeping dogs lie", "l", "", Result = "let sleeping dogs ie" )]
		[TestCase ( "let sleeping dogs lie", "t", "", Result = "le sleeping dogs lie" )]
		[TestCase ( "let sleeping dogs lie", "i", "", Result = "let sleeping dogs le" )]

		[TestCase ( "let sleeping dogs lie", "l", "@", Result = "let sleeping dogs @ie" )]
		[TestCase ( "let sleeping dogs lie", "t", "KUZ", Result = "leKUZ sleeping dogs lie" )]
		[TestCase ( "let sleeping dogs lie", "g", "KUZ", Result = "let sleeping doKUZs lie" )]
		[TestCase ( "let sleeping dogs lie", "e", "1234567890", Result = "let sleeping dogs li1234567890" )]
		public string ReplaceLastTests ( string line, string occurrence, string replacement )
		{
			if ( String.IsNullOrEmpty ( replacement ) )
				return line.ReplaceLast ( occurrence );
			return line.ReplaceLast ( occurrence, replacement );
		}

		// ---------------------------------------------------------------------------------


		[TestCase ( "let sleeping dogs lie", "l", "", Result = "et sleeping dogs ie" )]
		[TestCase ( "let sleeping dogs lie", "l", "a588", Result = "a588et sleeping dogs a588ie" )]
		[TestCase ( "She said several things, like \"yeah\", \"like\" and \"whatever\", I did not like her speech", "\"", "",
			Result = "She said several things, like yeah\", \"like\" and \"whatever, I did not like her speech" )]
		[TestCase ( "She said \"yeah\"", "\"", "", Result = "She said yeah" )]
		[TestCase ( "\"yeah, she said", "\"", "", Result = "yeah, she said" )]
		public string ReplaceFirstAndLastOnlyTests ( string line, string occurrence, string replacement )
		{
			if ( String.IsNullOrEmpty ( replacement ) )
				return line.ReplaceFirstAndLastOnly ( occurrence );

			return line.ReplaceFirstAndLastOnly ( occurrence, replacement );
		}


		// ---------------------------------------------------------------------------------


		[TestCase ( "let the [ show begin ] yeah", "[", "]", Result = true )]
		[TestCase ( "let the [ show begin yeah", "[", "]", Result = false )]
		[TestCase ( "let the show begin yeah", "[", "]", Result = false )]
		[TestCase ( "let the [show begin yeah", "[show", "]", Result = false )]
		[TestCase ( "]let the [show begin yeah", "[show", "]", Result = true )]
		public bool MultipleContains ( string sut, string s1, string s2 )
		{
			return sut.Contains ( new [] { s1, s2 } );
		}


		// ---------------------------------------------------------------------------------


		[TestCase ( "", Result = "" )]
		[TestCase ( "[]", Result = "[]" )]
		[TestCase ( "[", Result = "[" )]
		[TestCase ( "set value [LU + GD_Ctr_PIVA.Value] in GD_Ctr_VATRegistrationNumber", Result = "set value [LU + GD_Ctr_PIVA.Value]" )]
		[TestCase ( "set value LU + GD_Ctr_PIVA.Value in GD_Ctr_VATRegistrationNumber", Result = "set value LU + GD_Ctr_PIVA.Value in GD_Ctr_VATRegistrationNumber" )]
		public string TakeUntilTests ( string sut )
		{
			LineInfo li = new LineInfo ( sut );
			var reduced = li.Tokens.TakeUntil ( x => x.Token.EndsWith ( Grammar.ClosingSquareBracket.Token ) );
			return reduced.JoinTogether ().Token;
		}


		// ---------------------------------------------------------------------------------


		[TestCase ( "", Result = "" )]
		[TestCase ( "[]", Result = "[]" )]
		[TestCase ( "[", Result = "[" )]
		[TestCase ( "set value [LU + GD_Ctr_PIVA.Value] in GD_Ctr_VATRegistrationNumber", Result = "set value [LU + GD_Ctr_PIVA.Value]" )]
		[TestCase ( "set value LU + GD_Ctr_PIVA.Value in GD_Ctr_VATRegistrationNumber", Result = "set value LU + GD_Ctr_PIVA.Value in GD_Ctr_VATRegistrationNumber" )]
		public string RemoveAfterTest ( string sut )
		{
			LineInfo li = new LineInfo ( sut );
			var __li = li.Tokens.RemoveAfter ( x => x.Token.EndsWith ( Grammar.ClosingSquareBracket.Token ) );
			return __li.JoinTogether ().Token;
		}


		// ---------------------------------------------------------------------------------


		[Test]
		public void IndexOfFuncTests_01 ()
		{
			var arr = new [] { 1, 3, 5, 7, 9, 11 };
			Assert.That ( arr.IndexOf ( x => x > 6 ) == 3 );
		}


		[Test]
		public void IndexOfFuncTests_02 ()
		{
			var arr = "hey,you,what,are,you,doing".Split ( ',' );
			Assert.That ( arr.IndexOf ( x => x.EndsWith ( "t" ) ) == 2 );
		}

		[Test]
		[ExpectedException ( ExpectedException = typeof ( ArgumentNullException ),
			UserMessage = "The list to be searched cannot be null" )]
		public void IndexOfFuncTests_03 ()
		{
			List<object> arr = null;
			Assert.That ( arr.IndexOf ( x => x != null ) == 2 );
		}

		[Test]
		[ExpectedException ( ExpectedException = typeof ( ArgumentNullException ),
			UserMessage = "The search function cannot be null" )]
		public void IndexOfFuncTests_04 ()
		{
			var arr = "hey,you,what,are,you,doing".Split ( ',' );
			Assert.That ( arr.IndexOf ( null ) == 2 );
		}

		// ---------------------------------------------------------------------------------

		[Test]
		public void FindNthTests_01 ()
		{
			var arr = new [] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
			Assert.That ( arr.FindNth ( x => x % 2 == 0, 3 ) == 8 );
		}


		[Test]
		public void FindNthTests_02 ()
		{
			var arr = "hey,you,what,are,you,doing".Split ( ',' );
			Assert.That ( arr.FindNth ( x => x.Length == 3, 2 ) == "are" );
		}

		[Test]
		public void FindNthTests_03 ()
		{
			var arr = "hey,you,what,are,you,doing".Split ( ',' );
			Assert.That ( arr.FindNth ( x => x.EndsWith ( "g" ), 0 ) == "doing" );
		}

		[Test]
		[ExpectedException ( ExpectedException = typeof ( ArgumentNullException ),
			UserMessage = "The list to be searched cannot be null" )]
		public void FindNthTests_04 ()
		{
			List<string> arr = null;
			Assert.That ( arr.FindNth ( x => x.Length == 3, 2 ) == "are" );
		}

		[Test]
		[ExpectedException ( ExpectedException = typeof ( ArgumentNullException ),
			UserMessage = "The search function cannot be null" )]
		public void FindNthTests_05 ()
		{
			var arr = "hey,you,what,are,you,doing".Split ( ',' );
			Assert.That ( arr.FindNth ( null, 2 ) == "are" );
		}

		[Test]
		[ExpectedException ( ExpectedException = typeof ( ArgumentException ),
			UserMessage = "The index cannot greater than the number of elements in the list" )]
		public void FindNthTests_06 ()
		{
			var arr = "hey,you,what,are,you,doing".Split ( ',' );
			Assert.That ( arr.FindNth ( x => x.Length == 3, 6 ) == "are" );
		}

		[Test]
		[ExpectedException ( ExpectedException = typeof ( ArgumentException ),
			UserMessage = "The index cannot greater than the number of elements in the list" )]
		public void FindNthTests_07 ()
		{
			var arr = "hey,you,what,are,you,doing".Split ( ',' );
			Assert.That ( arr.FindNth ( x => x.Length == 3, 7 ) == "are" );
		}

		// ---------------------------------------------------------------------------------


		[Test]
		public void IndicesOfTests_01 ()
		{
			var arr = new [] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
			Assert.That ( arr.IndicesOf ( x => x % 2 == 0 ).Count () == 5 );
		}

		[Test]
		public void IndicesOfTests_02 ()
		{
			var arr = new [] { "kl", "lko", "98", "x", null, "" };
			Assert.That ( arr.IndicesOf ( x => x.LengthSafe () == 2 ).Count () == 2 );
		}

		[Test]
		public void IndicesOfTests_03 ()
		{
			var arr = new [] { "kl", "lko", "98", "x", null, "" };
			Assert.That ( arr.IndicesOf ( x => x.LengthSafe () == 0 ).Count () == 2 );
		}

		[Test]
		[ExpectedException ( ExpectedException = typeof ( ArgumentNullException ),
			UserMessage = "The list to be searched cannot be null" )]
		public void IndicesOfTests_04 ()
		{
			List<string> arr = null;
			Assert.That ( arr.IndicesOf ( x => x.Length == 3 ).Count () == 1 );
		}

		[Test]
		[ExpectedException ( ExpectedException = typeof ( ArgumentNullException ),
			UserMessage = "The search function cannot be null" )]
		public void IndicesOfTests_05 ()
		{
			var arr = "hey,you,what,are,you,doing".Split ( ',' );
			Assert.That ( arr.IndicesOf ( null ).Count () == 1 );
		}


		// ---------------------------------------------------------------------------------


		//public bool IsBetween ( string sut, string beginString, string endString)
		//{

		//}


		[TestCase ( "alabama", "a", "0,2,4,6", false, Result = 4 )]
		[TestCase ( "ALABAMA", "A", "0,2,4,6", false, Result = 4 )]
		[TestCase ( "alabama", "A", "0,2,4,6", true, Result = 4 )]
		[TestCase ( "alAbamA", "A", "0,2,4,6", true, Result = 4 )]
		[TestCase ( "Mississippi", "ss", "2,5", true, Result = 2 )]
		[TestCase ( "yeah { do { the } dance {", "{", "5,10, 24", true, Result = 3 )]

		public int IndicesOfAllTest ( string sut, string search, string expectedIndices, bool ignoreCase )
		{
			var _result = sut.IndicesOfAll ( search, ignoreCase );
			var _indices = expectedIndices.Split ( new char [] { ',' },
				StringSplitOptions.RemoveEmptyEntries ).Select ( x => Convert.ToInt32 ( x ) ); ;
			Assert.IsTrue ( _result.Count () == _indices.Count () );
			Assert.That ( _result.Sum () == _indices.Sum () );
			return _result.Count ();
		}


		// ---------------------------------------------------------------------------------


		[TestCase ( 4, "2,3,4,5,6,7", Result = true )]
		[TestCase ( 2, "2,3,4,5,6,7", Result = false )]
		[TestCase ( 1, "2,3,4,5,6,7", Result = false )]
		public bool IsBetweenMinAndMax_Test ( int sut, string searchArray )
		{
			var _indices = searchArray.Split ( new char [] { ',' },
				StringSplitOptions.RemoveEmptyEntries ).Select ( x => Convert.ToInt32 ( x ) );
			return sut.IsBetweenMinAndMax ( _indices );
		}


		// ---------------------------------------------------------------------------------


		[TestCase ( "a", "\"abcd\"efgh", "\"", Result = true )]
		[TestCase ( "f", "\"abcd\"efgh", "\"", Result = false )]
		[TestCase ( "x", "\"abcd\"efgh", "\"", Result = false )]
		[TestCase ( "bc", "\"abcd\"efgh", "\"", Result = true )]
		[TestCase ( "bc", "\"abcd\"efgh", "{", Result = false )]
		[TestCase ( "f", "\"abcd\"efgh", "{", Result = false )]
		[TestCase ( "z", "\"abcd\"efgh", "{", Result = false )]
		[TestCase ( "a", "{abcd{efgh", "{", Result = true )]
		[TestCase ( "z", "\"abcd\"efgh", "{", Result = false )]
		[TestCase ( "drugs", "sex and drugs and sex and money", "sex", Result = true )]
		[TestCase ( " ", "sex and drugs and sex and money", "sex", Result = true )]
		public bool IsBetween_Tests ( string sut, string search, string marker )
		{
			return sut.IsBetween ( search, marker );
		}

		[TestCase ( "a", "{abcd\"efgh", "{", Result = false )]
		[ExpectedException ( ExpectedMessage = "The marker string has to have an even number of occurrences" )]
		public bool IsBetween_Tests_02 ( string sut, string search, string marker )
		{
			return sut.IsBetween ( search, marker );
		}


		// ---------------------------------------------------------------------------------


		[Test]
		public void ReplaceExceptThoseInBetween ()
		{
			var sut = " x { 'y adda' : 'wei', 'you what?' : 'yeah I did it ' } xxx";
			var rep = sut.Replace ( " ", "", ( x, y ) => x.IsBetween ( sut, "'", y ) );
			Assert.IsTrue ( rep == " x {'y adda':'wei','you what?':'yeah I did it '} xxx", "Got this -> " + rep );
		}



		// ---------------------------------------------------------------------------------


		[Test]
		public void SplitCollectionTests_01 ()
		{

			var _list = new int [] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 0 };
			var _split = _list.Split ( 2 );

			Assert.That ( _split.Count () == 5, "has " + _split.Count () + " elements" );
			Assert.That ( _split.ElementAt ( 2 ).First () == 5 );
			Assert.That ( _split.Last ().Last () == 0 );

		}


		// ---------------------------------------------------------------------------------


		[Test]
		public void SplitCollectionTests_02 ()
		{

			var _list = new int [] { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
			var _split = _list.Split ( 3 );

			Assert.That ( _split.Count () == 3, "has " + _split.Count () + " elements" );
			Assert.That ( _split.ElementAt ( 2 ).First () == 7 );
			Assert.That ( _split.Last ().Last () == 9 );
			Assert.That ( _split.First ().Last () == 3 );

		}


		// ---------------------------------------------------------------------------------


		[Test]
		public void SplitCollectionTests_03 ()
		{

			var _list = new int [] { 1, 2, 3, 4, 5, 6, 7, 8 };
			var _split = _list.Split ( 3 );

			Assert.That ( _split.Count () == 3, "has " + _split.Count () + " elements" );
			Assert.That ( _split.ElementAt ( 2 ).First () == 7 );
			Assert.That ( _split.Last ().Last () == 8 );
			Assert.That ( _split.First ().Last () == 3 );

		}


		// ---------------------------------------------------------------------------------


		[Test]
		[ExpectedException ( ExpectedMessage = "A collection has to have items in order to be split" )]
		public void SplitCollectionTests_04 ()
		{

			List<Int32> _list = null;
			var _split = _list.Split ( 3 );

		}

		[Test]
		[ExpectedException ( ExpectedMessage = "A collection has to have items in order to be split" )]
		public void SplitCollectionTests_05 ()
		{
			List<Int32> _list = new List<int> ();
			var _split = _list.Split ( 3 );
		}
		[Test]
		[ExpectedException ( ExpectedMessage = "Invalid split length" )]
		public void SplitCollectionTests_06 ()
		{
			var _list = new [] { 1, 2, 3 };
			var _split = _list.Split ( 4 );
		}
		[Test]
		[ExpectedException ( ExpectedMessage = "Invalid split length" )]
		public void SplitCollectionTests_07 ()
		{
			var _list = new [] { 1, 2, 3 };
			var _split = _list.Split ( -4 );
		}

		[Test]
		public void SplitCollectionTests_08 ()
		{
			var _list = new [] { 1, 2, 3 };
			var _split = _list.Split ( 3 );
			Assert.That ( _split.Count () == 1, "has " + _split.Count () + " elements" );
			Assert.That ( _split.Last ().Last () == 3 );
			Assert.That ( _split.First ().Last () == 3 );
			Assert.That ( _split.First ().ElementAt ( 1 ) == 2 );
		}


		// ---------------------------------------------------------------------------------


		[TestCase ( "1,10,30,40,50,60,70,80", 9, 2, Result = true )]
		[TestCase ( "1,10,30,40,50,60,70,80", 29, 2, Result = false )]
		[TestCase ( "1,10,30,40,50,60,70,80", 219, 2, Result = false )]
		[TestCase ( "1,10,30,40,50,60,70,80", 39, 2, Result = true )]
		[TestCase ( "1,10,30,40,50,60,70,80", 79, 2, Result = true )]

		[TestCase ( "1,10,30,40,50,60,70,80", 9, 3, Result = true )]
		[TestCase ( "1,10,30,40,50,60,70,80", 29, 3, Result = true )]
		[TestCase ( "1,10,30,40,50,60,70,80", 219, 3, Result = false )]
		[TestCase ( "1,10,30,40,50,60,70,80", 31, 3, Result = false )]
		[TestCase ( "1,10,30,40,50,60,70,80", 39, 3, Result = false )]
		[TestCase ( "1,10,30,40,50,60,70,80", 79, 3, Result = true )]

		public bool IsBetweenAny_Int16Tests ( string searchArray, Int16 value, int numSplits )
		{
			var _indices = searchArray.Split ( new char [] { ',' },
				StringSplitOptions.RemoveEmptyEntries ).Select ( x => Convert.ToInt16 ( x ) );
			return value.InBetweenAny ( _indices.Split ( numSplits ) );
		}

		[TestCase ( "1,10,30,40,50,60,70,80", 9, 2, Result = true )]
		[TestCase ( "1,10,30,40,50,60,70,80", 29, 2, Result = false )]
		[TestCase ( "1,10,30,40,50,60,70,80", 219, 2, Result = false )]
		[TestCase ( "1,10,30,40,50,60,70,80", 39, 2, Result = true )]
		[TestCase ( "1,10,30,40,50,60,70,80", 79, 2, Result = true )]

		[TestCase ( "1,10,30,40,50,60,70,80", 9, 3, Result = true )]
		[TestCase ( "1,10,30,40,50,60,70,80", 29, 3, Result = true )]
		[TestCase ( "1,10,30,40,50,60,70,80", 219, 3, Result = false )]
		[TestCase ( "1,10,30,40,50,60,70,80", 31, 3, Result = false )]
		[TestCase ( "1,10,30,40,50,60,70,80", 39, 3, Result = false )]
		[TestCase ( "1,10,30,40,50,60,70,80", 79, 3, Result = true )]

		public bool IsBetweenAny_Int32Tests ( string searchArray, Int32 value, int numSplits )
		{
			var _indices = searchArray.Split ( new char [] { ',' },
				StringSplitOptions.RemoveEmptyEntries ).Select ( x => Convert.ToInt32 ( x ) );
			return value.InBetweenAny ( _indices.Split ( numSplits ) );
		}

		[TestCase ( "1,10,30,40,50,60,70,80", 9, 2, true, 1, 10, Result = true )]
		[TestCase ( "1,10,30,40,50,60,70,80", 29, 2, false, 0, 0, Result = false )]
		[TestCase ( "1,10,30,40,50,60,70,80", 219, 2, false, 0, 0, Result = false )]
		[TestCase ( "1,10,30,40,50,60,70,80", 39, 2, true, 30, 40, Result = true )]
		[TestCase ( "1,10,30,40,50,60,70,80", 79, 2, true, 70, 80, Result = true )]

		[TestCase ( "1,10,30,40,50,60,70,80", 9, 3, true, 1, 30, Result = true )]
		[TestCase ( "1,10,30,40,50,60,70,80", 29, 3, true, 1, 30, Result = true )]
		[TestCase ( "1,10,30,40,50,60,70,80", 219, 3, false, 0, 0, Result = false )]
		[TestCase ( "1,10,30,40,50,60,70,80", 31, 3, false, 0, 0, Result = false )]
		[TestCase ( "1,10,30,40,50,60,70,80", 39, 3, false, 0, 0, Result = false )]
		[TestCase ( "1,10,30,40,50,60,70,80", 79, 3, true, 70, 80, Result = true )]

		public bool IsBetweenAny_Int32Tests_WithOut ( string searchArray, Int32 value, int numSplits, bool shouldReturnInterval, int intervalMin, int intervalMax )
		{
			var _indices = searchArray.Split ( new char [] { ',' },
				StringSplitOptions.RemoveEmptyEntries ).Select ( x => Convert.ToInt32 ( x ) );
			IEnumerable<int> chosenInterval = new List<int> ();
			var ret = value.InBetweenAny ( _indices.Split ( numSplits ), out chosenInterval );
			if ( shouldReturnInterval )
				Assert.That ( chosenInterval.Min () == intervalMin && chosenInterval.Max () == intervalMax );
			else
				Assert.IsNull ( chosenInterval );
			return ret;
		}

		[TestCase ( "1,10,30,40,50,60,70,80", 9, 2, Result = true )]
		[TestCase ( "1,10,30,40,50,60,70,80", 1, 2, Result = true )]
		[TestCase ( "1,10,30,40,50,60,70,80", 10, 2, Result = true )]
		[TestCase ( "1,10,30,40,50,60,70,80", 1, 2, Result = true )]
		[TestCase ( "1,10,30,40,50,60,70,80", 29, 2, Result = false )]
		[TestCase ( "1,10,30,40,50,60,70,80", 219, 2, Result = false )]
		[TestCase ( "1,10,30,40,50,60,70,80", 39, 2, Result = true )]
		[TestCase ( "1,10,30,40,50,60,70,80", 79, 2, Result = true )]

		[TestCase ( "1,10,30,40,50,60,70,80", 9, 3, Result = true )]
		[TestCase ( "1,10,30,40,50,60,70,80", 1, 2, Result = true )]
		[TestCase ( "1,10,30,40,50,60,70,80", 29, 3, Result = true )]
		[TestCase ( "1,10,30,40,50,60,70,80", 80, 3, Result = true )]
		[TestCase ( "1,10,30,40,50,60,70,80", 219, 3, Result = false )]
		[TestCase ( "1,10,30,40,50,60,70,80", 31, 3, Result = false )]
		[TestCase ( "1,10,30,40,50,60,70,80", 39, 3, Result = false )]
		[TestCase ( "1,10,30,40,50,60,70,80", 79, 3, Result = true )]

		public bool IsBetweenAny_Int64Tests ( string searchArray, Int64 value, int numSplits )
		{
			var _indices = searchArray.Split ( new char [] { ',' },
				StringSplitOptions.RemoveEmptyEntries ).Select ( x => Convert.ToInt64 ( x ) );
			return value.InBetweenAny ( _indices.Split ( numSplits ) );
		}

		[TestCase ( "1,1|10,2|	30,03|40,09|	50,25|60,00|	70,01|80,33", 9, 2, 2, Result = true )]
		[TestCase ( "1,1|10,2|	30,03|40,09|	50,25|60,00|	70,01|80,33", 1.1F, 2, 2, Result = true )]
		[TestCase ( "1,1|10,2|	30,03|40,09|	50,25|60,00|	70,01|80,33", 10.2F, 2, 2, Result = true )]
		[TestCase ( "1,1|10,2|	30,03|40,09|	50,25|60,00|	70,01|80,33", 1.0F, 2, 2, Result = false )]
		[TestCase ( "1,1|10,2|	30,03|40,09|	50,25|60,00|	70,01|80,33", 50.24F, 2, 2, Result = false )]
		[TestCase ( "1,1|10,2|	30,03|40,09|	50,25|60,00|	70,01|80,33", 50.25F, 2, 2, Result = true )]
		[TestCase ( "1,1|10,2|	30,03|40,09|	50,25|60,00|	70,01|80,33", 50.26F, 2, 2, Result = true )]
		[TestCase ( "1,1|10,2|30,03|		40,09|50,25|60,00|	70,01|80,33", 50.26F, 3, 2, Result = true )]
		[TestCase ( "1,1|10,2|30,03|		40,09|50,25|60,00|	70,01|80,33", 32, 3, 2, Result = false )]
		[TestCase ( "1,1|10,2|30,03|		40,09|50,25|60,00|	70,01|80,33", 60.0001F, 3, 4, Result = false )]
		[TestCase ( "1,1|10,2|30,03|		40,09|50,25|60,00|	70,01|80,33", 60F, 3, 2, Result = true )]
		[TestCase ( "1,1|10,2|30,03|		40,09|50,25|60,00|	70,01|80,33", 80.33F, 3, 2, Result = true )]
		[TestCase ( "1,1|10,2|30,03|		40,09|50,25|60,00|	70,01|80,33", 80.341F, 3, 2, Result = false )]
		public bool IsBetweenAny_FloatTests ( string searchArray, double value, int numSplits, int precision = 2 )
		{
			var _indices = searchArray.Split ( new char [] { '|' },
				StringSplitOptions.RemoveEmptyEntries ).Select ( x => double.Parse ( x.Trim () ) );
			return value.InBetweenAny ( _indices.Split ( numSplits ), precision );
		}


		// ---------------------------------------------------------------------------------


		[TestCase ( "1-234-1-567-1-233-1", "1", "1", false, Result = "-234-" )]
		//[TestCase ( "1-234-1-567-1-233-1", "1", "1", true, Result = "1-234-1" )]
		[TestCase ( "{-234-}{-567-}{-233-}", "{", "}", false, Result = "-234-" )]
		[TestCase ( "{-234-}{-567-}{-233-}", "{", "}", true, Result = "{-234-}" )]
		[TestCase ( "the sequence starts with {-234-} goes on with {-567-} and dies off with {-233-}", "{", "}", true, Result = "{-234-}" )]
		[TestCase ( "the sequence starts with {-234-} goes on with {-567-} and dies off with {-233-}", "{", "}", false, Result = "-234-" )]
		public string FindAllBetween ( string sut, string a, string b, bool includeMarkers )
		{
			var _res = sut.FindAllBetween ( a, b, includeMarkers );
			Assert.That ( _res.Count () == 3 );
			return _res.First ();
		}

		[TestCase ( "load data from DATASOURCE.WORLD_COUNTRIES with arguments {\"Country\" : \"ES\",\"Active\" : true, \"Age\":30, \"Title\" : \"Anything Goes\" }", "{", "}", true,
			Result = "{\"Country\" : \"ES\",\"Active\" : true, \"Age\":30, \"Title\" : \"Anything Goes\" }" )]
		public string FindAllBetween_02 ( string sut, string a, string b, bool includeMarkers )
		{
			var _res = sut.FindAllBetween ( a, b, includeMarkers );
			Assert.That ( _res.Count () == 1 );
			return _res.First ();
		}


		// ---------------------------------------------------------------------------------


		[Test]
		public void ElementsAt_Test01 ()
		{
			var _list = new [] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 0 };
			var res = _list.ElementsAt ( new [] { 2, 4, 5, 8 } );
			Assert.That ( res.ElementAt ( 0 ) == 3 );
			Assert.That ( res.ElementAt ( 1 ) == 5 );
			Assert.That ( res.ElementAt ( 2 ) == 6 );
			Assert.That ( res.Last () == 9 );
		}


		[Test]
		public void ElementsAt_Test02 ()
		{
			var _list = new [] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 0 };
			var res = _list.ElementsAt ( x => x % 3 == 0 );
			Assert.That ( res.ElementAt ( 0 ) == 3 );
			Assert.That ( res.ElementAt ( 1 ) == 6 );
			Assert.That ( res.ElementAt ( 2 ) == 9 );
			Assert.That ( res.Last () == 0 );
		}


		// ---------------------------------------------------------------------------------


		[Test]
		public void IndicesOfAll_Overload_Tests ()
		{
			var sut = "In this string there is a { and there is also a [";
			var __indices = sut.IndicesOfAll ( new [] { "{", "[", "is" } );
			Assert.That ( __indices.Count () == 5 );

			Assert.That ( __indices.ElementAt ( 0 ) == 26 );
			Assert.That ( __indices.ElementAt ( 1 ) == 48 );
			Assert.That ( __indices.ElementAt ( 2 ) == 5 );
			Assert.That ( __indices.ElementAt ( 3 ) == 21 );
			Assert.That ( __indices.ElementAt ( 4 ) == 38 );

		}


		[Test]
		public void IndicesOfAll2_Tests ()
		{
			var sut = "In this string there is a { and there is also a [";
			var __indices = sut.IndicesOfAll2 ( new [] { "{", "[", "is" } );
			Assert.That ( __indices.Count () == 5 );

			Assert.That ( __indices.ElementAt ( 0 ).Key == 26 );
			Assert.That ( __indices.ElementAt ( 1 ).Key == 48 );
			Assert.That ( __indices.ElementAt ( 2 ).Key == 5 );
			Assert.That ( __indices.ElementAt ( 3 ).Key == 21 );
			Assert.That ( __indices.ElementAt ( 4 ).Key == 38 );

			Assert.That ( __indices.ElementAt ( 0 ).Value == "{" );
			Assert.That ( __indices.ElementAt ( 1 ).Value == "[" );
			Assert.That ( __indices.ElementAt ( 2 ).Value == "is" );
			Assert.That ( __indices.ElementAt ( 3 ).Value == "is" );
			Assert.That ( __indices.ElementAt ( 4 ).Value == "is" );

		}


		// ---------------------------------------------------------------------------------


		[Test]
		public void Split2_NormalSplit_Tests ()
		{
			var sut = "Normal split with plain old spaces";
			var spl = sut.Split2 ( new [] { " " } );
			Assert.That ( spl.Count () == 6 );
			Assert.That ( spl.First () == "Normal" );
			Assert.That ( spl.Last () == "spaces" );
		}

		[Test]
		public void Split2_NormalSplit_Tests_02 ()
		{
			var sut = "Normal split with plain old spaces and,some,commas";
			// 0    5   10   15   20   25   30   35    40    45    
			var spl = sut.Split2 ( new [] { " ", "," } );
			Assert.That ( spl.Count () == 9 );
			Assert.That ( spl.First () == "Normal" );
			Assert.That ( spl.Last () == "commas" );
		}

		[Test]
		public void Split2_NormalSplit_Tests_03 ()
		{
			var sut = "What is what is oh is this";
			var spl = sut.Split2 ( new [] { "is" }, StringSplitOptions.RemoveEmptyEntries, true );
			Assert.That ( spl.Count () == 4 );
			Assert.That ( spl.First () == "What" );
			Assert.That ( spl.Last () == "th" );
		}

		[Test]
		public void Split2_NormalSplit_Tests_04 ()
		{
			var sut = "What is what is oh is this";
			var spl = sut.Split2 ( new [] { "is" }, StringSplitOptions.RemoveEmptyEntries, false );
			Assert.That ( spl.Count () == 4 );
			Assert.That ( spl.First () == "What " );
			Assert.That ( spl.Last () == " th" );
		}


		[Test]
		public void Split2_NormalSplit_Tests_05 ()
		{
			var sut = "What is what is oh is this";
			var spl = sut.Split2 ( new [] { "is" } );
			Assert.That ( spl.Count () == 5 );
			Assert.That ( spl.First () == "What " );
			Assert.That ( spl.Last () == "" );
		}

		[Test]
		public void Split2_NormalSplit_Tests_06 ()
		{
			var sut = "What is what is oh is this";
			var spl = sut.Split2 ( new [] { "is" }, StringSplitOptions.None, true );
			Assert.That ( spl.Count () == 5 );
			Assert.That ( spl.First () == "What" );
			Assert.That ( spl.Last () == "" );
		}

		// ---------------------------------------------------------------------------------

		[Test]
		public void Split2_SplitWithExclusion_Tests ()
		{
			var sut = "Normal split with plain old spaces but { this should not be split } since it is between brackets";
			var spl = sut.Split2 ( new [] { " " }, "{", "}" );
			Assert.That ( spl.Count () == 13 );
			Assert.That ( spl.First () == "Normal" );
			Assert.That ( spl.Last () == "brackets" );
		}

		[Test]
		public void Split2_SplitWithExclusion_Tests_02 ()
		{
			var sut = "Normal split with plain old spaces but { this should not be split } since it is  { between brackets }";
			var spl = sut.Split2 ( new [] { " " }, "{", "}", StringSplitOptions.RemoveEmptyEntries, true );
			Assert.That ( spl.Count () == 12 );
			Assert.That ( spl.First () == "Normal" );
			Assert.That ( spl.Last () == "{ between brackets }" );
		}


		// ---------------------------------------------------------------------------------


		[Test]
		public void Split2_SplitWithExclusion_Tests_03 ()
		{
			var sut = "Normal split with plain old spaces but { this should not be split } since it is between brackets";
			var separators = new List<Tuple<string, string>> ();
			separators.Add ( new Tuple<string, string> ( "{", "}" ) );
			separators.Add ( new Tuple<string, string> ( "[", "]" ) );
			var spl = sut.Split2 ( new [] { " " }, separators);
			Assert.That ( spl.Count () == 13 );
			Assert.That ( spl.First () == "Normal" );
			Assert.That ( spl.Last () == "brackets" );
		}

		[Test]
		public void Split2_SplitWithExclusion_Tests_04 ()
		{
			var sut = "Normal split with plain old spaces but { this should not be split } since it is  { between brackets }";
			var separators = new List<Tuple<string, string>> ();
			separators.Add ( new Tuple<string, string> ( "{", "}" ) );
			separators.Add ( new Tuple<string, string> ( "[", "]" ) );
			var spl = sut.Split2 ( new [] { " " }, separators, StringSplitOptions.RemoveEmptyEntries, true );
			Assert.That ( spl.Count () == 12 );
			Assert.That ( spl.First () == "Normal" );
			Assert.That ( spl.Last () == "{ between brackets }" );
		}


		// ---------------------------------------------------------------------------------

		[Test]
		public void Split2_SplitWithExclusion_Tests_05 ()
		{
			// the sexorcisto and the yeezusfuckenchrist-o
			var sut = "Normal split with plain old spaces but { this should not be split } since it is  [ between brackets ]";
			var separators = new List<Tuple<string, string>> ();
			separators.Add ( new Tuple<string, string> ( "{", "}" ) );
			separators.Add ( new Tuple<string, string> ( "[", "]" ) );
			var spl = sut.Split2 ( new [] { " " }, separators, StringSplitOptions.RemoveEmptyEntries, true );
			Assert.That ( spl.Count () == 12 );
			Assert.That ( spl.First () == "Normal" );
			Assert.That ( spl.Last () == "[ between brackets ]" );
		}

		[Test]
		public void Split2_SplitWithExclusion_Tests_06 ()
		{
			// the sexorcisto and the yeezusfuckenchrist-o
			var sut = "Normal split with /*plain old spaces*/ but { this should not be split } since it is  [ between brackets ]";
			var separators = new List<Tuple<string, string>> ();
			separators.Add ( new Tuple<string, string> ( "{", "}" ) );
			separators.Add ( new Tuple<string, string> ( "[", "]" ) );
			separators.Add ( new Tuple<string, string> ( "/*", "*/" ) );
			var spl = sut.Split2 ( new [] { " " }, separators, StringSplitOptions.RemoveEmptyEntries, true );
			Assert.That ( spl.Count () == 10 );
			Assert.That ( spl.First () == "Normal" );
			Assert.That ( spl.Last () == "[ between brackets ]" );
		}
		// ---------------------------------------------------------------------------------

	}
}
