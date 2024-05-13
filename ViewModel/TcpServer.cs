using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Messenger.Model;

namespace Messenger
{
    public class TcpServer
    {
        public readonly Socket socket;
        public Dictionary<Client, CancellationTokenSource> Clients = new Dictionary<Client, CancellationTokenSource>();
        //public ObservableCollection<string> ExtendedLogs = new();
        //public ObservableCollection<string> Logs = new();
        public CancellationTokenSource MainToken;

        public TcpServer()
        {
            var ipAddress = new IPEndPoint(IPAddress.Any, 1111);
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            socket.Bind(ipAddress);
            socket.Listen(100);
            MainToken = new CancellationTokenSource();
            ListenToClient(MainToken.Token);
        }

        private async Task ListenToClient(CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                var clientSocket = await socket.AcceptAsync();

                var bytes = new byte[1024];
                await clientSocket.ReceiveAsync(new ArraySegment<byte>(bytes), SocketFlags.None);
                var sortByte = bytes?.Where(x => x != 0).ToArray();
                var name = Encoding.UTF8.GetString(sortByte);
                if (name != "/disconnect")
                {
                    var client = new Client(clientSocket, name);
                    Clients.Add(client, new CancellationTokenSource());
                    //Logs.Add(client.name);
                    //ExtendedLogs.Add($"{client.name} - подключился\n{DateTime.Now.ToString()}");
                    //await SendLogsToClient();
                    ReceiveMessage(client.socket, Clients[client].Token);
                }
                //else
                //{
                //    await MailingMessage("/disconnect");
                //}
            }
        }

        private async Task ReceiveMessage(Socket client, CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                var bytes = new byte[1024];
                //await client.SocketClient.ReceiveAsync(bytes, SocketFlags.None);
                await client.ReceiveAsync(new ArraySegment<byte>(bytes), SocketFlags.None);
                var sortByte = bytes?.Where(x => x != 0).ToArray();
                var message = Encoding.UTF8.GetString(sortByte);
                SendMessage(client, message);
                if (message == "/disconnect")
                    Clients[client].Cancel();
                else
                    await MailingMessage($"[{DateTime.Now.ToString()}][{client.Name}]: {message}");
            }

            //Clients.Remove(client);
            //Logs.Remove(client.Name);
            //ExtendedLogs.Add($"{client.Name} - отключился\n{DateTime.Now.ToString()}");
            //await SendLogsToClient();
        }

        //private async Task SendLogsToClient()
        //{
        //    var logsString = "/logs\n" + string.Join("\n", Logs);
        //    await MailingMessage(logsString);
        //}

        private async Task SendMessage(Socket client, string message)
        {
            var bytes = Encoding.UTF8.GetBytes(message);
            await client.SendAsync(new ArraySegment<byte>(bytes), SocketFlags.None);
        }

        //private async Task MailingMessage(string message)
        //{
        //    foreach (var item in Clients.Keys) await SendMessage(item, message);
        //}
    }
}
