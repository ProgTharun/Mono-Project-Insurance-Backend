namespace InsurancePolicy.Exceptions.EmployeeExceptions
{
    public class EmployeesDoesNotExistException:Exception
    {
        public EmployeesDoesNotExistException(string message) : base(message) { }
    }
}
