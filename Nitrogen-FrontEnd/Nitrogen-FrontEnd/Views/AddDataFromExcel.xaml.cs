﻿using Microsoft.Win32;
using Nitrogen_FrontEnd.Controls;
using Nitrogen_FrontEnd.Services.ExcelService.IO;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Page = System.Windows.Controls.Page;

namespace Nitrogen_FrontEnd.Views
{
    /// <summary>
    /// Interaction logic for AddDataFromExcel.xaml
    /// </summary>
    public partial class AddDataFromExcel : Page
    {
        ExcelReader ExcelReader;
        AreaCheckBoxes areaCheckBoxes;

        public AddDataFromExcel()
        {
            InitializeComponent();
        }

        public void HandleBrowseBtnClick(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Excel Files|*.xls;*.xlsx";
            if (openFileDialog.ShowDialog() == true)
            {
                txtFilePath.Text = openFileDialog.FileName;
            }
        }

        public void HandleSelectBtnClick(object sender, RoutedEventArgs e)
        {
            if (txtFilePath.Text != "")
            {
                if (fileTypeComboBox.SelectedItem != null)
                {
                    string selectedFileType = (fileTypeComboBox.SelectedItem as ComboBoxItem).Content.ToString();
                    switch (selectedFileType)
                    {
                        case "Equipment":
                            ReadEquipmentSpreadsheet();
                            break;
                        case "IO":
                            ReadIoSpreadsheet();
                            break;
                    }
                }
                else
                {
                    MessageBox.Show("Please Select File Type");
                }
            }
            else
            {
                MessageBox.Show("Please Select Excelsheet Filepath");
            }
        }

        private void ReadIoSpreadsheet()
        {
            IoExcelReader ioExcelReader = new IoExcelReader(txtFilePath.Text);

            ioExcelReader.ReadExcelFile();
        }

        public void ReadEquipmentSpreadsheet()
        {
            ExcelReader = new ExcelReader(txtFilePath.Text);

            AreaCheckBoxes existingCheckBoxes = stackPanel.Children.OfType<AreaCheckBoxes>().FirstOrDefault();
            if (existingCheckBoxes != null) stackPanel.Children.Remove(existingCheckBoxes);

            areaCheckBoxes = new AreaCheckBoxes(ExcelReader, stackPanel);
            stackPanel.Children.Add(areaCheckBoxes);
        }
    }
}
