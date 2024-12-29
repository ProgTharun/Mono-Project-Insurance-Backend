using AutoMapper;
using InsurancePolicy.DTOs;
using InsurancePolicy.enums;
using InsurancePolicy.Exceptions.DocumentExceptions;
using InsurancePolicy.Helpers;
using InsurancePolicy.Models;
using InsurancePolicy.Repositories;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace InsurancePolicy.Services
{
    public class DocumentService : IDocumentService
    {
        private readonly IRepository<Document> _repository;
        private readonly IRepository<Employee> _employeeRepository;
        private readonly IMapper _mapper;

        public DocumentService(IRepository<Document> repository, IRepository<Employee> employeeRepository, IMapper mapper)
        {
            _repository = repository;
            _employeeRepository = employeeRepository;
            _mapper = mapper;
        }

        public int GetDocumentCountByCustomerId(Guid cutsomerId)
        {
            var count=_repository.GetAll().Where(d=>d.CustomerId==cutsomerId).Count();
            return count;
        }
        public PageList<DocumentResponseDto> GetAllPaginated(PageParameters pageParameters)
        {
            var documents = _repository.GetAll()
                .Include(d => d.Customer)
                .Include(d => d.VerifiedBy)
                .ToList();

            var paginatedDocuments = PageList<DocumentResponseDto>.ToPagedList(
                _mapper.Map<List<DocumentResponseDto>>(documents),
                pageParameters.PageNumber,
                pageParameters.PageSize
            );
            Log.Information("Records retrieved. Count{count}", documents.Count);
            return paginatedDocuments;
        }

        public int GetDocumentCount()
        {
            return _repository.GetAll().Count();
        }
        public List<DocumentResponseDto> GetDocumentsByRoleId(Guid roleId, string roleType)
        {
            var documents = _repository.GetAll()
                .Where(d => (roleType == "Customer" && d.CustomerId == roleId) ||
                            (roleType == "Employee" && d.VerifiedById == roleId))
                .Include(d => d.Customer)
                .Include(d => d.VerifiedBy)
                .ToList();

            if (!documents.Any())
                throw new DocumentsDoesNotExistException("No documents found for the specified role.");
            Log.Information("Records Retrieved. Count{count}",documents.Count);
            return _mapper.Map<List<DocumentResponseDto>>(documents);
        }
        public PageList<DocumentResponseDto> GetDocumentsByCustomerId(Guid customerId,PageParameters pageParameters)
        {
            var documents = _repository.GetAll()
                .Where(d => (d.CustomerId == customerId) ||
                            (d.VerifiedById == customerId))
                .Include(d => d.Customer)
                .Include(d => d.VerifiedBy)
                .ToList();

            if (!documents.Any())
                throw new DocumentsDoesNotExistException("No documents found for the specified role.");

            var paginatedDocuments = PageList<DocumentResponseDto>.ToPagedList(
                _mapper.Map<List<DocumentResponseDto>>(documents),
                pageParameters.PageNumber,
                pageParameters.PageSize
            );
            Log.Information("Records Retrieved. Count{count}", documents.Count);
            return paginatedDocuments;
        }
        public DocumentResponseDto GetById(string documentId)
        {
            var document = _repository.GetAll()
                .Include(d => d.Customer)
                .Include(d => d.VerifiedBy)
                .FirstOrDefault(d => d.DocumentId == documentId);

            if (document == null)
                throw new DocumentNotFoundException("No such document found.");
            Log.Information("Record Retrieved:", document.DocumentId);

            return _mapper.Map<DocumentResponseDto>(document);
        }
        public bool Reupload(string documentId, string newUrl)

        {

            // Fetch the document from the repository using the provided ID

            var document = _repository.GetAll()

                .FirstOrDefault(d => d.DocumentId == documentId);

            // Check if the document exists

            if (document == null)

                throw new DocumentNotFoundException("No such document found.");

            // Update the document's URL

            document.DocumentPath = newUrl;

            // Save the changes to the database

            _repository.Update(document);
            Log.Information("Record updated. ",documentId);

            return true;

        }

        public string Add(DocumentRequestDto documentRequestDto)
        {
            var document = _mapper.Map<Document>(documentRequestDto);
            var existingDocument = _repository.GetAll()
                .FirstOrDefault(d => d.CustomerId == document.CustomerId && d.DocumentName == document.DocumentName);

            if (existingDocument != null)
            {
                existingDocument.CustomerId = document.CustomerId;
                existingDocument.DocumentName = document.DocumentName;
                existingDocument.Status = Status.PENDING;
                existingDocument.DocumentPath = document.DocumentPath;
                _repository.Update(existingDocument);
                Log.Information("Record Updated.",document.DocumentId);
                return existingDocument.DocumentId;
            }
            else
            {
                _repository.Add(document);
                Log.Information("New Record Added", document.DocumentId);
                return document.DocumentId;
            }

        }


        public bool Update(DocumentRequestDto documentRequestDto)
        {
            var existingDocument = _repository.GetAll()
                .FirstOrDefault(d => d.DocumentId == documentRequestDto.DocumentId);

            if (existingDocument == null)
                throw new DocumentNotFoundException("No such document found.");

            _mapper.Map(documentRequestDto, existingDocument);
            _repository.Update(existingDocument);
            Log.Information("Record upadted.",existingDocument.DocumentId);
            return true;
        }

        public void ApproveDocument(string documentId, Guid employeeId)
        {
            UpdateStatus(documentId, Status.APPROVED, employeeId);
        }

        public void RejectDocument(string documentId, Guid employeeId)
        {
            UpdateStatus(documentId, Status.REJECTED, employeeId);
        }

        private void UpdateStatus(string documentId, Status status, Guid employeeId)
        {
            var document = _repository.GetAll()
                .Include(d => d.VerifiedBy)
                .FirstOrDefault(d => d.DocumentId == documentId);

            if (document == null)
                throw new DocumentNotFoundException("No such document found.");

            document.Status = status;
            var employee = _employeeRepository.GetById(employeeId);
            if (employee == null)
                throw new KeyNotFoundException("Employee not found for verification.");

            document.VerifiedById = employeeId;
            Log.Information("Record updated.", documentId);
            _repository.Update(document);
        }
        public List<DocumentTypeDto> GetAllDocumentTypes()
        {
            return Enum.GetValues(typeof(DocumentType))
                       .Cast<DocumentType>()
                       .Select(dt => new DocumentTypeDto
                       {
                           Id = (int)dt,
                           Name = dt.ToString()
                       })
                       .ToList();

        }


    }
}
