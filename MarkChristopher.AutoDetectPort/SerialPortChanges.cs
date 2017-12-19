using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MarkChristopher.AutoDetectPort
{
    public interface ISerialPortChanges
    {
        Form GetMe();
        void SetMe(Form value);
    }
    public class SerialPortChanges

    {
        public SerialPortChanges()
        {

        }

        private const int DBT_DEVTYP_PORT = 0x00000003;
        private const int WM_DEVICECHANGE = 0x0219;
        public bool tap;
        Thread thread;
        public void start(EventDeviceChange eventDeviceChange)
        {
            thread = new Thread(new ThreadStart(() =>
            {
                var window = new MessageWindow(eventDeviceChange);
                var handle = window.Handle;
                Application.Run();
            }));
            thread.Start();
        }

        public void Stop()
        {

            thread.Abort();
        }





        public class EventDeviceChange
        {
            public event EventHandler DeviceChange = delegate { };


            public void OnDeviceChange()
            {
                DeviceChange?.Invoke(this, EventArgs.Empty);
            }


        }

        public class MessageWindow : Form
        {
            EventDeviceChange EventDeviceChange;
            private const int WM_DEVICECHANGE = 0x219;
            private const int DBT_DEVICEARRIVAL = 0x8000;
            private const int DBT_DEVICEREMOVECOMPLETE = 0x8004;
            private const int DBT_DEVTYP_VOLUME = 0x00000002;
            public MessageWindow(EventDeviceChange EventDeviceChange)
            {
                this.EventDeviceChange = EventDeviceChange;
            }
            protected override void WndProc(ref Message m)
            {
                base.WndProc(ref m);
                switch (m.Msg)
                {
                    case WM_DEVICECHANGE:
                        switch ((int)m.WParam)
                        {
                            case DBT_DEVICEARRIVAL:
                                EventDeviceChange.OnDeviceChange();
                                break;
                            case DBT_DEVICEREMOVECOMPLETE:
                                EventDeviceChange.OnDeviceChange();
                                break;
                        }
                        EventDeviceChange.OnDeviceChange();
                        break;
                }


            }
        }
    }
    public class SerialPortChangesEventArgs : EventArgs
    {
        public bool Tap { get; set; }
    }
}
