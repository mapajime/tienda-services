using System;
using TiendaServices.Common.Data;

namespace TiendaServices.Entities
{
    public class Purchase : IEntity
    {
        public Guid Id { get; set; }
        public DateTime PurchaseDate { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime ModificationDate { get; set; }
        public Guid FKCustomer { get; set; }
    }
}