using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BREadfruit.Exceptions;
using BREadfruit.Helpers;

namespace BREadfruit
{
    /// <summary>
    /// Represents information about a parsed line.
    /// </summary>
    public class LineInfo
    {
        private LineParser _lineParser = new LineParser ();

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
        private int _numberOfTokens;

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

        private string _representation;

        public string Representation
        {
            get { return _representation; }
        }

        private readonly bool _isEntityLine;
        public bool IsEntityLine
        {
            get { return this._isEntityLine; }
        }

        private readonly bool _isWithLine;
        public bool IsWithLine
        {
            get { return this._isWithLine; }
        }

        // ---------------------------------------------------------------------------------


        public LineInfo ( string representation )
        {
            this._representation = representation;
            this._indentLevel = this._lineParser.GetIndentCount ( this._representation );
            this._tokens = this._lineParser.ExtractTokens ( this._representation ).ToList ();
            if ( this._tokens != null )
                this._numberOfTokens = this._tokens.Count ();
            this._isValid = this._lineParser.IsAValidSentence ( this );
        }


        // ---------------------------------------------------------------------------------


        public LineInfo ( string representation, int indentLevel, IEnumerable<Symbol> tokens )
        {
            {
                this._representation = representation;
                this._indentLevel = indentLevel;
                if ( tokens != null )
                {
                    this._tokens = tokens.ToList ();
                    this._numberOfTokens = tokens.Count ();
                }
                this._isValid = this._lineParser.IsAValidSentence ( this );
                if ( tokens.Count () > 0 )
                {
                    this._isEntityLine = _tokens.First () == Grammar.EntitySymbol;
                    this._isWithLine = _tokens.First () == Grammar.WithSymbol;
                }
            }
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
            // first lets check for the presence of specific explicit "with_args" symbol, which
            // indicates a list of arguments to be passed (generally in calls to datasource or web service)
            if ( this.HasSymbol ( Grammar.WithArgumentsSymbol ) )
                return this._representation.TakeBetween ( Grammar.WithArgumentsSymbol.Token, "{", "}", trimResults: true );
            //return this._representation.SubStringAfter ( Grammar.WithArgumentsSymbol.Token, trimResults: true );
            return String.Empty;
        }


        // ---------------------------------------------------------------------------------


        protected internal Tuple<string, int, int> TokenizeValueListInCondition ()
        {
            // reeturn immediately in this case
            if ( this.IsEntityLine || this.IsWithLine )
                return null;

            if ( !this.HasSymbol ( Grammar.IsNotEmptyOperator )
                 && ( this.HasSymbol ( Grammar.InSymbol ) || this.HasSymbol ( Grammar.IsOperator ) || this.HasSymbol ( Grammar.IsNotOperator ) ) )
            {
                int i1 = 0, i2 = 0;
                if ( this.HasSymbol ( Grammar.StartsWithOperator ) && this.HasSymbol ( Grammar.ThenSymbol ) )
                {
                    i1 = this.IndexOfSymbol ( Grammar.StartsWithOperator ) + 1;
                    i2 = this.IndexOfSymbol ( Grammar.ThenSymbol ) - 1;
                    if ( i1 <= i2 )
                        return new Tuple<string, int, int> ( this.Tokens.JoinTogetherBetween ( i1, i2 ).Token, i1, i2 );
                }
                if ( this.HasSymbol ( Grammar.InSymbol ) && this.HasSymbol ( Grammar.ThenSymbol ) )
                {
                    i1 = this.IndexOfSymbol ( Grammar.InSymbol ) + 1;
                    i2 = this.IndexOfSymbol ( Grammar.ThenSymbol ) - 1;
                    if ( i1 <= i2 )
                        return new Tuple<string, int, int> ( this.Tokens.JoinTogetherBetween ( i1, i2 ).Token, i1, i2 );
                }
                if ( this.HasSymbol ( Grammar.IsNotOperator ) && this.HasSymbol ( Grammar.ThenSymbol ) )
                {
                    i1 = this.IndexOfSymbol ( Grammar.IsNotOperator ) + 1;
                    i2 = this.IndexOfSymbol ( Grammar.ThenSymbol ) - 1;
                    if ( i1 <= i2 )
                        return new Tuple<string, int, int> ( this.Tokens.JoinTogetherBetween ( i1, i2 ).Token, i1, i2 );
                }
                if ( this.HasSymbol ( Grammar.IsOperator ) && this.HasSymbol ( Grammar.ThenSymbol ) )
                {
                    i1 = this.IndexOfSymbol ( Grammar.IsOperator ) + 1;
                    i2 = this.IndexOfSymbol ( Grammar.ThenSymbol ) - 1;
                    if ( i1 <= i2 )
                        return new Tuple<string, int, int> ( this.Tokens.JoinTogetherBetween ( i1, i2 ).Token, i1, i2 );
                }

            }
            return null;
        }


