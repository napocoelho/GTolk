using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Net;
using System.Net.Sockets;

namespace SocketsDev.Protocol
{
    public class ServerHelper
    {
        public int Port { get; private set; }
        public TcpListener Listener { get; private set; }
        public bool IsRunning { get; private set; }
        public List<WeakReference<NetworkStream>> WeakList { get; private set; }

        public ServerHelper()
        {
            this.Port = 25999;
            this.Listener = null;
        }

        public void Start()
        {
            this.IsRunning = true;

            try
            {
                this.Listener = new TcpListener(IPAddress.Any, this.Port);
                this.Listener.Start();

                Task.Run(() =>
                    {
                        while (this.IsRunning)
                        {
                            try
                            {
                                TcpClient client = this.Listener.AcceptTcpClient();

                                Task.Run(() =>
                                    {
                                        this.EstablishConnection(client);
                                    });
                            }
                            catch (Exception e)
                            {
                                Console.WriteLine(e.ToString());
                            }
                            finally
                            {

                            }
                        }
                    });


            }
            catch (SocketException e)
            {
                throw e;
            }
        }

        private void EstablishConnection(TcpClient client)
        {
            try
            {
                using (client)
                {
                    using (NetworkStream stream = client.GetStream())
                    {
                        // WeakReference<NetworkStream> weak = new WeakReference<NetworkStream>(stream);
                        // this.WeakList.Add(weak);

                        ProtocolMessage message = Protocol.GTolkProtocol.Receive(stream);
                    }
                }
            }
            catch (Exception ex)
            {
            }
            finally
            {
            }
        }

        public void Stop()
        {
            this.IsRunning = false;
            this.Listener.Stop();
            this.Listener = null;
        }
    }
}