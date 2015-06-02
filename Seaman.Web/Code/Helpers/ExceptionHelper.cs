using System;
using System.Globalization;

namespace Seaman.Web.Code.Helpers
{
    public static class ExceptionHelper
    {
        private const string ExceptionFormat = "\r\nException Class : {2}\r\nMessage : {0}\r\nStack Trace : {1}";

        /// <summary>
        /// Get exception text as stored in log.
        /// </summary>
        /// <param name="exception"></param>
        /// <returns></returns>
        public static string GetExceptionText(this Exception exception)
        {
            if (exception != null)
            {
                String exceptionText = String.Format(CultureInfo.InvariantCulture, ExceptionFormat, exception.Message,
                    exception.StackTrace, exception.GetType());
                if (exception.InnerException != null)
                    exceptionText += "\r\nInner exception: " + GetExceptionText(exception.InnerException);
                return exceptionText;
            }
            return String.Empty;
        }
    }
}