namespace FaceIO.Contracts.Common.Exceptions
{
    using System;

    public class FaceIONotFoundException : Exception
    {
        public FaceIONotFoundException() : base()
        {
        }

        public FaceIONotFoundException(string? message) : base(message)
        {
        }

        public FaceIONotFoundException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}