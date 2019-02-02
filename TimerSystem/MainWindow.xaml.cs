using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using TimerSystem.SerialCom;

namespace TimerSystem
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Stopwatch _stopwatch;
        private DispatcherTimer _timer;
        private AutoDetectCom _autoDetectCom;
        private InputHandler _inputHandler;

        public MainWindow()
        {
            InitializeComponent();
            _stopwatch = new Stopwatch();
            _timer = new DispatcherTimer(DispatcherPriority.Normal);
            _timer.Interval = new TimeSpan(50000);
            _timer.Tick += new EventHandler(Tick);
            ButtonPause.IsEnabled = false;
            _autoDetectCom = new AutoDetectCom(new TimeSpan(5000));
            _autoDetectCom.ComPortChanged += ComPortChanged;
            _inputHandler = new InputHandler("COM3");
            _inputHandler.SnapTime += SnaptimeEvent;
            _inputHandler.TimerToggle += TimerToggleEvent;
        }

        private void SnaptimeEvent(object sender, SerialTimerEventArgs args)
        {
            if (args.High)
            {
                Application.Current.Dispatcher.Invoke(new Action(() =>
                {
                    throw new NotImplementedException();
                }));
            }
        }

        private void TimerToggleEvent(object sender, SerialTimerEventArgs args)
        {
            if (args.High)
            {
                Application.Current.Dispatcher.Invoke(new Action(() =>
                {
                    ToggleTimer();
                }));
            }
        }
        private void ComPortChanged(object sender, ComPortEventArgs args)
        {
            ComboBoxComPort.Items.Clear();
            foreach (var item in args.ComPortList.List)
            {
                ComboBoxComPort.Items.Add(item);
            }
            ComboBoxComPort.SelectedItem = ComboBoxComPort.Items[0];

        }

        private void Tick(object sender, EventArgs e)
        {
            LabelTime.Content = _stopwatch.Elapsed.ToString(@"m\:ss\:fff");
            LabelTime.InvalidateVisual();
            LabelTime.UpdateLayout();
        }

        private void ButtonToggleState_Click(object sender, RoutedEventArgs e)
        {
            ToggleTimer();
        }

        private void ToggleTimer()
        {
            //Timer is running
            if (_timer.IsEnabled)
            {
                ButtonPause.IsEnabled = false;
                _stopwatch.Stop();
                _timer.Stop();
                ButtonToggleState.Content = "Start";
            }
            //Timer is not running
            else
            {
                ButtonPause.IsEnabled = true;
                ButtonPause.Content = "Pause";
                _stopwatch.Start();
                _timer.Start();
                ButtonToggleState.Content = "Stop";

            }
        }
    }
}
