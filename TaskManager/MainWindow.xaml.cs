using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using System.Diagnostics;
using System.Timers;
using System.Windows.Threading;

namespace TaskManager
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static ObservableCollection<Task> Tasks { get; set; }


        public Task SelectedTask { get; set; }

        private static Timer aTimer;

        public MainWindow()
        {
            InitializeComponent();

            AddProc.Focus();

            DataContext = this;

            Tasks = new ObservableCollection<Task>();

            FillTasks();

            aTimer = new Timer();
            aTimer.Interval = 5000;

            aTimer.Elapsed += OnTimedEvent;
            aTimer.AutoReset = true;
            aTimer.Enabled = true;
        }

        private void OnTimedEvent(Object source, System.Timers.ElapsedEventArgs e)
        {
            Dispatcher.Invoke(() =>
            {
                Tasks.Clear();
            });

            Dispatcher.Invoke(() =>
            {
                FillTasks();
            });

        }

        void FillTasks()
        {
            double procTime = GetTotalProcTime();

            try
            {
                foreach (var item in Process.GetProcesses().OrderBy(f => f.ProcessName))
                {
                    Task task = new Task()
                    {
                        TaskName = item.ProcessName,
                        TaskId = item.Id,
                        RAM = Convert.ToDouble(item.WorkingSet64) / 1024.0,
                        CPU = (100.0 * procTime) / item.TotalProcessorTime.Ticks
                    };

                    Tasks.Add(task);
                }
            }
            catch (Exception)
            {
                // MessageBox.Show(ex.Message);
            }
        }

        double GetTotalProcTime()
        {
            double procTime = 0.0;

            foreach (var item in Process.GetProcesses())
            {
                try
                {
                    procTime += item.TotalProcessorTime.Ticks;
                }
                catch (Exception)
                {
                    continue;
                }
            }

            return procTime;
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var proc = Process.GetProcessById(SelectedTask.TaskId);

                proc.Kill();
            }
            catch (Exception)
            {
                //MessageBox.Show(ex.Message);
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Process.Start(NewTaskName.Text);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
