namespace InsurancePolicy.Exceptions.DocumentExceptions
{
    public class DocumentNotFoundException:Exception
    {
        public DocumentNotFoundException(string message) : base(message) { }
    }
}
