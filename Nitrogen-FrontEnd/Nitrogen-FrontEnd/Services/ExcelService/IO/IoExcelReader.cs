using Microsoft.Office.Interop.Excel;
using Nitrogen_FrontEnd.Models;
using Nitrogen_FrontEnd.Utilities;
using System;
using System.Collections.Generic;
using System.Windows;
using Application = Microsoft.Office.Interop.Excel.Application;

namespace Nitrogen_FrontEnd.Services.ExcelService.IO
{
    class IoExcelReader
    {
        private string FilePath;
        Application excelApp;
        Workbook workbook;
        string worksheetName;

        private string ProjectNumber;
        private Project project;
        private Dictionary<string, Dictionary<string, int>> IoColumnNumbers;
        private string currentPage;

        public IoExcelReader(string filePath)
        {
            FilePath = filePath;
            excelApp = new Application();
            workbook = excelApp.Workbooks.Open(FilePath);

            IoColumnNumbers = new Dictionary<string, Dictionary<string, int>>
            {
                { "IO", new Dictionary<string, int>() },
                { "RackList", new Dictionary<string, int>() }
            };
        }

        public void ReadExcelFile(Action<double> updateProgressBar, Action<string> updateProgressLabel)
        {

            for (int i = workbook.Sheets.Count; i >= 1; i--)
            {
                Worksheet worksheet = workbook.Sheets[i];

                worksheetName = worksheet.Name;

                currentPage = worksheetName.EndsWith("Rack Layout") ? "rack" : "io";

                bool isAreaInDbChecked = false;
                Range usedRange = worksheet.UsedRange;
                int rowCount = usedRange.Rows.Count;
                int columnCount = usedRange.Columns.Count;

                updateProgressLabel(worksheetName);
                updateProgressBar(0);

                for (int y = 1; y <= rowCount; y++)
                {
                    double progressPercentage = (double)y / rowCount * 100;
                    updateProgressBar(progressPercentage);


                    if (!isAreaInDbChecked && ProjectNumber != null)
                    {
                        isAreaInDbChecked = true;
                        if (IsAreaInDb())
                        {
                            MessageBox.Show("Area Already in Database");
                            Console.Write("Area Already in Database");
                            break;
                        }
                    }
                    if (IoColumnNumbers.Count == 0)
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
                        AddIoFromRow(usedRange, y);
                    }
                }

                InteropRelease.ReleaseObject(worksheet);

            }
            workbook.Close(false);
            excelApp.Quit();
            InteropRelease.ReleaseObject(workbook);
            InteropRelease.ReleaseObject(excelApp);
            MessageBox.Show("Complete");
        }

        private void FindProjectNumber(Range cell, Range usedRange)
        {
            if (cell.Value.ToString().Length >= 9)
            {
                string checkString = cell.Value.ToString().Substring(0, 7);
                if (checkString.ToLower() == "project")
                {
                    ProjectNumber = cell.Value.ToString().Substring(7).TrimStart(' ', '#');
                    project = new Project
                    {
                        ProjectNumber = cell.Value.ToString().Substring(10),
                        Description = usedRange.Cells[cell.Row - 2, cell.Column].Value.ToString(),
                    };
                    Console.WriteLine("Found project Number " + ProjectNumber);
                }
            }
        }

        private void AddIoFromRow(Range usedRange, int y)
        {
            throw new NotImplementedException();
        }

        private bool IsAreaInDb()
        {
            throw new NotImplementedException();
        }

        private void AddColumnNumbers(Range cell, int columnCount)
        {
            throw new NotImplementedException();
        }
    }
}
