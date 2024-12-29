namespace InsurancePolicy.Exceptions.DocumentExceptions
{
    public class DocumentsDoesNotExistException:Exception
    {
        public DocumentsDoesNotExistException(string message) : base(message) { }
    }
}
