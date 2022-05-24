using System;

namespace TiendaServices.Common.Exceptions
{
    public class DocumentTypeInvalidException : Exception
    {
        public DocumentTypeInvalidException() : base("The document type is invalid")
        {
        }

        public DocumentTypeInvalidException(string message) : base(message)
        {
        }
    }
}