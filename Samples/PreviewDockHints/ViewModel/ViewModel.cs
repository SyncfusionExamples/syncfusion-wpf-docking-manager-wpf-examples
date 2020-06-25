using Syncfusion.Windows.Shared;
using System;
using System.Windows.Input;

namespace DockingManager_PreviewDockHints_Event
{
    class ViewModel : NotificationObject
    {
        private ICommand previewDockHintsCommand;
        public ICommand PreviewDockHintsCommand
        {
            get
            {
                if (previewDockHintsCommand == null)
                    previewDockHintsCommand = new UpdateDockingHints();
                return previewDockHintsCommand;
            }
            set
            {
                previewDockHintsCommand = value;
            }
        }

        public ViewModel()
        {

        }
    }
}
