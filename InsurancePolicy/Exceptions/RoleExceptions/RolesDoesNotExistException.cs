namespace InsurancePolicy.Exceptions.RoleException
{
    public class RolesDoesNotExistException:Exception
    {
        public RolesDoesNotExistException(string message) : base(message) { }
    }
}
