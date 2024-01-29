using System.Net;
using TestDb34.Abstraction;
using TestDb34;

namespace TestProject1
{
    public class Tests
    {
        IMessageSource _source;
        IPEndPoint _endPoint;

        [SetUp]
        public void Setup()
        {
            
            _endPoint = new IPEndPoint(IPAddress.Any, 0);
        }

        [Test]
        public void TestReceiveMessage()
        {
            _source = new MockMessageSource();
            var result = _source.RecieveMessage(ref _endPoint);

            Assert.IsNotNull(result);
            Assert.IsNull(result.Text);
            Assert.IsNotNull(result.FromName);
            Assert.That("����", Is.EqualTo(result.FromName));
            Assert.That(Command.Register, Is.EqualTo(result.Command));

        }
    }
}

public class MockMessageSource : IMessageSource
{

    private Queue<MessageUDP> messages = new();
    

    public MockMessageSource()
    {
        messages.Enqueue(new MessageUDP { Command = Command.Register, FromName = "����" });
        messages.Enqueue(new MessageUDP { Command = Command.Register, FromName = "���" });
        messages.Enqueue(new MessageUDP { Command = Command.Message, FromName = "���", ToName = "����", Text = "�� ���" });
        messages.Enqueue(new MessageUDP { Command = Command.Message, FromName = "����", ToName = "���", Text = "�� ����" });


    }


   

    public MessageUDP RecieveMessage(ref IPEndPoint endPoint)
    {
        return messages.Peek();
    }


    public void SendMessage(MessageUDP message, IPEndPoint endPoint)
    {
        messages.Enqueue(message);
    }
}