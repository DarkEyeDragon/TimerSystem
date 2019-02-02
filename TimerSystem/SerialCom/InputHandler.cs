using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace TimerSystem.SerialCom
{
    class InputHandler : SerialPort
    {

        public delegate void SnapTimeEventHandler(object sender, SerialTimerEventArgs args);
        public delegate void ToggleTimerEventHandler(object sender, SerialTimerEventArgs args);
        public event SnapTimeEventHandler SnapTime;
        public event ToggleTimerEventHandler TimerToggle;


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
                    SnapTime?.Invoke(this, new SerialTimerEventArgs {High = DsrHolding});
                    TimerToggle?.Invoke(this, new SerialTimerEventArgs {High = CtsHolding});
                }
                catch(IOException ex)
                {
                    MessageBox.Show(ex.Message, ex.Source, MessageBoxButton.OK, MessageBoxImage.Error);
                } 
            }
        }
    }
}
