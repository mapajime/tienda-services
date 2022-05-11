using System;

namespace TiendaServices.Entities
{
    public class PurchaseDetail : IEntity
    {
        public Guid Id { get; set; }
        public int Quantity { get; set; }
        public float Price { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime ModificationDate { get; set; }
        public Guid FKProduct { get; set; }
        public Guid FKPurchase { get; set; }
    }
}