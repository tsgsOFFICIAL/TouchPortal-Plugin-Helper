using System;
using System.IO;
using System.Windows;
using System.Windows.Input;
using System.Windows.Controls;
using System.Windows.Navigation;
using System.Windows.Media.Imaging;

namespace TouchPortal_Plugin_Helper {
    public partial class Action_Editor : Page {
        public Action_Editor() {
            InitializeComponent();
            Restore();
            }

        private void Grid_LostFocus(object sender, RoutedEventArgs e) {
            Backup();
            }

        //Go back
        private void GoBackClick(object sender, RoutedEventArgs e) {

            }

        private void AddDataClick(object sender, RoutedEventArgs e) {
            NavigationService.Navigate(new AddActionData());
            }

        private void Label_MouseLeftButtonDown(object sender, MouseButtonEventArgs e) {
            if (e.LeftButton == MouseButtonState.Pressed) {
                if (ActionTypeComboBox.IsDropDownOpen) {
                    ActionTypeComboBox.IsDropDownOpen = false;
                    } else {
                    ActionTypeComboBox.IsDropDownOpen = true;
                    }
                }
            }

        //Find the active category
        private string FindActiveCategory() {
            string _activeCategory = null;
            try {
                _activeCategory = File.ReadAllText($@"{Path.GetTempPath()}\TouchPortalPluginHelper\active_category.TPPH");
                } catch (Exception) { }
            return _activeCategory;
            }

        //Find the active action
        private string FindActiveAction() {
            string _activeAction = null;
            try {
                _activeAction = File.ReadAllText($@"{Path.GetTempPath()}\TouchPortalPluginHelper\active_action.TPPH");
                } catch (Exception) { }
            return _activeAction;
            }

        //Take a backup
        private void Backup() {
            string _activeCategory = FindActiveCategory();
            string _activeAction = FindActiveAction();
            try {
                Directory.CreateDirectory($@"{Path.GetTempPath()}\TouchPortalPluginHelper\categories\category{_activeCategory}\actions\action{_activeAction}");
                } catch (Exception) { }
            try {
                File.WriteAllText($@"{Path.GetTempPath()}\TouchPortalPluginHelper\categories\category{_activeCategory}\actions\action{_activeAction}\name.TPPH", ActionNameTxtBox.Text);
                } catch (Exception) { }
            try {
                File.WriteAllText($@"{Path.GetTempPath()}\TouchPortalPluginHelper\categories\category{_activeCategory}\actions\action{_activeAction}\prefix.TPPH", ActionPrefixTxtBox.Text);
                } catch (Exception) { }
            try {
                File.WriteAllText($@"{Path.GetTempPath()}\TouchPortalPluginHelper\categories\category{_activeCategory}\actions\action{_activeAction}\id.TPPH", ActionIDTxtBox.Text);
                } catch (Exception) { }
            try {
                File.WriteAllText($@"{Path.GetTempPath()}\TouchPortalPluginHelper\categories\category{_activeCategory}\actions\action{_activeAction}\type.TPPH", ActionTypeComboBox.SelectedIndex.ToString());
                } catch (Exception) { }
            try {
                File.WriteAllText($@"{Path.GetTempPath()}\TouchPortalPluginHelper\categories\category{_activeCategory}\actions\action{_activeAction}\cmd.TPPH", ActionExecutionCMDTxtBox.Text);
                } catch (Exception) { }
            try {
                File.WriteAllText($@"{Path.GetTempPath()}\TouchPortalPluginHelper\categories\category{_activeCategory}\actions\action{_activeAction}\description.TPPH", ActionDescriptionTxtBox.Text);
                } catch (Exception) { }
            try {
                File.WriteAllText($@"{Path.GetTempPath()}\TouchPortalPluginHelper\categories\category{_activeCategory}\actions\action{_activeAction}\format.TPPH", ActionFormatTxtBox.Text);
                } catch (Exception) { }
            try {
                File.WriteAllText($@"{Path.GetTempPath()}\TouchPortalPluginHelper\categories\category{_activeCategory}\actions\action{_activeAction}\inline.TPPH", tryInlineCheckbox.IsChecked.ToString());
                } catch (Exception) { }
            }

        //Restore
        private void Restore() {
            string _activeCategory = FindActiveCategory();
            string _activeAction = FindActiveAction();

            string _name = "", _prefix = "", _id = "", _type = "", _cmd = "", _description = "", _format = "", _inline = "";

            //Retrieve the data
            try {
                _name = File.ReadAllText($@"{Path.GetTempPath()}\TouchPortalPluginHelper\categories\category{_activeCategory}\actions\action{_activeAction}\name.TPPH");
                } catch (Exception) {
                _name = null;
                }
            try {
                _prefix = File.ReadAllText($@"{Path.GetTempPath()}\TouchPortalPluginHelper\categories\category{_activeCategory}\actions\action{_activeAction}\prefix.TPPH");
                } catch (Exception) {
                _prefix = null;
                }
            try {
                _id = File.ReadAllText($@"{Path.GetTempPath()}\TouchPortalPluginHelper\categories\category{_activeCategory}\actions\action{_activeAction}\id.TPPH");
                } catch (Exception) {
                _id = null;
                }
            try {
                _type = File.ReadAllText($@"{Path.GetTempPath()}\TouchPortalPluginHelper\categories\category{_activeCategory}\actions\action{_activeAction}\type.TPPH");
                } catch (Exception) {
                _type = null;
                }
            try {
                _cmd = File.ReadAllText($@"{Path.GetTempPath()}\TouchPortalPluginHelper\categories\category{_activeCategory}\actions\action{_activeAction}\cmd.TPPH");
                } catch (Exception) {
                _cmd = null;
                }
            try {
                _description = File.ReadAllText($@"{Path.GetTempPath()}\TouchPortalPluginHelper\categories\category{_activeCategory}\actions\action{_activeAction}\description.TPPH");
                } catch (Exception) {
                _description = null;
                }
            try {
                _format = File.ReadAllText($@"{Path.GetTempPath()}\TouchPortalPluginHelper\categories\category{_activeCategory}\actions\action{_activeAction}\format.TPPH");
                } catch (Exception) {
                _format = null;
                }
            try {
                _inline = File.ReadAllText($@"{Path.GetTempPath()}\TouchPortalPluginHelper\categories\category{_activeCategory}\actions\action{_activeAction}\inline.TPPH");
                } catch (Exception) {
                _inline = null;
                }

            //Update the UI
            if (_name != null) {
                ActionNameTxtBox.Text = _name;
                }
            if (_prefix != null) {
                ActionPrefixTxtBox.Text = _prefix;
                }
            if (_id != null) {
                ActionIDTxtBox.Text = _id;
                }
            if (_type != null) {
                ActionTypeComboBox.SelectedIndex = Convert.ToInt32(_type);
                }
            if (_cmd != null) {
                ActionExecutionCMDTxtBox.Text = _cmd;
                }
            if (_description != null) {
                ActionDescriptionTxtBox.Text = _description;
                }
            if (_format != null) {
                ActionFormatTxtBox.Text = _format;
                }
            if (_inline != null) {
                if (_inline.ToLower().Equals("true")) {
                    tryInlineCheckbox.IsChecked = true;
                    } else {
                    tryInlineCheckbox.IsChecked = false;
                    }
                }
            }

        }
    }
