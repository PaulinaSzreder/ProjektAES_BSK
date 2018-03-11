using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Projekt_AES
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        public static String mode;
        public static FileStream fileStream;
        

        private void chooseFile_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
            {
                fileStream = new FileStream(openFileDialog.FileName, FileMode.Open);
            }
        }

        // Wyrzuca wyjątek kiedy nic się nie wybierze, należy go obsłużyć TODO
        private void Encryption_Click(object sender, RoutedEventArgs e) 
        {
            try
            {
                mode = EncryptionMode.SelectedItem.ToString().Remove(0, EncryptionMode.SelectedItem.ToString().Length - 3);
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                if (saveFileDialog.ShowDialog() == true)
                {
                    if (saveFileDialog.FileName != "")
                    {
                        System.IO.FileStream fs = (System.IO.FileStream)saveFileDialog.OpenFile();
                    }
                }
            }
            catch(NullReferenceException ex)
            {
                MessageBox.Show("Nie wybrano metody szyfrowania!!!", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

       
    }
}
