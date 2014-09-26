using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace BREadfruit.Exceptions
{
    public class MissingThenClauseException : Exception, ISerializable
    {


        public MissingThenClauseException ()
            : base ( Grammar.MissingThenClauseExceptionDefaultMessage )
        {
        }

        public MissingThenClauseException ( string message )
            : base ( message )
        {
        }

        public MissingThenClauseException ( string message, Exception inner )
            : base ( message, inner )
        {
        }

        protected MissingThenClauseException ( SerializationInfo info, StreamingContext context )
            : base ( info, context )
        {
        }
    }
}
