﻿using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using TabloidCLI.Models;
using TabloidCLI.Repositories;
using TabloidCLI.UserInterfaceManagers;

namespace TabloidCLI
{
    public class TagRepository : DatabaseConnector, IRepository<Tag>
    {
        public TagRepository(string connectionString) : base(connectionString) { }

        public List<Tag> GetAll()
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"SELECT id, Name FROM Tag";
                    List<Tag> tags = new List<Tag>();

                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        Tag tag = new Tag()
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            Name = reader.GetString(reader.GetOrdinal("Name")),
                        };
                        tags.Add(tag);
                    }

                    reader.Close();

                    return tags;
                }
            }
        }

        public Tag Get(int id)
        {
            throw new NotImplementedException();
        }

        public void Insert(Tag tag)
        {
            // start a connection
            using (SqlConnection conn = Connection)
            {
                //open the connection
                conn.Open();

                // use a command
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    // create the sql command
                    cmd.CommandText = @"INSERT INTO Tag (Name)
                                        VALUES (@name)
                                        ";
                    cmd.Parameters.AddWithValue("@name", tag.Name);

                    // execute the insert command
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void Update(Tag tag)
        {
            //start connection
            using(SqlConnection conn = Connection)
            {
                //open connection
                conn.Open();
                using(SqlCommand cmd = conn.CreateCommand())
                {
                    //create sql command
                    cmd.CommandText = @"UPDATE Tag
                                        SET Name = @name
                                        WHERE Id = @id";

                    cmd.Parameters.AddWithValue("@name", tag.Name);
                    cmd.Parameters.AddWithValue("@Id", tag.Id);

                    // execute the insert command
                    cmd.ExecuteNonQuery();
                }
            }
        }

        //Deletes tag from database
        public void Delete(int id)
        {
            //Creates connection that closes after using
            using (SqlConnection conn = Connection)
            {
                //Opens connection
                conn.Open();
                //Creates command that closes after using
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    //Assigns text for SQL query with brought in id
                    cmd.CommandText = @"DELETE FROM PostTag WHERE TagId = @id;
                                        DELETE FROM AuthorTag WHERE TagId = @id;
                                        DELETE FROM BlogTag WHERE TagId = @id;
                                        DELETE FROM Tag WHERE Id = @id";
                    cmd.Parameters.AddWithValue("@id", id);

                    //Executes command string and returns nothing
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public SearchResults<Author> SearchAuthors(string tagName)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"SELECT a.id,
                                               a.FirstName,
                                               a.LastName,
                                               a.Bio
                                          FROM Author a
                                               LEFT JOIN AuthorTag at on a.Id = at.AuthorId
                                               LEFT JOIN Tag t on t.Id = at.TagId
                                         WHERE t.Name LIKE @name";
                    cmd.Parameters.AddWithValue("@name", $"%{tagName}%");
                    SqlDataReader reader = cmd.ExecuteReader();

                    SearchResults<Author> results = new SearchResults<Author>();
                    while (reader.Read())
                    {
                        Author author = new Author()
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                            LastName = reader.GetString(reader.GetOrdinal("LastName")),
                            Bio = reader.GetString(reader.GetOrdinal("Bio")),
                        };
                        results.Add(author);
                    }

                    reader.Close();

                    return results;
                }
            }
        }
    }
}
