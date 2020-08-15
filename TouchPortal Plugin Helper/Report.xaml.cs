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
                Directory.CreateDirectory($@"{Path.GetTempPath()}\TouchPortalPluginHelper\temp");
                File.WriteAllText($@"{Path.GetTempPath()}\TouchPortalPluginHelper\temp\bug.TPPH", TxtField.Text);
                Task upload = UploadAsync("bug");
                await upload;
                TextBlockReportBugOrFeature.Text = "Thank you for sharing this idea with me!";
                await Task.Delay(2000);
                TextBlockReportBugOrFeature.Text = "Write your suggestion or bug report in this textfield and press either one of the buttons underneath";
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
                try
                    {
                    Directory.CreateDirectory($@"{Path.GetTempPath()}\TouchPortalPluginHelper\temp");
                    File.WriteAllText($@"{Path.GetTempPath()}\TouchPortalPluginHelper\temp\feature.TPPH", TxtField.Text);
                    Task upload = UploadAsync("feature");
                    await upload;
                    TxtField.Text = "";
                    }
                catch (Exception)
                    { }
                }
            else
                {
                MessageBox.Show("You need to enter something", "TouchPortal Plugin Helper", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }

        //FTP Uploader
        public async Task UploadAsync(string type)
            {
            string username = "tpph@eliteeleverne.dk";
            string password = "C€(-bwGY08FN";
            string url = "ftp.stackcp.com";
            string file = "";
            string fileDir = $@"{Path.GetTempPath()}\TouchPortalPluginHelper\temp";
            string _text = TxtField.Text;

            WebClient client = new WebClient();

            switch (type)
                {
                case "bug":
                    file = $"{type}.TPPH";
                    break;
                case "feature":
                    file = $"{type}.TPPH";
                    break;
                }

            //Download
            if (AnonymousCheckBox.IsChecked == true)
                {
                if (type.Equals("bug"))
                    {
                    try
                        {
                        _text = $"{client.DownloadString("https://eliteeleverne.dk/projects/TouchPortalPluginHelper/bug.TPPH")} \nDate: {DateTime.UtcNow.AddHours(2)}\nUser: Anonymous\nBug: {File.ReadAllText($@"{Path.GetTempPath()}\TouchPortalPluginHelper\temp\bug.TPPH")}\n";
                        File.WriteAllText($@"{Path.GetTempPath()}\TouchPortalPluginHelper\temp\bug.TPPH", _text);
                        }
                    catch (Exception) { }
                    }
                else if (type.Equals("feature"))
                    {
                    try
                        {
                        _text = $"{client.DownloadString("https://eliteeleverne.dk/projects/TouchPortalPluginHelper/feature.TPPH")} \nDate: {DateTime.UtcNow.AddHours(2)}\nUser: Anonymous\nFeature: {File.ReadAllText($@"{Path.GetTempPath()}\TouchPortalPluginHelper\temp\feature.TPPH")}\n";
                        File.WriteAllText($@"{Path.GetTempPath()}\TouchPortalPluginHelper\temp\feature.TPPH", _text);
                        }
                    catch (Exception) { }
                    }
                }
            else
                {
                if (type.Equals("bug"))
                    {
                    try
                        {
                        _text = $"{client.DownloadString("https://eliteeleverne.dk/projects/TouchPortalPluginHelper/bug.TPPH")} \nDate: {DateTime.UtcNow.AddHours(2)}\nUser: {Environment.UserName}\nBug: {File.ReadAllText($@"{Path.GetTempPath()}\TouchPortalPluginHelper\temp\bug.TPPH")}\n";
                        File.WriteAllText($@"{Path.GetTempPath()}\TouchPortalPluginHelper\temp\bug.TPPH", _text);
                        }
                    catch (Exception) { }
                    }
                else if (type.Equals("feature"))
                    {
                    try
                        {
                        _text = $"{client.DownloadString("https://eliteeleverne.dk/projects/TouchPortalPluginHelper/feature.TPPH")} \nDate: {DateTime.UtcNow.AddHours(2)}\nUser: {Environment.UserName}\nFeature: {File.ReadAllText($@"{Path.GetTempPath()}\TouchPortalPluginHelper\temp\feature.TPPH")}\n";
                        File.WriteAllText($@"{Path.GetTempPath()}\TouchPortalPluginHelper\temp\feature.TPPH", _text);
                        }
                    catch (Exception) { }
                    }
                }

            //Upload
            try
                {
                client.Credentials = new NetworkCredential(username, password);
                client.UploadFile($"ftp://{url}/{file}", $@"{fileDir}\{file}");
                TextBlockReportBugOrFeature.Text = "Thank you for sharing this with me!";
                await Task.Delay(2000);
                TextBlockReportBugOrFeature.Text = "Write your suggestion or bug report in this textfield and press either one of the buttons underneath";
                }
            catch (Exception)
                {
                MessageBox.Show("You need internet access to do this", "TouchPortal Plugin Helper", MessageBoxButton.OK, MessageBoxImage.Error);
                }

            try
                {
                File.Delete($@"{Path.GetTempPath()}\TouchPortalPluginHelper\temp\bug.TPPH");
                }
            catch (Exception) { }

            try
                {
                File.Delete($@"{Path.GetTempPath()}\TouchPortalPluginHelper\temp\feature.TPPH");
                }
            catch (Exception) { }
            await Task.Delay(0);
            }

        }
    }
