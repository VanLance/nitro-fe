using Microsoft.Win32;
using Nitrogen_FrontEnd.Controls;
using System.Windows;
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

        public void HandleImportBtnClick(object sender, RoutedEventArgs e)
        {
            if (txtFilePath.Text != "")
            {
                ExcelReader = new ExcelReader(txtFilePath.Text);
                
                areaCheckBoxes = new AreaCheckBoxes(ExcelReader, stackPanel);
                stackPanel.Children.Add(areaCheckBoxes);
            }
            else
            {
                MessageBox.Show("Please Select Excelsheet Filepath");
            }
        }
    }
}
