namespace Novixx.WebSharp
{
    public class Page
    {
        public string Name { get; set; }
        public string Content { get; set; }
        public Action OnGet { get; set; }
        public Action OnPost { get; set; }

        public Page() { }

        public Page(string name, string content)
        {
            Name = name;
            Content = content;
        }

        public Page(string name, string content, Action onGet, Action onPost)
        {
            Name = name;
            Content = content;
            OnGet = onGet;
            OnPost = onPost;
        }

        public Page(string name, string content, Action onPost)
        {
            Name = name;
            Content = content;
            OnPost = onPost;
        }

        public override string ToString()
        {
            return Name;
        }
    }
}