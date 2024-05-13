using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Messenger.ViewModel
{
    internal class ServerViewModel: BindingHelper
    {
        private ObservableCollection<string> usersorlogs;
        public ObservableCollection<string> UsersOrLogs
        {
            get => usersorlogs; set => usersorlogs = value;
        }

        private ObservableCollection<string> messages;
        public ObservableCollection<string> Messages
        {
            get => messages; set => messages = value;
        }

        private string message;
        public string Message
        {
            get => message; set
            {
                message = value;
                OnPropertyChanged(Message);
            }
        }

        private TcpClient client;
        private TcpServer server;


        public ServerViewModel(string name) 
        {
            server = new TcpServer();
            client = new TcpClient(name, "127.0.0.1");
            Messages = client.Messages;
            UsersOrLogs = client.Users;
        }

        public void Switch(object sender, RoutedEventArgs e)
        {

        }

        public void Exit(object sender, RoutedEventArgs e)
        {

        }

        public async void Send(object sender, RoutedEventArgs e)
        {
            await client.SendMessage(Message);
            Message = "";
        }
    }
}
