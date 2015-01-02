using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BREadfruit.Conditions
{
	public class UnaryAction : Symbol
	{

		/// <summary>
		/// Contains the name of the object / entity this unary constraint
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


		public PropertyType Property { get; protected internal set; }


		// ---------------------------------------------------------------------------------

		public virtual bool IsUnary
		{
			get
			{
				return true;
			}
		}


		// ---------------------------------------------------------------------------------


		public virtual bool IsResultAction
		{
			get
			{
				return false;
			}
		}

		// ---------------------------------------------------------------------------------


		/// <summary>
		/// 
		/// </summary>
		/// <param name="s"></param>
		internal UnaryAction ( Symbol s, string reference = "this" ) :
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
		internal UnaryAction ( string identifier, int indentLevel, bool isTerminal, IEnumerable<string> aliases = null, string reference = "this" )
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


		// ---------------------------------------------------------------------------------


		public static bool operator == ( UnaryAction action, Symbol y )
		{
			var t = Grammar.GetSymbolByToken ( action.Token, action.GetType () );
			return t == y;
		}


		// ---------------------------------------------------------------------------------


		public static bool operator != ( UnaryAction action, Symbol y )
		{
			var t = Grammar.GetSymbolByToken ( action.Token, action.GetType () );
			return t != y;
		}


		// ---------------------------------------------------------------------------------

		public override bool Equals ( object obj )
		{

			if ( obj == null )
				return false;

			if ( obj.GetType () != typeof ( UnaryAction ) )
				return false;

			var _ra = ( UnaryAction ) obj;

			// TODO: see if more is necessary in here .... 

			return
				( this.Token == _ra.Token &&
				  this.IsUnary == _ra.IsUnary &&
				  this.Action == _ra.Action &&
				  this.IsResultAction == _ra.IsResultAction );
		}


		// ---------------------------------------------------------------------------------


	}


	// ---------------------------------------------------------------------------------


	public enum PropertyType
	{
		LABEL,
		VALUE,
		INDEX
	}


	// ---------------------------------------------------------------------------------

}
