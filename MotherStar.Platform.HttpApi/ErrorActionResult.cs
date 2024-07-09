using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace MotherStar.Platform.HttpApi
{
    public class ErrorActionResult : IActionResult
    {
        private readonly ErrorResult _errorResult;

        public ErrorActionResult()
        {

        }

        public ErrorActionResult(ErrorResult errorResult)
        {
            _errorResult = errorResult;
        }

        public async Task ExecuteResultAsync(ActionContext context)
        {
            var objectResult = new ObjectResult(_errorResult.Exception)
            {
                Value = _errorResult.Response,
                StatusCode = _errorResult.Exception != null
                ? StatusCodes.Status500InternalServerError
                : StatusCodes.Status200OK
            };

            await objectResult.ExecuteResultAsync(context);
        }
    }

    public class ErrorResult
    {
        public ErrorResult(Exception exception, JsonErrorResponse response)
        {
            Exception = exception;
            Response = response;
        }

        public Exception Exception { get; }
        public JsonErrorResponse Response { get; }
    }
}
