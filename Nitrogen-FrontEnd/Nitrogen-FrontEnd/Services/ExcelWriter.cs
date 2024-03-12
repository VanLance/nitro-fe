using Microsoft.Office.Interop.Excel;
using Nitrogen_FrontEnd.Models;
using System;
using System.Collections.Generic;
using System.Windows;
using Application = Microsoft.Office.Interop.Excel.Application;

namespace Nitrogen_FrontEnd.Services
{
    class ExcelWriter
    {
        private string FilePath;
        static private DatabaseService DbService = new DatabaseService("Server=JAA-WIN10DEV-VM;Database=NitrogenDB;User Id=sa;Password=alpha;");
        private Application ExcelApp;
        private Workbook Workbook;

        public ExcelWriter(string filePath)
        {
            FilePath = filePath;
            ExcelApp = new Application();
            Workbook = ExcelApp.Workbooks.Open(FilePath);
        }

        public void WriteDataToExcelRow(Equipment equipment, EquipDbFieldToExcelColumnMap dbToExcelMap)
        {
            //try
            //{

            Worksheet worksheet = Workbook.Worksheets[equipment.Area];

            //Worksheet worksheet = null;
            //foreach (Worksheet ws in workbook.Worksheets)
            //{
            //    Console.WriteLine(ws.Name + "worksheet Name");
            //    if (ws.Name == equipment.Area)
            //    {
            //        worksheet = ws;
            //        break;
            //    }
            //}

            worksheet.Cells[equipment.ExcelRowNumber, dbToExcelMap.EquipListNumber].Value = equipment.EquipmentId + equipment.EquipmentSubId;
            worksheet.Cells[equipment.ExcelRowNumber, dbToExcelMap.Description].Value = equipment.Description;
            worksheet.Cells[equipment.ExcelRowNumber, dbToExcelMap.ControlPanel].Value = equipment.ControlPanel;
            worksheet.Cells[equipment.ExcelRowNumber, dbToExcelMap.Notes].Value = equipment.Notes;


            Workbook.Save();

            Workbook.Close();
            ExcelApp.Quit();
            ReleaseObject(worksheet);
            ReleaseObject(Workbook);
            ReleaseObject(ExcelApp);

            MessageBox.Show("Data written to Excel successfully!");
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show("Error writing to Excel: " + ex.Message);
            //}
        }

        public void WriteDataToExcelProject(int projectId)
        {
            Project project = DbService.GetProjectById(projectId);
            List<Equipment> projectEquipment = DbService.GetEquipmentForProject(project.ProjectNumber);
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
