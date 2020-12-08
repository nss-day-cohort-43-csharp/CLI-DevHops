using System;
using System.Collections.Generic;
using TabloidCLI.Models;

namespace TabloidCLI.UserInterfaceManagers
{
    class JournalManager : IUserInterfaceManager
    {
        // declare fields
        private readonly IUserInterfaceManager _parentUI;
        private JournalRepository _journalRepository;
        private string _connectionString;

        // initialize fields in the constructor
        public JournalManager(IUserInterfaceManager parentUI, string connectionString)
        {
            _parentUI = parentUI;
            _journalRepository = new JournalRepository(connectionString);
            _connectionString = connectionString;
        }

        // excute code when returned
        public IUserInterfaceManager Execute()
        {
            Console.WriteLine("Journal Menu");
            Console.WriteLine(" 1) List Journals");
            Console.WriteLine(" 2) Add Journal");
            Console.WriteLine(" 3) Edit Journal");
            Console.WriteLine(" 4) Remove Journal");
            Console.WriteLine(" 0) Go Back");

            Console.Write("> ");
            //Read User's choice/selection
            string choice = Console.ReadLine();

            switch(choice)
            {
                case "1":
                    //Show all Journal entries
                    List(); 
                    return this;
                case "2":
                    //Add a Journal entry 
                    Add();
                    return this;
                case "3":
                    //Edit a Journal entry
                    Edit();
                    return this;
                case "4":
                    //Remove a Journal entry
                    Remove();
                    return this;
                case "0":
                    //Return to preivous menu
                    return _parentUI;
                default:
                    Console.WriteLine("Invalid Selection");
                    return this;
            }
        }

        private void List()
        {
            List<Journal> journals = _journalRepository.GetAll();
            foreach (Journal journal in journals)
            {
                Console.WriteLine($"Title: {journal.Title}");
                Console.WriteLine($"Creation Date: {journal.CreateDateTime}");
                Console.WriteLine($"Content: {journal.Content}");
                Console.WriteLine($"");
            }
        }

        private void Add()
        {
           Console.WriteLine("New Journal");
           Journal journal = new Journal();


            while(true)
            {
                Console.Write("Title: ");
                //Check for a vaild title
                journal.Title = Console.ReadLine();
                if (journal.Title != "" && journal.Title.Length < 56)
                {
                    break;
                }
            }

            while(true)
            {

                Console.Write("Creation date (YYYY-MM-DD): ");

                //check for valid date
                try
                {
                    DateTime input = Convert.ToDateTime(Console.ReadLine());
                    DateTime limit = new DateTime(1753, 1, 1);
                    if (input.Date > limit.Date && input.Date < DateTime.Now)
                    {
                        journal.CreateDateTime = input;
                        break;
                    }
                }
                catch
                {
                    Console.WriteLine("Invalid Date");
                }
            }

            while(true)
            {
                Console.Write("Content: ");
                journal.Content = Console.ReadLine();
                //Check for a vaild content
                if (journal.Content != "" )
                {
                    break;
                }
            }

            //all validation passed add and break out of while loop
            _journalRepository.Insert(journal);
        }

        private Journal Choose(string prompt = null)
        {
            if(prompt == null)
            {
                prompt = "Please choose a Journal:";
            }

            Console.WriteLine(prompt);

            List<Journal> journals = _journalRepository.GetAll();
            //list all journal titles and selection number
            for(int i = 0; i < journals.Count; i++)
            {
                Journal journal = journals[i];
                Console.WriteLine($" {i+1}) {journal.Title}");
            }

            Console.Write("> ");
            string input = Console.ReadLine();
            //check for vaild selection
                try
                {
                    int choice = int.Parse(input);
                    return journals[choice - 1];
                }
                catch (Exception)
                {
                    Console.WriteLine("Invalid Selection");
                    return null;
                }
        }
        private void Remove()
        {
            Journal journalToDelete = Choose("Which journal would you like to remove?");
            //if there is no journal go to journal manager menu
            if(journalToDelete == null)
            {
                Execute();
            }
            else
            {
                _journalRepository.Delete(journalToDelete.Id);
            }
        }

        private void Edit()
        {
            Journal journalToEdit = Choose("Which journal would you like to edit?");
            if (journalToEdit == null)
            {
                Execute();
            }

            //Get user input for title. If blank, it remains unchanged
            Console.Write("New Title (blank to leave unchanged): ");
            while (true)
            {
                string title = Console.ReadLine();
                if(string.IsNullOrWhiteSpace(title))
                {
                    break;
                }
                else
                {
                    if (title.Length < 56)
                    {
                        journalToEdit.Title = title;
                        break;
                    }
                    else
                    {
                        Console.WriteLine("Title can not be longer than 55 characters.");
                    }
                }
            }

            //Get user input for date. If blank, it remains unchanged
            while (true)
            {
                Console.Write("New Creation Date (blank to leave unchanged): ");
                try
                {
                    string date = Console.ReadLine();
                    if (string.IsNullOrWhiteSpace(date))
                    {
                        break;
                    }

                    DateTime convertedDate = Convert.ToDateTime(date);
                    //check for valid date range
                    DateTime limit = new DateTime(1753, 1, 1);
                    if (convertedDate.Date > limit.Date && convertedDate.Date < DateTime.Now)
                    {
                        journalToEdit.CreateDateTime = convertedDate;
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

            //Get user input for content. If blank, it remains unchanged
            Console.Write("New Content (blank to leave unchanged): ");
            string content = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(content))
            {
                journalToEdit.Content = content;
            }

            if(journalToEdit != null)
            {
                //vaildation complete update post
                _journalRepository.Update(journalToEdit);
            }
        }
    }
}
