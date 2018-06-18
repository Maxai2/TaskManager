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

        private static Timer aTimer;

        public MainWindow()
        {
            InitializeComponent();

            DataContext = this;

            Tasks = new ObservableCollection<Task>();

            RefreshTaskList();

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
                foreach (var item in Process.GetProcesses())
                {
                    Task task = new Task()
                    {
                        TaskName = item.ProcessName,
                        TaskId = item.Id,
                        RAM = item.WorkingSet64
                    };

                    Tasks.Add(task);
                }
            });

        }

        void RefreshTaskList()
        {
            Tasks.Clear();

            foreach (var item in Process.GetProcesses())
            {
                Task task = new Task()
                {
                    TaskName = item.ProcessName,
                    TaskId = item.Id,
                    RAM = item.WorkingSet64
                };

                Tasks.Add(task);
            }
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
