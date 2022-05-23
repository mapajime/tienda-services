using System;
using TiendaServices.Common.Data;

namespace TiendaServices.Entities
{
    public class City : IEntity
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime ModificationDate { get; set; }
    }
}