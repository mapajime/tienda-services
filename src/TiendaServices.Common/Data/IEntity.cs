using System;

namespace TiendaServices.Common.Data
{
    public interface IEntity
    {
        public Guid Id { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime ModificationDate { get; set; }
    }
}