using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using BREadfruit.Clauses;
using BREadfruit.Conditions;

namespace BREadfruit
{
    /// <summary>
    /// Represents an entire entity in the configuration file
    /// for business rules.
    /// </summary>
    public class Entity
    {

        /// <summary>
        /// Name of the entity
        /// </summary>
        private readonly string _name;
        /// <summary>
        /// Gets the name of the Entity
        /// </summary>
        public string Name
        {
            get { return _name; }
        }

        private readonly string _typeDescription;
        /// <summary>
        /// Gets the type of the Entity (textbox, div, button, etc.)
        /// </summary>
        public string TypeDescription
        {
            get { return _typeDescription; }
        }

        private readonly string _businessProcessCode;
        /// <summary>
        /// Gets the BP to which this entity belongs to.
        /// </summary>
        public string BusinessProcessCode
        {
            get { return _businessProcessCode; }
        }

        private readonly string _form;
        /// <summary>
        /// Gets the BP to which this entity belongs to.
        /// </summary>
        public string Form
        {
            get { return _form; }
        }


        public Type Type { get; set; }
        /// <summary>
        /// Allows a caller to explicitly set the Type. This Type
        /// will probably be known only in the scope of specific applications
        /// or assemblies loaded later.
        /// </summary>
        /// <param name="t"></param>
        public void SetType ( Type t )
        {
            this.Type = t;
        }


        // ---------------------------------------------------------------------------------


        private IList<DefaultClause> _defaults;
        private IList<Rule> _rules;
        private IList<ResultAction> _resultactions;
        private IList<UnaryAction> _unaryactions;
        private IList<Trigger> _triggers;
        private IList<Constraint> _constraints;


        public IEnumerable<Rule> Rules
        {
            get
            {
                return this._rules;
            }
        }

        // ---------------------------------------------------------------------------------


        public IEnumerable<DefaultClause> Defaults
        {
            get
            {
                return this._defaults;
            }
        }

        // ---------------------------------------------------------------------------------


        public IEnumerable<UnaryAction> ConditionlessActions
        {
            get
            {
                // cast to list, in order to get rid of the ConcatIterator and the two "first" and "second" properties that appear upon inspection
                return this._unaryactions.Concat ( this._resultactions ).ToList ();
            }
        }

        // ---------------------------------------------------------------------------------

        public IEnumerable<Trigger> Triggers
        {
            get
            {
                return this._triggers;
            }
        }

        // ---------------------------------------------------------------------------------

        public IEnumerable<Constraint> Constraints
        {
            get
            {
                return this._constraints;
            }
        }

        // ---------------------------------------------------------------------------------


        public Entity ( string name, string typeDescription, string formName, string bpCode = "" )
        {
            this._name = name;
            this._typeDescription = typeDescription;
            this._businessProcessCode = bpCode;
            this._form = formName.Replace ( "\"", "" );
            this._defaults = new List<DefaultClause> ();
            this._rules = new List<Rule> ();
            this._resultactions = new List<ResultAction> ();
            this._unaryactions = new List<UnaryAction> ();
            this._triggers = new List<Trigger> ();
            this._constraints = new List<Constraint> ();
        }



        // ---------------------------------------------------------------------------------


        public bool AddDefaultClause ( DefaultClause defaultClause )
        {

            if ( defaultClause == null )
                throw new ArgumentNullException ( "Default clause cannot be null." );

			// cannot add duplicate default clauses except if we are talking column definitions for grids
			if ( defaultClause.Token == Grammar.DefineColumnDefaultClause )
			{
				this._defaults.Add ( defaultClause );
				return true;
			}
			else
			{
				if ( !this._defaults.Contains ( defaultClause ) )
				{
					this._defaults.Add ( defaultClause );
					return true;
				}
			}

            return false;

        }


        // ---------------------------------------------------------------------------------


        public bool AddRule ( Rule rule )
        {
            this._rules.Add ( rule );
            return true;
        }

        // ---------------------------------------------------------------------------------

        public bool AddResultAction ( ResultAction action )
        {
            this._resultactions.Add ( action );
            return true;
        }

        // ---------------------------------------------------------------------------------

        public bool AddUnaryAction ( UnaryAction action )
        {
            this._unaryactions.Add ( action );
            return true;
        }

        // ---------------------------------------------------------------------------------

        public bool AddTrigger ( Trigger trigger )
        {
            this._triggers.Add ( trigger );
            return true;
        }

        // ---------------------------------------------------------------------------------


        public bool AddConstraint ( Constraint constraint )
        {
            // check the constraint value is ok
            if ( Regex.IsMatch ( constraint.Name, Grammar.ConstraintLineRegex, RegexOptions.IgnoreCase ) )
            {
                this._constraints.Add ( constraint );
                return this._constraints.Contains ( constraint );
            }
            else
                throw new InvalidOperationException ( String.Format ( "Added wrong or unknown constraint type ('{0}')", constraint.Name ) );
        }

        // ---------------------------------------------------------------------------------


        public override string ToString ()
        {
            return String.Format ( "Entity {0} is {1}", this.Name, this.TypeDescription );
        }


        // ---------------------------------------------------------------------------------


    }
}
