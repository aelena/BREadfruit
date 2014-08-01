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


    }
}
