﻿using Nitrogen_FrontEnd.Models;
using Nitrogen_FrontEnd.Services;
using System;
using System.Collections.Generic;
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
    /// Interaction logic for ProjectEquipmentView.xaml
    /// </summary>
    public partial class ProjectEquipmentView : Page
    {
        private DatabaseService databaseService;
        public SqlConnection sqlConnection = new SqlConnection("Server=JAA-WIN10DEV-VM;Database=NitrogenDB;User Id=sa;Password=alpha;");
        private string ProjectNumber;

        public ProjectEquipmentView(string projectNumber)
        {
            InitializeComponent();

            databaseService = new DatabaseService("Server=JAA-WIN10DEV-VM;Database=NitrogenDB;User Id=sa;Password=alpha;");
            ProjectNumber = projectNumber;

            ShowEquipment();
        }

        private void ShowEquipment()
        {
            try
            {
                var equipmentDataList = databaseService.GetEquipmentForProject(ProjectNumber);

                equipmentList.ItemsSource = equipmentDataList;
                equipmentList.SelectedValuePath = "Id";
                equipmentList.AutoGenerateColumns = true;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }
        } 

        private void ViewEquipmentCard_Click(object sender, RoutedEventArgs e)
        {
            object selectedId = equipmentList.SelectedValue;
            if (selectedId != null)
            {
                SingleEquipmentView singleEquipmentView = new SingleEquipmentView((int)selectedId);
                NavigationService.Navigate(singleEquipmentView);
            }
            else
            {
                MessageBox.Show("Please Select Equipment");
            }
        }

        public void EditEquipment_Click(object sender, RoutedEventArgs e)
        {
            object selectedId = equipmentList.SelectedValue;
            if (selectedId != null)
            {
                Equipment selectedEquipment = (Equipment)equipmentList.SelectedItem;

                databaseService.EditEquipment(selectedEquipment);
            }
            else
            {
                MessageBox.Show("Please Select Equipment");
            }
        }
    }
}
