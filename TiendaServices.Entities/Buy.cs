using System;

namespace TiendaServices.Entities
{
    public class Buy
    {
        public Guid Id { get; set; }
        public DateTime BuyDate { get; set; }
        public Customer Customer { get; set; }
        public Product Product { get; set; }
    }
}