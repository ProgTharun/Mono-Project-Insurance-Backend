namespace InsurancePolicy.Exceptions.SchemeExceptions
{
    public class SchemesDoesNotExistException:Exception
    {
        public SchemesDoesNotExistException(string message) : base(message) { }
    }
}
