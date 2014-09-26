using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace BREadfruit.Exceptions
{
    public class InvalidWithClauseException : Exception, ISerializable
    {


        public InvalidWithClauseException ()
            : base ( Grammar.InvalidWithClauseExceptionDefaultMessage )
        {
        }

        public InvalidWithClauseException ( string message )
            : base ( message )
        {
        }

        public InvalidWithClauseException ( string message, Exception inner )
            : base ( message, inner )
        {
        }

        protected InvalidWithClauseException ( SerializationInfo info, StreamingContext context )
            : base ( info, context )
        {
        }
    }
}
