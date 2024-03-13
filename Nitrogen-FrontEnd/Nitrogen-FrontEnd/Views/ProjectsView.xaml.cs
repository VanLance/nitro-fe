using Nitrogen_FrontEnd.Models;
using Nitrogen_FrontEnd.Services;
using Nitrogen_FrontEnd.Services.DatabaseService;
using Nitrogen_FrontEnd.Utilities;
using System;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace Nitrogen_FrontEnd.Views
{
    /// <summary>
    /// Interaction logic for ProjectsView.xaml
    /// </summary>
    public partial class ProjectsView : Page
    {

        private readonly ProjectService projectService;

        public ProjectsView()
        {
            InitializeComponent();

            projectService = new ProjectService("Server=JAA-WIN10DEV-VM;Database=NitrogenDB;User Id=sa;Password=alpha;");

            ShowProjects();
        }

        private void ShowProjects()
        {
            try
            {
                var projects = projectService.GetAllProjects();
                projectList.ItemsSource = projects;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }
        }



        private void ViewProjectsEquipment_Click(object Sender, RoutedEventArgs e)
        {
            var selectedProjectNumber = projectList.SelectedValue;
            if (selectedProjectNumber != null)
            {
                ProjectEquipmentView projectEquipmentView = new ProjectEquipmentView(projectList.SelectedValue.ToString());
                NavigationService.Navigate(projectEquipmentView);
            }
            else
            {
                MessageBox.Show("Please Select Project");
            }
        }

        private void EditProject_Click(object sender, RoutedEventArgs e)
        {
            var selectedProjectNumber = projectList.SelectedValue;
            if (selectedProjectNumber != null)
            {
                Project selectedProject = (Project)projectList.SelectedItem;

                projectService.EditProject(selectedProject);
            }
            else
            {
                MessageBox.Show("Please Select Project");
            }
        }

        private void UpdateSpreadsheet_Click(object sender, RoutedEventArgs e)
        {
            object selectedProjectNumber = projectList.SelectedValue;

            if ( selectedProjectNumber != null)
            {

                string projectNumber = selectedProjectNumber.ToString();

                ExcelWriter excelWriter = ExcelWriterGenerator.ExcelWriter(projectNumber);

                excelWriter.WriteDataToExcelProject(projectNumber);
            }
        }
    }
}
