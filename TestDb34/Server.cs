using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using TestDb34.Models;
using TestDb34.Abstraction;

namespace TestDb34
{
    public class Server
    {

        private readonly IMessageSource _messageSource;
        private readonly IPEndPoint _peerEndPoint;
        private readonly string _name;

        public Server(IMessageSource messageSource, IPEndPoint peerEndPoint, string name)
        {
            _messageSource = messageSource;
            _peerEndPoint = peerEndPoint;
            _name = name;
        }

        Dictionary<String, IPEndPoint> clients = new Dictionary<string, IPEndPoint>();
        //UdpClient udpClient;

        void Register(MessageUDP message, IPEndPoint fromep)
        {
            Console.WriteLine("Message Register, name = " + message.FromName);
            clients.Add(message.FromName, fromep);


            using (var ctx = new Context())
            {
                if (ctx.Users.FirstOrDefault(x => x.Name == message.FromName) != null) return;

                ctx.Add(new User { Name = message.FromName });

                ctx.SaveChanges();
            }
        }

        void ConfirmMessageReceived(int? id)
        {
            Console.WriteLine("Message confirmation id=" + id);

            using (var ctx = new Context())
            {
                var msg = ctx.Messages.FirstOrDefault(x => x.Id == id);

                if (msg != null)
                {
                    msg.Received = true;
                    ctx.SaveChanges();
                }
            }
        }

        void RelyMessage(MessageUDP message)
        {
            int? id = null;
            if (clients.TryGetValue(message.ToName, out IPEndPoint ep))
            {
                using (var ctx = new Context())
                {
                    var fromUser = ctx.Users.First(x => x.Name == message.FromName);
                    var toUser = ctx.Users.First(x => x.Name == message.ToName);
                    var msg = new Message { FromUser = fromUser, ToUser = toUser, Received = false, Text = message.Text };
                    ctx.Messages.Add(msg);

                    ctx.SaveChanges();

                    id = msg.Id;
                }

                // Как я понимаю, вот так должно получиться
                _messageSource.SendMessage(message, ep);

                // Вместо ниже закоментированного)

                /*
                var forwardMessageJson = new MessageUDP() { Id = id, Command = Command.Message, ToName = message.ToName, FromName = message.FromName, Text = message.Text }.ToJson();

                byte[] forwardBytes = Encoding.ASCII.GetBytes(forwardMessageJson);

                udpClient.Send(forwardBytes, forwardBytes.Length, ep);*/
                
                Console.WriteLine($"Message Relied, from = {message.FromName} to = {message.ToName}");
                
            }
            else
            {
                Console.WriteLine("Пользователь не найден.");
            }
        }

        void ProcessMessage(MessageUDP message, IPEndPoint fromep)
        {
            Console.WriteLine($"Получено сообщение от {message.FromName} для {message.ToName} с командой {message.Command}:");
            Console.WriteLine(message.Text);


            if (message.Command == Command.Register)
            {
                Register(message, new IPEndPoint(fromep.Address, fromep.Port));

            }
            if (message.Command == Command.Confirmation)
            {
                Console.WriteLine("Confirmation receiver");
                ConfirmMessageReceived(message.Id);
            }
            if (message.Command == Command.Message)
            {
                RelyMessage(message);
            }
        }


        public void Work()
        {

            /*IPEndPoint remoteEndPoint;

            udpClient = new UdpClient(12345);
            remoteEndPoint = new IPEndPoint(IPAddress.Any, 0);*/

            IPEndPoint ep = new IPEndPoint(_peerEndPoint.Address, _peerEndPoint.Port);
            Console.WriteLine("UDP Клиент ожидает сообщений...");

            while (true)
            {
                var messageUdp = _messageSource.RecieveMessage(ref ep);

                Console.WriteLine(messageUdp.ToString());

                /*
                byte[] receiveBytes = udpClient.Receive(ref remoteEndPoint);
                string receivedData = Encoding.ASCII.GetString(receiveBytes);

                Console.WriteLine(receivedData);*/

                try
                {
                    //var message = MessageUDP.FromJson(receivedData);

                    ProcessMessage(messageUdp, ep);

                }
                catch (Exception ex)
                {
                    Console.WriteLine("Ошибка при обработке сообщения: " + ex.Message);
                }
            }

        }
    }

}
