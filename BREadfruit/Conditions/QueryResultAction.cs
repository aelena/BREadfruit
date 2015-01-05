using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BREadfruit.Conditions
{
	public class QueryResultAction : ParameterizedResultAction
	{


		private readonly string _query;


		public string Query
		{
			get
			{
				return this._query;
			}
		}


		protected internal QueryResultAction ( string query, Symbol s, object value, string reference = "this" ) :
            base ( s.Token, s.IndentLevel, s.IsTerminal )
        {
			this._query = query;
            this.Value = value;
            this.Reference = reference;
        }


        // ---------------------------------------------------------------------------------


		protected internal QueryResultAction ( string query, string identifier,
            int indentLevel,
            bool isTerminal,
            IEnumerable<string> aliases = null,
            string reference = "this" )
            : base ( identifier, indentLevel, isTerminal, aliases, reference )
        {
			this._query = query;

        }


        // ---------------------------------------------------------------------------------
	}
}
