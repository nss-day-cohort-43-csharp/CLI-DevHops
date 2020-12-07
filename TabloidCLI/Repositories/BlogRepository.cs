using System;
using System.Collections.Generic;
using System.Text;
using TabloidCLI.Models;
using Microsoft.Data.SqlClient;

namespace TabloidCLI.Repositories
{
    class BlogRepository : DatabaseConnector
    {
        /// repository constructor
        public BlogRepository(string _connectionString) : base(_connectionString) { }

        /// adds a blog to the database
        public void Insert(Blog blog)
        {
            /// start a connection
            using (SqlConnection conn = Connection)
            {
                /// open the connection
                conn.Open();

                /// use command
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    // create the sql command with the added values
                    cmd.CommandText = @"INSERT INTO Blog (Title, URL)
                                        VALUES (@title, @url)";
                    cmd.Parameters.AddWithValue("@title", blog.Title);
                    cmd.Parameters.AddWithValue("@url", blog.Url);

                    // execute the command
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}
