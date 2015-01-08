using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace BREadfruit.Exceptions
{
    public class UnexpectedClauseException : Exception, ISerializable
    {


        public UnexpectedClauseException ()
            : base ( Grammar.DuplicateEntityFoundExceptionDefaultMessage )
        {
        }

        public UnexpectedClauseException ( string message )
            : base ( message )
        {
        }

        public UnexpectedClauseException ( string message, Exception inner )
            : base ( message, inner )
        {
        }

		protected UnexpectedClauseException ( SerializationInfo info, StreamingContext context )
            : base ( info, context )
        {
        }
    }
}
