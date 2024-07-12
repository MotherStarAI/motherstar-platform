using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Hosting;
using RCommon.ApplicationServices.Validation;
using MotherStar.Platform.Domain.SEO.Lighthouse.Exceptions;

namespace MotherStar.Platform.HttpApi.Filters
{
    public class HttpGlobalExceptionFilter : IExceptionFilter
    {
        private readonly IWebHostEnvironment env;
        private readonly ILogger<HttpGlobalExceptionFilter> logger;

        public HttpGlobalExceptionFilter(IWebHostEnvironment env, ILogger<HttpGlobalExceptionFilter> logger)
        {
            this.env = env;
            this.logger = logger;
        }

        public void OnException(ExceptionContext context)
        {
            logger.LogError(new EventId(context.Exception.HResult),
                context.Exception,
                context.Exception.Message);

            if (context.Exception.GetType() == typeof(ValidationException) || context.Exception.GetType() == typeof(LighthouseDomainException))
            {
                var problemDetails = new ValidationProblemDetails()
                {
                    Instance = context.HttpContext.Request.Path,
                    Status = StatusCodes.Status400BadRequest,
                    Detail = "Please refer to the errors property for additional details."
                };

                problemDetails.Errors.Add("Validation Errors", new string[] { context.Exception.Message.ToString() });

                context.Result = new BadRequestObjectResult(problemDetails);
                context.HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            }
            else
            {

                var json = new JsonErrorResponse();

                if (env.IsDevelopment() || env.IsStaging())
                {
                    json.Messages = new[] { "An error occured while processing this request.", context.Exception.Message };
                    json.DeveloperMessage ??= context.Exception.StackTrace;
                }
                else
                {
                    json.Messages = new[] { "An error occured while processing this request." };
                    json.DeveloperMessage = "N/A";
                }

                context.Result = new ErrorActionResult(new ErrorResult(context.Exception, json));
                context.HttpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            }
            context.ExceptionHandled = true;
        }
    }
}
