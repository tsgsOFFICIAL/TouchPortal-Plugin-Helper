using System;
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
                if (!TypeComboBox.IsDropDownOpen)
                {
                    TypeComboBox.IsDropDownOpen = true;
                }
                else
                {
                    TypeComboBox.IsDropDownOpen = false;
                }
            }
        }

        private void Backup()
        {

        }

        private void Restore()
        {

        }
    }
}
