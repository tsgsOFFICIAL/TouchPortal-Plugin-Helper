using System;
using System.IO;
using System.Net;
using System.Windows;
using System.Diagnostics;
using System.Windows.Input;
using System.Windows.Media;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using System.Threading.Tasks;
using System.Threading;
using System.Reflection;

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
            //SetActivePage("home");
            }

        //Go to actions page button
        public void Button_Click_DefineActions(object sender, RoutedEventArgs e)
            {
            Display.Content = new DefineCategories();
            //SetActivePage("category");
            Run = false;
            }

        //Go to edit page
        private void Button_Click_Edit(object sender, RoutedEventArgs e)
            {
            Display.Content = new Edit();
            //SetActivePage("edit");
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
            Ping pingSender = new Ping();
            string[] tempVersion = Assembly.GetExecutingAssembly().GetName().Version.ToString().Split('.');
            int[] _clientVersion = new int[4];
            for (int i = 0; i < _clientVersion.Length; i++)
                {
                _clientVersion[i] = int.Parse(tempVersion[i]);
                }
            List<int> _serverVersion = new List<int>();

            //Send a ping to see if there is a connection
            try
                {
                PingReply reply = pingSender.Send("8.8.8.8", 1000);

                if (reply.Status.ToString().Equals("Success"))
                    {
                    try
                        {
                        string[] _onlineVersion = Convert.ToString(client.DownloadString("https://eliteeleverne.dk/projects/TouchPortalPluginHelper/version.txt")).Split('.');

                        //Convert a list to an array
                        for (int i = 0; i < _onlineVersion.Length; i++)
                            {
                            _serverVersion.Add(Convert.ToInt32(_onlineVersion[i]));
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
                            return false; //Return true if a new update is available
                            }

                        //Else show you are up-to-date
                        UpdateButton.Content = "You are up-to-date!";
                        UpdateButton.ToolTip = $"You are on the newest version ({_clientVersion[0]}.{_clientVersion[1]}.{_clientVersion[2]}.{_clientVersion[3]})\nYou can click at anytime to check again";
                        UpdateButton.Foreground = Brushes.LawnGreen;
                        return false;
                        //Catch if the client.DownloadString failed, maybe the link changed or the server is down
                        }
                    catch (Exception) { }
                    }

                //Catch the ping exception
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
                client.DownloadFile("https://eliteeleverne.dk/projects/TouchPortalPluginHelper/Updater.exe", "Updater.exe");
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
