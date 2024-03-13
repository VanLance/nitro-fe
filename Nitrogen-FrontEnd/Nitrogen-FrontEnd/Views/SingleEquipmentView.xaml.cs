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
        private readonly DatabaseService databaseService;
        private int equipmentId;
        private readonly Equipment equipment;

        public SingleEquipmentView(int equipmentId)
        {
            InitializeComponent();

            this.equipmentId = equipmentId;
            databaseService = new DatabaseService("Server=JAA-WIN10DEV-VM;Database=NitrogenDB;User Id=sa;Password=alpha;");
            equipment = databaseService.GetSingleEquipmentById(equipmentId);

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
