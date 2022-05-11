using System;

namespace TiendaServices.Entities
{
    public class Customer
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string DocumentNumber { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public Guid FKCountry { get; set; }
        public Guid FKDocument { get; set; }
    }
}