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

        public MainWindow()
        {
            InitializeComponent();

            customString = new CustomString();

            callbacks = new Callbacks();

            watcher = new Stopwatch();
        }
        /// <summary>
        /// Function calling when joinFilesCallback delegate calls
        /// </summary>
        /// <param name="result"> Deleted strings </param>
        public void JoinFilesDone(int result)
        {
            watcher.Stop();
            MessageBox.Show($"Объединение файлов произошло успешно!" +
                        $"\nПонадобилось времени: {watcher.ElapsedMilliseconds / 1000} секунд" +
                        $"\nУдалено строк: {result}", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
        }
        /// <summary>
        /// Function calling when generateFilesCallback delegate calls
        /// </summary>
        public void GenerateFilesDone()
        {
            watcher.Stop();
            MessageBox.Show($"Файлы созданы успешно!" +
                $"\nПонадобилось времени: {watcher.ElapsedMilliseconds / 1000} секунд", 
                "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
        }
        /// <summary>
        /// Function calling when insertToDBCallback delegate calls
        /// </summary>
        /// <param name="lines"> Imported to database from result file lines </param>
        public void ImportToDBDone(int lines)
        {
            /* Invokes UI elements in Main thread */
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
        /// <summary>
        /// Function that handles beginGenerationButton
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void beginFilesGenerationButton_Click(object sender, RoutedEventArgs e)
        {
            Callbacks.generateFilesCallback generateFilesCallback = new Callbacks.generateFilesCallback(GenerateFilesDone);

            if (Directory.Exists(folderWithFilesPath))
            {
                watcher.Restart();
                try
                {
                    /* Call given function in another thread to not block a UI */
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
        /// <summary>
        /// Function that handles concatFilesButton
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void concatFilesButton_Click(object sender, RoutedEventArgs e)
        {
            var text = Interaction.InputBox("Хотите ввести часть строки, которые будут удалены из файла?");

            Callbacks.joinFilesCallback callback = new Callbacks.joinFilesCallback(JoinFilesDone);

            if (Directory.Exists(folderWithFilesPath))
            {
                watcher.Restart();
                /* Call given function in another thread to not block a UI */
                Thread joinFilesThread = new Thread(() => FilesHandler.JoinFiles(folderWithFilesPath, resultFilePath, text, callback));
                joinFilesThread.Start();
            }

            else
            {
                MessageBox.Show("Не выбрана директория с итоговым файлом!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        /// <summary>
        /// Function that handles selectResultFileButton
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void selectResultFileButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Пожалуйста учтите, что далее, при выборе итогового файла, все 100 файлов будут создан в папке с итоговым файлом!",
                            "Информация", MessageBoxButton.OK, MessageBoxImage.Information);

            var openFileDialog = new OpenFileDialog();
            /* Settings for file dialog */
            openFileDialog.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
            openFileDialog.Title = "Выберите файл, в который будут записаны все файлы из папки";
            openFileDialog.InitialDirectory = "c:\\";
            /* Open file dialog */
            if (openFileDialog.ShowDialog() == true)
            {
                resultFilePath = openFileDialog.FileName;
                folderWithFilesPath = System.IO.Path.GetDirectoryName(resultFilePath);
                searchInfoLabel.Content += $"{resultFilePath} )";
                searchInfoLabel.Visibility = Visibility.Visible;
            }
        }
        /// <summary>
        /// Function that handles importToDBButton
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void importToDBButton_Click(object sender, RoutedEventArgs e)
        {
            Callbacks.insertIntoDB dbCallback = new Callbacks.insertIntoDB(ImportToDBDone);
            /* Intervals for progress bar */
            progressBarSteps = maxFileLength * maxFilesCount / 10000;
            if (Directory.Exists(folderWithFilesPath))
            {

                var stringsList = await Task.Run(() => FilesHandler.GetCustomsFromFile(resultFilePath));
                resFileLength = stringsList.Count;
                /* Show progressbar and label  */
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
        /// <summary>
        /// Function that handles calculateValuesButton
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void calculateValuesButton_Click(object sender, RoutedEventArgs e)
        {
            watcher.Restart();

            var numbersSum = await Task.Run(() => DB.CalculateNumbersSum());
            var doublesMedian = await Task.Run(() => DB.CalculateDoubleNumbersMedian());

            watcher.Stop();
            /* Show labels  */
            numbersSumLabel.Visibility = Visibility.Visible;
            numbersSumValueLabel.Visibility = Visibility.Visible;

            doublesMedianLabel.Visibility = Visibility.Visible;
            doublesMedianValueLabel.Visibility = Visibility.Visible;

            numbersSumValueLabel.Content = numbersSum;
            doublesMedianValueLabel.Content = doublesMedian;

            MessageBox.Show($"Рассчет суммы средних и медианы дробных завершен успешно!" +
                    $"\nПонадобилось времени: {watcher.ElapsedMilliseconds / 1000 } секунд",
                    "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}
