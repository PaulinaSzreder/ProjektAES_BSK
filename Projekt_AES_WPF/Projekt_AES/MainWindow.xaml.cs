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
using System.Security.Cryptography;

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
        Aes myAes = Aes.Create();
        byte[] encryptedText;
        string decryptedText;

        private void chooseFile_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
            {
                fileStream = new FileStream(openFileDialog.FileName, FileMode.Open);
            }
        }

        private void encryptFile_Click(object sender, RoutedEventArgs e)
        {
            string original = Input.Text;
               
            encryptedText = EncryptStringToBytes(original, myAes.Key, myAes.IV);

            SaveFileDialog saveFileDialog = new SaveFileDialog();
            if (saveFileDialog.ShowDialog() == true)
            {
                if (saveFileDialog.FileName != "")
                {
                    FileStream fs = new FileStream(saveFileDialog.FileName, FileMode.CreateNew);
                    fs.Write(encryptedText, 0, encryptedText.Length);
                    fs.Close();
                }
            }
            MessageBox.Show("Zaszyfrowano tekst");
        }

        private void decryptFile_Click(object sender, RoutedEventArgs e)
        {
            
            decryptedText = DescryptStringFromBytes(encryptedText, myAes.Key, myAes.IV);

            MessageBox.Show("Odszyfrowano tekst");
            MessageBox.Show(decryptedText);
        }


        // Wyrzuca wyjątek kiedy nic się nie wybierze, należy go obsłużyć TODO
        private void Encryption_Click(object sender, RoutedEventArgs e) 
        {
            try
            {
                mode = EncryptionMode.SelectedItem.ToString().Remove(0, EncryptionMode.SelectedItem.ToString().Length - 3);
            }
            catch(NullReferenceException ex)
            {
                MessageBox.Show("Nie wybrano metody szyfrowania!!!", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // Encrypt the string to an array of bytes.
        private static byte[] EncryptStringToBytes(string data, byte[] key, byte[] initVector)
        {
            byte[] encrypted;
            // Create an Aes object with the specified key and IV.
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = key;
                aesAlg.IV = initVector;

                // Create a decrytor to perform the stream transform.
                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                // Create the streams used for encryption.
                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                        {
                            //Write all data to the stream.
                            swEncrypt.Write(data);
                        }
                        encrypted = msEncrypt.ToArray();
                    }
                }
            }
            
            // Return the encrypted bytes from the memory stream.
            return encrypted;
        }

        // Decrypt the bytes to a string.
        static string DescryptStringFromBytes(byte[] cipherText, byte[] key, byte[] initVector)
        {
            string decryptedText = null;

            // Create an Aes object with the specified key and IV.
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = key;
                aesAlg.IV = initVector;

                // Create a decryptor to perform the stream transform.
                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                // Create the streams used for decryption.
                using (MemoryStream msDecrypt = new MemoryStream(cipherText))
                {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                        {

                            // Read the decrypted bytes from the decrypting stream and place them in a string.
                            decryptedText = srDecrypt.ReadToEnd();
                        }
                    }
                }

            }

            return decryptedText;
        }
    }
}
