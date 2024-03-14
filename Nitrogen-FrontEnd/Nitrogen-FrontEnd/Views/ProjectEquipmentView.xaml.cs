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
        private ExcelWriter ExcelWriter;
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

        private void SubmitButton_Click(object sender, RoutedEventArgs e)
        {
            if (actionComboBox.SelectedItem != null)
            {
                string selectedAction = (actionComboBox.SelectedItem as ComboBoxItem).Content.ToString();
                switch (selectedAction)
                {
                    case "View Equipment Card":
                        ViewEquipmentCard();
                        break;
                    case "Update Database":
                        UpdateDatabase();
                        break;
                    case "Update Spreadsheet":
                        UpdateSpreadsheet();
                        break;
                }
            }
        }
        private void ViewEquipmentCard()
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

        public void UpdateDatabase()
        {
            object selectedId = equipmentList.SelectedValue;
            if (selectedId != null)
            {
                EquipmentUpdater.UpdateDatabase(equipmentService, equipmentList);
            }
            else
            {
                MessageBox.Show("Please Select Equipment");
            }
        }

        public void UpdateSpreadsheet()
        {
            object selectedId = equipmentList.SelectedValue;
            if (selectedId != null)
            {
                Project project = projectService.GetProjectByProjectNumber(projectNumber);
                sheetFormat = sheetFormatService.GetSheetFormatById(project.EquipSheetFormatId);
                ExcelWriter = ExcelWriterGenerator.ExcelWriter(projectNumber);
                EquipmentUpdater.UpdateExcel(mappingService, ExcelWriter, equipmentList, sheetFormat);
            }
            else
            {
                MessageBox.Show("Please Select Equipment");
            }
        }
    }
}
