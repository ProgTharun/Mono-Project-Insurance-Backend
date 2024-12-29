using InsurancePolicy.DTOs;
using InsurancePolicy.Helpers;
using InsurancePolicy.Models;

namespace InsurancePolicy.Services
{
    public interface IDocumentService
    {
        PageList<DocumentResponseDto> GetAllPaginated(PageParameters pageParameters);
        List<DocumentResponseDto> GetDocumentsByRoleId(Guid roleId, string roleType);
       // List<DocumentResponseDto> GetDocumentsByCustomerId(Guid roleId);
        string Add(DocumentRequestDto documentRequestDto);
        bool Update(DocumentRequestDto documentRequestDto);
        void ApproveDocument(string documentId, Guid employeeId);
        void RejectDocument(string documentId, Guid employeeId);
        DocumentResponseDto GetById(string documentId);
        List<DocumentTypeDto> GetAllDocumentTypes();
        public bool Reupload(string documentId, string newUrl);

        public int GetDocumentCount();
        public int GetDocumentCountByCustomerId(Guid cutsomerId);
        public PageList<DocumentResponseDto> GetDocumentsByCustomerId(Guid customerId, PageParameters pageParameters);
    }
}
