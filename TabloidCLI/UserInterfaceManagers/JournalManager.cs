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
            journals.ForEach(j => Console.WriteLine($"{j.Title}"));
            }
    }
}