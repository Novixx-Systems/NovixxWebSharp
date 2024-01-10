namespace Novixx.WebSharp
{
    public class Page
    {
        public string Name { get; set; }
        public string Content { get; set; }
        public Func<string> OnGet { get; set; }
        public Func<string> OnPost { get; set; }

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
        public Page(string name, string content, Func<string> onGet, Func<string> onPost)
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
        public Page(string name, string content, Func<string> onPost)
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
        public Page(string name, Func<string> onGet, Func<string> onPost)
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