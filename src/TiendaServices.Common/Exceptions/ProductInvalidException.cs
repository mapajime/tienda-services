using System;

namespace TiendaServices.Common.Exceptions
{
    public class ProductInvalidException : Exception
    {
        public ProductInvalidException() : base("The producto is invalid")
        {
        }

        public ProductInvalidException(string message) : base(message)
        {
        }
    }
}