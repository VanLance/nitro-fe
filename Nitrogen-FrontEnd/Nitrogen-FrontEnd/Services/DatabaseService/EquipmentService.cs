using Nitrogen_FrontEnd.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Windows;

namespace Nitrogen_FrontEnd.Services.DatabaseService
{
    public class EquipmentService
    {
        private readonly string connectionString;

        public EquipmentService(string connectionString)
        {
            this.connectionString = connectionString;
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

        public List<Equipment> GetEquipmentForArea(string projectNumber, string area)
        {
            Console.WriteLine($"area: {area}");
            List<Equipment> equipmentList = new List<Equipment>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string equipmentQuery = $"SELECT * From Equipment WHERE ProjectNumber = @ProjectNumber AND Area = @Area";

                using (SqlCommand command = new SqlCommand(equipmentQuery, connection))
                {
                    command.Parameters.AddWithValue("@ProjectNumber", projectNumber);
                    command.Parameters.AddWithValue("@Area", area);

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
            try
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
            catch (Exception e)
            {
                MessageBox.Show("Error: " + e.ToString());
            }
        }

    }
}
