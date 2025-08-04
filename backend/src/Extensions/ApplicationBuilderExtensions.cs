using Microsoft.AspNetCore.Builder;
using backend.src.Middlewares;

namespace backend.src.Extensions
{
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseErrorHandling(this IApplicationBuilder app)
        {
            return app.UseMiddleware<ErrorHandlingMiddleware>();
        }
    }
}
