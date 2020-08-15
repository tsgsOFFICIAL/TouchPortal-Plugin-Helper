using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;

namespace TouchPortal_Plugin_Helper {
    public partial class AddActionData : Page {
        public AddActionData() {
            InitializeComponent();
            Restore();
            }

        private void Grid_LostFocus(object sender, RoutedEventArgs e) {
            Backup();
            }

        //Data objects
        //Enter key
        private void DataKeyDown(object sender, KeyEventArgs e) {
            if (e.Key == Key.Return) {
                if (!DatasTextBox.Text.ToString().Trim().Equals("")) {
                    AddData(DatasTextBox.Text);
                    }
                }
            }

        //Delete and backspace
        private void DataDeleteKeyDown(object sender, KeyEventArgs e) {
            if (e.Key == Key.Delete || e.Key == Key.Back) {
                try {
                    e.Handled = true;
                    RemoveData(DataListBox.SelectedIndex);
                    } catch (Exception) { }
                }
            }

        //Selection changed
        private void DataSelectionChanged(object sender, SelectionChangedEventArgs e) {
            }

        //Left click
        private void DataMouseDown(object sender, MouseButtonEventArgs e) {
            if (e.LeftButton == MouseButtonState.Pressed) {
                DataListBox.SelectedItem = null;
                DataListBox.Focus();
                }
            }

        //ClearDataList Button
        private void ClearDataList(object sender, RoutedEventArgs e) {
            if (MessageBox.Show("Are you sure you want to clear the data list?\nThis action cannot be undone", "TouchPortal Plugin Helper", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes) {
                DataListBox.Items.Clear();

                ////Delete all categories, and everything held by them
                //try {
                //    Directory.Delete($@"{Path.GetTempPath()}\TouchPortalPluginHelper\categories", true);
                //    } catch (Exception) { }
                }
            }

        //AddDataList Button
        private void AddDataButton(object sender, RoutedEventArgs e) {
            if (!DatasTextBox.Text.ToString().Trim().Equals("")) {
                AddData(DatasTextBox.Text);
                }
            }

        //RemoveDataList Button
        private void RemoveDataButton(object sender, RoutedEventArgs e) {
            if (DataListBox.SelectedIndex != -1) {
                RemoveData(DataListBox.SelectedIndex);
                }
            }

        //Add Data 
        private void AddData(string _content) {

            //////////////// Stolen from DefineCategories
            //int _directoryCount = FindCategoryNumber();

            ////Try to create a new category, and save its name along with it
            //try {
            //    Directory.CreateDirectory($@"{Path.GetTempPath()}\TouchPortalPluginHelper\categories\category{_directoryCount}");
            //    File.WriteAllText($@"{Path.GetTempPath()}\TouchPortalPluginHelper\categories\category{_directoryCount}\name.TPPH", _content);
            //    } catch (Exception) { }

            ////Create a new ListBoxItem object and give it the properties we want
            //ListBoxItem _category = new ListBoxItem();
            //_category.Content = _content;
            //_category.ToolTip = "Right click to edit (Or double click)";
            //_category.AddHandler(MouseRightButtonDownEvent, new MouseButtonEventHandler(CategoryRightClick), true);

            //CategoriesListBox.Items.Add(_category); //Pass our object to the list
            //CategoriesTextBox.Text = ""; //Clear the input field
            //CategoriesListBox.SelectedItem = _category; //Select our newly passed item
            }

        //ContextMenu method for DataListBox
        private void DataListRightClick(object sender, MouseButtonEventArgs e) {
            if (e.RightButton == MouseButtonState.Pressed) {
                ContextMenu cm = (ContextMenu)Resources["cmData"];
                cm.IsOpen = true;
                }
            }

        //Data ContextMenu Rename
        private void DataRename(object sender, RoutedEventArgs e) {

            }

        //Data ContextMenu REMOVE
        private void DataRemove(object sender, RoutedEventArgs e) {
            RemoveData(DataListBox.SelectedIndex);
            }

        //Remove Data 
        private void RemoveData(int _index) {
            //Remove from UI
            DataListBox.Items.RemoveAt(_index);
            //Remove from Backend
            }


        //DataChoices

        //Enter key
        private void DataChoicesKeyDown(object sender, KeyEventArgs e) {

            }

        //Delete and backspace
        private void DataChoicesDeleteKeyDown(object sender, KeyEventArgs e) {

            }

        //DataChoices Selection changed
        private void DataChoicesSelectionChanged(object sender, SelectionChangedEventArgs e) {
            }

        //DataChoices Left click
        private void DataChoicesMouseDown(object sender, MouseButtonEventArgs e) {

            }

        //ClearDataChoicesList Button
        private void ClearDataChoicesList(object sender, RoutedEventArgs e) {

            }

        //AddDataChoices Button
        private void AddDataChoicesButton(object sender, RoutedEventArgs e) {

            }

        //RemoveDataChoices Button
        private void RemoveDataChoicesButton(object sender, RoutedEventArgs e) {

            }

        //Add Data 
        private void AddDataChoice(string _content) {

            }

        //ContextMenu method for DataChoicesListBox
        private void DataChoicesListRightClick(object sender, MouseButtonEventArgs e) {
            if (e.RightButton == MouseButtonState.Pressed) {
                ContextMenu cm = (ContextMenu)Resources["cmData"];
                cm.IsOpen = true;
                }
            }

        //Data ContextMenu Rename
        private void DataChoicesRename(object sender, RoutedEventArgs e) {

            }

        //Data ContextMenu REMOVE
        private void DataChoicesRemove(object sender, RoutedEventArgs e) {
            RemoveDataChoice(DataListBox.SelectedIndex);
            }

        //Remove Data 
        private void RemoveDataChoice(int _index) {
            //Remove from UI
            DataChoicesListBox.Items.RemoveAt(_index);
            //Remove from Backend
            }

        //ComboBox event, to allow clicking on the arrow label
        private void Label_MouseLeftButtonDown(object sender, MouseButtonEventArgs e) {
            if (e.LeftButton == MouseButtonState.Pressed) {
                if (DataTypeComboBox.IsDropDownOpen) {
                    DataTypeComboBox.IsDropDownOpen = false;
                    } else {
                    DataTypeComboBox.IsDropDownOpen = true;
                    }
                }
            }

        //ComboBox event, to allow clicking on the arrow label
        private void Label2_MouseLeftButtonDown(object sender, MouseButtonEventArgs e) {
            if (e.LeftButton == MouseButtonState.Pressed) {
                if (DataDefaultValueComboBox.IsDropDownOpen) {
                    DataDefaultValueComboBox.IsDropDownOpen = false;
                    } else {
                    DataDefaultValueComboBox.IsDropDownOpen = true;
                    }
                }
            }

        //backup
        private void Backup() {

            }

        //Restore
        private void Restore() {

            }

        }
    }
