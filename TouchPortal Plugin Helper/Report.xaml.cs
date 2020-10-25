using System;
using System.IO;
using System.Net;
using System.Windows;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace TouchPortal_Plugin_Helper
{
    public partial class Report : Page
    {
        public Report()
        {
            InitializeComponent();
        }

        //Report a bug
        private async void ReportABug(object sender, RoutedEventArgs e)
        {
            if (!TxtField.Text.Trim().Equals(""))
            {
                Task upload = UploadAsync("Bug");
                await upload;
                TxtField.Text = "";
            }
            else
            {
                MessageBox.Show("You need to enter something", "TouchPortal Plugin Helper", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        //Send a feature request
        private async void SuggestAFeature(object sender, RoutedEventArgs e)
        {
            if (!TxtField.Text.Trim().Equals(""))
            {
                Task upload = UploadAsync("Feature");
                await upload;
                TxtField.Text = "";
            }
            else
            {
                MessageBox.Show("You need to enter something", "TouchPortal Plugin Helper", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        //Uploader
        public async Task UploadAsync(string type)
        {
            string person = Environment.UserName;
            if (AnonymousCheckBox.IsChecked == true)
            {
                person = "Anonymous";
            }
            string data = $"Date: {DateTime.UtcNow.AddHours(2).ToString(@"dd-MM-yyyy hh:mm:ss tt", new System.Globalization.CultureInfo("en-US"))}\nUser: {person}\n{type}: {TxtField.Text}\n";

            //Upload
            try
            {
                PHPMaster.GetPost(@"https://eliteeleverne.dk/FileStorage/projects/TouchPortalPluginHelper/tpph.php", "Username", "tpph", "Password", "ReporterYo", "Data", data, "Output", $"{type.ToLower()}.TPPH");
            }
            catch (Exception)
            {
                MessageBox.Show("You need internet access to do this", "TouchPortal Plugin Helper", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            await Task.Delay(0);
        }

    }
}
