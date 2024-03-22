using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
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
                checkBox.Foreground = Brushes.White;
                AreaCheckBoxStackPanel.Children.Add(checkBox);
            }
        }

        public async void UpdateSelectedAreas_Click(object sender, RoutedEventArgs e)
        {
            foreach (CheckBox checkBox in AreaCheckBoxStackPanel.Children)
            {
                ExcelReader.SelectedAreas[checkBox.Content.ToString()] = checkBox.IsChecked ?? false;
            }
            progressBar.Visibility = Visibility.Visible;

            progressBar.Value = 50;
            await Task.Run(() => ExcelReader.ReadExcelFile(UpdateProgressBar, UpdateProgressLabel));

            StackPanel.Children.Remove(this);

            progressBar.Visibility = Visibility.Collapsed;
        }

        public void UpdateProgressBar(double progressValue)
        {
            Dispatcher.Invoke(() =>
            {
                progressBar.Value = progressValue;
            });
        }

        public void UpdateProgressLabel(string labelValue)
        {
            Dispatcher.Invoke(() =>
            {
                progressLabel.Text = $"{labelValue} Status";
            });
        }
    }
}
