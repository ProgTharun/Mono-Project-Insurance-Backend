namespace InsurancePolicy.Exceptions.PolicyExceptions
{
    public class PolicyNotFoundException:Exception
    {
        public PolicyNotFoundException(string message) : base(message) { }
    }
}
