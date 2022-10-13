using System;
using System.Collections.Generic;
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
            var (res, time) = Generator.GenerateFiles(customString, "files");
            if (res == true)
            {
                MessageBox.Show($"Файлы созданы успешно!\nПонадобилось времени: {time} секунд", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show("Произошла ошибка при создании файлов!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
