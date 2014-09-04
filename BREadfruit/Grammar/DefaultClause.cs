using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using BREadfruit.Helpers;


namespace BREadfruit.Clauses
{
    /// <summary>
    /// Represents a default clause such as 
    /// 
    /// min_length 10,
    ///	mandatory true
    ///	
    /// </summary>
    public class DefaultClause : Symbol
    {

        /// <summary>
        /// Regular expression that will validate the value that can be
        /// taken by an instance of DefaultClause.
        /// </summary>
        private readonly string _regexPattern;
        /// <summary>
        /// Pattern for the regular expression (as string,
        /// not as instance of Regex) that validates the values
        /// that can be assigned to this instance.
        /// </summary>
        public String RegexPattern
        {
            get { return _regexPattern; }
        }


        // ---------------------------------------------------------------------------------


        /// <summary>
        /// Value of the default clause.
        /// </summary>
        private Object _value;
        /// <summary>
        /// Value that can be assigned to this instance.
        /// </summary>
        public Object Value
        {
            get { return _value; }
        }



        // ---------------------------------------------------------------------------------


        private SortedList<string, string> _arguments;

        public SortedList<string, string> Arguments
        {
            get { return this._arguments; }
        }


        // ---------------------------------------------------------------------------------


        /// <summary>
        /// Parameterized constructor.
        /// </summary>
        /// <param name="token"></param>
        /// <param name="regexPattern"></param>
        /// <param name="aliases"></param>
        public DefaultClause ( string token, string regexPattern, IEnumerable<String> aliases = null ) :
            base ( token, 0 /*indentLevel*/)
        {
            //this._token = token;
            if ( aliases != null )
                this._aliases = aliases.ToList ();
            this._regexPattern = regexPattern;
            this._arguments = new SortedList<string, string> ();
        }


        // ---------------------------------------------------------------------------------


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
                    this._value = value.In ( Grammar.EmptyStringMarkers ) ? "" : value;
                    return true;
                }
                else
                {
                    // TODO: Pending some testing here...
                    throw new Exception ( String.Format (
                        "Attempting to set invalid value {0} for Default Clause {1}. Ensure the value passes the Regex filter {2}",
                        value, this.Token, this.RegexPattern ) );
                }
            }
            return false;
        }


        // ---------------------------------------------------------------------------------


        public override string ToString ()
        {
            return String.Format ( "{0} {1}", this.Token, this.Value );
        }


        // ---------------------------------------------------------------------------------


        protected internal void AddArgumentsFromString ( string argumentsToken )
        {
            if ( String.IsNullOrWhiteSpace ( argumentsToken ) )
                throw new ArgumentNullException ( "Argument string is null or empty", "argumentsToken" );

            // check that argumentsToken contains both "{" and "}"
            var _args = argumentsToken.TakeBetween ( "{", "}" ).Split ( new char [] {','}, StringSplitOptions.RemoveEmptyEntries );
            foreach ( var _a in _args )
            {
                var _argPair = _a.Split ( new char [] { ':' }, StringSplitOptions.RemoveEmptyEntries );
                this._arguments.Add ( _argPair.First ().Trim (), _argPair.Last ().Trim () );
            }

            //.Replace("\"", "" )
            
        }


        // ---------------------------------------------------------------------------------


    }
}
