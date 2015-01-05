using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BREadfruit.Helpers;

namespace BREadfruit.Conditions
{
	public class ParameterizedResultAction : ResultAction
	{
		private SortedList<string, string> _arguments;
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


		internal void AddArgument ( string key, string value )
		{
			if ( this._arguments == null )
				this._arguments = new SortedList<string, string> ();

			this._arguments.Add ( key, value );
		}


		// ---------------------------------------------------------------------------------


		internal void AddOutputArgument ( string dataField, string controlName )
		{
			if ( this._outputArguments == null )
				this._outputArguments = new List<FieldControlPair> ();

			this._outputArguments.Add ( new FieldControlPair ( dataField, controlName ) );
		}


		// ---------------------------------------------------------------------------------


		protected internal ParameterizedResultAction ( Symbol s, object value, string reference = "this" ) :
            base ( s.Token, s.IndentLevel, s.IsTerminal )
        {
            this.Value = value;
            this.Reference = reference;
        }


        // ---------------------------------------------------------------------------------


		protected internal ParameterizedResultAction ( string identifier,
            int indentLevel,
            bool isTerminal,
            IEnumerable<string> aliases = null,
            string reference = "this" )
            : base ( identifier, indentLevel, isTerminal, aliases, reference )
        {

        }


        // ---------------------------------------------------------------------------------

	}
}
