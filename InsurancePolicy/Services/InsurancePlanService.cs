using AutoMapper;
using InsurancePolicy.DTOs;
using InsurancePolicy.Exceptions.PlanExceptions;
using InsurancePolicy.Helpers;
using InsurancePolicy.Models;
using InsurancePolicy.Repositories;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Stripe;
using System.ComponentModel.DataAnnotations;

namespace InsurancePolicy.Services
{
    public class InsurancePlanService:IInsurancePlanService
    {
        private readonly IRepository<InsurancePlan> _repository;
        private readonly IMapper _mapper;

        public InsurancePlanService(IRepository<InsurancePlan> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public Guid Add(InsurancePlanRequestDto planRequest)
        {
            var existingPlan = _repository.GetAll()
                                  .FirstOrDefault(p => p.PlanName.ToLower() == planRequest.PlanName.ToLower());
            if (existingPlan != null)
            {
                throw new ValidationException("Plan already exists");
            }

            var plan = _mapper.Map<InsurancePlan>(planRequest);
            _repository.Add(plan);

            Log.Information("New Record Added.",plan.PlanId);
            return plan.PlanId;
        }

        public int GetPlanCount()
        {
            return _repository.GetAll().Count();
        }

        public void Delete(Guid id)
        {
            var plan = _repository.GetById(id);
            if (plan == null)
                throw new PlanNotFoundException("No such plan found to delete");
            Log.Information("Record Updated.", id);

            _repository.Delete(plan);
        }
        public void Activate(Guid id)
        {
            var plan = _repository.GetById(id);
            if (plan == null)
                throw new PlanNotFoundException("No such plan found to delete");
            Log.Information("Record Updated.", id);
            _repository.Activate(plan);
        }

        public InsurancePlanResponseDto GetById(Guid id)
        {
            var plan = _repository.GetAll()
                .Include(p => p.Schemes)
                .FirstOrDefault(p => p.PlanId == id);

            if (plan == null)
                throw new PlanNotFoundException("No such plan found");
            Log.Information("Record Retrieved. ", id);

            return _mapper.Map<InsurancePlanResponseDto>(plan);
        }
        public PageList<InsurancePlanResponseDto> GetAllPaginated(PageParameters pageParameters)
        {
            // Fetch only active plans
            var activePlans = _repository.GetAll().Include(p => p.Schemes).ToList();

            // Map to response DTO and apply pagination
            var paginatedPlans = PageList<InsurancePlanResponseDto>.ToPagedList(
                _mapper.Map<List<InsurancePlanResponseDto>>(activePlans),
                pageParameters.PageNumber,
                pageParameters.PageSize
            );
            Log.Information("Records Retrieved. Count{count}", activePlans.Count);

            return paginatedPlans;
        }


        public List<InsurancePlanResponseDto> GetAll()
        {
            // Fetch only active plans
            var activePlans = _repository.GetAll()
                .Include(p => p.Schemes)
                .Where(p => p.Status) // Filter plans with Status == true
                .ToList();

            if (!activePlans.Any())
                throw new PlansDoesNotExistException("No active plans exist");
            Log.Information("Records Retrieved. Count{count}", activePlans.Count);

            return _mapper.Map<List<InsurancePlanResponseDto>>(activePlans);
        }


        public void Update(InsurancePlanRequestDto planRequest)
        {
            var existingPlan = _repository.GetAll()
         .Include(p => p.Schemes) // Include related schemes
         .FirstOrDefault(p => p.PlanId == planRequest.PlanId);

            if (existingPlan == null)
                throw new PlanNotFoundException("No such plan found");

            // Update the plan details
            _mapper.Map(planRequest, existingPlan);

            // If the plan status is set to false, deactivate related schemes
            if (!existingPlan.Status && existingPlan.Schemes != null)
            {
                foreach (var scheme in existingPlan.Schemes)
                {
                    scheme.Status = false;
                }
            }
            Log.Information("Record upadted", planRequest.PlanId);// Log the count of records retrieved

            _repository.Update(existingPlan);

        }
    }
}
