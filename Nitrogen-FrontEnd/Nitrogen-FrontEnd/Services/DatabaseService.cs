﻿using Nitrogen_FrontEnd.Models;
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


        public Project GetProjectById(int id)
        {
            Project project = null;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string projectQuery = "SELECT * FROM Project WHERE Id = @Id";

                using (SqlCommand command = new SqlCommand(projectQuery, connection))
                {
                    command.Parameters.AddWithValue("@Id", id);

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

        public EquipDbFieldToExcelColumnMap GetEquipDbToExcelMap(string equipListNumber)
        {
            EquipDbFieldToExcelColumnMap equipDbFieldToExcelColumnMap = null;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string equipDbFieldToExcelColumnMapQuery = "SELECT * FROM EquipDbFieldToExcelColumnMap WHERE EquipListNumber = @EquipListNumber";

                using (SqlCommand command = new SqlCommand(equipDbFieldToExcelColumnMapQuery, connection))
                {
                    command.Parameters.AddWithValue("@EquipListNumber", equipListNumber);

                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.Read())
                    {

                        equipDbFieldToExcelColumnMap = new EquipDbFieldToExcelColumnMap
                        {
                            EquipListNumber = (int)reader["EquipListNumber"],
                        };

                    }
                    return equipDbFieldToExcelColumnMap;
                }
            }
        }

        public EquipDbFieldToExcelColumnMap GetEquipDbToExcelMapById(int Id)
        {
            EquipDbFieldToExcelColumnMap equipMap = null;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string equipmentQuery = "Select * From EquipDbFieldToExcelColumnMap WHERE Id = @Id";

                using (SqlCommand command = new SqlCommand(equipmentQuery, connection))
                {
                    command.Parameters.AddWithValue("@Id", Id);

                    connection.Open();

                    SqlDataReader reader = command.ExecuteReader();
                    if (reader.Read())
                    {

                        equipMap = new EquipDbFieldToExcelColumnMap
                        {
                            Id = (int)reader["Id"],
                            EquipListNumber = (int)reader["EquipListNumber"],
                            Description = (int)reader["Description"],
                            ControlPanel = (int)reader["ControlPanel"],
                            Notes = (int)reader["Notes"],
                        };

                    }
                    return equipMap;
                }
            }
        }

        public int AddEquipDbFieldToExcelColumnMap(EquipDbFieldToExcelColumnMap dbToExcelMap)
        {

            using (SqlConnection connection = new SqlConnection(connectionString))
            {

                string insertQuery = "INSERT INTO EquipDbFieldToExcelColumnMap (EquipListNumber, Description, ControlPanel, Notes) OUTPUT INSERTED.Id VALUES (@EquipListNumber, @Description, @ControlPanel, @Notes)";
                using (SqlCommand command = new SqlCommand(insertQuery, connection))
                {
                    command.Parameters.AddWithValue("@EquipListNumber", dbToExcelMap.EquipListNumber);
                    command.Parameters.AddWithValue("@Description", dbToExcelMap.Description);
                    command.Parameters.AddWithValue("@ControlPanel", dbToExcelMap.ControlPanel);
                    command.Parameters.AddWithValue("@Notes", dbToExcelMap.Notes);
                    connection.Open();
                    return (int)command.ExecuteScalar();
                }
            }
        }

        public EquipSheetFormat GetSheetFormatById(int Id)
        {
            EquipSheetFormat sheetFormat = null;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string equipmentQuery = "Select * From EquipSheetFormat WHERE Id = @Id";

                using (SqlCommand command = new SqlCommand(equipmentQuery, connection))
                {
                    command.Parameters.AddWithValue("@Id", Id);

                    connection.Open();

                    SqlDataReader reader = command.ExecuteReader();
                    if (reader.Read())
                    {

                        sheetFormat = new EquipSheetFormat
                        {
                            Id = (int)reader["Id"],
                            FileName = reader["FileName"].ToString(),
                            StartingDataLine = (int)reader["StartingDataLine"],
                            EquipDbFieldToExcelColumnMapId = (int)reader["EquipDbFieldToExcelColumnMapId"],

                        };

                    }
                    return sheetFormat;
                }
            }
        }

        public int AddEquipSheetFormat(EquipSheetFormat sheetFormat)
        {

            using (SqlConnection connection = new SqlConnection(connectionString))
            {

                string insertQuery = "INSERT INTO EquipSheetFormat (FileName, StartingDataLine, EquipDbFieldToExcelColumnMapId ) OUTPUT INSERTED.Id VALUES (@FileName, @StartingDataLine, @EquipDbFieldToExcelColumnMapId)";
                using (SqlCommand command = new SqlCommand(insertQuery, connection))
                {

                    command.Parameters.AddWithValue("@FileName", sheetFormat.FileName);
                    command.Parameters.AddWithValue("@StartingDataLine", sheetFormat.StartingDataLine);
                    command.Parameters.AddWithValue("@EquipDbFieldToExcelColumnMapId", sheetFormat.EquipDbFieldToExcelColumnMapId);

                    connection.Open();
                    return (int)command.ExecuteScalar();
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
                            Id = (int)reader["id"],
                            ProjectNumber = reader["ProjectNumber"].ToString(),
                            Description = reader["Description"].ToString(),
                            EquipmentId = reader["EquipmentId"].ToString(),
                            EquipmentSubId = reader["EquipmentSubId"].ToString(),
                            ControlPanel = reader["ControlPanel"] != DBNull.Value ? reader["ControlPanel"].ToString() : null,
                            Area = reader["Area"].ToString(),
                            Notes = reader["Notes"].ToString(),
                            ParentEquipmentId = reader["ParentEquipmentId"] != DBNull.Value ? (int)reader["ParentEquipmentId"] : (int?)null,
                            ExcelRowNumber = (int)reader["ExcelRowNumber"]
                        };

                        equipmentList.Add(equipment);
                    }

                    reader.Close();
                }
            }

            return equipmentList;
        }

        public Equipment GetSingleEquipmentById(int id)
        {
            Equipment equipment = null;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string equipmentQuery = "Select * From Equipment WHERE Id = @Id";

                using (SqlCommand command = new SqlCommand(equipmentQuery, connection))
                {
                    command.Parameters.AddWithValue("@Id", id);

                    connection.Open();

                    SqlDataReader reader = command.ExecuteReader();
                    if (reader.Read())
                    {

                        equipment = new Equipment
                        {
                            Id = (int)reader["Id"],
                            ProjectNumber = reader["ProjectNumber"].ToString(),
                            EquipmentId = reader["EquipmentId"].ToString(),
                            EquipmentSubId = reader["EquipmentSubId"].ToString(),
                            Area = reader["Area"].ToString(),
                            Description = reader["Description"].ToString(),
                            ControlPanel = reader["ControlPanel"].ToString(),
                            Notes = reader["Notes"].ToString(),
                            ExcelRowNumber = (int)reader["ExcelRowNumber"]
                        };

                    }
                    return equipment;
                }
            }
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
                            Id = (int)reader["Id"],
                            ProjectNumber = reader["ProjectNumber"].ToString(),
                            EquipmentId = reader["EquipmentId"].ToString(),
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
                            Id = (int)reader["id"],
                            ProjectNumber = reader["ProjectNumber"].ToString(),
                            Description = reader["Description"].ToString(),
                            EquipmentId = reader["EquipmentId"].ToString(),
                            EquipmentSubId = reader["EquipmentSubId"].ToString(),
                            ControlPanel = reader["ControlPanel"] != DBNull.Value ? reader["ControlPanel"].ToString() : null,
                            Area = reader["Area"].ToString(),
                            Notes = reader["Notes"].ToString(),
                            ExcelRowNumber = (int)reader["ExcelRowNumber"]
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
            //if(GetSingleEquipmentByIdsAndProjectNumber())
            Equipment parentEquipment = GetSingleEquipmentByIdsAndProjectNumber(equipment.EquipmentId, null, equipment.ProjectNumber);

            using (SqlConnection connection = new SqlConnection(connectionString))
            {

                string insertQuery = "INSERT INTO Equipment (" +
                            "ProjectNumber, Description, EquipmentId, EquipmentSubId, ParentEquipmentId, ControlPanel, Area, Notes, ExcelRowNumber" +
                        ") VALUES (" +
                            "@ProjectNumber, @Description, @EquipmentId, @EquipmentSubId, @ParentEquipmentId, @ControlPanel, @Area, @Notes, @ExcelRowNumber" +
                        ")";
                using (SqlCommand command = new SqlCommand(insertQuery, connection))
                {

                    command.Parameters.AddWithValue("@ProjectNumber", equipment.ProjectNumber);
                    command.Parameters.AddWithValue("@Description", (object)equipment.Description ?? DBNull.Value);
                    command.Parameters.AddWithValue("@EquipmentId", equipment.EquipmentId);
                    command.Parameters.AddWithValue("@Area", equipment.Area);
                    command.Parameters.AddWithValue("@EquipmentSubId", (object)equipment.EquipmentSubId ?? DBNull.Value);
                    command.Parameters.AddWithValue("@Notes", (object)equipment.Notes ?? DBNull.Value);
                    command.Parameters.AddWithValue("@ParentEquipmentId", parentEquipment?.Id != null ? (object)parentEquipment.Id : DBNull.Value);
                    command.Parameters.AddWithValue("@ControlPanel", (object)equipment.ControlPanel ?? DBNull.Value);
                    command.Parameters.AddWithValue("@ExcelRowNumber", equipment.ExcelRowNumber);

                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
        }

        public void EditEquipment(Equipment equipment)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {

                string insertQuery = "Update Equipment Set ProjectNumber = @ProjectNumber, Description = @Description, EquipmentId = @EquipmentId, EquipmentSubId = @EquipmentSubId , ParentEquipmentId = @ParentEquipmentId, ControlPanel = @ControlPanel, Area = @Area, Notes= @Notes WHERE Id = @Id;";

                using (SqlCommand command = new SqlCommand(insertQuery, connection))
                {

                    command.Parameters.AddWithValue("@ProjectNumber", equipment.ProjectNumber);
                    command.Parameters.AddWithValue("@Description", (object)equipment.Description ?? DBNull.Value);
                    command.Parameters.AddWithValue("@EquipmentId", equipment.EquipmentId);
                    command.Parameters.AddWithValue("@Area", equipment.Area);
                    command.Parameters.AddWithValue("@EquipmentSubId", (object)equipment.EquipmentSubId ?? DBNull.Value);
                    command.Parameters.AddWithValue("@Notes", (object)equipment.Notes ?? DBNull.Value);
                    command.Parameters.AddWithValue("@ParentEquipmentId", (object)equipment.ParentEquipmentId ?? DBNull.Value);
                    command.Parameters.AddWithValue("@ControlPanel", (object)equipment.ControlPanel ?? DBNull.Value);
                    command.Parameters.AddWithValue("@Id", equipment.Id);

                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
        }
    }
}
