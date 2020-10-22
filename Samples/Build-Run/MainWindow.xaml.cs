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
        /// <summary>
        /// Enum for active mode
        /// </summary>
        public enum ActiveMode
        {
            EditMode,
            RunMode
        }

        /// <summary>
        /// Gets or sets the current active mode
        /// </summary>
        public ActiveMode CurrentMode { get; set; }

        //Gets or sets to bool value to load the default layout
        public bool IsEnableResetLayout { get; set; }

        //Gets or sets the missed child windows
        List<string> MissedChildrens { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            dockingManager.Loaded += DockingManager_Loaded;
            this.Closing += MainWindow_Closing;
        }

        /// <summary>
        /// Get the layout windows from the saved XML file.
        /// </summary>
        /// <param name="layoutPath">Path of the loading XML file</param>
        /// <returns></returns>
        private List<string> OnGetSavedWindowList(string layoutPath)
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
        private void OnFindMissedChidren(DockingManager contentControl, List<string> savedControlList)
        {
            MissedChildrens = new List<string>();
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
                    MissedChildrens.Add(savedChild);
                }
            }
        }

        /// <summary>
        /// Adding the missed windows that is not available in DockingManager
        /// </summary>
        /// <param name="missedChildrens">It contains the missed childrens list.</param>
        private void OnAddMissedChildrensIntoDockingManager(List<string> missedChildrens)
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
        private void OnSavePreviousModeLayoutAndLoadCurrentModeLayout(string saveLayoutPath, string loadLayoutPath)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            this.dockingManager.SaveDockState(formatter, StorageFormat.Xml, saveLayoutPath);

            //Check and load the missed windows from saved layout
            if (!dockingManager.LoadDockState(loadLayoutPath))
            {
                OnFindMissedChidren(dockingManager, OnGetSavedWindowList(loadLayoutPath));
                OnAddMissedChildrensIntoDockingManager(MissedChildrens);
            }
            this.dockingManager.LoadDockState(formatter, StorageFormat.Xml, loadLayoutPath);
        }

        //Based on the mode, set the save and load curren tlayout file path
        private void OnRunButtonClicked(object sender, RoutedEventArgs e)
        {
            string saveLayoutPath = string.Empty;
            string loadLayoutPath = string.Empty;
            string layout_Header = (sender as MenuItem).Header.ToString();
            BinaryFormatter formatter = new BinaryFormatter();

            //Saving the current Edit mode layout and loading the Run mode layout
            if (layout_Header == "Run")
            {
                CurrentMode = ActiveMode.RunMode;
                (sender as MenuItem).Header = "Stop";
                saveLayoutPath = @"Layouts/CurrentEditLayout.xml";
                try
                {
                    loadLayoutPath = @"Layouts/CurrentRunLayout.xml";
                    XmlDocument document = new XmlDocument();
                    document.Load(loadLayoutPath);
                }
                catch (XmlException exc)
                {
                    loadLayoutPath = @"Layouts/DefaultRunLayout.xml";
                }
            }

            //Saving the current Run mode layout and loading the Edit mode layout
            else if (layout_Header == "Stop")
            {
                CurrentMode = ActiveMode.EditMode;
                saveLayoutPath = @"Layouts/CurrentRunLayout.xml";

                this.dockingManager.SaveDockState(formatter, StorageFormat.Xml, saveLayoutPath);
                (sender as MenuItem).Header = "Run";
                loadLayoutPath = @"Layouts/CurrentEditLayout.xml";
            }

            //Passing path of previous mode layout and current mode layout
            OnSavePreviousModeLayoutAndLoadCurrentModeLayout(saveLayoutPath, loadLayoutPath);
        }

        /// <summary>
        /// Replace the default layout to current layout when "Reset Layout" menu item is clicked. 
        /// </summary>
        /// <param name="currentLayoutPath">The current active mode layout path.</param>
        /// <param name="defaultLayoutPath">The default layout path of the active mode.</param>
        public void OnResetToDefaultLayout(string currentLayoutPath, string defaultLayoutPath)
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
            string defaultLayout;
            BinaryFormatter formatter = new BinaryFormatter();
            IsEnableResetLayout = true;

            //Resetting the current edit layout with default edit layout.
            currentLayout = @"Layouts/CurrentEditLayout.xml";
            defaultLayout = @"Layouts/DefaultEditLayout.xml";
            OnResetToDefaultLayout(currentLayout, defaultLayout);

            //Resetting the current run layout with default run layout.
            currentLayout = @"Layouts/CurrentRunLayout.xml";
            defaultLayout = @"Layouts/DefaultRunLayout.xml";
            OnResetToDefaultLayout(currentLayout, defaultLayout);

            if (CurrentMode == ActiveMode.RunMode)
            {
                currentLayout = @"Layouts/CurrentRunLayout.xml";
            }
            else
            {
                currentLayout = @"Layouts/CurrentEditLayout.xml";
            }

            if (!this.dockingManager.LoadDockState(currentLayout))
            {
                OnFindMissedChidren(dockingManager, OnGetSavedWindowList(currentLayout));
                OnAddMissedChildrensIntoDockingManager(MissedChildrens);
            }
            this.dockingManager.LoadDockState(formatter, StorageFormat.Xml, currentLayout);
        }

        //Check and load the currently saved Edit mode layout
        private void DockingManager_Loaded(object sender, RoutedEventArgs e)
        {
            string currentEditLayoutpath = @"Layouts/CurrentEditLayout.xml";
            string defaultEditLayoutpath = @"Layouts/DefaultEditLayout.xml";
            BinaryFormatter formatter = new BinaryFormatter();

            //Check and load the currently saved Edit mode layout
            if (File.Exists(currentEditLayoutpath))
            {
                try
                {
                    XmlDocument document = new XmlDocument();
                    document.Load(currentEditLayoutpath);
                    //Check and load the missed windows from saved layout
                    if (!dockingManager.LoadDockState(currentEditLayoutpath))
                    {
                        OnFindMissedChidren(dockingManager, OnGetSavedWindowList(currentEditLayoutpath));
                        OnAddMissedChildrensIntoDockingManager(MissedChildrens);
                    }
                    this.dockingManager.LoadDockState(formatter, StorageFormat.Xml, currentEditLayoutpath);
                }
                catch (XmlException exc)
                {
                    //Check and load the missed windows from default layout
                    if (!dockingManager.LoadDockState(defaultEditLayoutpath))
                    {
                        OnFindMissedChidren(dockingManager, OnGetSavedWindowList(defaultEditLayoutpath));
                        OnAddMissedChildrensIntoDockingManager(MissedChildrens);
                    }
                    //Loading the last Edit mode saved layout
                    this.dockingManager.LoadDockState(formatter, StorageFormat.Xml, defaultEditLayoutpath);

                    //Save the current Edit mode layout
                    this.dockingManager.SaveDockState(formatter, StorageFormat.Xml, currentEditLayoutpath);
                }
            }
            else
            {
                //Check and load the missed windows from default layout
                if (!dockingManager.LoadDockState(defaultEditLayoutpath))
                {
                    OnFindMissedChidren(dockingManager, OnGetSavedWindowList(defaultEditLayoutpath));
                    OnAddMissedChildrensIntoDockingManager(MissedChildrens);
                }
                //Loading the default Edit mode layout
                this.dockingManager.LoadDockState(formatter, StorageFormat.Xml, defaultEditLayoutpath);
            }
        }

        //Saving the current mode layout
        private void MainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            string layoutPath = string.Empty;
            BinaryFormatter formatter = new BinaryFormatter();

            //Save the current active mode layout while closing the application
            if (CurrentMode == ActiveMode.EditMode)
            {
                layoutPath = @"Layouts/CurrentEditLayout.xml";
            }
            else
            {
                layoutPath = @"Layouts/CurrentRunLayout.xml";
            }
            this.dockingManager.SaveDockState(formatter, StorageFormat.Xml, layoutPath);
        }

    }

        
}
