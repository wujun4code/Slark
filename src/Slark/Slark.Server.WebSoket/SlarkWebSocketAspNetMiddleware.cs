using System;
using Microsoft.AspNetCore.Http;

namespace Slark.Server.WebSoket
{
    public class SlarkWebSocketAspNetMiddleware
    {
        private readonly RequestDelegate _next;
        public SlarkWebSocketAspNetMiddleware(RequestDelegate next)
        {
            _next = next;
        }
    }
}
