using Microsoft.Office.Interop.Excel;
using Nitrogen_FrontEnd.Models;
using Nitrogen_FrontEnd.Services;
using System;
using System.Collections.Generic;

namespace Nitrogen_FrontEnd
{


    class ExcelToSqlServer
    {

        static DatabaseService DbService = new DatabaseService("Server=JAA-WIN10DEV-VM;Database=NitrogenDB;User Id=sa;Password=alpha;");
        static public Dictionary<string, int> ColumnNumbers = new Dictionary<string, int>();
        static private List<Equipment> Equipment = new List<Equipment>();

        static public void ReadExcelFile()
        {

            Application excelApp = new Application();
            Workbook workbook = excelApp.Workbooks.Open(@"C:\Jobs-Controls\NitrogenDb\6013 Equipment List.xls");
            Worksheet worksheet = workbook.Sheets[2];

            Range usedRange = worksheet.UsedRange;
            int rowCount = usedRange.Rows.Count;
            int columnCount = usedRange.Columns.Count;

            Console.WriteLine(rowCount.ToString() + " " + columnCount.ToString());

            List<string[]> projectData = new List<string[]>();

            for (int i = 1; i <= rowCount; i++)
            {
                string[] rowData = new string[columnCount];
                for (int j = 1; j <= columnCount; j++)
                {
                    Range cell = usedRange.Cells[i, j];
                    if (cell.Value != null)
                    {

                        FindProjectNumber(cell);
                        AddColumnNumbers(cell);
                        rowData[j - 1] = cell.Value.ToString();
                    }
                }

                projectData.Add(rowData);
            }

            foreach (var entry in ColumnNumbers) Console.WriteLine($"{entry.Key} {entry.Value}");

            foreach (string[] rowData in projectData)
            {
                foreach (string value in rowData)
                {
                    Console.Write(value + "\t");
                }
                Console.WriteLine();
                Console.Read();
            }

            workbook.Close(false);
            excelApp.Quit();
            ReleaseObject(worksheet);
            ReleaseObject(workbook);
            ReleaseObject(excelApp);

        }

        static private void FindProjectNumber(Range cell)
        {
            if (cell.Value.ToString().Length >= 9)
            {
                string checkString = cell.Value.ToString().Substring(0, 9);
                if (checkString.ToLower() == "project #")
                {
                    Console.WriteLine("------------ Found Project -------------------");
                    Console.WriteLine(cell.Value.ToString().Substring(10));

                    Project project = new Project
                    { 
                        ProjectNumber = cell.Value.ToString().Substring(10)
                    };

                    DbService.AddProject(project);
                }
            }
        }

        static private void AddColumnNumbers(Range cell)
        {
            bool found = false;
            var cellValue = cell.Value.ToString().ToLower();
            switch (cellValue)
            {
                case "equip list #":
                case "associated control panel":
                case "description":
                    if (!ColumnNumbers.ContainsKey(cellValue))
                    {
                        ColumnNumbers[cellValue] = cell.Column;
                        found = true;
                    }
                    break;
            }
            if (found) Console.WriteLine("------------ added coll number========");
        }

        static private void ReleaseObject(object obj)
        {
            try
            {
                System.Runtime.InteropServices.Marshal.ReleaseComObject(obj);
                obj = null;
            }
            catch (Exception ex)
            {
                obj = null;
                Console.WriteLine("Exception Occured while releasing object " + ex.ToString());
            }
            finally
            {
                GC.Collect();
            }
        }
    }
}

