using Syncfusion.Windows.Shared;
using Syncfusion.Windows.Tools.Controls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfApp2
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

    public class DockViewModel : NotificationObject
    {
        public DockViewModel()
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

            DockItemCollection.Add(new DockItem() { Header = new Header { DocumentTitle = "ToolBox", DocumentIsModified = true}, Name = "tool", State = DockState.Dock, DesiredWidthInDockedMode = 300d, HeaderTemplate = (DataTemplate)Application.Current.FindResource("HeaderTemplate") });

            DockItemCollection.Add(new DockItem() { Header = new Header { DocumentTitle = "Integration", DocumentIsModified = true }, State = DockState.Dock, HeaderTemplate = (DataTemplate)Application.Current.FindResource("HeaderTemplate") });

            DockItemCollection.Add(new DockItem() { Header = "Features", State = DockState.Dock});

            DockItemCollection.Add(new DockItem() { Header = "Docking", State = DockState.Dock });

            
        }
    }
}
