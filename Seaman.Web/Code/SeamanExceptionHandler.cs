using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.ExceptionHandling;
using System.Web.Http.Results;
using Common.Logging;
using Seaman.Core;

namespace Seaman.Web.Code
{
    public class SeamanExceptionHandler: ExceptionHandler
    {
        
        public override void Handle(ExceptionHandlerContext context)
        {
            //base.Handle(context);

            var httpStatusCode = HttpStatusCode.InternalServerError;
            var ex = (context.Exception as SeamanException) ?? (context.Exception.GetBaseException() as SeamanException);
            if (ex != null)
            {
                string errorCode = ex.Code;
                HttpError error;

                var ime = ex as SeamanInvalidModelException;
                if (ime != null)
                {
                    error = new HttpError(ime.ModelState, context.Request.ShouldIncludeErrorDetail()) { { "Code", errorCode } };  
                }
                else
                {
                    switch (ex.Code)
                    {
                        case SeamanResultCode.Undefined:
                            errorCode = SeamanResultCode.UnknownError;
                            break;
                        case SeamanResultCode.InvalidGrant:
                            httpStatusCode = HttpStatusCode.BadRequest;
                            break;
                        case SeamanResultCode.Forbidden:
                            httpStatusCode = HttpStatusCode.Forbidden;
                            break;
                        //case ApiResultCode.NotFound:
                        //case ApiResultCode.TargetNotFound:
                        //    httpError = HttpStatusCode.NotFound;
                        //    break;
                    }
                    error = new HttpError(ex, context.Request.ShouldIncludeErrorDetail()) { { "Code", errorCode } };  
                }
                

                var msg = context.Request.CreateResponse(httpStatusCode, error);
                context.Result = new ResponseMessageResult(msg);
                _log.WarnFormat("Astra exception. Code:{0} Message:{1}", ex, ex.Code, ex.Message);

            }
            else
            {
                _log.Error("Unhandled exception", context.Exception);
            }
            
            
        }

        #region Private

        private static readonly ILog _log = LogManager.GetLogger<SeamanExceptionHandler>(); 

        #endregion

    }

    
    
}