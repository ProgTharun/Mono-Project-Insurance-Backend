using InsurancePolicy.Exceptions.AdminExceptions;
using InsurancePolicy.Exceptions.AgentExceptions;
using InsurancePolicy.Exceptions.ClaimExceptions;
using InsurancePolicy.Exceptions.CustomerExceptions;
using InsurancePolicy.Exceptions.DocumentExceptions;
using InsurancePolicy.Exceptions.EmployeeExceptions;
using InsurancePolicy.Exceptions.PaymentExceptions;
using InsurancePolicy.Exceptions.PlanExceptions;
using InsurancePolicy.Exceptions.PolicyExceptions;
using InsurancePolicy.Exceptions.RoleException;
using InsurancePolicy.Exceptions.SchemeDetailsExceptions;
using InsurancePolicy.Exceptions.SchemeExceptions;
using InsurancePolicy.Models;
using Microsoft.AspNetCore.Diagnostics;
using Serilog;
using System.ComponentModel.DataAnnotations;

namespace InsurancePolicy.Exceptions
{
    public class AppExceptionHandler:IExceptionHandler
    {
        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext,
           Exception exception, CancellationToken cancellationToken)
        {

            var response = new ErrorResponse();
            if (exception is ValidationException)
            {
                response.StatusCode = StatusCodes.Status400BadRequest;
                response.Title = "Invalid Input";
                response.ErrorMessage = exception.Message;
            }

            else if (exception is AdminNotFoundException || exception is AgentNotFoundException ||exception is ClaimNotFoundException|| exception is CustomerNotFoundException ||
    exception is DocumentNotFoundException || exception is EmployeeNotFoundException || exception is SchemeExceptions.SchemeNotFoundException ||
    exception is RoleNotFoundException || exception is PlanExceptions.PlanNotFoundException||exception is PolicyNotFoundException||exception is SchemeDetailsNotFoundException
    ||exception is PaymentNotFoundException)
            {
                response.StatusCode = StatusCodes.Status404NotFound;
                response.ErrorMessage = exception.Message;
                response.Title = "Wrong Input";
            }
            else if (exception is AdminsDoesNotExistException || exception is AgentsDoesNotExistException || exception is ClaimsDoesNotExistException||exception is CustomersDoesNotExistException ||
         exception is DocumentsDoesNotExistException || exception is EmployeesDoesNotExistException ||
         exception is RolesDoesNotExistException || exception is PlansDoesNotExistException || exception is SchemesDoesNotExistException
         ||exception is PoliciesDoesNotExistException || exception is SchemeDetailsDoesNotExistException||exception is PaymentsDoesNotExistException)
            {
                response.StatusCode = StatusCodes.Status204NoContent;
                response.ErrorMessage = exception.Message;
                response.Title = "empty []";
            }
            else if (exception is SchemeNameAlreadyExistsException)
            {
                response.StatusCode = StatusCodes.Status409Conflict; // HTTP 409 Conflict
                response.ErrorMessage = exception.Message;
                response.Title = "Duplicate Scheme Name";
            }
            else
            {
                response.StatusCode = StatusCodes.Status500InternalServerError;
                response.ErrorMessage = exception.StackTrace;
                response.Title = "Something went wrong!";
            }
            await httpContext.Response.WriteAsJsonAsync(response, cancellationToken);
            Log.Error("Error Occured:"+exception.Message);
            return true;
        }
    }
}
