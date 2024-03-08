using Nitrogen_FrontEnd.Models;
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
    /// Interaction logic for EquipmentView.xaml
    /// </summary>
    public partial class SingleEquipmentView : Page
    {
        private DatabaseService databaseService;
        public SqlConnection sqlConnection = new SqlConnection("Server=JAA-WIN10DEV-VM;Database=NitrogenDB;User Id=sa;Password=alpha;");
        private int EquipmentId;
        private Equipment equipment;

        public SingleEquipmentView(int equipmentId)
        {
            InitializeComponent();

            EquipmentId = equipmentId;

            databaseService = new DatabaseService("Server=JAA-WIN10DEV-VM;Database=NitrogenDB;User Id=sa;Password=alpha;");
            equipment = databaseService.GetSingleEquipmentById(EquipmentId);

            ShowEquipment();
        }

        private void ShowEquipment()
        {
            equipmentCard.DataContext = equipment;
        }

        private void ViewEquipmentFamily_Click(object sender, RoutedEventArgs e)
        {
            EquipmentFamilyView equipmentFamilyView = new EquipmentFamilyView(equipment.EquipmentId.ToString(), equipment.ProjectNumber);
            NavigationService.Navigate(equipmentFamilyView);
        }
    }
}
