using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace TouchPortal_Plugin_Helper
{
    public partial class State_Editor : Page
    {
        public State_Editor()
        {
            InitializeComponent();
            Restore();
        }

        private void Grid_LostFocus(object sender, RoutedEventArgs e)
        {
            Backup();
        }

        //Drop down menu
        private void Label_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                if (!ValueChoiceComboBox.IsDropDownOpen)
                {
                    ValueChoiceComboBox.IsDropDownOpen = true;
                }
                else
                {
                    ValueChoiceComboBox.IsDropDownOpen = false;
                }
            }
        }

        //Define value choices
        private void DefineValueChoicesBtnClick(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new DefineValueChoicesState());
        }

        //Go back
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new DefineCategories());
        }

        private void Backup()
        {
            //ID
            try
            {
                File.WriteAllText($@"{Path.GetTempPath()}\TouchPortalPluginHelper\categories\category{FindActiveCategory()}\states\state{FindActiveState()}\id.TPPH", IDTxtBox.Text);
            }
            catch (Exception)
            { }
            //Type
            try
            {
                File.WriteAllText($@"{Path.GetTempPath()}\TouchPortalPluginHelper\categories\category{FindActiveCategory()}\states\state{FindActiveState()}\type.TPPH", TypeTxtBox.Text);
            }
            catch (Exception)
            { }
            //Description
            try
            {
                File.WriteAllText($@"{Path.GetTempPath()}\TouchPortalPluginHelper\categories\category{FindActiveCategory()}\states\state{FindActiveState()}\desc.TPPH", DescriptionTxtBox.Text);
            }
            catch (Exception)
            { }
            //Default
            try
            {
                File.WriteAllText($@"{Path.GetTempPath()}\TouchPortalPluginHelper\categories\category{FindActiveCategory()}\states\state{FindActiveState()}\default.TPPH", DefaultTxtBox.Text);
            }
            catch (Exception)
            { }
        }

        private void Restore()
        {
            string id = "", type = "", desc = "", def = "";
            string[] valueChoices = new string[0];

            //ID
            try
            {
                id = File.ReadAllText($@"{Path.GetTempPath()}\TouchPortalPluginHelper\categories\category{FindActiveCategory()}\states\state{FindActiveState()}\id.TPPH");
            }
            catch (Exception)
            { }
            //Type
            try
            {
                type = File.ReadAllText($@"{Path.GetTempPath()}\TouchPortalPluginHelper\categories\category{FindActiveCategory()}\states\state{FindActiveState()}\type.TPPH");
            }
            catch (Exception)
            { }
            //Description
            try
            {
                desc = File.ReadAllText($@"{Path.GetTempPath()}\TouchPortalPluginHelper\categories\category{FindActiveCategory()}\states\state{FindActiveState()}\desc.TPPH");
            }
            catch (Exception)
            { }
            //Default
            try
            {
                def = File.ReadAllText($@"{Path.GetTempPath()}\TouchPortalPluginHelper\categories\category{FindActiveCategory()}\states\state{FindActiveState()}\default.TPPH");
            }
            catch (Exception)
            { }
            //Value chocies
            try
            {
                valueChoices = File.ReadAllLines($@"{Path.GetTempPath()}\TouchPortalPluginHelper\categories\category{FindActiveCategory()}\states\state{FindActiveState()}\valueChoices.TPPH");
            }
            catch (Exception)
            { }

            //Update the UI
            //ID
            try
            {
                IDTxtBox.Text = id;
            }
            catch (Exception)
            { }
            //Type
            try
            {
                TypeTxtBox.Text = type;
            }
            catch (Exception)
            { }
            //Decsription
            try
            {
                DescriptionTxtBox.Text = desc;
            }
            catch (Exception)
            { }
            //Default
            try
            {
                DefaultTxtBox.Text = def;
            }
            catch (Exception)
            { }
            //Value Choices
            foreach (string _value in valueChoices)
            {
                try
                {
                    ComboBoxItem i = new ComboBoxItem();
                    i.Content = _value;
                    ValueChoiceComboBox.Items.Add(i);
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

        //Find the active state
        private string FindActiveState()
        {
            string _activeAction = null;
            try
            {
                _activeAction = File.ReadAllText($@"{Path.GetTempPath()}\TouchPortalPluginHelper\active_state.TPPH");
            }
            catch (Exception) { }
            return _activeAction;
        }
    }
}
