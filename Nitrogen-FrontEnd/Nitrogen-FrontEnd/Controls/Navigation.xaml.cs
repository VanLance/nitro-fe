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

namespace Nitrogen_FrontEnd.Controls
{
    /// <summary>
    /// Interaction logic for Navigation.xaml
    /// </summary>
    public partial class Navigation : UserControl
    {
        public static readonly DependencyProperty NavigationServiceProperty =
            DependencyProperty.Register("NavigationService", typeof(NavigationService), typeof(Navigation));

        public NavigationService NavigationService
        {
            get { return (NavigationService)GetValue(NavigationServiceProperty); }
            set { SetValue(NavigationServiceProperty, value); }
        }
        public Navigation()
        {
            InitializeComponent();
        }

        private void AddData_Click(object sender, RoutedEventArgs e)
        {
            NavigateToPage("Views/AddDataFromExcel.xaml");
        }

        private void Projects_Click(object sender, RoutedEventArgs e)
        {
            NavigateToPage("Views/ProjectsView.xaml");
        }

        private void NavigateToPage(string pageName)
        {
            Uri uri = new Uri(pageName, UriKind.Relative);
            NavigationService?.Navigate(uri);
        }
    }
}
