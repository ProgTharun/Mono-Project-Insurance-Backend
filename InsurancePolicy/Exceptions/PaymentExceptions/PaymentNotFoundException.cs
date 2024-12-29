namespace InsurancePolicy.Exceptions.PaymentExceptions
{
    public class PaymentNotFoundException:Exception
    {
        public PaymentNotFoundException(String  message) : base(message) { }    
    }
}
