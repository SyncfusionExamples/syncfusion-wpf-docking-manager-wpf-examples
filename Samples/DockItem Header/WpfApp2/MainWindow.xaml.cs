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

            DockItemCollection.Add(new DockItem() { Header = new Header { DocumentIsModified = true, DocumentTitle = "ToolBox"}, Name = "tool", State = DockState.Dock, DesiredWidthInDockedMode = 200d, HeaderTemplate = (DataTemplate)Application.Current.FindResource("HeaderTemplate") });

            DockItemCollection.Add(new DockItem() { Header = "Integration", State = DockState.Document});

            DockItemCollection.Add(new DockItem() { Header = new Header { DocumentIsModified = true, DocumentTitle = "Admin" }, State = DockState.Document, HeaderTemplate = (DataTemplate)Application.Current.FindResource("HeaderTemplate") });

            DockItemCollection.Add(new DockItem() { Header = new Header { DocumentIsModified = true, DocumentTitle = "Docking" }, State = DockState.Document, HeaderTemplate = (DataTemplate)Application.Current.FindResource("HeaderTemplate") });

            DockItemCollection.Add(new DockItem() { Header = new Header { DocumentIsModified = true, DocumentTitle = "Bookmark" }, Name = "solution", State = DockState.Dock, SideInDockedMode = DockSide.Right, DesiredWidthInDockedMode = 300d, HeaderTemplate = (DataTemplate)Application.Current.FindResource("HeaderTemplate") });

            DockItemCollection.Add(new DockItem() { Header = "Properties Window", Name = "Properties", State = DockState.Dock, SideInDockedMode = DockSide.Tabbed, TargetNameInDockedMode = "solution" });

            DockItemCollection.Add(new DockItem() { Header = new Header { DocumentIsModified = true, DocumentTitle = "Clipboard" }, Name = "Output", State = DockState.Dock, SideInDockedMode = DockSide.Bottom, DesiredHeightInDockedMode = 200d, HeaderTemplate = (DataTemplate)Application.Current.FindResource("HeaderTemplate") });

            DockItemCollection.Add(new DockItem() { Header = "Error List", State = DockState.Dock, SideInDockedMode = DockSide.Tabbed, TargetNameInDockedMode = "Output" });

            DockItemCollection.Add(new DockItem() { Header = "Find Symbol Results", State = DockState.Dock, SideInDockedMode = DockSide.Tabbed, TargetNameInDockedMode = "Output" });

            DockItemCollection.Add(new DockItem() { Header = "Find Results", State = DockState.Dock, SideInDockedMode = DockSide.Tabbed, TargetNameInDockedMode = "Output" });


        }
    }
}
