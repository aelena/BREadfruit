using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace BREadfruit.Exceptions
{
    public class MissingInClauseException : Exception, ISerializable
    {


        public MissingInClauseException ()
            : base ( Grammar.InvalidShowElementExceptionDefaultMessage )
        {
        }

        public MissingInClauseException ( string message )
            : base ( message )
        {
        }

        public MissingInClauseException ( string message, Exception inner )
            : base ( message, inner )
        {
        }

        protected MissingInClauseException ( SerializationInfo info, StreamingContext context )
            : base ( info, context )
        {
        }
    }
}
