using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using TabloidCLI.Models;

namespace TabloidCLI.Repositories
{
    public class PostRepository : DatabaseConnector, IRepository<Post>
    {
        public PostRepository(string connectionString) : base(connectionString) { }

        //Fetches and returns all posts from database
        public List<Post> GetAll()
        {
            //Creates a connection that closes after using
            using (SqlConnection conn = Connection)
            {
                //Opens connection
                conn.Open();
                //Creates a command that closes after using
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    //Command given SQL string to execute
                    cmd.CommandText = @"SELECT p.Id, p.Title, p.URL, PublishDateTime, AuthorId, BlogId, FirstName, LastName, Bio, b.Title AS BlogTitle, b.URL AS BlogURL
                                        FROM Post p
                                        JOIN Author a ON a.Id = AuthorId
                                        JOIN Blog b ON b.Id = BlogId";

                    //Executes command and stores returned info in reader variable
                    SqlDataReader reader = cmd.ExecuteReader();

                    List<Post> posts = new List<Post>();

                    //Loops through all data and creates a post for each row that gets added to posts variable
                    while (reader.Read())
                    {
                        posts.Add(new Post
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            Title = reader.GetString(reader.GetOrdinal("Title")),
                            Url = reader.GetString(reader.GetOrdinal("URL")),
                            PublishDateTime = reader.GetDateTime(reader.GetOrdinal("PublishDateTime")),
                            Author = new Author
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("AuthorId")),
                                FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                                LastName = reader.GetString(reader.GetOrdinal("LastName")),
                                Bio = reader.GetString(reader.GetOrdinal("Bio"))
                            },
                            Blog = new Blog
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("BlogId")),
                                Title = reader.GetString(reader.GetOrdinal("BlogTitle")),
                                Url = reader.GetString(reader.GetOrdinal("BlogURL"))
                            }
                        });
                    }
                    reader.Close();

                    return posts;
                }
            }
        }

        public Post Get(int id)
        {
            throw new NotImplementedException();
        }

        public List<Post> GetByAuthor(int authorId)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"SELECT p.id,
                                               p.Title As PostTitle,
                                               p.URL AS PostUrl,
                                               p.PublishDateTime,
                                               p.AuthorId,
                                               p.BlogId,
                                               a.FirstName,
                                               a.LastName,
                                               a.Bio,
                                               b.Title AS BlogTitle,
                                               b.URL AS BlogUrl
                                          FROM Post p 
                                               LEFT JOIN Author a on p.AuthorId = a.Id
                                               LEFT JOIN Blog b on p.BlogId = b.Id 
                                         WHERE p.AuthorId = @authorId";
                    cmd.Parameters.AddWithValue("@authorId", authorId);
                    SqlDataReader reader = cmd.ExecuteReader();

                    List<Post> posts = new List<Post>();
                    while (reader.Read())
                    {
                        Post post = new Post()
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            Title = reader.GetString(reader.GetOrdinal("PostTitle")),
                            Url = reader.GetString(reader.GetOrdinal("PostUrl")),
                            PublishDateTime = reader.GetDateTime(reader.GetOrdinal("PublishDateTime")),
                            Author = new Author()
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("AuthorId")),
                                FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                                LastName = reader.GetString(reader.GetOrdinal("LastName")),
                                Bio = reader.GetString(reader.GetOrdinal("Bio")),
                            },
                            Blog = new Blog()
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("BlogId")),
                                Title = reader.GetString(reader.GetOrdinal("BlogTitle")),
                                Url = reader.GetString(reader.GetOrdinal("BlogUrl")),
                            }
                        };
                        posts.Add(post);
                    }

                    reader.Close();

                    return posts;
                }
            }
        }

        //Adds a post to the database
        public void Insert(Post post)
        {
            //Creates a connection that closes after using
            using (SqlConnection conn = Connection)
            {
                //Opens connection
                conn.Open();
                //Creates a command that closes after using
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    //Command given SQL string to execute with parameters that inherit from brought in post
                    cmd.CommandText = @"INSERT INTO Post (Title, URL, PublishDateTime, AuthorId, BlogId)
                                        VALUES (@title, @url, @pubDate, @aId, @bId)";
                    cmd.Parameters.AddWithValue("@title", post.Title);
                    cmd.Parameters.AddWithValue("@url", post.Url);
                    cmd.Parameters.AddWithValue("@pubDate", post.PublishDateTime);
                    cmd.Parameters.AddWithValue("@aId", post.Author.Id);
                    cmd.Parameters.AddWithValue("@bId", post.Blog.Id);

                    //Executes command and returns nothing
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void Update(Post post)
        {
            //Creates a connection that closes after using
            using (SqlConnection conn = Connection)
            {
                //Opens connection
                conn.Open();
                //Creates a command that closes after using
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    //Command given SQL string to execute with parameters that inherit from brought in post
                    cmd.CommandText = @"UPDATE Post 
                                           SET Title = @title,
                                               URL = @url,
                                               PublishDateTime = @pubDate,
                                               AuthorId = @aId,
                                               BlogId = @bId
                                         WHERE Id = @id";

                    cmd.Parameters.AddWithValue("@title", post.Title);
                    cmd.Parameters.AddWithValue("@url", post.Url);
                    cmd.Parameters.AddWithValue("@pubDate", post.PublishDateTime);
                    cmd.Parameters.AddWithValue("@aId", post.Author.Id);
                    cmd.Parameters.AddWithValue("@bId", post.Blog.Id);
                    cmd.Parameters.AddWithValue("@id", post.Id);

                    //Executes command and returns nothing
                    cmd.ExecuteNonQuery();
                }
            }
        }

        //Removes post from database
        public void Delete(int id)
        {
            //Creates a connection that closes after using
            using (SqlConnection conn = Connection)
            {
                //Opens connection
                conn.Open();
                //Creates a command that closes after using
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    //Command given SQL string to execute with parameters that inherit from brought in post id
                    cmd.CommandText = @"DELETE FROM Note WHERE PostId = @id;
                                        DELETE FROM Post WHERE Id = @id";
                    cmd.Parameters.AddWithValue("@id", id);

                    //Executes command and returns nothing
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}
