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
using System.Windows.Shapes;
using System.Threading;

namespace SPNP
{
    /// <summary>
    /// Логика взаимодействия для SynchroWindow.xaml
    /// </summary>
    public partial class SynchroWindow : Window
    {
        private double sum;
        private int threadCount; // kilkist aktyvnuch potokiv
        public SynchroWindow()
        {
            InitializeComponent();
        }

        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            sum = 100;
            threadCount = 12;
            for(int i = 0; i < threadCount; i++)
            {
                new Thread(AddPercent).Start( new MonthData { Month = i+1});
            }
        }
        private void AddPercent(object? data)
        {
            var monthData = data as MonthData;
            Thread.Sleep(200);
            double localSum;
            lock (sumLocker)
            {
                localSum = 
                    sum = sum * 1.1;
            }

            Dispatcher.Invoke(() =>
            {
                LogTextBlock.Text += $"{monthData?.Month} {localSum}\n";
            });
            threadCount--;
            Thread.Sleep(1);
            if(threadCount == 0)
            {
                Dispatcher.Invoke(()=>
                    LogTextBlock.Text += $"----\nresult = {sum}");
            }
        }
        private void AddPercent4()
        {
            Thread.Sleep(200);

            lock (sumLocker)
            {
                sum = sum * 1.1;
            }

            Dispatcher.Invoke(() => {
                LogTextBlock.Text += $"{sum}\n";
            });
        }

        private void AddPercent2()
        {
            Thread.Sleep(200);
            double localSum = sum;
            localSum *= 1.1;
            sum = localSum;
            Dispatcher.Invoke(() => {
                LogTextBlock.Text += $"{sum}\n";
            });
        }

        private void AddPercent1()
        {
            double localSum = sum;
            Thread.Sleep(200);
            localSum *= 1.1;
            sum = localSum;
            Dispatcher.Invoke(() => {
                LogTextBlock.Text += $"{sum}\n";
            });
        }
        private object sumLocker = new(); //synchronisation object
        private void AddPercent3()
        {
            lock (sumLocker)
            {
                double localSum = sum;
                Thread.Sleep(200);
                localSum *= 1.1;
                sum = localSum;
                Dispatcher.Invoke(() =>
                {
                    LogTextBlock.Text += $"{sum}\n";
                });
            }
        }

        class MonthData
        {
            public int Month { set; get; }
        }
    }
}
