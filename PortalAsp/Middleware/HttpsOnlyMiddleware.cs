using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace PortalAsp.Middleware
{
    public class HttpsOnlyMiddleware
    {
        private readonly RequestDelegate _next;
        public HttpsOnlyMiddleware(RequestDelegate nextDelegate) => _next = nextDelegate;

        public async Task Invoke(HttpContext context)
        {
            if (context.Request.IsHttps)
            {
                await _next(context);
            }
            else
            {
                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                await context.Response.WriteAsync("Use Https instead of Http");
            }
        }
    }
}
