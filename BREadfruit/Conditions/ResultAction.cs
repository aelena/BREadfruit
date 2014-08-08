using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BREadfruit.Conditions
{

    /// <summary>
    /// Represens a result / action that performs an action on an entity,
    /// being that an DOM element or an object's field,
    /// or an in-memory representation of state.
    /// </summary>
    public class ResultAction : UnaryAction
    {

        public object Value { get; protected internal set; }

        public ResultAction ( Symbol s, object value, string reference = "this" ) :
            base ( s.Token, s.IndentLevel, s.IsTerminal )
        {
            this.Value = value;
            this.Reference = reference;
        }

        public ResultAction ( string identifier,
            int indentLevel,
            bool isTerminal,
            IEnumerable<string> aliases = null,
            string reference = "this" )
            : base ( identifier, indentLevel, isTerminal, aliases, reference )
        {

        }

        public override string ToString ()
        {
            return String.Format ( "{0} {1} {2}", this.Token, this.Value.ToString (), this.Reference );
        }

    }
}
