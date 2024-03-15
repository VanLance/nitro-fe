using System;
using System.Windows;
using System.Windows.Controls;
using Nitrogen_FrontEnd.Models;
using Nitrogen_FrontEnd.Services;
using Nitrogen_FrontEnd.Services.DatabaseService;

namespace Nitrogen_FrontEnd.Utilities
{
    public static class EquipmentUpdater
    {

        public static void UpdateDatabase(EquipmentService databaseService, DataGrid equipmentGrid)
        {
            object selectedId = equipmentGrid.SelectedValue;
            if (selectedId != null)
            {
                Equipment selectedEquipment = (Equipment)equipmentGrid.SelectedItem;
                databaseService.EditEquipment(selectedEquipment);
                MessageBox.Show("Database Updated");
            }
            else
            {
                MessageBox.Show("Please Select Equipment");
            }
        }

        public static void UpdateExcel(MappingService databaseService, ExcelWriter excelWriter, DataGrid equipmentGrid, EquipSheetFormat sheetFormat)
        {
            object selectedId = equipmentGrid.SelectedValue;
            if (selectedId != null)
            {
                Equipment selectedEquipment = (Equipment)equipmentGrid.SelectedItem;
   
                EquipDbFieldToExcelColumnMap equipmentMap = databaseService.GetEquipDbToExcelMapById(sheetFormat.EquipDbFieldToExcelColumnMapId);
                excelWriter.WriteDataToSingleRow(selectedEquipment, equipmentMap);
            }
            else
            {
                MessageBox.Show("Please Select Equipment");
            }
        }

        public static void UpdateDbAndExcel(EquipmentService equipmentService, DataGrid equipmentGrid, MappingService mappingService, ExcelWriter excelWriter, EquipSheetFormat sheetFormat)
        {
            UpdateDatabase(equipmentService, equipmentGrid);
            UpdateExcel(mappingService, excelWriter, equipmentGrid, sheetFormat);
        }
    }
}
