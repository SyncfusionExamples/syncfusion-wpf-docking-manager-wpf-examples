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

namespace DocumentTabOrdering
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

        

        private void documentcontainer1_DocumentTabOrderChanged(object sender, Syncfusion.Windows.Tools.Controls.DocumentTabOrderChangedEventArgs e)
        {
            var drag_Drop_Item = e.TargetTabGroup;

            //Get the old and new index of the SourceTabItem
            var oldIndex = e.OldIndex;
            var newIndex = e.NewIndex;

            //Get the old and new tab group of the SourceTabItem
            var sourceTabGroup = e.SourceTabGroup;
            var targetTabGroup = e.TargetTabGroup;
        }

        private void documentcontainer1_DocumentTabOrderChanging(object sender, Syncfusion.Windows.Tools.Controls.DocumentTabOrderChangingEventArgs e)
        {
            // Restrict the TDI item re-ordering
            e.Cancel = true;
        }
    }
}
