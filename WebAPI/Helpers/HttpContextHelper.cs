using Microsoft.AspNetCore.Http.Features;

namespace WebApi.Helpers
{
    public class HttpContextHelper
    {
        public static string GetClientIpAddress(HttpContext httpContext)
        {
            return httpContext.Features.Get<IHttpConnectionFeature>().RemoteIpAddress.ToString();
            var extractedIpAddress = httpContext.Request.Headers["X-Forwarded-For"];

            if (!string.IsNullOrEmpty(extractedIpAddress))
            {
                return extractedIpAddress;
            }
        }
    }
}
