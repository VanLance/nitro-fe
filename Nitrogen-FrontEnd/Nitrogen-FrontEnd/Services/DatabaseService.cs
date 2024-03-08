using Nitrogen_FrontEnd.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

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
                string equipmentQuery = $"SELECT * From Equipment WHERE ProjectNumber = @ProjectNumber";

                using (SqlCommand command = new SqlCommand(equipmentQuery, connection))
                {
                    command.Parameters.AddWithValue("@ProjectNumber", projectNumber);

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

        public Equipment GetSingleEquipmentByIdsAndProjectNumber(string id, string subId, string projectNumber)
        {
            Equipment equipment = null;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string equipmentQuery = "SELECT * FROM Equipment WHERE ProjectNumber = @ProjectNumber AND EquipmentId = @Id AND (EquipmentSubId = @SubId OR (EquipmentSubId IS NULL AND @SubId IS NULL))";

                using (SqlCommand command = new SqlCommand(equipmentQuery, connection))
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

        public List<Equipment> GetEquipmentFamily(string equipmentId, string projectNumber)
        {
            List<Equipment> equipmentList = new List<Equipment>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string equipmentQuery = $"SELECT * From Equipment WHERE ProjectNumber = @ProjectNumber and EquipmentId = @EquipmentId";

                using (SqlCommand command = new SqlCommand(equipmentQuery, connection))
                {
                    command.Parameters.AddWithValue("@ProjectNumber", projectNumber);
                    command.Parameters.AddWithValue("@EquipmentId", equipmentId);

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

        public void AddEquipment(Equipment equipment)
        {
            Equipment parentEquipment = GetSingleEquipmentByIdsAndProjectNumber(equipment.EquipmentId, null, equipment.ProjectNumber);

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
                    command.Parameters.AddWithValue("@Description", (object)equipment.Description ?? DBNull.Value);
                    command.Parameters.AddWithValue("@EquipmentId", equipment.EquipmentId);
                    command.Parameters.AddWithValue("@Area", equipment.Area);
                    command.Parameters.AddWithValue("@EquipmentSubId", (object)equipment.EquipmentSubId ?? DBNull.Value);
                    command.Parameters.AddWithValue("@ParentEquipmentId", parentEquipment?.Id != null ? (object)parentEquipment.Id : DBNull.Value);
                    command.Parameters.AddWithValue("@ControlPanel", (object)equipment.ControlPanel ?? DBNull.Value);

                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
        }
    }
}
