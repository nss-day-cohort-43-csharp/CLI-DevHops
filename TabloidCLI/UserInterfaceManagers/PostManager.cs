using System;
using System.Collections.Generic;
using System.Text;
using TabloidCLI.Models;
using TabloidCLI.Repositories;

namespace TabloidCLI.UserInterfaceManagers
{
    class PostManager : IUserInterfaceManager
    {
        private readonly IUserInterfaceManager _parentUI;
        private PostRepository _postRepository;
        private string _connectionString;

        public PostManager(IUserInterfaceManager parentUI, string connectionString)
        {
            _parentUI = parentUI;
            _postRepository = new PostRepository(connectionString);
            _connectionString = connectionString;
        }

        public IUserInterfaceManager Execute()
        {
            Console.WriteLine("Post Menu");
            Console.WriteLine(" 1) List Posts");
            Console.WriteLine(" 2) Add Post");
            Console.WriteLine(" 3) Edit Post");
            Console.WriteLine(" 4) Remove Post");
            Console.WriteLine(" 0) Go Back");

            Console.Write("> ");
            string choice = Console.ReadLine();
            switch (choice)
            {
                case "1":
                    
                    return this;
                case "2":
                    Add();
                    return this;
                case "3":
                    
                    return this;
                case "4":
                    
                    return this;
                case "0":
                    return _parentUI;
                default:
                    Console.WriteLine("Invalid Selection");
                    return this;
            }
        }
        private void Add()
        {
            Console.WriteLine("New Post");
            Post post = new Post();

            Console.Write("Title: ");
            post.Title = Console.ReadLine();

            Console.Write("URL: ");
            post.Url = Console.ReadLine();

            while (true)
            {
                Console.Write("Publication date (YYYY-MM-DD): ");
                try
                {
                    post.PublishDateTime = Convert.ToDateTime(Console.ReadLine());
                    break;
                }
                catch
                {
                    Console.WriteLine("Invalid Date");
                }
            }

            AuthorRepository authorRepo = new AuthorRepository(_connectionString);
            List<Author> authors = authorRepo.GetAll();

            while (true)
            {
                Console.WriteLine("Please choose an author");
                for (int i = 0; i < authors.Count; i++)
                {
                    Author author = authors[i];
                    Console.WriteLine($" {i + 1}) {author.FullName}");
                }
                Console.Write("> ");

                string input = Console.ReadLine();
                try
                {
                    int choice = int.Parse(input);
                    post.Author = authors[choice - 1];
                    break;
                }
                catch
                {
                    Console.WriteLine("Invalid Selection");
                }
            }

            BlogRepository blogRepo = new BlogRepository(_connectionString);
            List<Blog> blogs = blogRepo.GetAll();

            while (true)
            {
                Console.WriteLine("Please choose a blog");
                for (int i = 0; i < blogs.Count; i++)
                {
                    Blog blog = blogs[i];
                    Console.WriteLine($" {i + 1}) {blog.Title}");
                }
                Console.Write("> ");

                string input = Console.ReadLine();
                try
                {
                    int choice = int.Parse(input);
                    post.Blog = blogs[choice - 1];
                    break;
                }
                catch
                {
                    Console.WriteLine("Invalid Selection");
                }
            }

            _postRepository.Insert(post);
        }
    }
}
