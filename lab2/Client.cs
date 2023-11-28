using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

class AsyncUpdClient
{
    private const int port = 8001;
    private const string ip = "127.0.0.1";



    static void printMenu()
    {
        Console.WriteLine("\n Choose an action");
        Console.WriteLine("1)add person to file - addp + data");
        Console.WriteLine("2)show persons - showps");
        Console.WriteLine("3)choose person by number - chnum + number");
        Console.WriteLine("4)delete person by number - dnum + number");
        Console.WriteLine("exit - esc");
        Console.WriteLine();
    }



    static void Main()
    {
        UdpClient client = new UdpClient();

        while (true)
        {

            printMenu();


            string command = Console.ReadLine();
            string response = "";

            Console.Clear();

            byte[] requestData = Encoding.UTF8.GetBytes(command);
            client.Send(requestData, requestData.Length, ip, port);

            if (command == "esc")
                break;

            IPEndPoint serverEndPoint = new IPEndPoint(IPAddress.Any, port);
            byte[] responseData = client.Receive(ref serverEndPoint);

            response = Encoding.UTF8.GetString(responseData);
            Console.WriteLine("Server answer: ");
            Console.WriteLine(response);

        }

        client.Close();
    }
}














//namespace lab2
//{
//    class Client
//    {
//        static void Main(string[] args)
//        {
//        }
//    }
//}
