namespace InsurancePolicy.Exceptions.PlanExceptions
{
    public class PlanNotFoundException:Exception
    {
        public PlanNotFoundException(string message) : base(message) { }
    }
}
