using Nitrogen_FrontEnd.Models;
using Nitrogen_FrontEnd.Services.DatabaseService;
using Nitrogen_FrontEnd.Utilities;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace Nitrogen_FrontEnd.Views
{
    /// <summary>
    /// Interaction logic for SearchView.xaml
    /// </summary>
    public partial class SearchView : Page
    {
        private readonly ProjectService ProjectService;
        private readonly EquipmentService EquipmentService;
        private List<Equipment> EquipmentList;

        public SearchView()
        {
            InitializeComponent();

            ProjectService = new ProjectService(SqlConnectionString.connectionString);
            EquipmentService = new EquipmentService(SqlConnectionString.connectionString);

            LoadProjectItems();
        }

        private void LoadProjectItems()
        {
            List<Project> projects = ProjectService.GetAllProjects();

            projectComboBox.ItemsSource = projects;
        }

        private void Search_Click(object sender, RoutedEventArgs e)
        {
            Project selectedProject = projectComboBox.SelectedItem as Project;
            string projectNumber = selectedProject.ProjectNumber;
            string searchBy = (searchByComboBox.SelectedItem as ComboBoxItem).Content.ToString();
            switch (searchBy)
            {
                case "Family Id":
                    ViewEquipmentFamily(searchInput.Text, projectNumber);
                    break;
                case "Equipment List #":
                    ViewEquipmentCard(searchInput.Text, projectNumber);
                    break;
                case "Area":
                    ViewEquipmentArea(searchInput.Text, projectNumber);
                    break;
                case "Description":
                    break;
            }
        }

        private void ViewEquipmentFamily(string equipmentId, string projectNumber)
        {
            EquipmentList = EquipmentService.GetEquipmentFamily(searchInput.Text, projectNumber);

            EquipmentView equipmentView = new EquipmentView(EquipmentList, projectNumber, $"Equipment Family: {equipmentId}");
            NavigationService.Navigate(equipmentView);
        }

        private void ViewEquipmentCard(string equipmentId, string projectNumber)
        {

            var equipmentIds = EquipmentUtility.ExtractEquipmentIdAndSubId(equipmentId);
            Equipment equipment = EquipmentService.GetSingleEquipmentByIdsAndProjectNumber(equipmentIds["id"], equipmentIds["subId"], projectNumber);

            SingleEquipmentView singleEquipmentView = new SingleEquipmentView(equipment.Id);
            NavigationService.Navigate(singleEquipmentView);
        }

        private void ViewEquipmentArea(string area, string projectNumber)
        {
            EquipmentList = EquipmentService.GetEquipmentForArea(projectNumber, area);

            EquipmentView EquipmentView = new EquipmentView(EquipmentList, projectNumber, $"Area: {area} Equpiment");
            NavigationService.Navigate(EquipmentView);
        }
    }
}
