using System;

namespace TiendaServices.Entities
{
    public class DocumentType : IEntity
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime ModificationDate { get; set; }
    }
}