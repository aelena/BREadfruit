using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BREadfruit
{
    public class Constraint
    {
        private readonly string _constraintName;
        /// <summary>
        /// Gets an naming descriptor that indicates the kind 
        /// of constraint this represents
        /// </summary>
        public string Name
        {
            get { return _constraintName; }
        }

        // ---------------------------------------------------------------------------------

        private readonly string _regex;

        /// <summary>
        /// Gets the regex that was set up during construction.
        /// Not all constraints might have regular expressions and therefore this
        /// property could return null.
        /// </summary>
        public string RegexValidationPattern
        {
            get { return _regex; }
        }

        // ---------------------------------------------------------------------------------


        public Constraint ( string name, string regexValidationPattern = null )
        {
            this._constraintName = name;
            this._regex = regexValidationPattern;
        }

        // ---------------------------------------------------------------------------------


        public override string ToString ()
        {
            if ( RegexValidationPattern != null )
                return String.Format ( "{0} {1}", this.Name, this.RegexValidationPattern );
            return this.Name;
        }

        // ---------------------------------------------------------------------------------

    }
}
