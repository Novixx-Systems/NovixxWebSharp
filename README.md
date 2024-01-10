# What is Novixx WebSharp?

Novixx WebSharp is a library to create websites in C#! It is easy to use.

# How to use?

Import the library (obviously) and then create a new instance of the `Website` class.

Here is an example program:

```cs
using Novixx.WebSharp;

namespace Qualle
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Website website = new Website("Qualle", @"
<html>
    <head>
        <title>Qualle</title>
    </head>
    <body>
        <h1>Qualle</h1>
        <p>Welcome to Qualle!</p>", @"
<footer>
    <p>Qualle is a project by <a href=""https://novixx.com"">Novixx Systems</a>.</p>
</footer>
    </body>
</html>", 8000);
            Page index = new Page("/", "This will get replaced", new Func<string>(createLog), new Func<string>(createLog));
            website.Pages.Add(index);
            website.Run();
        }

        static string createLog()
        {
            if (new Random().Next(0, 2) == 0)
            {
                return "Hello";
            }
            else
            {
                return "Hello World";
            }
        }
    }
}
```
