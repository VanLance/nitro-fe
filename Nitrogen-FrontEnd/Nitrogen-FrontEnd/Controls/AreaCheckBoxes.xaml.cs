using Microsoft.Office.Interop.Excel;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using CheckBox = System.Windows.Controls.CheckBox;

namespace Nitrogen_FrontEnd.Controls
{
    /// <summary>
    /// Interaction logic for AreaCheckBoxes.xaml
    /// </summary>
    public partial class AreaCheckBoxes : UserControl
    {
        ExcelReader ExcelReader;
        StackPanel StackPanel;

        public AreaCheckBoxes(ExcelReader excelReader, StackPanel stackPanel)
        {
            ExcelReader = excelReader;
            StackPanel = stackPanel;
            InitializeComponent();
            LoadAreaCheckBoxes();
        }

        public void LoadAreaCheckBoxes()
        {
            foreach (string area in ExcelReader.SelectedAreas.Keys)
            {
                CheckBox checkBox = new CheckBox()
                {
                    Content = area,
                };
                AreaCheckBoxStackPanel.Children.Add(checkBox);
            }
        }

        public void UpdateSelectedAreas_Click(object sender, RoutedEventArgs e)
        {
            foreach( CheckBox checkBox in AreaCheckBoxStackPanel.Children)
            {
                ExcelReader.SelectedAreas[checkBox.Content.ToString()] = checkBox.IsChecked ?? false;
            }
            ExcelReader.ReadExcelFile();
            StackPanel.Children.Remove(this);
        }
    }
}
