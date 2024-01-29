using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using TestDb34.Abstraction;

namespace TestDb34
{
    public class Client
    {
        private readonly IMessageSource _messageSource;
        private readonly IPEndPoint _peerEndPoint;
        private readonly string _name;

        public Client(IMessageSource messageSource, IPEndPoint peerEndPoint, string name)
        {
            _messageSource = messageSource;
            _peerEndPoint = peerEndPoint;
            _name = name;
        }

        private void Registered()
        {
            var messageJson = new MessageUDP() 
            {
                Command = Command.Register,
                FromName = _name,
            };

            _messageSource.SendMessage(messageJson, _peerEndPoint);

        }

        public void ClientSendler() 
        {
            

            while (true)
            {
                Console.WriteLine("Введите сообщение: ");
                string message = Console.ReadLine();

                Console.WriteLine("Введите имя получателя: ");
                string toUser = Console.ReadLine();

                if (string.IsNullOrEmpty(message) || string.IsNullOrEmpty(toUser))
                {
                    continue;
                }
                else
                {
                    var messageJson = new MessageUDP()
                    {
                        Text = message,
                        FromName = _name,
                        ToName = toUser 
                    };

                    _messageSource.SendMessage(messageJson, _peerEndPoint);
                }
            }
        }

        public void ClientListener()
        {
            Registered();
            IPEndPoint ep = new IPEndPoint(_peerEndPoint.Address, _peerEndPoint.Port);

            while (true)
            {
                
                var messageUdp = _messageSource.RecieveMessage(ref ep);

                Console.WriteLine(messageUdp.ToString());

            }

        }
    }
}
