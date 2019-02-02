using System;

namespace TimerSystem.SerialCom
{
    public class SerialTimerEventArgs : EventArgs
    {
        public bool High { get; set; }
    }
}