using Nitrogen_FrontEnd.Models;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace Nitrogen_FrontEnd.Views
{
    public partial class EquipmentView : UserControl
    {
        public EquipmentView()
        {
            InitializeComponent();
        }

        // Method to load equipment data into the DataGrid
        public void LoadEquipmentData(List<Equipment> equipmentDataList)
        {
            equipmentGrid.ItemsSource = equipmentDataList;
        }

        // Event handler for the Submit button click
        private void SubmitButton_Click(object sender, RoutedEventArgs e)
        {
            if (actionComboBox.SelectedItem != null)
            {
                string selectedAction = (actionComboBox.SelectedItem as ComboBoxItem).Content.ToString();
                switch (selectedAction)
                {
                    case "View Equipment Card":
                        // Handle view equipment card action
                        break;
                    case "Update Database and Spreadsheet":
                        // Handle update database and spreadsheet action
                        break;
                    case "Update Database":
                        // Handle update database action
                        break;
                    case "Update Spreadsheet":
                        // Handle update spreadsheet action
                        break;
                    case "View Equipment Family":
                        // Handle view equipment family action
                        break;
                }
            }
        }
    }
}
