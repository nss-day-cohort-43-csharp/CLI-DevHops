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
            _blogRepository = new BlogRepository(connectionString);
        }

        // excute code when returned
        public IUserInterfaceManager Execute()
        {
            // Display Blog Options
            Console.WriteLine("Blog Menu");
            Console.WriteLine("1) Add Blog");
            Console.WriteLine("0) Back");

            //read user entry
            int selection = 0;
            while (true)
            {
                try
                {
                    selection = Int32.Parse(Console.ReadLine());
                    if (selection < 0 && selection > 1)
                    {
                        throw new Exception();
                    }
                    break;
                }
                catch
                {
                    Console.WriteLine("Please enter a valid selection");
                }
            }

            // invoke the selected method
            switch(selection)
            {
                case 1:
                    Add(_blogRepository);
                    return this;
                case 0:
                    return _parentUI;
                default:
                    return this;
            }
           
        }

        private static void Add(BlogRepository blogRepo)
        {
            // read the user entered title
            Console.Write("Title: ");
            string title = Console.ReadLine();

            // read the user entered url
            Console.Write("URL: ");
            string url = Console.ReadLine();

            Blog blog = new Blog()
            {
                Title = title,
                Url = url
            };

            blogRepo.Insert(blog);
        }
    }
}
