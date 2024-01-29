using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace TestDb34.Abstraction
{
    public interface IMessageSource
    {
        void SendMessage(MessageUDP message, IPEndPoint endPoint);

        MessageUDP RecieveMessage(ref IPEndPoint endPoint);
    }
}
