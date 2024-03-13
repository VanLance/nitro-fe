using System.Windows;
using System.Windows.Controls;
using Nitrogen_FrontEnd.Models;
using Nitrogen_FrontEnd.Services;
using Nitrogen_FrontEnd.Services.DatabaseService;

namespace Nitrogen_FrontEnd.Utilities
{
    public static class EquipmentUpdater
    {

        public static void UpdateDatabase(EquipmentService databaseService, DataGrid equipmentList)
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

        public static void UpdateExcel(MappingService databaseService, ExcelWriter excelWriter, DataGrid equipmentList, EquipSheetFormat sheetFormat)
        {
            object selectedId = equipmentList.SelectedValue;
            if (selectedId != null)
            {
                Equipment selectedEquipment = (Equipment)equipmentList.SelectedItem;
                EquipDbFieldToExcelColumnMap equipmentMap = databaseService.GetEquipDbToExcelMapById(sheetFormat.EquipDbFieldToExcelColumnMapId);
                excelWriter.WriteDataToSingleRow(selectedEquipment, equipmentMap);
            }
            else
            {
                MessageBox.Show("Please Select Equipment");
            }
        }
    }
}
