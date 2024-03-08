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
    /// Interaction logic for EquipmentFamilyView.xaml
    /// </summary>
    public partial class EquipmentFamilyView : Page
    {
        private DatabaseService databaseService;
        public SqlConnection sqlConnection = new SqlConnection("Server=JAA-WIN10DEV-VM;Database=NitrogenDB;User Id=sa;Password=alpha;");
        private string ProjectNumber;
        private string EquipmentId;

        public EquipmentFamilyView(string equipmentId, string projectNumber)
        {
            InitializeComponent();

            databaseService = new DatabaseService("Server=JAA-WIN10DEV-VM;Database=NitrogenDB;User Id=sa;Password=alpha;");
            ProjectNumber = projectNumber;
            EquipmentId = equipmentId;

            ShowEquipmentFamily();
        }

        public void ShowEquipmentFamily()
        {
            try
            {
                var equipmentDataList = databaseService.GetEquipmentFamily(EquipmentId, ProjectNumber);

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
            SingleEquipmentView singleEquipmentView = new SingleEquipmentView((int)equipmentList.SelectedValue);
            NavigationService.Navigate(singleEquipmentView);
        }
    }
}
