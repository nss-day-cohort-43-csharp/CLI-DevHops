using System;
using System.Collections.Generic;
using System.Text;
using TabloidCLI.Models;
using TabloidCLI.Repositories;

namespace TabloidCLI.UserInterfaceManagers
{
    class PostManager : IUserInterfaceManager
    {
        //Declares fields
        private readonly IUserInterfaceManager _parentUI;
        private PostRepository _postRepository;
        private string _connectionString;

        //Initializes fields upon creation of a PostManager instance
        public PostManager(IUserInterfaceManager parentUI, string connectionString)
        {
            _parentUI = parentUI;
            _postRepository = new PostRepository(connectionString);
            _connectionString = connectionString;
        }

        //Creates menu for posts and performs given case
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
                    Console.WriteLine("Invalid Selection");
                    return this;
            }
        }

        //Lists all posts
        private void List()
        {
            //Get all the posts from the database
            List<Post> posts = _postRepository.GetAll();

            //Output title and url for each post to the console
            foreach (Post post in posts)
            {
                Console.WriteLine($"Title: {post.Title}");
                Console.WriteLine($"URL: {post.Url}\n");
            }
        }

        //Takes in user input for title, url, date, author, and blog and then runs the insert method on the post repo
        private void Add()
        {
            Console.WriteLine("New Post");
            Post post = new Post();
            string userResponse;
            
            while (true)
            {
                Console.Write("Title: ");
                userResponse = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(userResponse))
                {
                    post.Title = userResponse;
                    break;
                }
            }

            while (true)
            {
                Console.Write("URL: ");
                userResponse = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(userResponse))
                {
                    post.Url = userResponse;
                    break;
                }
            }

            while (true)
            {
                Console.Write("Publication date (YYYY-MM-DD): ");
                try
                {
                    DateTime input = Convert.ToDateTime(Console.ReadLine());
                    DateTime limit = new DateTime(1753, 1, 1);
                    //Check if date is within SQL limits and not in the future
                    if (input.Date > limit.Date && input.Date < DateTime.Now)
                    {
                        //Converts user input to DateTime type
                        post.PublishDateTime = input;
                        break;
                    }
                    else
                    {
                        throw new Exception();
                    }
                }
                catch
                {
                    Console.WriteLine("Invalid Date");
                }
            }

            //Creates a new author repo instance
            AuthorRepository authorRepo = new AuthorRepository(_connectionString);
            //Creates list of all authors in database
            List<Author> authors = authorRepo.GetAll();

            //User chooses author for post from the presented list of all authors
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

            //Creates a new blog repo instance
            BlogRepository blogRepo = new BlogRepository(_connectionString);
            //Creates list of all blogs in database
            List<Blog> blogs = blogRepo.GetAll();

            //User chooses blog for post from the presented list of all blogs
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

        private void Edit()
        {
            //Gets list of all posts
            List<Post> posts = _postRepository.GetAll();
            //Declares postToEdit variable and sets it to null
            Post postToEdit = null;

            //Creates list of posts for user to choose from
            Console.WriteLine("Which post would you like to edit?");
            for (int i = 0; i < posts.Count; i++)
            {
                Post post = posts[i];
                Console.WriteLine($" {i + 1}) {post.Title}");
            }
            Console.Write("> ");

            //Trys to get selected post from list
            string input = Console.ReadLine();
            try
            {
                int choice = int.Parse(input);
                postToEdit = posts[choice - 1];
            }
            catch
            {
                //If invalid selection, go back to post menu
                Console.WriteLine("Invalid Selection");
                Execute();
            }

            Console.WriteLine();
            Console.Write("New Title (blank to leave unchanged): ");
            //Get user input for new title. If blank, it remains unchanged
            string title = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(title))
            {
                postToEdit.Title = title;
            }

            Console.Write("New URL (blank to leave unchanged): ");
            string url = Console.ReadLine();
            //Get user input for new url. If blank, it remains unchanged
            if (!string.IsNullOrWhiteSpace(url))
            {
                postToEdit.Url = url;
            }

            //Get user input for new date. If blank, it remains unchanged
            while (true)
            {
                Console.Write("New publication date (blank to leave unchanged): ");
                try
                {
                    string date = Console.ReadLine();
                    //If user input blank, break out of loop
                    if (string.IsNullOrWhiteSpace(date))
                    {
                        break;
                    }

                    DateTime newDate = Convert.ToDateTime(date);
                    DateTime limit = new DateTime(1753, 1, 1);
                    //Check if date is within SQL limits and not in the future
                    if (newDate.Date > limit.Date && newDate.Date < DateTime.Now)
                    {
                        //Converts user input to DateTime type
                        postToEdit.PublishDateTime = newDate;
                        break;
                    }
                    else
                    {
                        throw new Exception();
                    }
                }
                catch
                {
                    Console.WriteLine("Invalid Date");
                }
            }
            
            //Creates a new author repo instance
            AuthorRepository authorRepo = new AuthorRepository(_connectionString);
            //Creates list of all authors in database
            List<Author> authors = authorRepo.GetAll();

            //User chooses new author for post from the presented list of all authors. If blank, it remains unchanged
            while (true)
            {
                Console.WriteLine("New author (blank to leave unchanged): ");
                for (int i = 0; i < authors.Count; i++)
                {
                    Author author = authors[i];
                    Console.WriteLine($" {i + 1}) {author.FullName}");
                }
                Console.Write("> ");

                string newAuthor = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(newAuthor))
                {
                    break;
                }

                try
                {
                    int choice = int.Parse(newAuthor);
                    postToEdit.Author = authors[choice - 1];
                    break;
                }
                catch
                {
                    Console.WriteLine("Invalid Selection");
                }
            }

            //Creates a new blog repo instance
            BlogRepository blogRepo = new BlogRepository(_connectionString);
            //Creates list of all blogs in database
            List<Blog> blogs = blogRepo.GetAll();

            //User chooses new blog for post from the presented list of all blogs. If blank, it remains unchanged
            while (true)
            {
                Console.WriteLine("New blog (blank to leave unchanged): ");
                for (int i = 0; i < blogs.Count; i++)
                {
                    Blog blog = blogs[i];
                    Console.WriteLine($" {i + 1}) {blog.Title}");
                }
                Console.Write("> ");

                string newBlog = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(newBlog))
                {
                    break;
                }

                try
                {
                    int choice = int.Parse(newBlog);
                    postToEdit.Blog = blogs[choice - 1];
                    break;
                }
                catch
                {
                    Console.WriteLine("Invalid Selection");
                }
            }
            
            if (postToEdit != null)
            {
                _postRepository.Update(postToEdit);
            }
        }

        //Receives user input on which post to delete and then runs that post id through the delete method from the post repo
        private void Remove()
        {
            //Gets list of all posts
            List<Post> posts = _postRepository.GetAll();
            //Declares postToDelete variable and sets it to null
            Post postToDelete = null;

            //Creates list of posts for user to choose from
            Console.WriteLine("Which post would you like to remove?");
            for (int i = 0; i < posts.Count; i++)
            {
                Post post = posts[i];
                Console.WriteLine($" {i + 1}) {post.Title}");
            }
            Console.Write("> ");

            //Trys to get selected post from list
            string input = Console.ReadLine();
            try
            {
                int choice = int.Parse(input);
                postToDelete = posts[choice - 1];
            }
            catch
            {
                Console.WriteLine("Invalid Selection");
            }

            //If the user selected a valid post, it's id is sent to the post repos delete method
            if (postToDelete != null)
            {
                _postRepository.Delete(postToDelete.Id);
            }
        }
    }
}
