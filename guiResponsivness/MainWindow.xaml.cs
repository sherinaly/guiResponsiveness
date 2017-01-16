using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace guiResponsivness
{
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        
        private static string _fileContents;

        public MainWindow()
        {
            InitializeComponent();
            _fileContents = "";

        }

     private void ReadFile_Click(object sender, RoutedEventArgs e)
        {
            var task = Task.Factory.StartNew(ReadFile);
        }

        private async void ReadFile()
        {
            Dispatcher.Invoke(() => loadingLabel.Content = "Loading");
            byte[] results;
            using (var fs = File.Open(Environment.CurrentDirectory + @"\test.txt", FileMode.Open))
            {
                results = new byte[fs.Length];
                await fs.ReadAsync(results, 0, (int) fs.Length);
            }

           
            _fileContents = Encoding.ASCII.GetString(results);
            Dispatcher.Invoke(() => loadingLabel.Content = "Done Loading");
        }

        private void WriteOnFile_Click(object sender, RoutedEventArgs e)
        {
            Task.Factory.StartNew(WriteToFile);
        }

        private async void WriteToFile()
        {
            if (_fileContents.Length > 0)
            {
                var uniEncoding = new UnicodeEncoding();
                var filename = Environment.CurrentDirectory + @"\testCopy.txt";

                using (var sourceStream = File.Open(filename, FileMode.Create))
                {
                    var result = uniEncoding.GetBytes(_fileContents);
                    sourceStream.Seek(0, SeekOrigin.End);
                    await sourceStream.WriteAsync(result, 0, result.Length);
                }
                Dispatcher.Invoke(() =>
                {
                    WritingTextBox.Text = _fileContents;
                    message.Content = "";
                });
            }
            else
            {
                Dispatcher.Invoke(() => message.Content += "Not Done Reading Yet\n");
            }
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
           StatusTetBox.Text = "I am doing Something Else....";
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            _fileContents = "";
            loadingLabel.Content = "";
            message.Content = "";
            WritingTextBox.Text = "";
            StatusTetBox.Text = "";
        }
    }
}