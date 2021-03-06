﻿using System;
using System.Configuration;
using System.Web;
using Hotmod.Configuration;

namespace Hotmod
{
    public class Module : IHttpModule
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
            httpContext.Response.Filter = new HotmodStream(httpContext.Response.Filter, httpContext, GetConfig());
        }

        static HotmodSection GetConfig()
        {
            return (HotmodSection)ConfigurationManager.GetSection("hotmod") ?? new HotmodSection();
        }

        public void Dispose() { }
    }
}
