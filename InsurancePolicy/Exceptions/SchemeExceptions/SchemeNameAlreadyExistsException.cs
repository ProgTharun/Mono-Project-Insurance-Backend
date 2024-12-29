namespace InsurancePolicy.Exceptions.SchemeExceptions
{
      public class SchemeNameAlreadyExistsException : Exception
    {
        public SchemeNameAlreadyExistsException(string message) : base(message) { }
    }
}
