using System;
using TabloidCLI.Models;
using TabloidCLI.Repositories;

namespace TabloidCLI.UserInterfaceManagers
{
    public class TagManager : IUserInterfaceManager
    {
        private readonly IUserInterfaceManager _parentUI;
        private TagRepository _tagRepo;

        public TagManager(IUserInterfaceManager parentUI, string connectionString)
        {
            _parentUI = parentUI;
            _tagRepo = new TagRepository(connectionString);
        }

        public IUserInterfaceManager Execute()
        {
            Console.WriteLine("Tag Menu");
            Console.WriteLine(" 1) List Tags");
            Console.WriteLine(" 2) Add Tag");
            Console.WriteLine(" 3) Edit Tag");
            Console.WriteLine(" 4) Remove Tag");
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

        private void List()
        {
            throw new NotImplementedException();
        }

        private void Add()
        {
            Console.WriteLine("New Tag");

            // declare a new tag
            Tag tag = new Tag();

            // loop until a valid name is given
            string name = "";
            while (true)
            {
                //prompt for the name
                Console.Write("Name: ");
                name = Console.ReadLine();

                // if conditions are met, insert the new tag
                if(name.Length <= 55 && !string.IsNullOrWhiteSpace(name))
                {
                    tag.Name = name;
                    _tagRepo.Insert(tag);
                    break;
                }

                // if the name is too long
                if(name.Length > 55)
                {
                    Console.WriteLine("Name must be under 55 characters");
                    continue;
                }

                // name must have been blank
                Console.WriteLine("Name cannot be blank");
                

            }

        }

        private void Edit()
        {
            throw new NotImplementedException();
        }

        private void Remove()
        {
            throw new NotImplementedException();
        }
    }
}
