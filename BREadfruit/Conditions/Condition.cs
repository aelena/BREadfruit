using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BREadfruit.Conditions
{
    public class Condition
    {

        private readonly string _operand;
        /// <summary>
        /// Operand in the condition (the left side
        /// of the condition).
        /// </summary>
        public string Operand
        {
            get { return _operand; }
        }

        private readonly Operator _operator;
        /// <summary>
        /// Condition operator.
        /// </summary>
        public Operator Operator
        {
            get { return _operator; }
        }

        private readonly Object _value;
        /// <summary>
        /// Value of the condition.
        /// </summary>
        public Object Value
        {
            get { return _value; }
        }

        private string _suffixLogicalOperator;

        // probably this does not need to be public...
        protected internal string SuffixLogicalOperator
        {
            get { return _suffixLogicalOperator; }
        } 

        //private List<Condition> _conditions = new List<Condition> ();
        ///// <summary>
        ///// Returns nested conditions inside a condition instance.
        ///// </summary>
        //public IEnumerable<Condition> Conditions
        //{
        //    get
        //    {
        //        return this._conditions;
        //    }
        //}


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


        private List<ResultAction> _resultActions = new List<ResultAction> ();
        public IEnumerable<ResultAction> ResultActions
        {
            get
            {
                return this._resultActions;
            }
        }


        // ---------------------------------------------------------------------------------


        protected internal void SetLogicalOperator (string symbol)
        {
            this._suffixLogicalOperator = symbol;
        }


        // ---------------------------------------------------------------------------------


        protected internal Condition ( string operand, Operator operatorItem, object value )
        {
            this._operand = operand;
            this._operator = operatorItem;
            this._value = value;
            this._suffixLogicalOperator = null;
        }


        // ---------------------------------------------------------------------------------

        protected internal Condition ( string operand, Operator operatorItem, object value, string suffixLogixOperator )
        {
            this._operand = operand;
            this._operator = operatorItem;
            this._value = value;
            this._suffixLogicalOperator = suffixLogixOperator;
        }


        // ---------------------------------------------------------------------------------
        public override string ToString ()
        {
            return String.Format ( "{0} {1} {2}", 
                this.Operand, this.Operator.Token, this.Value.ToString() );
        }


        // ---------------------------------------------------------------------------------

        //protected internal void AddNestedCondition ( Condition condition )
        //{
        //    if ( condition == null )
        //        throw new ArgumentException ( "Cannot add a nested condition that is null" );
        //    _conditions.Add ( condition );
        //}

        // ---------------------------------------------------------------------------------

        protected internal Condition AddUnaryAction ( UnaryAction result )
        {
            if ( result == null )
                throw new ArgumentNullException ( "result" );

            this._results.Add ( result );
            return this;
        }

        // ---------------------------------------------------------------------------------


        protected internal Condition AddResultAction ( ResultAction result )
        {
            if ( result == null )
                throw new ArgumentNullException ( "result" );

            this._resultActions.Add ( result );
            return this;
        }

        // ---------------------------------------------------------------------------------

    }
}
