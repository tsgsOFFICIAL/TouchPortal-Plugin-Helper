using System;
using System.IO;
using System.Windows;
using System.Windows.Media;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Navigation;
using System.Collections.Generic;
using System.Security.Authentication.ExtendedProtection;

namespace TouchPortal_Plugin_Helper
{
    public partial class Edit : Page
    {
        public Edit()
        {
            InitializeComponent();
        }

        //Drag and drop
        private async void TextBlock_Drop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);


                if (files.Length == 1) //Only allow 1 file
                {
                    if (CheckFileType(files[0]) == "tp") //Only allow files of type: 'tp'
                    {
                        DragAndDropTextblock.Text = "Checking for errors..";
                        if (CheckDotTP())
                        {
                            //Delete Backup, since we're editing a new file!
                            try
                            {
                                Directory.Delete($@"{Path.GetTempPath()}\TouchPortalPluginHelper", true);
                            }
                            catch (Exception) { }

                            DragAndDropTextblock.Text = "Looking good! Just adding everything to the program ;)";

                            AddFromDotTP();
                            NavigationService.Navigate(new Main()); //Navigate to main again, once the 'entry.tp' file has been loaded all in
                        }
                        else
                        {
                            DragAndDropTextblock.Foreground = Brushes.Red;
                            DragAndDropTextblock.Text = "The file is not valid";
                            await Task.Delay(1250);
                            DragAndDropTextblock.Foreground = Brushes.GhostWhite;
                            DragAndDropTextblock.Text = "Drag and drop your 'entry.tp' file here";
                        }
                    }
                    else
                    {
                        DragAndDropTextblock.Foreground = Brushes.Red;
                        DragAndDropTextblock.Text = $"Only '.tp' files! You can't use a \"{CheckFileType(files[0])}\" file";
                        await Task.Delay(1250);
                        DragAndDropTextblock.Foreground = Brushes.GhostWhite;
                        DragAndDropTextblock.Text = "Drag and drop your 'entry.tp' file here";
                    }
                }
                else
                {
                    DragAndDropTextblock.Foreground = Brushes.Red;
                    DragAndDropTextblock.Text = "You can only edit 1 at a time";
                    await Task.Delay(1250);
                    DragAndDropTextblock.Foreground = Brushes.GhostWhite;
                    DragAndDropTextblock.Text = "Drag and drop your 'entry.tp' file here";
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="FullFileName">FullFileName means including the path as "C:\Users\secret Document.txt"</param>
        /// <returns></returns>
        private string CheckFileType(string FullFileName)
        {
            try
            {

                string _file = FullFileName.Substring(FullFileName.LastIndexOf('\\') + 1, FullFileName.Length - FullFileName.LastIndexOf('\\') - 1);
                string _filePath = FullFileName.Substring(0, FullFileName.Length - FullFileName.LastIndexOf('.'));



                //CONTINUE HERE
                string FilePath = FullFileName.Substring(0, FullFileName.LastIndexOf('\\') + 1); //Get the file path
                string[] _temp = FullFileName.Split('.'); //Split into _temp[0] == file name and _temp[1] == file extension
                string FileName = _temp[0].Substring(_temp[0].LastIndexOf('\\') + 1); //Get the filename
                string Extension = _temp[1]; //Get the extension

                return Extension; //Return the extension
            }
            catch (Exception)
            {
                return "folder";
            }
        }

        #region IMPORT
        //Check if the .tp file is valid
        private bool CheckDotTP()
        {

            return true;
        }

        //Add the stuff from the entry.tp file to the program
        private void AddFromDotTP()
        {

        }
        #endregion

        #region EXPORT
        //Export button click
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (!ExportLocation.Text.Trim().Equals(null) && !ExportLocation.Text.Trim().Equals(""))
            {
                if (CheckInternalTP())
                {
                    string version = "", pluginName = "", pluginID = "";
                    try
                    {
                        version = File.ReadAllText($@"{Path.GetTempPath()}\TouchPortalPluginHelper\backup\version.TPPH");
                    }
                    catch (Exception)
                    { }
                    try
                    {
                        pluginName = File.ReadAllText($@"{Path.GetTempPath()}\TouchPortalPluginHelper\backup\pluginName.TPPH");
                    }
                    catch (Exception)
                    { }
                    pluginID = $"{pluginName.ToLower()}_{version}";
                    //Plugin info
                    string export = "{\n" +
                        "\t\"sdk\":2,\n" +
                        $"\t\"version\":{version},\n" +
                        $"\t\"name\":\"{pluginName}\",\n" +
                        $"\t\"id\":\"{pluginID}\",\n";
                    //Configuration
                    if (!File.ReadAllText($@"{Path.GetTempPath()}\TouchPortalPluginHelper\backup\darkColor.TPPH").Trim('#', ' ').Equals("") && !File.ReadAllText($@"{Path.GetTempPath()}\TouchPortalPluginHelper\backup\lightColor.TPPH").Trim('#', ' ').Equals(""))
                    {
                        export += "\t\"configuration\":{\n" +
                            $"\t\t\"colorDark\":\"{File.ReadAllText($@"{Path.GetTempPath()}\TouchPortalPluginHelper\backup\darkColor.TPPH")}\",\n" +
                            $"\t\t\"colorLight\":\"{File.ReadAllText($@"{Path.GetTempPath()}\TouchPortalPluginHelper\backup\lightColor.TPPH")}\"\n" +
                            "\t},\n";
                    }
                    //Start CMD
                    if (!File.ReadAllText($@"{Path.GetTempPath()}\TouchPortalPluginHelper\backup\startCMD.TPPH").Trim().Equals(""))
                    {
                        export += $"\t\"plugin_start_cmd\":\"{File.ReadAllText($@"{Path.GetTempPath()}\TouchPortalPluginHelper\backup\startCMD.TPPH")}\",\n";
                    }
                    //Categories
                    export += "\t\"categories\":[\n" +
                            "\t\t{\n";

                    for (int i = 0; i < Directory.GetDirectories($@"{Path.GetTempPath()}\TouchPortalPluginHelper\categories").Length; i++)
                    {
                        //Category info
                        export += $"\t\t\t\"id\":\"{File.ReadAllText($@"{Path.GetTempPath()}\TouchPortalPluginHelper\categories\category{i}\id.TPPH")}\",\n" +
                            $"\t\t\t\"name\":\"{File.ReadAllText($@"{Path.GetTempPath()}\TouchPortalPluginHelper\categories\category{i}\name.TPPH")}\",\n";
                        //Category img
                        if (!File.ReadAllText($@"{Path.GetTempPath()}\TouchPortalPluginHelper\categories\category{i}\img.TPPH").Trim().Equals(""))
                        {
                            export += $"\t\t\t\"imagepath\":\"{File.ReadAllText($@"{Path.GetTempPath()}\TouchPortalPluginHelper\categories\category{i}\img.TPPH")}\",\n";
                        }
                        //Category actions
                        export += "\t\t\t\"actions\":[\n";
                        for (int j = 0; j < Directory.GetDirectories($@"{Path.GetTempPath()}\TouchPortalPluginHelper\categories\category{i}\actions").Length; j++)
                        {
                            export += "\t\t\t\t{\n";
                            string _type = File.ReadAllText($@"{Path.GetTempPath()}\TouchPortalPluginHelper\categories\category{i}\actions\action{j}\type.TPPH");
                            if (_type.Equals("0"))
                            {
                                _type = "execute";
                            }
                            else
                            {
                                _type = "communicate";
                            }
                            export += $"\t\t\t\t\t\"id\":\"{File.ReadAllText($@"{Path.GetTempPath()}\TouchPortalPluginHelper\categories\category{i}\actions\action{j}\id.TPPH")}\",\n" +
                                $"\t\t\t\t\t\"prefix\":\"{File.ReadAllText($@"{Path.GetTempPath()}\TouchPortalPluginHelper\categories\category{i}\actions\action{j}\prefix.TPPH")}\",\n" +
                                $"\t\t\t\t\t\"name\":\"{File.ReadAllText($@"{Path.GetTempPath()}\TouchPortalPluginHelper\categories\category{i}\actions\action{j}\name.TPPH")}\",\n" +
                                $"\t\t\t\t\t\"type\":\"{_type}\",\n";
                            //CMD
                            if (!File.ReadAllText($@"{Path.GetTempPath()}\TouchPortalPluginHelper\categories\category{i}\actions\action{j}\cmd.TPPH").Trim().Equals(""))
                            {
                                export += $"\t\t\t\t\t\"execution_cmd\":\"{File.ReadAllText($@"{Path.GetTempPath()}\TouchPortalPluginHelper\categories\category{i}\actions\action{j}\cmd.TPPH")}\",\n";
                            }
                            //Description
                            if (!File.ReadAllText($@"{Path.GetTempPath()}\TouchPortalPluginHelper\categories\category{i}\actions\action{j}\description.TPPH").Trim().Equals(""))
                            {
                                export += $"\t\t\t\t\t\"description\":\"{File.ReadAllText($@"{Path.GetTempPath()}\TouchPortalPluginHelper\categories\category{i}\actions\action{j}\description.TPPH")}\",\n";
                            }
                            //Format
                            if (!File.ReadAllText($@"{Path.GetTempPath()}\TouchPortalPluginHelper\categories\category{i}\actions\action{j}\format.TPPH").Trim().Equals(""))
                            {
                                export += $"\t\t\t\t\t\"format\":\"{File.ReadAllText($@"{Path.GetTempPath()}\TouchPortalPluginHelper\categories\category{i}\actions\action{j}\format.TPPH")}\",\n";
                            }
                            //Try inline
                            if (!File.ReadAllText($@"{Path.GetTempPath()}\TouchPortalPluginHelper\categories\category{i}\actions\action{j}\inline.TPPH").Trim().Equals(""))
                            {
                                export += $"\t\t\t\t\t\"tryInline\":{File.ReadAllText($@"{Path.GetTempPath()}\TouchPortalPluginHelper\categories\category{i}\actions\action{j}\inline.TPPH").ToLower()},\n";
                            }
                            //Data
                            if (Directory.Exists($@"{Path.GetTempPath()}\TouchPortalPluginHelper\categories\category{i}\actions\action{j}\data"))
                            {
                                export += $"\t\t\t\t\t\"data\":[\n";
                                for (int k = 0; k < Directory.GetDirectories($@"{Path.GetTempPath()}\TouchPortalPluginHelper\categories\category{i}\actions\action{j}\data").Length; k++)
                                {
                                    export += "\t\t\t\t\t\t{\n" +
                                        $"\t\t\t\t\t\t\t\"id\":\"{File.ReadAllText($@"{Path.GetTempPath()}\TouchPortalPluginHelper\categories\category{i}\actions\action{j}\data\data{k}\id.TPPH")}\",\n" +
                                        $"\t\t\t\t\t\t\t\"type\":\"{File.ReadAllText($@"{Path.GetTempPath()}\TouchPortalPluginHelper\categories\category{i}\actions\action{j}\data\data{k}\type.TPPH")}\",\n" +
                                        $"\t\t\t\t\t\t\t\"label\":\"{File.ReadAllText($@"{Path.GetTempPath()}\TouchPortalPluginHelper\categories\category{i}\actions\action{j}\data\data{k}\name.TPPH")}\",\n";
                                    if (File.ReadAllText($@"{Path.GetTempPath()}\TouchPortalPluginHelper\categories\category{i}\actions\action{j}\data\data{k}\type.TPPH").Trim().ToLower().Equals("text"))
                                    {
                                        export += $"\t\t\t\t\t\t\t\"default\":\"{File.ReadAllText($@"{Path.GetTempPath()}\TouchPortalPluginHelper\categories\category{i}\actions\action{j}\data\data{k}\defaultValue.TPPH")}\",\n";
                                    }
                                    else
                                    {
                                        export += $"\t\t\t\t\t\t\t\"default\":{File.ReadAllText($@"{Path.GetTempPath()}\TouchPortalPluginHelper\categories\category{i}\actions\action{j}\data\data{k}\defaultValue.TPPH")},\n";
                                    }
                                    export += "\t\t\t\t\t\t\t\"valueChoices\":[\n";
                                    foreach (string str in File.ReadAllLines($@"{Path.GetTempPath()}\TouchPortalPluginHelper\categories\category{i}\actions\action{j}\data\data{k}\choices.TPPH"))
                                    {
                                        export += $"\t\t\t\t\t\t\t\t\"{str}\",\n";
                                    }
                                    export += "\t\t\t\t\t\t\t],\n";
                                    //Extensions
                                    if (!File.ReadAllText($@"{Path.GetTempPath()}\TouchPortalPluginHelper\categories\category{i}\actions\action{j}\data\data{k}\dataExtension.TPPH").Trim().Equals(""))
                                    {
                                        export += $"\t\t\t\t\t\t\t\"extensions\":[\n";
                                        foreach (string str in File.ReadAllText($@"{Path.GetTempPath()}\TouchPortalPluginHelper\categories\category{i}\actions\action{j}\data\data{k}\dataExtension.TPPH").Trim().Split(','))
                                        {
                                            export += $"\t\t\t\t\t\t\t\t\"*.{str}\",\n";
                                        }
                                        export += "\t\t\t\t\t\t\t]\n";
                                    }
                                    export += "\t\t\t\t\t\t},\n";
                                }
                                export += "\t\t\t\t\t]\n"; //End of data
                            }
                            if (j == Directory.GetDirectories($@"{Path.GetTempPath()}\TouchPortalPluginHelper\categories\category{i}\actions").Length - 1)
                            {
                                export += "\t\t\t\t}\n" +
                                    "\t\t\t],\n"; //End of actions
                            }
                            else
                            {
                                export += "\t\t\t\t},\n"; //End of actions
                            }
                        }
                        //Events
                        export += "\t\t\t\"events\":[\n";
                        for (int j = 0; j < Directory.GetDirectories($@"{Path.GetTempPath()}\TouchPortalPluginHelper\categories\category{i}\events").Length; j++)
                        {
                            export += "\t\t\t\t{\n" +
                            $"\t\t\t\t\t\"id\":\"{File.ReadAllText($@"{Path.GetTempPath()}\TouchPortalPluginHelper\categories\category{i}\events\event{j}\id.TPPH")}\",\n" +
                            $"\t\t\t\t\t\"name\":\"{File.ReadAllText($@"{Path.GetTempPath()}\TouchPortalPluginHelper\categories\category{i}\events\event{j}\name.TPPH")}\",\n" +
                            $"\t\t\t\t\t\"format\":\"{File.ReadAllText($@"{Path.GetTempPath()}\TouchPortalPluginHelper\categories\category{i}\events\event{j}\format.TPPH")}\",\n" +
                            $"\t\t\t\t\t\"type\":\"{File.ReadAllText($@"{Path.GetTempPath()}\TouchPortalPluginHelper\categories\category{i}\events\event{j}\type.TPPH")}\",\n" +
                            $"\t\t\t\t\t\"valueType\":\"{File.ReadAllText($@"{Path.GetTempPath()}\TouchPortalPluginHelper\categories\category{i}\events\event{j}\value.TPPH")}\",\n" +
                            $"\t\t\t\t\t\"valueChoices\":[\n";

                            foreach (string str in File.ReadAllLines($@"{Path.GetTempPath()}\TouchPortalPluginHelper\categories\category{i}\events\event{j}\valueChoices.TPPH"))
                            {
                                export += $"\t\t\t\t\t\t\"{str}\",\n";
                            }
                            export += "\t\t\t\t\t],\n" +
                                $"\t\t\t\t\t\"valueStateId\":\"{File.ReadAllText($@"{Path.GetTempPath()}\TouchPortalPluginHelper\categories\category{i}\events\event{j}\stateID.TPPH")}\",\n";
                            if (j == Directory.GetDirectories($@"{Path.GetTempPath()}\TouchPortalPluginHelper\categories\category{i}\events").Length - 1)
                            {
                                export += "\t\t\t\t}\n" +
                                    "\t\t\t],\n"; //End of events
                            }
                            else
                            {
                                export += "\t\t\t\t},\n";
                            }
                        }
                        //States
                        export += "\t\t\t\"states\":[\n";
                        for (int j = 0; j < Directory.GetDirectories($@"{Path.GetTempPath()}\TouchPortalPluginHelper\categories\category{i}\states").Length; j++)
                        {
                            export += "\t\t\t\t{\n" +
                                $"\t\t\t\t\t\"id\":\"{File.ReadAllText($@"{Path.GetTempPath()}\TouchPortalPluginHelper\categories\category{i}\states\state{j}\id.TPPH")}\",\n" +
                                $"\t\t\t\t\t\"type\":\"{File.ReadAllText($@"{Path.GetTempPath()}\TouchPortalPluginHelper\categories\category{i}\states\state{j}\type.TPPH")}\",\n" +
                                $"\t\t\t\t\t\"desc\":\"{File.ReadAllText($@"{Path.GetTempPath()}\TouchPortalPluginHelper\categories\category{i}\states\state{j}\desc.TPPH")}\",\n" +
                                $"\t\t\t\t\t\"default\":\"{File.ReadAllText($@"{Path.GetTempPath()}\TouchPortalPluginHelper\categories\category{i}\states\state{j}\default.TPPH")}\",\n";

                            //Value Choices
                            if (!File.ReadAllText($@"{Path.GetTempPath()}\TouchPortalPluginHelper\categories\category{i}\states\state{j}\valueChoices.TPPH").Trim().Equals(""))
                            {
                                export += "\t\t\t\t\t\"valueChoices\":[\n";

                                foreach (string str in File.ReadAllLines($@"{Path.GetTempPath()}\TouchPortalPluginHelper\categories\category{i}\states\state{j}\valueChoices.TPPH"))
                                {
                                    export += $"\t\t\t\t\t\t\"{str}\",\n";
                                }
                                export += "\t\t\t\t\t],\n";
                            }
                            if (j == Directory.GetDirectories($@"{Path.GetTempPath()}\TouchPortalPluginHelper\categories\category{i}\states").Length - 1)
                            {
                                export += "\t\t\t\t}\n" +
                                    "\t\t\t]\n"; //End of states
                            }
                            else
                            {
                                export += "\t\t\t\t},\n";
                            }
                        }
                        //End of categories
                        export += "\t\t}\n" +
                            "\t]\n" +
                            "}";
                    }
                    try
                    {
                        File.WriteAllText(ExportLocation.Text, export);
                        MessageBox.Show("Successfully written!", "TouchPortal Plugin Helper", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    catch (Exception)
                    {
                        MessageBox.Show("Failed to write, try again or contact the developer", "TouchPortal Plugin Helper", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                else
                {
                    MessageBox.Show("Looks like your're missing something", "TouchPortal Plugin Helper", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("You need to enter a location for the export", "TouchPortal Plugin Helper", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private bool CheckInternalTP()
        {
            //Check if the required files are there
            if (!File.Exists($@"{Path.GetTempPath()}\TouchPortalPluginHelper\backup\pluginName.TPPH"))
            {
                return false;
            }

            if (!File.Exists($@"{Path.GetTempPath()}\TouchPortalPluginHelper\backup\version.TPPH"))
            {
                return false;
            }

            if (!Directory.Exists($@"{Path.GetTempPath()}\TouchPortalPluginHelper\categories"))
            {
                return false;
            }

            //Check if each category has the required files
            for (int i = 0; i < Directory.GetDirectories($@"{Path.GetTempPath()}\TouchPortalPluginHelper\categories").Length; i++)
            {
                if (!File.Exists($@"{Path.GetTempPath()}\TouchPortalPluginHelper\categories\category{i}\name.TPPH"))
                {
                    return false;
                }
                if (!File.Exists($@"{Path.GetTempPath()}\TouchPortalPluginHelper\categories\category{i}\id.TPPH"))
                {
                    return false;
                }

                if (!Directory.Exists($@"{Path.GetTempPath()}\TouchPortalPluginHelper\categories\category{i}\actions"))
                {
                    return false;
                }

                for (int j = 0; j < Directory.GetDirectories($@"{Path.GetTempPath()}\TouchPortalPluginHelper\categories\category{i}\actions").Length; j++)
                {
                    if (!File.Exists($@"{Path.GetTempPath()}\TouchPortalPluginHelper\categories\category{i}\actions\action{j}\name.TPPH"))
                    {
                        return false;
                    }
                    if (!File.Exists($@"{Path.GetTempPath()}\TouchPortalPluginHelper\categories\category{i}\actions\action{j}\prefix.TPPH"))
                    {
                        return false;
                    }
                    if (!File.Exists($@"{Path.GetTempPath()}\TouchPortalPluginHelper\categories\category{i}\actions\action{j}\id.TPPH"))
                    {
                        return false;
                    }
                    if (!File.Exists($@"{Path.GetTempPath()}\TouchPortalPluginHelper\categories\category{i}\actions\action{j}\type.TPPH"))
                    {
                        return false;
                    }
                }

                if (!Directory.Exists($@"{Path.GetTempPath()}\TouchPortalPluginHelper\categories\category{i}\events"))
                {
                    return false;
                }

                for (int j = 0; j < Directory.GetDirectories($@"{Path.GetTempPath()}\TouchPortalPluginHelper\categories\category{i}\events").Length; j++)
                {
                    if (!File.Exists($@"{Path.GetTempPath()}\TouchPortalPluginHelper\categories\category{i}\events\event{j}\name.TPPH"))
                    {
                        return false;
                    }
                    if (!File.Exists($@"{Path.GetTempPath()}\TouchPortalPluginHelper\categories\category{i}\events\event{j}\id.TPPH"))
                    {
                        return false;
                    }
                    if (!File.Exists($@"{Path.GetTempPath()}\TouchPortalPluginHelper\categories\category{i}\events\event{j}\format.TPPH"))
                    {
                        return false;
                    }
                    if (!File.Exists($@"{Path.GetTempPath()}\TouchPortalPluginHelper\categories\category{i}\events\event{j}\type.TPPH"))
                    {
                        return false;
                    }
                    if (!File.Exists($@"{Path.GetTempPath()}\TouchPortalPluginHelper\categories\category{i}\events\event{j}\stateID.TPPH"))
                    {
                        return false;
                    }
                    if (!File.Exists($@"{Path.GetTempPath()}\TouchPortalPluginHelper\categories\category{i}\events\event{j}\value.TPPH"))
                    {
                        return false;
                    }
                    if (!File.Exists($@"{Path.GetTempPath()}\TouchPortalPluginHelper\categories\category{i}\events\event{j}\valueChoices.TPPH"))
                    {
                        return false;
                    }
                }

                if (!Directory.Exists($@"{Path.GetTempPath()}\TouchPortalPluginHelper\categories\category{i}\states"))
                {
                    return false;
                }

                for (int j = 0; j < Directory.GetDirectories($@"{Path.GetTempPath()}\TouchPortalPluginHelper\categories\category{i}\states").Length; j++)
                {
                    if (!File.Exists($@"{Path.GetTempPath()}\TouchPortalPluginHelper\categories\category{i}\states\state{j}\name.TPPH"))
                    {
                        return false;
                    }
                    if (!File.Exists($@"{Path.GetTempPath()}\TouchPortalPluginHelper\categories\category{i}\states\state{j}\id.TPPH"))
                    {
                        return false;
                    }
                    if (!File.Exists($@"{Path.GetTempPath()}\TouchPortalPluginHelper\categories\category{i}\states\state{j}\type.TPPH"))
                    {
                        return false;
                    }
                    if (!File.Exists($@"{Path.GetTempPath()}\TouchPortalPluginHelper\categories\category{i}\states\state{j}\default.TPPH"))
                    {
                        return false;
                    }
                    if (!File.Exists($@"{Path.GetTempPath()}\TouchPortalPluginHelper\categories\category{i}\states\state{j}\desc.TPPH"))
                    {
                        return false;
                    }
                }
            }

            return true;
        }
        #endregion
    }
}
