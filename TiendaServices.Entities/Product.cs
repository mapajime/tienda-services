using System;

namespace TiendaServices.Entities
{
    public class Product
    {
        public Guid Id { get; set; }
        public string ProductName { get; set; }
        public float Price { get; set; }
        public string TradeMark { get; set; }
        public string Description { get; set; }
        public int Stock { get; set; }
        public DateTime CreateDate { get; set; }
        public Guid FKCategory { get; set; }
    }
}