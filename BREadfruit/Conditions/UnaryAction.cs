using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BREadfruit.Conditions
{
    public class UnaryAction : AliasedSymbol
    {

        /// <summary>
        /// 
        /// </summary>
        /// <param name="s"></param>
        public UnaryAction ( Symbol s ) :
            base ( s.Token, s.IndentLevel, s.IsTerminal )
        {
            if ( s.Children != null )
                foreach ( var c in s.Children )
                    base.AddValidChild ( c );
        }


        // ---------------------------------------------------------------------------------


        /// <summary>
        /// 
        /// </summary>
        /// <param name="identifier"></param>
        /// <param name="indentLevel"></param>
        /// <param name="isTerminal"></param>
        /// <param name="aliases"></param>
        public UnaryAction ( string identifier, int indentLevel, bool isTerminal, IEnumerable<string> aliases = null )
            : base ( identifier, indentLevel, isTerminal )
        {
            if ( String.IsNullOrWhiteSpace ( identifier ) )
                throw new ArgumentNullException ( "identifier", "The identifier for a ResultAction instance cannot be null" );

            this.Token = identifier;

            if ( aliases != null )
                this._aliases.AddRange ( aliases );

        }


        // ---------------------------------------------------------------------------------

    }
}
