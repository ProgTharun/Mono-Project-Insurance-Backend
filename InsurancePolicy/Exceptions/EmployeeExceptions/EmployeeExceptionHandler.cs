using InsurancePolicy.Exceptions.AgentExceptions;
using InsurancePolicy.Models;
using Microsoft.AspNetCore.Diagnostics;

namespace InsurancePolicy.Exceptions.EmployeeExceptions
{
    public class EmployeeExceptionHandler:IExceptionHandler
    {
        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext,
           Exception exception, CancellationToken cancellationToken)
        {
            var response = new ErrorResponse();
            if (exception is EmployeeNotFoundException)
            {
                response.StatusCode = StatusCodes.Status404NotFound;
                response.ErrorMessage = exception.Message;
                response.Title = "Wrong Input";
            }
            else if (exception is EmployeesDoesNotExistException)
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
