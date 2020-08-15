using System;
using System.IO;
using System.Windows;
using System.Windows.Media;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace TouchPortal_Plugin_Helper {
    public partial class Edit : Page {
        public Edit() {
            InitializeComponent();
            }

        //Drag and drop
        private async void TextBlock_Drop(object sender, DragEventArgs e) {
            if (e.Data.GetDataPresent(DataFormats.FileDrop)) {
                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);

                //Only allow 1 file
                if (files.Length == 1) {
                    //Only allow files of type: 'tp'
                    if (CheckFileType(files[0]) == "tp") {
                        //Success, now add the elements of the .tp file to the program
                        //Delete Backup
                        try {
                            Directory.Delete($@"{Path.GetTempPath()}\TouchPortalPluginHelper", true);
                            } catch (Exception) { }

                        DragAndDropTextblock.Text = "Checking for errors.."; //Delete me
                        if (CheckDotTP()) {
                            DragAndDropTextblock.Text = "Looking good! Just adding everything to the program ;)"; //Delete me
                            AddFromDotTP();
                            NavigationService.Navigate(new Main());
                            } else {
                            DragAndDropTextblock.Text = "The file is not valid";
                            }
                        } else {
                        DragAndDropTextblock.Foreground = Brushes.Red;
                        DragAndDropTextblock.Text = $"Only '.tp' files! You can't use a \"{CheckFileType(files[0])}\" file";
                        await Task.Delay(2000);
                        DragAndDropTextblock.Text = "Drag and drop your 'entry.tp' file here";
                        DragAndDropTextblock.Foreground = Brushes.GhostWhite;
                        }
                    } else {
                    DragAndDropTextblock.Foreground = Brushes.Red;
                    DragAndDropTextblock.Text = "You can only edit 1 at a time";
                    await Task.Delay(2000);
                    DragAndDropTextblock.Text = "Drag and drop your 'entry.tp' file here";
                    DragAndDropTextblock.Foreground = Brushes.GhostWhite;
                    }

                }
            }

        //Check file type
        private string CheckFileType(string FullFileName) {
            string FilePath = FullFileName.Substring(0, FullFileName.LastIndexOf('\\') + 1); //Get the file path
            string[] _temp = FullFileName.Split('.'); //Split into _temp[0] == file name and _temp[1] == file extension
            string FileName = _temp[0].Substring(_temp[0].LastIndexOf('\\') + 1); //Get the filename
            string Extension = _temp[1]; //Get the extension

            return Extension; //Return the extension
            }

        //Check if the .tp file is valid
        private bool CheckDotTP() {

            return true;
            }

        //Add the stuff from the entry.tp file to the program
        private void AddFromDotTP() {

            }
        }
    }
