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

        private DatabaseService databaseService;
        public SqlConnection sqlConnection = new SqlConnection("Server=JAA-WIN10DEV-VM;Database=NitrogenDB;User Id=sa;Password=alpha;");

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

        private void ViewProjectsEquipment_Click( object Sender, RoutedEventArgs e )
        {
            
            ProjectEquipmentView projectEquipmentView = new ProjectEquipmentView(projectList.SelectedValue.ToString());

            NavigationService.Navigate(projectEquipmentView);
        }

    }
}
