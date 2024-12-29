using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using InsurancePolicy.DTOs;
using InsurancePolicy.enums;
using InsurancePolicy.Helpers;
using InsurancePolicy.Models;
using InsurancePolicy.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.ComponentModel.DataAnnotations;

namespace InsurancePolicy.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DocumentController : ControllerBase
    {
        private readonly IDocumentService _service;
        private readonly Cloudinary _cloudinary;
        public DocumentController(IDocumentService service, IOptions<CloudinarySettings> cloudinarySettings)
        {
            _service = service;
            var settings = cloudinarySettings.Value;
            _cloudinary = new Cloudinary(new Account(settings.CloudName, settings.ApiKey, settings.ApiSecret));
        }
        [HttpGet("Count")]
        public IActionResult GetCount()
        {
            var count=_service.GetDocumentCount();
            return Ok(new { count });   
        }
        [HttpGet("CustomerCount")]
        public IActionResult GetDocumentCountByCustomerId(Guid customerId)
        {
            var count =_service.GetDocumentCountByCustomerId(customerId);
            return Ok(new { count });
        }


        [HttpGet]
        public IActionResult GetAll([FromQuery] PageParameters pageParameters)
        {
            var documents = _service.GetAllPaginated(pageParameters);

            Response.Headers.Add("X-Total-Count", documents.TotalCount.ToString());
            Response.Headers.Add("X-Page-Size", documents.PageSize.ToString());
            Response.Headers.Add("X-Current-Page", documents.CurrentPage.ToString());
            Response.Headers.Add("X-Total-Pages", documents.TotalPages.ToString());
            Response.Headers.Add("X-Has-Next", documents.HasNext.ToString());
            Response.Headers.Add("X-Has-Previous", documents.HasPrevious.ToString());

            return Ok(documents);
        }
        [HttpGet("{documentId}")]
        public IActionResult GetById(string documentId)
        {
            var document = _service.GetById(documentId);
            return Ok(document);
        }

        [HttpGet("{roleId}/{roleType}")]
        public IActionResult GetDocumentsByRole(Guid roleId, string roleType)
        {
            var documents = _service.GetDocumentsByRoleId(roleId, roleType);
            return Ok(documents);
        }

        [HttpGet("customer/{customerId}"), Authorize(Roles = "Admin,Employee")]
        public IActionResult GetDocumentByCustomerId(Guid customerId, [FromQuery] PageParameters pageParameters)
        {
            var documents = _service.GetDocumentsByCustomerId(customerId, pageParameters);

            Response.Headers.Add("X-Total-Count", documents.TotalCount.ToString());
            Response.Headers.Add("X-Page-Size", documents.PageSize.ToString());
            Response.Headers.Add("X-Current-Page", documents.CurrentPage.ToString());
            Response.Headers.Add("X-Total-Pages", documents.TotalPages.ToString());
            Response.Headers.Add("X-Has-Next", documents.HasNext.ToString());
            Response.Headers.Add("X-Has-Previous", documents.HasPrevious.ToString());

            return Ok(documents);
        }



        [HttpPost]
        public IActionResult Add([FromBody] DocumentRequestDto documentRequestDto)
        {
            var documentId = _service.Add(documentRequestDto);
            return Ok(new { DocumentId = documentId, Message = "Document added successfully." });
        }
        [HttpPut("reupload")]
        public IActionResult Reupload(string id, string url)
        {
            _service.Reupload(id, url);
            return Ok(new { Message = "Document updated Successfully." });
        }

        [HttpPut]
        public IActionResult Update([FromBody] DocumentRequestDto documentRequestDto)
        {
             _service.Update(documentRequestDto);
            return Ok(new { Message = "Document updated successfully." });
        }

        [HttpPut("approve/{documentId}"), Authorize(Roles = "Employee")]
        public IActionResult ApproveDocument(string documentId, [FromQuery] Guid employeeId)
        {
            _service.ApproveDocument(documentId, employeeId);
            return Ok(new { Message = "Document approved successfully." });
        }

        [HttpPut("reject/{documentId}"), Authorize(Roles = "Employee")]
        public IActionResult RejectDocument(string documentId, [FromQuery] Guid employeeId)
        {
           _service.RejectDocument(documentId, employeeId);
            return Ok(new { Message = "Document rejected successfully." });
        }
        [HttpGet("types")]
        public IActionResult GetDocumentTypes()
        {
            var documentTypes = Enum.GetValues(typeof(DocumentType))
                                    .Cast<DocumentType>()
                                    .Select(dt => new
                                    {
                                        Id = (int)dt,
                                        Name = dt.ToString()
                                    })
                                    .ToList();

            return Ok(documentTypes);
        }
        [HttpPost("upload")]
        public async Task<IActionResult> UploadPhoto(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("File is required.");
            }

            // Ensure the file is a valid image
            if (!file.ContentType.StartsWith("image/"))
                return BadRequest("Only image files are allowed.");

            // Upload the photo to Cloudinary
            var uploadParams = new ImageUploadParams
            {
                File = new FileDescription(file.FileName, file.OpenReadStream()),
                Folder = "insurance-documents" // Optional: Specify a folder in Cloudinary
            };

            var uploadResult = await _cloudinary.UploadAsync(uploadParams);

            if (uploadResult.StatusCode != System.Net.HttpStatusCode.OK)
                return StatusCode((int)uploadResult.StatusCode, "Photo upload failed.");

            return Ok(new
            {
                PublicId = uploadResult.PublicId,
                Url = uploadResult.SecureUrl
            });
        }


    }
}
