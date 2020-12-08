using System;
using System.Collections.Generic;
using System.Text;
using TabloidCLI.Repositories;
using TabloidCLI.Models;

namespace TabloidCLI.UserInterfaceManagers
{
    class BlogDetailManager : IUserInterfaceManager
    {
        // declare fields
        private IUserInterfaceManager _parentUI;
        private BlogRepository _blogRepo;


        public BlogDetailManager(IUserInterfaceManager parentUI, string connectionString)
        {
            _parentUI = parentUI;
            _blogRepo = new BlogRepository(connectionString);
        }

        public IUserInterfaceManager Execute()
        {

        }

    }
}
