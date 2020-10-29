using System;
using System.IO;
using System.Windows;
using System.Windows.Media;
using System.Windows.Controls;
using System.Windows.Navigation;
using System.Text.RegularExpressions;

namespace TouchPortal_Plugin_Helper
{
    public partial class Main : Page
    {
        public Main()
        {
            InitializeComponent();
            Restore();
        }
        private void CreateCategoriesClick(object sender, RoutedEventArgs e)
        {
            //Switch page in the frame to DefineCategories
            NavigationService.Navigate(new DefineCategories());
        }

        //Eventhandler for when grid loses focus
        private void Grid_LostFocus(object sender, RoutedEventArgs e)
        {
            Backup();
        }

        //Backup of data
        private void Backup()
        {
            try
            {
                Directory.CreateDirectory($@"{Path.GetTempPath()}\TouchPortalPluginHelper\backup"); //Create a backup directory
            }
            catch (Exception) { }

            File.WriteAllText($@"{Path.GetTempPath()}\TouchPortalPluginHelper\backup\pluginName.TPPH", TextBox_PluginName.Text);
            File.WriteAllText($@"{Path.GetTempPath()}\TouchPortalPluginHelper\backup\version.TPPH", TextBox_Version.Text);
            File.WriteAllText($@"{Path.GetTempPath()}\TouchPortalPluginHelper\backup\darkColor.TPPH", TextBox_DarkColor.Text);
            File.WriteAllText($@"{Path.GetTempPath()}\TouchPortalPluginHelper\backup\lightColor.TPPH", TextBox_LightColor.Text);
            File.WriteAllText($@"{Path.GetTempPath()}\TouchPortalPluginHelper\backup\startCMD.TPPH", TextBox_StartCMD.Text);
        }

        //Restore data
        private void Restore()
        {
            try
            {
                if (File.ReadAllText($@"{Path.GetTempPath()}\TouchPortalPluginHelper\backup\pluginName.TPPH") != null)
                {
                    TextBox_PluginName.Text = File.ReadAllText($@"{Path.GetTempPath()}\TouchPortalPluginHelper\backup\pluginName.TPPH");
                }
                if (File.ReadAllText($@"{Path.GetTempPath()}\TouchPortalPluginHelper\backup\version.TPPH") != null)
                {
                    TextBox_Version.Text = File.ReadAllText($@"{Path.GetTempPath()}\TouchPortalPluginHelper\backup\version.TPPH");
                }
                if (File.ReadAllText($@"{Path.GetTempPath()}\TouchPortalPluginHelper\backup\darkColor.TPPH") != null)
                {
                    TextBox_DarkColor.Text = File.ReadAllText($@"{Path.GetTempPath()}\TouchPortalPluginHelper\backup\darkColor.TPPH");
                    try
                    {
                        TextBox_DarkColor.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString(TextBox_DarkColor.Text));
                    }
                    catch (Exception)
                    {
                        TextBox_DarkColor.Background = new SolidColorBrush(Color.FromRgb(255, 255, 255));
                    }

                }
                if (File.ReadAllText($@"{Path.GetTempPath()}\TouchPortalPluginHelper\backup\lightColor.TPPH") != null)
                {
                    TextBox_LightColor.Text = File.ReadAllText($@"{Path.GetTempPath()}\TouchPortalPluginHelper\backup\lightColor.TPPH");
                    try
                    {
                        TextBox_LightColor.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString(TextBox_LightColor.Text));
                    }
                    catch (Exception)
                    {
                        TextBox_LightColor.Background = new SolidColorBrush(Color.FromRgb(255, 255, 255));
                    }
                }
                if (File.ReadAllText($@"{Path.GetTempPath()}\TouchPortalPluginHelper\backup\startCMD.TPPH") != null)
                {
                    TextBox_StartCMD.Text = File.ReadAllText($@"{Path.GetTempPath()}\TouchPortalPluginHelper\backup\startCMD.TPPH");
                }
            }
            catch (Exception) { }
        }

        private void ReportButtonClick(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Report());
        }

        //Only allow numbers for version input
        private void OnlyNumbers(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            e.Handled = !IsTextAllowed(e.Text);
        }

        private static bool IsTextAllowed(string text)
        {
            Regex _regex = new Regex("[^0-9.]+"); //regex that matches disallowed text
            return !_regex.IsMatch(text);
        }

        //ColorPickers
        //DarkColorPicker
        private void DarkColorPicker(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.ColorDialog clrDialog = new System.Windows.Forms.ColorDialog();

            //show the colour dialog and check that user clicked ok
            if (clrDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                //save the colour that the user chose
                TextBox_DarkColor.Text = "#" + clrDialog.Color.R.ToString("X2") + clrDialog.Color.G.ToString("X2") + clrDialog.Color.B.ToString("X2");
                TextBox_DarkColor.Background = new SolidColorBrush(Color.FromArgb(0xFF, clrDialog.Color.R, clrDialog.Color.G, clrDialog.Color.B));
            }
        }

        //LightColorPicker
        private void LightColorPicker(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.ColorDialog clrDialog = new System.Windows.Forms.ColorDialog();

            //show the colour dialog and check that user clicked ok
            if (clrDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                //save the colour that the user chose
                TextBox_LightColor.Text = "#" + clrDialog.Color.R.ToString("X2") + clrDialog.Color.G.ToString("X2") + clrDialog.Color.B.ToString("X2");
                TextBox_LightColor.Background = new SolidColorBrush(Color.FromArgb(0xFF, clrDialog.Color.R, clrDialog.Color.G, clrDialog.Color.B));
            }
        }

        private void TextBox_DarkColor_LostFocus(object sender, RoutedEventArgs e)
        {
            try
            {
                TextBox_DarkColor.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString(TextBox_DarkColor.Text));
            }
            catch (Exception)
            {

            }
        }

        private void TextBox_LightColor_LostFocus(object sender, RoutedEventArgs e)
        {
            try
            {
                TextBox_LightColor.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString(TextBox_LightColor.Text));
            }
            catch (Exception)
            {

            }
        }

        private void TextBox_DarkColor_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!TextBox_DarkColor.Text.Contains("#"))
            {
                string temp = TextBox_DarkColor.Text;
                TextBox_DarkColor.Text = "#" + temp;
            }
            try
            {
                TextBox_DarkColor.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString(TextBox_DarkColor.Text));
            }
            catch (Exception)
            {
                TextBox_DarkColor.Background = new SolidColorBrush(Color.FromRgb(255, 255, 255));
            }
        }

        private void TextBox_LightColor_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!TextBox_LightColor.Text.Contains("#"))
            {
                string temp = TextBox_LightColor.Text;
                TextBox_LightColor.Text = "#" + temp;
            }
            try
            {
                TextBox_LightColor.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString(TextBox_LightColor.Text));
            }
            catch (Exception)
            {
                TextBox_LightColor.Background = new SolidColorBrush(Color.FromRgb(255, 255, 255));
            }
        }
    }
}
