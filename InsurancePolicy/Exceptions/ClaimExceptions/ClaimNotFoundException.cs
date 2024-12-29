namespace InsurancePolicy.Exceptions.ClaimExceptions
{
    public class ClaimNotFoundException:Exception
    {
        public ClaimNotFoundException(string message):base(message) { }
    }
}
