using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;

namespace WpfApplication1
{
    /// <summary>
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            this.originFileInfos = new List<FileInfo>();
            this.mappedFileNames = new List<FileNameMap>();
            this.button_reset.IsEnabled = false;
        }

        #region PRIVATE_METHOD
        private void Clearlist()
        {
            this.FileList.Items.Clear();
            this.originFileInfos.Clear();
            this.mappedFileNames.Clear();
            this.button_reset.IsEnabled = false;
        }

        private void button_preview_Click(object sender, RoutedEventArgs e)
        {
            Console.WriteLine("Button!!!");
            this.FileList.Items.Clear();
            int _count = 0;
            foreach (var fi in this.originFileInfos)
            {
                string _extension = Path.GetExtension(fi.FullName);
                _count++;
                string _newFileName = hopedNameFormat.Replace("<nn>", string.Format("{0:D2}", _count)) + _extension;
                string _newPath =  fi.DirectoryName + Path.DirectorySeparatorChar + _newFileName;
                Console.WriteLine(_newPath);
                this.mappedFileNames.Add(new FileNameMap(fi.FullName, _newPath));
                this.FileList.Items.Add(_newFileName);
            }

            this.button_reset.IsEnabled = true;
        }

        private void button_reset_Click(object sender, RoutedEventArgs e)
        {
            this.FileList.Items.Clear();
            foreach (var fi in this.originFileInfos)
            {
                Console.WriteLine(fi.FullName);
                this.FileList.Items.Add(fi.Name);
            }
            this.mappedFileNames.Clear();
        }

        private void button_rename_Click(object sender, RoutedEventArgs e)
        {
            this.button_reset.IsEnabled = false;

            foreach (var map in this.mappedFileNames)
            {
                File.Move(map.originFilePath, map.hopedFilePath);
            }


        }

        private void button_clear_Click(object sender, RoutedEventArgs e)
        {
            this.Clearlist();
        }

        private void FileList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            object _item = this.FileList.SelectedItem;
            if (_item == null)
                return;

            this.textBox.Text = _item.ToString();
        }

        private void FileList_PreviewDrop(object sender, DragEventArgs e)
        {
            Console.WriteLine("ListBox_PreviewDrop!!!");

            //MyFileList list = this.DataContext as MyFileList;

            string[] files = e.Data.GetData(DataFormats.FileDrop) as string[];
            if (files != null)
            {
                foreach (var path in files)
                {
                    FileInfo fi = new FileInfo(path);
                    (sender as ListBox).Items.Add(fi.Name);
                    this.originFileInfos.Add(fi);
                    //this.mappedFileNames.Add(new FileNameMap(fi.FullName));
                }
            }

        }

        private void FileList_Drop(object sender, DragEventArgs e)
        {
            Console.WriteLine("ListBox_Drop!!!");
            if (e.Data.GetDataPresent(DataFormats.FileDrop, true))
                e.Effects = DragDropEffects.Copy;
            else
                e.Effects = DragDropEffects.None;
            e.Handled = true;
        }



        private void textBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            hopedNameFormat = this.textBox.Text;
            // hopedNameFormat.Replace("<n>", "{0}");
        }
        #endregion

        #region PRIVATE_MEMBER
        List<FileInfo> originFileInfos;
        List<FileNameMap> mappedFileNames;
        string hopedNameFormat;
        #endregion
    }

    internal class FileNameMap
    {
        public string originFilePath;
        public string hopedFilePath;

        public FileNameMap(string org, string hope = null)
        {
            this.originFilePath = org;
            this.hopedFilePath = hope;
            Console.WriteLine(originFilePath);
            Console.WriteLine(hopedFilePath);
        }
    }

}
