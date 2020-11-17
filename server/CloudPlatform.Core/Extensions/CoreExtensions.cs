using System;
using System.Text;

namespace CloudPlatform.Core.Extensions
{
    public static class CoreExtensions
    {
        public static string GetExceptionChain(this Exception ex)
        {
            var message = new StringBuilder(ex.Message);

            if (ex.InnerException is not null)
            {
                message.AppendLine();
                message.AppendLine(GetExceptionChain(ex.InnerException));
            }

            return message.ToString();
        }
    }
}