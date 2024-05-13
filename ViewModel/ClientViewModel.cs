using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Messenger.ViewModel
{
    internal class ClientViewModel
    {
        private ObservableCollection<string> messages;
        public ObservableCollection<string> Messages;

        private ObservableCollection<string> users;
        public ObservableCollection<string> Users
        {
            get => users; set => users = value;
        }

        private string message;
        public string Message
        {
            get => message; set => message = value;
        }

        public ClientViewModel() 
        {
        
        }

        public void Exit(object sender, RoutedEventArgs e)
        {

        }

        public void Send(object sender, RoutedEventArgs e)
        {

        }
    }
}
