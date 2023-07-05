using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;
using BrzaGeografija.Classes;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace BrzaGeografija
{
    public partial class Form3 : Form
    {
        public Form3()
        {
            InitializeComponent();
        }

        private void Form3_Load(object sender, EventArgs e)
        {
            
        }

        private async Task ConnectToServer()
        {
            string serverIp = "127.0.0.1";
            int serverPort = 1234;

            using (TcpClient client = new TcpClient())
            {
                try
                {
                    await client.ConnectAsync(serverIp, serverPort);

                    NetworkStream stream = client.GetStream();

                    while (true)
                    {
                        byte[] buffer = new byte[client.ReceiveBufferSize];
                        int bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length);
                        string receivedData = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                        MessageBox.Show("Received data from server: " + receivedData);
                    }
                }
                catch (Exception ex)
                { 
                    Console.WriteLine("An error occurred: " + ex.Message);
                }
            }
        }

        private CancellationTokenSource cancellationTokenSource;

        private async void StartServer()
        {
            cancellationTokenSource = new CancellationTokenSource();

            try
            {
                await Task.Run(() => ListenForClients(cancellationTokenSource.Token));
            }
            catch (TaskCanceledException)
            {
                
            }
        }

        private void StopServer()
        {
            cancellationTokenSource?.Cancel();
        }

        private async Task ListenForClients(CancellationToken cancellationToken)
        {
            int port = 1234;
            TcpListener listener = new TcpListener(IPAddress.Any, port);
            listener.Start();

            while (!cancellationToken.IsCancellationRequested)
            {
                if (listener.Pending())
                {
                    TcpClient client = await listener.AcceptTcpClientAsync();
                    _ = ProcessClient(client, cancellationToken);
                }

                await Task.Delay(100);
            }

            listener.Stop();
        }

        private async Task ProcessClient(TcpClient client, CancellationToken cancellationToken)
        {
            NetworkStream stream = client.GetStream();

            try
            {
                while (!cancellationToken.IsCancellationRequested)
                {
                    byte[] receivedData = new byte[client.ReceiveBufferSize];
                    int bytesRead = await stream.ReadAsync(receivedData, 0, client.ReceiveBufferSize);
                    string receivedObject = Encoding.UTF8.GetString(receivedData, 0, bytesRead);

                    MessageBox.Show(receivedObject);

                    // Send response to the client
                    string response = "Response from server";
                    byte[] responseData = Encoding.UTF8.GetBytes(response);
                    await stream.WriteAsync(responseData, 0, responseData.Length);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred: " + ex.Message);
            }
            finally
            {
                stream.Close();
                client.Close();
            }
        }



        private void button2_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            if(textBox2.Text.Length < 1)
            {
                MessageBox.Show("За да хостирате игра мора да внесете порта!");
                return;
            }
            int port = Convert.ToInt32(textBox2.Text);
            TcpListener server = new TcpListener(IPAddress.Any, port);
            server.Start();

            TcpClient client = server.AcceptTcpClient();
            NetworkStream stream = client.GetStream();

            byte[] receivedData = new byte[client.ReceiveBufferSize];
            int bytesRead = stream.Read(receivedData, 0, client.ReceiveBufferSize);

            string receivedObject = Encoding.UTF8.GetString(receivedData, 0, bytesRead);

            //Game deserializedObject = JsonConvert.DeserializeObject<Game>(receivedObject);
            //MessageBox.Show(deserializedObject.ToString());

            //stream.Close();
            //client.Close();
            //server.Stop();

            ServerView serverView = new ServerView(server, client, stream, port);
            serverView.Show();
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            //Game igra = new Game("Igor", "Ilok", "Indija", "Ind", "Ivanjica", "Iris", "Iguana", "Igla", 100);
            //string serializedObject = JsonConvert.SerializeObject(igra);


            if (textBox2.Text.Length < 1)
            {
                MessageBox.Show("За да хостирате игра мора да внесете порта!");
                return;
            }
            else if (textBox1.Text.Length < 1)
            {
                MessageBox.Show("За да се поврзере на игра мора да внесете IP адреса!");
                return;
            }
            int port = Convert.ToInt32(textBox2.Text);

            IPEndPoint endpoint = new IPEndPoint(IPAddress.Parse(textBox1.Text), port);

            TcpClient client = new TcpClient();
            client.Connect(endpoint);

            NetworkStream stream = client.GetStream();
            byte[] data = Encoding.UTF8.GetBytes("start");

            stream.Write(data, 0, data.Length);
            //stream.Close();
            //client.Close();

            ClientView clientView = new ClientView(client, stream);
            clientView.Show();

        }
    }
}
