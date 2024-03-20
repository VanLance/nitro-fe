using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

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

        private void Search_Click(object sender, RoutedEventArgs e)
        {
            NavigateToPage("Views/SearchView.xaml");
        }

        private void NavigateToPage(string pageName)
        {
            Uri uri = new Uri(pageName, UriKind.Relative);
            NavigationService?.Navigate(uri);
        }
    }
}
