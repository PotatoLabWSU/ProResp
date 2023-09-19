using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlowMeterConsoleApp
{
    using System.Diagnostics.Tracing;
    using System.IO.Ports;
    using System.Text;
    public class FlowMeter
    {
        private SerialPort device;
        private string channel;

        public int WriteTimeout
        {
            get { return device.WriteTimeout; }
            set { device.ReadTimeout = value; }
        }
        public int ReadTimeout
        {
            get { return device.ReadTimeout; }
            set { device.ReadTimeout = value; }
        }
        public string DataHeader
        {
            get;
            private set;
        }

        public FlowMeter()
        {
            this.device = new SerialPort("COM4", 19200, Parity.None, 8, StopBits.One);
            this.WriteTimeout = 1000;
            this.ReadTimeout = 1000;
            this.channel = "A";
            this.device.Open();
        }

        public string Poll()
        {
            string response = string.Empty;

            //this.device.Open();
            this.WriteToDevice(channel);
            response = this.ReadFromDevice();
            //this.device.Close();
            return response;
        }

        private void WriteToDevice(string command)
        {
            if (!command.Contains("\r"))
            {
                command += "\r";
            }
            byte[] bytes = Encoding.ASCII.GetBytes(command);

            device.Write(bytes, 0, bytes.Length);
        }

        private string ReadFromDevice()
        {
            string response = string.Empty;

            try
            {
                byte[] data = new byte[1024];
                int bytesRead = device.Read(data, 0, data.Length);
                while (bytesRead > 0)
                {
                    response += Encoding.ASCII.GetString(data).TrimEnd((Char)0);
                    data = new byte[1024];
                    bytesRead = device.Read(data, 0, data.Length);
                }
                //if (bytesRead > 0)
                //{
                //    response = Encoding.ASCII.GetString(data);
                //}
            }
            catch (Exception ex)
            {
                //response = "Error: " + ex.Message;
            }

            return response;
        }
    }
}
