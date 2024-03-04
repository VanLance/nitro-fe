using Nitrogen_FrontEnd.Services;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Nitrogen_FrontEnd.Views
{
    /// <summary>
    /// Interaction logic for ProjectsView.xaml
    /// </summary>
    public partial class ProjectsView : Window
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
                // Retrieve projects from the DatabaseService
                var projects = databaseService.GetAllProjects();

                // Bind projects to the DataGrid
                projectList.ItemsSource = projects;
                projectList.SelectedValuePath = "ProjectNumber";
                projectList.AutoGenerateColumns = true;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }
        }
    }
}
