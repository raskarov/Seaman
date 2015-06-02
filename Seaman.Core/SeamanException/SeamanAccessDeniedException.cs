using System;
using System.Runtime.Serialization;

namespace Seaman.Core
{
    public class SeamanAccessDeniedException : SeamanException
    {
        public SeamanAccessDeniedException() : this("access denied")
        {
            Code = SeamanResultCode.Forbidden;
        }

        public SeamanAccessDeniedException(string message) : base(message)
        {
            Code = SeamanResultCode.Forbidden;
        }

        public SeamanAccessDeniedException(string message, Exception innerException) : base(message, innerException)
        {
            Code = SeamanResultCode.Forbidden;
        }

        protected SeamanAccessDeniedException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            Code = SeamanResultCode.Forbidden;
        }
    }
}