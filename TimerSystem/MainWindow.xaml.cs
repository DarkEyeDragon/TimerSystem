using System;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Threading;
using TimerSystem.ExcelHandler;
using TimerSystem.SerialCom;

namespace TimerSystem
{
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly AutoDetectCom _autoDetectCom;

        private readonly Settings _settingsWindow;
        private readonly Stopwatch _stopwatch;
        private readonly DispatcherTimer _timer;
        private Excel _excel;
        private InputHandler _inputHandler;

        public MainWindow()
        {
            InitializeComponent();
            _settingsWindow = new Settings();
            _settingsWindow.InitializeComponent();
            _stopwatch = new Stopwatch();
            _timer = new DispatcherTimer(DispatcherPriority.Normal);
            _timer.Interval = new TimeSpan(50000);
            _timer.Tick += Tick;
            _autoDetectCom = new AutoDetectCom(new TimeSpan(5000));
            _autoDetectCom.ComPortChanged += ComPortChanged;
        }

        private void SnaptimeEvent(object sender, SerialTimerEventArgs args)
        {
            if (args.High)
                Application.Current.Dispatcher.Invoke(() =>
                {
                    if (_excel == null) _excel = new Excel(_settingsWindow.TextBoxPath.Text, 1);
                });
        }

        private void TimerToggleEvent(object sender, SerialTimerEventArgs args)
        {
            if (args.High) Application.Current.Dispatcher.Invoke(ToggleTimer);
        }

        private void ComPortChanged(object sender, ComPortEventArgs args)
        {
            ComboBoxComPort.Items.Clear();
            foreach (var item in args.ComPortList.List) ComboBoxComPort.Items.Add(item);

            if (ComboBoxComPort.Items.Count == 0) return;
            ComboBoxComPort.SelectedItem = ComboBoxComPort.Items[0];
            _inputHandler = new InputHandler(ComboBoxComPort.Text);
            _inputHandler.SnapTime += SnaptimeEvent;
            _inputHandler.TimerToggle += TimerToggleEvent;
            try
            {
                _inputHandler.Open();
            }
            catch (IOException e)
            {
                MessageBox.Show(e.Message, e.Source, MessageBoxButton.OK, MessageBoxImage.Error);
            }
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
                _stopwatch.Stop();
                _timer.Stop();
                ButtonToggleState.Content = "Start";
            }
            //Timer is not running
            else
            {
                _stopwatch.Start();
                _timer.Start();
                ButtonToggleState.Content = "Stop";
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            _settingsWindow.ShowDialog();
        }
    }
}