using System;

namespace TiendaServices.Entities
{
    public class Purchase
    {
        public Guid Id { get; set; }
        public DateTime PurchaseDate { get; set; }
        public Guid FKCustomer { get; set; }
        public Guid FKProduct { get; set; }
    }
}