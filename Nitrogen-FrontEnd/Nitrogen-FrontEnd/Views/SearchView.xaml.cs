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
        private List<Equipment> Equipment;

        public SearchView()
        {
            InitializeComponent();

            ProjectService = new ProjectService("Server=JAA-WIN10DEV-VM;Database=NitrogenDB;User Id=sa;Password=alpha;");
            EquipmentService = new EquipmentService("Server=JAA-WIN10DEV-VM;Database=NitrogenDB;User Id=sa;Password=alpha;");

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
                    Equipment = EquipmentService.GetEquipmentFamily(searchInput.Text, projectNumber);
                    break;
                case "Equipment List #":
                    var equipmentIds = EquipmentUtility.ExtractEquipmentIdAndSubId(searchInput.Text);
                    Equipment = new List<Equipment>()
                    {
                        EquipmentService.GetSingleEquipmentByIdsAndProjectNumber(equipmentIds["id"], equipmentIds["subId"], projectNumber)
                    };
                    break;
                case "Area":
                    Equipment = EquipmentService.GetEquipmentForArea(projectNumber, searchInput.Text);
                    break;
                case "Description":
                    break;
            }
        }
    }
}
