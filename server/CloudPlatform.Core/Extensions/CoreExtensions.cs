using System;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;

namespace CloudPlatform.Core.Extensions
{
    public static class CoreExtensions
    {
        private static readonly string urlPattern = "[^a-zA-Z0-9-.]";

        public static string UrlEncode(this string url)
        {
            var friendlyUrl = Regex.Replace(url, @"\s", "-").ToLower();
            friendlyUrl = Regex.Replace(friendlyUrl, urlPattern, string.Empty);
            return friendlyUrl;
        }
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

        public static void HandleError(this IApplicationBuilder app)
        {
            app.Run(async context =>
            {
                var error = context.Features.Get<IExceptionHandlerFeature>();

                if (error is not null)
                {
                    var ex = error.Error;
                    await context.SendErrorResponse(ex);
                }
            });
        }

        static async Task SendErrorResponse(this HttpContext context, Exception ex)
        {
            context.Response.StatusCode = 500;
            context.Response.ContentType = "application/json";
            await context.Response.WriteAsync(ex.GetExceptionChain(), Encoding.UTF8);
        }
    }
}