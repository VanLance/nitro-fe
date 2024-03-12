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
    /// Interaction logic for EquipmentFamilyView.xaml
    /// </summary>
    public partial class EquipmentFamilyView : Page
    {
        private DatabaseService databaseService;
        public SqlConnection sqlConnection = new SqlConnection("Server=JAA-WIN10DEV-VM;Database=NitrogenDB;User Id=sa;Password=alpha;");
        private ExcelWriter ExcelWriter;
        private EquipSheetFormat SheetFormat;
        private string ProjectNumber;
        private string EquipmentId;

        public EquipmentFamilyView(string equipmentId, string projectNumber)
        {
            InitializeComponent();

            databaseService = new DatabaseService("Server=JAA-WIN10DEV-VM;Database=NitrogenDB;User Id=sa;Password=alpha;");
            ProjectNumber = projectNumber;
            EquipmentId = equipmentId;
            ExcelWriter = GenerateExcelWriter();

            ShowEquipmentFamily();
        }

        public void ShowEquipmentFamily()
        {
            try
            {
                List<Equipment> equipmentDataList = databaseService.GetEquipmentFamily(EquipmentId, ProjectNumber);

                equipmentList.ItemsSource = equipmentDataList;
                equipmentList.SelectedValuePath = "EquipmentId";
                equipmentList.AutoGenerateColumns = true;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }
        }

        private ExcelWriter GenerateExcelWriter()
        {
            ExcelWriter excelWriter = null;

            Project project = databaseService.GetProjectByProjectNumber(ProjectNumber);
            SheetFormat = databaseService.GetSheetFormatById(project.EquipSheetFormatId);
            excelWriter = new ExcelWriter(SheetFormat.FileName);
            return excelWriter;
        }

        public void UpdateDb_Click(object sender, RoutedEventArgs e)
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

        public void UpdateExcel_Click(object sender, RoutedEventArgs e)
        {
            object selectedId = equipmentList.SelectedValue;
            if (selectedId != null)
            {
                Equipment selectedEquipment = (Equipment)equipmentList.SelectedItem;
                Console.WriteLine(SheetFormat.EquipDbFieldToExcelColumnMapId.ToString() + " sheet fromat map fk");
                EquipDbFieldToExcelColumnMap equipmentMap = databaseService.GetEquipDbToExcelMapById(SheetFormat.EquipDbFieldToExcelColumnMapId);
                Console.WriteLine(equipmentMap.Id.ToString() + " map pk");
                ExcelWriter.WriteDataToExcelRow(selectedEquipment, equipmentMap);
            }
            else
            {
                MessageBox.Show("Please Select Equipment");
            }
        }

    }
}
