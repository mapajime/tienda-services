using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TiendaServices.Business.Intarfaces;
using TiendaServices.Common.Exceptions;
using TiendaServices.DataAccess.Interfaces;
using TiendaServices.Entities;

namespace TiendaServices.Business.Implementations
{
    public class DocumentTypeBusiness : IDocumentTypeBusiness
    {
        private readonly IDocumentTypeRepository _documentTypeRepository;

        public DocumentTypeBusiness(IDocumentTypeRepository documentTypeRepository)
        {
            _documentTypeRepository = documentTypeRepository;
        }

        public async Task<bool> CreateDocumentTypeAsync(DocumentType documentType)
        {
            await ValidateAsync(documentType);
            var isDocumentType = await _documentTypeRepository.CreateAsync(documentType);
            return isDocumentType;
        }

        public async Task<bool> DeleteDocumentTypeAsync(Guid id)
        {
            var isDelete = await _documentTypeRepository.DeleteAsync(id);
            return isDelete;
        }

        public async Task<IEnumerable<DocumentType>> GetAllDocumentTypeAsync()
        {
            var documentType = (await _documentTypeRepository.GetAllAsync()).ToList();
            return documentType;
        }

        public async Task<DocumentType> GetDocumentTypeByIdAsync(Guid id)
        {
            var documentType = await _documentTypeRepository.GetByIdAsync(id);
            return documentType;
        }

        public async Task<bool> UpdateDocumentTypeAsync(DocumentType documentType)
        {
            await ValidateAsync(documentType);
            var isUpdateDocumentType = await _documentTypeRepository.UpdateAsync(documentType);
            return isUpdateDocumentType;
        }

        private async Task ValidateAsync(DocumentType documentType)
        {
            if (documentType == null)
            {
                throw new DocumentTypeInvalidException("The document type is null");
            }
            if (string.IsNullOrEmpty(documentType.Name))
            {
                throw new DocumentTypeInvalidException("The name document type is empty");
            }
            if (await _documentTypeRepository.GetByNameAsync(documentType.Name) != null)
            {
                throw new DocumentTypeInvalidException("The name document type already exists");
            }
        }
    }
}