﻿using Microsoft.Office.Interop.Excel;
using Nitrogen_FrontEnd.Models;
using Nitrogen_FrontEnd.Services;
using System;
using System.Windows;
using System.Collections.Generic;
using Application = Microsoft.Office.Interop.Excel.Application;
using Nitrogen_FrontEnd.Services.DatabaseService;

namespace Nitrogen_FrontEnd
{
    public class ExcelReader
    {
        private string FilePath;
        static private ProjectService projectService;
        private readonly MappingService mappingService;
        private readonly EquipmentService equipmentService;
        private readonly EquipmentSheetFormatService sheetFormatService;
        private string ProjectNumber;
        private Project project;
        private Dictionary<string, int> ColumnNumbers = new Dictionary<string, int>();
        public Dictionary<string, bool> SelectedAreas = new Dictionary<string, bool>();
        Application excelApp;
        Workbook workbook;
        string worksheetName;

        public ExcelReader(string filePath)
        {
            FilePath = filePath;
            projectService = new ProjectService("Server=JAA-WIN10DEV-VM;Database=NitrogenDB;User Id=sa;Password=alpha;");
            mappingService = new MappingService("Server=JAA-WIN10DEV-VM;Database=NitrogenDB;User Id=sa;Password=alpha;");
            equipmentService = new EquipmentService("Server=JAA-WIN10DEV-VM;Database=NitrogenDB;User Id=sa;Password=alpha;");
            sheetFormatService = new EquipmentSheetFormatService("Server=JAA-WIN10DEV-VM;Database=NitrogenDB;User Id=sa;Password=alpha;");

            excelApp = new Application();
            workbook = excelApp.Workbooks.Open(FilePath);

            GenerateSelctedAreas();
        }

        public void GenerateSelctedAreas()
        {
            foreach (Worksheet worksheet in workbook.Sheets)
            {
                SelectedAreas.Add(worksheet.Name, false);
            }
        }

        public void ReadExcelFile()
        {

            foreach (Worksheet worksheet in workbook.Sheets)
            {
                worksheetName = worksheet.Name;
                bool isAreaInDbChecked = false;
                if (SelectedAreas[worksheetName])
                {
                    Range usedRange = worksheet.UsedRange;
                    int rowCount = usedRange.Rows.Count;
                    int columnCount = usedRange.Columns.Count;

                    for (int y = 1; y <= rowCount; y++)
                    {
                        Console.WriteLine(isAreaInDbChecked + "IS AREA CHECKEC");
                        if (!isAreaInDbChecked && ProjectNumber != null)
                        {
                            isAreaInDbChecked = true;
                            if (IsAreaInDb())
                            {
                                isAreaInDbChecked = true;
                                MessageBox.Show("Area Already in Databse");
                                Console.Write("Area Already in Database");
                                break;
                            }
                        }
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
                                    else
                                    {
                                        AddColumnNumbers(cell, columnCount);
                                    }
                                }
                            }
                        }
                        else
                        {
                            AddEquipmentFromRow(usedRange, y);
                        }
                    }

                    ReleaseObject(worksheet);
                }
            }
            workbook.Close(false);
            excelApp.Quit();
            ReleaseObject(workbook);
            ReleaseObject(excelApp);
            MessageBox.Show("Complete");
        }

        private void FindProjectNumber(Range cell, Range usedRange)
        {
            if (cell.Value.ToString().Length >= 9)
            {
                string checkString = cell.Value.ToString().Substring(0, 9);
                if (checkString.ToLower() == "project #")
                {
                    ProjectNumber = cell.Value.ToString().Substring(10);

                    project = new Project
                    {
                        ProjectNumber = cell.Value.ToString().Substring(10),
                        Description = usedRange.Cells[cell.Row - 2, cell.Column].Value.ToString(),
                    };
                    Console.WriteLine("Found project Number " + ProjectNumber);
                }
            }
        }

        private bool IsProjectInDb()
        {
            return projectService.GetProjectByProjectNumber(project.ProjectNumber) != null;
        }

        private bool IsAreaInDb()
        {
            List<Equipment> equipment = equipmentService.GetEquipmentForArea(ProjectNumber, worksheetName);
            Console.WriteLine("Checking area equip count \n\n" + equipment.Count);
            return equipment.Count > 0;
        }

        private void AddColumnNumbers(Range cell, int columnCount)
        {
            string cellValue = cell.Value.ToString().Trim().ToLower();

            switch (cellValue)
            {
                case "comments":
                    ColumnNumbers["notes"] = cell.Column;
                    break;
                case "equip list #":
                case "associated control panel":
                case "description":
                case "notes":
                    if (!ColumnNumbers.ContainsKey(cellValue))
                    {
                        ColumnNumbers[cellValue] = cell.Column;
                    }
                    break;
            }
            if (ColumnNumbers.Count == 4) AddProjectToDb(cell, worksheetName);
        }

        private void AddProjectToDb(Range cell, string worksheetName)
        {
            int dbExcelMapPk = AddEquipDbExcelMap();

            int sheetFormatPk = AddEquipSheetFormat(cell, dbExcelMapPk);

            project.EquipSheetFormatId = sheetFormatPk;
            projectService.AddProject(project);
        }

        private int AddEquipDbExcelMap()
        {
            EquipDbFieldToExcelColumnMap dbToExcelMap = new EquipDbFieldToExcelColumnMap();
            foreach (var entry in ColumnNumbers)
            {
                switch (entry.Key)
                {
                    case "notes":
                    case "comments":
                        dbToExcelMap.Notes = entry.Value;
                        break;
                    case "equip list #":
                        dbToExcelMap.EquipListNumber = entry.Value;
                        break;
                    case "associated control panel":
                        dbToExcelMap.ControlPanel = entry.Value;
                        break;
                    case "description":
                        dbToExcelMap.Description = entry.Value;
                        break;
                }
            }
            int dbExcelMapPk = mappingService.AddEquipDbFieldToExcelColumnMap(dbToExcelMap);
            return dbExcelMapPk;
        }

        private int AddEquipSheetFormat(Range cell, int dbExcelMapPk)
        {
            EquipSheetFormat sheetFormat = new EquipSheetFormat()
            {
                FileName = FilePath,
                StartingDataLine = cell.Row + 1,
                EquipDbFieldToExcelColumnMapId = dbExcelMapPk,
            };

            int sheetFormatPK = sheetFormatService.AddEquipSheetFormat(sheetFormat);
            return sheetFormatPK;
        }

        private void AddEquipmentFromRow(Range usedRange, int row)
        {
            Range equipCell = usedRange.Cells[row, ColumnNumbers["equip list #"]];
            if (equipCell.Value != null)
            {
                Dictionary<string, string> ids = ExtractEquipmentIdAndSubId(usedRange.Cells[row, ColumnNumbers["equip list #"]].Value?.ToString());

                Console.WriteLine(equipCell.Value.ToString());
                Equipment equipment = CreateEquipmentFromRow(usedRange, row, ids);

                equipmentService.AddEquipment(equipment);
            }
        }

        private Equipment CreateEquipmentFromRow(Range usedRange, int row, Dictionary<string, string> ids)
        {
            Equipment equipment = new Equipment
            {
                ProjectNumber = ProjectNumber,
                Area = usedRange.Parent.Name,
                EquipmentId = ids["id"],
                ExcelRowNumber = row,
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
            if (usedRange.Cells[row, ColumnNumbers["notes"]].Value != null)
            {
                equipment.Notes = usedRange.Cells[row, ColumnNumbers["notes"]]?.Value.ToString();
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