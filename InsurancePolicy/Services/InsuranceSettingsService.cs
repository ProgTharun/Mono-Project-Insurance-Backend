using AutoMapper;
using InsurancePolicy.DTOs;
using InsurancePolicy.Models;
using InsurancePolicy.Repositories;

namespace InsurancePolicy.Services
{
    public class InsuranceSettingsService : IInsuranceSettingsService

    {
        private readonly IRepository<InsuranceSettings> _repository;
        private readonly IMapper _mapper;

        public InsuranceSettingsService(IRepository<InsuranceSettings> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public Guid Add(InsuranceSettingsRequestDto requestDto)
        {
            var settings = _mapper.Map<InsuranceSettings>(requestDto);
            _repository.Add(settings);
            return settings.Id;
        }

        public InsuranceSettingsResponseDto GetById(Guid id)
        {
            var settings = _repository.GetById(id);
            if (settings == null)
                throw new Exception("Insurance settings not found.");

            return _mapper.Map<InsuranceSettingsResponseDto>(settings);
        }

        public List<InsuranceSettingsResponseDto> GetAll()
        {
            var settings = _repository.GetAll().ToList();
            return _mapper.Map<List<InsuranceSettingsResponseDto>>(settings);
        }
    }
}

