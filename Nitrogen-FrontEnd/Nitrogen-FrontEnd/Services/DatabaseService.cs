using Nitrogen_FrontEnd.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nitrogen_FrontEnd.Services
{
    public class DatabaseService
    {
        private string connectionString; // Connection string for your SQL Server database

        public DatabaseService(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public List<Project> GetAllProjects()
        {
            List<Project> projects = new List<Project>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT ProjectNumber, Description, EquipListFormatDef, IoListFormatDef FROM Project";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        Project project = new Project
                        {
                            ProjectNumber = reader["ProjectNumber"].ToString(),
                            Description = reader["Description"].ToString(),
                            EquipListFormatDef = reader["EquipListFormatDef"].ToString(),
                            IoListFormatDef = reader["IoListFormatDef"].ToString()
                        };

                        projects.Add(project);
                    }

                    reader.Close();
                }
            }

            return projects;
        }

        public void AddProject(Project project)
        {

            using (SqlConnection connection = new SqlConnection(connectionString))
            {

                string insertQuery = "INSERT INTO Project (ProjectNumber, Description) VALUES (@ProjectNumber, @Description)";
                using (SqlCommand command = new SqlCommand(insertQuery, connection))
                {
                    
                    command.Parameters.AddWithValue("@ProjectNumber", project.ProjectNumber);
                    command.Parameters.AddWithValue("@Description", project.Description);
                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
        }

        public void AddEquipment(Equipment equipment)
        {

            using (SqlConnection connection = new SqlConnection(connectionString))
            {

                string insertQuery = "INSERT INTO Equipment (ProjectNumber, Description, EquipmentId, ControlPanel) VALUES (@ProjectNumber, @Description, @ControlPanel, @EquipmentId)";
                using (SqlCommand command = new SqlCommand(insertQuery, connection))
                {

                    command.Parameters.AddWithValue("@ProjectNumber", equipment.ProjectNumber);
                    command.Parameters.AddWithValue("@Description", equipment.Description);
                    command.Parameters.AddWithValue("@EquipmentId", equipment.EquipmentId);
                    command.Parameters.AddWithValue("@ControlPanel", equipment.ControlPanel);
                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
        }
    }
}
