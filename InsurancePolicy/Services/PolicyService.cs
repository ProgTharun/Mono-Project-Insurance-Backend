using AutoMapper;
using InsurancePolicy.Data;
using InsurancePolicy.DTOs;
using InsurancePolicy.enums;
using InsurancePolicy.Exceptions.PolicyExceptions;
using InsurancePolicy.Helpers;
using InsurancePolicy.Models;
using InsurancePolicy.Repositories;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace InsurancePolicy.Services
{
    public class PolicyService : IPolicyService
    {
        private readonly IRepository<Policy> _policyRepository;
        private readonly IRepository<Installment> _installmentRepository;
        private readonly IRepository<Commission> _commissionRepository;
        private readonly IRepository<Nominee> _nomineeRepository;
        private readonly IRepository<InsuranceScheme> _insuranceSchemeRepository;
        private readonly IRepository<TaxSettings> _taxSettingsRepository;
       // private readonly IRepository<InsuranceSettings> _insuranceSettingsRepository;
        private readonly IMapper _mapper;
        private readonly AppDbContext _context;

        public PolicyService(
            IRepository<Policy> policyRepository,
            IRepository<Installment> installmentRepository,
            IRepository<Commission> commissionRepository,
            IRepository<Nominee> nomineeRepository,
            IRepository<InsuranceScheme> insuranceSchemeRepository,
            IRepository<TaxSettings> taxSettingsRepository,
            //IRepository<InsuranceSettings> insuranceSettingsRepository,
            IMapper mapper, AppDbContext context)
        {
            _policyRepository = policyRepository;
            _installmentRepository = installmentRepository;
            _commissionRepository = commissionRepository;
            _nomineeRepository = nomineeRepository;
            _insuranceSchemeRepository = insuranceSchemeRepository;
            _taxSettingsRepository = taxSettingsRepository;
            //_insuranceSettingsRepository = insuranceSettingsRepository;
            _mapper = mapper;
            _context = context;
        }

        public int GetPoliciesCountByCustomerId(Guid customerId)
        {
            var count=_policyRepository.GetAll().Where(p=>p.CustomerId == customerId).Count();
            return count;
        }
        public PageList<PolicyResponseDto> GetAll(PageParameters pageParameters)
        {
            var policiesQuery = _policyRepository.GetAll()
                .Include(p => p.InsuranceScheme).ThenInclude(p => p.InsurancePlan)
                .Include(p => p.Nominees)
                .Include(p => p.Installments)
                .Include(p => p.Payments)
                .Include(p => p.Claim)
                .Include(d=>d.Documents)
                .AsQueryable();

            var paginatedPolicies = PageList<PolicyResponseDto>.ToPagedList(
                _mapper.Map<List<PolicyResponseDto>>(policiesQuery.ToList()),
                pageParameters.PageNumber,
                pageParameters.PageSize
            );
            Log.Information("Records Retrieved.",paginatedPolicies.Count);
            return paginatedPolicies;
        }

        public PolicyResponseDto GetById(Guid id)
        {
            // Retrieve the policy along with related data
            var policy = _policyRepository.GetAll()
                .Include(p => p.InsuranceScheme)
                .Include(p => p.Nominees)
                .Include(p => p.Installments)
                .Include(p => p.Payments)
                .Include(p => p.Claim).Include(d=>d.Documents)
                .FirstOrDefault(p => p.PolicyId == id);

            if (policy == null)
                throw new PolicyNotFoundException("Policy not found.");

            // Retrieve documents associated with the customer
            var selectedDocuments = _context.Documents
                .Where(d => d.CustomerId == policy.CustomerId) // Filter by CustomerId
                .ToList();

            // Map the policy to the response DTO
            var response = _mapper.Map<PolicyResponseDto>(policy);

            // Add selected documents to the response DTO
            response.Documents = selectedDocuments.Select(d => new DocumentResponseDto
            {
                DocumentId = d.DocumentId,
                DocumentName = d.DocumentName.ToString(), // Convert enum to string
                DocumentPath = d.DocumentPath,
                Status=d.Status
            }).ToList();

            return response;
        }


        public Guid Add(PolicyRequestDto requestDto)
        {
            // Map PolicyRequestDto to Policy entity
            var policy = _mapper.Map<Policy>(requestDto);

            // Set default values and validate relationships
            policy.Status = PolicyStatus.PENDING;
            policy.IssueDate = DateTime.Now;

            // Validate and set Insurance Scheme
            var insuranceScheme = _insuranceSchemeRepository.GetById(requestDto.InsuranceSchemeId);
            if (insuranceScheme == null)
                throw new InvalidOperationException("Invalid Insurance Scheme ID.");
            policy.InsuranceScheme = insuranceScheme;

            // Validate and set Tax Settings
            if (requestDto.TaxId.HasValue)
            {
                var taxSettings = _taxSettingsRepository.GetById(requestDto.TaxId.Value);
                if (taxSettings == null)
                    throw new InvalidOperationException("Invalid TaxSettings ID.");
                policy.TaxSettings = taxSettings;
            }

            // Validate and set Insurance Settings
            //if (requestDto.InsuranceSettingId.HasValue)
            //{
            //    var insuranceSettings = _insuranceSettingsRepository.GetById(requestDto.InsuranceSettingId.Value);
            //    if (insuranceSettings == null)
            //        throw new InvalidOperationException("Invalid InsuranceSettings ID.");
            //    policy.InsuranceSettings = insuranceSettings;
            //}

            // Add Nominees
            if (requestDto.Nominees != null)
            {
                policy.Nominees = requestDto.Nominees.Select(n => _mapper.Map<Nominee>(n)).ToList();
            }

            policy.InstallmentAmount = Math.Round(policy.PremiumAmount / policy.PolicyTerm, 2);

            // Generate Installments
            policy.Installments = GenerateInstallments(policy);

            var totalAmount = policy.Installments.Sum(i => i.AmountDue);
            var totalWithProfit = totalAmount * (policy.InsuranceScheme.ProfitRatio / 100);
            policy.SumAssured = Math.Round((double)(totalAmount + totalWithProfit), 2);
            policy.MaturityDate = policy.IssueDate.AddMonths((int)policy.PolicyTerm);

            // Add Policy to the repository
            _policyRepository.Add(policy);

            // Save changes to ensure PolicyId is generated
            _context.SaveChanges();

            // Handle Selected Documents
            if (requestDto.SelectedDocumentIds != null && requestDto.SelectedDocumentIds.Any())
            {
                // Retrieve and validate selected documents
                var selectedDocuments = _context.Documents
                    .Where(d => requestDto.SelectedDocumentIds.Contains(d.DocumentId) && d.CustomerId == requestDto.CustomerId)
                    .ToList();

                if (selectedDocuments.Count != requestDto.SelectedDocumentIds.Count)
                {
                    throw new InvalidOperationException("One or more provided document IDs are invalid or do not belong to the specified customer.");
                }

                // Log or process the selected documents
                foreach (var document in selectedDocuments)
                {
                    Console.WriteLine($"Document Linked: {document.DocumentName}, Path: {document.DocumentPath}");
                }
            }

            // Update Commissions if Agent exists
            if (policy.AgentId.HasValue)
            {
                AddCommissionForAgent(policy);
            }

            Log.Information("New record added:" + policy.PolicyId);
            return policy.PolicyId;
        }

        private List<Installment> GenerateInstallments(Policy policy)

        {

            var installments = new List<Installment>();

            if (policy.PremiumType == PremiumType.SINGLE)

            {

                installments.Add(new Installment

                {

                    PolicyId = policy.PolicyId,

                    DueDate = policy.IssueDate,

                    AmountDue = policy.SumAssured,

                    Status = InstallmentStatus.PENDING,

                    PaymentReference = "-"

                });

            }

            else

            {

                int intervalInMonths = policy.PremiumType switch

                {

                    PremiumType.MONTHLY => 1,

                    PremiumType.QUARTERLY => 3,

                    PremiumType.HALF_YEARLY => 6,

                    PremiumType.YEARLY => 12,

                    _ => throw new ArgumentException("Invalid PremiumType.")

                };

                //double installmentAmount = policy.SumAssured / (policy.PolicyTerm / intervalInMonths);

                int totalInstallments = (int)policy.PolicyTerm * (12 / intervalInMonths);

                var taxAmountPerInstallment = policy.InstallmentAmount * (policy.TaxSettings.TaxPercentage / 100);

                var installmentAmountWithTax = policy.InstallmentAmount + taxAmountPerInstallment;

                installmentAmountWithTax *= intervalInMonths;

                for (int i = 0; i < policy.PolicyTerm / intervalInMonths; i++)

                {

                    installments.Add(new Installment

                    {

                        PolicyId = policy.PolicyId,

                        DueDate = policy.IssueDate.AddMonths(i * intervalInMonths),

                        AmountDue = Math.Round((double)installmentAmountWithTax, 2),

                        Status = InstallmentStatus.PENDING,

                        PaymentReference = "-"

                    });

                }

            }

            return installments;

        }


        public bool Update(PolicyRequestDto requestDto)
        {
            var policy = _policyRepository.GetById(requestDto.PolicyId.Value);
            if (policy == null)
                throw new PolicyNotFoundException("Policy not found.");

            _mapper.Map(requestDto, policy);
            _policyRepository.Update(policy);
            Log.Information("Record upadted", policy.PolicyId);// Log the count of records retrieved

            return true;
        }

        public bool Delete(Guid id)
        {
            var policy = _policyRepository.GetById(id);
            if (policy == null)
                throw new PolicyNotFoundException("Policy not found.");

            policy.Status = PolicyStatus.DROPPED;
            policy.CancellationDate = DateTime.Now;

            _policyRepository.Update(policy);

            CancelInstallments(id);
            CancelCommissions(id);
            Log.Information("Record upadted", id);// Log the count of records retrieved

            return true;
        }

        // --- Helper Methods ---
        private void CalculateCommissions(Policy policy)
        {
            if (policy.AgentId.HasValue && policy.InsuranceScheme != null)
            {
                var commissionAmount = policy.PremiumAmount * policy.InsuranceScheme.RegistrationCommRatio / 100;

                _commissionRepository.Add(new Commission
                {
                    AgentId = policy.AgentId.Value,
                    PolicyNo = policy.PolicyId,
                    Amount = commissionAmount,
                    CommissionType = CommissionType.REGISTRATION
                });
            }
        }

        private void CancelInstallments(Guid policyId)
        {
            var installments = _installmentRepository.GetAll().Where(i => i.PolicyId == policyId).ToList();
            foreach (var installment in installments)
            {
                installment.Status = InstallmentStatus.CANCELLED;
                _installmentRepository.Update(installment);
            }
        }

        private void CancelCommissions(Guid policyId)
        {
            var commissions = _commissionRepository.GetAll().Where(c => c.PolicyNo == policyId).ToList();
            foreach (var commission in commissions)
            {
                commission.Amount = 0;
                _commissionRepository.Update(commission);
            }
        }
        private void AddCommissionForAgent(Policy policy)
        {
            // Ensure agent exists
            if (!policy.AgentId.HasValue || policy.InsuranceScheme == null)
                return;

            // Calculate registration commission
            var commissionAmount = policy.PremiumAmount * policy.InsuranceScheme.RegistrationCommRatio / 100;

            // Create the commission
            var commission = new Commission
            {
                AgentId = policy.AgentId.Value,
                PolicyNo = policy.PolicyId, // Link commission to the policy
                Amount = commissionAmount,
                CommissionType = CommissionType.REGISTRATION,
                IssueDate = DateTime.Now
            };

            // Add the commission to the repository
            _commissionRepository.Add(commission);

            // Save changes
            Log.Information("New Record Added.",commission.CommissionId);
            _context.SaveChanges();
        }
        public PageList<PolicyResponseDto> GetPoliciesByAgentId(Guid agentId, PageParameters pageParameters)
        {
            var policiesQuery = _policyRepository.GetAll().Include(c=>c.Customer).Include(s=>s.InsuranceScheme)
                .Where(p => p.AgentId == agentId)
                .AsQueryable();

            var paginatedPolicies = PageList<PolicyResponseDto>.ToPagedList(
                _mapper.Map<List<PolicyResponseDto>>(policiesQuery.ToList()),
                pageParameters.PageNumber,
                pageParameters.PageSize
            );
            Log.Information("Records Retrieved. Count{count}", paginatedPolicies.Count);


            return paginatedPolicies;
        }

        public PageList<PolicyResponseDto> GetPoliciesByCustomerId(Guid customerId, PageParameters pageParameters)
        {
            var policiesQuery = _policyRepository.GetAll()
                .Where(p => p.CustomerId == customerId).Include(p => p.InsuranceScheme)
                .Include(p => p.Nominees)
                .Include(p => p.Installments)
                .Include(p => p.Payments)
                .Include(p => p.Claim)
                .Include(p=>p.Documents)
                .ToList();

            var paginatedPolicies = PageList<PolicyResponseDto>.ToPagedList(
                _mapper.Map<List<PolicyResponseDto>>(policiesQuery.ToList()),
                pageParameters.PageNumber,
                pageParameters.PageSize
            );
            Log.Information("Records Retrieved. Count{count}", paginatedPolicies.Count);
            return paginatedPolicies;
        }
        public List<PolicyResponseDto> GetPoliciesByCustomer(Guid customerId)
        {
            var policiesQuery = _policyRepository.GetAll()
                .Where(p => p.CustomerId == customerId).Include(p => p.InsuranceScheme)
                .Include(p => p.Nominees)
                .Include(p => p.Installments)
                .Include(p => p.Payments)
                .Include(p => p.Claim)
                .Include(p => p.Documents)
                .ToList();

            var paginatedPolicies = _mapper.Map<List<PolicyResponseDto>>(policiesQuery.ToList());

            return paginatedPolicies;
        }
        public PageList<PolicyResponseDto> GetPoliciesByCustomerIdActive(Guid customerId, PageParameters pageParameters)
        {
            var policiesQuery = _policyRepository.GetAll()
                .Where(p => p.CustomerId == customerId && (p.Status!=PolicyStatus.PENDING && p.Status!=PolicyStatus.REJECTED)).Include(p => p.InsuranceScheme)
                .Include(p => p.Nominees)
                .Include(p => p.Installments)
                .Include(p => p.Payments)
                .Include(p => p.Claim)
                .Include(p => p.Documents)
                .ToList();

            var paginatedPolicies = PageList<PolicyResponseDto>.ToPagedList(
                _mapper.Map<List<PolicyResponseDto>>(policiesQuery.ToList()),
                pageParameters.PageNumber,
                pageParameters.PageSize
            );
            Log.Information("Records Retrieved. Count{count}", paginatedPolicies.Count);

            return paginatedPolicies;
        }
        public PageList<PolicyResponseDto> GetPoliciesByCustomerIdPending(Guid customerId, PageParameters pageParameters)
        {
            var policiesQuery = _policyRepository.GetAll()
                .Where(p => p.CustomerId == customerId && (p.Status == PolicyStatus.PENDING || p.Status==PolicyStatus.REJECTED)).Include(p => p.InsuranceScheme)
                .Include(p => p.Nominees)
                .Include(p => p.Installments)
                .Include(p => p.Payments)
                .Include(p => p.Claim)
                .Include(p => p.Documents)
                .ToList();

            var paginatedPolicies = PageList<PolicyResponseDto>.ToPagedList(
                _mapper.Map<List<PolicyResponseDto>>(policiesQuery.ToList()),
                pageParameters.PageNumber,
                pageParameters.PageSize
            );
            Log.Information("Records Retrieved. Count{count}", paginatedPolicies.Count);

            return paginatedPolicies;
        }

        public PageList<PolicyResponseDto> GetPoliciesPending(PageParameters pageParameters)
        {
            var policiesQuery = _policyRepository.GetAll()
                .Where(p=>p.Status == PolicyStatus.PENDING).Include(p => p.InsuranceScheme)
                .Include(p => p.Nominees)
                .Include(p => p.Installments)
                .Include(p => p.Payments)
                .Include(p => p.Claim)
                .Include(p => p.Documents)
                .ToList();

            var paginatedPolicies = PageList<PolicyResponseDto>.ToPagedList(
                _mapper.Map<List<PolicyResponseDto>>(policiesQuery.ToList()),
                pageParameters.PageNumber,
                pageParameters.PageSize
            );
            Log.Information("Records Retrieved. Count{count}", paginatedPolicies.Count);

            return paginatedPolicies;
        }


        public PageList<PolicyResponseDto> GetPoliciesBySchemeId(Guid schemeId, PageParameters pageParameters)
        {
            var policiesQuery = _policyRepository.GetAll()
                .Where(p => p.InsuranceSchemeId == schemeId)
                .AsQueryable();

            var paginatedPolicies = PageList<PolicyResponseDto>.ToPagedList(
                _mapper.Map<List<PolicyResponseDto>>(policiesQuery.ToList()),
                pageParameters.PageNumber,
                pageParameters.PageSize
            );
            Log.Information("Records Retrieved. Count{count}", paginatedPolicies.Count);

            return paginatedPolicies;
        }

        public PageList<PolicyResponseDto> GetPoliciesByPlanId(Guid planId, PageParameters pageParameters)
        {
            var policiesQuery = _policyRepository.GetAll()
                .Where(p => p.InsuranceScheme.InsurancePlan.PlanId == planId)
                .AsQueryable();

            var paginatedPolicies = PageList<PolicyResponseDto>.ToPagedList(
                _mapper.Map<List<PolicyResponseDto>>(policiesQuery.ToList()),
                pageParameters.PageNumber,
                pageParameters.PageSize
            );
            Log.Information("Records Retrieved. Count{count}", paginatedPolicies.Count);

            return paginatedPolicies;
        }
        public void ApprovePolicy(Guid policyId)
        {
            // Retrieve the policy with associated documents
            var policy = _policyRepository.GetAll()
                .Include(p => p.Documents)
                .FirstOrDefault(p => p.PolicyId == policyId);

            if (policy == null)
                throw new PolicyNotFoundException("Policy not found.");

            // Update policy status
            policy.Status = PolicyStatus.ACTIVE;
            policy.IssueDate = DateTime.Now;
            // Update policy in repository
            _policyRepository.Update(policy);
            Log.Information("Record upadted", policyId);// Log the count of records retrieved

        }
        public string GetEmail(Guid policyId)
        {
            var customer = _policyRepository.GetAll().Include(c => c.Customer).Where(p => p.PolicyId == policyId).FirstOrDefault();

            return customer.Customer.Email;
        }

        public void RejectPolicy(Guid policyId)
        {
            // Retrieve the policy with associated documents
            var policy = _policyRepository.GetAll()
                .Include(p => p.Documents)
                .FirstOrDefault(p => p.PolicyId == policyId);

            if (policy == null)
                throw new PolicyNotFoundException("Policy not found.");

            // Check if any document is explicitly rejected
            policy.IssueDate= DateTime.Now;
            policy.Status = PolicyStatus.REJECTED;

            // Update policy in repository
            _policyRepository.Update(policy);
            Log.Information("Record upadted", policyId);// Log the count of records retrieved

        }
        public int GetCount()
        {
            return _policyRepository.GetAll().Count();
        }
        public int TotalPolicies(Guid agentId)
        {
            var totalPolicies = _policyRepository
                .GetAll()
                .Where(a => a.AgentId == agentId)
                .Count(); // Count the total policies
            Log.Information("Records Retrieved. Count{count}", totalPolicies);

            return totalPolicies;
        }
    }

}
