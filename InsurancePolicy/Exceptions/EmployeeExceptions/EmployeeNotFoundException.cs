namespace InsurancePolicy.Exceptions.EmployeeExceptions
{
    public class EmployeeNotFoundException:Exception
    {
        public EmployeeNotFoundException(string message) : base(message) { }
    }
}
