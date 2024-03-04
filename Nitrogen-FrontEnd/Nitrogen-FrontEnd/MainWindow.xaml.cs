using Nitrogen_FrontEnd.Views;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Nitrogen_FrontEnd
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void ViewProjects_Click(object sender, RoutedEventArgs e)
        {
            ProjectsView projectsView = new ProjectsView();
            projectsView.Show();
            this.Close();
        }

        private void AddDataFromExcel_Click(object sender, RoutedEventArgs e)
        {
            AddDataFromExcel addDataFromExcelView = new AddDataFromExcel();
            addDataFromExcelView.Show();
            this.Close();
        }
    }
}
