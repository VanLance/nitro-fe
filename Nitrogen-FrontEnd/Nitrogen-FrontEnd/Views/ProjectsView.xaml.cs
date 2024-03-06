﻿using Nitrogen_FrontEnd.Services;
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
            ProjectEquipmentView projectEquipmentView = new ProjectEquipmentView(projectList.SelectedValuePath);

            projectEquipmentView.Show();
            this.Close();
        }

    }
}
