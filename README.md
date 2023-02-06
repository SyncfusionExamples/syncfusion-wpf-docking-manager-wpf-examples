## DockingManager control samples
This repository contains the samples that demonstrate the functionalities of DockingManager.

<table>
 <tr>
  <td><a href="Samples/AutoHideWindow-Dragging">Dragging AutoHideWindow</a></td>
  <td><a href="Samples/PreviewDockHints">Restrict docking hints</a></td>
  <td><a href="Samples/Build-Run">Build and Run mode like VisualStudio</a></td>
  <td><a href="Samples/TabbedWindowOrdering">Restrict Tabbed and Document window ordering</a></td>
 </tr>
  <tr>
  <td><a href="Samples/TabbedWindowOrdering">Tabbed and Document window re-ordering notification</a></td>
  <td><a href="Samples/DocumentTabOrdering">Restrict TDI window ordering in DocumentContainer</a></td>
  <td><a href="Samples/DocumentTabOrdering">TDI window re-ordering notification in DocumentContainer</a></td>
 </tr>
</table>

## AutoHide Window
AutoHidden window can be placed in four different sides such as Top, Bottom, Left and Right. To place the four auto hidden children in four different sides, set SideInDockedMode property according to its corresponding values in the DockingManager.

The AutoHideWindow can be placed on a required target window through the TargetNameInDockedMode property of the DockingManager. DockingWindow will auto hidden in place according to its Parent position, if any target exist. For example: Here “Output” docked at bottom of “SolutionExplorer” which docked at left side. While auto hiding Output window, it will auto hide at left due to it’s TargetWindow side.

## Custom ContextMenu
A dockable window is well associated with a default context menu with default menu items. You can also add custom menu items to the existing context menu items of the dockable window. CustomMenuItems property is used for this purpose.

CustomMenuItems - This property is attached to a docking manager child, and gives an ability to add some additional menu items to the context menu. This can easily extend GUI functionality by using the Custom menu items.

To add the custom menu item:

## C#
    <syncfusion:DockingManager Name="DockingManager1" >            

	    <syncfusion:DockingManager.CustomMenuItems>                

	    <syncfusion:CustomMenuItemCollection>                    

	    <syncfusion:CustomMenuItem Header="CustomItem" />                

	    </syncfusion:CustomMenuItemCollection>            

	    </syncfusion:DockingManager.CustomMenuItems>            

	    <TextBox syncfusion:DockingManager.SideInDockedMode="Left" syncfusion:DockingManager.State="Dock"/> 

    </syncfusion:DockingManager>

## DocumentTabOrdering
You will be notified when the TDI item’s order is changed by using the DocumentTabOrderChanged event. You can get the order changed TDI window with its old and new index values by using the SourceTabItem, OldIndex and NewIndex properties. You can also get old and new tab group of the order changed item by using the the SourceTabGroup and TargetTabGroup properties.

## Restrict DocumentTabOrdering
If you want to restrict the user to reordering the TDI window by drag and drop operation, use the DocumentTabOrderChanging event and set Cancel property value as true.

## C#
    private void DockingManager_DocumentTabOrderChanging(object sender, DocumentTabOrderChangingEventArgs e)
    {
        // Restrict the TDI window re-ordering
        e.Cancel = true;
    }

## TabbedWindowOrdering
You will be notified when the tabbed window’s order is changed by using the TabOrderChanged event. You can get the order changed tabbed window with its old and new index values by using the TargetItem, OldIndex and NewIndex properties.

## C#
    private void DockingManager1_TabOrderChanged(object sender, TabOrderChangedEventArgs e)
    {
        var dragged_Item = e.TargetItem;
        var oldIndex = e.OldIndex;
        var newIndex = e.NewIndex;
    }

## Restrict Tabbed Windoe ReOrdering
If you want to restrict the user to reordering the tabbed window by drag and drop operation, use the TabOrderChanging event and set Cancel property value as true.

## Restricting DockingHints
You can disabled the dock hints at run-time by handling PreviewDockHints event in the DockingManager. It helps to handle before displaying the dock hints when drag the windows in DockingManager based on mouse hovered window. This event will be triggered for both inner dockability and outer dockability while drag the windows. It receives an argument of type PreviewDockHintsEventArgs containing the following information about the event.

* DraggingSource - Gets or sets the dragging element of DockingManager that raises the PreviewDockHints event.
* DraggingTarget - Gets or sets the target element in which the dragging window of DockingManager to be docked.
* DockAbility - Gets or sets the DockAbility, to restrict docking on target window.
* OuterDockAbility - Gets or sets the OuterDockability, to restrict docking on edges of DockingManager. 