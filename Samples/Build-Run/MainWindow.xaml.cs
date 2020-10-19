using Syncfusion.Windows.Shared;
using Syncfusion.Windows.Tools.Controls;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows;
using System.Windows.Controls;
using System.Xml;

namespace Build_Run
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        bool isResetLayout = false;
        bool isRunModeEnabled = false;
        BinaryFormatter formatter = new BinaryFormatter();
        string path = string.Empty;
        public MainWindow()
        {
            InitializeComponent();
            dockingManager.Loaded += DockingManager_Loaded;
            this.Closing += MainWindow_Closing;            
        }

        private void DockingManager_Loaded(object sender, RoutedEventArgs e)
        {
            path = @"Layouts/CurrentLayout.xml";

            ///Check and load the missed elements from saved layout
            if (!dockingManager.LoadDockState(path))
            {
                FindAndAddMissedChidren(dockingManager, GetSavedControlList(path));
            }           

            //Loading the finally saved layout
            this.dockingManager.LoadDockState(formatter, StorageFormat.Xml, @"Layouts/CurrentLayout.xml");
        }

        private void MainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            //Save the layout when current mode is not a run mode.
            if(!isRunModeEnabled)
                this.dockingManager.SaveDockState(formatter, StorageFormat.Xml, @"Layouts/CurrentLayout.xml");
        }

        private void layout_Click(object sender, RoutedEventArgs e)
        {     
            string layout_Header = (sender as MenuItem).Header.ToString();

            // Saving the current state and loading the running state
            if (layout_Header == "Run")
            {
                (sender as MenuItem).Header = "Stop";
                this.dockingManager.SaveDockState(formatter, StorageFormat.Xml, @"Layouts/CurrentLayout.xml");
                if (isResetLayout)
                {
                    path = @"Layouts/DefaultRunLayout.xml";
                }
                else
                {
                    path = @"Layouts/CurrentRunLayout.xml";
                }

                //Check and load the missed elements from saved layout
                if (!dockingManager.LoadDockState(path))
                {
                    FindAndAddMissedChidren(dockingManager, GetSavedControlList(path));
                }
                this.dockingManager.LoadDockState(formatter, StorageFormat.Xml, path);
                isRunModeEnabled = true;

            }

            //Saving the current running state and loading the normal state
            else if (layout_Header == "Stop")
            {
                this.dockingManager.SaveDockState(formatter, StorageFormat.Xml, @"Layouts/CurrentRunLayout.xml");
                (sender as MenuItem).Header = "Run";
                if (isResetLayout)
                {
                    path = @"Layouts/DefaultLayout.xml";
                }
                else
                {
                    path = @"Layouts/CurrentLayout.xml";
                }

                //Check and load the missed elements from saved layout
                if (!dockingManager.LoadDockState(path))
                {
                    FindAndAddMissedChidren(dockingManager, GetSavedControlList(path));
                }
                this.dockingManager.LoadDockState(formatter, StorageFormat.Xml, path);
                isRunModeEnabled = false;
            }

            //Loading the Default layout when "Reset Layout" option is clicked. 
            else if (layout_Header == "Reset Layout")
            {
                isResetLayout = true;
                if (isRunModeEnabled)
                {
                    path = @"Layouts/DefaultRunLayout.xml";
                }
                else
                {
                    path = @"Layouts/DefaultLayout.xml";
                }

                //Check and load the missed elements from saved layout
                if (!dockingManager.LoadDockState(path))
                {
                    FindAndAddMissedChidren(dockingManager, GetSavedControlList(path));
                }
                this.dockingManager.LoadDockState(formatter, StorageFormat.Xml, path);
            }
        }

        /// <summary>
        /// Get the layout windows from the saved XML file.
        /// </summary>
        /// <param name="layoutPath">Path of the XML file</param>
        /// <returns></returns>
        private List<string> GetSavedControlList(string layoutPath)
        {
            List<string> savedControlNameList = new List<string>();
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(layoutPath);
            XmlNodeList nodes = xmlDoc.DocumentElement.ChildNodes;
            foreach (XmlNode node in nodes)
            {
                if (node.HasChildNodes)
                {
                    //Adding the windows name to list
                    savedControlNameList.Add(node.SelectSingleNode("Name").InnerText);
                }
            }
            return savedControlNameList;
        }

        /// <summary>
        /// Check and add the missed windows to Docking Manager
        /// </summary>
        /// <param name="contentControl">Instance of current DockingManager</param>
        /// <param name="savedControlList">List of windows name from saved layouts</param>
        protected void FindAndAddMissedChidren(DockingManager contentControl, List<string> savedControlList)
        {
            if (contentControl != null && savedControlList !=null)
            {
                bool isChildrenPresent= false;
                foreach (string savedChild in savedControlList)
                {
                    foreach (FrameworkElement element in contentControl.Children)
                    {
                        if (element.Name == savedChild)
                        {
                            isChildrenPresent = true;
                            break;
                        }
                        else
                        {
                            isChildrenPresent = false;
                        }
                    }
                    //Adding the missed windows if not available in DockingManager
                    if (!isChildrenPresent)
                    {
                        ContentControl dummyChild = new ContentControl();
                        dummyChild.Name = savedChild;
                        dockingManager.Children.Add(dummyChild);
                    }
                }                
            }
        }
    }   
}
