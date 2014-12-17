using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace BREadfruit.Exceptions
{
    public class InvalidElseStatementClauseException : Exception, ISerializable
    {


        public InvalidElseStatementClauseException ()
            : base ( Grammar.InvalidElseStatementClauseExceptionDefaultMessage )
        {
        }

        public InvalidElseStatementClauseException ( string message )
            : base ( message )
        {
        }

        public InvalidElseStatementClauseException ( string message, Exception inner )
            : base ( message, inner )
        {
        }

		protected InvalidElseStatementClauseException ( SerializationInfo info, StreamingContext context )
            : base ( info, context )
        {
        }
    }
}
