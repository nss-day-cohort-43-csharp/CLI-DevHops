using System;
using System.Collections.Generic;
using System.Text;

namespace TabloidCLI.UserInterfaceManagers
{
    public class BackgroundManager : IUserInterfaceManager
    {
        private readonly IUserInterfaceManager _parentUI;
        public BackgroundManager(IUserInterfaceManager parentUI)
        {
            _parentUI = parentUI;
        }
        public IUserInterfaceManager Execute()
        {
            Console.WriteLine("Background Color Choices");
            Console.WriteLine(" 1) Blue");
            Console.WriteLine(" 2) Green");
            Console.WriteLine(" 3) White");
            Console.WriteLine(" 0) Exit");

            Console.Write("> ");
            string choice = Console.ReadLine();
            switch (choice)
            {
                case "1":
                    SetColorWithWhiteText(ConsoleColor.Blue);
                    return this;
                case "2":
                    SetColorWithBlackText(ConsoleColor.Green);
                    return this;
                case "3":
                    SetColorWithBlackText(ConsoleColor.White);
                    return this;
                case "0":
                    return _parentUI;
                default:
                    Console.WriteLine("Invalid Selection");
                    return this;

            }
        }
        private void SetColorWithWhiteText(ConsoleColor color)
        {
            Console.BackgroundColor = color;
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.White;
        }
        private void SetColorWithBlackText(ConsoleColor color)
        {
            Console.BackgroundColor = color;
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Black;
        }
        
    }
}
