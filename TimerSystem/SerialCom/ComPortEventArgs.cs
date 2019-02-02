using System;

namespace TimerSystem.SerialCom
{
    public class ComPortEventArgs : EventArgs
    {
        public ComPortList ComPortList { get; set; }
    }
}