using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace BREadfruit.Exceptions
{
    public class InvalidLineFoundException : Exception, ISerializable
    {


        public InvalidLineFoundException ()
            : base ( Grammar.InvalidLineFoundExceptionDefaultMessage )
        {
        }

        public InvalidLineFoundException ( string message )
            : base ( message )
        {
        }

        public InvalidLineFoundException ( string message, Exception inner )
            : base ( message, inner )
        {
        }

        protected InvalidLineFoundException ( SerializationInfo info, StreamingContext context )
            : base ( info, context )
        {
        }
    }
}
