namespace InsurancePolicy.Exceptions.CustomerExceptions
{
    public class CustomersDoesNotExistException:Exception
    {
        public CustomersDoesNotExistException(string message) : base(message) { }
    }
}
