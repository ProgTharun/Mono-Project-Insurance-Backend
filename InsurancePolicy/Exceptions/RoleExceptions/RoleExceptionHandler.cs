using InsurancePolicy.Exceptions.RoleException;
using InsurancePolicy.Exceptions.UserExceptions;
using InsurancePolicy.Models;
using Microsoft.AspNetCore.Diagnostics;

namespace InsurancePolicy.Exceptions.RoleExceptions
{
    public class RoleExceptionHandler : IExceptionHandler
    {
        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext,
           Exception exception, CancellationToken cancellationToken)
        {
            var response = new ErrorResponse();
            if (exception is RoleNotFoundException)
            {
                response.StatusCode = StatusCodes.Status404NotFound;
                response.ErrorMessage = exception.Message;
                response.Title = "Wrong Input";
            }
            else if (exception is RolesDoesNotExistException)
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
