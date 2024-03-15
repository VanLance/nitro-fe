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
        private ExcelWriter ExcelWriter;
        private readonly Project project;
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
            project = projectService.GetProjectByProjectNumber(projectNumber);
            sheetFormat = sheetFormatService.GetSheetFormatById(project.EquipSheetFormatId);
            ShowEquipmentFamily();
        }

        public void ShowEquipmentFamily()
        {
            try
            {
                List<Equipment> equipmentDataList = equipmentService.GetEquipmentFamily(equipmentId, projectNumber);

                equipmentGrid.ItemsSource = equipmentDataList;
                equipmentGrid.SelectedValuePath = "EquipmentId";
                equipmentGrid.AutoGenerateColumns = true;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }
        }

        private void SubmitButton_Click(object sender, RoutedEventArgs e)
        {
            // Handle the submit button click event here
            if (actionComboBox.SelectedItem != null)
            {
                string selectedAction = (actionComboBox.SelectedItem as ComboBoxItem).Content.ToString();
                switch (selectedAction)
                {
                    case "View Equipment Card":
                        ViewEquipmentCard();
                        break;
                    case "Update Database and Spreadsheet":
                        UpdateDatabaseAndSpreadsheet();
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
            object selectedId = equipmentGrid.SelectedValue;
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
            object selectedId = equipmentGrid.SelectedValue;
            if (selectedId != null)
            {
                EquipmentUpdater.UpdateDatabase(equipmentService, equipmentGrid);
            }
            else
            {
                MessageBox.Show("Please Select Equipment");
            }
        }

        public void UpdateSpreadsheet()
        {
            object selectedId = equipmentGrid.SelectedValue;
            if (selectedId != null)
            {   
                ExcelWriter = ExcelWriterGenerator.ExcelWriter(projectNumber);
                EquipmentUpdater.UpdateExcel(mappingService, ExcelWriter, equipmentGrid, sheetFormat);
            }
            else
            {
                MessageBox.Show("Please Select Equipment");
            }
        }

        private void UpdateDatabaseAndSpreadsheet()
        {
            object selectedId = equipmentGrid.SelectedValue;
            if (selectedId != null)
            {
                ExcelWriter = ExcelWriterGenerator.ExcelWriter(projectNumber);
                EquipmentUpdater.UpdateDbAndExcel(equipmentService, equipmentGrid, mappingService, ExcelWriter, sheetFormat);
            }
            else
            {
                MessageBox.Show("Please Select Equipment");
            }
        }
    }
}
