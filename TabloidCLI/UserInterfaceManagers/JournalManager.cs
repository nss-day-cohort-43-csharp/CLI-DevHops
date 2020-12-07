﻿using System;
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
            Console.WriteLine(" 0) Go Back");

            Console.Write("> ");
            //Read User's choice/selection
            string choice = Console.ReadLine();

            switch(choice)
            {
                case "1":
                    //Show all Jornal entries
                    List(); 
                    return this;
                case "2":
                    Add();
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
            while (true)
            {
                try
                {
                    Console.WriteLine("New Journal");
                    Journal journal = new Journal();

                    Console.Write("Title: ");
                    journal.Title = Console.ReadLine();
                    if (journal.Title == "")
                    {
                        throw new Exception();
                    }

                    Console.Write("Creation date (YYYY-MM-DD): ");
                    journal.CreateDateTime = Convert.ToDateTime(Console.ReadLine());
                    if (journal.CreateDateTime == null)
                    {
                        throw new Exception();
                    }
                    Console.Write("Content: ");
                    journal.Content = Console.ReadLine();
                    if (journal.Title == ""|| journal.Content == "" || journal.CreateDateTime == null)
                    {
                        throw new Exception();
                    }
                }
                catch
                {
                    Console.WriteLine("Please enter valid information");
                }
            }

           // _journalRepository.Insert(journal);
        }
    }
}
