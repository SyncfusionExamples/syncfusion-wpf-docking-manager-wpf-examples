using Syncfusion.Windows.Shared;
using System;
using System.Windows.Input;

namespace DockingManager_PreviewDockHints_Event
{
    class ViewModel
    {
        private ICommand previewDockHintsCommand;

        public ICommand PreviewDockHintsCommand
        {
            get
            {
                return previewDockHintsCommand;
            }
        }
        public ViewModel()
        {
            previewDockHintsCommand = new DelegateCommand<object>(previewDockHintsChanged);
        }

        private void previewDockHintsChanged(object obj)
        {
            //if (e.DraggingTarget == Output)
            //{
            //    e.DockAbility = DockAbility.Top;
            //    e.OuterDockAbility = OuterDockAbility.Top;
            //}
        }
    }
}
