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

namespace TabbedWindowOrdering
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

        private void dockingmanager1_TabOrderChanging(object sender, Syncfusion.Windows.Tools.Controls.TabOrderChangingEventArgs e)
        {
            // Restrict the Tab item order changing
            e.Cancel = true;
        }

        private void dockingmanager1_TabOrderChanged(object sender, Syncfusion.Windows.Tools.Controls.TabOrderChangedEventArgs e)
        {
            var dragged_Item = e.TargetItem;
            var oldIndex = e.OldIndex;
            var newIndex = e.NewIndex;
        }

        private void dockingmanager1_DocumentTabOrderChanging(object sender, Syncfusion.Windows.Tools.Controls.DocumentTabOrderChangingEventArgs e)
        {
            // Restrict the Tab item order changing
            e.Cancel = true;
        }

        private void dockingmanager1_DocumentTabOrderChanged(object sender, Syncfusion.Windows.Tools.Controls.DocumentTabOrderChangedEventArgs e)
        {
            var drag_Drop_Item = e.SourceTabItem;

            //Get the old and new index of the SourceTabItem
            var oldIndex = e.OldIndex;
            var newIndex = e.NewIndex;

            //Get the old and new tab group of the SourceTabItem
            var sourceTabGroup = e.SourceTabGroup;
            var targetTabGroup = e.TargetTabGroup;
        }
    }
}
