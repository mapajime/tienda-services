using System;

namespace TiendaServices.Common.Exceptions
{
    public class UserInvalidException : Exception
    {
        public UserInvalidException() : base("User Invalid")
        {
        }

        public UserInvalidException(string message) : base(message)
        {
        }
    }
}