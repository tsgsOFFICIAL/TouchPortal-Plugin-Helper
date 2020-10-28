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
    public partial class DefineValueChoices : Page
    {
        public DefineValueChoices()
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
            if (MessageBox.Show("Are you sure you want to clear the data list?\nThis action cannot be undone", "TouchPortal Plugin Helper", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {
                ValueChoiceListBox.Items.Clear();

                //Delete all data, and everything held by them
                try
                {
                    //Directory.Delete($@"{Path.GetTempPath()}\TouchPortalPluginHelper\categories\category{FindActiveCategory()}\actions\action{FindActiveAction()}\data", true);
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
                ContextMenu cm = (ContextMenu)Resources["cmValueChoice"];
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
                //Directory.Delete($@"{Path.GetTempPath()}\TouchPortalPluginHelper\categories\category{FindActiveCategory()}\actions\action{FindActiveAction()}\data\data{FindActiveValueChoice()}", true);
            }
            catch (Exception)
            { }

            //Re-arrange actions, matching the index numbers
            for (int i = 0; i < ValueChoiceListBox.Items.Count; i++)
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

        private void Backup()
        {

        }

        private void Restore()
        {

        }
    }
}
