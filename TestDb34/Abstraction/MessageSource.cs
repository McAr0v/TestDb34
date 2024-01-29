using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace TestDb34.Abstraction
{
    internal class MessageSource : IMessageSource
    {
        private readonly UdpClient _udpClient;

        public MessageSource(int port) { 
        
            _udpClient = new UdpClient(port);

        }

        public MessageUDP RecieveMessage(ref IPEndPoint endPoint)
        {
            // Получаем сообщение
            byte[] data = _udpClient.Receive(ref endPoint);

            // Преобразуем его в строку json
            string json = Encoding.UTF8.GetString(data);

            return MessageUDP.FromJson(json);


        }

        public void SendMessage(MessageUDP message, IPEndPoint endPoint)
        {
            string json = message.ToJson();
            byte[] data = Encoding.UTF8.GetBytes(json);
            _udpClient.Send(data, data.Length, endPoint);
        }
    }
}
