using Syncfusion.Windows.Shared;
using Syncfusion.Windows.Tools.Controls;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows;
using System.Windows.Controls;
using System.Xml;
using System.IO;
using System.Security.RightsManagement;
using System.Linq;

namespace Build_Run
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    { 
        public enum ActiveMode
        {
            EditMode,
            RunMode
        }

        //Gets or sets the current active mode
        public ActiveMode CurrentMode { get; set; }

        //Gets or sets to bool value to load th default layout
        public bool IsEnableResetLayout { get; set; }       

        public MainWindow()
        {
            InitializeComponent();
            dockingManager.Loaded += DockingManager_Loaded;
            this.Closing += MainWindow_Closing;
        }


        /// <summary>
        /// Loading the Default layout when "Reset Layout" meni item is clicked. 
        /// </summary>
        /// <param name="currentLayoutPath">The current active mode layout path.</param>
        /// <param name="defaultLayoutPath">The default layout path of the active mode.</param>
        public void OnResetToDefaultLayout(string currentLayoutPath, string defaultLayoutPath)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            XmlDocument document = new XmlDocument();
            document.Load(defaultLayoutPath);

            //Save the default layout into Current mode layout file 
            document.Save(currentLayoutPath);
            if (!this.dockingManager.LoadDockState(currentLayoutPath))
            {
                FindMissedChidren(dockingManager, GetSavedControlList(currentLayoutPath));
            }
            this.dockingManager.LoadDockState(formatter, StorageFormat.Xml, currentLayoutPath);
        }

        private void CheckAndLoadCUrrentlayout()
        {

        }
        private void DockingManager_Loaded(object sender, RoutedEventArgs e)
        {
            string currentEditLayoutpath = @"Layouts/CurrentEditLayout.xml";
            string defaultEditLayoutpath = @"Layouts/DefaultEditLayout.xml";
            string defaultRunLayoutpath = @"Layouts/DefaultRunLayout.xml";
            string currentRunLayoutpath = @"Layouts/CurrentEditLayout.xml";
            BinaryFormatter formatter = new BinaryFormatter();

            //Check and load the currently saved Edit mode layout
            if (File.Exists(currentEditLayoutpath))
            {
                try
                {
                    XmlDocument document = new XmlDocument();
                    document.Load(currentEditLayoutpath);
                    this.dockingManager.LoadDockState(formatter, StorageFormat.Xml, currentEditLayoutpath);
                }
                catch (XmlException exc)
                {
                    //Loading the last Edit mode saved layout
                    this.dockingManager.LoadDockState(formatter, StorageFormat.Xml, defaultEditLayoutpath);

                    //Save the current Edit mode layout
                    this.dockingManager.SaveDockState(formatter, StorageFormat.Xml, currentEditLayoutpath);
                }
            }
            else
            {   
                //Loading the default Edit mode layout
                this.dockingManager.LoadDockState(formatter, StorageFormat.Xml, defaultEditLayoutpath);
            }
        }

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

        private void SavePreviousModeLayoutAndLoadCurrentModeLayout(string saveLayoutPath, string loadLayoutPath)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            this.dockingManager.SaveDockState(formatter, StorageFormat.Xml, saveLayoutPath);          

            //Check and load the missed elements from Run mode saved layout
            if (!dockingManager.LoadDockState(loadLayoutPath))
            {
                FindMissedChidren(dockingManager, GetSavedControlList(loadLayoutPath));
            }
            this.dockingManager.LoadDockState(formatter, StorageFormat.Xml, loadLayoutPath);
        }

        private void OnRunButtonClicked(object sender, RoutedEventArgs e)
        {
            string saveLayoutPath = string.Empty;
            string loadLayoutPath = string.Empty;
            string layout_Header = (sender as MenuItem).Header.ToString();
            BinaryFormatter formatter = new BinaryFormatter();

            // Saving the current Edit mode layout and loading the Run mode layout
            if (layout_Header == "Run")
            {                
                CurrentMode = ActiveMode.RunMode;
                (sender as MenuItem).Header = "Stop";
                saveLayoutPath = @"Layouts/CurrentEditLayout.xml";
                if (IsEnableResetLayout)
                {
                    loadLayoutPath = @"Layouts/DefaultRunLayout.xml";
                }
                else
                {
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
            }

            //Saving the current Run mode layout and loading the Edit mode layout
            else if (layout_Header == "Stop")
            {
                CurrentMode = ActiveMode.EditMode;
                saveLayoutPath = @"Layouts/CurrentRunLayout.xml";
              
                this.dockingManager.SaveDockState(formatter, StorageFormat.Xml, saveLayoutPath);
                (sender as MenuItem).Header = "Run";
                if (IsEnableResetLayout)
                {
                    loadLayoutPath = @"Layouts/DefaultEditLayout.xml";
                }
                else
                {
                    loadLayoutPath = @"Layouts/CurrentEditLayout.xml";
                }
            }
            SavePreviousModeLayoutAndLoadCurrentModeLayout(saveLayoutPath, loadLayoutPath);
        }

        //Reset the current layout to default layout
        private void OnResetLayoutClicked(object sender, RoutedEventArgs e)
        {
            string currentLayout;
            string defaultLayout;
            IsEnableResetLayout = true;
            if (CurrentMode == ActiveMode.RunMode)
            {                
                currentLayout = @"Layouts/CurrentRunLayout.xml";
                defaultLayout = @"Layouts/DefaultRunLayout.xml";
            }
            else
            {                
                currentLayout = @"Layouts/CurrentEditLayout.xml";
                defaultLayout = @"Layouts/DefaultEditLayout.xml";                
            }
            OnResetToDefaultLayout(currentLayout, defaultLayout);
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
        protected List<string> FindMissedChidren(DockingManager contentControl, List<string> savedControlList)
        {
            List<string> MissedChildrens = new List<string>();
            if (contentControl != null && savedControlList != null)
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
                    MissedChildrens.Add(savedChild);
                }
            }
            return MissedChildrens;
        }

        /// <summary>
        /// Adding the missed windows that is not available in DockingManager
        /// </summary>
        /// <param name="missedChildrens">It contains the missed childrens list.</param>
        private void AddMissedChildren(List<string> missedChildrens)
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
    }
}
