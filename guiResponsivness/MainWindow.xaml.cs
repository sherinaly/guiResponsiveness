using System;
using System.IO;
using System.Text;
using System.Threading;
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
        
        ReaderWriterLockSlim WritingLocker=new ReaderWriterLockSlim();

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
            try
            {
                using (var fs = File.Open(Environment.CurrentDirectory + @"\test.txt", FileMode.Open))
                {
                    results = new byte[fs.Length];
                    await fs.ReadAsync(results, 0, (int)fs.Length);
                }

                _fileContents = Encoding.ASCII.GetString(results);

                Dispatcher.Invoke(() =>
                {
                    loadingLabel.Content = "Done Loading";
                    WritingTextBox.Text = _fileContents;

                });
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
               // throw;
            }
            
        }

        private void WriteOnFile_Click(object sender, RoutedEventArgs e)
        {
            Task.Factory.StartNew(WriteToFile);
        }

        private  void WriteToFile()
        {
          
            if (_fileContents.Length > 0)
            {
                try
                {
                    var filename = Environment.CurrentDirectory + @"\testCopy.txt";

                    using (StreamWriter sourceStream = new StreamWriter(filename))
                    {
                        WritingLocker.EnterWriteLock();
                        sourceStream.Write(_fileContents);
                        WritingLocker.ExitWriteLock();
                    }

                    _fileContents = "";
                    Dispatcher.Invoke(() =>
                    {
                        message.Content = "";
                    });
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                  //  throw;
                }
                   
                   
           
                
             
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