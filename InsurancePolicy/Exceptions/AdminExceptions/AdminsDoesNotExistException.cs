namespace InsurancePolicy.Exceptions.AdminExceptions
{
    public class AdminsDoesNotExistException:Exception
    {
        public AdminsDoesNotExistException(string message) : base(message) { }
    }
}
