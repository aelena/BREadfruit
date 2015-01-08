using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace BREadfruit.Exceptions
{
    public class DuplicateEntityFoundException : Exception, ISerializable
    {


        public DuplicateEntityFoundException ()
            : base ( Grammar.DuplicateEntityFoundExceptionDefaultMessage )
        {
        }

        public DuplicateEntityFoundException ( string message )
            : base ( message )
        {
        }

        public DuplicateEntityFoundException ( string message, Exception inner )
            : base ( message, inner )
        {
        }

        protected DuplicateEntityFoundException ( SerializationInfo info, StreamingContext context )
            : base ( info, context )
        {
        }
    }
}
