using System;
using System.Runtime.Serialization;

namespace Seaman.Core
{
    public class SeamanException : ApplicationException
    {
        public SeamanException()
        {
        }

        public SeamanException(string message) : base(message)
        {
        }

        public SeamanException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected SeamanException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
        public String Code { get; set; }
    }


    public static class SeamanResultCode
    {
        public const String Undefined = null;
        public const String Ok = "";
        public const String UnknownError = "unknown_error";
        public const String InvalidGrant = "invalid_grant";
        public const String Forbidden = "forbidden";
        public const String DataValidationFailed = "data_validation_failed";
        public const String NotFound = "not_found";

    }
}