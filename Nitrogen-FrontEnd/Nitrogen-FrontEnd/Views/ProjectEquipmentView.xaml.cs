using Nitrogen_FrontEnd.Models;
using Nitrogen_FrontEnd.Services;
using Nitrogen_FrontEnd.Utilities;
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
        private readonly DatabaseService databaseService;
        private readonly ExcelWriter ExcelWriter;
        private EquipSheetFormat sheetFormat;
        private readonly string projectNumber;

        public ProjectEquipmentView(string projectNumber)
        {
            InitializeComponent();

            this.projectNumber = projectNumber;
            databaseService = new DatabaseService("Server=JAA-WIN10DEV-VM;Database=NitrogenDB;User Id=sa;Password=alpha;");
            ExcelWriter = GenerateExcelWriter();
            ShowEquipment();
        }

        private void ShowEquipment()
        {
            try
            {
                var equipmentDataList = databaseService.GetEquipmentForProject(projectNumber); ;

                equipmentList.ItemsSource = equipmentDataList;
                equipmentList.SelectedValuePath = "Id";
                equipmentList.AutoGenerateColumns = true;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }
        }

        private ExcelWriter GenerateExcelWriter()
        {

            Project project = databaseService.GetProjectByProjectNumber(projectNumber);
            sheetFormat = databaseService.GetSheetFormatById(project.EquipSheetFormatId);
            return new ExcelWriter(sheetFormat.FileName);
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

        public void UpdateDb_Click(object sender, RoutedEventArgs e)
        {
            EquipmentUpdater.UpdateDatabase(databaseService, equipmentList);
        }

        public void UpdateExcel_Click(object sender, RoutedEventArgs e)
        {
            EquipmentUpdater.UpdateExcel(databaseService, ExcelWriter, equipmentList, sheetFormat);
        }
    }
}
