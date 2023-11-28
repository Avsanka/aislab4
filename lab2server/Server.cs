using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Data.Entity;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using lab2server;
using classPerson;


namespace Server
{
    class Program
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();
        static void Main(string[] args)
        {

            UdpClient listener = new UdpClient(8000);

            string localIP = GetLocalIPAddress();

            Console.WriteLine("Сервер запущен на {0}:8000", localIP);

            while (true)
            {

                IPEndPoint remoteEP = new IPEndPoint(IPAddress.Any, 0);
                byte[] request = listener.Receive(ref remoteEP);

                string requestString = Encoding.UTF8.GetString(request);

                logger.Info("Получено сообщение '{0}' от {1}", request, remoteEP);


                Task.Run(() => HandleRequest(requestString, remoteEP, listener));
            }
        }

        static async Task HandleRequest(string request, IPEndPoint remoteEP, UdpClient listener)
        {
            string dopString = "";
            if (request.StartsWith("POST"))
            {
                dopString = request.Substring(4);
                request = "POST";
            }
            using (PersonContext db = new PersonContext())
            {
                switch (request)
                {
                    case "GET":

                        var people = db.People.ToList();

                        string responseString = JsonConvert.SerializeObject(people);

                        byte[] response = Encoding.UTF8.GetBytes(responseString);

                        await listener.SendAsync(response, response.Length, remoteEP);

                        logger.Info("Отправлен ответ '{0}' на {1}", response, remoteEP);
                        break;

                    case "POST":
                        var newPeople = JsonConvert.DeserializeObject<List<Person>>(dopString);
                        db.People.RemoveRange(db.People);
                        db.People.AddRange(newPeople);
                        db.SaveChanges();
                        response = Encoding.UTF8.GetBytes("Database Updated");
                        await listener.SendAsync(response, response.Length, remoteEP);
                        logger.Info("Отправлен ответ '{0}' на {1}", response, remoteEP);
                        break;
                    default:
                        string errorString = "error";
                        byte[] error = Encoding.UTF8.GetBytes(errorString);
                        await listener.SendAsync(error, error.Length, remoteEP);
                        break;
                }
            }
        }

        static string GetLocalIPAddress()
        {

            var interfaces = Dns.GetHostEntry(Dns.GetHostName()).AddressList;
            foreach (var address in interfaces)
            {
                if (address.AddressFamily == AddressFamily.InterNetwork)
                {
                    return address.ToString();
                }
            }
            return "";
        }
    }
}