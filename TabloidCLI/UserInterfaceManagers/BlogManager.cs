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
            // read the user entered title
            Console.Write("Title: ");
            string title = Console.ReadLine();

            // read the user entered url
            Console.Write("URL: ");
            string url = Console.ReadLine();

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
            // display all the blogs
            Console.WriteLine("Choose a blog to delete");
            List<Blog> blogs = _blogRepository.GetAll();

            foreach(Blog blog in blogs)
            {
                Console.WriteLine($"{blog.Id}) {blog.Title}");
            }
        }
    }
}
