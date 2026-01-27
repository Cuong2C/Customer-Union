using System.Net;

namespace Customer_Union.Middleware
{
    public class IPWhiteListMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<IPWhiteListMiddleware> _logger;
        private readonly byte[][] _safelist;

        public IPWhiteListMiddleware(RequestDelegate next, ILogger<IPWhiteListMiddleware> logger, string safelist)
        {
            var ips = safelist.Split(';');
            _safelist = new byte[ips.Length][];
            for (var i = 0; i < ips.Length; i++)
            {
                _safelist[i] = IPAddress.Parse(ips[i]).GetAddressBytes();
            }

            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            var remoteIp = context.Connection.RemoteIpAddress;

            // for testing, when test dotnet does not use TCP connection so remoteIp is null => skip check IP
            if (remoteIp != null)
            {
                var bytes = remoteIp.GetAddressBytes();
                var badIp = true;
                foreach (var address in _safelist)
                {
                    if (address.SequenceEqual(bytes))
                    {
                        badIp = false;
                        break;
                    }
                }

                if (badIp)
                {
                    _logger.LogWarning("Forbidden Request from Remote IP address: {RemoteIp}", remoteIp);
                    context.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                    await context.Response.WriteAsync("403 Forbidden - Access Denied.");
                    return;
                }
            }

            await _next.Invoke(context);
        }
    }
}
