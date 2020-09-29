using Syncfusion.Windows.Tools.Controls;
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

namespace Build_Run
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

        //Setting the docking state for the build operation
        void Build_Click(object sender, RoutedEventArgs e)
        {
            DockingManager.SetState(Output, DockState.Float);
        }

        //Setting the docking state for the start run operation
        void Start_Click(object sender, RoutedEventArgs e)
        {
            DockingManager.SetState(SolutionExplorer, DockState.AutoHidden);
            DockingManager.SetState(Output, DockState.Hidden);
        }

        //Setting the docking state for the stop run operation
        void Stop_Click(object sender, RoutedEventArgs e)
        {
            DockingManager.SetState(SolutionExplorer, DockState.Dock);
            DockingManager.SetSideInDockedMode(SolutionExplorer, DockSide.Right);
            DockingManager.SetDesiredWidthInDockedMode(SolutionExplorer, 200);
            DockingManager.SetState(Output, DockState.Dock);
            DockingManager.SetTargetNameInDockedMode(Output, "FindResults");
            DockingManager.SetSideInDockedMode(Output, DockSide.Tabbed);
        }

    }
}
