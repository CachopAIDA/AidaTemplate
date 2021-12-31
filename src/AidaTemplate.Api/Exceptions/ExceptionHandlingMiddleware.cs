using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace AidaTemplate.Api.Exceptions {
    public class ExceptionHandlingMiddleware {
        private readonly RequestDelegate next;
        private readonly ILogger<ExceptionHandlingMiddleware> logger;

        public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger) {
            this.next = next;
            this.logger = logger;
        }

        public async Task Invoke(HttpContext context/* other dependencies */) {
            try {
                await next(context);
            }
            catch (Exception ex) {
                logger.LogError(ex, ex.Message);
                await HandleExceptionAsync(context, ex);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception) {
            var problemDetails = GetProblemDetails(exception);
            var result = JsonConvert.SerializeObject(problemDetails);
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = GetResponseStatus(problemDetails.Status);
            return context.Response.WriteAsync(result);
        }

        private static int GetResponseStatus(int? problemDetailsStatus) {
            if (problemDetailsStatus.HasValue) return problemDetailsStatus.Value;
            return (int) HttpStatusCode.InternalServerError;
        }

        private static ProblemDetails GetProblemDetails(Exception exception) =>
            exception switch {
                _ => new ProblemDetails {
                    Title = "Unknown error",
                    Status = (int)HttpStatusCode.InternalServerError,
                    Type = exception.GetType().FullName
                }
            };
    }
}