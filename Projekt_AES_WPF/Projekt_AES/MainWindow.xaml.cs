﻿using Microsoft.Win32;
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
        public static FileStream fileStreamEncrypted;
        Aes myAes = Aes.Create();
        byte[] encryptedText = null;
        string decryptedText;
        byte[] data;


        private void chooseFileClick(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
            {
                fileStream = new FileStream(openFileDialog.FileName, FileMode.Open);
                data = new byte[fileStream.Length];
                fileStream.Read(data, 0, (int)fileStream.Length);
                fileStream.Close();
            }
        }

        private void encryptFileClick(object sender, RoutedEventArgs e)
        {
            try
            {
                mode = EncryptionMode.SelectedItem.ToString().Remove(0, EncryptionMode.SelectedItem.ToString().Length - 3);

                string var = System.Text.Encoding.Default.GetString(data);

                encryptedText = encryptStringToBytes(var, myAes.Key, myAes.IV);

                MessageBox.Show("Zaszyfrowano plik!");

            }
            catch (NullReferenceException ex)
            {
                if (fileStream == null)
                    MessageBox.Show("Nie wybrano pliku do szyfrowania!!!", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                else
                    MessageBox.Show("Nie wybrano metody szyfrowania!!!", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void decryptFileClick(object sender, RoutedEventArgs e)
        {
            decryptedText = decryptStringFromBytes(encryptedText, myAes.Key, myAes.IV);

            MessageBox.Show("Odszyfrowano tekst");
            MessageBox.Show(decryptedText);
        }

        private void writeFileClick(object sender, RoutedEventArgs e)
        {
            if (encryptedText != null)
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                if (saveFileDialog.ShowDialog() == true)
                {
                    if (saveFileDialog.FileName != "")
                    {
                        fileStreamEncrypted = new FileStream(saveFileDialog.FileName, FileMode.Create);
                        fileStreamEncrypted.Write(encryptedText, 0, encryptedText.Length);
                        fileStreamEncrypted.Close();
                    }
                }
            }
        }
        

        // Encrypt the string to an array of bytes.
        private static byte[] encryptStringToBytes(string data, byte[] key, byte[] initVector)
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
        static string decryptStringFromBytes(byte[] cipherText, byte[] key, byte[] initVector)
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
