using Microsoft.Office.Interop.Excel;
using Nitrogen_FrontEnd.Models;
using Nitrogen_FrontEnd.Models.IO;
using Nitrogen_FrontEnd.Services.DatabaseService;
using Nitrogen_FrontEnd.Services.DatabaseService.IO;
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

        private readonly PartTypeService partTypeService;
        private readonly SlotService slotService;
        private readonly EquipmentService equipmentService;
        private readonly IoLayoutFormatService ioFormatService;

        private string ProjectNumber;
        private Project project;
        private Dictionary<string, IoSheets> FormattedWorksheets;
        private IoColumnNumbers IoColumnNumbers;

        public IoExcelReader(string filePath)
        {
            FilePath = filePath;
            excelApp = new Application();
            workbook = excelApp.Workbooks.Open(FilePath);

            partTypeService = new PartTypeService(SqlConnectionString.connectionString);
            slotService = new SlotService(SqlConnectionString.connectionString);
            equipmentService = new EquipmentService(SqlConnectionString.connectionString);
            ioFormatService = new IoLayoutFormatService(SqlConnectionString.connectionString);

            FormattedWorksheets = new Dictionary<string, IoSheets>();
            IoColumnNumbers = new IoColumnNumbers()
            {
                IO = new Dictionary<string, int>(),
                RackLayout = new Dictionary<string, int>()
            };
        }

        public void ReadExcelFile(/*Action<double> updateProgressBar, Action<string> updateProgressLabel*/)
        {
            UpdateFormattedWorksheetsDict(/*updateProgressBar, updateProgressLabel*/);

            ParseIoSheets(/*updateProgressBar, updateProgressLabel*/);

            workbook.Close(false);
            excelApp.Quit();
            InteropRelease.ReleaseObject(workbook);
            InteropRelease.ReleaseObject(excelApp);
            MessageBox.Show("Complete");
        }

        private void ParseIoSheets(/*Action<double> updateProgressBar, Action<string> updateProgressLabel*/)
        {
            foreach (KeyValuePair<string, IoSheets> entry in FormattedWorksheets)
            {
                Range usedRange = entry.Value.IoList.UsedRange;
                int rowCount = usedRange.Rows.Count;
                int columnCount = usedRange.Columns.Count;

                int currentSlot = -1;

                //updateProgressLabel(worksheetName);
                //updateProgressBar(0);

                for (int y = 1; y <= rowCount; y++)
                {
                    double progressPercentage = (double)y / rowCount * 100;
                    //updateProgressBar(progressPercentage);

                    if (IoColumnNumbers.IO.Count == 0)
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
                                    AddRackColumnNumbers(cell, columnCount);
                                }
                            }
                        }
                    }
                    else
                    {
                        currentSlot = UpdateCurrentBank(usedRange, y, currentSlot);
                        if (currentSlot != -1 && entry.Value.IoDataStartingLine == 0)
                        {
                            entry.Value.IoDataStartingLine = y;
                            AddIoFormatToDb(y, IoColumnNumbers.IO, entry.Value);
                        }
                        AddIoFromRow(usedRange, y, currentSlot, entry.Value);
                    }
                }
            }
        }

        private void AddIoFormatToDb(int row, Dictionary<string, int> io, IoSheets ioSheets)
        {
            int ioMapId = ioFormatService.AddIoDbFieldToExcelMap(io);
            int ioSheetFormatId = ioFormatService.AddIoSheetFormat(FilePath, ioMapId, ioSheets.RackDbToExcelMapId);
            ioSheets.SheetFormatId = ioFormatService.AddIoSheets(row, ioSheets, ioSheetFormatId);
        }

        private void UpdateFormattedWorksheetsDict(/*Action<double> updateProgressBar, Action<string> updateProgressLabel*/)
        {
            foreach (Worksheet worksheet in workbook.Sheets)
            {
                if (worksheet.Name == "Rack Layout" && FormattedWorksheets.ContainsKey("IO List"))
                {
                    ParseRackSheetForSingleIo(FormattedWorksheets["IO List"], worksheet/*, updateProgressBar, updateProgressLabel*/);
                }
                else if (worksheet.Name.EndsWith("Rack Layout"))
                {
                    int index = worksheet.Name.LastIndexOf("Rack Layout");
                    string RackArea = worksheet.Name.Substring(0, index).Trim();

                    if (FormattedWorksheets.ContainsKey(RackArea)) ParseRackSheetForSingleIo(FormattedWorksheets[RackArea], worksheet/*, updateProgressBar, updateProgressLabel*/);

                    else if (RackArea == "PLC" && FormattedWorksheets.ContainsKey("PLC Control")) ParseRackSheetForSingleIo(FormattedWorksheets["PLC Control"], worksheet/*, updateProgressBar, updateProgressLabel*/);
                }
                else if (worksheet.Name == "Rack Layouts") ParseRackSheetForMultipleIos(worksheet);
                else
                {
                    FormattedWorksheets[worksheet.Name] = new IoSheets()
                    {
                        IoList = worksheet
                    };
                }
            }
        }

        private void ParseRackSheetForSingleIo(IoSheets ioSheets, Worksheet rackWorksheet/*, Action<double> updateProgressBar, Action<string> updateProgressLabel*/)
        {

            Range usedRange = rackWorksheet.UsedRange;
            int rowCount = usedRange.Rows.Count;
            int columnCount = usedRange.Columns.Count;

            int currentBank = -1;

            //updateProgressLabel(worksheetName);
            //updateProgressBar(0);

            for (int y = 1; y <= rowCount; y++)
            {
                double progressPercentage = (double)y / rowCount * 100;
                //updateProgressBar(progressPercentage);

                if (IoColumnNumbers.RackLayout.Count == 0)
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
                                AddRackColumnNumbers(cell, columnCount);
                            }
                        }
                    }
                }
                else
                {
                    var bankCellValue = usedRange[y, IoColumnNumbers.RackLayout["bank"]].Value;
                    if (bankCellValue != null && bankCellValue.ToString().ToLower() == "bank") continue;
                    currentBank = UpdateCurrentBank(usedRange, y, currentBank);
                    if (currentBank != -1 && ioSheets.RackDataStartingLine == 0)
                    {
                        ioSheets.RackDataStartingLine = y;
                        AddRackFormatToDb(IoColumnNumbers.RackLayout, ioSheets);
                    }
                    AddRackLayoutFromRow(usedRange, y, currentBank, ioSheets);
                }
            }
        }

        private void AddRackFormatToDb(Dictionary<string, int> rackLayout, IoSheets ioSheets)
        {
            ioSheets.RackDbToExcelMapId = ioFormatService.AddRackDbFieldToExcelMap(rackLayout);
        }

        private void AddIoFromRow(Range usedRange, int row, int currentSlot, IoSheets ioSheets)
        {
            Dictionary<string, int> ioDict = IoColumnNumbers.IO;
            Dictionary<string, string> ioEquipmentIds = EquipmentUtility.ExtractEquipmentIdAndSubId(usedRange.Cells[row, ioDict["id"]]);
            IoModel io = new IoModel()
            {
                Description = usedRange.Cells[row, ioDict["description"]],
                PlcPoint = usedRange[row, ioDict["plc point"]],
                SlotId = ioSheets.RackLayout[currentSlot].Id,

                IoSheetsId = ioSheets.SheetFormatId,
            };

            var idsDict = EquipmentUtility.ExtractEquipmentIdAndSubId(usedRange.Cells[row, ioDict["Id"]]);
            io.EquipmentId = equipmentService.GetSingleEquipmentByIdsAndProjectNumber(idsDict["id"], idsDict["subId"], ProjectNumber).Id;

        }

        private int UpdateCurrentSlotValue(Range usedRange, int y, int currentSlotValue)
        {
            int slotCellValue = (int)usedRange[y, IoColumnNumbers.RackLayout["slot"]];
            return usedRange.Cells[y, slotCellValue] != null ? slotCellValue : currentSlotValue;
        }

        private int UpdateCurrentBank(Range usedRange, int y, int bank)
        {
            Console.WriteLine(bank + " bank \n\n");
            var bankCellValue = usedRange[y, IoColumnNumbers.RackLayout["bank"]].Value ;
            if (bankCellValue != null) Console.WriteLine(bankCellValue.ToString());
            return bankCellValue != null ? (int)bankCellValue : bank;
        }

        private void AddRackLayoutFromRow(Range usedRange, int row, int bank, IoSheets ioSheets)
        {
            if (usedRange.Cells[row, IoColumnNumbers.RackLayout["type"]].Value == null) return; 

            PartType partType = new PartType()
            {
                Id = usedRange.Cells[row, IoColumnNumbers.RackLayout["type"]].Value,
                Description = usedRange.Cells[row, IoColumnNumbers.RackLayout["description"]].Value.ToString()
            };
            partTypeService.AddPartType(partType);

            Console.WriteLine(usedRange.Cells[row, IoColumnNumbers.RackLayout["slot"]].Value + " PArt Type number \n\n");
            string slotNumber = usedRange.Cells[row, IoColumnNumbers.RackLayout["slot"]].Value.ToString();
            Slot slot = new Slot()
            {
                Number = slotNumber != "-" ? (int)usedRange.Cells[row, IoColumnNumbers.RackLayout["slot"]].Value : -1,
                Bank = bank,
                TypeId = partType.Id
            };

            slot.Id = slotService.AddSlot(slot);
            ioSheets.RackLayout[slot.Number] = slot;
        }

        private void ParseRackSheetForMultipleIos(Worksheet worksheet)
        {
            throw new NotImplementedException();
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
                        Description = usedRange.Cells[cell.Row - 1, cell.Column].Value.ToString(),
                    };
                    Console.WriteLine("Found project Number " + ProjectNumber);
                }
            }
        }

        private bool IsAreaInDb()
        {
            throw new NotImplementedException();
        }

        private void AddIoColumnNumbers(Range cell, int columnCount)
        {
            string cellValue = cell.Value.ToString().Trim().ToLower();

            switch (cellValue)
            {
                case "plc":
                    if (!IoColumnNumbers.IO.ContainsKey(cellValue))
                    {
                        IoColumnNumbers.IO["plc point"] = cell.Column;
                    }
                    break;
                case "bank":
                case "slot":
                case "elec page":
                case "description":
                case "id":
                case "plc point":
                    if (!IoColumnNumbers.IO.ContainsKey(cellValue))
                    {
                        IoColumnNumbers.IO[cellValue] = cell.Column;
                    }
                    break;
            }
        }

        private void AddRackColumnNumbers(Range cell, int columnCount)
        {
            string cellValue = cell.Value.ToString().Trim().ToLower();

            switch (cellValue)
            {
                case "bank":
                case "slot":
                case "type":
                case "description":
                    if (!IoColumnNumbers.RackLayout.ContainsKey(cellValue))
                    {
                        IoColumnNumbers.RackLayout[cellValue] = cell.Column;
                    }
                    break;
            }
        }
    }

    class IoSheets
    {
        public Worksheet IoList;
        public Dictionary<int, Slot> RackLayout = new Dictionary<int, Slot>();
        public int RackDataStartingLine;
        public int IoDataStartingLine;
        public int RackDbToExcelMapId;
        public int SheetFormatId;
    }

    class IoColumnNumbers
    {
        public Dictionary<string, int> RackLayout;
        public Dictionary<string, int> IO;
    }
}
