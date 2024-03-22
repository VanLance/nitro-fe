using Nitrogen_FrontEnd.Models;
using Nitrogen_FrontEnd.Services;
using Nitrogen_FrontEnd.Services.DatabaseService;
using System;

namespace Nitrogen_FrontEnd.Utilities
{
    public static class ExcelWriterGenerator
    {
        private static readonly ProjectService projectService = new ProjectService(SqlConnectionString.connectionString); // Instantiate the service if it's not injected
        private static readonly EquipmentSheetFormatService sheetFormatService = new EquipmentSheetFormatService(SqlConnectionString.connectionString); // Instantiate the service if it's not injected

        public static ExcelWriter ExcelWriter(string projectNumber)
        {
            Console.WriteLine(projectNumber + "        ============== projectNumber from excelWriter");
            Project project = projectService.GetProjectByProjectNumber(projectNumber);
            EquipSheetFormat sheetFormat = sheetFormatService.GetSheetFormatById(project.EquipSheetFormatId);
            return new ExcelWriter(sheetFormat.FileName);
        }
    }
}
