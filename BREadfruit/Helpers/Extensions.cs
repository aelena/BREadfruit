using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BREadfruit.Helpers
{
	/// <summary>
	/// Provides a series of useful extension methods used during all 
	/// manner of parsing operations.
	/// </summary>
	public static class Extensions
	{
		/// <summary>
		/// Joins together a series of Symbols. 
		/// This is basically a matter of concatenating their tokens
		/// </summary>
		/// <param name="series"></param>
		/// <returns></returns>
		public static Symbol JoinTogether ( this IEnumerable<Symbol> series )
		{
			if ( series == null )
				throw new ArgumentNullException ( "series", "Cannot join together a null series of symbols" );

			string _s = String.Empty;
			foreach ( var s in series )
				_s += s.Token + " ";

			if ( _s.Count () > 0 )
				// we return a new symbol with the concatenated representation
				// assuming we take the indent level of the first in the series
				// (maybe we should check all symbols in the series have the same indentlevel)
				// and the isTerminal value for the last in the series
				return new Symbol ( _s.Trim (), series.First ().IndentLevel, series.Last ().IsTerminal );

			return Grammar.EmptySymbol;
		}


		// ---------------------------------------------------------------------------------


		/// <summary>
		/// Joins together a series of Symbols. 
		/// This is basically a matter of concatenating their tokens
		/// </summary>
		/// <param name="series"></param>
		/// <returns></returns>
		public static Symbol JoinTogetherBetween ( this IEnumerable<Symbol> series, int from, int to )
		{
			if ( series == null )
				throw new ArgumentNullException ( "Cannot join together a null series of symbols", "series" );

			if ( from < 0 )
				throw new ArgumentException ( "Cannot specify a starting position lower than 0", "from" );

			if ( to < from )
				throw new ArgumentException ( "Cannot specify a upper index lower than the starting index", "to" );


			series = series.From ( from ).To ( to - from );

			string _s = String.Empty;
			foreach ( var s in series )
				_s += s.Token + " ";

			// we return a new symbol with the concatenated representation
			// assuming we take the indent level of the first in the series
			// (maybe we should check all symbols in the series have the same indentlevel)
			// and the isTerminal value for the last in the series
			return new Symbol ( _s.Trim (), series.First ().IndentLevel, series.Last ().IsTerminal );

		}


		// ---------------------------------------------------------------------------------


		/// <summary>
		/// Extension to Take where a caller can indicate from and to parameters.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="list"></param>
		/// <param name="from"></param>
		/// <param name="to"></param>
		/// <returns></returns>
		public static IEnumerable<T> Take<T> ( this IEnumerable<T> list, int from, int to )
		{
			return list.ToList ().GetRange ( from, ( to - from ) + 1 );
		}


		// ---------------------------------------------------------------------------------


		/// <summary>
		/// Returns a subset of an instance of IEnumerable starting from the "from" position.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="list"></param>
		/// <param name="from"></param>
		/// <returns></returns>
		public static IEnumerable<T> From<T> ( this IEnumerable<T> list, int from )
		{
			return list.ToList ().GetRange ( from, ( list.Count () - from ) );
		}


		// ---------------------------------------------------------------------------------


		/// <summary>
		/// Returns from 0 to the "to" position on a IEnumerable instance.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="list"></param>
		/// <param name="to"></param>
		/// <returns></returns>
		public static IEnumerable<T> To<T> ( this IEnumerable<T> list, int to )
		{
			return list.ToList ().GetRange ( 0, ++to );
		}


		// ---------------------------------------------------------------------------------


		/// <summary>
		/// Extension method to see if any of the 'searches' elements
		/// is contained in the list.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="value"></param>
		/// <param name="searches"></param>
		/// <returns></returns>
		public static bool Contains ( this string value, IEnumerable<string> searches )
		{
			if ( value == null )
				throw new ArgumentNullException ( "list", "the list cannot be null." );

			if ( searches != null )
				return searches.All ( x => value.IndexOf ( x ) >= 0 );

			return false;
		}


		// ---------------------------------------------------------------------------------


		/// <summary>
		/// Extension method to see if any of the 'searches' elements
		/// is contained in the list.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="list"></param>
		/// <param name="searches"></param>
		/// <returns></returns>
		public static bool ContainsAny<T> ( this IEnumerable<T> list, IEnumerable<T> searches )
		{
			if ( list == null )
				throw new ArgumentNullException ( "list", "the list cannot be null." );

			if ( searches != null )
				foreach ( var s in searches )
					if ( list.Contains ( s ) )
						return true;

			return false;
		}


		// ---------------------------------------------------------------------------------


		/// <summary>
		/// Extension method to see if any of the 'searches' elements
		/// is contained in the list.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="list"></param>
		/// <param name="searches"></param>
		/// <returns>
		/// Returns a Tuple<bool,object> indicating true or false if an item was found or not
		/// and if it was, then the second item in the tuple, the object, will contain that value.
		/// </returns>
		public static Tuple<bool, object> ContainsAny2<T> ( this IEnumerable<T> list, IEnumerable<T> searches )
		{
			if ( list == null )
				throw new ArgumentNullException ( "list", "the list cannot be null." );

			if ( searches != null )
				foreach ( var s in searches )
					if ( list.Contains ( s ) )
						return new Tuple<bool, object> ( true, s );
			return new Tuple<bool, object> ( false, null );
		}

		// ---------------------------------------------------------------------------------


		/// <summary>
		/// Extension method to see if any of the 'searches' elements
		/// is contained in the list.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="list"></param>
		/// <param name="searches"></param>
		/// <returns></returns>
		public static bool ContainsAny ( this string searchee, IEnumerable<string> searches, bool wordBoundaries = false )
		{
			if ( searchee == null )
				throw new ArgumentNullException ( "searchee", "the string cannot be null." );
			if ( !wordBoundaries )
			{
				if ( searches != null )
					foreach ( var s in searches )
						if ( searchee.Contains ( s ) )
							return true;
			}
			else
			{
				if ( searches != null )
				{
					foreach ( var s in searches )
					{
						var _rx = "\\b" + s + "\\b";
						var _matches = Regex.Matches ( searchee, _rx );
						if ( _matches != null && _matches.Count > 0 )
							return true;
					}
				}
			}
			return false;

		}


		// ---------------------------------------------------------------------------------

		/// <summary>
		/// Extension method to see if any of the 'searches' elements
		/// is contained in the list.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="list"></param>
		/// <param name="searches"></param>
		/// <returns>Returns a boolean value to indicate if the search was successful
		/// and returns also the value itself.</returns>
		public static Tuple<bool, string> ContainsAny2 ( this string searchee, IEnumerable<string> searches, bool wordBoundaries = false )
		{
			if ( searchee == null )
				throw new ArgumentNullException ( "searchee", "the string cannot be null." );

			if ( !wordBoundaries )
			{
				if ( searches != null )
					foreach ( var s in searches )
						if ( searchee.Contains ( s ) )
							return new Tuple<bool, string> ( true, s );
			}
			else
			{
				if ( searches != null )
				{
					foreach ( var s in searches )
					{
						var _rx = "\\b" + s + "\\b";
						var _matches = Regex.Matches ( searchee, _rx );
						if ( _matches != null && _matches.Count > 0 )
							return new Tuple<bool, string> ( true, s );

					}
				}
			}

			return new Tuple<bool, string> ( false, String.Empty );

		}



		// ---------------------------------------------------------------------------------


		/// <summary>
		/// Prepends a given string to another string a n number of times 
		/// (by default 1).
		/// </summary>
		/// <param name="s"></param>
		/// <param name="prependValue"></param>
		/// <param name="repetitions"></param>
		/// <returns></returns>
		public static string Prepend ( this string s, string prependValue, int repetitions = 1 )
		{
			if ( s == null )
				s = "";
			if ( prependValue == null )
				prependValue = "";

			if ( repetitions < 0 )
				repetitions = 1;

			for ( int i = 0; i < repetitions; i++ )
				s = prependValue + s;

			return s;
		}

		// ---------------------------------------------------------------------------------


		/// <summary>
		/// Gets the penultimate element in a list.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="list"></param>
		/// <returns></returns>
		public static T Penultimate<T> ( this IEnumerable<T> list ) where T : class
		{

			if ( list != null )
				return list.ElementAt ( list.Count () - 2 );

			return null;

		}


		// ---------------------------------------------------------------------------------


		/// <summary>
		/// Returns the element that sits in the indicated position,
		/// starting from 1 and starting from the back of the collection.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="list"></param>
		/// <param name="index"></param>
		/// <returns></returns>
		public static T ElementAtFromLast<T> ( this IEnumerable<T> list, int index ) where T : class
		{
			if ( list != null )
				return list.Reverse ().ElementAt ( index - 1 );
			return null;

		}

		// ---------------------------------------------------------------------------------


		/// <summary>
		/// Checks if an element of type T exists in a list of Ts.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="value"></param>
		/// <param name="list"></param>
		/// <returns></returns>
		public static bool In<T> ( this T value, IEnumerable<T> list )
		{
			if ( list == null )
				return false;
			return list.Contains ( value );
		}

		// ---------------------------------------------------------------------------------


		/// <summary>
		/// Returns a boolean value to tell if the instance is null or empty.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="list"></param>
		/// <returns></returns>
		public static bool IsNullOrEmpty<T> ( this IEnumerable<T> list )
		{
			if ( list == null )
				return true;
			return list.Count () == 0;
		}


		// ---------------------------------------------------------------------------------


		public static IEnumerable<T> TakeAfter<T> ( this IEnumerable<T> list, Func<T, bool> func )
		{
			int i = 0;
			foreach ( var l in list )
			{
				if ( func ( l ) )
				{
					return list.Skip ( ++i );
				}
				i++;
			}
			return null;
		}


		// ---------------------------------------------------------------------------------


		public static IEnumerable<T> TakeUntil<T> ( this IEnumerable<T> list, Func<T, bool> func )
		{
			int i = 0;
			foreach ( var l in list )
			{
				if ( func ( l ) )
				{
					return list.Take ( ++i );
				}
				i++;
			}
			return list;
		}


		// ---------------------------------------------------------------------------------


		/// <summary>
		/// Returns the index of the first element that satisfies the function.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="list"></param>
		/// <param name="func"></param>
		/// <returns></returns>
		public static int IndexOf<T> ( this IEnumerable<T> list, Func<T, bool> func )
		{
			if ( list == null )
				throw new ArgumentNullException ( "list", "The list to be searched cannot be null" );
			if ( func == null )
				throw new ArgumentNullException ( "func", "The search function cannot be null" );

			int i = 0;
			foreach ( var l in list )
			{
				if ( func ( l ) )
				{
					return i;
				}
				i++;
			}
			return -1;
		}


		// ---------------------------------------------------------------------------------


		/// <summary>
		/// Returns the index of the first element that satisfies the function
		/// but it performs the search only after a certain index.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="list"></param>
		/// <param name="func"></param>
		/// <returns></returns>
		/// <exception cref="ArgumentNullException" />
		/// <exception cref="ArgumentException" />
		public static int IndexOf<T> ( this IEnumerable<T> list, Func<T, bool> func, int afterIndex )
		{

			if ( list == null )
				throw new ArgumentNullException ( "list", "The list to be searched cannot be null" );
			if ( afterIndex < 0 )
				throw new ArgumentException ( "afterIndex", "The index from which to perform the search cannot be lower than zero" );
			if ( afterIndex > list.Count () )
				throw new ArgumentException ( "afterIndex", "The index from which to perform the search cannot greater than the number of elements in the list" );

			int i = 0;
			foreach ( var l in list.Skip ( afterIndex ) )
			{
				if ( func ( l ) )
				{
					return i;
				}
				i++;
			}
			return -1;
		}


		// ---------------------------------------------------------------------------------


		/// <summary>
		/// Finds and returns the nth element in the collection that satisfies the function. 
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="list"></param>
		/// <param name="func"></param>
		/// <returns></returns>
		public static T FindNth<T> ( this IEnumerable<T> list, Func<T, bool> func, int index )
		{

			if ( list == null )
				throw new ArgumentNullException ( "list", "The list to be searched cannot be null" );
			if ( func == null )
				throw new ArgumentNullException ( "func", "The search function cannot be null" );
			if ( index >= list.Count () )
				throw new ArgumentException ( "The index cannot greater than the number of elements in the list", "index" );

			var _all = list.Where ( func );
			if ( _all.HasItems () && index < list.Count () )
			{
				return _all.ElementAt ( index );
			}
			// do not like this for value types....
			return default ( T );
		}


		// ---------------------------------------------------------------------------------


		/// <summary>
		/// Return the indices of all elements in the collection that satisfy
		/// the function passed.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="list"></param>
		/// <param name="func"></param>
		/// <returns></returns>
		public static IEnumerable<int> IndicesOf<T> ( this IEnumerable<T> list, Func<T, bool> func )
		{
			if ( list == null )
				throw new ArgumentNullException ( "list", "The list to be searched cannot be null" );
			if ( func == null )
				throw new ArgumentNullException ( "func", "The search function cannot be null" );
			var _indices = new List<int> ();
			var i = 0;
			foreach ( var l in list )
			{
				if ( func ( l ) )
					_indices.Add ( i );
				++i;
			}

			return _indices;
		}


		// ---------------------------------------------------------------------------------


		public static int LengthSafe ( this string value )
		{
			return value == null ? 0 : value.Length;
		}

		// ---------------------------------------------------------------------------------


		public static IEnumerable<T> RemoveAfter<T> ( this IEnumerable<T> list, Func<T, bool> func )
		{
			int i = 0;
			foreach ( var l in list )
			{
				if ( func ( l ) )
				{
					return list.Take ( ++i );
				}
				i++;
			}
			return list;
		}


		// ---------------------------------------------------------------------------------

		/// <summary>
		/// This extension removes n number of characters starting from the end of the string.
		/// If the string is null or empty, it returns String.Empty, therefore it does not crash on null.
		/// 
		/// If the number of characters to substract from the end is bigger that the actual
		/// character count in the string it returns String.Empty, therefore it does not crash.
		/// 
		/// Caller can specify in a end trim is wanted or not, in case whitespace needs to be preserved.
		/// By default this is assumed to be false, so pass true for the trimming to take effect.
		/// </summary>
		/// <param name="value">String on which to perform the operation.</param>
		/// <param name="numberOfCharacters">Number of characters to remove from the end of the string.</param>
		/// <param name="trimEndFirst">Boolean value that indicates if a TrimEnd is to be done 
		/// before the removal.</param>
		/// <returns></returns>
		public static string RemoveFromEnd ( this string value, int numberOfCharacters, bool trimEndFirst = false )
		{
			if ( String.IsNullOrEmpty ( value ) )
				return string.Empty;

			if ( trimEndFirst )
				value = value.TrimEnd ();

			if ( numberOfCharacters > value.Length )
				return String.Empty;

			return value.Remove ( value.Length - numberOfCharacters );

		}

		// ---------------------------------------------------------------------------------


		/// <summary>
		/// Makes a substring operation where the criteria is that the
		/// substring starts from the end of the first occurrence of the mark string.
		/// 
		/// So in a string like 'the quick brown fox jumps over the lazy dog' 
		/// where the mark string is 'fox', returns ' jumps over the lazy dog'
		/// including the white space or not depending on the optional parameter, 
		/// for which the default is false.
		/// </summary>
		/// <param name="value"></param>
		/// <param name="markString"></param>
		/// <param name="trimResults"></param>
		/// <returns></returns>
		public static string SubStringAfter ( this string value, string markString, StringComparison comparisonOptions = StringComparison.CurrentCulture, bool trimResults = false )
		{
			if ( String.IsNullOrEmpty ( value ) )
				return String.Empty;

			var _index = value.IndexOf ( markString, comparisonOptions );
			if ( _index >= 0 )
			{
				var s = value.Substring ( _index + markString.Length );
				if ( trimResults )
					return s.Trim ();
				return s;
			}

			return value;
		}

		// ---------------------------------------------------------------------------------


		/// <summary>
		/// Makes a substring operation where the criteria is that the
		/// substring starts from the end of last ocurrence of the mark string.
		/// 
		/// So in a string like 'the quick brown fox jumps over the lazy dog' 
		/// where the mark string is 'fox', returns ' jumps over the lazy dog'
		/// including the white space or not depending on the optional parameter, 
		/// for which the default is false.
		/// </summary>
		/// <param name="value"></param>
		/// <param name="markString"></param>
		/// <param name="trimResults"></param>
		/// <returns></returns>
		public static string SubStringAfterLast ( this string value, string markString,
			StringComparison comparisonOptions = StringComparison.CurrentCulture, bool trimResults = false )
		{
			if ( String.IsNullOrEmpty ( value ) )
				return String.Empty;

			var _index = value.LastIndexOf ( markString, comparisonOptions );
			if ( _index >= 0 )
			{
				var s = value.Substring ( _index + markString.Length );
				if ( trimResults )
					return s.Trim ();
				return s;
			}

			return value;
		}

		// ---------------------------------------------------------------------------------


		public static string TakeBetween ( this string value, string beginningString, string endString, bool trimResults = false )
		{
			if ( String.IsNullOrWhiteSpace ( value ) )
				throw new ArgumentException ( "String cannot be null" );

			int _index1 = 0;
			int _index2 = 0;
			if ( beginningString != endString )
			{
				_index1 = value.IndexOf ( beginningString );
				_index2 = value.IndexOf ( endString );
			}
			else
			{
				_index1 = value.IndexOf ( beginningString );
				_index2 = value.LastIndexOf ( endString );
			}

			if ( _index2 < _index1 )
				throw new Exception ( "End string cannot appear earlier than beginning string" );

			if ( _index1 == -1 && _index2 == -1 )
				return String.Empty;

			_index1 += beginningString.Length;

			return value.Substring ( _index1, _index2 - _index1 );


		}


		// ---------------------------------------------------------------------------------



		/// <summary>
		/// Removes all ocurrences of a string in the interval denoted by the 
		/// beginningString and endString markers.
		/// </summary>
		/// <param name="value"></param>
		/// <param name="stringToRemove"></param>
		/// <param name="beginningString"></param>
		/// <param name="endString"></param>
		/// <returns></returns>
		public static string RemoveBetween ( this string value, string stringToRemove, string beginningString, string endString )
		{
			return value.RemoveBetween ( new List<string> { stringToRemove }, beginningString, endString );
		}


		// ---------------------------------------------------------------------------------


		/// <summary>
		/// Removes all ocurrences of a string in the interval denoted by the 
		/// beginningString and endString markers.
		/// </summary>
		/// <param name="value"></param>
		/// <param name="stringToRemove"></param>
		/// <param name="beginningString"></param>
		/// <param name="endString"></param>
		/// <returns></returns>
		public static string RemoveBetween ( this string value,
											 string stringToRemove,
											 string beginningString,
											 string endString,
											 Func<String, int, bool> func )
		{

			var rep = value.Replace ( " ", "", func );
			return rep;
		}


		// ---------------------------------------------------------------------------------


		/// <summary>
		/// Removes all ocurrences of a string in the interval denoted by the 
		/// beginningString and endString markers.
		/// </summary>
		/// <param name="value"></param>
		/// <param name="stringToRemove"></param>
		/// <param name="beginningString"></param>
		/// <param name="endString"></param>
		/// <returns></returns>
		public static string RemoveBetween ( this string value, IEnumerable<string> stringsToRemove, string beginningString, string endString )
		{
			if ( String.IsNullOrWhiteSpace ( value ) )
				throw new ArgumentException ( "String cannot be null" );

			if ( stringsToRemove == null )
				return value;

			var _index1 = value.IndexOf ( beginningString );
			var _index2 = value.IndexOf ( endString );

			if ( _index1 == -1 || _index2 == -1 )
				return value;

			if ( _index2 < _index1 )
				throw new Exception ( "End string cannot appear earlier than beginning string" );

			var _subset = value.Substring ( _index1, ( _index2 - _index1 ) + endString.Length );
			foreach ( var s in stringsToRemove )
				_subset = _subset.Replace ( s, String.Empty );

			var _ = String.Format ( "{0}{1}{2}", value.Substring ( 0, _index1 ), _subset, value.Substring ( _index2 + endString.Length ) );
			return _;

		}


		// ---------------------------------------------------------------------------------



		public static string TakeBetween ( this string value, string markString, string beginningString, string endString, bool trimResults = false )
		{
			if ( String.IsNullOrWhiteSpace ( value ) )
				throw new ArgumentException ( "String cannot be null (value)" );
			if ( String.IsNullOrWhiteSpace ( markString ) )
				throw new ArgumentException ( "String cannot be null (markString)" );

			value = value.SubStringAfter ( markString );

			var _index1 = value.IndexOf ( beginningString );
			var _index2 = value.IndexOf ( endString );

			if ( _index1 == -1 || _index2 == -1 )
				return value;

			if ( _index2 < _index1 )
				throw new Exception ( "End string cannot appear earlier than beginning string" );

			_index1 += beginningString.Length;

			if ( trimResults )
				return value.Substring ( _index1, _index2 - _index1 ).Trim ();

			return value.Substring ( _index1, _index2 - _index1 );


		}


		// ---------------------------------------------------------------------------------


		public static bool HasItems<T> ( this IEnumerable<T> t )
		{
			return t != null && t.Count () > 0;
		}



		// ---------------------------------------------------------------------------------


		public static string ReplaceWord ( this string subject, string oldValue, string newValue )
		{


			var _rx = @"([\t\s]|\b)+" + oldValue + @"([\t\s)]|\b)+";
			var _matches = Regex.Matches ( subject, _rx );
			if ( _matches != null && _matches.Count > 0 )
			{
				var _replacements = new List<Tuple<string, string>> ();
				for ( int i = 0; i < _matches.Count; i++ )
					_replacements.Add ( new Tuple<string, string> ( _matches [ i ].Value, _matches [ i ].Value.Replace ( oldValue, newValue ) ) );

				foreach ( var s in _replacements )
					subject = subject.Replace ( s.Item1, s.Item2 );

				return subject;
			}
			return subject;

		}



		// ---------------------------------------------------------------------------------


		public static string MultipleReplace ( this string subject, IEnumerable<string> occurrencesToRemove )
		{
			if ( String.IsNullOrWhiteSpace ( subject ) )
				throw new ArgumentException ( "String cannot be null (subject)" );

			if ( occurrencesToRemove == null || occurrencesToRemove.Count () == 0 )
				return subject;

			foreach ( var o in occurrencesToRemove )
				subject = subject.Replace ( o, "" );

			return subject;
		}


		// ---------------------------------------------------------------------------------

		public static string Take ( this string subject, int index )
		{
			if ( String.IsNullOrWhiteSpace ( subject ) )
				throw new ArgumentException ( "String cannot be null (subject)" );
			if ( index < 0 )
				throw new ArgumentException ( "Index cannot be zero or less than zero." );

			return subject.Substring ( 0, index );

		}


		// ---------------------------------------------------------------------------------


		public static string Skip ( this string subject, int index )
		{
			if ( String.IsNullOrWhiteSpace ( subject ) )
				throw new ArgumentException ( "String cannot be null (subject)" );
			if ( index < 0 )
				throw new ArgumentException ( "Index cannot be zero or less than zero." );
			if ( index > subject.Length )
				throw new ArgumentException ( "Index cannot be bigger than actual string length" );

			return subject.Substring ( index );

		}


		// ---------------------------------------------------------------------------------
		public static string ReplaceFirst ( this string subject, string occurrenceToRemove, string replacement = "" )
		{

			if ( String.IsNullOrWhiteSpace ( subject ) )
				throw new ArgumentException ( "String cannot be null (subject)" );
			if ( String.IsNullOrWhiteSpace ( occurrenceToRemove ) )
				throw new ArgumentException ( "String cannot be null (occurrenceToRemove)" );

			var i = subject.IndexOf ( occurrenceToRemove );

			if ( i >= 0 )
				return string.Format ( "{0}{1}{2}", subject.Take ( i ), replacement, subject.Skip ( i + occurrenceToRemove.Length ) );
			return subject;

		}


		// ---------------------------------------------------------------------------------


		public static string ReplaceLast ( this string subject, string occurrenceToRemove, string replacement = "" )
		{

			if ( String.IsNullOrWhiteSpace ( subject ) )
				throw new ArgumentException ( "String cannot be null (subject)" );
			if ( String.IsNullOrWhiteSpace ( occurrenceToRemove ) )
				throw new ArgumentException ( "String cannot be null (occurrenceToRemove)" );

			var i = subject.LastIndexOf ( occurrenceToRemove );
			if ( i >= 0 )
				return string.Format ( "{0}{1}{2}", subject.Take ( i ), replacement, subject.Skip ( i + occurrenceToRemove.Length ) );
			return subject;

		}


		// ---------------------------------------------------------------------------------
		public static string ReplaceFirstAndLastOnly ( this string subject, string occurrenceToRemove, string replacement = "" )
		{
			// TODO: ADD EXCEPTION CONTROL
			// TODO: ADD CULTURE AND COMPARISON INFO

			if ( String.IsNullOrWhiteSpace ( subject ) )
				throw new ArgumentException ( "String cannot be null (subject)" );
			if ( String.IsNullOrWhiteSpace ( occurrenceToRemove ) )
				throw new ArgumentException ( "String cannot be null (occurrenceToRemove)" );

			subject = subject.ReplaceFirst ( occurrenceToRemove, replacement );
			subject = subject.ReplaceLast ( occurrenceToRemove, replacement );

			return subject;
		}


		// ---------------------------------------------------------------------------------


		public static bool IsBetweenMinAndMax ( this int search, IEnumerable<int> list )
		{
			return search > list.Min () && search < list.Max ();
		}

		/// <summary>
		/// Returns a boolean value that indicates if a char or string is contained
		/// between the marker strings passed.
		/// </summary>
		/// <param name="?"></param>
		/// <returns></returns>
		public static bool IsBetween ( this string sut, string search, string markerString, int startIndex = 0 )
		{

			// TODO: ADD EXCEPTION CONTROL
			// TODO: ADD CULTURE AND COMPARISON INFO
			var indexOfSut = search.IndexOf ( sut, startIndex );
			var indices = search.IndicesOfAll ( markerString );

			// indices must be of a even length, otherwise is not "between"
			if ( indices.Count () % 2 > 0 )
				throw new Exception ( "The marker string has to have an even number of occurrences" );

			if ( indices.Count () > 0 )
			{

				// split the indices in pairs
				var _split = indices.Split ( 2 );

				if ( indexOfSut.InBetweenAny ( _split ) )
				{
					return true;
				}
			}
			return false;
		}


		// ---------------------------------------------------------------------------------


		/// <summary>
		/// Splits a collection into a collection of collections
		/// where each element is of the same length as indicated by splitLength.
		/// Splitting a 9-element collection with a value of 3 for splitLength, yields
		/// a list of 3-element collections.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="list"></param>
		/// <param name="splitLength"></param>
		/// <returns></returns>
		public static IEnumerable<IEnumerable<T>> Split<T> ( this IEnumerable<T> list, int splitLength )
		{
			if ( !list.HasItems () )
				throw new ArgumentException ( "A collection has to have items in order to be split" );
			if ( splitLength <= 0 || splitLength > list.Count () )
				throw new ArgumentException ( "Invalid split length" );

			var __list = new List<List<T>> ();
			List<T> __subList = new List<T> (); ;

			var i = 0;
			foreach ( var x in list )
			{
				if ( i > 0 && i % splitLength == 0 )
				{
					__list.Add ( __subList );
					__subList = new List<T> ();
				}

				__subList.Add ( x );
				i++;
			}

			// on exit add last split
			__list.Add ( __subList );

			return __list;

		}


		// ---------------------------------------------------------------------------------

		public static bool InBetweenAny ( this Int16 t, IEnumerable<IEnumerable<Int16>> intervals )
		{
			if ( !intervals.HasItems () )
				return false;

			foreach ( var interval in intervals )
			{
				if ( t <= interval.Max () && t >= interval.Min () )
					return true;
			}
			return false;
		}

		public static bool InBetweenAny ( this Int32 t, IEnumerable<IEnumerable<Int32>> intervals )
		{
			if ( !intervals.HasItems () )
				return false;

			foreach ( var interval in intervals )
			{
				if ( t <= interval.Max () && t >= interval.Min () )
					return true;
			}
			return false;
		}

		public static bool InBetweenAny ( this Int64 t, IEnumerable<IEnumerable<Int64>> intervals )
		{
			if ( !intervals.HasItems () )
				return false;

			foreach ( var interval in intervals )
			{
				if ( t <= interval.Max () && t >= interval.Min () )
					return true;
			}
			return false;
		}

		public static bool InBetweenAny ( this float t, IEnumerable<IEnumerable<float>> intervals )
		{
			if ( !intervals.HasItems () )
				return false;

			foreach ( var interval in intervals )
			{
				if ( t <= interval.Max () && t >= interval.Min () )
					return true;
			}
			return false;
		}

		public static bool InBetweenAny ( this Decimal t, IEnumerable<IEnumerable<Decimal>> intervals )
		{
			if ( !intervals.HasItems () )
				return false;

			foreach ( var interval in intervals )
			{
				if ( t <= interval.Max () && t >= interval.Min () )
					return true;
			}
			return false;
		}

		public static bool InBetweenAny ( this Double t, IEnumerable<IEnumerable<Double>> intervals, int roundToPrecision = 2 )
		{
			if ( !intervals.HasItems () )
				return false;

			foreach ( var interval in intervals )
			{
				if ( Math.Round ( t, roundToPrecision ) <= interval.Max () && Math.Round ( t, roundToPrecision ) >= interval.Min () )
					return true;
			}
			return false;
		}

		// ---------------------------------------------------------------------------------


		public static IEnumerable<int> IndicesOfAll ( this string value, string search, bool ignoreCase = false )
		{
			// TODO: ADD EXCEPTION CONTROL
			// TODO: ADD CULTURE AND COMPARISON INFO

			var i = 0;
			var _indices = new List<int> ();
			if ( !ignoreCase )
			{
				while ( ( i = value.IndexOf ( search, i ) ) != -1 )
				{
					_indices.Add ( i++ );
				}
			}
			else
			{
				while ( ( i = value.ToUpperInvariant ().IndexOf ( search.ToUpperInvariant (), i ) ) != -1 )
				{
					_indices.Add ( i++ );
				}
			}
			return _indices;
		}



		// ---------------------------------------------------------------------------------


		//public static string Replace ( this string sut, char replacee, char replacement, Func<String, bool> func )
		//{
		//	if ( sut.Length > 25 )
		//	{
		//		var __sb = new StringBuilder ();
		//		foreach ( var c in sut )
		//		{
		//			if ( c == replacee && func ( replacee.ToString () ) )
		//				__sb.Append ( replacement );
		//			else
		//				__sb.Append ( replacee );
		//		}
		//		return __sb.ToString ();
		//	}
		//	else
		//	{
		//		var __sb = String.Empty;
		//		foreach ( var c in sut )
		//		{
		//			if ( c == replacee && func ( replacee.ToString () ) )
		//				__sb += replacement;
		//			else
		//				__sb += replacee;
		//		}
		//		return __sb;
		//	}
		//}


		// ---------------------------------------------------------------------------------

		public static string Replace ( this string sut, string replacee, string replacement, Func<String, Int32, bool> func )
		{
			var i = 0;
			if ( sut.Length > 25 )
			{
				var __sb = new StringBuilder ();
				foreach ( var c in sut )
				{
					if ( c.ToString () == replacee )
					{
						if ( func ( c.ToString (), i ) )
							__sb.Append ( c );
						else
							__sb.Append ( replacement );
					}
					else
						__sb.Append ( c );
					i++;
				}
				return __sb.ToString ();
			}
			else
			{
				var __sb = String.Empty;
				foreach ( var c in sut )
				{
					if ( c.ToString () == replacee )
					{
						if ( func ( c.ToString (), i ) )
							__sb += c;
						else
							__sb += replacement;
					}
					else
						__sb += c;
					i++;
				}
				return __sb;
			}
		}


		// ---------------------------------------------------------------------------------


		public static IEnumerable<String> FindAllBetween ( this string value, string s1, string s2, bool includeMarkers = false )
		{
			int _1 = 0;
			var _occurrences = new List<String> ();

			if ( !includeMarkers )
			{
				while ( ( _1 = value.IndexOf ( s1, _1 ) ) != -1 )
				{
					var _2 = value.IndexOf ( s2, _1 + 1 );
					if ( _2 != -1 )
					{
						_occurrences.Add ( value.Substring ( ++_1, _2 - _1 ) );
					}
					_1 += _2 - _1;
					if ( _1 == -1 )
						break;
				}
			}
			else
			{
				while ( ( _1 = value.IndexOf ( s1, _1 ) ) != -1 )
				{
					var _2 = value.IndexOf ( s2, _1 + 1 );
					if ( _2 != -1 )
					{
						_occurrences.Add ( value.Substring ( _1, ++_2 - _1 ) );
					}
					_1 += _2 - _1;
					if ( _1 == -1 )
						break;
				}
			}
			return _occurrences;
		}

		// ---------------------------------------------------------------------------------

	}
}
