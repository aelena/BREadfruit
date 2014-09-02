using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BREadfruit.Conditions
{
    public class UnaryAction : Symbol
    {

        /// <summary>
        /// Contains the name of the object / entity this unary action
        /// refers to. 
        /// If it is not specified then a value of this is assumed
        /// </summary>
        public string Reference { get; protected set; }


        public string Action
        {
            get
            {
                return this.Token;
            }
        }



        // ---------------------------------------------------------------------------------


        /// <summary>
        /// 
        /// </summary>
        /// <param name="s"></param>
        public UnaryAction ( Symbol s, string reference = "this" ) :
            base ( s.Token, s.IndentLevel, s.IsTerminal )
        {
            if ( s.Children != null )
                foreach ( var c in s.Children )
                    base.AddValidChild ( c );

            this.Reference = reference;
        }


        // ---------------------------------------------------------------------------------


        /// <summary>
        /// 
        /// </summary>
        /// <param name="identifier"></param>
        /// <param name="indentLevel"></param>
        /// <param name="isTerminal"></param>
        /// <param name="aliases"></param>
        public UnaryAction ( string identifier, int indentLevel, bool isTerminal, IEnumerable<string> aliases = null, string reference = "this" )
            : base ( identifier, indentLevel, isTerminal )
        {
            if ( String.IsNullOrWhiteSpace ( identifier ) )
                throw new ArgumentNullException ( "identifier", "The identifier for a ResultAction instance cannot be null" );

            this.Token = identifier;
            this.Reference = "this";

            if ( aliases != null )
                this._aliases.AddRange ( aliases );

        }


        // ---------------------------------------------------------------------------------


        public override string ToString ()
        {
            return String.Format ( "{0} {1}", this.Token, this.Reference );
        }

    }
}
