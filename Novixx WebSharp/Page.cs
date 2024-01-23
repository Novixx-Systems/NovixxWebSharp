namespace Novixx.WebSharp
{
    public class Page
    {
        /// <summary>
        /// The name of the page, use / for index
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// The content of the page, will be used if OnGet and/or OnPost return null, or are not specified at all
        /// </summary>
        public string Content { get; set; }
        /// <summary>
        /// A function that gets executed on a GET request
        /// </summary>
        public Func<Request, string> OnGet { get; set; }
        /// <summary>
        /// A function that gets executed on a POST request
        /// </summary>
        public Func<Request, string> OnPost { get; set; }

        public Page() { }

        public Page(string name, string content)
        {
            Name = name;
            Content = content;
        }

        /// <summary>
        /// Create a new page with a name, content, and a function to run when the page is requested with GET or POST
        /// </summary>
        /// <param name="name">The name of the page, use / for index</param>
        /// <param name="content">The content of the page, will be used if OnGet and/or OnPost return null</param>
        /// <param name="onGet">A function that gets executed on a GET request</param>
        /// <param name="onPost">A function that gets executed on a POST request</param>
        public Page(string name, string content, Func<Request, string> onGet, Func<Request, string> onPost)
        {
            Name = name;
            Content = content;
            OnGet = onGet;
            OnPost = onPost;
        }

        /// <summary>
        /// Create a new page with a name, content, and a function to run when the page is requested with POST
        /// </summary>
        /// <param name="name">The name of the page, use / for index</param>
        /// <param name="content">The content of the page, will be used if OnPost returns null</param>
        /// <param name="onPost">A function that gets executed on a POST request</param>
        public Page(string name, string content, Func<Request, string> onPost)
        {
            Name = name;
            Content = content;
            OnPost = onPost;
        }

        /// <summary>
        /// Create a new page with a name and a function to run when the page is requested with GET
        /// </summary>
        /// <param name="name">The name of the page, use / for index</param>
        /// <param name="onGet">A function that gets executed on a GET request</param>
        /// <param name="onPost">A function that gets executed on a POST request</param>
        public Page(string name, Func<Request, string> onGet, Func<Request, string> onPost)
        {
            Name = name;
            OnGet = onGet;
            OnPost = onPost;
        }

        public override string ToString()
        {
            return Name;
        }
    }
}