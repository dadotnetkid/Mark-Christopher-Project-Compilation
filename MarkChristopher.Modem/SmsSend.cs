using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MarkChristopher.Modem
{
    public class SmsSend
    {
        public string ContactNumber { get; set; }
        public string Message { get; set; }
        public string PortName { get; set; }
        public SmsSend(string ContactNumber, string Message, string PortName)
        {
            this.ContactNumber = ContactNumber;
            this.Message = Message;
            this.PortName = PortName;
        }

        public async Task<bool> SendAsync()
        {

            using (SerialPort sp = new SerialPort())
            {
                try
                {
                    sp.BaudRate = 0x2580;
                    sp.DataBits = 8;
                    sp.ReadBufferSize = 0x1000;
                    sp.ReadTimeout = 0x3e8;
                    sp.ReceivedBytesThreshold = 1;
                    sp.RtsEnable = true;
                    sp.StopBits = StopBits.One;
                    sp.WriteBufferSize = 0x800;
                    sp.WriteTimeout = -1;
                    sp.DtrEnable = false;
                    sp.Handshake = Handshake.None;
                    sp.Parity = Parity.None;
                    await Task.Run(new Action(() =>
                    {
                        sp.PortName = this.PortName;
                        sp.NewLine = Environment.NewLine;
                        sp.Open();
                        sp.Write("AT+CMGF=1" + Environment.NewLine);
                        Thread.Sleep(1000);
                        sp.Write(string.Concat(new object[] { "AT+CMGS=", '"', this.Message, '"', Environment.NewLine }));
                        Thread.Sleep(1000);
                        sp.Write(this.Message + '\x001a');
                        Thread.Sleep(1000);
                        sp.Close();
                    }));
                    return true;
                }
                catch (Exception)
                {
                    return false;
                }
            }

        }
    }
}
