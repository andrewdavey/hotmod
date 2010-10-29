using System;
using System.Configuration;
using System.Web;
using Hotmod.Configuration;

namespace Hotmod
{
    public class HtmlTransformer : IHttpModule
    {
        public void Init(HttpApplication application)
        {
            application.BeginRequest += application_BeginRequest;
        }

        void application_BeginRequest(object sender, EventArgs e)
        {
            var httpContext = new HttpContextWrapper(HttpContext.Current);
            InstallResponseFilter(httpContext);
        }

        public static void InstallResponseFilter(HttpContextWrapper httpContext)
        {
            if (httpContext.Response.ContentType == "text/html")
            {
                httpContext.Response.Filter = new HotFilter(httpContext.Response.Filter, httpContext, GetConfig());
            }
        }

        static HotmodSection GetConfig()
        {
            return (HotmodSection)ConfigurationManager.GetSection("hot") ?? new HotmodSection();
        }

        public void Dispose() { }
    }
}
