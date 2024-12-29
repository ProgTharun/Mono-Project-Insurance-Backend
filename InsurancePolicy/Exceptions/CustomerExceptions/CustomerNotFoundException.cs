namespace InsurancePolicy.Exceptions.CustomerExceptions
{
    public class CustomerNotFoundException:Exception
    {
        public CustomerNotFoundException(string message) : base(message) { }    
    }
}
