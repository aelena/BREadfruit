using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace BREadfruit.Exceptions
{
    public class TokenNotFoundException : Exception, ISerializable
    {


        public TokenNotFoundException ()
            : base ( Grammar.MissingThenClauseExceptionDefaultMessage )
        {
        }

        public TokenNotFoundException ( string message )
            : base ( message )
        {
        }

        public TokenNotFoundException ( string message, Exception inner )
            : base ( message, inner )
        {
        }

        protected TokenNotFoundException ( SerializationInfo info, StreamingContext context )
            : base ( info, context )
        {
        }
    }
}
