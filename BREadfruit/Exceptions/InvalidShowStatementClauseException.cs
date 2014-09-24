using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace BREadfruit.Exceptions
{
    public class InvalidShowStatementClauseException : Exception, ISerializable
    {


        public InvalidShowStatementClauseException ()
            : base ( Grammar.InvalidShowElementExceptionDefaultMessage )
        {
        }

        public InvalidShowStatementClauseException ( string message )
            : base ( message )
        {
        }

        public InvalidShowStatementClauseException ( string message, Exception inner )
            : base ( message, inner )
        {
        }

        protected InvalidShowStatementClauseException ( SerializationInfo info, StreamingContext context )
            : base ( info, context )
        {
        }
    }
}
