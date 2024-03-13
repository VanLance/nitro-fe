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

        //public EquipDbFieldToExcelColumnMap GetEquipDbToExcelMap(string equipListNumber)
        //{
        //    // Implementation remains the same
        //}

        //public EquipDbFieldToExcelColumnMap GetEquipDbToExcelMapById(int Id)
        //{
        //    // Implementation remains the same
        //}

        //// Other mapping-related methods...
        //GetEquipDbToExcelMap
        //GetEquipDbToExcelMapById
        //AddEquipDbFieldToExcelColumnMap
        //GetSheetFormatById
        //AddEquipSheetFormat
    }
}
