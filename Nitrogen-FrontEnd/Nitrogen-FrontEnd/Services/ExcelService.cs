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
        static public string ProjectNumber;

        static public void ReadExcelFile()
        {

            string filePath1 = @"C:\Jobs-Controls\NitrogenDb\7024 Equipment List.xls";
            string filePath2 = @"C:\Jobs-Controls\NitrogenDb\6013 Equipment List.xls";

            Application excelApp = new Application();
            Workbook workbook = excelApp.Workbooks.Open(filePath2);
            Worksheet worksheet = workbook.Sheets[2];

            Range usedRange = worksheet.UsedRange;
            int rowCount = usedRange.Rows.Count;
            int columnCount = usedRange.Columns.Count;

            Console.WriteLine(rowCount.ToString() + " " + columnCount.ToString());

            List<string[]> projectData = new List<string[]>();

            for (int y = 1; y <= rowCount; y++)
            {
                Console.WriteLine(ColumnNumbers.Count.ToString() + "========== dict count");
                string[] rowData = new string[columnCount];
                if (ColumnNumbers.Count == 0)
                {

                    for (int x = 1; x <= columnCount; x++)
                    {
                        Range cell = usedRange.Cells[y, x];
                        if (cell.Value != null)
                        {
                            if (ProjectNumber == null)
                            {
                                FindProjectNumber(cell, usedRange);
                            }
                            AddColumnNumbers(cell);
                            rowData[x - 1] = cell.Value.ToString();
                        }
                    }
                }
                else
                {
                    Console.WriteLine(ColumnNumbers["equip list #"] + "checking value in dict");
                    Console.WriteLine(ColumnNumbers["equip list #"].ToString() + "equip list number col");
                    Console.WriteLine(y.ToString() + "y ");
                    Range equipCell = usedRange.Cells[y, ColumnNumbers["equip list #"]];
                    if (equipCell.Value != null)
                    {
                        Console.WriteLine(equipCell.Value.ToString());
                        Equipment equipment = new Equipment
                        {
                            ProjectNumber = ProjectNumber,
                            EquipmentId = usedRange.Cells[y, ColumnNumbers["equip list #"]].Value.ToString(),
                            Description = usedRange.Cells[y, ColumnNumbers["description"]].Value.ToString(),
                            ControlPanel = usedRange.Cells[y, ColumnNumbers["associated control panel"]].Value.ToString(),
                        };
                        DbService.AddEquipment(equipment);
                    }
                    else
                    {
                        Console.WriteLine("NULL");
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

        static private void FindProjectNumber(Range cell, Range usedRange)
        {
            if (cell.Value.ToString().Length >= 9)
            {
                string checkString = cell.Value.ToString().Substring(0, 9);
                if (checkString.ToLower() == "project #")
                {
                    Console.WriteLine("------------ Found Project -------------------");
                    ProjectNumber = cell.Value.ToString().Substring(10);

                    Project project = new Project
                    {
                        ProjectNumber = cell.Value.ToString().Substring(10),
                        Description = usedRange.Cells[cell.Row - 2, cell.Column].Value.ToString(),
                    };

                    DbService.AddProject(project);

                }
            }
        }

        static private void AddColumnNumbers(Range cell)
        {
            bool found = false;
            string cellValue = cell.Value.ToString().ToLower();

            switch (cellValue)
            {
                case "equip list #":
                case "associated control panel":
                case "description":
                    Console.WriteLine(cellValue + "  SHOuld be in dict =============");
                    if (!ColumnNumbers.ContainsKey(cellValue))
                    {
                        ColumnNumbers[cellValue] = cell.Column;
                        Console.WriteLine(ColumnNumbers[cellValue.Trim()] + "     in dict ============");
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

