using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace TimerSystem.SerialCom
{
    class InputHandler : SerialPort
    {
        public delegate void InputHandlerEventHandler(object sender, SerialTimerEventArgs args);

        public event InputHandlerEventHandler SnapTime;
        public event InputHandlerEventHandler TimerToggle;

        /// <summary>
        /// Set to true if you want to debounce
        /// </summary>
        public bool Debounce { get; set; }
        /// <summary>
        /// Set the debounce threshold in ms.
        /// </summary>
        public int DebounceThreshold { get; set; }

        private TimeSpan _debounceTimespan;

        private Stopwatch _debounceTimer;
        public InputHandler(string portName) : base(portName)
        {
            BaudRate = 9600;
            DataBits = 8;
            Parity = Parity.None;
            StopBits = StopBits.One;
            PinChanged += PinChangedEvent;
            DataReceived += DataReceivedEvent;
            ErrorReceived += ErrorReceivedEvent;
            DtrEnable = true;
            RtsEnable = true;
            _debounceTimer = new Stopwatch();
            _debounceTimespan = new TimeSpan(0,0,0,0,DebounceThreshold);
        }

        private void ErrorReceivedEvent(object sender, SerialErrorReceivedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void DataReceivedEvent(object sender, SerialDataReceivedEventArgs e)
        {
            throw new NotImplementedException();
        }

        protected virtual void PinChangedEvent(object sender, SerialPinChangedEventArgs e)
        {
            if (IsOpen)
            {
                try
                {
                    SnapTime?.Invoke(this, new SerialTimerEventArgs { High = DsrHolding });
                    TimerToggle?.Invoke(this, new SerialTimerEventArgs { High = CtsHolding });
                    _debounceTimer.Start();
                }
                catch (IOException ex)
                {
                    MessageBox.Show(ex.Message, ex.Source, MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
    }
}
