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
        internal String RegexPattern
        {
            get { return _regexPattern; }
        }


        // ---------------------------------------------------------------------------------


        /// <summary>
        /// Value of the default clause.
        /// </summary>
        private Object _value;
        /// <summary>
        /// Value that can be assigned to the default clause instance.
        /// </summary>
        public Object Value
        {
            get { return _value; }
        }



        // ---------------------------------------------------------------------------------


        private SortedList<string, string> _arguments;

        /// <summary>
        /// In the case of default clauses that need to have argument (for example
        /// calls to web services or external datasources) this list will contain 
        /// arguments in the form of a list of key and value pairs, where the
        /// key is the name of the parameter / argument, and the value in the pair 
        /// is the value for the argument.
        /// </summary>
        public IEnumerable<KeyValuePair<string, string>> Arguments
        {
            get { return this._arguments.AsEnumerable (); }
        }


        // ---------------------------------------------------------------------------------


		private List<FieldControlPair> _outputArguments;
		public IEnumerable<FieldControlPair> OutputArguments
		{
			get { return this._outputArguments; }
		}


		// ---------------------------------------------------------------------------------


        /// <summary>
        /// Parameterized constructor.
        /// </summary>
        /// <param name="token"></param>
        /// <param name="regexPattern"></param>
        /// <param name="aliases"></param>
        protected internal DefaultClause ( string token, string regexPattern, IEnumerable<String> aliases = null ) :
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
        internal bool SetValue ( Object value )
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

            string [] _args = null;
            // check that argumentsToken contains both "{" and "}"
            if (argumentsToken.Contains("{") && argumentsToken.Contains ( "}") )
                _args = argumentsToken.TakeBetween ( "{", "}" ).Split ( new char [] {','}, StringSplitOptions.RemoveEmptyEntries );
            else
                _args = argumentsToken.Split ( new char [] { ',' }, StringSplitOptions.RemoveEmptyEntries );

            foreach ( var _a in _args )
            {
                var _argPair = _a.Split ( new char [] { ':' }, StringSplitOptions.RemoveEmptyEntries );
                this._arguments.Add ( _argPair.First ().Trim (), _argPair.Last ().Trim () );
            }

            
        }


        // ---------------------------------------------------------------------------------


		protected internal void AddOutputArgumentsFromString ( string argumentsToken )
		{
			if ( String.IsNullOrWhiteSpace ( argumentsToken ) )
				throw new ArgumentNullException ( "Argument string is null or empty", "argumentsToken" );

			string [] _args = null;
			// check that argumentsToken contains both "{" and "}"
			if ( argumentsToken.Contains ( "{" ) && argumentsToken.Contains ( "}" ) )
				_args = argumentsToken.TakeBetween ( "{", "}" ).Split ( new char [] { ',' }, StringSplitOptions.RemoveEmptyEntries );
			else
				_args = argumentsToken.Split ( new char [] { ',' }, StringSplitOptions.RemoveEmptyEntries );

			foreach ( var _a in _args )
			{
				var _argPair = _a.Split ( new char [] { ':' }, StringSplitOptions.RemoveEmptyEntries );
				if ( this._outputArguments == null )
					this._outputArguments = new List<FieldControlPair> ();
				this._outputArguments.Add ( new FieldControlPair ( _argPair.Last ().Trim ().ReplaceFirstAndLastOnly ( "\"" ), _argPair.First ().Trim () ) );
			}


		}


		// ---------------------------------------------------------------------------------


        public new DefaultClause Clone ()
        {

            var _df = new DefaultClause ( this.Token, this.RegexPattern, this._aliases );
            _df.IndentLevel = this.IndentLevel;
            _df.IsTerminal = this.IsTerminal;
			//_df._arguments = this._arguments;
            _df._value = this._value;
            return _df;
        }


        // ---------------------------------------------------------------------------------


    }
}
