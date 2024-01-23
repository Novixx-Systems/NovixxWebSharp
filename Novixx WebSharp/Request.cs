using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Novixx.WebSharp
{
    public class Request
    {
        /// <summary>
        /// The query string of the request, use request.query["key"] to get the value of the key
        /// </summary>
        public Dictionary<string, string> query = new Dictionary<string, string>();
        public string method = "NULL";
        /// <summary>
        /// The post data of the request, use request.form["key"] to get the value of the key
        /// </summary>
        public Dictionary<string, string> form = new Dictionary<string, string>();
        public string path = "NULL";

        public CookieCollection Cookies;

        /// <summary>
        /// Create a new request from an HttpListenerContext, used internally by Novixx WebSharp
        /// </summary>
        /// <param name="context">The HttpListenerContext to create the request from</param>
        public Request(HttpListenerContext context)
        {
            var request = context.Request;
            method = request.HttpMethod;
            if (request.Url != null)
            {
                var query = request.Url.Query;
                if (query != null && query.Length > 1)
                {
                    var pairs = query.Substring(1).Split('&');
                    foreach (var pair in pairs)
                    {
                        var key = HttpUtility.UrlDecode(pair.Split('=')[0]);
                        var value = HttpUtility.UrlDecode(pair.Split('=')[1]);
                        this.query.Add(key, value);
                    }
                }
                path = request.Url.AbsolutePath;
            }
            Cookies = request.Cookies;
            if (method == "POST")
            {
                var body = new StreamReader(request.InputStream).ReadToEnd();
                if (body == null || body.Length < 1)
                {
                    return;
                }
                var pairs = body.Split('&');
                foreach (var pair in pairs)
                {
                    var key = HttpUtility.UrlDecode(pair.Split('=')[0]);
                    var value = HttpUtility.UrlDecode(pair.Split('=')[1]);
                    form.Add(key, value);
                }
            }
        }
    }
}
