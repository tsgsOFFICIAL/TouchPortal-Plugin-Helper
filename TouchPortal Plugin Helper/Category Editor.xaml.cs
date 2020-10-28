using System;
using System.IO;
using System.Windows;
using System.Windows.Media;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Navigation;
using System.Windows.Media.Imaging;

namespace TouchPortal_Plugin_Helper
{
    public partial class Category_Editor : Page
    {
        public Category_Editor()
        {
            InitializeComponent();
            Restore();
        }

        //Drag'n'Drop
        private async void ImageDrop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);

                //Only allow 1 file
                if (files.Length == 1)
                {
                    //Only allow images of type: 'png'
                    if (CheckFileType(files[0]) == "png")
                    {
                        CatIMG.Source = new BitmapImage(new Uri(files[0]));
                        CatIMGTxtBox.Text = files[0];
                        Backup();
                    }
                    else
                    {
                        CatIMGTxtBox.Foreground = Brushes.Red;
                        CatIMGTxtBox.Text = $"PNG's only! You can't use a \"{CheckFileType(files[0])}\" file";
                        await Task.Delay(2000);
                        CatIMGTxtBox.Text = "";
                        CatIMGTxtBox.Foreground = Brushes.GhostWhite;
                    }
                }
                else
                {
                    CatIMGTxtBox.Foreground = Brushes.Red;
                    CatIMGTxtBox.Text = "You can only have 1 image here!";
                    await Task.Delay(2000);
                    CatIMGTxtBox.Text = "";
                    CatIMGTxtBox.Foreground = Brushes.GhostWhite;
                }

            }
        }

        //When grid loses focus, its probably time to backup!
        private void Grid_LostFocus(object sender, RoutedEventArgs e)
        {
            Backup();
        }

        //Go back to define more categories
        private void GoBackClick(object sender, RoutedEventArgs e)
        {
            Backup();
            NavigationService.Navigate(new DefineCategories());
        }

        //e.Handled to enable textbox drag and drop
        private void TextBox_PreviewDragOver(object sender, DragEventArgs e)
        {
            e.Handled = true;
        }

        //Check file type
        private string CheckFileType(string FullFileName)
        {
            string FilePath = FullFileName.Substring(0, FullFileName.LastIndexOf('\\') + 1); //Get the file path
            string[] _temp = FullFileName.Split('.'); //Split into _temp[0] == file name and _temp[1] == file extension
            string FileName = _temp[0].Substring(_temp[0].LastIndexOf('\\') + 1); //Get the filename
            string Extension = _temp[1]; //Get the extension

            return Extension; //Return the extension
        }

        //Find the active category
        private string FindActiveCategory(bool ignoreName)
        {
            string _activeCategory = null;
            string _name = null;
            try
            {
                _activeCategory = File.ReadAllText($@"{Path.GetTempPath()}\TouchPortalPluginHelper\active_category.TPPH");
                _name = File.ReadAllText($@"{Path.GetTempPath()}\TouchPortalPluginHelper\categories\category{_activeCategory}\name.TPPH");
            }
            catch (Exception) { }
            //Set the active category
            if (_name != null && ignoreName == false)
            {
                CategoryName.Text = _name;
            }
            return _activeCategory;
        }

        //Take a backup
        private void Backup()
        {
            string _activeCategory = FindActiveCategory(true);
            try
            {
                if (CategoryName.Text.Trim().Equals(""))
                {
                    RestoreName();
                }
                File.WriteAllText($@"{Path.GetTempPath()}\TouchPortalPluginHelper\categories\category{_activeCategory}\name.TPPH", CategoryName.Text);
            }
            catch (Exception) { }
            try
            {
                File.WriteAllText($@"{Path.GetTempPath()}\TouchPortalPluginHelper\categories\category{_activeCategory}\id.TPPH", CategoryID.Text);
            }
            catch (Exception) { }
            try
            {
                File.WriteAllText($@"{Path.GetTempPath()}\TouchPortalPluginHelper\categories\category{_activeCategory}\img.TPPH", CatIMGTxtBox.Text);
            }
            catch (Exception) { }
        }

        //Backup the name
        private void BackupName()
        {
            string _activeCategory = FindActiveCategory(true);
            try
            {
                File.WriteAllText($@"{Path.GetTempPath()}\TouchPortalPluginHelper\categories\category{_activeCategory}\name.TPPH", CategoryName.Text);
            }
            catch (Exception) { }
        }

        //Restore
        private void Restore()
        {
            string _activeCategory = FindActiveCategory(false);
            string ActiveID;
            string ActiveIMG;
            try
            {
                ActiveID = File.ReadAllText($@"{Path.GetTempPath()}\TouchPortalPluginHelper\categories\category{_activeCategory}\id.TPPH");
            }
            catch (Exception)
            {
                ActiveID = null;
            }

            try
            {
                ActiveIMG = File.ReadAllText($@"{Path.GetTempPath()}\TouchPortalPluginHelper\categories\category{_activeCategory}\img.TPPH");
            }
            catch (Exception)
            {
                ActiveIMG = null;
            }


            //Set the active ID
            if (ActiveID != null)
            {
                CategoryID.Text = ActiveID;
            }
            //Set the active IMG
            if (ActiveIMG != null)
            {
                CatIMGTxtBox.Text = ActiveIMG;
                try
                {
                    CatIMG.Source = new BitmapImage(new Uri(ActiveIMG));
                }
                catch (Exception) { }
            }
        }

        //Restore the name
        private void RestoreName()
        {
            string _activeCategory = FindActiveCategory(true);
            try
            {
                string _name = File.ReadAllText($@"{Path.GetTempPath()}\TouchPortalPluginHelper\categories\category{_activeCategory}\name.TPPH");
                CategoryName.Text = _name;
            }
            catch (Exception) { }
        }

        private void CategoryName_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (CategoryName.Text.Trim().Equals(""))
            {
                //MessageBox.Show("It needs to have a name", "TouchPortal Plugin Helper", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else
            {
                BackupName();
            }
        }

    }
}