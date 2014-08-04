using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BREadfruit.Conditions
{
    public abstract class AliasedToken
    {
        protected string _name;
        public string Identifier
        {
            get { return _name; }
        }


        // ---------------------------------------------------------------------------------


        /// <summary>
        /// Stores other syntax that might be available for this same operator.
        /// For example, we can accept 'bigger or equals than' or '>=' as well 
        /// within the valid file syntax
        /// </summary>
        protected List<String> _aliases;
        public IEnumerable<String> Aliases
        {
            get
            {
                return this._aliases;
            }
        }


        /// <summary>
        /// Clients call pass a specified token ( any arbitrary string actually )
        /// and get a value that indicates whether that token is an valid and +
        /// known alias for this Operator.
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public bool MatchesToken ( string token )
        {
            if ( !String.IsNullOrWhiteSpace ( token ) )
            {
                if ( this._aliases != null )
                {
                    var matched = from x in _aliases
                                  where x == token
                                  select x;

                    return ( matched != null && matched.Count () > 0 );
                }
            }
            return false;

        }


        // ---------------------------------------------------------------------------------

    }
}
