﻿using System.IO;
using System.Net;
using System.Security.Principal;
using System.Text;

namespace Novixx.WebSharp
{
    public class Website
    {
        /// <summary>
        /// The name of the website
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// The header of the website, will be prepended to every page
        /// </summary>
        public string header { get; set; }
        /// <summary>
        /// The footer of the website, will be appended after every page
        /// </summary>
        public string footer { get; set; }
        /// <summary>
        /// Port number, 8080 recommended for development
        /// </summary>
        public int Port { get; set; }
        /// <summary>
        /// The list of pages on the website, use Pages.Add() to add a page
        /// </summary>
        public List<Page> Pages { get; set; } = new List<Page>();

        public Website() { }

        public Website(string name, string header, string footer, int port)
        {
            Name = name;
            this.header = header;
            this.footer = footer;
            Port = port;
        }

        /// <summary>
        /// Runs the website on the specified port
        /// </summary>
        public void Run()
        {
            var listener = new HttpListener();
            // if running as admin, use * instead of localhost
            if (OperatingSystem.IsWindows() && new WindowsPrincipal(WindowsIdentity.GetCurrent()).IsInRole(WindowsBuiltInRole.Administrator))
            {
                listener.Prefixes.Add($"http://*:{Port}/");
            }
            else if (OperatingSystem.IsLinux() || OperatingSystem.IsMacOS())
            {
                // Linux and macOS support * in the prefix even without admin as far as I know
                listener.Prefixes.Add($"http://*:{Port}/");
            }
            else
            {
                listener.Prefixes.Add($"http://localhost:{Port}/");
            }
            listener.Start();
            Console.WriteLine($"Listening on port {Port}...");
            while (true)
            {
                // Multithreading
                var context = listener.GetContext();
                // Start a new thread
                new Thread(() =>
                {
                    var request = context.Request;
                    var response = context.Response;
                    try
                    {
                        var path = request.Url.AbsolutePath;
                        var page = Pages.FirstOrDefault(p => p.Name == path);
                        if (page != null)
                        {
                            string content = page.Content;
                            if (request.HttpMethod == "GET")
                            {
                                content = page.OnGet?.Invoke(new Request(context)) ?? content;
                            }
                            else if (request.HttpMethod == "POST")
                            {
                                content = page.OnPost?.Invoke(new Request(context)) ?? content;
                            }
                            var buffer = Encoding.UTF8.GetBytes(header + content + footer);
                            response.ContentType = "text/html";
                            response.ContentLength64 = buffer.Length;
                            response.OutputStream.Write(buffer, 0, buffer.Length);
                        }
                        else
                        {
                            var buffer = Encoding.UTF8.GetBytes($"<h1>404 Not Found</h1><p>The requested URL {path} was not found on this server.</p>");
                            response.ContentLength64 = buffer.Length;
                            response.OutputStream.Write(buffer, 0, buffer.Length);
                        }
                        response.OutputStream.Close();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                        var buffer = Encoding.UTF8.GetBytes($"<h1>404 Not Found</h1><p>The requested URL was not found on this server.</p>");
                        response.ContentLength64 = buffer.Length;
                        response.OutputStream.Write(buffer, 0, buffer.Length);
                    }
                }).Start();
            }
        }
    }
}