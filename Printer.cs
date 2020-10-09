using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Ports;
using System.Drawing;
using System.Drawing.Printing;

namespace SERTECFARMASL_PGOF_connector_IOFSfera
{
    class Printer
    {
        private SerialPort serialPort = new SerialPort();
        private string portName;
        private int baudRate;
        public Printer()
        {
            portName = "COM3";
            baudRate = 38400;
        }
        public void InitializePrinter()
        {
            Console.WriteLine("InitializePrinter");
            serialPort.Write(Char.ConvertFromUtf32(27) + char.ConvertFromUtf32(64));
        }


        public void OpenPort()
        {
            serialPort.PortName = portName;
            serialPort.Encoding = Encoding.ASCII;
            serialPort.BaudRate = baudRate;
            serialPort.Parity = System.IO.Ports.Parity.None;
            serialPort.DataBits = 8;
            serialPort.StopBits = System.IO.Ports.StopBits.One;
            serialPort.DtrEnable = true;
            try
            {
                Console.WriteLine($"OPEN PORT : PORT={portName} | BAUDRATE={baudRate}");
                serialPort.Open();
            }
            catch (Exception ex)
            {
                serialPort.Close();
                serialPort.Open();
                Console.WriteLine(ex);
            }
        }

        public void OpenDrawer()
        {
            //27,112,48,55,121 https://bibase.com/epos.htm
            Console.WriteLine($"OPEN DRAWER");
            serialPort.Write(
               char.ConvertFromUtf32(27) +
               char.ConvertFromUtf32(112) +
               char.ConvertFromUtf32(48) +
               char.ConvertFromUtf32(55) +
               char.ConvertFromUtf32(121));
        }

        public void Print(string data)
        {
            PrintDocument printDocument = new PrintDocument();
            printDocument.PrintPage += delegate (object sender1, PrintPageEventArgs _event)
            {
                _event.Graphics.DrawString(data, new Font("Times New Roman", 12), new SolidBrush(Color.Black), new RectangleF(0, 0, printDocument.DefaultPageSettings.PrintableArea.Width, printDocument.DefaultPageSettings.PrintableArea.Height));
            };
            try
            {
                printDocument.Print();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Printer: Exception: {ex}");
            }
        }
    }
}
