using Syncfusion.Windows.Shared;
using Syncfusion.Windows.Tools.Controls;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows;
using System.Windows.Controls;
using System.Xml;
using System.IO;

namespace Build_Run
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //String variables that storing the default and current run and edit mode layout XML file path
        string currentEditLayout = @"Layouts/CurrentEditLayout.xml";
        string defaultEditLayout = @"Layouts/DefaultEditLayout.xml";
        string currentRunLayout = @"Layouts/CurrentRunLayout.xml";
        string defaultRunLayout = @"Layouts/DefaultRunLayout.xml";

        /// <summary>
        /// Enum for active mode
        /// </summary>
        public enum VisualStudioMode
        {
            EditMode,
            RunMode
        }

        /// <summary>
        /// Gets or sets the current active mode
        /// </summary>
        public VisualStudioMode CurrentMode { get; set; }

        //Gets or sets to bool value to load the default layout
        // public bool IsEnableResetLayout { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            //Invoking the docking manager loaded event
            dockingManager.Loaded += OnLoading;

            //Invoking the window closing event
            this.Closing += OnClosing;

            //Invoking the reset layout button click event
            resetLayout.Click += OnResetLayoutClicked;

            //Invoking the Stop and Run mode switch button click event
            runButton.Click += OnRunButtonClicked;
        }

        /// <summary>
        /// Get the layout windows from the saved XML file.
        /// </summary>
        /// <param name="layoutPath">Path of the loading XML file</param>
        /// <returns></returns>
        private List<string> GetSavedWindowList(string layoutPath)
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
        /// Check and get the missed windows list
        /// </summary>
        /// <param name="contentControl">Instance of DockingManager</param>
        /// <param name="savedControlList">List of windows name from saved layouts</param>
        private List<string> FindMissingChidren(DockingManager contentControl, List<string> savedControlList)
        {
            var missedChildrens = new List<string>();
            if (contentControl != null && savedControlList != null)
            {
                foreach (string savedChild in savedControlList)
                {
                    foreach (FrameworkElement element in contentControl.Children)
                    {
                        if (element.Name == savedChild)
                        {
                            break;
                        }
                    }
                    missedChildrens.Add(savedChild);
                }
            }
            return missedChildrens;
        }

        /// <summary>
        /// Adding the missed windows that is not available in DockingManager
        /// </summary>
        /// <param name="missedChildrens">It contains the missed children list.</param>
        private void AddMissedChildrensIntoDockingManager(List<string> missedChildrens)
        {
            if (missedChildrens.Count > 0)
            {
                foreach (string children in missedChildrens)
                {
                    ContentControl dummyChild = new ContentControl();
                    dummyChild.Name = children;
                    dockingManager.Children.Add(dummyChild);
                }
            }
        }

        //Save the Previous mode layout and loads the current mode layout
        private void Switch(VisualStudioMode mode)
        {
            string currentLayoutPath;
            string defaultLayoutPath;
            if(mode == VisualStudioMode.EditMode)
            {
                currentLayoutPath = currentEditLayout;
                defaultLayoutPath = defaultEditLayout;
            }
            else
            {
                currentLayoutPath = currentRunLayout;
                defaultLayoutPath = defaultRunLayout;
            }

            XmlDocument document = new XmlDocument();
            document.Load(currentLayoutPath);

            if (document.ChildNodes[1].ChildNodes.Count < 1)
            {
                //Load the default layout
                Load(defaultLayoutPath);

                //Save the default layout as current layout
                Save(currentLayoutPath);
            }
            else
            {
                //Load currently saved layout
                Load(currentLayoutPath);
            }
        }

        /// <summary>
        /// To save the previous mode layout
        /// </summary>
        /// <param name="saveLayoutPath">Path of the saving layout file</param>
        private void Save(string saveLayoutPath)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            this.dockingManager.SaveDockState(formatter, StorageFormat.Xml, saveLayoutPath);
        }

        /// <summary>
        /// Load the current mode layout
        /// </summary>
        /// <param name="loadLayoutPath">Path of the loading layout path</param>
        private void Load(string loadLayoutPath)
        {
            BinaryFormatter formatter = new BinaryFormatter();

            //Check and load the missed windows from saved layout
            if (!dockingManager.LoadDockState(loadLayoutPath))
            {
                var savedWindows = GetSavedWindowList(loadLayoutPath);
                var missingChildren = FindMissingChidren(dockingManager, savedWindows);
                AddMissedChildrensIntoDockingManager(missingChildren);
            }
            this.dockingManager.LoadDockState(formatter, StorageFormat.Xml, loadLayoutPath);
        }

        //Based on the mode, set the save and load current layout file path
        private void OnRunButtonClicked(object sender, RoutedEventArgs e)
        {
            string layout_Header = (sender as MenuItem).Header.ToString();

            //Saving the current Edit mode layout and loading the Run mode layout
            if (layout_Header == "Run")
            {
                (sender as MenuItem).Header = "Stop";
                CurrentMode = VisualStudioMode.RunMode;
                Save(currentEditLayout);
                Switch(CurrentMode);
            }

            //Saving the current Run mode layout and loading the Edit mode layout
            else if (layout_Header == "Stop")
            {
                (sender as MenuItem).Header = "Run";
                CurrentMode = VisualStudioMode.EditMode;
                Save(currentRunLayout);
                Switch(CurrentMode);
            }
        }

        /// <summary>
        /// Replace the default layout to current layout when "Reset Layout" menu item is clicked. 
        /// </summary>
        /// <param name="currentLayoutPath">The current active mode layout path.</param>
        /// <param name="defaultLayoutPath">The default layout path of the active mode.</param>
        public void ResetToDefaultLayout(string currentLayoutPath, string defaultLayoutPath)
        {
            XmlDocument document = new XmlDocument();
            document.Load(defaultLayoutPath);

            //Save the default layout into Current mode layout file 
            document.Save(currentLayoutPath);
        }

        //Reset the current layout to default layout
        private void OnResetLayoutClicked(object sender, RoutedEventArgs e)
        {
            string currentLayout;

            //Resetting the current edit layout with default edit layout.
            ResetToDefaultLayout(currentEditLayout, defaultEditLayout);

            //Resetting the current run layout with default run layout.
            ResetToDefaultLayout(currentRunLayout, defaultRunLayout);

            if (CurrentMode == VisualStudioMode.RunMode)
            {
                currentLayout = currentRunLayout;
            }
            else
            {
                currentLayout = currentEditLayout;
            }

            Load(currentLayout);
        }

        //Check and load the currently saved Edit mode layout
        private void OnLoading(object sender, RoutedEventArgs e)
        {
            Switch(CurrentMode);
        }

        //Saving the current mode layout
        private void OnClosing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            string layoutPath;

            //Save the current active mode layout while closing the application
            if (CurrentMode == VisualStudioMode.EditMode)
            {
                layoutPath = currentEditLayout;
            }
            else
            {
                layoutPath = currentRunLayout;
            }

            Save(layoutPath);
        }
    }
}
