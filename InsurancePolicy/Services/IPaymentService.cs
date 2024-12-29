using InsurancePolicy.DTOs;
using InsurancePolicy.Helpers;
using InsurancePolicy.Models;

namespace InsurancePolicy.Services
{
    public interface IPaymentService
    {
        public List<PaymentResponseDto> GetAllPayments();
        PageList<PaymentResponseDto> GetAllPaginated(PageParameters pageParameters);

        public List<PaymentResponseDto> GetPaymentsByPolicy(Guid id);
        public Guid AddPayment(PaymentRequestDto payment);
        public int GetCount();
    }
}
