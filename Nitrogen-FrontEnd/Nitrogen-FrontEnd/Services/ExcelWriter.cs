using Microsoft.Office.Interop.Excel;
using Nitrogen_FrontEnd.Models;
using System;
using System.Collections.Generic;
using System.Windows;
using Application = Microsoft.Office.Interop.Excel.Application;

namespace Nitrogen_FrontEnd.Services
{
    public class ExcelWriter
    {
        private string FilePath;
        private Application ExcelApp;
        private Workbook Workbook;
        private DatabaseService DbService;

        public ExcelWriter(string filePath)
        {
            FilePath = filePath;
            ExcelApp = new Application();
            Workbook = ExcelApp.Workbooks.Open(FilePath);
            DbService = new DatabaseService("Server=JAA-WIN10DEV-VM;Database=NitrogenDB;User Id=sa;Password=alpha;");
        }

        public void CloseWorkbook()
        {
            Workbook?.Close();
            Workbook = null;
        }

        public void ReleaseResources()
        {
            ReleaseObject(Workbook);
            ReleaseObject(ExcelApp);
            DbService = null;
        }

        private void WriteDataToExcelRow(Equipment equipment, EquipDbFieldToExcelColumnMap dbToExcelMap)
        {
            try
            {
                Worksheet worksheet = Workbook.Worksheets[equipment.Area];

                worksheet.Cells[equipment.ExcelRowNumber, dbToExcelMap.EquipListNumber].Value = equipment.EquipmentId + equipment.EquipmentSubId;
                worksheet.Cells[equipment.ExcelRowNumber, dbToExcelMap.Description].Value = equipment.Description;
                worksheet.Cells[equipment.ExcelRowNumber, dbToExcelMap.ControlPanel].Value = equipment.ControlPanel;
                worksheet.Cells[equipment.ExcelRowNumber, dbToExcelMap.Notes].Value = equipment.Notes;

                Workbook.Save();

                MessageBox.Show("Data written to Excel successfully!");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error writing to Excel: " + ex.Message);
            }
        }

        public void WriteDataToSingleRow(Equipment equipment, EquipDbFieldToExcelColumnMap dbToExcelMap)
        {
            WriteDataToExcelRow(equipment, dbToExcelMap);
            CloseWorkbook();
            ReleaseResources();
        }

        public void WriteDataToExcelProject(int projectId)
        {
            Project project = DbService.GetProjectById(projectId);
            EquipSheetFormat sheetFormat = DbService.GetSheetFormatById(project.EquipSheetFormatId);
            EquipDbFieldToExcelColumnMap dbToExcelMap = DbService.GetEquipDbToExcelMapById(sheetFormat.EquipDbFieldToExcelColumnMapId);
            List<Equipment> projectEquipment = DbService.GetEquipmentForProject(project.ProjectNumber);
            foreach (Equipment equipment in projectEquipment)
            {
                WriteDataToExcelRow(equipment, dbToExcelMap);
            }
            CloseWorkbook();
            ReleaseResources();
        }

        private void ReleaseObject(object obj)
        {
            try
            {
                System.Runtime.InteropServices.Marshal.ReleaseComObject(obj);
                obj = null;
            }
            catch (Exception ex)
            {
                obj = null;
                Console.WriteLine("Exception Occurred while releasing object " + ex.ToString());
            }
            finally
            {
                GC.Collect();
            }
        }
    }
}
