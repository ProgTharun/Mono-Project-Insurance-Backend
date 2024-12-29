using AutoMapper;
using InsurancePolicy.DTOs;
using InsurancePolicy.Models;
using InsurancePolicy.Repositories;
using Serilog;
using Stripe;

namespace InsurancePolicy.Services
{
    public class TaxSettingsService : ITaxSettingsService
    {
        private readonly IRepository<TaxSettings> _repository;
        private readonly IMapper _mapper;

        public TaxSettingsService(IRepository<TaxSettings> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public Guid Add(TaxSettingsRequestDto requestDto)
        {
            var taxSettings = _mapper.Map<TaxSettings>(requestDto);
            _repository.Add(taxSettings);
            Log.Information("New record added:" + taxSettings.TaxId);
            return taxSettings.TaxId;
        }

        public TaxSettingsResponseDto GetById(Guid id)
        {
            var taxSettings = _repository.GetById(id);
            if (taxSettings == null)
                throw new Exception("Tax settings not found.");
            Log.Information("Record Retrieved. ", taxSettings.TaxId);

            return _mapper.Map<TaxSettingsResponseDto>(taxSettings);
        }
        public void Update(TaxSettingsRequestDto requestDto)
        {
            var existingTaxSettings = _repository.GetById(requestDto.TaxId);
            if (existingTaxSettings == null)
                throw new Exception("Tax settings not found.");

            _mapper.Map(requestDto, existingTaxSettings); // TaxId is already validated here
            Log.Information("Record upadted", existingTaxSettings.TaxId);// Log the count of records retrieved

            _repository.Update(existingTaxSettings);
        }

        public List<TaxSettingsResponseDto> GetAll()
        {
            var taxSettingsList = _repository.GetAll().ToList();
            Log.Information("Records Retrieved. Count{count}", taxSettingsList.Count);

            return _mapper.Map<List<TaxSettingsResponseDto>>(taxSettingsList);
        }

    }

}
