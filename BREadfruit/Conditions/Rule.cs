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


        private List<Condition> _conditions = new List<Condition> ();
        public IEnumerable<Condition> Conditions
        {
            get
            {
                return this._conditions;
            }
        }


        // ---------------------------------------------------------------------------------


        private List<UnaryAction> _results = new List<UnaryAction> ();
        public IEnumerable<UnaryAction> Results
        {
            get
            {
                return this._results;
            }
        }


        // ---------------------------------------------------------------------------------


        public Rule AddCondition ( Condition condition )
        {
            if ( condition == null )
                throw new ArgumentNullException ( "condition" );

            this._conditions.Add ( condition );
            return this;
        }


        // ---------------------------------------------------------------------------------


        public Rule AddResultAction ( UnaryAction result )
        {
            if ( result == null )
                throw new ArgumentNullException ( "result" );

            this._results.Add ( result );
            return this;
        }

        // ---------------------------------------------------------------------------------


    }
}
