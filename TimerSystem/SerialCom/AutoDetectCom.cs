using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace TimerSystem.SerialCom
{
    public class ComPortEventArgs : EventArgs
    {
        public ComPortList ComPortList { get; set; }
    }


    class AutoDetectCom : DispatcherTimer
    {
        public delegate void ComportChangeEventHandler(object sender, ComPortEventArgs args);
        public event ComportChangeEventHandler ComPortChanged;

        public ComPortList Comports { get; set; }
        

        public AutoDetectCom(TimeSpan interval)
        {
            Comports = new ComPortList();
            Interval = interval;
            Tick += new EventHandler(TimerTick);
            Start();
        }

        private void TimerTick(object sender, EventArgs e)
        {
            var tempComports = SerialPort.GetPortNames();
            if (Comports.List == null || !tempComports.SequenceEqual(Comports.List))
            {
                Comports.List = tempComports;
                OnComPortChange();
            }
        }

        protected virtual void OnComPortChange()
        {
            ComPortChanged?.Invoke(this, new ComPortEventArgs { ComPortList = Comports });
        }
    }
}
