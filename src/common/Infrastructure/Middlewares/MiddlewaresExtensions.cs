using Microsoft.AspNetCore.Builder;

namespace Infrastructure.Middlewares
{
    public static class MiddlewaresExtensions
    {
        /// <summary>
        /// Use global error handling as middleware
        /// </summary>
        /// <param name="builder">Application Builder</param>
        /// <returns></returns>
        public static IApplicationBuilder UseGlobalErrorHandling(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<GlobalErrorHandling>();
        }
    }
}
