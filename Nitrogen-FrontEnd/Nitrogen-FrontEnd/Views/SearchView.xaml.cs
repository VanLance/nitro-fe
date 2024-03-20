using Nitrogen_FrontEnd.Models;
using Nitrogen_FrontEnd.Services.DatabaseService;
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

        public SearchView()
        {
            InitializeComponent();

            ProjectService = new ProjectService("Server=JAA-WIN10DEV-VM;Database=NitrogenDB;User Id=sa;Password=alpha;");

            LoadProjectItems();
        }

        private void LoadProjectItems()
        {
            List<Project> projects = ProjectService.GetAllProjects();

            foreach( Project project in projects)
            {
                ComboBoxItem comboItem = new ComboBoxItem()
                {
                    Content = project.Description
                };
                projectComboBox.Items.Add(comboItem);
            }
        }

        private void Search_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
