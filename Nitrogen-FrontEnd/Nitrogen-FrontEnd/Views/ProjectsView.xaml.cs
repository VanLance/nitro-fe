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

                projectGrid.ItemsSource = projects;
                projectGrid.SelectedValuePath = "ProjectNumber";

                projectGrid.Columns.Add(new DataGridTextColumn { Header = "Project Number", Binding = new Binding("ProjectNumber") });
                projectGrid.Columns.Add(new DataGridTextColumn { Header = "Description", Binding = new Binding("Description") });
                projectGrid.AutoGenerateColumns = false;

            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }

        }




        private void ViewProjectsEquipment_Click(object sender, RoutedEventArgs e)
        {
            Project selectedProject = (Project)projectList.SelectedItem;
            if (selectedProject != null)
            {
                ProjectEquipmentView projectEquipmentView = new ProjectEquipmentView(selectedProject.ProjectNumber);
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
                    case "View Projects Equipment":
                        ViewProjectsEquipment();
                        break;
                    case "Update Spreadsheet from DB":
                        UpdateSpreadsheet();
                        break;
                    case "Update Database from Edit":
                        UpdateDatabaseFromEdit();
                        break;
                    default:
                        break;
                }
            }
        }

        private void ViewProjectsEquipment()
        {
            var selectedProjectNumber = projectGrid.SelectedValue;
            if (selectedProjectNumber != null)
            {
                ProjectEquipmentView projectEquipmentView = new ProjectEquipmentView(projectGrid.SelectedValue.ToString());

                NavigationService.Navigate(projectEquipmentView);
            }
            else
            {
                MessageBox.Show("Please Select Project");
            }
        }

        private void UpdateDatabaseFromEdit()
        {
            var selectedProjectNumber = projectGrid.SelectedValue;
            if (selectedProjectNumber != null)
            {
                Project selectedProject = (Project)projectGrid.SelectedItem;

                projectService.EditProject(selectedProject);
            }
            else
            {
                MessageBox.Show("Please Select Project");
            }
        }

        private void UpdateSpreadsheet()
        {

            Project selectedProjectNumber = (Project)projectList.SelectedValue;


            if ( selectedProjectNumber != null)
            {

                string projectNumber = selectedProjectNumber.ProjectNumber;

                ExcelWriter excelWriter = ExcelWriterGenerator.ExcelWriter(projectNumber);

                excelWriter.WriteDataToExcelProject(projectNumber);
            }
            else
            {
                MessageBox.Show("Please Select Project");
            }
        }
    }
}
