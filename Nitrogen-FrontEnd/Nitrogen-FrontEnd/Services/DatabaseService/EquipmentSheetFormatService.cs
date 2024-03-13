using Nitrogen_FrontEnd.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Nitrogen_FrontEnd.Services.DatabaseService
{
    class EquipmentSheetFormatService
    {
        private readonly string connectionString;

        public EquipmentSheetFormatService(string connectionString)
        {
            this.connectionString = connectionString;
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
    }
}
