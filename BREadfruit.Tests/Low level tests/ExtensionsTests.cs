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

		public string RemoveBetweenTests ( string original, string removee, string s1, string s2 )
		{
			return original.RemoveBetween ( removee, s1, s2 );
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
		public string TakeUntilTests( string sut )
		{
			LineInfo li = new LineInfo ( sut );
			var reduced = li.Tokens.TakeUntil ( x => x.Token.EndsWith ( Grammar.ClosingSquareBracket.Token ) );
			return reduced.JoinTogether ().Token;
		}


		// ---------------------------------------------------------------------------------


		//[TestCase ( "", Result = "" )]
		//[TestCase ( "[]", Result = "[]" )]
		//[TestCase ( "[", Result = "[" )]
		//[TestCase ( "set value [LU + GD_Ctr_PIVA.Value] in GD_Ctr_VATRegistrationNumber", Result = "set value [LU + GD_Ctr_PIVA.Value]" )]
		//[TestCase ( "set value LU + GD_Ctr_PIVA.Value in GD_Ctr_VATRegistrationNumber", Result = "set value LU + GD_Ctr_PIVA.Value in GD_Ctr_VATRegistrationNumber" )]
		//public string RemoveAfterTest ( string sut )
		//{
		//	LineInfo li = new LineInfo ( sut );
		//	li.Tokens.RemoveAfter ( x => x.Token.EndsWith ( Grammar.ClosingSquareBracket.Token ) );
		//	return li.Tokens.JoinTogether ().Token;
		//}


		// ---------------------------------------------------------------------------------

	}
}
