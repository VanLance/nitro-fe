using DocumentFormat.OpenXml.Wordprocessing;
using Nitrogen_FrontEnd.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Nitrogen_FrontEnd.Services.DatabaseService
{
    public class ProjectService
    {
        private readonly string connectionString;

        public ProjectService(string connectionString)
        {
            this.connectionString = connectionString;
        }


        public Project GetProjectByProjectNumber(string projectNumber)
        {
            Project project = null;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string projectQuery = "SELECT * FROM Project WHERE ProjectNumber = @ProjectNumber";

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
                            EquipSheetFormatId = (int)reader["EquipSheetFormatId"],
                            IoSheetFormatId = reader["IoSheetFormatID"] != DBNull.Value ? (int)reader["IoSheetFormatId"] : (int?)null,
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
                string query = "SELECT ProjectNumber, Description, EquipSheetFormatId, IoSheetFormatId FROM Project";

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
                            EquipSheetFormatId = (int)reader["EquipSheetFormatId"],
                            IoSheetFormatId = reader["IoSheetFormatID"] != DBNull.Value ? (int)reader["IoSheetFormatId"] : (int?)null,
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
            if (GetProjectByProjectNumber(project.ProjectNumber) != null) return;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {

                string insertQuery = "INSERT INTO Project (ProjectNumber, Description, EquipSheetFormatId) VALUES (@ProjectNumber, @Description, @EquipSheetFormatId)";
                using (SqlCommand command = new SqlCommand(insertQuery, connection))
                {

                    command.Parameters.AddWithValue("@ProjectNumber", project.ProjectNumber);
                    command.Parameters.AddWithValue("@Description", project.Description);
                    command.Parameters.AddWithValue("@EquipSheetFormatId", project.EquipSheetFormatId);
                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }

        }

        public void EditProject(Project project)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {

                string insertQuery = "Update Project SET Description = @Description WHERE ProjectNumber = @ProjectNumber";
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

}

