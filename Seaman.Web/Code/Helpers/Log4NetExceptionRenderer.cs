using System;
using System.IO;
using log4net.ObjectRenderer;

namespace Seaman.Web.Code.Helpers
{
    public class Log4NetExceptionRenderer : IObjectRenderer
    {
        public void RenderObject(RendererMap rendererMap, object obj, TextWriter writer)
        {
            var exception = obj as Exception;
            if (exception == null)
                return;
            var info = exception.GetExceptionText();
            
            var additional = String.Empty;//exception.GetExtendedInfo();
            if (!string.IsNullOrWhiteSpace(additional))
                info += "\r\nAdditional Info:" + additional;
            writer.Write(info);
        }
    }
}