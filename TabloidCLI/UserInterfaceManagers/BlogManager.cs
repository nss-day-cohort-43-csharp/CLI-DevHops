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
            Console.WriteLine("3) Delete a Blog");
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
                if(title.Trim() != "")
                {
                    break;
                }
            }

            //read the user entered url
            while (true)
            {
                Console.Write("URL: ");
                url = Console.ReadLine();
                if (url.Trim() != "")
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


        // deletes a user selected blog
        private void Remove()
        {
            // get all of the blogs to list      
            List<Blog> blogs = _blogRepository.GetAll();

            // declare and initialize the id
            int id = 0;

           
            // list all of the blogs
            Console.WriteLine("Choose a blog to delete");
            for (int i = 1; i <= blogs.Count; i++)
            {
                Console.WriteLine($"{i}) {blogs[i - 1]}");
            }

            // try to delete the blog
            Console.Write("> ");
            try
            {
                id = blogs[Int32.Parse(Console.ReadLine()) - 1].Id;
                _blogRepository.Delete(id);
            }
            // let the user know if what they entered does not work
            catch 
            {
                Console.WriteLine("Invalid Selection");
            }       
        }
    }
}
