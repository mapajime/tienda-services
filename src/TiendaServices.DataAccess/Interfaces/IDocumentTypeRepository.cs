using System.Threading.Tasks;
using TiendaServices.Common.Data;
using TiendaServices.Entities;

namespace TiendaServices.DataAccess.Interfaces
{
    public interface IDocumentTypeRepository : IRepository<DocumentType>
    {
        Task<DocumentType> GetByNameAsync(string name);
    }
}