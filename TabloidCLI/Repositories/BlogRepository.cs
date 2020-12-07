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

        // returns a list of all the blogs in the database
        public List<Blog> GetAll()
        {
            // start a connection
            using (SqlConnection conn = Connection)
            {
                // open the connection
                conn.Open();

                // use a command
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    // create the sql command
                    cmd.CommandText = @"SELECT Id, Title, URL
                                        From Blog";

                    // execute reader and store the returned reader
                    SqlDataReader reader = cmd.ExecuteReader();

                    // instaniate an empty blog list to store the blogs in
                    List<Blog> blogs = new List<Blog>() { };

                    // read while there is still data
                    while(reader.Read())
                    {
                        // get the values
                        int id = reader.GetInt32(reader.GetOrdinal("Id"));
                        string title = reader.GetString(reader.GetOrdinal("Title"));
                        string url = reader.GetString(reader.GetOrdinal("URL"));

                        // create a new blog object
                        Blog blog = new Blog()
                        {
                            Id = id,
                            Title = title,
                            Url = url
                        };

                        // add the new blog to the list of blogs
                        blogs.Add(blog);
                    }

                    //close the reader
                    reader.Close();

                    // return the list of blogs
                    return blogs;
                }
            }
        }

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
