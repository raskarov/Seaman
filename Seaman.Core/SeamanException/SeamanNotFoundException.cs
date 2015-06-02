using System;
using System.Runtime.Serialization;

namespace Seaman.Core
{
    public class SeamanNotFoundException : SeamanException
    {
        public SeamanNotFoundException()
        {
            Code = SeamanResultCode.NotFound;
        }
        
        public SeamanNotFoundException(string message) : base(message)
        {
            Code = SeamanResultCode.NotFound;
        }

        public SeamanNotFoundException(string message, Exception innerException) : base(message, innerException)
        {
            Code = SeamanResultCode.NotFound;
        }

        protected SeamanNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
            Code = SeamanResultCode.NotFound;
        }
    }
}