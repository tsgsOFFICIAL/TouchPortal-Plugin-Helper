using System;
using System.IO;
using System.Net;
using System.Windows;
using System.Reflection;
using System.Diagnostics;
using System.Windows.Input;
using System.Windows.Media;
using System.Collections.Generic;

namespace TouchPortal_Plugin_Helper
{
    public partial class MainWindow : Window
    {
        public bool Run = true;
        public MainWindow()
        {
            InitializeComponent();
            CheckForUpdates();
            DeleteUpdater();
            DeleteBackup();
            Display.Content = new Main();
            //StartAsync();
        }

        //Minimize button
        public void Button_Click_Minimize(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        //Close button
        public void Button_Click_Close(object sender, RoutedEventArgs e)
        {
            DeleteBackup();
            this.Close();
        }

        //Drag header
        public void WindowHeader_Mousedown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        //Return home
        public void Button_Click_ReturnHome(object sender, RoutedEventArgs e)
        {
            Display.Content = new Main();
        }

        //Go to actions page button
        public void Button_Click_DefineActions(object sender, RoutedEventArgs e)
        {
            Display.Content = new DefineCategories();
            Run = false;
        }

        //Go to edit page
        private void Button_Click_Edit(object sender, RoutedEventArgs e)
        {
            Display.Content = new Edit();
            Run = false;
        }

        //Update button
        private void UpdateButtonClick(object sender, RoutedEventArgs e)
        {
            if ((string)UpdateButton.Content != "Update Now")
            {
                CheckForUpdates();
            }
            else
            {
                if (CheckForUpdates())
                {
                    UpdateProgram();
                }
            }
        }

        //Check for updates
        private bool CheckForUpdates()
        {
            //Create a new instance of WebClient, and search for the current version
            WebClient client = new WebClient();
            List<int> _serverVersion = new List<int>();
            int[] _clientVersion = new int[4];
            string[] version = FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location).FileVersion.Split('.');

            for (int i = 0; i < _clientVersion.Length; i++)
            {
                _clientVersion[i] = int.Parse(version[i]);
            }

            try
            {
                //Convert the array to a list
                foreach (string onlineVersion in client.DownloadString("https://eliteeleverne.dk/FileStorage/projects/TouchPortalPluginHelper/version.txt").Split('.')) {
                    _serverVersion.Add(Convert.ToInt32(onlineVersion));
                }

                //Check if the update is newer
                if ((_clientVersion[0] < _serverVersion[0]) || (_clientVersion[1] < _serverVersion[1] && _clientVersion[0] <= _serverVersion[0]) || (_clientVersion[2] < _serverVersion[2] && _clientVersion[1] <= _serverVersion[1] && _clientVersion[0] <= _serverVersion[0]) || (_clientVersion[3] < _serverVersion[3] && _clientVersion[2] <= _serverVersion[2] && _clientVersion[1] <= _serverVersion[1] && _clientVersion[0] <= _serverVersion[0]))
                {
                    UpdateButton.Content = "Update Now";
                    UpdateButton.ToolTip = $"Version {_serverVersion[0]}.{_serverVersion[1]}.{_serverVersion[2]}.{_serverVersion[3]} is now available!\nYou're on version {_clientVersion[0]}.{_clientVersion[1]}.{_clientVersion[2]}.{_clientVersion[3]}\nClick to update now";
                    UpdateButton.Foreground = Brushes.Orange;
                    return true; //Return true if a new update is available
                }

                //Else check if the user is on a newer build than the server
                if ((_clientVersion[0] > _serverVersion[0]) || (_clientVersion[1] > _serverVersion[1] && _clientVersion[0] >= _serverVersion[0]) || (_clientVersion[2] > _serverVersion[2] && _clientVersion[1] >= _serverVersion[1] && _clientVersion[0] >= _serverVersion[0]) || (_clientVersion[3] > _serverVersion[3] && _clientVersion[2] >= _serverVersion[2] && _clientVersion[1] >= _serverVersion[1] && _clientVersion[0] >= _serverVersion[0]))
                {
                    UpdateButton.Content = "You're on a dev build";
                    UpdateButton.ToolTip = $"Woooo! Look at you, you're on a dev build, version: {_clientVersion[0]}.{_clientVersion[1]}.{_clientVersion[2]}.{_clientVersion[3]}\nBe careful, Dev builds don't tend to be as stable.. ;)";
                    UpdateButton.Foreground = Brushes.GreenYellow;
                    return false;
                }
                UpdateButton.Content = "You are up-to-date!";
                UpdateButton.ToolTip = $"You are on the newest version ({_clientVersion[0]}.{_clientVersion[1]}.{_clientVersion[2]}.{_clientVersion[3]})\nYou can click at anytime to check again";
                UpdateButton.Foreground = Brushes.LawnGreen;
                return false;
                //Catch if the client.DownloadString failed, maybe the link changed or the server is down
            }
            catch (Exception) { }
            UpdateButton.Foreground = Brushes.Red;
            UpdateButton.Content = "Could not establish a connection";
            UpdateButton.ToolTip = $"You are on version ({_clientVersion[0]}.{_clientVersion[1]}.{_clientVersion[2]}.{_clientVersion[3]})";
            return false;
        }

        //Update the program
        private bool UpdateProgram()
        {
            WebClient client = new WebClient();

            try
            {
                client.DownloadFile("https://eliteeleverne.dk/FileStorage/projects/TouchPortalPluginHelper/Updater.exe", "Updater.exe");
            }
            catch (Exception e)
            {
                MessageBox.Show($"Update wasn't found \"{e.Message}\"", "TouchPortal Plugin Helper", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            //Run the updater
            Process.Start("Updater.exe");
            Close();
            return true;

        }

        //DeleteUpdater
        private void DeleteUpdater()
        {
            //Delete updater.exe
            File.Delete("Updater.exe");
        }

        //DeleteBackup
        private void DeleteBackup()
        {
            //Delete Backup
            try
            {
                Directory.Delete($@"{Path.GetTempPath()}\TouchPortalPluginHelper", true);
            }
            catch (Exception) { }
        }
    }
}
