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
        
        private static object _fileContents;

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
            
            Dispatcher.Invoke(() =>
            {
                loadingLabel.Content = "Done Loading";
                WritingTextBox.Text = _fileContents.ToString();
              
            });
        }

        private void WriteOnFile_Click(object sender, RoutedEventArgs e)
        {
            Task.Factory.StartNew(WriteToFile);
        }

        private async void WriteToFile()
        {
            if (_fileContents.ToString().Length > 0)
            {
                var uniEncoding = new UnicodeEncoding();
                var filename = Environment.CurrentDirectory + @"\testCopy.txt";

                using (var sourceStream = File.Open(filename, FileMode.Create))
                {
                    var result = uniEncoding.GetBytes(_fileContents.ToString());
                    sourceStream.Seek(0, SeekOrigin.End);
                    await sourceStream.WriteAsync(result, 0, result.Length);
                }
                _fileContents = "";
                Dispatcher.Invoke(() =>
                {
                    message.Content = "";
                });
            }
            else
            {
                Dispatcher.Invoke(() => message.Content += "Wait till Read is Done \nor Press Load again.\n");
            }
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
           StatusTetBox.Text = "I am doing Something Else....";
        }

       
    }
}