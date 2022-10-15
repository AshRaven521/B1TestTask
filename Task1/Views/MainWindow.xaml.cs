using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using Task1.Models;
using Task1.Utils;

namespace Task1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private CustomString customString;

        private string folderWithFilesPath = string.Empty;
        private string resultFilePath = string.Empty;

        public MainWindow()
        {
            InitializeComponent();

            customString = new CustomString();

            
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void beginFilesGenerationButton_Click(object sender, RoutedEventArgs e)
        {
            if (Directory.Exists(folderWithFilesPath))
            {
                var watcher = new Stopwatch();

                try
                {
                    
                    watcher.Start();
                    FilesHandler.GenerateFiles(customString, folderWithFilesPath);
                    watcher.Stop();
                    MessageBox.Show($"Файлы созданы успешно!\nПонадобилось времени: {watcher.ElapsedMilliseconds / 1000} секунд", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);

                }
                catch(Exception ex)
                {
                    watcher.Stop();
                    MessageBox.Show($"Произошла ошибка при создании файлов!\nОшибка: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);

                }

            }

            else
            {
                MessageBox.Show("Не выбрана директория с итоговым файлом!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }

        private void concatFilesButton_Click(object sender, RoutedEventArgs e)
        {
            if (Directory.Exists(folderWithFilesPath))
            {
                var watcher = new Stopwatch();
                watcher.Start();
                var res = FilesHandler.JoinFiles(folderWithFilesPath, resultFilePath);
                watcher.Stop();
                if (res == true)
                {
                    MessageBox.Show($"Объединение файлов произошло успешно!\nПонадобилось времени: {watcher.ElapsedMilliseconds / 1000} секунд", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    MessageBox.Show("Произошла ошибка при объединении файлов!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }

            }

            else
            {
                MessageBox.Show("Не выбрана директория с итоговым файлом!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void searchButton_Click(object sender, RoutedEventArgs e)
        {
            string searchParameter = searchStringTextBox.Text;
            if (File.Exists(resultFilePath))
            {
                //var groups = FilesHandler.SearchInFile(searchParameter, resultFilePath);
                int deletedLines = FilesHandler.DeleteFromFile(resultFilePath, searchParameter);
                MessageBox.Show($"Было удалено: {deletedLines} строк", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show("Нет итогового файла для поиска!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void selectResultFile_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Пожалуйста учтите, что далее, при выборе итогового файла, все 100 файлов будут создан в папке с итоговым файлом!",
                            "Информация", MessageBoxButton.OK, MessageBoxImage.Information);

            var openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
            openFileDialog.Title = "Выберите файл, в который будут записаны все файлы из папки";
            openFileDialog.InitialDirectory = "c:\\";
            if (openFileDialog.ShowDialog() == true)
            {
                resultFilePath = openFileDialog.FileName;
                folderWithFilesPath = System.IO.Path.GetDirectoryName(resultFilePath);
                searchInfoLabel.Content += $"{resultFilePath} )";
                searchInfoLabel.Visibility = Visibility.Visible;
            }
        }
    }
}
