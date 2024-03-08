using Microsoft.Office.Interop.Excel;
using Nitrogen_FrontEnd.Models;
using Nitrogen_FrontEnd.Services;
using System;
using System.Collections.Generic;

namespace Nitrogen_FrontEnd
{
    class ExcelService
    {
        private string FilePath;
        static private DatabaseService DbService = new DatabaseService("Server=JAA-WIN10DEV-VM;Database=NitrogenDB;User Id=sa;Password=alpha;");
        private Dictionary<string, int> ColumnNumbers;
        private string ProjectNumber;

        public ExcelService(string filePath)
        {
            FilePath = filePath;
            ColumnNumbers = new Dictionary<string, int>();
        }

        public void ReadExcelFile()
        {
            Application excelApp = new Application();
            Workbook workbook = excelApp.Workbooks.Open(FilePath);

            foreach (Worksheet worksheet in workbook.Sheets)
            {
                string worksheetName = worksheet.Name;
                if (worksheetName != "Notes")
                {
                    Range usedRange = worksheet.UsedRange;
                    int rowCount = usedRange.Rows.Count;
                    int columnCount = usedRange.Columns.Count;

                    for (int y = 1; y <= rowCount; y++)
                    {
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
                                }
                            }
                        }
                        else
                        {
                            AddEquipmentFromRow(usedRange, y);
                        }
                    }

                    foreach (var entry in ColumnNumbers)
                        Console.WriteLine($"{entry.Key} {entry.Value}");
                    ReleaseObject(worksheet);
                }
            }
            workbook.Close(false);
            excelApp.Quit();
            ReleaseObject(workbook);
            ReleaseObject(excelApp);
        }

        private void FindProjectNumber(Range cell, Range usedRange)
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

        private void AddColumnNumbers(Range cell)
        {
            string cellValue = cell.Value.ToString().ToLower();

            switch (cellValue)
            {
                case "equip list #":
                case "associated control panel":
                case "description":
                    if (!ColumnNumbers.ContainsKey(cellValue))
                    {
                        ColumnNumbers[cellValue] = cell.Column;
                    }
                    break;
            }
        }

        private void AddEquipmentFromRow(Range usedRange, int row)
        {
            Range equipCell = usedRange.Cells[row, ColumnNumbers["equip list #"]];
            if (equipCell.Value != null)
            {
                Dictionary<string, string> ids = ExtractEquipmentIdAndSubId(usedRange.Cells[row, ColumnNumbers["equip list #"]].Value?.ToString());
                if (DbService.GetSingleEquipmentByIdsAndProjectNumber(ids["id"], ids.ContainsKey("subId") ? ids["subId"] : null, ProjectNumber) == null)
                {
                    Console.WriteLine(equipCell.Value.ToString());
                    Equipment equipment = CreateEquipmentFromRow(usedRange, row, ids);
                    DbService.AddEquipment(equipment);
                }
            }
        }

        private Equipment CreateEquipmentFromRow(Range usedRange, int row, Dictionary<string, string> ids)
        {
            Equipment equipment = new Equipment
            {
                ProjectNumber = ProjectNumber,
                Area = usedRange.Parent.Name,
                EquipmentId = ids["id"],
            };

            if (ids.ContainsKey("subId"))
            {
                equipment.EquipmentSubId = ids["subId"];
            };

            if (usedRange.Cells[row, ColumnNumbers["description"]].Value != null)
            {
                equipment.Description = usedRange.Cells[row, ColumnNumbers["description"]].Value?.ToString();
            }
            if (usedRange.Cells[row, ColumnNumbers["associated control panel"]].Value != null)
            {
                equipment.ControlPanel = usedRange.Cells[row, ColumnNumbers["associated control panel"]]?.Value.ToString();
            }

            return equipment;
        }

        private Dictionary<string, string> ExtractEquipmentIdAndSubId(string equipmentListNum)
        {
            string[] ids = equipmentListNum.Split('.');

            Dictionary<string, string> idDict = new Dictionary<string, string>()
            {
                { "id" , ids[0] },
            };

            if (ids.Length > 1 && ids[1] != "0" && ids[1] != "00")
            {
                idDict.Add("subId", ids[1]);
            };

            return idDict;
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
