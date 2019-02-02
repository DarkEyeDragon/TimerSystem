using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Microsoft.Win32;

namespace TimerSystem
{
    /// <summary>
    /// Interaction logic for Settings.xaml
    /// </summary>
    public partial class Settings : Window
    {
        private readonly OpenFileDialog _fileDialog;

        public Settings()
        {
            InitializeComponent();
            _fileDialog = new OpenFileDialog();
            _fileDialog.FileName = "spreadsheet";
            _fileDialog.DefaultExt = ".xlsx";
            _fileDialog.Filter = "Excel documents (.xlsx)|*.xlsx";
            _fileDialog.FileOk += FileOkEvent;
        }

        private void FileOkEvent(object sender, CancelEventArgs e)
        {
            TextBoxPath.Text = _fileDialog.FileName;
        }

        private void Settings_OnClosing(object sender, CancelEventArgs e)
        {
            e.Cancel = true;
            Hide();
        }

        private void ButtonBrowse_Click(object sender, RoutedEventArgs e)
        {
            _fileDialog.ShowDialog();
        }
    }
}
