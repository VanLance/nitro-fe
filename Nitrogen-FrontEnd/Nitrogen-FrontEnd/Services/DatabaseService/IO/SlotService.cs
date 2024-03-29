using Nitrogen_FrontEnd.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nitrogen_FrontEnd.Services.DatabaseService.IO
{
    class SlotService
    {
        private readonly string connectionString;

        public SlotService(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public int AddSlot(Slot slot)
        {

            using (SqlConnection connection = new SqlConnection(connectionString))
            {

                string insertQuery = "INSERT INTO Slot ( Number, Bank, TypeId) OUTPUT INSERTED.Id VALUES ( @Number, @Bank, @TypeId)";
                using (SqlCommand command = new SqlCommand(insertQuery, connection))
                {
                    command.Parameters.AddWithValue("@Number", slot.Number);
                    command.Parameters.AddWithValue("@Bank", slot.Bank);
                    command.Parameters.AddWithValue("@TypeId", slot.TypeId);

                    connection.Open();
                    return (int)command.ExecuteScalar();
                }
            }
        }
    }
}
