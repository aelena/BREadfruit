using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BREadfruit.Conditions
{

    /// <summary>
    /// Represents a rule instance
    /// </summary>
    public class Rule
    {

        /// <summary>
        /// A rule can have one or more conditions.
        /// </summary>
        private List<Condition> _conditions = new List<Condition> ();

        /// <summary>
        /// Gets a list of the conditions that this rule has.
        /// </summary>
        public IEnumerable<Condition> Conditions
        {
            get
            {
                return this._conditions;
            }
        }


		// ---------------------------------------------------------------------------------


		/// <summary>
		/// Indicates if this rule is the last one to be taken into account
		/// and other rules to be ignored on account of finding a return statement.
		/// </summary>
		public bool IsFinalRule { get; protected internal set; }


		// ---------------------------------------------------------------------------------


		/// <summary>
		/// Indicates if the Rule has a series of conditions acting as Else branch
		/// </summary>
		public bool HasElseClause { get; protected internal set; }

		

        // ---------------------------------------------------------------------------------


        /// <summary>
        /// Gets the number of conditions the rule has.
        /// </summary>
        public int NumberOfConditions
        {
            get
            {
                return this._conditions.Count ();
            }
        }


        // ---------------------------------------------------------------------------------


        /// <summary>
        /// Returns a boolean value that indicates whether this is a 
        /// "simple condition rule", that is, that it only has a condition.
        /// </summary>
        public bool IsSimpleConditionRule
        {
            get
            {
                return this._conditions.Count () == 1;
            }
        }

        // ---------------------------------------------------------------------------------


        /// <summary>
        /// Returns a boolean value that indicates whether this is a 
        /// "multiple condition rule", that is, that it has more than a condition.
        /// </summary>
        public bool IsMultipleConditionRule
        {
            get
            {
                return this._conditions.Count () > 1;
            }
        }


        // ---------------------------------------------------------------------------------


        /// <summary>
        /// Allows privileged callers (protected internal) to add conditions to the rule
        /// as these conditions are parsed from the file.
        /// </summary>
        /// <param name="condition">Condition instance.</param>
        /// <returns></returns>
        protected internal Rule AddCondition ( Condition condition )
        {
            if ( condition == null )
                throw new ArgumentNullException ( "condition" );

            this._conditions.Add ( condition );
            return this;
        }


        // ---------------------------------------------------------------------------------


        public override string ToString ()
        {
            string _ = "";
            this._conditions.ForEach ( x => _ += x.ToString () + Environment.NewLine );
            return _;
        }


        // ---------------------------------------------------------------------------------

    }
}
