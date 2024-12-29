namespace InsurancePolicy.Exceptions.UserExceptions
{
    public class UsersDoesNotExistException:Exception
    {
        public UsersDoesNotExistException(string message) : base(message) { }
    }
}
