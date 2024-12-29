namespace InsurancePolicy.Exceptions.AgentExceptions
{
    public class AgentsDoesNotExistException:Exception
    {
        public AgentsDoesNotExistException(string message) : base(message) { }
    }
}
