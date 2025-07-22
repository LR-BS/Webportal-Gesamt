using System.Net;

namespace ISTA.Portal.Application.Exceptions;

public class GeneralException : Exception
{
    public string FieldName { get; }
    public HttpStatusCode HttpStatusCode { get; }

    public GeneralException(string message, string fieldName, HttpStatusCode _httpStatusCode) : base(message)
    {
        FieldName = fieldName;
        HttpStatusCode = _httpStatusCode;
    }
}