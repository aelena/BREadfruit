using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BREadfruit.Clauses
{
    /// <summary>
    /// Represents a default clause such as 
    /// 
    /// min_length 10,
    ///	mandatory true
    ///	
    /// </summary>
    public class DefaultClause
    {

        /// <summary>
        /// Main token of the default clause, for example 'max_length'.
        /// </summary>
        private readonly string _token;
        /// <summary>
        /// Allowed aliases for the default clauses, for example 'max_length'
        /// could be expressed also as 'max', 'length' or 'maximum_length'.
        /// </summary>
        private readonly List<string> _aliases;
        /// <summary>
        /// Regular expression that will validate the value that can be
        /// taken by an instance of DefaultClause.
        /// </summary>
        private readonly string _regexPattern;
        /// <summary>
        /// Value of the default clause.
        /// </summary>
        private Object _value;

        /// <summary>
        /// Main identifier for the Default clause
        /// </summary>
        public string Token
        {
            get { return _token; }
        }

        /// <summary>
        /// Other valid identifiers for this default clause
        /// </summary>
        public List<string> Aliases
        {
            get { return _aliases; }
        }

        /// <summary>
        /// Pattern for the regular expression (as string,
        /// not as instance of Regex) that validates the values
        /// that can be assigned to this instance.
        /// </summary>
        public String RegexPattern
        {
            get { return _regexPattern; }
        }

        /// <summary>
        /// Value that can be assigned to this instance.
        /// </summary>
        public Object Value
        {
            get { return _value; }
        }

        /// <summary>
        /// Parameterized constructor.
        /// </summary>
        /// <param name="token"></param>
        /// <param name="regexPattern"></param>
        /// <param name="aliases"></param>
        public DefaultClause ( string token, string regexPattern, IEnumerable<String> aliases = null )
        {
            this._token = token;
            if ( aliases != null )
                this._aliases = aliases.ToList ();
            this._regexPattern = regexPattern;
        }

        /// <summary>
        /// Allows a caller to set a value for this default clause.
        /// The value passed is subject to regex validation
        /// according to the regex pattern that was assigned in the
        /// constructor.
        /// </summary>
        /// <param name="value">value to be assigned to this default instance.</param>
        /// <returns>returns true if the value has been assigned, which
        /// depends on it matching the regex pattern. Otherwise
        /// the value returned is false.</returns>
        public bool SetValue ( Object value )
        {
            if ( value != null )
            {
                if ( Regex.IsMatch ( value.ToString (), this.RegexPattern ) )
                {
                    this._value = value;
                    return true;
                }
            }
            return false;
        }

    }
}
