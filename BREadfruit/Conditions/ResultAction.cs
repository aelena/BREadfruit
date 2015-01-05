using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BREadfruit.Helpers;

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


		public override bool Equals ( object obj )
		{

			if ( obj == null)
				return false;

			if ( obj.GetType() != typeof(ResultAction))
				return false;

			var _ra = (ResultAction)obj;

			// TODO: see if more is necessary in here .... 

			return
				( this.Token == _ra.Token &&
				  this.Value == _ra.Value &&
				  this.IsUnary == _ra.IsUnary &&
				  this.IsResultAction == _ra.IsResultAction );
		}


		// ---------------------------------------------------------------------------------



    }
}
