using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace TouchPortal_Plugin_Helper {
    public partial class DefineCategories : Page {
        public DefineCategories() {
            InitializeComponent();
            try {
                RestoreCategories();
                } catch (Exception) { }
            }

        //Categories input
        //Add button
        private void AddCategoryButton(object sender, RoutedEventArgs e) {
            if (!CategoriesTextBox.Text.ToString().Trim().Equals("")) {
                AddCategory(CategoriesTextBox.Text);
                }
            }

        //Remove button
        private void RemoveCategoryButton(object sender, RoutedEventArgs e) {
            if (CategoriesListBox.SelectedIndex != -1) {
                RemoveCategory(CategoriesListBox.SelectedIndex);
                }
            }

        //EnterKey
        private void CategoriesKeyDown(object sender, KeyEventArgs e) {
            if (e.Key == Key.Return) {
                if (!CategoriesTextBox.Text.ToString().Trim().Equals("")) {
                    AddCategory(CategoriesTextBox.Text);
                    }
                }
            }

        //DeleteKey
        private void CategoriesDeleteKeyDown(object sender, KeyEventArgs e) {
            if (e.Key == Key.Delete || e.Key == Key.Back) {
                try {
                    e.Handled = true;
                    RemoveCategory(CategoriesListBox.SelectedIndex);
                    } catch (Exception) { }
                }
            }

        //Actions input
        //Add button
        private void AddActionButton(object sender, RoutedEventArgs e) {
            if (!ActionsTextBox.Text.ToString().Trim().Equals("")) {
                AddAction(ActionsTextBox.Text);
                }
            }

        //Remove button
        private void RemoveActionButton(object sender, RoutedEventArgs e) {
            if (ActionsListBox.SelectedIndex != -1) {
                RemoveAction(ActionsListBox.SelectedIndex, CategoriesListBox.SelectedIndex);
                }
            }

        //EnterKey
        private void ActionsEnter(object sender, KeyEventArgs e) {
            if (e.Key == Key.Return) {
                if (!ActionsTextBox.Text.ToString().Trim().Equals("")) {
                    AddAction(ActionsTextBox.Text);
                    }
                }
            }

        //DeleteKey
        private void ActionsDeleteKeyDown(object sender, KeyEventArgs e) {
            if ((e.Key == Key.Delete || e.Key == Key.Back) && (ActionsListBox.SelectedIndex != -1)) {
                try {
                    e.Handled = true;
                    RemoveAction(ActionsListBox.SelectedIndex, CategoriesListBox.SelectedIndex);
                    } catch (Exception) { }
                }
            }

        //Events input
        //Add button
        private void AddEventButton(object sender, RoutedEventArgs e) {
            if (!EventsTextBox.Text.ToString().Trim().Equals("")) {
                AddEvent(EventsTextBox.Text);
                }
            }

        //Remove button
        private void RemoveEventButton(object sender, RoutedEventArgs e) {
            if (EventsListBox.SelectedIndex != -1) {
                RemoveEvent(EventsListBox.SelectedIndex, CategoriesListBox.SelectedIndex);
                }
            }

        //EnterKey
        private void EventsEnter(object sender, KeyEventArgs e) {
            if (e.Key == Key.Return) {
                if (!EventsTextBox.Text.ToString().Trim().Equals("")) {
                    AddEvent(EventsTextBox.Text);
                    }
                }
            }

        //DeleteKey
        private void EventsDeleteKeyDown(object sender, KeyEventArgs e) {
            if ((e.Key == Key.Delete || e.Key == Key.Back) && (EventsListBox.SelectedIndex != -1)) {
                try {
                    e.Handled = true;
                    RemoveEvent(EventsListBox.SelectedIndex, CategoriesListBox.SelectedIndex);
                    } catch (Exception) { }
                }
            }

        //States input
        //Add button
        private void AddStateButton(object sender, RoutedEventArgs e) {
            if (!StatesTextBox.Text.ToString().Trim().Equals("")) {
                AddState(StatesTextBox.Text);
                }
            }

        //Remove button
        private void RemoveStateButton(object sender, RoutedEventArgs e) {
            if (StatesListBox.SelectedIndex != -1) {
                RemoveState(StatesListBox.SelectedIndex, CategoriesListBox.SelectedIndex);
                }
            }

        //EnterKey
        private void StatesEnter(object sender, KeyEventArgs e) {
            if (e.Key == Key.Return) {
                if (!StatesTextBox.Text.ToString().Trim().Equals("")) {
                    AddState(StatesTextBox.Text);
                    }
                }
            }

        //DeleteKey
        private void StatesDeleteKeyDown(object sender, KeyEventArgs e) {
            if ((e.Key == Key.Delete || e.Key == Key.Back) && (StatesListBox.SelectedIndex != -1)) {
                try {
                    e.Handled = true;
                    RemoveState(StatesListBox.SelectedIndex, CategoriesListBox.SelectedIndex);
                    } catch (Exception) { }
                }
            }

        //Category ADD
        private void AddCategory(string _content) {
            int _directoryCount = FindCategoryNumber();

            //Try to create a new category, and save its name along with it
            try {
                Directory.CreateDirectory($@"{Path.GetTempPath()}\TouchPortalPluginHelper\categories\category{_directoryCount}");
                File.WriteAllText($@"{Path.GetTempPath()}\TouchPortalPluginHelper\categories\category{_directoryCount}\name.TPPH", _content);
                } catch (Exception) { }

            //Create a new ListBoxItem object and give it the properties we want
            ListBoxItem _category = new ListBoxItem();
            _category.Content = _content;
            _category.ToolTip = "Right click to edit (Or double click)";
            _category.AddHandler(MouseRightButtonDownEvent, new MouseButtonEventHandler(CategoryRightClick), true);

            CategoriesListBox.Items.Add(_category); //Pass our object to the list
            CategoriesTextBox.Text = ""; //Clear the input field
            CategoriesListBox.SelectedItem = _category; //Select our newly passed item
            }

        private void CategoryRightClick(object sender, MouseButtonEventArgs e) {
            if (e.RightButton == MouseButtonState.Pressed) {
                ContextMenu cm = (ContextMenu)Resources["cmCategory"];
                cm.IsOpen = true;
                }
            }

        //Category ContextMenu EDIT
        private void Edit_Category_Click(object sender, RoutedEventArgs e) {
            File.WriteAllText($@"{Path.GetTempPath()}\TouchPortalPluginHelper\active_category.TPPH", CategoriesListBox.SelectedIndex.ToString()); //Remember this line!
            NavigationService.Navigate(new Category_Editor());
            }

        //Category ContextMenu REMOVE
        private void Remove_Category_Click(object sender, RoutedEventArgs e) {
            RemoveCategory(CategoriesListBox.SelectedIndex);
            }

        //Action ADD
        private void AddAction(string _content) {
            if (CategoriesListBox.SelectedIndex != -1) {
                //Try to get a string array of directories, catch if none exist
                try {
                    string _previousContent = File.ReadAllText($@"{ Path.GetTempPath()}\TouchPortalPluginHelper\categories\category{ CategoriesListBox.SelectedIndex}\actions.TPPH");
                    File.WriteAllText($@"{Path.GetTempPath()}\TouchPortalPluginHelper\categories\category{CategoriesListBox.SelectedIndex}\actions.TPPH", $"{_previousContent}\n{_content}");
                    } catch (Exception) { 
                    File.WriteAllText($@"{Path.GetTempPath()}\TouchPortalPluginHelper\categories\category{CategoriesListBox.SelectedIndex}\actions.TPPH", $"{_content}");
                    }

                ListBoxItem _action = new ListBoxItem();
                _action.Content = _content;
                _action.ToolTip = "Right click to edit (Or double click)";
                _action.AddHandler(MouseRightButtonDownEvent, new MouseButtonEventHandler(ActionRightClick), true);

                ActionsListBox.Items.Add(_action); //Pass our object to the list
                ActionsTextBox.Text = ""; //Clear the input field
                ActionsListBox.SelectedItem = _action; //Select our newly passed item
                } else {
                MessageBox.Show("You have to select a category first", "TouchPortal Plugin Helper", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }

        private void ActionRightClick(object sender, MouseButtonEventArgs e) {
            if (e.RightButton == MouseButtonState.Pressed) {
                ContextMenu cm = (ContextMenu)Resources["cmAction"];
                cm.IsOpen = true;
                }
            }

        //Action ContextMenu EDIT
        private void Edit_Action_Click(object sender, RoutedEventArgs e) {
            File.WriteAllText($@"{Path.GetTempPath()}\TouchPortalPluginHelper\active_category.TPPH", CategoriesListBox.SelectedIndex.ToString()); //Remember this line!
            File.WriteAllText($@"{Path.GetTempPath()}\TouchPortalPluginHelper\active_action.TPPH", ActionsListBox.SelectedIndex.ToString()); //Remember this line!
            NavigationService.Navigate(new Action_Editor());
            }

        //Action ContextMenu REMOVE
        private void Remove_Action_Click(object sender, RoutedEventArgs e) {
            RemoveAction(ActionsListBox.SelectedIndex, CategoriesListBox.SelectedIndex);
            }

        //Event ADD
        private void AddEvent(string _content) {
            if (CategoriesListBox.SelectedIndex != -1) {
                //Try to get a string array of directories, catch if none exist
                try {
                    string _previousContent = File.ReadAllText($@"{ Path.GetTempPath()}\TouchPortalPluginHelper\categories\category{ CategoriesListBox.SelectedIndex}\events.TPPH");
                    File.WriteAllText($@"{Path.GetTempPath()}\TouchPortalPluginHelper\categories\category{CategoriesListBox.SelectedIndex}\events.TPPH", $"{_previousContent}\n{_content}");
                    } catch (Exception) {
                    File.WriteAllText($@"{Path.GetTempPath()}\TouchPortalPluginHelper\categories\category{CategoriesListBox.SelectedIndex}\events.TPPH", $"{_content}");
                    }

                ListBoxItem _event = new ListBoxItem();
                _event.Content = _content;
                _event.ToolTip = "Right click to edit (Or double click)";
                _event.AddHandler(MouseRightButtonDownEvent, new MouseButtonEventHandler(EventRightClick), true);

                EventsListBox.Items.Add(_event); //Pass our object to the list
                EventsTextBox.Text = ""; //Clear the input field
                EventsListBox.SelectedItem = _event; //Select our newly passed item
                } else {
                MessageBox.Show("You have to select a category first", "TouchPortal Plugin Helper", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }

        private void EventRightClick(object sender, MouseButtonEventArgs e) {
            if (e.RightButton == MouseButtonState.Pressed) {
                ContextMenu cm = (ContextMenu)Resources["cmEvent"];
                cm.IsOpen = true;
                }
            }

        //Event ContextMenu EDIT
        private void Edit_Event_Click(object sender, RoutedEventArgs e) {
            File.WriteAllText($@"{Path.GetTempPath()}\TouchPortalPluginHelper\active_category.TPPH", CategoriesListBox.SelectedIndex.ToString()); //Remember this line!
            File.WriteAllText($@"{Path.GetTempPath()}\TouchPortalPluginHelper\active_event.TPPH", EventsListBox.SelectedIndex.ToString()); //Remember this line!
            NavigationService.Navigate(new Event_Editor());
            }

        //Event ContextMenu REMOVE
        private void Remove_Event_Click(object sender, RoutedEventArgs e) {
            RemoveEvent(EventsListBox.SelectedIndex, CategoriesListBox.SelectedIndex);
            }

        //State ADD
        private void AddState(string _content) {
            if (CategoriesListBox.SelectedIndex != -1) {
                //Try to get a string array of directories, catch if none exist
                try {
                    string _previousContent = File.ReadAllText($@"{ Path.GetTempPath()}\TouchPortalPluginHelper\categories\category{ CategoriesListBox.SelectedIndex}\states.TPPH");
                    File.WriteAllText($@"{Path.GetTempPath()}\TouchPortalPluginHelper\categories\category{CategoriesListBox.SelectedIndex}\states.TPPH", $"{_previousContent}\n{_content}");
                    } catch (Exception) {
                    File.WriteAllText($@"{Path.GetTempPath()}\TouchPortalPluginHelper\categories\category{CategoriesListBox.SelectedIndex}\states.TPPH", $"{_content}");
                    }

                ListBoxItem _state = new ListBoxItem();
                _state.Content = _content;
                _state.ToolTip = "Right click to edit (Or double click)";
                _state.AddHandler(MouseRightButtonDownEvent, new MouseButtonEventHandler(StateRightClick), true);

                StatesListBox.Items.Add(_state); //Pass our object to the list
                StatesTextBox.Text = ""; //Clear the input field
                StatesListBox.SelectedItem = _state; //Select our newly passed item
                } else {
                MessageBox.Show("You have to select a category first", "TouchPortal Plugin Helper", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }

        private void StateRightClick(object sender, MouseButtonEventArgs e) {
            if (e.RightButton == MouseButtonState.Pressed) {
                ContextMenu cm = (ContextMenu)Resources["cmState"];
                cm.IsOpen = true;
                }
            }

        //State ContextMenu EDIT
        private void Edit_State_Click(object sender, RoutedEventArgs e) {
            File.WriteAllText($@"{Path.GetTempPath()}\TouchPortalPluginHelper\active_category.TPPH", CategoriesListBox.SelectedIndex.ToString()); //Remember this line!
            File.WriteAllText($@"{Path.GetTempPath()}\TouchPortalPluginHelper\active_state.TPPH", StatesListBox.SelectedIndex.ToString()); //Remember this line!
            NavigationService.Navigate(new State_Editor());
            }

        //State ContextMenu REMOVE
        private void Remove_State_Click(object sender, RoutedEventArgs e) {
            RemoveState(StatesListBox.SelectedIndex, CategoriesListBox.SelectedIndex);
            }

        //Category Remove
        private void RemoveCategory(int _Index) {
            //Remove category from UI
            CategoriesListBox.Items.RemoveAt(_Index);
            //Remove category from Backend
            try {
                Directory.Delete($@"{Path.GetTempPath()}\TouchPortalPluginHelper\categories\category{_Index}", true);
                } catch (Exception) { }
            //Re-arrange the categories, aka Match the indexes
            for (int i = 0; i < CategoriesListBox.Items.Count; i++) {
                if (!Directory.Exists($@"{Path.GetTempPath()}\TouchPortalPluginHelper\categories\category{i}")) {
                    try {
                        Directory.Move($@"{Path.GetTempPath()}\TouchPortalPluginHelper\categories\category{i + 1}", $@"{Path.GetTempPath()}\TouchPortalPluginHelper\categories\category{i}");
                        } catch (Exception) { }
                    }
                }
            }

        //Action Remove
        private void RemoveAction(int _Index, int _catIndex) {
            //Remove from UI
            ActionsListBox.Items.RemoveAt(_Index);
            //Remove from backend
            try {
                List<string> _actions = File.ReadAllLines($@"{Path.GetTempPath()}\TouchPortalPluginHelper\categories\category{_catIndex}\actions.TPPH").ToList();
                _actions.RemoveAt(_Index);
                string[] _arrayActions = _actions.ToArray();
                File.Delete($@"{Path.GetTempPath()}\TouchPortalPluginHelper\categories\category{_catIndex}\actions.TPPH");
                File.WriteAllLines($@"{Path.GetTempPath()}\TouchPortalPluginHelper\categories\category{_catIndex}\actions.TPPH", _arrayActions);
                } catch (Exception) { }
            }

        //Event Remove
            private void RemoveEvent(int _Index, int _catIndex) {
            //Remove from UI
            EventsListBox.Items.RemoveAt(_Index);
            //Remove from backend
            try {
                List<string> _events = File.ReadAllLines($@"{Path.GetTempPath()}\TouchPortalPluginHelper\categories\category{_catIndex}\events.TPPH").ToList();
                _events.RemoveAt(_Index);
                string[] _arrayEvents = _events.ToArray();
                File.Delete($@"{Path.GetTempPath()}\TouchPortalPluginHelper\categories\category{_catIndex}\events.TPPH");
                File.WriteAllLines($@"{Path.GetTempPath()}\TouchPortalPluginHelper\categories\category{_catIndex}\events.TPPH", _arrayEvents);
                } catch (Exception) { }
            }

        //State Remove
        private void RemoveState(int _Index, int _catIndex) {
            //Remove from UI
            StatesListBox.Items.RemoveAt(_Index);
            //Remove from backend
            try {
                List<string> _states = File.ReadAllLines($@"{Path.GetTempPath()}\TouchPortalPluginHelper\categories\category{_catIndex}\states.TPPH").ToList();
                _states.RemoveAt(_Index);
                string[] _arrayStates = _states.ToArray();
                File.Delete($@"{Path.GetTempPath()}\TouchPortalPluginHelper\categories\category{_catIndex}\states.TPPH");
                File.WriteAllLines($@"{Path.GetTempPath()}\TouchPortalPluginHelper\categories\category{_catIndex}\states.TPPH", _arrayStates);
                } catch (Exception) { }
            }

        //CLEAR
        //Clear category
        private void ClearCategoryList(object sender, RoutedEventArgs e) {
            if (MessageBox.Show("Are you sure you want to clear the categories?\nThis action cannot be undone", "TouchPortal Plugin Helper", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes) {
                CategoriesListBox.Items.Clear();

                //Delete all categories, and everything held by them
                try {
                    Directory.Delete($@"{Path.GetTempPath()}\TouchPortalPluginHelper\categories", true);
                    } catch (Exception) { }
                }
            }

        //Clear action
        private void ClearActionList(object sender, RoutedEventArgs e) {
            if (CategoriesListBox.SelectedItem != null) {
                if (MessageBox.Show($"Are you sure you want to clear the actions of \"{CategoriesListBox.SelectedItem.ToString().Substring(CategoriesListBox.SelectedItem.ToString().LastIndexOf(':') + 2)}\"?\nThis action cannot be undone", "TouchPortal Plugin Helper", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes) {
                    //Remove from UI
                    ActionsListBox.Items.Clear();
                    //Remove from backend
                    try {
                        File.Delete($@"{Path.GetTempPath()}\TouchPortalPluginHelper\categories\category{CategoriesListBox.SelectedIndex}\actions.TPPH");
                        } catch (Exception) { }

                    }
                } else {
                MessageBox.Show("You need to select a category first", "TouchPortal Plugin Helper", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }

        //Clear event
        private void ClearEventList(object sender, RoutedEventArgs e) {
            if (CategoriesListBox.SelectedItem != null) {
                if (MessageBox.Show($"Are you sure you want to clear the events of \"{CategoriesListBox.SelectedItem.ToString().Substring(CategoriesListBox.SelectedItem.ToString().LastIndexOf(':') + 2)}\"?\nThis action cannot be undone", "TouchPortal Plugin Helper", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes) {
                    EventsListBox.Items.Clear();

                    }
                } else {
                MessageBox.Show("You need to select a category first", "TouchPortal Plugin Helper", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }

        //Clear state
        private void ClearStateList(object sender, RoutedEventArgs e) {
            if (CategoriesListBox.SelectedItem != null) {
                if (MessageBox.Show($"Are you sure you want to clear the states of \"{CategoriesListBox.SelectedItem.ToString().Substring(CategoriesListBox.SelectedItem.ToString().LastIndexOf(':') + 2)}\"?\nThis action cannot be undone", "TouchPortal Plugin Helper", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes) {
                    StatesListBox.Items.Clear();

                    }
                } else {
                MessageBox.Show("You need to select a category first", "TouchPortal Plugin Helper", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }

        //When a new category is selected
        private void CategorySelectionChanged(object sender, SelectionChangedEventArgs e) {
            //Clear the UI List boxes (Except for the category one, duuh)
            ActionsListBox.Items.Clear();
            EventsListBox.Items.Clear();
            StatesListBox.Items.Clear();
            
            //Check if a new category is selected, or nothing was selected
            if (CategoriesListBox.SelectedIndex != -1) {
                List<string> ActionsList = new List<string>();
                List<string> EventsList = new List<string>();
                List<string> StatesList = new List<string>();
                //Load the saved stuff back
                try {
                    ActionsList = LoadActions(CategoriesListBox.SelectedIndex);
                    } catch (Exception) { }
                try {
                    EventsList = LoadEvents(CategoriesListBox.SelectedIndex);
                    } catch (Exception) { }
                try {
                    StatesList = LoadStates(CategoriesListBox.SelectedIndex);
                    } catch (Exception) { }
                //Fill up the UI elements again
                //Actions
                for (int i = 0; i < ActionsList.Count; i++) {
                    ListBoxItem _action = new ListBoxItem();
                    _action.Content = ActionsList[i];
                    _action.ToolTip = "Right click to edit (Or double click)";
                    _action.AddHandler(MouseRightButtonDownEvent, new MouseButtonEventHandler(ActionRightClick), true);

                    ActionsListBox.Items.Add(_action); //Pass our object to the list
                    ActionsTextBox.Text = ""; //Clear the input field
                    ActionsListBox.SelectedItem = _action; //Select our newly passed object
                    }
                //Events
                for (int i = 0; i < EventsList.Count; i++) {
                    ListBoxItem _event = new ListBoxItem();
                    _event.Content = EventsList[i];
                    _event.ToolTip = "Right click to edit (Or double click)";
                    _event.AddHandler(MouseRightButtonDownEvent, new MouseButtonEventHandler(EventRightClick), true);

                    EventsListBox.Items.Add(_event); //Pass our object to the list
                    EventsTextBox.Text = ""; //Clear the input field
                    EventsListBox.SelectedItem = _event; //Select our newly passed object
                    }
                //States
                for (int i = 0; i < StatesList.Count; i++) {
                    ListBoxItem _state = new ListBoxItem();
                    _state.Content = StatesList[i];
                    _state.ToolTip = "Right click to edit (Or double click)";
                    _state.AddHandler(MouseRightButtonDownEvent, new MouseButtonEventHandler(StateRightClick), true);

                    StatesListBox.Items.Add(_state); //Pass our object to the list
                    StatesTextBox.Text = ""; //Clear the input field
                    StatesListBox.SelectedItem = _state; //Select our newly passed item
                    }
                }
            }

        private List<string> LoadActions(int _catIndex) {
            return File.ReadAllLines($@"{Path.GetTempPath()}\TouchPortalPluginHelper\categories\category{_catIndex}\actions.TPPH").ToList();
            }

        private List<string> LoadEvents(int _catIndex) {
            return File.ReadAllLines($@"{Path.GetTempPath()}\TouchPortalPluginHelper\categories\category{_catIndex}\events.TPPH").ToList();
            }

        private List<string> LoadStates(int _catIndex) {
            return File.ReadAllLines($@"{Path.GetTempPath()}\TouchPortalPluginHelper\categories\category{_catIndex}\states.TPPH").ToList();
            }

        //MouseDown events
        private void CategoryMouseDown(object sender, MouseButtonEventArgs e) {
            if (e.LeftButton == MouseButtonState.Pressed) {
                CategoriesListBox.SelectedItem = null;
                CategoriesTextBox.Focus();
                }
            }

        private void ActionMouseDown(object sender, MouseButtonEventArgs e) {
            if (e.LeftButton == MouseButtonState.Pressed) {
                ActionsListBox.SelectedItem = null;
                ActionsTextBox.Focus();
                }
            }

        private void EventMouseDown(object sender, MouseButtonEventArgs e) {
            if (e.LeftButton == MouseButtonState.Pressed) {
                EventsListBox.SelectedItem = null;
                EventsTextBox.Focus();
                }
            }

        private void StateMouseDown(object sender, MouseButtonEventArgs e) {
            if (e.LeftButton == MouseButtonState.Pressed) {
                StatesListBox.SelectedItem = null;
                StatesTextBox.Focus();
                }
            }

        //Double click events
        private void CategoriesMouseDoubleClick(object sender, MouseButtonEventArgs e) {
            if (e.LeftButton == MouseButtonState.Pressed) {
                if (CategoriesListBox.SelectedItem != null) {
                    File.WriteAllText($@"{Path.GetTempPath()}\TouchPortalPluginHelper\active_category.TPPH", CategoriesListBox.SelectedIndex.ToString()); //Remember this line!
                    NavigationService.Navigate(new Category_Editor());
                    }

                }
            }

        private void ActionsMouseDoubleClick(object sender, MouseButtonEventArgs e) {
            if (e.LeftButton == MouseButtonState.Pressed) {
                if (ActionsListBox.SelectedItem != null) {
                    //Open a window to edit the action information
                    File.WriteAllText($@"{Path.GetTempPath()}\TouchPortalPluginHelper\active_category.TPPH", CategoriesListBox.SelectedIndex.ToString()); //Remember this line!
                    File.WriteAllText($@"{Path.GetTempPath()}\TouchPortalPluginHelper\active_action.TPPH", ActionsListBox.SelectedIndex.ToString()); //Remember this line!
                    NavigationService.Navigate(new Action_Editor());
                    }
                }
            }

        private void EventsMouseDoubleClick(object sender, MouseButtonEventArgs e) {
            if (e.LeftButton == MouseButtonState.Pressed) {
                if (EventsListBox.SelectedItem != null) {
                    //Open a window to edit the event information
                    File.WriteAllText($@"{Path.GetTempPath()}\TouchPortalPluginHelper\active_category.TPPH", CategoriesListBox.SelectedIndex.ToString()); //Remember this line!
                    File.WriteAllText($@"{Path.GetTempPath()}\TouchPortalPluginHelper\active_event.TPPH", EventsListBox.SelectedIndex.ToString()); //Remember this line!
                    NavigationService.Navigate(new Event_Editor());
                    }
                }
            }

        private void StatesMouseDoubleClick(object sender, MouseButtonEventArgs e) {
            if (e.LeftButton == MouseButtonState.Pressed) {
                if (StatesListBox.SelectedItem != null) {
                    //Open a window to edit the state information
                    File.WriteAllText($@"{Path.GetTempPath()}\TouchPortalPluginHelper\active_category.TPPH", CategoriesListBox.SelectedIndex.ToString()); //Remember this line!
                    File.WriteAllText($@"{Path.GetTempPath()}\TouchPortalPluginHelper\active_state.TPPH", StatesListBox.SelectedIndex.ToString()); //Remember this line!
                    NavigationService.Navigate(new State_Editor());
                    }
                }
            }

        //Restore categories
        private void RestoreCategories() {
            int _directoryCount = FindCategoryNumber();
            for (int i = 0; i < _directoryCount; i++) {
                //Create a new ListBoxItem object and give it the properties we want
                ListBoxItem _category = new ListBoxItem();
                _category.Content = File.ReadAllText($@"{Path.GetTempPath()}\TouchPortalPluginHelper\categories\category{i}\name.TPPH");
                _category.ToolTip = "Right click to edit (Or double click)";
                _category.AddHandler(MouseRightButtonDownEvent, new MouseButtonEventHandler(CategoryRightClick), true);

                CategoriesListBox.Items.Add(_category); //Pass our object to the list
                CategoriesTextBox.Text = ""; //Clear the input field
                CategoriesListBox.SelectedItem = _category; //Select our newly passed item
                }
            }

        //Find number of categories
        private int FindCategoryNumber() {
            string[] _directories;
            int _directoryCount = 0;
            //Try to find how many categories exist, catch if none exist
            try {
                _directories = Directory.GetDirectories($@"{Path.GetTempPath()}\TouchPortalPluginHelper\categories");
                _directoryCount = _directories.Length; //Count how many folders exist within this path
                } catch (Exception) { }
            return _directoryCount;
            }

        private void Grid_LostFocus(object sender, RoutedEventArgs e) {
            Backup();
            }

        private void Backup() {
            if (ActionsListBox.SelectedIndex != -1) {
                File.WriteAllText($@"{Path.GetTempPath()}\TouchPortalPluginHelper\active_action.TPPH", ActionsListBox.SelectedIndex.ToString());
                }
            }

        }
    }
