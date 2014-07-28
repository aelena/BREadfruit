using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BREadfruit
{
    public class Symbol
    {

        /// <summary>
        /// Gets the string representation of the token, for example
        /// 'Entity' or 'with'.
        /// </summary>
        public string Token { get; private set; }

        /// <summary>
        /// Gets the indentation level where the token can live.
        /// For example, a symbol with IndentLevel = 1 cannot appear 
        /// at the beginning of a line or at a second level of indentation (2 tabs).
        /// </summary>
        public int IndentLevel { get; private set; }

        /// <summary>
        /// Gets a boolean value that indicates whether this is
        /// a terminal or nonterminal.
        /// </summary>
        public bool IsTerminal { get; private set; }

        /// <summary>
        /// Private list of other Symbols that can be considered children
        /// or siblings of this instance (for the time being, no distinction
        /// is made between child and sibling).
        /// </summary>
        private IList<Symbol> _validChildren = new List<Symbol> ();

        /// <summary>
        /// Enumerable list of children for callers or validators to consume.
        /// </summary>
        public IEnumerable<Symbol> Children
        {
            get
            {
                return this._validChildren;
            }
        }

        /// <summary>
        /// Basic constructors. Assumes that the Symbol will be terminal,
        /// meaning no Children can be added afterwards. If you want to add
        /// children use the other constructor.
        /// </summary>
        /// <param name="symbol">string representation of the token, for example
        /// 'Entity' or 'with'.
        /// </param>
        public Symbol ( string symbol, int indentLevel )
        {
            this.Token = symbol;
            this.IndentLevel = indentLevel;
            this.IsTerminal = true;
        }

        /// <summary>
        /// Constructors that allows the caller to create a symbol and also
        /// to specify if this will be a terminal or a nonterminal.
        /// </summary>
        /// <param name="symbol">string representation of the token, for example
        /// 'Entity' or 'with'.
        /// </param>
        /// <param name="isTerminal">Pass false to create a nonterminal symbol, otherwise
        /// pass true to create a terminal.</param>
        public Symbol ( string symbol, int indentLevel, bool isTerminal)
        {
            this.Token = symbol;
            this.IndentLevel = indentLevel;
            this.IsTerminal = isTerminal;
        }

        /// <summary>
        /// Constructors that allows the caller to create a symbol and also
        /// a list of valid children. By allowing the caller to pass a list of 
        /// valid children, the Symbol is assumed to be a nonterminal.
        /// </summary>
        /// <param name="symbol">string representation of the token, for example
        /// 'Entity' or 'with'.
        /// </param>
        /// <param name="validChildren">
        /// List of valid children for this Symbol instance.
        /// </param>
        public Symbol ( string symbol, int indentLevel, IList<Symbol> validChildren)
        {
            this.Token = symbol;
            this.IndentLevel = indentLevel;
            this.IsTerminal = false;
            this._validChildren = validChildren;
        }

        /// <summary>
        /// Allows a caller to add child Symbols to a nonterminal Symbol.
        /// </summary>
        /// <param name="s"></param>
        public void AddValidChild ( Symbol s )
        {
            if ( this.IsTerminal )
                throw new InvalidOperationException ( "This is a terminal symbol. No symbols can be added." );

            if ( this._validChildren != null )
                this._validChildren.Add ( s );

            // if any sort of child Symbol is added, then this cannot be a nonterminal
            this.IsTerminal = false;
        }


        public IEnumerable<String> ValidChildTokens()
        {
            return from c in this._validChildren
                          select c.Token;
        }
      
    }
}
