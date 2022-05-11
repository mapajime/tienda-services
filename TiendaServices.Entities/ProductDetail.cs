using System;

namespace TiendaServices.Entities
{
    public class ProductDetail
    {
        public Guid Id { get; set; }
        public int Quantity { get; set; }
        public float Price { get; set; }
        public Product Product { get; set; }
        public Buy Buy { get; set; }
    }
}