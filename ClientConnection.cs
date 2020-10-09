using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;

namespace SERTECFARMASL_PGOF_connector_IOFSfera
{
    class ClientConnection
    {
        private string HOST;
        private int PORT;
        private TcpClient client;
        private NetworkStream stream;
        private EndPoint remoteEndPoint;
        private string remoteAddress;
        private string remotePort;
        private EndPoint localEndPoint;
        private string localAddress;
        private string locaPort;
        private Printer printer;

        public ClientConnection(string host, int port)
        {
            HOST = host;
            PORT = port;
            printer = new Printer();
        }

        public void Connect()
        {
            try
            {
                if (this.client != null)
                {
                    Console.WriteLine($"CONNECTION ALREADY OPEN");
                }
                else
                {
                    this.client = new TcpClient(this.HOST, this.PORT);
                    stream = client.GetStream();

                    remoteEndPoint = client.Client.RemoteEndPoint;
                    string[] remoteAddressPort = remoteEndPoint.ToString().Split(':');
                    remoteAddress = remoteAddressPort[0];
                    remotePort = remoteAddressPort[1];

                    localEndPoint = client.Client.LocalEndPoint;
                    string[] localAddressPort = localEndPoint.ToString().Split(':');
                    localAddress = localAddressPort[0];
                    locaPort = localAddressPort[1];

                    Console.WriteLine($"CONNECTED : REMOTE END POINT : IP={remoteAddress}  PORT={remotePort} |  LOCA END POINT : IP={localAddress}  PORT={locaPort} ");
                    Listener(true);
                }
            }
            catch (ArgumentNullException e)
            {
                Console.WriteLine("ClientConnection: ArgumentNullException: {0}", e);
            }
            catch (SocketException ex)
            {
                Console.WriteLine($"ClientConnection: SocketException: {ex}");

            }
        }

        public void Close()
        {
            if (this.client == null)
            {
                Console.WriteLine($"CONNECTION IS NOT OPEN OR ALREADY CLOSE");
                return;

            }
            try
            {
                Listener(false);
                this.client.Close();
                Console.WriteLine($"CLOSED : REMOTE END POINT : IP={remoteAddress}  PORT={remotePort} |  LOCA END POINT : IP={localAddress}  PORT={locaPort} ");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ClientConnection: Exception: {ex}");

            }
            finally
            {
                this.client = null;
            }
        }


        private async Task Listener(bool Islisten)
        {
            byte[] bytes = new byte[256];
            string data = null;

            while (Islisten)
            {
                try
                {
                    data = null;
                    int i;
                    byte[] receiveBytes = new byte[client.ReceiveBufferSize];
                    int readBytes = stream.Read(receiveBytes, 0, receiveBytes.Length);
                    if (readBytes != 0)
                    {
                        Console.WriteLine($"RECEIVE: {Encoding.ASCII.GetString(receiveBytes, 0, readBytes)}");
                        printer.Print(Encoding.ASCII.GetString(receiveBytes, 0, readBytes));
                    }

                }
                catch (SocketException ex)
                {
                    Console.WriteLine($"ClientConnection: SocketException: {ex}");

                }
            }
        }

        public void Emit(string data)
        {
            if (client == null)
            {
                Console.WriteLine($"CONNECTION IS NOT OPEN OR ALREADY CLOSE - IMPOSSIBLE TO EMIT DATA");
                return;
            }

            // SEND
            byte[] sendBytes = ASCIIEncoding.ASCII.GetBytes(data);
            stream.Write(sendBytes, 0, sendBytes.Length);

            // RECEIVE
            byte[] receiveBytes = new byte[client.ReceiveBufferSize];
            int readBytes = stream.Read(receiveBytes, 0, receiveBytes.Length);
            Console.WriteLine($"RECEIVE: {Encoding.ASCII.GetString(receiveBytes, 0, readBytes)}");

        }
    }
}
