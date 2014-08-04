using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BREadfruit.Helpers
{
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
        /// <returns></returns>
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
    }
}
