using AutoMapper;
using InsurancePolicy.DTOs;
using InsurancePolicy.Models;
using InsurancePolicy.Repositories;
using Serilog;
using Stripe;

namespace InsurancePolicy.Services
{
    public class NomineeService : INomineeService
    {
        private readonly IRepository<Nominee> _nomineeRepository;
        private readonly IMapper _mapper;

        public NomineeService(IRepository<Nominee> nomineeRepository, IMapper mapper)
        {
            _nomineeRepository = nomineeRepository;
            _mapper = mapper;
        }

        public Guid AddNominee(NomineeRequestDto nomineeDto)
        {
            var nominee = _mapper.Map<Nominee>(nomineeDto);
            _nomineeRepository.Add(nominee);
            Log.Information("New record added:" + nominee.NomineeId);
            return nominee.NomineeId;
        }

        public bool UpdateNominee(NomineeRequestDto nomineeDto)
        {
            var existingNominee = _nomineeRepository.GetById(nomineeDto.NomineeId.Value);
            if (existingNominee == null)
                throw new KeyNotFoundException("Nominee not found.");

            _mapper.Map(nomineeDto, existingNominee);
            _nomineeRepository.Update(existingNominee);
            Log.Information("Record upadted", existingNominee.NomineeId);// Log the count of records retrieved

            return true;
        }

        public NomineeResponseDto GetNomineeById(Guid nomineeId)
        {
            var nominee = _nomineeRepository.GetById(nomineeId);
            if (nominee == null)
                throw new KeyNotFoundException("Nominee not found.");
            Log.Information("Record Retrieved. ", nominee.NomineeId);

            return _mapper.Map<NomineeResponseDto>(nominee);
        }

        public List<NomineeResponseDto> GetAllNomineesForPolicy(Guid policyId)
        {
            var nominees = _nomineeRepository.GetAll().Where(n => n.PolicyNo == policyId).ToList();
            Log.Information("Records Retrieved. Count{count}",nominees.Count);

            return _mapper.Map<List<NomineeResponseDto>>(nominees);
        }

        public bool DeleteNominee(Guid nomineeId)
        {
            var nominee = _nomineeRepository.GetById(nomineeId);
            if (nominee == null)
                throw new KeyNotFoundException("Nominee not found.");

            _nomineeRepository.Delete(nominee);
            Log.Information("Record upadted", nomineeId);// Log the count of records retrieved

            return true;
        }
    }
}
