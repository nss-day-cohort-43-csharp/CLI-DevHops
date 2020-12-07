using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using TabloidCLI.Models;
using TabloidCLI.Repositories;

namespace TabloidCLI
{
    class JournalRepository : DatabaseConnector
    {
        public JournalRepository(string connectionString) : base(connectionString) { }

        public List<Journal> GetAll()
        {
            using(SqlConnection conn = Connection)
            {
                conn.Open();
                {
                    using(SqlCommand cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = @"SELECT Id, Title, Content, CreateDateTime
                                            FROM Journal";
                        List<Journal> journals = new List<Journal>();

                        SqlDataReader reader = cmd.ExecuteReader();
                        
                        while(reader.Read())
                        {
                            Journal journal = new Journal
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("id")),
                                Title = reader.GetString(reader.GetOrdinal("Title")),
                                Content = reader.GetString(reader.GetOrdinal("Content")),
                                CreateDateTime = reader.GetDateTime(reader.GetOrdinal("CreateDateTime"))
                            };
                            journals.Add(journal);
                        }
                        reader.Close();
                        return journals;
                    }
                }
            }
        }

        public void Insert (Journal journal)
        {
            using (SqlConnection)
        }
    }
}
