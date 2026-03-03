using System;

namespace HoloRed.Domain.Exceptions
{
    public class ExternalServiceUnavailableException : Exception
    {
        public string ServiceName { get; }
        public string ErrorCode { get; }

        public ExternalServiceUnavailableException(string serviceName, string message, string errorCode = "EXTERNAL_UNAVAILABLE", Exception? inner = null)
            : base(message, inner)
        {
            ServiceName = serviceName;
            ErrorCode = errorCode;
        }
    }
}