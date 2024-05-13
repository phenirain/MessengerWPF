using Messenger.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Messenger
{
    public class TcpClient
    {
        private Socket server;
        public ObservableCollection<string> Messages = new ObservableCollection<string>();
        public CancellationTokenSource TokenClient;
        public ObservableCollection<string> Users = new ObservableCollection<string>();

        public TcpClient(string name, string ip)
        {
            server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            server.Connect(ip, 1111);
            SendMessage(name);
            TokenClient = new CancellationTokenSource();
            RecieveMessage(TokenClient.Token);
        }

        public async Task SendMessage(string message)
        {
            var bytes = Encoding.UTF8.GetBytes(message);
            await server.SendAsync(new ArraySegment<byte>(bytes), SocketFlags.None);
        }

        private async Task RecieveMessage(CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                var bytes = new byte[1024];
                await server.ReceiveAsync(new ArraySegment<byte>(bytes), SocketFlags.None);
                var sortByte = bytes.Where(item => item != 0).ToArray();
                var message = Encoding.UTF8.GetString(sortByte);
                Messages.Add(message);
                //if (message.Substring(0, 5) != "/logs" && message != "/disconnect")
                //{
                //    Message.Add(Encoding.UTF8.GetString(bytes));
                //}
                //else if (message == "/disconnect")
                //{
                //    if (_viewModel.GetType() == typeof(ServerViewModel))
                //        (_viewModel as ServerViewModel).CloseWindow();
                //    else
                //        (_viewModel as ClientViewModel).CloseWindow();
                //}
                //else
                //{
                //    var obs = new ObservableCollection<string>(message.Split('\n'));
                //    obs.RemoveAt(0);
                //    Users.Clear();
                //    foreach (var item in obs)
                //    {
                //        Users.Add(item);
                //    }
                //}
            }

            server.Close();
        }
    }
}
