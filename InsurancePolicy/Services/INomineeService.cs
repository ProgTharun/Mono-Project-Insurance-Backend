using InsurancePolicy.DTOs;

namespace InsurancePolicy.Services
{
    public interface INomineeService
    {
        Guid AddNominee(NomineeRequestDto nomineeDto);
        bool UpdateNominee(NomineeRequestDto nomineeDto);
        NomineeResponseDto GetNomineeById(Guid nomineeId);
        List<NomineeResponseDto> GetAllNomineesForPolicy(Guid policyId);
        bool DeleteNominee(Guid nomineeId);

    }
}
