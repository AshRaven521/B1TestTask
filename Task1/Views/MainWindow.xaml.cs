using Microsoft.VisualBasic;
using Microsoft.Win32;
using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Windows;
using Task1.Models;
using Task1.Utils;
using Task1.Data;
using System.Threading.Tasks;

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

        private readonly int maxFileLength = 100000;
        private readonly int maxFilesCount = 100;
        private double progressBarSteps = 0.0;

        private int resFileLength = 0;

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

        public void ImportToDBDone(int lines)
        {
            Dispatcher.Invoke(() => importToDBProgressBar.Value++);
            Dispatcher.Invoke(() => importProgressValueLabel.Content = lines.ToString());

            if (lines >= resFileLength - 10000)
            {
                watcher.Stop();
                MessageBox.Show($"Импорт в базу данных завершен успешно!" +
                    $"\nПонадобилось времени: {watcher.ElapsedMilliseconds / 1000} секунд" +
                    $"\nСтрок импортировано: {lines}",
                    "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
            }

        }

        private void beginFilesGenerationButton_Click(object sender, RoutedEventArgs e)
        {
            Callbacks.generateFilesCallback generateFilesCallback = new Callbacks.generateFilesCallback(GenerateFilesDone);

            if (Directory.Exists(folderWithFilesPath))
            {
                watcher.Restart();
                try
                {
                    Thread generateFilesThread = new Thread(() => FilesHandler.GenerateAllFiles(customString, folderWithFilesPath, generateFilesCallback));
                    generateFilesThread.Start();
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
                watcher.Restart();
                Thread joinFilesThread = new Thread(() => FilesHandler.JoinFiles(folderWithFilesPath, resultFilePath, text, callback));
                joinFilesThread.Start();
            }

            else
            {
                MessageBox.Show("Не выбрана директория с итоговым файлом!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
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

        private async void importToDBButton_Click(object sender, RoutedEventArgs e)
        {
            Callbacks.insertIntoDB dbCallback = new Callbacks.insertIntoDB(ImportToDBDone);
            progressBarSteps = maxFileLength * maxFilesCount / 10000;
            if (Directory.Exists(folderWithFilesPath))
            {

                var stringsList = await Task.Run(() => FilesHandler.GetCustomsFromFile(resultFilePath));
                resFileLength = stringsList.Count;

                importToDBProgressBar.Visibility = Visibility.Visible;
                importProgressLabel.Visibility = Visibility.Visible;
                importToDBProgressBar.Value = 1;
                importToDBProgressBar.Minimum = 1;
                importToDBProgressBar.Maximum = progressBarSteps;

                watcher.Restart();

                Thread importToDBThread = new Thread(() => DB.InsertIntoDB(stringsList, dbCallback));
                importToDBThread.Start();
            }
            else
            {
                MessageBox.Show("Не выбрана директория с итоговым файлом!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
