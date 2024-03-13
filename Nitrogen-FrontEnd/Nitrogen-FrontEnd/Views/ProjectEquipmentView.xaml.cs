using Nitrogen_FrontEnd.Models;
using Nitrogen_FrontEnd.Services;
using Nitrogen_FrontEnd.Services.DatabaseService;
using Nitrogen_FrontEnd.Utilities;
using System;
using System.Windows;
using System.Windows.Controls;

namespace Nitrogen_FrontEnd.Views
{
    /// <summary>
    /// Interaction logic for ProjectEquipmentView.xaml
    /// </summary>
    public partial class ProjectEquipmentView : Page
    {
        private readonly ProjectService projectService;
        private readonly EquipmentService equipmentService;
        private readonly EquipmentSheetFormatService sheetFormatService;
        private readonly MappingService mappingService;
        private readonly ExcelWriter ExcelWriter;
        private EquipSheetFormat sheetFormat;
        private readonly string projectNumber;

        public ProjectEquipmentView(string projectNumber)
        {
            InitializeComponent();

            this.projectNumber = projectNumber;
            projectService = new ProjectService("Server=JAA-WIN10DEV-VM;Database=NitrogenDB;User Id=sa;Password=alpha;");
            equipmentService = new EquipmentService("Server=JAA-WIN10DEV-VM;Database=NitrogenDB;User Id=sa;Password=alpha;");
            sheetFormatService = new EquipmentSheetFormatService("Server=JAA-WIN10DEV-VM;Database=NitrogenDB;User Id=sa;Password=alpha;");
            mappingService = new MappingService("Server=JAA-WIN10DEV-VM;Database=NitrogenDB;User Id=sa;Password=alpha;");
            ExcelWriter = ExcelWriterGenerator.ExcelWriter(projectNumber);
            ShowEquipment();
        }

        private void ShowEquipment()
        {
            try
            {
                var equipmentDataList = equipmentService.GetEquipmentForProject(projectNumber); ;

                equipmentList.ItemsSource = equipmentDataList;
                equipmentList.SelectedValuePath = "Id";
                equipmentList.AutoGenerateColumns = true;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }
        }

        private void ViewEquipmentCard_Click(object sender, RoutedEventArgs e)
        {
            object selectedId = equipmentList.SelectedValue;
            if (selectedId != null)
            {
                SingleEquipmentView singleEquipmentView = new SingleEquipmentView((int)selectedId);
                NavigationService.Navigate(singleEquipmentView);
            }
            else
            {
                MessageBox.Show("Please Select Equipment");
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
            EquipmentUpdater.UpdateExcel(mappingService, ExcelWriter, equipmentList, sheetFormat);
        }
    }
}
