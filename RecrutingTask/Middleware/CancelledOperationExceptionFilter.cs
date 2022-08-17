using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace RecrutingTask.Api.Middleware
{
    public class CancelledOperationExceptionFilter : ExceptionFilterAttribute
    {
        private readonly ILogger _logger;

        public CancelledOperationExceptionFilter(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<CancelledOperationExceptionFilter>();
        }

        public override void OnException(ExceptionContext context)
        {
            if (context.Exception is OperationCanceledException)
            {
                _logger.LogInformation("Request was cancelled");
                context.ExceptionHandled = true;
                context.Result = new StatusCodeResult(StatusCodes.Status400BadRequest);
            }
        }
    }
}
