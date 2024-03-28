using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Nitrogen_FrontEnd.Services.ExcelService.IO;

namespace Nitrogen_FrontEnd.Services.DatabaseService.IO
{
    class IoLayoutFormatService
    {
        private readonly string connectionString;

        public IoLayoutFormatService(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public int AddRackDbFieldToExcelMap(Dictionary<string, int> dbToExcelMap)
        {

            using (SqlConnection connection = new SqlConnection(connectionString))
            {

                string insertQuery = "INSERT INTO RackDbFieldToExcelColumnMap ( Slot, Type, Description ) OUTPUT INSERTED.Id VALUES ( @Slot, @Type, @Description )";
                using (SqlCommand command = new SqlCommand(insertQuery, connection))
                {
                    
                    command.Parameters.AddWithValue("@Description", dbToExcelMap["description"]);
                    command.Parameters.AddWithValue("@Slot", dbToExcelMap["slot"]);
                    command.Parameters.AddWithValue("@Type", dbToExcelMap["type"]);

                    connection.Open(); 
                    return (int)command.ExecuteScalar();
                }
            }
        }

        public int AddIoDbFieldToExcelMap(Dictionary<string, int> dbToExcelMap)
        {

            using (SqlConnection connection = new SqlConnection(connectionString))
            {

                string insertQuery = "INSERT INTO IoDbFieldToExcelColumnMap ( Slot, PlcPoint, Description, EquipmentId ) OUTPUT INSERTED.Id VALUES ( @Slot, @PlcPoint, @Description, @EquipmentId)";
                using (SqlCommand command = new SqlCommand(insertQuery, connection))
                {

                    command.Parameters.AddWithValue("@Description", dbToExcelMap["description"]);
                    command.Parameters.AddWithValue("@Slot", dbToExcelMap["slot"]);
                    command.Parameters.AddWithValue("@PlcPoint", dbToExcelMap["plc point"]);
                    command.Parameters.AddWithValue("@EquipmentId", dbToExcelMap["equipmentId"]);

                    connection.Open();
                    return (int)command.ExecuteScalar();
                }
            }
        }

        public int AddIoSheetFormat(string filePath, int ioMapId, int rackMapId)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {

                string insertQuery = "INSERT INTO IoSheetFormat ( FileName, IoDbFieldToExcelColumnMapId, RackDbFieldToExcelColumnMapId ) OUTPUT INSERTED.Id VALUES ( @FileName, @IoDbFieldToExcelColumnMapId, @RackDbFieldToExcelColumnMapId)";
                using (SqlCommand command = new SqlCommand(insertQuery, connection))
                {

                    command.Parameters.AddWithValue("@FileName", filePath);
                    command.Parameters.AddWithValue("@IoIoDbFieldToExcelColumnMapId", ioMapId);
                    command.Parameters.AddWithValue("@RackDbFieldToExcelColumnMapId", rackMapId);

                    connection.Open();
                    return (int)command.ExecuteScalar();
                }
            }
        }

        internal int AddIoSheets(int startingIoRow, IoSheets ioSheets, int ioSheetFormatId)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string insertQuery = "INSERT INTO IoSheetFormat ( Name, StartingIODataLine, StartingRackDataLine, IoSheetFormatId ) OUTPUT INSERTED.Id VALUES ( @Name, @StartingIODataLine, @StartingRackDataLine, @IoSheetFormatId)";
                using (SqlCommand command = new SqlCommand(insertQuery, connection))
                {
                    command.Parameters.AddWithValue("@Name", ioSheets.IoList.Name);
                    command.Parameters.AddWithValue("@StartingIODataLine", startingIoRow);
                    command.Parameters.AddWithValue("@StartingRackDataLine", ioSheets.RackDataStartingLine);
                    command.Parameters.AddWithValue("@StartingRackDataLine", ioSheets.RackDataStartingLine);
                    command.Parameters.AddWithValue("@IoSheetFormatId", ioSheetFormatId);

                    connection.Open();
                    return (int)command.ExecuteScalar();
                }
            }
        }
    }
}
