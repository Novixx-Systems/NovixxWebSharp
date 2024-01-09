using System.Net;
using System.Security.Principal;
using System.Text;

namespace Novixx.WebSharp
{
    public class Website
    {
        public string Name { get; set; }
        public string header { get; set; }
        public string footer { get; set; }
        public int Port { get; set; }

        public List<Page> Pages { get; set; }

        public Website() { }

        public Website(string name, string header, string footer, int port)
        {
            Name = name;
            this.header = header;
            this.footer = footer;
            Port = port;
        }

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
                    var path = request.Url.AbsolutePath;
                    var page = Pages.FirstOrDefault(p => p.Name == path);
                    if (page != null)
                    {
                        if (request.HttpMethod == "GET")
                        {
                            page.OnGet?.Invoke();
                        }
                        else if (request.HttpMethod == "POST")
                        {
                            page.OnPost?.Invoke();
                        }
                        var buffer = Encoding.UTF8.GetBytes(page.Content);
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
                }).Start();
            }
        }
    }
}