# Syncfusion WPF DockingManager Samples

A collection of focused WPF examples showcasing Syncfusion DockingManager and DocumentContainer behaviors: dock hint customization, tab/document reordering control, layout persistence, auto-hide window dragging, and custom dock item headers.

## Features Demonstrated

- Dock hints interception and customization at drag time (PreviewDockHints → ICommand → DockAbility)
- Restricting tabbed tool window and document tab reordering; inspecting OldIndex/NewIndex and source/target groups
- TDI (Tabbed Document Interface) with DocumentContainer and reorder notifications
- Layout persistence with XML files and VS-like mode switching (Edit/Run)
- Custom dock item headers (icon/title) via DataTemplates

## About the Samples

- AutoHideWindow-Dragging: WPF scaffold for auto-hide pane dragging.
- Build-Run: Save/Load XML layouts to switch between Edit and Run modes; includes helpers (GetSavedWindowList, FindMissingChidren) and save-on-close.
- Custom-ContextMenu: WPF scaffold targeting context menu customization scenarios.
- DockItem Header: App scaffold for demonstrating dock item header customization.
- DocumentTabOrdering: Cancel reordering in DocumentTabOrderChanging; inspect OldIndex/NewIndex and Source/TargetTabGroup in DocumentTabOrderChanged.
- PreviewDockHints: EventToCommandBehavior forwards PreviewDockHints to a command; UpdateDockingHints modifies DockAbility by DraggingTarget.Name.
- TabbedWindowOrdering: Project scaffolding for reordering control of tool/document tabs.
