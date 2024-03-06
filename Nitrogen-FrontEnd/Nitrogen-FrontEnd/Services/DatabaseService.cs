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

        public List<Equipment> GetEquipmentForProject(string projectNumber)
        {
            List<Equipment> equipmentList = new List<Equipment>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string equipmentQuery = $"SELECT * From Equipment WHERE ProjectNumber = {projectNumber}";

                using (SqlCommand command = new SqlCommand(equipmentQuery, connection))
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    while ( reader.Read() )
                    {
                        Equipment equipment = new Equipment
                        {
                            ProjectNumber = reader["ProjectNumber"].ToString(),
                            Description = reader["Description"].ToString(),
                            EquipmentId = reader["EquipmentId"].ToString(),
                            ControlPanel = reader["ControlPanel"] != DBNull.Value ? reader["ControlPanel"].ToString() : null,
                            Area = reader["Area"].ToString(),
                        };

                        equipmentList.Add(equipment);
                    }

                    reader.Close();
                }
            }

            return equipmentList;
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

                string insertQuery = "INSERT INTO Equipment (" +
                            "ProjectNumber, Description, EquipmentId, ControlPanel, Area" +
                        ") VALUES (" +
                            "@ProjectNumber, @Description, @EquipmentId, @ControlPanel, @Area" +
                        ")";
                using (SqlCommand command = new SqlCommand(insertQuery, connection))
                {

                    command.Parameters.AddWithValue("@ProjectNumber", equipment.ProjectNumber);
                    command.Parameters.AddWithValue("@EquipmentId", equipment.EquipmentId);
                    command.Parameters.AddWithValue("@Area", equipment.Area);
                    if (equipment.Description != null)
                    {
                        command.Parameters.AddWithValue("@Description", equipment.Description);
                    }
                    else
                    {
                        command.Parameters.AddWithValue("@Description", DBNull.Value);
                    }
                    if (equipment.ControlPanel != null)
                    {
                        command.Parameters.AddWithValue("@ControlPanel", equipment.ControlPanel);
                    }
                    else
                    {
                        command.Parameters.AddWithValue("@ControlPanel", DBNull.Value);
                    }

                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
        }
    }
}
