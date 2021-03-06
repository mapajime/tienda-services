using System;
using TiendaServices.Common.Data;

namespace TiendaServices.Entities
{
    public class Product : IEntity
    {
        public Guid Id { get; set; }
        public string ProductName { get; set; }
        public float Price { get; set; }
        public string Trademark { get; set; }
        public string Description { get; set; }
        public int Stock { get; set; }
        public DateTime CreationDate { get; set; }
        public bool Active { get; set; }
        public DateTime ModificationDate { get; set; }
        public Guid FKCategory { get; set; }
    }
}