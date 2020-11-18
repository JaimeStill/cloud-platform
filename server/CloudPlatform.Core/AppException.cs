using System;

namespace CloudPlatform.Core
{
    public enum ExceptionType
    {
        Validation,
        Authorization
    }

    public class AppException : Exception
    {
        public ExceptionType ExceptionType { get; set; }

        public AppException(string message, ExceptionType exceptionType) : base(message)
        {
            ExceptionType = exceptionType;
        }
    }
}
