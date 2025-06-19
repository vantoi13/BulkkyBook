namespace BulkkyBook.Exceptions
{
    public class BusinessException : Exception
    {
        public string ErrorCode { get; }

        public BusinessException(string message) : base(message)
        {
            ErrorCode = "BUSINESS_ERROR";
        }

        public BusinessException(string message, string errorCode) : base(message)
        {
            ErrorCode = errorCode;
        }

        public BusinessException(string message, Exception innerException) : base(message, innerException)
        {
            ErrorCode = "BUSINESS_ERROR";
        }
    }

    public class NotFoundException : BusinessException
    {
        public NotFoundException(string message) : base(message, "NOT_FOUND")
        {
        }
    }

    public class ValidationException : BusinessException
    {
        public ValidationException(string message) : base(message, "VALIDATION_ERROR")
        {
        }
    }

    public class ConflictException : BusinessException
    {
        public ConflictException(string message) : base(message, "CONFLICT_ERROR")
        {
        }
    }
}