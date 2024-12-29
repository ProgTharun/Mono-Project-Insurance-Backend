namespace InsurancePolicy.Exceptions.PolicyExceptions
{
    public class PoliciesDoesNotExistException:Exception
    {
        public PoliciesDoesNotExistException(string  message) : base(message) { }   
    }
}
