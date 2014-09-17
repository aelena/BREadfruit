﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

            // we return a new symbol with the concatenated representation
            // assuming we take the indent level of the first in the series
            // (maybe we should check all symbols in the series have the same indentlevel)
            // and the isTerminal value for the last in the series
            return new Symbol ( _s.Trim (), series.First ().IndentLevel, series.Last ().IsTerminal );

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
        public static bool ContainsAny ( this string searchee, IEnumerable<string> searches )
        {
            if ( searchee == null )
                throw new ArgumentNullException ( "searchee", "the string cannot be null." );

            if ( searches != null )
                foreach ( var s in searches )
                    if ( searchee.Contains ( s ) )
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
        /// <returns>Returns a boolean value to indicate if the search was successful
        /// and returns also the value itself.</returns>
        public static Tuple<bool, string> ContainsAny2 ( this string searchee, IEnumerable<string> searches )
        {
            if ( searchee == null )
                throw new ArgumentNullException ( "searchee", "the string cannot be null." );

            if ( searches != null )
                foreach ( var s in searches )
                    if ( searchee.Contains ( s ) )
                        return new Tuple<bool, string> ( true, s );

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
            foreach ( var l in list){
                if ( func (l) )
                {
                    return list.Skip ( ++i );
                }
                i++;
            }
            return null;
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


        public static string TakeBetween ( this string value, string beginningString, string endString )
        {
            if ( String.IsNullOrWhiteSpace ( value ) )
                throw new ArgumentException ( "String cannot be null" );

            var _index1 = value.IndexOf ( beginningString );
            var _index2 = value.IndexOf ( endString );

            if ( _index2 < _index1 )
                throw new Exception ( "End string cannot appear earlier than beginning string" );

            _index1 += beginningString.Length;

            return value.Substring ( _index1, _index2 - _index1 );


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

            if ( _index2 < _index1 )
                throw new Exception ( "End string cannot appear earlier than beginning string" );

            _index1 += beginningString.Length;

            if ( trimResults )
                return value.Substring ( _index1, _index2 - _index1 ).Trim ();

            return value.Substring ( _index1, _index2 - _index1 );


        }

        // ---------------------------------------------------------------------------------



    }
}