        // ---------------------------------------------------------------------------------


        public override string ToString ()
        {
            return this.Representation;
        }


        // ---------------------------------------------------------------------------------


        /// <summary>
        /// Returns a boolean value that indicates if the instance contains
        /// a specific symbol within its tokens.
        /// </summary>
        /// <param name="symbol">Symbol from the Grammar that is to be searched.</param>
        /// <returns>Returns a boolean value that indicates if the instance contains the passed symbol.</returns>
        internal bool HasSymbol ( Symbol symbol )
        {
            if ( symbol != null )
            {
                // first search for Symbol's main token
                if ( this._representation.Contains ( symbol.Token ) )
                    return true;

                // then, if there are aliases, search for them
                if ( symbol.Aliases.Count () > 0 )
                    return this._representation.ContainsAny ( symbol.Aliases );
            }
            return false;
        }


        // ---------------------------------------------------------------------------------


        /// <summary>
        /// Returns the index of a Symbol instance within
        /// the instance's Tokens collection.
        /// </summary>
        /// <param name="symbol"></param>
        /// <returns>Returns the index of a Symbol instance or 
        /// -1 if the Symbol is not found.</returns>
        internal int IndexOfSymbol ( Symbol symbol )
        {
            if ( this.HasSymbol ( symbol ) )
                return this._tokens.ToList ().IndexOf ( symbol );
            return -1;
        }


        // ---------------------------------------------------------------------------------


        /// <summary>
        /// Adds a Token to the internal collection.
        /// </summary>
        /// <param name="s">Token's Symbol to be added.</param>
        /// <returns></returns>
        protected internal IEnumerable<Symbol> AddToken ( Symbol s )
        {
            if ( this._tokens == null )
                this._tokens = new List<Symbol> ();
            this._tokens.Add ( s );
            this._numberOfTokens = this._tokens.Count ();
			this._representation = this.Tokens.JoinTogether ().Token;
            return this.Tokens;
        }

        // ---------------------------------------------------------------------------------


        /// <summary>
        /// Adds a Token to the internal collection.
        /// </summary>
        /// <param name="s">Token's Symbol to be added.</param>
        /// <returns></returns>
        protected internal IEnumerable<Symbol> AddTokens ( IEnumerable<Symbol> symbols )
        {
            if ( this._tokens == null )
                this._tokens = new List<Symbol> ();
            this._tokens.AddRange ( symbols );
            this._numberOfTokens = this._tokens.Count ();
			this._representation = this.Tokens.JoinTogether ().Token;
			return this.Tokens;
        }


        // ---------------------------------------------------------------------------------


        /// <summary>
        /// Starting from the specified index (0 or higher), it removes all
        /// tokens that come after that index.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        protected internal IEnumerable<Symbol> RemoveTokensFromIndex ( int index )
        {
            if ( index >= 0 )
            {
                this._tokens = this._tokens.Take ( index ).ToList ();
                this._numberOfTokens = this._tokens.Count ();
				this._representation = this.Tokens.JoinTogether ().Token;
				return this.Tokens;
            }
            else
                throw new ArgumentException ( "You cannot remove all tokens or specify a negative index.", "index" );
        }


        // ---------------------------------------------------------------------------------


        /// <summary>
        /// Removes all tokens after the first occurrence of a specific symbol.
        /// Has an optional parameter that allows a caller to indicate if 
        /// the specific symbol is to be cut off or be included.
        /// </summary>
        /// <param name="s"></param>
        /// <param name="includeCurrentToken"></param>
        /// <returns></returns>
        protected internal IEnumerable<Symbol> RemoveTokensAfterSymbol ( Symbol s, bool includeCurrentToken = false )
        {

            var index = this.IndexOfSymbol ( s );

            if ( index >= 0 )
                this._tokens = this._tokens.Take ( includeCurrentToken ? index + 1 : index ).ToList ();

            this._numberOfTokens = this._tokens.Count ();
			this._representation = this.Tokens.JoinTogether ().Token;

            return this.Tokens;

        }


