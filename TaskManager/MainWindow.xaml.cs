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
            foreach (var item in Process.GetProcesses().OrderBy(f => f.ProcessName))
            {
                Task task = new Task()
                {
                    TaskName = item.ProcessName,
                    TaskId = item.Id,
                    RAM = Convert.ToDouble(item.WorkingSet64) / 1024.0,
                    CPU = 100.0 / Convert.ToDouble(Process.GetProcesses().Length)
                };

                Tasks.Add(task);
            }
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var proc = Process.GetProcessById(SelectedTask.TaskId);

                proc.Kill();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                throw;
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
                throw;
            }
        }
    }
}
