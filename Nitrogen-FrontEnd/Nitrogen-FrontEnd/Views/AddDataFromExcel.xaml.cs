using Microsoft.Win32;
using System;
using System.Collections.Generic;
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
    /// Interaction logic for AddDataFromExcel.xaml
    /// </summary>
    public partial class AddDataFromExcel : Window
    {
        ExcelService excelService;
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

        public void HandleImportBtnClick(object sender, RoutedEventArgs e)
        {
            if (txtFilePath.Text != null)
            {
                excelService = new ExcelService(txtFilePath.Text);
                excelService.ReadExcelFile();
            }
            else
            {
                MessageBox.Show("Please Select Excelsheet Filepath");
            }
        }
    }
}
