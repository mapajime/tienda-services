using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TiendaServices.Entities;

namespace TiendaServices.Business.Intarfaces
{
    public interface IDocumentTypeBusiness
    {
        Task<bool> CreateDocumentTypeAsync(DocumentType City);

        Task<bool> UpdateDocumentTypeAsync(DocumentType City);

        Task<bool> DeleteDocumentTypeAsync(Guid id);

        Task<DocumentType> GetDocumentTypeByIdAsync(Guid id);

        Task<IEnumerable<DocumentType>> GetAllDocumentTypeAsync();
    }
}