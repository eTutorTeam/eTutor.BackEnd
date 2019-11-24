using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using eTutor.Core.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace eTutor.ServerApi.Helpers
{
    public class HttpExceptionHandler
    {
        private const string ContentType = "application/json";

        private readonly RequestDelegate _next;

        /// <summary>
        /// Creates an instance of a <see cref="HttpStatusCodeExceptionMiddleware"/>
        /// </summary>
        /// <param name="next">Request delegate</param>
        /// <param name="loggerFactory">Logger factory</param>
        public HttpExceptionHandler(RequestDelegate next)
        {
            _next = next ?? throw new ArgumentNullException(nameof(next));
        }

        /// <summary>
        /// Executes the request passing it through pipe lines
        /// </summary>
        /// <param name="context">A <see cref="HttpContext"/></param>
        /// <returns> A <see cref="Task"/></returns>
        public async Task Invoke(HttpContext context)
        {
            try
            {
                await TryUnauthorizedResponse(context);
            }
            catch (Exception ex)
            {
                if (context.Response.HasStarted)
                {
                    throw;
                }

                await BuildInternalServerErrorResponse(context, ex);
            }
        }

        private async Task BuildInternalServerErrorResponse(HttpContext context, Exception ex)
        {
            context.Response.StatusCode = (int) HttpStatusCode.InternalServerError;
            context.Response.ContentType = ContentType;

            using (var writer = new StreamWriter(context.Response.Body))
            {
                writer.Write(JsonConvert.SerializeObject(BuildInternalServerError(ex)));
                await writer.FlushAsync().ConfigureAwait(false);
            }
        }

        private async Task TryUnauthorizedResponse(HttpContext context)
        {
            await _next(context);
            int responseStatusCode = context.Response.StatusCode;

            if (context.Response.StatusCode == (int) HttpStatusCode.Unauthorized)
            {
                context.Response.StatusCode = responseStatusCode;
                context.Response.ContentType = ContentType;

                using (var writer = new StreamWriter(context.Response.Body))
                {
                    writer.Write(JsonConvert.SerializeObject(BuildUnathorizedBody()));
                    await writer.FlushAsync().ConfigureAwait(false);
                }
            }
        }

        private static  Error BuildInternalServerError(Exception ex)
            => new Error
            {
                Code = 500,
                Message = ex.Message,
                ReasonPhrase = "InternalServerError"
            };

        private static Error BuildUnathorizedBody()
            => new Error
            {
                Code = 401,
                Message = "Unauthorized",
                ReasonPhrase = "Unauthorized"
            };
    }
}