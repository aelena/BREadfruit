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
        private readonly IEnumerable<Symbol> _tokens;

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


        private LineInfo () { }

        public LineInfo ( string representation)
        {
            this._representation = representation;
            this._indentLevel = LineParser.GetIndentCount ( this._representation );
            this._tokens = LineParser.ExtractTokens ( this._representation );
            this._isValid = LineParser.IsAValidSentence ( this );
        }


        // ---------------------------------------------------------------------------------


        public LineInfo ( string representation, int indentLevel, IEnumerable<Symbol> tokens )
        {
            this._representation = representation;
            this._indentLevel = indentLevel;
            this._tokens = tokens;
            if ( tokens != null )
                this._numberOfTokens = tokens.Count ();
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


    }
}
