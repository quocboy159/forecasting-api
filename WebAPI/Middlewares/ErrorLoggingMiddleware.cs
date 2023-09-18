using ForecastingSystem.Domain.Common;
using Microsoft.AspNetCore.Http;
using Polly;
using Serilog;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace ForecastingSystem.BackendAPI.Middlewares
{
    public class ErrorLoggingMiddleware
    {
        const string MessageTemplate = "HTTP {RequestMethod} {RequestPath} responded {RequestCode}";
        static readonly ILogger Log = Serilog.Log.ForContext<ErrorLoggingMiddleware>();
        private readonly RequestDelegate _next;

        public ErrorLoggingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception exception)
            {
                var request = httpContext.Request;

                var log = Log
                       .ForContext("RequestHeaders", request.Headers.ToDictionary(h => h.Key, h => h.Value.ToString()), destructureObjects: true)
                       .ForContext("RequestHost", request.Host)
                       .ForContext("RequestProtocol", request.Protocol);

                if (request.HasFormContentType)
                    log = log.ForContext("RequestForm", request.Form.ToDictionary(v => v.Key, v => v.Value.ToString()));
                log.Error(exception, MessageTemplate, httpContext.Request.Method, httpContext.Request.Path, 500);

                switch (exception)
                {
                    case BussinessException:
                        {
                            httpContext.Response.ContentType = "application/json";
                            httpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                            await httpContext.Response.WriteAsync(exception.Message);
                            break;
                        }

                    default:
                        {
                            throw;
                        }
                }
            }
        }
    }
}
