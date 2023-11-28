using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Net;
using System.Net.Sockets;
using System.Windows;
using System.Windows.Input;
using Newtonsoft.Json;
using System.Windows.Documents;
//using GalaSoft.MvvmLight.Command;


namespace lab4
{
    public class AppVM : INotifyPropertyChanged
    {
        private Person selectedPerson;

        public BindingList<Person> People { get; set; }
        public Person SelectedPerson
        {
            get { return selectedPerson; }
            set
            {
                selectedPerson = value;
                OnPropertyChanged("SelectedPerson");
            }
        }

        public AppVM()
        {
            People = new BindingList<Person>();
        }

        public bool AddCommand
        {
            get
            {
                return false;
            }
            set
            {
                Add();

            }
        }

        public bool LoadCommand
        {
            get
            {
                return false;
            }
            set
            {
                Load();

            }
        }
        public bool SaveCommand
        {
            get
            {
                return false;
            }
            set
            {
                Save();

            }
        }




        public void Load()
        {
            UdpClient sender = new UdpClient();
            string request = "GET";
            byte[] data = Encoding.UTF8.GetBytes(request);
            sender.Send(data, data.Length, "127.0.0.1", 8000);
            sender.Client.ReceiveTimeout = 5000;
            try
            {
                IPEndPoint remoteEP = new IPEndPoint(IPAddress.Any, 0);
                byte[] data2 = sender.Receive(ref remoteEP);
                string response = Encoding.UTF8.GetString(data2);
                var people = JsonConvert.DeserializeObject<List<Person>>(response);

                People.Clear();

                foreach (var person in people)
                {
                    People.Add(person);
                }

                MessageBox.Show("Данные загружены с сервера");
            }
            catch (SocketException ex)
            {

                MessageBox.Show("Не удалось получить ответ от сервера: " + ex.Message);
            }

            sender.Close();
        }
        public void Add()
        {
            bool kostil = false;
            Person newPerson = new Person(kostil);

            People.Add(newPerson);
        }

        public void Save()
        {

            UdpClient sender = new UdpClient();

            string request = "POST" + JsonConvert.SerializeObject(People);

            byte[] data = Encoding.UTF8.GetBytes(request);

            sender.Send(data, data.Length, "127.0.0.1", 8000);
            try
            {

                IPEndPoint remoteEP = new IPEndPoint(IPAddress.Any, 0);
                byte[] data2 = sender.Receive(ref remoteEP);

                string response = Encoding.UTF8.GetString(data2);

                MessageBox.Show(response);
            }
            catch (SocketException ex)
            {

                MessageBox.Show("Не удалось получить ответ от сервера: " + ex.Message);
            }
        }



        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
