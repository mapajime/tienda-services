using System;

namespace TiendaServices.Common.Exceptions
{
    public class CategoryInvalidException : Exception
    {
        public CategoryInvalidException() : base("Category invalid")
        {
        }

        public CategoryInvalidException(string message) : base(message)
        {
        }
    }
}