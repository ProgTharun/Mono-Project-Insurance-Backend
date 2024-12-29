using InsurancePolicy.Exceptions.AdminExceptions;
using InsurancePolicy.Models;
using Microsoft.AspNetCore.Diagnostics;

namespace InsurancePolicy.Exceptions.AgentExceptions
{
    public class AgentExceptionHandler:IExceptionHandler
    {
        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext,
           Exception exception, CancellationToken cancellationToken)
        {
            var response = new ErrorResponse();
            if (exception is AgentNotFoundException)
            {
                response.StatusCode = StatusCodes.Status404NotFound;
                response.ErrorMessage = exception.Message;
                response.Title = "Wrong Input";
            }
            else if (exception is AgentsDoesNotExistException)
            {
                response.StatusCode = StatusCodes.Status204NoContent;
                response.ErrorMessage = exception.Message;
                response.Title = "empty []";
            }
            else
            {
                response.StatusCode = StatusCodes.Status500InternalServerError;
                response.ErrorMessage = exception.Message;
                response.Title = "Something went wrong!";
            }
            await httpContext.Response.WriteAsJsonAsync(response, cancellationToken);
            return true;
        }
    }
}
