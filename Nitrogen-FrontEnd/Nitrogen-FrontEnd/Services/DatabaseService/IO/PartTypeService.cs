using Nitrogen_FrontEnd.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Windows;
namespace Nitrogen_FrontEnd.Services.DatabaseService.IO
{
    class PartTypeService
    {
        private readonly string connectionString;

        public PartTypeService(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public PartType GetPartTypeById(string id)
        {
            PartType partType = null;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string equipmentQuery = "Select * From PartType WHERE Id = @Id";

                using (SqlCommand command = new SqlCommand(equipmentQuery, connection))
                {
                    command.Parameters.AddWithValue("@Id", id);

                    connection.Open();

                    SqlDataReader reader = command.ExecuteReader();
                    if (reader.Read())
                    {

                        partType = new PartType
                        {
                            Id = (string)reader["Id"],
                            Description = reader["Description"].ToString()
                        };

                    }
                    return partType;
                }
            }
        }

        public void AddPartType(PartType partType)
        {
            PartType checkPartType = GetPartTypeById(partType.Id);
            if ( checkPartType != null) return ;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {

                string insertQuery = "INSERT INTO PartType ( Id, Description ) VALUES ( @Id, @Description )";
                using (SqlCommand command = new SqlCommand(insertQuery, connection))
                {

                    command.Parameters.AddWithValue("@Id", partType.Id);
                    command.Parameters.AddWithValue("@Description", (object)partType.Description ?? DBNull.Value);
                    
                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
        }

    }
}

