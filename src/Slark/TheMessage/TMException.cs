using System;
namespace TheMessage
{
    public class TMException : Exception
    {
        public TMException()
        {

        }

        public int ErrorCode { get; set; }

        public TMException(int errorCode, string message) : base(message)
        {
            ErrorCode = errorCode;
        }
    }
}
