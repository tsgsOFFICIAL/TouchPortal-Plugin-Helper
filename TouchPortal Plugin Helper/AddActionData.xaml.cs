using System;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Controls;
using System.Collections.Generic;

namespace TouchPortal_Plugin_Helper
{
    public partial class AddActionData : Page
    {
        public AddActionData()
        {
            InitializeComponent();
            Restore();
        }

        //Data objects
        //Enter key
        private void DataKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                if (!DataTextBox.Text.ToString().Trim().Equals(""))
                {
                    AddData(DataTextBox.Text);
                }
            }
        }

        //Delete and backspace
        private void DataDeleteKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Delete || e.Key == Key.Back)
            {
                try
                {
                    e.Handled = true;
                    RemoveData(DataListBox.SelectedIndex);
                }
                catch (Exception) { }
            }
        }

        //Selection changed
        private void DataSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //Clear the UI
            DataTypeComboBox.SelectedIndex = -1;
            DataChoicesListBox.Items.Clear();
            DataChoicesTextBox.Text = "";
            DataDefaultValue.Text = "";
            dataExtensions.Text = "";
            dataLabel.Text = "";
            dataID.Text = "";

            //Fill the UI with new information if selected index is not -1
            if (DataListBox.SelectedIndex != -1)
            {
                List<string> choiceList = new List<string>();
                try
                {
                    File.WriteAllText($@"{Path.GetTempPath()}\TouchPortalPluginHelper\active_data.TPPH", DataListBox.SelectedIndex.ToString());
                }
                catch (Exception)
                { }

                try
                {
                    choiceList = File.ReadAllLines($@"{Path.GetTempPath()}\TouchPortalPluginHelper\categories\category{FindActiveCategory()}\actions\action{FindActiveAction()}\data\data{FindActiveData()}\choices.TPPH").ToList();
                }
                catch (Exception)
                { }

                foreach (string choice in choiceList)
                {
                    ListBoxItem item = new ListBoxItem();
                    item.Content = choice;
                    item.ToolTip = "Right click to rename";
                    item.AddHandler(MouseRightButtonDownEvent, new MouseButtonEventHandler(DataChoicesListRightClick), true);

                    DataChoicesListBox.Items.Add(item); //Pass our object to the list
                    DataTextBox.Text = ""; //Clear the input field
                    DataChoicesListBox.SelectedItem = item; //Select our newly passed object
                }

                try
                {
                    dataLabel.Text = File.ReadAllText($@"{Path.GetTempPath()}\TouchPortalPluginHelper\categories\category{FindActiveCategory()}\actions\action{FindActiveAction()}\data\data{FindActiveData()}\name.TPPH");
                }
                catch (Exception) { }

                try
                {
                    dataID.Text = File.ReadAllText($@"{Path.GetTempPath()}\TouchPortalPluginHelper\categories\category{FindActiveCategory()}\actions\action{FindActiveAction()}\data\data{FindActiveData()}\id.TPPH");
                }
                catch (Exception) { }

                try
                {
                    DataTypeComboBox.SelectedIndex = Convert.ToInt32(File.ReadAllText($@"{Path.GetTempPath()}\TouchPortalPluginHelper\categories\category{FindActiveCategory()}\actions\action{FindActiveAction()}\data\data{FindActiveData()}\type.TPPH"));
                }
                catch (Exception) { }

                try
                {
                    DataDefaultValue.Text = File.ReadAllText($@"{Path.GetTempPath()}\TouchPortalPluginHelper\categories\category{FindActiveCategory()}\actions\action{FindActiveAction()}\data\data{FindActiveData()}\defaultValue.TPPH");
                }
                catch (Exception) { }

                try
                {
                    dataExtensions.Text = File.ReadAllText($@"{Path.GetTempPath()}\TouchPortalPluginHelper\categories\category{FindActiveCategory()}\actions\action{FindActiveAction()}\data\data{FindActiveData()}\dataExtension.TPPH");
                }
                catch (Exception) { }

                //Enable the input fields
                dataLabel.IsEnabled = true;
                dataID.IsEnabled = true;
                DataTypeComboBox.IsHitTestVisible = true;
                DataTypeComboBox.Focusable = true;
                DataDefaultValue.IsEnabled = true;
                dataExtensions.IsEnabled = true;
                DataTypeLabel.IsEnabled = true;
            }
            else
            {
                //Disable the input fields
                dataLabel.IsEnabled = false;
                dataID.IsEnabled = false;
                DataTypeComboBox.IsHitTestVisible = false;
                DataTypeComboBox.Focusable = false;
                DataDefaultValue.IsEnabled = false;
                dataExtensions.IsEnabled = false;
                DataTypeLabel.IsEnabled = false;
            }
        }

        //Left click
        private void DataMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DataListBox.SelectedItem = null;
                DataTextBox.Focus();
            }
        }

        //ClearDataList Button
        private void ClearDataList(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to clear the data list?\nThis action cannot be undone", "TouchPortal Plugin Helper", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {
                DataListBox.Items.Clear();

                //Delete all data, and everything held by them
                try
                {
                    Directory.Delete($@"{Path.GetTempPath()}\TouchPortalPluginHelper\categories\category{FindActiveCategory()}\actions\action{FindActiveAction()}\data", true);
                }
                catch (Exception) { }
            }
        }

        //AddDataList Button
        private void AddDataButton(object sender, RoutedEventArgs e)
        {
            if (!DataTextBox.Text.ToString().Trim().Equals(""))
            {
                AddData(DataTextBox.Text);
            }
        }

        //RemoveDataList Button
        private void RemoveDataButton(object sender, RoutedEventArgs e)
        {
            if (DataListBox.SelectedIndex != -1)
            {
                RemoveData(DataListBox.SelectedIndex);
            }
        }

        //Add Data 
        private void AddData(string _content)
        {
            int dataNumber = 0;
            try
            {
                dataNumber = Directory.GetDirectories($@"{Path.GetTempPath()}\TouchPortalPluginHelper\categories\category{FindActiveCategory()}\actions\action{FindActiveAction()}\data\").Length;
            }
            catch (Exception)
            { }

            //Create a new data dir
            try
            {
                Directory.CreateDirectory($@"{Path.GetTempPath()}\TouchPortalPluginHelper\categories\category{FindActiveCategory()}\actions\action{FindActiveAction()}\data\data{dataNumber}");
                File.WriteAllText($@"{Path.GetTempPath()}\TouchPortalPluginHelper\categories\category{FindActiveCategory()}\actions\action{FindActiveAction()}\data\data{dataNumber}\name.TPPH", _content);
            }
            catch (Exception)
            { }

            ListBoxItem dataItem = new ListBoxItem();

            dataItem.Content = _content;
            dataItem.ToolTip = "Right click to rename";
            dataItem.AddHandler(MouseRightButtonDownEvent, new MouseButtonEventHandler(DataListRightClick), true);

            DataListBox.Items.Add(dataItem); //Pass our object to the list
            DataTextBox.Text = ""; //Clear the input field
            DataListBox.SelectedItem = dataItem; //Select our newly passed item
        }

        //ContextMenu method for DataListBox
        private void DataListRightClick(object sender, MouseButtonEventArgs e)
        {
            if (e.RightButton == MouseButtonState.Pressed)
            {
                ContextMenu cm = (ContextMenu)Resources["cmData"];
                cm.IsOpen = true;
            }
        }

        //Data ContextMenu Rename
        private void DataRename(object sender, RoutedEventArgs e)
        {
            //FIX ME
        }

        //Data ContextMenu REMOVE
        private void DataRemove(object sender, RoutedEventArgs e)
        {
            RemoveData(DataListBox.SelectedIndex);
        }

        //Remove Data 
        private void RemoveData(int _index)
        {
            //Remove from UI
            DataListBox.Items.RemoveAt(_index);
            //Remove from backend
            try
            {
                Directory.Delete($@"{Path.GetTempPath()}\TouchPortalPluginHelper\categories\category{FindActiveCategory()}\actions\action{FindActiveAction()}\data\data{FindActiveData()}", true);
            }
            catch (Exception)
            { }

            //Re-arrange actions, matching the index numbers
            for (int i = 0; i < DataListBox.Items.Count; i++)
            {
                if (!Directory.Exists($@"{Path.GetTempPath()}\TouchPortalPluginHelper\categories\category{FindActiveCategory()}\actions\action{FindActiveAction()}\data\data{i}"))
                {
                    try
                    {
                        Directory.Move($@"{Path.GetTempPath()}\TouchPortalPluginHelper\categories\category{FindActiveCategory()}\actions\action{FindActiveAction()}\data\data{i + 1}", $@"{Path.GetTempPath()}\TouchPortalPluginHelper\categories\category{FindActiveCategory()}\actions\action{FindActiveAction()}\data\data{i}");
                    }
                    catch (Exception) { }
                }
            }
        }


        //DataChoices

        //Enter key
        private void DataChoicesKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                if (!DataChoicesTextBox.Text.ToString().Trim().Equals(""))
                {
                    AddDataChoice(DataChoicesTextBox.Text);
                }
            }
        }

        //Delete and backspace
        private void DataChoicesDeleteKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Delete || e.Key == Key.Back)
            {
                try
                {
                    e.Handled = true;
                    RemoveDataChoice(DataChoicesListBox.SelectedIndex);
                }
                catch (Exception) { }
            }
        }

        //DataChoices Left click
        private void DataChoicesMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DataChoicesListBox.SelectedItem = null;
                DataChoicesTextBox.Focus();
            }
        }

        //ClearDataChoicesList Button
        private void ClearDataChoicesList(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to clear the data choice list?\nThis action cannot be undone", "TouchPortal Plugin Helper", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {
                DataChoicesListBox.Items.Clear();

                //Delete all data choices, and everything held by them
                try
                {
                    File.Delete($@"{Path.GetTempPath()}\TouchPortalPluginHelper\categories\category{FindActiveCategory()}\actions\action{FindActiveAction()}\data\data{FindActiveData()}\choices.TPPH");
                }
                catch (Exception) { }
            }
        }

        //AddDataChoices Button
        private void AddDataChoicesButton(object sender, RoutedEventArgs e)
        {
            if (!DataChoicesTextBox.Text.ToString().Trim().Equals(""))
            {
                AddDataChoice(DataChoicesTextBox.Text);
            }
        }

        //RemoveDataChoices Button
        private void RemoveDataChoicesButton(object sender, RoutedEventArgs e)
        {
            if (DataChoicesListBox.SelectedIndex != -1)
            {
                RemoveDataChoice(DataChoicesListBox.SelectedIndex);
            }
        }

        //Add Data 
        private void AddDataChoice(string _content)
        {
            if (DataListBox.SelectedIndex != -1)
            {
                //Add UI
                ListBoxItem item = new ListBoxItem();

                item.Content = _content;
                item.ToolTip = "Right click to rename";
                item.AddHandler(MouseRightButtonDownEvent, new MouseButtonEventHandler(DataChoicesListRightClick), true);

                DataChoicesListBox.Items.Add(item); //Pass our object to the list
                DataChoicesTextBox.Text = ""; //Clear the input field
                DataChoicesListBox.SelectedItem = item; //Select our newly passed item

                //Add backend
                try
                {
                    List<string> temp = File.ReadAllLines($@"{Path.GetTempPath()}\TouchPortalPluginHelper\categories\category{FindActiveCategory()}\actions\action{FindActiveAction()}\data\data{FindActiveData()}\choices.TPPH").ToList();
                    temp.Add(_content);
                    File.WriteAllLines($@"{Path.GetTempPath()}\TouchPortalPluginHelper\categories\category{FindActiveCategory()}\actions\action{FindActiveAction()}\data\data{FindActiveData()}\choices.TPPH", temp);
                }
                catch (Exception)
                {
                    File.WriteAllText($@"{Path.GetTempPath()}\TouchPortalPluginHelper\categories\category{FindActiveCategory()}\actions\action{FindActiveAction()}\data\data{FindActiveData()}\choices.TPPH", _content);
                }
            }
            else
            {
                MessageBox.Show("You have to select a data piece first", "TouchPortal Plugin Helper", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        //ContextMenu method for DataChoicesListBox
        private void DataChoicesListRightClick(object sender, MouseButtonEventArgs e)
        {
            if (e.RightButton == MouseButtonState.Pressed)
            {
                ContextMenu cm = (ContextMenu)Resources["cmDataChoices"];
                cm.IsOpen = true;
            }
        }

        //Data ContextMenu Rename
        private void DataChoicesRename(object sender, RoutedEventArgs e)
        {
            //FIX ME
        }

        //Data ContextMenu REMOVE
        private void DataChoicesRemove(object sender, RoutedEventArgs e)
        {
            RemoveDataChoice(DataChoicesListBox.SelectedIndex);
        }

        //Remove Data 
        private void RemoveDataChoice(int _index)
        {
            //Remove from UI
            DataChoicesListBox.Items.RemoveAt(_index);
            //Remove from Backend
            try
            {
                List<string> temp = File.ReadAllLines($@"{Path.GetTempPath()}\TouchPortalPluginHelper\categories\category{FindActiveCategory()}\actions\action{FindActiveAction()}\data\data{FindActiveData()}\choices.TPPH").ToList();
                temp.RemoveAt(_index);
                File.WriteAllLines($@"{Path.GetTempPath()}\TouchPortalPluginHelper\categories\category{FindActiveCategory()}\actions\action{FindActiveAction()}\data\data{FindActiveData()}\choices.TPPH", temp);
            }
            catch (Exception)
            { }
        }

        //ComboBox event, to allow clicking on the arrow name
        private void Label_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                if (DataTypeComboBox.IsDropDownOpen)
                {
                    DataTypeComboBox.IsDropDownOpen = false;
                }
                else
                {
                    DataTypeComboBox.IsDropDownOpen = true;
                }
            }
        }

        //Restore
        private void Restore()
        {
            try
            {
                for (int i = 0; i < Directory.GetDirectories($@"{Path.GetTempPath()}\TouchPortalPluginHelper\categories\category{FindActiveCategory()}\actions\action{FindActiveAction()}\data").Length; i++)
                {
                    //Create a new ListBoxItem object and give it the properties we want
                    ListBoxItem _category = new ListBoxItem();
                    _category.Content = File.ReadAllText($@"{Path.GetTempPath()}\TouchPortalPluginHelper\categories\category{FindActiveCategory()}\actions\action{FindActiveAction()}\data\data{i}\name.TPPH");
                    _category.ToolTip = "Right click to rename";
                    _category.AddHandler(MouseRightButtonDownEvent, new MouseButtonEventHandler(DataListRightClick), true);

                    DataListBox.Items.Add(_category); //Pass our object to the list
                    DataTextBox.Text = ""; //Clear the input field
                    DataListBox.SelectedItem = _category; //Select our newly passed item
                }
            }
            catch (Exception)
            { }
        }

        //Find the active category
        private string FindActiveCategory()
        {
            string _activeCategory = null;
            try
            {
                _activeCategory = File.ReadAllText($@"{Path.GetTempPath()}\TouchPortalPluginHelper\active_category.TPPH");
            }
            catch (Exception) { }
            return _activeCategory;
        }

        //Find the active action
        private string FindActiveAction()
        {
            string _activeAction = null;
            try
            {
                _activeAction = File.ReadAllText($@"{Path.GetTempPath()}\TouchPortalPluginHelper\active_action.TPPH");
            }
            catch (Exception) { }
            return _activeAction;
        }

        private string FindActiveData()
        {
            string _activeData = null;
            try
            {
                _activeData = File.ReadAllText($@"{Path.GetTempPath()}\TouchPortalPluginHelper\active_data.TPPH");
            }
            catch (Exception)
            { }
            return _activeData;
        }

        private void dataLabel_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (Convert.ToInt32(FindActiveData()) == DataListBox.SelectedIndex)
            {
                try
                {
                    File.WriteAllText($@"{Path.GetTempPath()}\TouchPortalPluginHelper\categories\category{FindActiveCategory()}\actions\action{FindActiveAction()}\data\data{FindActiveData()}\name.TPPH", dataLabel.Text);
                }
                catch (Exception)
                { }
            }
        }

        private void dataID_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (Convert.ToInt32(FindActiveData()) == DataListBox.SelectedIndex)
            {
                try
                {
                    File.WriteAllText($@"{Path.GetTempPath()}\TouchPortalPluginHelper\categories\category{FindActiveCategory()}\actions\action{FindActiveAction()}\data\data{FindActiveData()}\id.TPPH", dataID.Text);
                }
                catch (Exception)
                { }
            }
        }

        private void DataTypeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (Convert.ToInt32(FindActiveData()) == DataListBox.SelectedIndex)
            {
                try
                {
                    File.WriteAllText($@"{Path.GetTempPath()}\TouchPortalPluginHelper\categories\category{FindActiveCategory()}\actions\action{FindActiveAction()}\data\data{FindActiveData()}\type.TPPH", $"{DataTypeComboBox.SelectedIndex}");
                }
                catch (Exception)
                { }
            }
        }

        private void DataDefaultValue_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (Convert.ToInt32(FindActiveData()) == DataListBox.SelectedIndex)
            {
                try
                {
                    File.WriteAllText($@"{Path.GetTempPath()}\TouchPortalPluginHelper\categories\category{FindActiveCategory()}\actions\action{FindActiveAction()}\data\data{FindActiveData()}\defaultValue.TPPH", DataDefaultValue.Text);
                }
                catch (Exception)
                { }
            }
        }

        private void dataExtensions_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (Convert.ToInt32(FindActiveData()) == DataListBox.SelectedIndex)
            {
                try
                {
                    File.WriteAllText($@"{Path.GetTempPath()}\TouchPortalPluginHelper\categories\category{FindActiveCategory()}\actions\action{FindActiveAction()}\data\data{FindActiveData()}\dataExtension.TPPH", dataExtensions.Text);
                }
                catch (Exception)
                { }
            }
        }

        private void dataLabel_LostFocus(object sender, RoutedEventArgs e)
        {
            try
            {
                ListBoxItem dataItem = new ListBoxItem();

                dataItem.Content = dataLabel.Text;
                dataItem.ToolTip = "Right click to rename";
                dataItem.AddHandler(MouseRightButtonDownEvent, new MouseButtonEventHandler(DataListRightClick), true);

                DataListBox.Items.Insert(DataListBox.SelectedIndex, dataItem); //Pass our object to the list
                DataListBox.Items.RemoveAt(DataListBox.SelectedIndex);
                DataTextBox.Text = ""; //Clear the input field
                DataListBox.SelectedItem = dataItem; //Select our newly passed item
            }
            catch (Exception)
            { }
        }
    }
}
