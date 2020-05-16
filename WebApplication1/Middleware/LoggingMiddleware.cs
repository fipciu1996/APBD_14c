﻿namespace WebApplication1.Middleware
{
    using Microsoft.AspNetCore.Http;
    using System;
    using System.IO;
    using System.Text;
    using System.Threading.Tasks;

    public class LoggingMiddleware
    {
        private readonly RequestDelegate _next;

        public LoggingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            context.Request.EnableBuffering();
            if (context.Request != null)
            {
                string path = context.Request.Path;
                string method = context.Request.Method;
                string queryString = context.Request.QueryString.ToString();
                string bodyStr = "";

                using (var reader = new StreamReader(context.Request.Body, Encoding.UTF8, true, 1024, true))
                {
                    bodyStr = await reader.ReadToEndAsync();
                    context.Request.Body.Position = 0;
                }


                string docPath = Environment.CurrentDirectory;

                string[] lines = { method, path, bodyStr, queryString };

                File.AppendAllLines(Path.Combine(docPath, "requestsLog.txt"), lines);
            }

            if (_next != null) await _next(context);
        }
    }
}