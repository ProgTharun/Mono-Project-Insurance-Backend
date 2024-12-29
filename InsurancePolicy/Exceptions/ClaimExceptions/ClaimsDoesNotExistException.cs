namespace InsurancePolicy.Exceptions.ClaimExceptions
{
    public class ClaimsDoesNotExistException:Exception
    {
        public ClaimsDoesNotExistException(string message):base(message) { }
    }
}
