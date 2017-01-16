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
using System.Threading;
using System.IO;
using System.Runtime.CompilerServices;

namespace guiResponsivness
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

        private  void ReadFile_Click(object sender, RoutedEventArgs e)
        {
            
            Thread thread2 = new Thread(ReadFile);
            thread2.IsBackground = true;
            thread2.Start();
           

        }

     async void ReadFile()
        {
            byte[] results;
            using (var fs = File.Open(Environment.CurrentDirectory + @"\test.txt", FileMode.Open))
            {
                results = new byte[fs.Length];
               await  fs.ReadAsync(results, 0, (int) fs.Length);
                Dispatcher.Invoke(() =>
                 { InputTextBox.Text = Encoding.ASCII.GetString(results); });
            }
          //  MessageBox.Show("in reading");
         
        }
        private  void WriteOnFile_Click(object sender, RoutedEventArgs e)
        {
            Thread thread = new Thread(WriteToFile);
            thread.IsBackground = true;
            thread.Start();
        }

        async void WriteToFile()
        {
            var uniEncoding = new UnicodeEncoding();
            var filename = Environment.CurrentDirectory + @"\testCopy.txt";
            var text="";
            Dispatcher.Invoke(() =>
            {
                 text = WritingTextBox.Text;
                
            });

            using (var sourceStream = File.Open(filename, FileMode.Create))
            {
                var result = uniEncoding.GetBytes(text);
                sourceStream.Seek(0, SeekOrigin.End);
                await sourceStream.WriteAsync(result, 0, result.Length);
            }
        }
        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
          //  for (int i = 0; i < 100; i++)
            {
                StatusTetBox.Text = "I am doing Something Else....";
                
            }
           
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            InputTextBox.Text = ""; StatusTetBox.Text = "";
        }
    }
}
