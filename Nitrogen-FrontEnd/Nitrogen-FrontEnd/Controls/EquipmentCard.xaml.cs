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
    /// Interaction logic for EquipmentCard.xaml
    /// </summary>
    public partial class EquipmentCard : UserControl
    {
        public static readonly DependencyProperty EquipmentIdProperty =
            DependencyProperty.Register("EquipmentId", typeof(string), typeof(EquipmentCard));

        public static readonly DependencyProperty ProjectNumberProperty =
            DependencyProperty.Register("ProjectNumber", typeof(string), typeof(EquipmentCard));

        public static readonly DependencyProperty AreaProperty =
            DependencyProperty.Register("Area", typeof(string), typeof(EquipmentCard));

        public static readonly DependencyProperty EquipmentSubIdProperty =
            DependencyProperty.Register("EquipmentSubId", typeof(string), typeof(EquipmentCard));

        public static readonly DependencyProperty DescriptionProperty =
            DependencyProperty.Register("Description", typeof(string), typeof(EquipmentCard));

        public static readonly DependencyProperty ControlPanelProperty =
            DependencyProperty.Register("ControlPanel", typeof(string), typeof(EquipmentCard));

        public string EquipmentId
        {
            get { return (string)GetValue(EquipmentIdProperty); }
            set { SetValue(EquipmentIdProperty, value); }
        }

        public string ProjectNumber
        {
            get { return (string)GetValue(ProjectNumberProperty); }
            set { SetValue(ProjectNumberProperty, value); }
        }

        public string Area
        {
            get { return (string)GetValue(AreaProperty); }
            set { SetValue(AreaProperty, value); }
        }

        public string EquipmentSubId
        {
            get { return (string)GetValue(EquipmentSubIdProperty); }
            set { SetValue(EquipmentSubIdProperty, value); }
        }

        public string Description
        {
            get { return (string)GetValue(DescriptionProperty); }
            set { SetValue(DescriptionProperty, value); }
        }

        public string ControlPanel
        {
            get { return (string)GetValue(ControlPanelProperty); }
            set { SetValue(ControlPanelProperty, value); }
        }

        public EquipmentCard()
        {
            InitializeComponent();
            DataContext = this;
        }
    }
}
