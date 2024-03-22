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
    /// Interaction logic for EquipmentView.xaml
    /// </summary>
    public partial class EquipmentView : Page
    {
        private readonly ProjectService projectService;
        private readonly EquipmentService equipmentService;
        private readonly EquipmentSheetFormatService sheetFormatService;
        private readonly MappingService mappingService;
        private ExcelWriter ExcelWriter;
        private readonly Project project;
        private EquipSheetFormat sheetFormat;
        private readonly string projectNumber;
        private readonly string title;
        private readonly List<Equipment> equipmentList;
        private Dictionary<int, Equipment> updatedRows;

        public EquipmentView(List<Equipment> equipmentList, string projectNumber, string title)
        {
            InitializeComponent();

            this.equipmentList = equipmentList;
            this.projectNumber = projectNumber;
            this.title = title;

            projectService = new ProjectService(SqlConnectionString.connectionString);
            project = projectService.GetProjectByProjectNumber(projectNumber);

            equipmentService = new EquipmentService(SqlConnectionString.connectionString);
            sheetFormatService = new EquipmentSheetFormatService(SqlConnectionString.connectionString);
            mappingService = new MappingService(SqlConnectionString.connectionString);
            sheetFormat = sheetFormatService.GetSheetFormatById(project.EquipSheetFormatId);

            updatedRows = new Dictionary<int, Equipment>();
            equipmentGrid.CellEditEnding += EquipmentGrid_CellEditEnding;

            ShowEquipment();
        }

        public void ShowEquipment()
        {
            equipmentGrid.ItemsSource = equipmentList;
            equipmentGrid.SelectedValuePath = "EquipmentId";
            equipmentGrid.AutoGenerateColumns = true;
        }

        private void EquipmentGrid_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {

            Equipment editedEquipment = (Equipment)e.Row.Item;

            if (editedEquipment != null)
            {
                updatedRows[editedEquipment.Id] = editedEquipment;
            }
        }

        private void SaveAllButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                foreach (KeyValuePair<int, Equipment> row in updatedRows)
                {
                    equipmentService.EditEquipment(row.Value);
                }
                updatedRows.Clear();
                MessageBox.Show("Database Updated");
            }
            catch (Exception err)
            {
                MessageBox.Show("Error: " + err.ToString());
                Console.WriteLine(err.ToString());
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
                    case "Update Database and Spreadsheet":
                        UpdateDatabaseAndSpreadsheet();
                        break;
                    case "Update Database":
                        UpdateDatabase();
                        break;
                    case "Update Spreadsheet":
                        UpdateSpreadsheet();
                        break;
                    case "View Equipment Family":
                        ViewEquipmentFamily();
                        break;
                }
            }
        }

        private void ViewEquipmentCard()
        {
            object selectedId = equipmentGrid.SelectedValue;

            if (selectedId != null)
            {
                Equipment equipment = (Equipment)equipmentGrid.SelectedItem;
                SingleEquipmentView singleEquipmentView = new SingleEquipmentView(equipment.Id);
                NavigationService.Navigate(singleEquipmentView);
            }
            else
            {
                MessageBox.Show("Please Select Equipment");
            }
        }

        private void ViewEquipmentFamily()
        {
            object selectedId = equipmentGrid.SelectedValue;
            if (selectedId != null)
            {
                Equipment selectedEquipment = (Equipment)equipmentGrid.SelectedItem;

                List<Equipment> equipmentList = equipmentService.GetEquipmentFamily(selectedEquipment.EquipmentId, projectNumber);

                EquipmentView equipmentFamilyView = new EquipmentView(equipmentList, selectedEquipment.ProjectNumber, $"Equipment Family: {selectedEquipment.EquipmentId}");
                NavigationService.Navigate(equipmentFamilyView);
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

