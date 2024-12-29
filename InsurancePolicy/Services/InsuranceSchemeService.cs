using AutoMapper;
using InsurancePolicy.DTOs;
using InsurancePolicy.Exceptions.SchemeExceptions;
using InsurancePolicy.Exceptions.PlanExceptions;
using InsurancePolicy.Models;
using InsurancePolicy.Repositories;
using Microsoft.EntityFrameworkCore;
using InsurancePolicy.Helpers;
using System.ComponentModel.DataAnnotations;
using Serilog;
using Stripe;
using InsurancePolicy.enums;

namespace InsurancePolicy.Services
{
    public class InsuranceSchemeService : IInsuranceSchemeService
    {
        private readonly IRepository<InsuranceScheme> _repository;
        private readonly IRepository<InsurancePlan> _planRepository;
        private readonly IMapper _mapper;

        public InsuranceSchemeService(
            IRepository<InsuranceScheme> repository,
            IRepository<InsurancePlan> planRepository,
            IMapper mapper)
        {
            _repository = repository;
            _planRepository = planRepository;
            _mapper = mapper;
        }

        public int GetSchemeCount()
        {
            return _repository.GetAll().Count();
        }
        public PageList<InsuranceSchemeResponseDto> GetAllPaginated(PageParameters pageParameters)
        {
            var schemes = _repository.GetAll()
                .Include(s => s.InsurancePlan)
                .Include(s => s.Policies)
                .ToList();

            if (!schemes.Any())
                throw new SchemesDoesNotExistException("No schemes found.");

            var paginatedSchemes = PageList<InsuranceSchemeResponseDto>.ToPagedList(
                _mapper.Map<List<InsuranceSchemeResponseDto>>(schemes),
                pageParameters.PageNumber,
                pageParameters.PageSize
            );

            Log.Information("Records Retrieved. Count{count}", schemes.Count);

            return paginatedSchemes;
        }

        public void Activate(Guid id)
        {
            var scheme = _repository.GetById(id);
            if (scheme == null)
                throw new SchemeNotFoundException("No such scheme found to activate");
            Log.Information("Record upadted", id);// Log the count of records retrieved

            _repository.Activate(scheme);
        }

        public PageList<InsuranceSchemeResponseDto> GetAllByPlanIdPaginated(Guid planId, PageParameters pageParameters)
        {
            var plan = _planRepository.GetById(planId);
            if (plan == null)
                throw new PlanNotFoundException("No such plan found.");

            var schemes = _repository.GetAll()
                .Where(s => s.PlanId == planId)
                .Include(s => s.Policies)
                .ToList();

            if (!schemes.Any())
                throw new SchemesDoesNotExistException("No schemes found for the specified plan.");

            var paginatedSchemes = PageList<InsuranceSchemeResponseDto>.ToPagedList(
                _mapper.Map<List<InsuranceSchemeResponseDto>>(schemes),
                pageParameters.PageNumber,
                pageParameters.PageSize
            );
            Log.Information("Records Retrieved. Count{count}", paginatedSchemes.Count);

            return paginatedSchemes;
        }

        public InsuranceSchemeResponseDto GetById(Guid id)
        {
            var scheme = _repository.GetAll()
                .Include(s => s.InsurancePlan)
                .Include(s => s.Policies)
                .FirstOrDefault(s => s.SchemeId == id);

            if (scheme == null)
                throw new SchemeNotFoundException("No such scheme found.");
            Log.Information("Record Retrieved. ", id);

            return _mapper.Map<InsuranceSchemeResponseDto>(scheme);
        }

        public Guid Add(InsuranceSchemeRequestDto schemeDto)
        { 
            var errors = new List<string>();
            var plan = _planRepository.GetById(schemeDto.PlanId);
            if (plan == null || !plan.Status)
                throw new PlanNotFoundException("Plan is deactivated.");

            var scheme = _mapper.Map<InsuranceScheme>(schemeDto);

            if (scheme.MinAmount > scheme.MaxAmount)
                errors.Add("Minimum amount cannot be greater than the maximum amount.");
            if (scheme.MinAge > scheme.MaxAge)
                errors.Add("Minimum age cannot be greater than the maximum age.");

            if (scheme.MinInvestTime > scheme.MaxInvestTime)
                errors.Add("Minimum investment time cannot be greater than the maximum investment time.");


            // If there are any validation errors, throw a ValidationException
            
            if (errors.Any())
                throw new ValidationException(string.Join("; ", errors));


            _repository.Add(scheme);
            Log.Information("New record added:" + scheme.SchemeId);
            return scheme.SchemeId;
        }

        public bool Update(InsuranceSchemeRequestDto schemeDto)
        {
            var errors = new List<string>();
            var plan = _planRepository.GetById(schemeDto.PlanId);
            if (plan == null || !plan.Status)
                throw new PlanNotFoundException("Plan is deactivated.");

            var existingScheme = _repository.GetAll()
                .FirstOrDefault(s => s.SchemeId == schemeDto.SchemeId);

            if (existingScheme == null)
                throw new SchemeNotFoundException("No such scheme found.");

            var updatedScheme = _mapper.Map(schemeDto, existingScheme);

            if (updatedScheme.MinAmount > updatedScheme.MaxAmount)
                errors.Add("Minimum amount cannot be greater than the maximum amount.");
            if (updatedScheme.MinAge > updatedScheme.MaxAge)
                errors.Add("Minimum age cannot be greater than the maximum age.");

            if (updatedScheme.MinInvestTime > updatedScheme.MaxInvestTime)
                errors.Add("Minimum investment time cannot be greater than the maximum investment time.");
            // If there are any validation errors, throw a ValidationException

            if (errors.Any())
                throw new ValidationException(string.Join("; ", errors));

            _repository.Update(updatedScheme);
            Log.Information("Record upadted", updatedScheme.SchemeId);// Log the count of records retrieved

            return true;
        }
        public bool IsCustomerAssociatedWithScheme(Guid schemeId, Guid customerId)
        {
            // Fetch the scheme including its plan and related policies
            var scheme = _repository.GetAll()
                .Include(s => s.Policies) // Include related policies
                .Include(s => s.InsurancePlan) // Include the related plan
                .FirstOrDefault(s => s.SchemeId == schemeId);

            if (scheme == null)
                throw new SchemeNotFoundException("No such scheme found.");

            // Check if the customer is associated with any policy under this scheme
            var isCustomerAssociated = scheme.Policies.Any(p => p.CustomerId == customerId && p.Status != PolicyStatus.REJECTED);
            return isCustomerAssociated;
        }
        public bool Delete(Guid id)
        {
            var scheme = _repository.GetById(id);

            if (scheme == null)
                throw new SchemeNotFoundException("No such scheme found.");

            _repository.Delete(scheme);
            Log.Information("Record upadted", id);// Log the count of records retrieved

            return true;
        }
    }
}