        // ---------------------------------------------------------------------------------


		public void RemoveTokensAfter ( Func<Symbol, bool> func )
		{
			int i = 0;
			foreach ( var l in this.Tokens )
			{
				if ( func ( l ) )
				{
					this._tokens = this.Tokens.Take ( ++i ).ToList ();
					this._representation = this.Tokens.JoinTogether ().Token;
					this._numberOfTokens = this.Tokens.Count ();
				}
				i++;
			}
		}


		// ---------------------------------------------------------------------------------


		public void RemoveTokensFrom ( Func<Symbol, bool> func )
		{
			int i = 0;
			foreach ( var l in this.Tokens )
			{
				if ( func ( l ) )
				{
					this._tokens = this.Tokens.Take ( i ).ToList ();
					this._representation = this.Tokens.JoinTogether ().Token;
					this._numberOfTokens = this.Tokens.Count ();
				}
				i++;
			}
		}


		// ---------------------------------------------------------------------------------

        /// <summary>
        /// 
        /// 
        /// This is a command (which is why it has return type void) that modifies the state of the current
        /// instance's token collection.
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        internal void RemoveTokensFromTo ( int from, int to )
        {
            if ( from < 0 )
                throw new ArgumentException ( "Cannot specify a starting position lower than 0", "from" );
            if ( to < from )
                throw new ArgumentException ( "Cannot specify a upper index lower than the starting index", "to" );

            this._tokens = this._tokens.Take ( from ).Concat ( this._tokens.Skip ( to ) ).ToList ();
            this._numberOfTokens = this._tokens.Count ();
			this._representation = this.Tokens.JoinTogether ().Token;

        }


        // ---------------------------------------------------------------------------------


        internal void InsertTokenAt ( int index, Symbol token )
        {
            this._tokens.Insert ( index, token );
        }


        // ---------------------------------------------------------------------------------


        /// <summary>
        /// Given a symbol S searches in the current instance until that token is found
        /// returning only the tokens before that position.
        /// The flag allows the caller to specify whether to include or not the search token
        /// as part of the results.
        /// This method does not modify the state of the current token list in the present instance.
        /// </summary>
        /// <param name="s">Symbol to be looked for.</param>
        /// <param name="includeSearchToken">allows the caller to specify whether to include or not the search token
        /// as part of the results</param>
        /// <returns>A list of symbols or throws an exception if the Symbol s is not found.</returns>
        protected internal IEnumerable<Symbol> TakeFrom ( Symbol s, bool includeSearchToken = false )
        {
            var index = this.IndexOfSymbol ( s );

            if ( index >= 0 )
                return this._tokens.Skip ( !includeSearchToken ? index + 1 : index ).ToList ();
            else
                throw new TokenNotFoundException ( String.Format ( Grammar.TokenNotFoundExceptionDefaultTemplate, s.Token, this.Representation ) );
        }


        // ---------------------------------------------------------------------------------


        /// <summary>
        /// Given a symbol S searches in the current instance until that token is found
        /// returning only the tokens after that position.
        /// The flag allows the caller to specify whether to include or not the search token
        /// as part of the results.
        /// This method does not modify the state of the current token list in the present instance.
        /// </summary>
        /// <param name="s">Symbol to be looked for.</param>
        /// <param name="includeSearchToken">allows the caller to specify whether to include or not the search token
        /// as part of the results</param>
        /// <returns>A list of symbols or throws an exception if the Symbol s is not found.</returns>
        protected internal IEnumerable<Symbol> TakeUntil ( Symbol s, bool includeSearchToken = false )
        {
            var index = this.IndexOfSymbol ( s );

            if ( index >= 0 )
                return this._tokens.Take ( includeSearchToken ? index + 1 : index ).ToList ();
            else
                throw new TokenNotFoundException ( String.Format ( Grammar.TokenNotFoundExceptionDefaultTemplate, s.Token, this.Representation ) );

        }



        // ---------------------------------------------------------------------------------


    }
}
