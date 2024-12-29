using AutoMapper;
using InsurancePolicy.DTOs;
using InsurancePolicy.Helpers;
using InsurancePolicy.Models;
using InsurancePolicy.Repositories;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace InsurancePolicy.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly IRepository<Payment> _paymentRepository;
        private readonly IRepository<Policy> _policyRepository;
        private readonly IMapper _mapper;

        public PaymentService(
            IRepository<Payment> paymentRepository,
            IRepository<Policy> policyRepository,
            IMapper mapper)
        {
            _paymentRepository = paymentRepository;
            _policyRepository = policyRepository;
            _mapper = mapper;
        }
        public int GetCount()
        {
           return  _paymentRepository.GetAll().Count();
        }

        public Guid AddPayment(PaymentRequestDto paymentDto)
        {
            // Validate associated policy
            var policy = _policyRepository.GetById(paymentDto.PolicyId);
            if (policy == null)
                throw new ArgumentException("Invalid Policy ID.");

            // Map DTO to Entity
            var payment = _mapper.Map<Payment>(paymentDto);

            // Calculate total payment
            payment.TotalPayment = payment.AmountPaid + payment.Tax;

            // Save payment
            _paymentRepository.Add(payment);
            Log.Information("New record added:" + payment.PaymentId);
            return payment.PaymentId;
        }

        public List<PaymentResponseDto> GetPaymentsByPolicy(Guid policyId)
        {
            var payments = _paymentRepository.GetAll()
                .Where(p => p.PolicyId == policyId)
                .ToList();
            Log.Information("Records Retrived. Count{count}", payments.Count);
            return _mapper.Map<List<PaymentResponseDto>>(payments);
        }
        public PageList<PaymentResponseDto> GetAllPaginated(PageParameters pageParameters)
        {
            var paymentsQuery = _paymentRepository.GetAll()
                .Include(p => p.Policy)
                .AsQueryable();

            var paginatedPayments = PageList<PaymentResponseDto>.ToPagedList(
                _mapper.Map<List<PaymentResponseDto>>(paymentsQuery.ToList()),
                pageParameters.PageNumber,
                pageParameters.PageSize
            );
            Log.Information("Records Retrived. Count{count}", paginatedPayments.Count);
            return paginatedPayments;
        }

        public List<PaymentResponseDto> GetAllPayments()
        {
            var payments = _paymentRepository.GetAll().Include(p=> p.Policy).ToList();
            Log.Information("Records Retrieved. Count{Count}", payments.Count);
            return _mapper.Map<List<PaymentResponseDto>>(payments);
        }
    }
}
