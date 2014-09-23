using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace BREadfruit.Exceptions
{
    class InvalidEntityDeclarationException : Exception, ISerializable
    {


        public InvalidEntityDeclarationException ()
            : base ( "Invalid Entity declaration found." )
        {
        }

        public InvalidEntityDeclarationException ( string message )
            : base ( message )
        {
        }

        public InvalidEntityDeclarationException ( string message, Exception inner )
            : base ( message, inner )
        {
        }

        protected InvalidEntityDeclarationException ( SerializationInfo info, StreamingContext context ) : base (info, context)
        {
        }
    }
}
