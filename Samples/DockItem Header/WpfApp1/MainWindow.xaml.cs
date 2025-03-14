using Syncfusion.Windows.Shared;
using Syncfusion.Windows.Tools.Controls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Runtime.Remoting.Messaging;
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

namespace WpfApp1
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

}
    public class Header
    {
        public string DocumentTitle { get; set; }
        public bool DocumentIsModified { get; set; }
    }
    public class ViewModel : NotificationObject
    {

        public ViewModel()
        {
            GenerateDockItemCollection();
        }
        private ObservableCollection<DockItem> dockItems;

        public ObservableCollection<DockItem> DockItemCollection
        {
            get { return dockItems; }
            set { dockItems = value; this.RaisePropertyChanged(nameof(DockItemCollection)); }
        }

        private void GenerateDockItemCollection()
        {

            DockItemCollection = new ObservableCollection<DockItem>();

            DockItem errorlist = new DockItem()
            {
                Content = new ContentControl() { Content = "Item1" },
                Name = "ErrorListWindow",
                Header = new Header { DocumentTitle = "ErrorList", DocumentIsModified = true },
                State = DockState.Dock,
                SideInDockedMode = DockSide.Bottom,
                DesiredHeightInDockedMode = 200,
                HeaderTemplate = (DataTemplate)Application.Current.FindResource("HeaderTemplate"),
            };

            DockItem solutionexplorer = new DockItem()
            {
                Content = new ContentControl() { Content = "Item2" },
                Name = "SolutionExplorerWindow",
                Header = "Explorer",
                State = DockState.Dock,
                SideInDockedMode = DockSide.Right,
                DesiredWidthInDockedMode = 300,
                //HeaderTemplate = (DataTemplate)Application.Current.FindResource("HeaderTemplate"),
            };

            DockItem toolbox = new DockItem()
            {
                Content = new ContentControl() { Content = "Item 3" },
                Name = "ToolboxWindow",
                Header = new Header { DocumentTitle = "Toolbox", DocumentIsModified = true },
                State = DockState.Dock,
                SideInDockedMode = DockSide.Left,
                DesiredWidthInDockedMode = 250,
                HeaderTemplate = (DataTemplate)Application.Current.FindResource("HeaderTemplate"),
            };

            DockItem classview = new DockItem()
            {
                Content = new ContentControl() { Content = "Item4" },
                Name = "ClassViewWindow",
                Header = new Header { DocumentTitle = "Class View", DocumentIsModified = false },
                State = DockState.Dock,
                TargetNameInDockedMode = "SolutionExplorerWindow",
                SideInDockedMode = DockSide.Tabbed,
                HeaderTemplate = (DataTemplate)Application.Current.FindResource("HeaderTemplate"),
            };

            DockItem mainwindow = new DockItem()
            {
                Content = new ContentControl() { Content = "Item4" },
                Name = "MainWindow",
                Header = new Header { DocumentTitle = "Main Window", DocumentIsModified = true },
                State = DockState.Document,
                HeaderTemplate = (DataTemplate)Application.Current.FindResource("HeaderTemplate"),
            };

            DockItem properties = new DockItem()
            {
                Content = new ContentControl() { Content = "Item5" },
                Name = "PropertiesWindow",
                Header = new Header { DocumentTitle = "Properties", DocumentIsModified = false },
                State = DockState.Dock,
                TargetNameInDockedMode = "SolutionExplorerWindow",
                SideInDockedMode = DockSide.Bottom,
                HeaderTemplate = (DataTemplate)Application.Current.FindResource("HeaderTemplate"),
            };


            DockItemCollection.Add(mainwindow);
            DockItemCollection.Add(errorlist);
            DockItemCollection.Add(toolbox);
            DockItemCollection.Add(solutionexplorer);
            DockItemCollection.Add(classview);
            DockItemCollection.Add(properties);
        }
    }

    public class BoolVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool boolValue)
            {
                return boolValue ? Visibility.Visible : Visibility.Collapsed;
            }
            return Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Visibility visibility)
            {
                return visibility == Visibility.Visible;
            }
            return false;
        }
    }
}
