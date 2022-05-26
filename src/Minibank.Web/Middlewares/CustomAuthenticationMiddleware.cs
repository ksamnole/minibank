using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;
using Minibank.Core;

namespace Minibank.Web.Middlewares
{
    public class CustomAuthenticationMiddleware
    {
        public readonly RequestDelegate next;

        public CustomAuthenticationMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            var handler = new JwtSecurityTokenHandler();
            var accessToken = httpContext.Request.Headers["Authorization"];
            
            if (accessToken.Count > 0)
            {
                var token = handler.ReadJwtToken(accessToken[0].Split()[1]);
                
                if (token.ValidTo < DateTime.UtcNow)
                {
                    httpContext.Response.StatusCode = StatusCodes.Status403Forbidden;
                    await httpContext.Response.WriteAsJsonAsync(new { Message = "JWT токен просрочен" });
                }
            }
            else
            {
                await next(httpContext);
            }
        }
    }
}