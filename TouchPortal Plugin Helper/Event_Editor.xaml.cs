using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace TouchPortal_Plugin_Helper
{
    /// <summary>
    /// Interaction logic for Event_Editor.xaml
    /// </summary>
    public partial class Event_Editor : Page
    {
        public Event_Editor()
        {
            InitializeComponent();
            Restore();
        }

        private void Grid_LostFocus(object sender, RoutedEventArgs e)
        {
            Backup();
        }

        //Go back
        private void GoBackClick(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new DefineCategories());
        }

        //Define value choices
        private void DefineValueChoicesBtnClick(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new DefineValueChoicesEvent());
        }

        private void TypeDropDownLeftClick(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                if (!ValueComboBox.IsDropDownOpen)
                {
                    ValueComboBox.IsDropDownOpen = true;
                }
                else
                {
                    ValueComboBox.IsDropDownOpen = false;
                }
            }
        }

        private void Backup()
        {
            //Name
            try
            {
                File.WriteAllText($@"{Path.GetTempPath()}\TouchPortalPluginHelper\categories\category{FindActiveCategory()}\events\event{FindActiveEvent()}\name.TPPH", NameTxtBox.Text);
            }
            catch (Exception)
            { }
            //ID
            try
            {
                File.WriteAllText($@"{Path.GetTempPath()}\TouchPortalPluginHelper\categories\category{FindActiveCategory()}\events\event{FindActiveEvent()}\id.TPPH", IDTxtBox.Text);
            }
            catch (Exception)
            { }
            //Format
            try
            {
                File.WriteAllText($@"{Path.GetTempPath()}\TouchPortalPluginHelper\categories\category{FindActiveCategory()}\events\event{FindActiveEvent()}\format.TPPH", FormatTxtBox.Text);
            }
            catch (Exception)
            { }
            //TypeTxtBox
            try
            {
                File.WriteAllText($@"{Path.GetTempPath()}\TouchPortalPluginHelper\categories\category{FindActiveCategory()}\events\event{FindActiveEvent()}\type.TPPH", TypeTxtBox.Text);
            }
            catch (Exception)
            { }
            //Value type
            try
            {
                File.WriteAllText($@"{Path.GetTempPath()}\TouchPortalPluginHelper\categories\category{FindActiveCategory()}\events\event{FindActiveEvent()}\value.TPPH", ValueTypeTxtBox.Text);
            }
            catch (Exception)
            { }
            //Value state id
            try
            {
                File.WriteAllText($@"{Path.GetTempPath()}\TouchPortalPluginHelper\categories\category{FindActiveCategory()}\events\event{FindActiveEvent()}\stateID.TPPH", ValueStateIDTxtBox.Text);
            }
            catch (Exception)
            { }
        }

        private void Restore()
        {
            string name = "", id = "", format = "", type = "", value = "", stateID = "";
            string[] valueChoices = null;
            //Name
            try
            {
                name = File.ReadAllText($@"{Path.GetTempPath()}\TouchPortalPluginHelper\categories\category{FindActiveCategory()}\events\event{FindActiveEvent()}\name.TPPH");
            }
            catch (Exception)
            { }
            //ID
            try
            {
                id = File.ReadAllText($@"{Path.GetTempPath()}\TouchPortalPluginHelper\categories\category{FindActiveCategory()}\events\event{FindActiveEvent()}\id.TPPH");
            }
            catch (Exception)
            { }
            //Format
            try
            {
                format = File.ReadAllText($@"{Path.GetTempPath()}\TouchPortalPluginHelper\categories\category{FindActiveCategory()}\events\event{FindActiveEvent()}\format.TPPH");
            }
            catch (Exception)
            { }
            //TypeTxtBox
            try
            {
                type = File.ReadAllText($@"{Path.GetTempPath()}\TouchPortalPluginHelper\categories\category{FindActiveCategory()}\events\event{FindActiveEvent()}\type.TPPH");
            }
            catch (Exception)
            { }
            //Value type
            try
            {
                value = File.ReadAllText($@"{Path.GetTempPath()}\TouchPortalPluginHelper\categories\category{FindActiveCategory()}\events\event{FindActiveEvent()}\value.TPPH");
            }
            catch (Exception)
            { }
            //Value state id
            try
            {
                stateID = File.ReadAllText($@"{Path.GetTempPath()}\TouchPortalPluginHelper\categories\category{FindActiveCategory()}\events\event{FindActiveEvent()}\stateID.TPPH");
            }
            catch (Exception)
            { }
            //Value state id
            try
            {
                valueChoices = File.ReadAllLines($@"{Path.GetTempPath()}\TouchPortalPluginHelper\categories\category{FindActiveCategory()}\events\event{FindActiveEvent()}\valueChoices.TPPH");
            }
            catch (Exception)
            { }

            //Update the UI
            try
            {
                NameTxtBox.Text = name;
            }
            catch (Exception)
            { }
            try
            {
                IDTxtBox.Text = id;
            }
            catch (Exception)
            { }
            try
            {
                FormatTxtBox.Text = format;
            }
            catch (Exception)
            { }
            try
            {
                TypeTxtBox.Text = type;
            }
            catch (Exception)
            { }
            try
            {
                ValueTypeTxtBox.Text = value;
            }
            catch (Exception)
            { }
            try
            {
                ValueStateIDTxtBox.Text = stateID;
            }
            catch (Exception)
            { }
            foreach (string _value in valueChoices)
            {
                try
                {
                    ComboBoxItem i = new ComboBoxItem();
                    i.Content = _value;
                    ValueComboBox.Items.Add(i);
                }
                catch (Exception)
                { }
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

        //Find the active event
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
    }
}
