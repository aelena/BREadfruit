using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BREadfruit.Clauses;

namespace BREadfruit
{
    /// <summary>
    /// Represents an entire entity in the configuration file
    /// for business rules.
    /// </summary>
    public class Entity
    {

        private readonly string _name;
        public string Name
        {
            get { return _name; }
        }

        private readonly string _typeDescription;
        public string TypeDescription
        {
            get { return _typeDescription; }
        }

        private readonly string _businessProcessCode;
        public string BusinessProcessCode
        {
            get { return _businessProcessCode; }
        } 

        public Type Type { get; set; }
        /// <summary>
        /// Allows a caller to explicitly set the Type. This Type
        /// will probably be known only in the scope of specific applications
        /// or assemblies loaded later.
        /// </summary>
        /// <param name="t"></param>
        public void SetType(Type t)
        {
            this.Type = t;
        }


        private IList<DefaultClause> _defaults;

        // ---------------------------------------------------------------------------------
        

        public Entity ( string name, string typeDescription, string bpCode = "")
        {
            this._name = name;
            this._typeDescription = typeDescription;
            this._businessProcessCode = bpCode;
            this._defaults = new List<DefaultClause> ();
        }



        // ---------------------------------------------------------------------------------


        public bool AddDefaultClause(DefaultClause defaultClause)
        {
            this._defaults.Add ( defaultClause );
            return true;
        }


        // ---------------------------------------------------------------------------------


    }
}
