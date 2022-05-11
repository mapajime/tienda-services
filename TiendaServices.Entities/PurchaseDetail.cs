using System;

namespace TiendaServices.Entities
{
    public class PurchaseDetail
    {
        public Guid Id { get; set; }
        public int Quantity { get; set; }
        public float Price { get; set; }
        public Guid FKProduct { get; set; }
        public Guid FKPurchase { get; set; }
    }
}