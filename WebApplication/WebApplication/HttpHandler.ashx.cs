using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication
{
    /// <summary>
    /// Summary description for HttpHandler
    /// </summary>
    public class HttpHandler : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            string simulate500 = context.Request.QueryString["simulate500"] ;
            if (simulate500 == "Yes")
            {
                context.Response.StatusCode = 500;
                context.Response.StatusDescription = "Task failed successfully";
            }

            context.Response.ContentType = "text/plain";
            context.Response.Write(DateTime.UtcNow.ToString(@"MM\/dd\/yyyy HH:mm"));
            context.Response.StatusDescription = "Task completed successfully";
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}