using Nitrogen_FrontEnd.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nitrogen_FrontEnd.Services.DatabaseService.IO
{
    class IoService
    {

        private readonly string connectionString;

        public IoService(string connectionString)
        {
            this.connectionString = connectionString;
        }


        public void AddIo(IoModel io)
        {
            Console.WriteLine($"io desc: {io.Description}\nEquipId: {io.EquipmentId}\n");
            using (SqlConnection connection = new SqlConnection(connectionString))
            {

                string insertQuery = "INSERT INTO IO ( Description, EquipmentId, SlotId, PlcPoint, IoSheetsId ) " +
                                           " VALUES (" +
                            "@Description,  @EquipmentId, @SlotId, @PlcPoint, @IoSheetsId)";
                using (SqlCommand command = new SqlCommand(insertQuery, connection))
                {

                    command.Parameters.AddWithValue("@IoSheetsId", io.IoSheetsId);
                    command.Parameters.AddWithValue("@Description", io.Description);
                    command.Parameters.AddWithValue("@SlotId", io.SlotId == 0 ? DBNull.Value : (object)io.SlotId);
                    command.Parameters.AddWithValue("@EquipmentId", io.EquipmentId == 0 ? DBNull.Value : (object)io.EquipmentId);
                    command.Parameters.AddWithValue("@PlcPoint", (Object)io.PlcPoint ?? DBNull.Value);

                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
        }
    }
}
