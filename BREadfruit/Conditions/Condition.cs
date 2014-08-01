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

        private List<Condition> _conditions = new List<Condition> ();
        /// <summary>
        /// Returns nested conditions inside a condition instance.
        /// </summary>
        public IEnumerable<Condition> Conditions
        {
            get
            {
                return this._conditions;
            }
        }


        // ---------------------------------------------------------------------------------


        public Condition ( string operand, Operator operatorItem, object value )
        {
            this._operand = operand;
            this._operator = operatorItem;
            this._value = value;
        }


        // ---------------------------------------------------------------------------------


        public override string ToString ()
        {
            return String.Format ( "{0} {1} {2}", 
                this.Operand, this.Operator.Identifier, this.Value.ToString() );
        }


        // ---------------------------------------------------------------------------------


    }
}
