using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace BREadfruit.Exceptions
{
    public class InvalidHideStatementClauseException : Exception, ISerializable
    {


        public InvalidHideStatementClauseException ()
            : base ( Grammar.InvalidHideElementExceptionDefaultMessage )
        {
        }

        public InvalidHideStatementClauseException ( string message )
            : base ( message )
        {
        }

        public InvalidHideStatementClauseException ( string message, Exception inner )
            : base ( message, inner )
        {
        }

        protected InvalidHideStatementClauseException ( SerializationInfo info, StreamingContext context )
            : base ( info, context )
        {
        }
    }
}
