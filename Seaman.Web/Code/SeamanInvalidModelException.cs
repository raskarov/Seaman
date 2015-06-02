using System;
using System.Runtime.Serialization;
using System.Web.Http.ModelBinding;
using Seaman.Core;

namespace Seaman.Web.Code
{
    public sealed class SeamanInvalidModelException : SeamanException
    {

        #region Constructors

        public SeamanInvalidModelException()
        {
        }

        public SeamanInvalidModelException(ModelStateDictionary modelStateDictionary)
        {
            Data.Add(ModelStateKey, modelStateDictionary);
        }

        public SeamanInvalidModelException(string message)
            : base(message)
        {
        }

        public SeamanInvalidModelException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        private SeamanInvalidModelException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        } 

        #endregion


        public ModelStateDictionary ModelState
        {
            get
            {
                if (!Data.Contains(ModelStateKey))
                    return null;
                return (ModelStateDictionary)Data[ModelStateKey];

            }
        }

        private const String ModelStateKey = "modelstate";
    }
}