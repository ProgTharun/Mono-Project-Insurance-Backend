namespace InsurancePolicy.Exceptions.PlanExceptions
{
    public class PlansDoesNotExistException:Exception
    {
        public PlansDoesNotExistException(string message) : base(message) { }
    }
}
