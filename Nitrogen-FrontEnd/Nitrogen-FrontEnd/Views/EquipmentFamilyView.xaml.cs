using Nitrogen_FrontEnd.Models;
using Nitrogen_FrontEnd.Services;
using Nitrogen_FrontEnd.Services.DatabaseService;
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
        private readonly ProjectService projectService;
        private readonly EquipmentService equipmentService;
        private readonly EquipmentSheetFormatService sheetFormatService;
        private readonly MappingService mappingService;
        private readonly ExcelWriter excelWriter;
        private EquipSheetFormat sheetFormat;
        private readonly string projectNumber;
        private readonly string equipmentId;

        public EquipmentFamilyView(string equipmentId, string projectNumber)
        {
            InitializeComponent();

            this.equipmentId = equipmentId;
            this.projectNumber = projectNumber;
            projectService = new ProjectService("Server=JAA-WIN10DEV-VM;Database=NitrogenDB;User Id=sa;Password=alpha;");
            equipmentService = new EquipmentService("Server=JAA-WIN10DEV-VM;Database=NitrogenDB;User Id=sa;Password=alpha;");
            sheetFormatService = new EquipmentSheetFormatService("Server=JAA-WIN10DEV-VM;Database=NitrogenDB;User Id=sa;Password=alpha;");
            mappingService = new MappingService("Server=JAA-WIN10DEV-VM;Database=NitrogenDB;User Id=sa;Password=alpha;");
            excelWriter = ExcelWriterGenerator.ExcelWriter(projectNumber);

            ShowEquipmentFamily();
        }

        public void ShowEquipmentFamily()
        {
            try
            {
                List<Equipment> equipmentDataList = equipmentService.GetEquipmentFamily(equipmentId, projectNumber);

                equipmentList.ItemsSource = equipmentDataList;
                equipmentList.SelectedValuePath = "EquipmentId";
                equipmentList.AutoGenerateColumns = true;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }
        }


        public void UpdateDb_Click(object sender, RoutedEventArgs e)
        {
            EquipmentUpdater.UpdateDatabase(equipmentService, equipmentList);
        }

        public void UpdateExcel_Click(object sender, RoutedEventArgs e)
        {
            Project project = projectService.GetProjectByProjectNumber(projectNumber);
            sheetFormat = sheetFormatService.GetSheetFormatById(project.EquipSheetFormatId);
            EquipmentUpdater.UpdateExcel(mappingService, excelWriter, equipmentList, sheetFormat);
        }

    }
}
