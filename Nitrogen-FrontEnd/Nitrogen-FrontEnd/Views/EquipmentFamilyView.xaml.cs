using Nitrogen_FrontEnd.Models;
using Nitrogen_FrontEnd.Services;
using Nitrogen_FrontEnd.Utilities;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace Nitrogen_FrontEnd.Views
{
    /// <summary>
    /// Interaction logic for EquipmentFamilyView.xaml
    /// </summary>
    public partial class EquipmentFamilyView : Page
    {
        private readonly DatabaseService databaseService;
        private readonly ExcelWriter excelWriter;
        private EquipSheetFormat sheetFormat;
        private readonly string projectNumber;
        private readonly string equipmentId;

        public EquipmentFamilyView(string equipmentId, string projectNumber)
        {
            InitializeComponent();

            this.equipmentId = equipmentId;
            this.projectNumber = projectNumber;
            databaseService = new DatabaseService("Server=JAA-WIN10DEV-VM;Database=NitrogenDB;User Id=sa;Password=alpha;");
            excelWriter = GenerateExcelWriter();

            ShowEquipmentFamily();
        }

        public void ShowEquipmentFamily()
        {
            try
            {
                List<Equipment> equipmentDataList = databaseService.GetEquipmentFamily(equipmentId, projectNumber);

                equipmentList.ItemsSource = equipmentDataList;
                equipmentList.SelectedValuePath = "EquipmentId";
                equipmentList.AutoGenerateColumns = true;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }
        }

        private ExcelWriter GenerateExcelWriter()
        {

            Project project = databaseService.GetProjectByProjectNumber(projectNumber);
            sheetFormat = databaseService.GetSheetFormatById(project.EquipSheetFormatId);
            return new ExcelWriter(sheetFormat.FileName);
        }

        public void UpdateDb_Click(object sender, RoutedEventArgs e)
        {
            EquipmentUpdater.UpdateDatabase(databaseService, equipmentList);
        }

        public void UpdateExcel_Click(object sender, RoutedEventArgs e)
        {
            EquipmentUpdater.UpdateExcel(databaseService, excelWriter, equipmentList, sheetFormat);
        }

    }
}
