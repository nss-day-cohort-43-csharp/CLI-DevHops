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
                    cmd.CommandText = @"SELECT Title, URL
                                        From Blog";

                    // execute reader and store the returned reader
                    SqlDataReader reader = cmd.ExecuteReader();

                    // instaniate an empty blog list to store the blogs in
                    List<Blog> blogs = new List<Blog>() { };

                    // read while there is still data
                    while(reader.Read())
                    {
                        // get the values
                        string title = reader.GetString(reader.GetOrdinal("Title"));
                        string url = reader.GetString(reader.GetOrdinal("URL"));

                        // create a new blog object
                        Blog blog = new Blog()
                        {
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

        // deletes the blog with the given id
        public void Delete(int id)
        {
            // start connection
            using (SqlConnection conn = Connection)
            {
                // open connection
                conn.Open();

                //use command
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    // creat sql command
                    cmd.CommandText = @"DELETE FROM BlogTag WHERE BlogId = @id;
                                        DELETE FROM Blog WHERE Id = @id;";
                    cmd.Parameters.AddWithValue("@id", id);

                    // excecute the sql command
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}
