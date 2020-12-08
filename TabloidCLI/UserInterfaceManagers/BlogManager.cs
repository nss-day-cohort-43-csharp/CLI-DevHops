using System;
using System.Collections.Generic;
using System.Text;
using TabloidCLI.Repositories;
using TabloidCLI.Models;


namespace TabloidCLI.UserInterfaceManagers
{
    class BlogManager : IUserInterfaceManager
    {
        // declare fields
        private readonly IUserInterfaceManager _parentUI;
        private BlogRepository _blogRepository;
        private string _connectionString;

        // initialize fields in the constructor
        public BlogManager(IUserInterfaceManager parentUI, string connectionString)
        {
            _parentUI = parentUI;
            _connectionString = connectionString;
            _blogRepository = new BlogRepository(_connectionString);
        }

        // excute code when returned
        public IUserInterfaceManager Execute()
        {
            // Display Blog Options
            Console.WriteLine("Blog Menu");
            Console.WriteLine("1) List Blogs");
            Console.WriteLine("2) Add a Blog");
            Console.WriteLine("3) Edit a Blog");
            Console.WriteLine("4) Delete a Blog");
            Console.WriteLine("0) Back");

            //read user entry
            Console.Write("> ");
            string selection = Console.ReadLine();

            // invoke the selected method
            switch(selection)
            {
                case "1":
                    List();
                    return this;
                case "2":
                    Add();
                    return this;
                case "3":
                    Edit();
                    return this;
                case "4":
                    Remove();
                    return this;
                case "0":
                    return _parentUI;
                default:
                    Console.WriteLine("Invalid selection");
                    return this;
            }
           
        }

        // List all of the blogs
        private void List()
        {
            // get all of the blogs from the databse
            List<Blog> blogs = _blogRepository.GetAll();

            // write the title and url of each
            foreach(Blog blog in blogs)
            {
                Console.WriteLine($"Title: {blog.Title}");
                Console.WriteLine($"URL: {blog.Url}\n");
            }
        }

        // add a user given blog
        private void Add()
        {
            string title = "";
            string url = "";

            // read the user entered title
            while (true)
            {
                Console.Write("Title: ");
                title = Console.ReadLine();
                if(title.Trim() != "" && title.Length <= 55)
                {
                    break;
                }
            }

            //read the user entered url
            while (true)
            {
                Console.Write("URL: ");
                url = Console.ReadLine();
                if (url.Trim() != "" && url.Length <= 2000)
                {
                    break;
                }
            }

            // create a new blog with the user entered info
            Blog blog = new Blog()
            {
                Title = title,
                Url = url
            };

            // invoke the blogRepo inster method
            _blogRepository.Insert(blog);
        }

        // lists all the blogs for the user to choose
        private int Choose(string prompt)
        {
            // get all of the blogs to list      
            List<Blog> blogs = _blogRepository.GetAll();

            // declare and initialize the id
            int id = -1;


            // list all of the blogs
            Console.WriteLine(prompt);
            for (int i = 1; i <= blogs.Count; i++)
            {
                Console.WriteLine($"{i}) {blogs[i - 1]}");
            }

            // try to set the id
            Console.Write("> ");
            try
            {
                id = blogs[Int32.Parse(Console.ReadLine()) - 1].Id;
            }
            // let the user know if what they entered does not work
            catch
            {
                Console.WriteLine("Invalid Selection");
            }

            //return the id
            return id;
        }

        // deletes a user selected blog
        private void Remove()
        {
            // get the user chosen id
            int id = Choose("Choose a blog to delete");
            // if the id was valid, delete the blog
            if(id != -1)
            {
                _blogRepository.Delete(id);
            }
        }

        // edits a user selected blog with the user given data
        private void Edit()
        {
            // get the id chose by the user
            int id = Choose("Choose a blog to edit");

            // if the id was valid
            if(id != -1)
            {
                // get the blog
                Blog blog = _blogRepository.Get(id);

                // loop until a valid title is entered
                string title = "";
                while (true)
                {
                    //prompt for the title
                    Console.Write("Title (blank to leave unchanged): ");
                    title = Console.ReadLine();
                    if(title.Length <= 55)
                    {
                        break;
                    }

                    Console.WriteLine("Title too long");
                }
                

                // set the blogs title
                if (!string.IsNullOrWhiteSpace(title))
                {
                    blog.Title = title;
                }

                string url = "";
                while (true)
                {
                    //prompt for the url
                    Console.Write("URL (blank to leave unchanged): ");
                    url = Console.ReadLine();
                    if(url.Length <= 2000)
                    {
                        break;
                    }
                    Console.WriteLine("URL too long");
                }
                

                //set the blog's url
                if (!string.IsNullOrWhiteSpace(url)  && url.Length <= 2000)
                {
                    blog.Url = url;
                }

                // edit the blog with the given data
                _blogRepository.Update(blog);
            }
        }
    }
}
