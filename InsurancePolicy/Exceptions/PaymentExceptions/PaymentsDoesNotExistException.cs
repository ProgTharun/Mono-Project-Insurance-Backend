namespace InsurancePolicy.Exceptions.PaymentExceptions
{
    public class PaymentsDoesNotExistException:Exception
    {
        public PaymentsDoesNotExistException(string message) : base(message) { }
    }
}
