using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;

namespace TouchPortal_Plugin_Helper
{
    public partial class DefineValueChoicesEvent : Page
    {
        public DefineValueChoicesEvent()
        {
            InitializeComponent();
            Restore();
        }

        //Enter key
        private void ValueChoiceKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                if (!ValueChoiceTextBox.Text.ToString().Trim().Equals(""))
                {
                    AddValueChoice(ValueChoiceTextBox.Text);
                }
            }
        }

        //Delete and backspace
        private void ValueChoiceDeleteKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Delete || e.Key == Key.Back)
            {
                try
                {
                    e.Handled = true;
                    RemoveValueChoice(ValueChoiceListBox.SelectedIndex);
                }
                catch (Exception) { }
            }
        }

        //Left click
        private void ValueChoiceMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                ValueChoiceListBox.SelectedItem = null;
                ValueChoiceTextBox.Focus();
            }
        }

        //ClearValueChoiceList Button
        private void ClearValueChoiceList(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to clear the list?\nThis action cannot be undone", "TouchPortal Plugin Helper", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {
                ValueChoiceListBox.Items.Clear();

                //Delete all data, and everything held by them
                try
                {
                    File.Delete($@"{Path.GetTempPath()}\TouchPortalPluginHelper\categories\category{FindActiveCategory()}\events\event{FindActiveEvent()}\valueChoices.TPPH");
                }
                catch (Exception) { }
            }
        }

        //AddValueChoiceList Button
        private void AddValueChoiceButton(object sender, RoutedEventArgs e)
        {
            if (!ValueChoiceTextBox.Text.ToString().Trim().Equals(""))
            {
                AddValueChoice(ValueChoiceTextBox.Text);
            }
        }

        //RemoveValueChoiceList Button
        private void RemoveValueChoiceButton(object sender, RoutedEventArgs e)
        {
            if (ValueChoiceListBox.SelectedIndex != -1)
            {
                RemoveValueChoice(ValueChoiceListBox.SelectedIndex);
            }
        }

        //Add ValueChoice 
        private void AddValueChoice(string _content)
        {
            try
            {
                File.AppendAllText($@"{Path.GetTempPath()}\TouchPortalPluginHelper\categories\category{FindActiveCategory()}\events\event{FindActiveEvent()}\valueChoices.TPPH", _content + "\n");
            }
            catch (Exception)
            { }

            ListBoxItem dataItem = new ListBoxItem();

            dataItem.Content = _content;
            dataItem.ToolTip = "Right click to rename";
            dataItem.AddHandler(MouseRightButtonDownEvent, new MouseButtonEventHandler(ValueChoiceListRightClick), true);

            ValueChoiceListBox.Items.Add(dataItem); //Pass our object to the list
            ValueChoiceTextBox.Text = ""; //Clear the input field
            ValueChoiceListBox.SelectedItem = dataItem; //Select our newly passed item
        }

        //ContextMenu method for ValueChoiceListBox
        private void ValueChoiceListRightClick(object sender, MouseButtonEventArgs e)
        {
            if (e.RightButton == MouseButtonState.Pressed)
            {
                ContextMenu cm = (ContextMenu)Resources["cm"];
                cm.IsOpen = true;
            }
        }

        //ValueChoice ContextMenu Rename
        private void Rename(object sender, RoutedEventArgs e)
        {
            //FIX ME
        }

        //ValueChoice ContextMenu REMOVE
        private void Remove(object sender, RoutedEventArgs e)
        {
            RemoveValueChoice(ValueChoiceListBox.SelectedIndex);
        }

        //Remove ValueChoice 
        private void RemoveValueChoice(int _index)
        {
            //Remove from UI
            ValueChoiceListBox.Items.RemoveAt(_index);
            //Remove from backend
            try
            {
                string[] temp = File.ReadAllLines($@"{Path.GetTempPath()}\TouchPortalPluginHelper\categories\category{FindActiveCategory()}\events\event{FindActiveEvent()}\valueChoices.TPPH");
                List<string> temp2 = new List<string>();
                for (int i = 0; i < temp.Length; i++)
                {
                    if (i != _index)
                    {
                        temp2.Add(temp[i]);
                    }
                }
                File.WriteAllLines($@"{Path.GetTempPath()}\TouchPortalPluginHelper\categories\category{FindActiveCategory()}\events\event{FindActiveEvent()}\valueChoices.TPPH", temp2);
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
        private string FindActiveEvent()
        {
            string _activeAction = null;
            try
            {
                _activeAction = File.ReadAllText($@"{Path.GetTempPath()}\TouchPortalPluginHelper\active_event.TPPH");
            }
            catch (Exception) { }
            return _activeAction;
        }
        private void Restore()
        {
            try
            {
                string[] choices = File.ReadAllLines($@"{Path.GetTempPath()}\TouchPortalPluginHelper\categories\category{FindActiveCategory()}\events\event{FindActiveEvent()}\valueChoices.TPPH");
                foreach (string choice in choices)
                {
                    ListBoxItem item = new ListBoxItem();
                    item.Content = choice;
                    item.ToolTip = "Right click to edit";
                    item.AddHandler(MouseRightButtonDownEvent, new MouseButtonEventHandler(ValueChoiceListRightClick), true);

                    ValueChoiceListBox.Items.Add(item); //Pass our object to the list
                    ValueChoiceTextBox.Text = ""; //Clear the input field
                    ValueChoiceListBox.SelectedItem = item; //Select our newly passed object
                }
            }
            catch (Exception)
            { }
        }
    }
}
