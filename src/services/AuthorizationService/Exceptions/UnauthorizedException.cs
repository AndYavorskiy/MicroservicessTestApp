using System;

namespace AuthorizationService.Exceptions
{
    public class UnauthorizedException : Exception
    {
        public UnauthorizedException(string message = null, Exception ex = null) : base(message, ex)
        {
        }
    }
}
