using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using TestDb34.Models;
using static System.Net.Mime.MediaTypeNames;

namespace TestDb34
{
    public enum Command
    {
        Register,
        Message,
        Confirmation
    }

   

    public class MessageUDP
    {
        public Command Command { get; set; }
        public int? Id { get; set; }
        public string FromName { get; set; }
        public string ToName { get; set; }
        public string Text { get; set; }

        // Метод для сериализации в JSON
        public string ToJson()
        {
            return JsonSerializer.Serialize(this);
        }

        // Статический метод для десериализации JSON в объект MyMessage
        public static MessageUDP FromJson(string json)
        {
            return JsonSerializer.Deserialize<MessageUDP>(json);
        }

        public override string ToString()
        {
            return $"{DateTime.Now} \n Получено сообщение {Text} \n от {FromName} ";
        }
    }
}
