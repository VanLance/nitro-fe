using Nitrogen_FrontEnd.Models;
using Nitrogen_FrontEnd.Services;
using System;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Controls;

namespace Nitrogen_FrontEnd.Views
{
    /// <summary>
    /// Interaction logic for ProjectsView.xaml
    /// </summary>
    public partial class ProjectsView : Page
    {
        private readonly DatabaseService databaseService;

        public ProjectsView()
        {
            InitializeComponent();

            databaseService = new DatabaseService("Server=JAA-WIN10DEV-VM;Database=NitrogenDB;User Id=sa;Password=alpha;");
            ShowProjects();
        }

        private void ShowProjects()
        {
            try
            {
                var projects = databaseService.GetAllProjects();

                projectList.ItemsSource = projects;
                projectList.SelectedValuePath = "ProjectNumber";
                projectList.AutoGenerateColumns = true;
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

                databaseService.EditProject(selectedProject);
            }
            else
            {
                MessageBox.Show("Please Select Project");
            }
        }
    }
}
