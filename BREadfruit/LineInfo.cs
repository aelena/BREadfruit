using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BREadfruit.Helpers;

namespace BREadfruit
{
    /// <summary>
    /// Represents information about a parsed line.
    /// </summary>
    public class LineInfo
    {

        /// <summary>
        /// Indicates the level of indent of the line 
        /// this instance represents.
        /// </summary>
        private readonly int _indentLevel;
        /// <summary>
        /// Gets the indent level of the current line instance.
        /// </summary>
        public int IndentLevel
        {
            get { return _indentLevel; }
        }

        /// <summary>
        /// Number of tokens found in this line
        /// </summary>
        private readonly int _numberOfTokens;

        /// <summary>
        /// Number of tokens found in this line
        /// </summary>
        public int NumberOfTokens
        {
            get { return _numberOfTokens; }
        }

        private bool _isValid;

        /// <summary>
        /// Indicates if the line is valid according to the rules of 
        /// the document format.
        /// </summary>
        public bool IsValid
        {
            get { return _isValid; }
        }

        /// <summary>
        /// Internal list of tokens.
        /// </summary>
        private List<Symbol> _tokens;

        /// <summary>
        /// Enumerable list of the tokens found in this instance.
        /// </summary>
        internal IEnumerable<Symbol> Tokens
        {
            get { return _tokens; }
        }

        private readonly string _representation;

        public string Representation
        {
            get { return _representation; }
        }


        // ---------------------------------------------------------------------------------


        public LineInfo ( string representation )
        {
            this._representation = representation;
            this._indentLevel = LineParser.GetIndentCount ( this._representation );
            this._tokens = LineParser.ExtractTokens ( this._representation ).ToList ();
            if ( this._tokens != null )
                this._numberOfTokens = this._tokens.Count ();
            this._isValid = LineParser.IsAValidSentence ( this );
        }


        // ---------------------------------------------------------------------------------


        public LineInfo ( string representation, int indentLevel, IEnumerable<Symbol> tokens )
        {
            this._representation = representation;
            this._indentLevel = indentLevel;
            if ( tokens != null )
            {
                this._tokens = tokens.ToList ();
                this._numberOfTokens = tokens.Count ();
            }
            this._isValid = LineParser.IsAValidSentence ( this );

        }


        // ---------------------------------------------------------------------------------


        /// <summary>
        /// Returns a value that indicates if a certain instance of LineInfo correspond to a 
        /// condition with the following pattern:
        /// 
        /// this starts with 0 then disable
        /// 
        /// Basically, the check is that 'then' is the penultimate item
        /// and that the last item is a valid unary result.
        /// </summary>
        /// <param name="lineInfo"></param>
        /// <returns></returns>
        public static bool RepresentsUnaryResultCondition ( LineInfo lineInfo )
        {
            return lineInfo.Tokens.Penultimate () == Grammar.ThenSymbol;
        }


        // ---------------------------------------------------------------------------------


        public IEnumerable<Symbol> GetSymbolsAfterThen ()
        {
            var thenClause = this.Tokens.SkipWhile ( x => x != Grammar.ThenSymbol );
            return thenClause.ToList ().Skip ( 1 );
        }


        // ---------------------------------------------------------------------------------


        public string GetArgumentsAsString ()
        {
            if ( this.HasSymbol ( Grammar.WithArgumentsSymbol ) )
            {
                return this._representation.SubStringAfter ( Grammar.WithArgumentsSymbol.Token, trimResults: true );
            }
            return String.Empty;
        }

        // ---------------------------------------------------------------------------------


        public override string ToString ()
        {
            return this.Representation;
        }


        // ---------------------------------------------------------------------------------


        internal bool HasSymbol ( Symbol symbol )
        {
            if ( symbol != null )
            {
                if ( this._representation.Contains ( symbol.Token ) )
                    return true;

                if ( symbol.Aliases.Count () > 0 )
                {
                    return this._representation.ContainsAny ( symbol.Aliases );
                }
            }
            return false;
        }


        // ---------------------------------------------------------------------------------


        internal int IndexOfSymbol ( Symbol symbol )
        {
            if ( this.HasSymbol ( symbol ) )
            {
                return this._tokens.ToList ().IndexOf ( symbol );
            }
            return -1;
        }


        // ---------------------------------------------------------------------------------


        protected internal IEnumerable<Symbol> AddToken ( Symbol s )
        {
            if ( this._tokens == null )
                this._tokens = new List<Symbol> ();
            this._tokens.Add ( s );
            return this.Tokens;
        }

        // ---------------------------------------------------------------------------------


        protected internal IEnumerable<Symbol> RemoveTokensFromIndex ( int index )
        {
            if ( index >= 0 )
            {
                this._tokens = this._tokens.Take ( index ).ToList ();
                return this.Tokens;
            }
            else
                throw new ArgumentException ( "You cannot remove all tokens or specify a negative index.", "index" );
        }


        // ---------------------------------------------------------------------------------


        protected internal IEnumerable<Symbol> RemoveTokensAfterSymbol ( Symbol s, bool includeCurrentToken = false )
        {

            var index = this.IndexOfSymbol ( s );

            if ( index >= 0 )
                this._tokens = this._tokens.Take ( includeCurrentToken ? index + 1 : index ).ToList ();

            return this.Tokens;


        }

    }
}
