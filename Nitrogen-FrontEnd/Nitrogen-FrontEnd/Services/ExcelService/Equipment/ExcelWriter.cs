using Microsoft.Office.Interop.Excel;
using Nitrogen_FrontEnd.Models;
using Nitrogen_FrontEnd.Services.DatabaseService;
using Nitrogen_FrontEnd.Utilities;
using System;
using System.Collections.Generic;
using System.Windows;
using Application = Microsoft.Office.Interop.Excel.Application;

namespace Nitrogen_FrontEnd.Services
{
    public class ExcelWriter : IDisposable
    {
        private string FilePath;
        private Application ExcelApp;
        private Workbook Workbook;
        private readonly ProjectService projectService;
        private readonly EquipmentService equipmentService;
        private readonly EquipmentSheetFormatService sheetFormatService;
        private readonly MappingService mappingService;
        private bool disposed = false;

        public ExcelWriter(string filePath)
        {
            FilePath = filePath;
            ExcelApp = new Application();
            Workbook = ExcelApp.Workbooks.Open(FilePath);
            projectService = new ProjectService(SqlConnectionString.connectionString);
            equipmentService = new EquipmentService(SqlConnectionString.connectionString);
            sheetFormatService = new EquipmentSheetFormatService(SqlConnectionString.connectionString);
            mappingService = new MappingService(SqlConnectionString.connectionString);
        }

        public void CloseWorkbook()
        {
            Workbook?.Close();
        }

        public void ReleaseResources()
        {
            ReleaseObjects(Workbook, ExcelApp);
        }

        private void ReleaseObjects(params object[] objs)
        {
            foreach (var obj in objs)
            {
                try
                {
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(obj);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Exception occurred while releasing object: " + ex.Message);
                }
            }

            GC.Collect();
            GC.WaitForPendingFinalizers();
        }

        private void WriteDataToExcelRow(Equipment equipment, EquipDbFieldToExcelColumnMap dbToExcelMap)
        {
            try
            {
                Worksheet worksheet = Workbook.Worksheets[equipment.Area];

                worksheet.Cells[equipment.ExcelRowNumber, dbToExcelMap.EquipListNumber].Value = $"{equipment.EquipmentId}.{equipment.EquipmentSubId}";
                worksheet.Cells[equipment.ExcelRowNumber, dbToExcelMap.Description].Value = equipment.Description;
                worksheet.Cells[equipment.ExcelRowNumber, dbToExcelMap.ControlPanel].Value = equipment.ControlPanel;
                worksheet.Cells[equipment.ExcelRowNumber, dbToExcelMap.Notes].Value = equipment.Notes;

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error writing to Excel: " + ex.Message);
            }
        }

        public void WriteDataToSingleRow(Equipment equipment, EquipDbFieldToExcelColumnMap dbToExcelMap)
        {
            try
            {
                WriteDataToExcelRow(equipment, dbToExcelMap);
                Workbook.Save();
                MessageBox.Show("Data written to Excel successfully!");
            }
            finally
            {
                CloseWorkbook();
                ReleaseResources();
            }
        }

        public void WriteDataToExcelProject(string projectId)
        {
            Project project = projectService.GetProjectByProjectNumber(projectId);
            EquipSheetFormat sheetFormat = sheetFormatService.GetSheetFormatById(project.EquipSheetFormatId);
            EquipDbFieldToExcelColumnMap dbToExcelMap = mappingService.GetEquipDbToExcelMapById(sheetFormat.EquipDbFieldToExcelColumnMapId);
            List<Equipment> projectEquipment = equipmentService.GetEquipmentForProject(project.ProjectNumber);

            try
            {
                foreach (Equipment equipment in projectEquipment)
                {
                    WriteDataToExcelRow(equipment, dbToExcelMap);
                }

                Workbook.Save();
                MessageBox.Show("Data written to Excel successfully!");
            }
            finally
            {
                CloseWorkbook();
                ReleaseResources();
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    CloseWorkbook();
                    ReleaseResources();
                }

                disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~ExcelWriter()
        {
            Dispose(false);
        }
    }
}
