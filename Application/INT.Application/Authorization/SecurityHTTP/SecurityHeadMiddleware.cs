using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace INT.Application.Authorization
{
    public class SecurityHeadMiddleware
    {
        #region Miembros

        private readonly RequestDelegate RequestDelegate;

        #endregion

        #region Constructor

        public SecurityHeadMiddleware(RequestDelegate requestDelegate)
        {
            RequestDelegate = requestDelegate;
        }

        #endregion

        #region Métodos

        public async Task Invoke(HttpContext context)
        {
            try
            {
                context.Response.OnStarting(() =>
                {
                    if (context.Response.Headers.ContainsKey("x-frame-options"))
                    {
                        context.Response.Headers.Remove("x-frame-options");
                    }
                    if (context.Response.Headers.ContainsKey("x-xss-protection"))
                    {
                        context.Response.Headers.Remove("x-xss-protection");
                    }
                    // https://developer.mozilla.org/en-US/docs/Web/HTTP/Headers/X-Frame-Options
                    context.Response.Headers.Add("x-frame-options", new StringValues("SAMEORIGIN"));

                    //https://developer.mozilla.org/en-US/docs/Web/HTTP/Headers/X-XSS-Protection
                    context.Response.Headers.Add("x-xss-protection", new StringValues("1; mode=block"));

                    return Task.CompletedTask;
                });

                await RequestDelegate(context);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion
    }
}
