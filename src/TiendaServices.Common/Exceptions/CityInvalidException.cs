using System;

namespace TiendaServices.Common.Exceptions
{
    public class CityInvalidException : Exception
    {
        public CityInvalidException() : base("Country is invalid")
        {
        }

        public CityInvalidException(string message) : base(message)
        {
        }
    }
}