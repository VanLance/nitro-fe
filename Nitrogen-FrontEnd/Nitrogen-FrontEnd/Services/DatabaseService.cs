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
        private string connectionString;

        public DatabaseService(string connectionString)
        {
            this.connectionString = connectionString;
        }


        public Project GetProjectByProjectNumber(string projectNumber)
        {
            Project project = null;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string projectQuery = "SELECT ProjectNumber, Description FROM Project WHERE ProjectNumber = @ProjectNumber";

                using (SqlCommand command = new SqlCommand(projectQuery, connection))
                {
                    command.Parameters.AddWithValue("@ProjectNumber", projectNumber);

                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.Read())
                    {

                        project = new Project
                        {
                            ProjectNumber = reader["ProjectNumber"].ToString(),
                            Description = reader["Description"].ToString(),

                        };

                    }
                    return project;
                }
            }
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
            if (GetProjectByProjectNumber(project.ProjectNumber) == null)
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
                    while (reader.Read())
                    {
                        Equipment equipment = new Equipment
                        {
                            ProjectNumber = reader["ProjectNumber"].ToString(),
                            Description = reader["Description"].ToString(),
                            EquipmentId = reader["EquipmentId"].ToString(),
                            EquipmentSubId = reader["EquipmentSubId"].ToString(),
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

        public Equipment GetEquipmentByIdsAndProjectNumber(string id, string subId, string projectNumber)
        {
            Equipment equipment = null;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string projectQuery = "SELECT * FROM Equipment WHERE ProjectNumber = @ProjectNumber and EquipmentId = @Id and EquipmentSubId = @SubId";

                using (SqlCommand command = new SqlCommand(projectQuery, connection))
                {
                    command.Parameters.AddWithValue("@ProjectNumber", projectNumber);
                    command.Parameters.AddWithValue("@Id", id);
                    if (subId != null)
                    {
                        command.Parameters.AddWithValue("@SubId", subId);
                    }
                    else
                    {
                        command.Parameters.AddWithValue("@SubId", DBNull.Value);
                    }

                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.Read())
                    {

                        equipment = new Equipment
                        {
                            ProjectNumber = reader["ProjectNumber"].ToString(),

                        };

                    }
                    return equipment;
                }
            }
        }

        public void AddEquipment(Equipment equipment)
        {
            Equipment parentEquipment = GetEquipmentByIdsAndProjectNumber(equipment.EquipmentId, null, equipment.ProjectNumber);

            using (SqlConnection connection = new SqlConnection(connectionString))
            {

                string insertQuery = "INSERT INTO Equipment (" +
                            "ProjectNumber, Description, EquipmentId, EquipmentSubId, ParentEquipmentId, ControlPanel, Area" +
                        ") VALUES (" +
                            "@ProjectNumber, @Description, @EquipmentId, @EquipmentSubId, @ParentEquipmentId, @ControlPanel, @Area" +
                        ")";
                using (SqlCommand command = new SqlCommand(insertQuery, connection))
                {

                    command.Parameters.AddWithValue("@ProjectNumber", equipment.ProjectNumber);
                    command.Parameters.AddWithValue("@EquipmentId", equipment.EquipmentId);
                    command.Parameters.AddWithValue("@Area", equipment.Area);

                    if (equipment.EquipmentSubId != null)
                    {
                        command.Parameters.AddWithValue("@EquipmentSubId", equipment.EquipmentSubId);
                    }
                    else
                    {
                        command.Parameters.AddWithValue("@EquipmentSubId", DBNull.Value);
                    }
                    if (parentEquipment != null)
                    {
                        command.Parameters.AddWithValue("@ParentEquipmentId",parentEquipment.Id);
                    }
                    else
                    {
                        command.Parameters.AddWithValue("@ParentEquipmentId", DBNull.Value);
                    }
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
