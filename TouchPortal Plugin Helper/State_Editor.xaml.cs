using System;
using System.Windows;
using System.Windows.Controls;

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

        private void Backup()
        {

        }

        private void Restore()
        {

        }

        //Drop down menu
        private void Label_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {

        }

        //Go back
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new DefineCategories());
        }
    }
}
