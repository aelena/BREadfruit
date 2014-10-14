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


        // ---------------------------------------------------------------------------------


        public override bool IsUnary
        {
            get
            {
                return false;
            }
        }


        // ---------------------------------------------------------------------------------


        public override bool IsResultAction
        {
            get
            {
                return true;
            }
        }


        // ---------------------------------------------------------------------------------


        private SortedList<string, string> _arguments;
        public IEnumerable<KeyValuePair<string, string>> Arguments
        {
            get { return this._arguments.AsEnumerable (); }
        }


        // ---------------------------------------------------------------------------------

        protected internal ResultAction ( Symbol s, object value, string reference = "this" ) :
            base ( s.Token, s.IndentLevel, s.IsTerminal )
        {
            this.Value = value;
            this.Reference = reference;
        }


        // ---------------------------------------------------------------------------------


        protected internal ResultAction ( string identifier,
            int indentLevel,
            bool isTerminal,
            IEnumerable<string> aliases = null,
            string reference = "this" )
            : base ( identifier, indentLevel, isTerminal, aliases, reference )
        {

        }


        // ---------------------------------------------------------------------------------


        internal void AddArgument ( string key, string value )
        {
            if ( this._arguments == null )
                this._arguments = new SortedList<string, string> ();

            this._arguments.Add ( key, value );
        }


        // ---------------------------------------------------------------------------------


        public override string ToString ()
        {
            if ( this.Value.ToString () != this.Reference )
                return String.Format ( "{0} {1} {2}", this.Token, this.Value.ToString (), this.Reference );
            else
                return String.Format ( "{0} {1}", this.Token, this.Value.ToString () );

        }


        // ---------------------------------------------------------------------------------


        public static bool operator == ( ResultAction resultAction, Symbol y )
        {
            var t = Grammar.GetSymbolByToken ( resultAction.Action );
            return t == y;
        }


        // ---------------------------------------------------------------------------------


        public static bool operator != ( ResultAction resultAction, Symbol y )
        {
            var t = Grammar.GetSymbolByToken ( resultAction.Action );
            return t != y;
        }


        // ---------------------------------------------------------------------------------


    }
}
