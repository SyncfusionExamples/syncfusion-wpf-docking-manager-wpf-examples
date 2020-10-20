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
            path = @"Layouts/CurrentEditLayout.xml";

            ///Check and load the missed elements from Edit mode saved layout
            if (!dockingManager.LoadDockState(path))
            {
                FindAndAddMissedChidren(dockingManager, GetSavedControlList(path));
            }

            //Loading the last Edit mode saved layout
            this.dockingManager.LoadDockState(formatter, StorageFormat.Xml,
                                              @"Layouts/CurrentEditLayout.xml");
        }

        private void MainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            //Save the Edit mode layout
            if (!isRunModeEnabled)
            {
                this.dockingManager.SaveDockState(formatter, StorageFormat.Xml,
                                                  @"Layouts/CurrentEditLayout.xml");

            }
            else
            {
                this.dockingManager.SaveDockState(formatter, StorageFormat.Xml,
                                                 @"Layouts/CurrentRunLayout.xml");
            }
        }

        private void layout_Click(object sender,
            RoutedEventArgs e)
        {
            string layout_Header = (sender as MenuItem).Header.ToString();

            // Saving the current Edit mode layout and loading the Run mode layout
            if (layout_Header == "Run")
            {
                (sender as MenuItem).Header = "Stop";
                this.dockingManager.SaveDockState(formatter, StorageFormat.Xml,
                                                  @"Layouts/CurrentEditLayout.xml");
                if (isResetLayout)
                {
                    path = @"Layouts/DefaultRunLayout.xml";
                }
                else
                {
                    path = @"Layouts/CurrentRunLayout.xml";
                }

                //Check and load the missed elements from Run mode saved layout
                if (!dockingManager.LoadDockState(path))
                {
                    FindAndAddMissedChidren(dockingManager, GetSavedControlList(path));
                }
                this.dockingManager.LoadDockState(formatter, StorageFormat.Xml, path);
                isRunModeEnabled = true;
            }

            //Saving the current Run mode layout and loading the Edit mode layout
            else if (layout_Header == "Stop")
            {
                this.dockingManager.SaveDockState(formatter, StorageFormat.Xml,
                                                  @"Layouts/CurrentRunLayout.xml");
                (sender as MenuItem).Header = "Run";
                if (isResetLayout)
                {
                    path = @"Layouts/DefaultEditLayout.xml";
                }
                else
                {
                    path = @"Layouts/CurrentEditLayout.xml";
                }

                //Check and load the missed elements from Edit mode saved layout
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
                    path = @"Layouts/DefaultEditLayout.xml";
                }

                //Check and load the missed elements from Default Edit mode saved layout
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
                    //Adding the windows name to the list
                    savedControlNameList.Add(node.SelectSingleNode("Name").InnerText);
                }
            }
            return savedControlNameList;
        }

        /// <summary>
        /// Check and add the missed windows to Docking Manager
        /// </summary>
        /// <param name="contentControl">Instance of DockingManager</param>
        /// <param name="savedControlList">List of windows name from saved layouts</param>
        protected void FindAndAddMissedChidren(DockingManager contentControl, List<string> savedControlList)
        {
            if (contentControl != null && savedControlList != null)
            {
                bool isChildrenPresent = false;
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
                    //Adding the missed windows if currently not available in DockingManager
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
