using System;
using TabloidCLI.Models;
using System.Collections.Generic;
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
            // get all the tags
            List<Tag> tags = _tagRepo.GetAll();

            Console.WriteLine("Tag Names\n");

            // print each tag
            foreach(Tag tag in tags)
            {
                Console.WriteLine($"  {tag.Name}");    
            }
            Console.WriteLine();
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
            //gobal tag 
            Tag selectedTag = null;

            //get all tags to display them
            List<Tag> tags = _tagRepo.GetAll();

            Console.WriteLine("Please choose a tag");
            for(int i = 0; i< tags.Count; i++)
            {
                Tag tag = tags[i];
                Console.WriteLine($"{i + 1 }) {tag.Name}");
            }
            Console.Write("> ");
            string input = Console.ReadLine();

            //check if tag is a vaild selection
            try
            {
                int inputToInt = int.Parse(input);
                selectedTag = tags[inputToInt - 1];
            }
            catch
            {
                Console.WriteLine("Invalid Selection");
                Execute();
            }

            //check for blank tag or less than 55 chars
            while(true)
            {
                Console.Write("New Tag (blank to leave unchanged): ");
                string name = Console.ReadLine();
                if(string.IsNullOrWhiteSpace(name))
                {
                    break;
                }
                else
                {
                    if(name.Length <= 55)
                    {
                        selectedTag.Name = name;
                        break;
                    }
                    else
                    {
                        Console.WriteLine("Name must be less than 55 characters.");
                    }
                }
            }

            //check for null tag
            if(selectedTag != null)
            {
                _tagRepo.Update(selectedTag);
            }
            
        }

        private void Remove()
        {
            throw new NotImplementedException();
        }
    }
}
