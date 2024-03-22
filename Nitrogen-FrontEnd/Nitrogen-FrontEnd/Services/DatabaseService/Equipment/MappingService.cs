using Nitrogen_FrontEnd.Models;
using System.Data.SqlClient;

namespace Nitrogen_FrontEnd.Services.DatabaseService
{
    public class MappingService
    {
        private readonly string connectionString;

        public MappingService(string connectionString)
        {
            this.connectionString = connectionString;
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

        //// Other mapping-related methods...
        //GetEquipDbToExcelMap
        //GetEquipDbToExcelMapById
        //AddEquipDbFieldToExcelColumnMap
        //GetSheetFormatById
        //AddEquipSheetFormat
    }
}
