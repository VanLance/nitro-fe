using Nitrogen_FrontEnd.Models;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Nitrogen_FrontEnd.Services.DatabaseService
{
    public class EquipmentService
    {
        private readonly string connectionString;

        public EquipmentService(string connectionString)
        {
            this.connectionString = connectionString;
        }

        //public List<Equipment> GetEquipmentForProject(string projectNumber)
        //{
        //    // Implementation remains the same
        //}

        //public Equipment GetSingleEquipmentById(int id)
        //{
        //    // Implementation remains the same
//        }

//    // Other equipment-related methods...
//    GetEquipmentForProject
//GetSingleEquipmentById
//    GetSingleEquipmentByIdsAndProjectNumber
//    GetEquipmentFamily
//    AddEquipment
//    EditEquipment
    }
}
