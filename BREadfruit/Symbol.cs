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
        public string Token { get; protected set; }

        /// <summary>
        /// Gets the indentation level where the token can live.
        /// For example, a symbol with IndentLevel = 1 cannot appear 
        /// at the beginning of a line or at a second level of indentation (2 tabs).
        /// </summary>
        protected internal int IndentLevel { get; protected set; }

        /// <summary>
        /// Gets a boolean value that indicates whether this is
        /// a terminal or nonterminal.
        /// </summary>
        protected internal bool IsTerminal { get; protected set; }

        /// <summary>
        /// Private list of other Symbols that can be considered children
        /// or siblings of this instance (for the time being, no distinction
        /// is made between child and sibling).
        /// </summary>
        private IList<Symbol> _validChildren = new List<Symbol> ();


        // ---------------------------------------------------------------------------------


        /// <summary>
        /// Stores other syntax that might be available for this same operator.
        /// For example, we can accept 'bigger or equals than' or '>=' as well 
        /// within the valid file syntax
        /// </summary>
        protected List<String> _aliases = new List<string> ();
        protected internal IEnumerable<String> Aliases
        {
            get
            {
                return this._aliases;
            }
        }


        // ---------------------------------------------------------------------------------


        /// <summary>
        /// Clients call pass a specified token ( any arbitrary string actually )
        /// and get a value that indicates whether that token is an valid and 
        /// known alias for this Operator.
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        protected internal bool MatchesToken ( string token )
        {
            if ( !String.IsNullOrWhiteSpace ( token ) )
            {
                if ( this._aliases != null )
                {
                    var matched = from x in _aliases
                                  where x == token
                                  select x;

                    return ( matched != null && matched.Count () > 0 );
                }
            }
            return false;

        }


        // ---------------------------------------------------------------------------------


        /// <summary>
        /// Enumerable list of children for callers or validators to consume.
        /// </summary>
        protected internal IEnumerable<Symbol> Children
        {
            get
            {
                return this._validChildren;
            }
        }


        // ---------------------------------------------------------------------------------


        /// <summary>
        /// Basic constructors. Assumes that the Symbol will be terminal,
        /// meaning no Children can be added afterwards. If you want to add
        /// children use the other constructor.
        /// </summary>
        /// <param name="symbol">string representation of the token, for example
        /// 'Entity' or 'with'.
        /// </param>
        protected internal Symbol ( string symbol, int indentLevel )
        {
            this.Token = symbol;
            this.IndentLevel = indentLevel;
            this.IsTerminal = true;
        }


        // ---------------------------------------------------------------------------------


        /// <summary>
        /// Basic constructors. Assumes that the Symbol will be terminal,
        /// meaning no Children can be added afterwards. If you want to add
        /// children use the other constructor.
        /// </summary>
        /// <param name="symbol">string representation of the token, for example
        /// 'Entity' or 'with'.
        /// </param>
        protected internal Symbol ( string symbol, int indentLevel, IEnumerable<string> aliases )
        {
            this.Token = symbol;
            this.IndentLevel = indentLevel;
            this.IsTerminal = true;
            if ( aliases != null )
                this._aliases = aliases.ToList ();

        }


        // ---------------------------------------------------------------------------------


        /// <summary>
        /// Constructors that allows the caller to create a symbol and also
        /// to specify if this will be a terminal or a nonterminal.
        /// </summary>
        /// <param name="symbol">string representation of the token, for example
        /// 'Entity' or 'with'.
        /// </param>
        /// <param name="isTerminal">Pass false to create a nonterminal symbol, otherwise
        /// pass true to create a terminal.</param>
        protected internal Symbol ( string symbol, int indentLevel, bool isTerminal )
        {
            this.Token = symbol;
            this.IndentLevel = indentLevel;
            this.IsTerminal = isTerminal;
        }


        // ---------------------------------------------------------------------------------


        /// <summary>
        /// Constructors that allows the caller to create a symbol and also
        /// to specify if this will be a terminal or a nonterminal.
        /// </summary>
        /// <param name="symbol">string representation of the token, for example
        /// 'Entity' or 'with'.
        /// </param>
        /// <param name="isTerminal">Pass false to create a nonterminal symbol, otherwise
        /// pass true to create a terminal.</param>
        protected internal Symbol ( string symbol, int indentLevel, bool isTerminal, IEnumerable<string> aliases )
        {
            this.Token = symbol;
            this.IndentLevel = indentLevel;
            this.IsTerminal = isTerminal;
            if ( aliases != null )
                this._aliases = aliases.ToList ();
        }

        // ---------------------------------------------------------------------------------


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
        protected internal Symbol ( string symbol, int indentLevel, IEnumerable<string> aliases, IList<Symbol> validChildren )
        {
            this.Token = symbol;
            this.IndentLevel = indentLevel;
            this.IsTerminal = false;
            this._validChildren = validChildren;
            if ( aliases != null )
                this._aliases = aliases.ToList ();

        }


        // ---------------------------------------------------------------------------------


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
        protected internal Symbol ( string symbol, int indentLevel, IList<Symbol> validChildren )
        {
            this.Token = symbol;
            this.IndentLevel = indentLevel;
            this.IsTerminal = false;
            this._validChildren = validChildren;
        }


        // ---------------------------------------------------------------------------------


        /// <summary>
        /// Allows a caller to add child Symbols to a nonterminal Symbol.
        /// </summary>
        /// <param name="s"></param>
        protected internal void AddValidChild ( Symbol s )
        {
            if ( this.IsTerminal )
                throw new InvalidOperationException ( "This is a terminal symbol. No symbols can be added." );

            if ( this._validChildren != null )
                this._validChildren.Add ( s );

            // if any sort of child Symbol is added, then this cannot be a nonterminal
            this.IsTerminal = false;
        }


        // ---------------------------------------------------------------------------------

        protected internal IEnumerable<String> ChildTokens
        {
            get
            {
                return from c in this._validChildren
                       select c.Token;
            }
        }


        // ---------------------------------------------------------------------------------


        // override object.Equals
        public override bool Equals ( object obj )
        {

            if ( obj == null || this.GetType () != obj.GetType () )
            {
                return false;
            }

            Symbol _s = ( Symbol ) obj;

            if ( this.Token != _s.Token )
                return false;

            if ( this.IsTerminal != _s.IsTerminal )
                return false;

            if ( this.IndentLevel != _s.IndentLevel )
                return false;

            if ( this.Children.Count () != _s.Children.Count () )
                return false;

            var diffSet = this.Children.Except ( _s.Children );
            if ( diffSet != null && diffSet.Count () > 0 )
                return false;

            return true;
        }


        // ---------------------------------------------------------------------------------


        // override object.GetHashCode
        public override int GetHashCode ()
        {

            /*
             * Domain-Driven Design makes the distinction between Entities and Value Objects. 
             * This is a good distinction to observe since it guides how you implement Equals.
             * Entities are equal if their IDs equal each other.
             * Value Objects are equal if all their (important) constituent elements are equal to each other.
             * In any case, the implementation of GetHashCode should base itself on the same values 
             * that are used to determine equality. In other words, for Entities, 
             * the hash code should be calculated directly from the ID, 
             * whereas for Value Objects it should be calculated from all the constituent values. 
             */

            unchecked // Overflow is fine, just wrap
            {
                int hash = 17;
                // Suitable nullity checks etc, of course :)
                hash = hash * 23 + this.Token.GetHashCode ();
                hash = hash * 23 + this.IndentLevel.GetHashCode ();
                hash = hash * 23 + this.IsTerminal.GetHashCode ();
                return hash;
            }
        }


        public override string ToString ()
        {
            return this.Token;
        }

    }


    // ---------------------------------------------------------------------------------



    // ---------------------------------------------------------------------------------

}
