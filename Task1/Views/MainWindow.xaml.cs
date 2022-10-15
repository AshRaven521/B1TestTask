using Microsoft.VisualBasic;
using Microsoft.Win32;
using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Windows;
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

        
        private Callbacks callbacks;

        private Stopwatch watcher;
        //private delegate void joinFilesCallback(int result);

        public MainWindow()
        {
            InitializeComponent();

            customString = new CustomString();

            callbacks = new Callbacks();

            watcher = new Stopwatch();
        }

        public void JoinFilesDone(int result)
        {
            watcher.Stop();
            MessageBox.Show($"Объединение файлов произошло успешно!" +
                        $"\nПонадобилось времени: {watcher.ElapsedMilliseconds / 1000} секунд" +
                        $"\nУдалено строк: {result}", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        public void GenerateFilesDone()
        {
            watcher.Stop();
            MessageBox.Show($"Файлы созданы успешно!" +
                $"\nПонадобилось времени: {watcher.ElapsedMilliseconds / 1000} секунд", 
                "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void beginFilesGenerationButton_Click(object sender, RoutedEventArgs e)
        {
            Callbacks.generateFilesCallback generateFilesCallback = new Callbacks.generateFilesCallback(GenerateFilesDone);

            if (Directory.Exists(folderWithFilesPath))
            {
                //var watcher = new Stopwatch();
                watcher.Restart();
                try
                {

                    //watcher.Start();
                    //FilesHandler.GenerateFiles(customString, folderWithFilesPath);
                    Thread generateFilesThread = new Thread(() => FilesHandler.GenerateAllFiles(customString, folderWithFilesPath, generateFilesCallback));
                    generateFilesThread.Start();
                    //watcher.Stop();
                    //MessageBox.Show($"Файлы созданы успешно!\nПонадобилось времени: {watcher.ElapsedMilliseconds / 1000} секунд", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);

                }
                catch (Exception ex)
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
            var text = Interaction.InputBox("Хотите ввести часть строки, которые будут удалены из файла?");

            Callbacks.joinFilesCallback callback = new Callbacks.joinFilesCallback(JoinFilesDone);

            if (Directory.Exists(folderWithFilesPath))
            {
                //var watcher = new Stopwatch();
                watcher.Restart();
                Thread joinFilesThread = new Thread(() => FilesHandler.JoinFiles(folderWithFilesPath, resultFilePath, text, callback));
                joinFilesThread.Start();
                //var (isGood, deletedStrings) = FilesHandler.JoinFiles(folderWithFilesPath, resultFilePath, text);
                //watcher.Stop();
                //if (isGood == true)
                //{
                //    MessageBox.Show($"Объединение файлов произошло успешно!" +
                //        $"\nПонадобилось времени: {watcher.ElapsedMilliseconds / 1000} секунд" +
                //        $"\nУдалено строк: {deletedStrings}", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
                //}
                //else
                //{
                //    MessageBox.Show("Произошла ошибка при объединении файлов!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                //}

            }

            else
            {
                MessageBox.Show("Не выбрана директория с итоговым файлом!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void searchButton_Click(object sender, RoutedEventArgs e)
        {
            //string searchParameter = searchStringTextBox.Text;
            if (File.Exists(resultFilePath))
            {
                //var groups = FilesHandler.SearchInFile(searchParameter, resultFilePath);
                //int deletedLines = FilesHandler.DeleteFromFile(resultFilePath, searchParameter);
                //MessageBox.Show($"Было удалено: {deletedLines} строк", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
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
