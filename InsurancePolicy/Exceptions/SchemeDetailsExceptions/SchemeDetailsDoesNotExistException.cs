namespace InsurancePolicy.Exceptions.SchemeDetailsExceptions
{
    public class SchemeDetailsDoesNotExistException:Exception
    {
        public SchemeDetailsDoesNotExistException(string message) : base(message) { }
    }
}
