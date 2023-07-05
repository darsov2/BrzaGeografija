using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
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
    public partial class ClientView : Form
    {
        private List<Game> games;
        private TcpClient client;
        private Game serverGame;
        private bool serverDone;
        private bool clientDone;
        private NetworkStream stream;
        private CancellationTokenSource cancellationTokenSource;
        int timeLeft;
        public ClientView(TcpClient client, NetworkStream stream)
        {
            InitializeComponent();
            this.stream = stream;
            this.client = client;
            games = new List<Game>();
        }

        private async void ConnectToServer()
        {
            string serverIp = "192.168.14.83";
            int serverPort = 1234;

            try
            {
                //client = new TcpClient();
                //await client.ConnectAsync(serverIp, serverPort);

                stream = client.GetStream();

                cancellationTokenSource = new CancellationTokenSource();
                await ReceiveDataFromServer();
            }
            catch (Exception ex)
            {
                // Handle any exceptions that may occur during the connection process
                Console.WriteLine("An error occurred while connecting: " + ex.Message);
            }
        }

        private async Task ReceiveDataFromServer()
        {
            try
            {
                byte[] receivedData = new byte[client.ReceiveBufferSize];

                while (true)
                {
                    if (cancellationTokenSource.IsCancellationRequested)
                        break;

                    int bytesRead = await stream.ReadAsync(receivedData, 0, client.ReceiveBufferSize);
                    string receivedObject = Encoding.UTF8.GetString(receivedData, 0, bytesRead);
                    try
                    {   
                        Game deserializedObject = JsonConvert.DeserializeObject<Game>(receivedObject);
                        serverGame = deserializedObject;
                        serverDone = true;
                        if(clientDone)
                        {
                            label3.Hide();
                            fillOpponent(serverGame);
                            this.Height = 500;
                        }
                        MessageBox.Show(deserializedObject.ToString());
                        timeLeft = Math.Min(10, timeLeft);
                        label3.Show();
                    }
                    catch(Exception ex)
                    {
                        if(receivedObject.StartsWith("letter"))
                        {
                            label1.Text = receivedObject.Substring(6);
                            //MessageBox.Show("Избраната буква е: " + receivedObject.Substring(6) + "\n\nИграта започнува за 3 секунди!");
                            timeLeft = 300;
                            timer1.Start();
                            label3.Hide();
                            this.Height = 300;
                        }
                        else if (receivedObject.Equals("serverDone"))
                        {
                            label3.Hide();
                            timer1.Stop();
                            fillOpponent(serverGame);
                            this.Height = 500;
                        }
                    }

                    // Process the received object
                    //MessageBox.Show(receivedObject);
                }
            }
            catch (Exception ex)
            {
                // Handle any exceptions that may occur during the receiving process
                Console.WriteLine("An error occurred while receiving data: " + ex.Message);
            }
        }

        private async void SendDataToServer(int type, string text, Game game = null)
        {
            if (client == null || !client.Connected)
            {
                MessageBox.Show("Нема конекција со серверот!");
                return;
            }

            try
            {
                // Get the data to send from user input or other sources
                string dataToSend = text;
                if (type == 0)
                {
                    byte[] data = Encoding.UTF8.GetBytes(dataToSend);
                    await stream.WriteAsync(data, 0, data.Length);
                }
                else
                {
                    string serializedObject = JsonConvert.SerializeObject(game);
                    byte[] data = Encoding.UTF8.GetBytes(serializedObject);
                    await stream.WriteAsync(data, 0, data.Length);
                }
            }
            catch (Exception ex)
            {
                // Handle any exceptions that may occur during the sending process
                MessageBox.Show("Настана грешка при испраќање на податоците: " + ex.Message);
            }
        }

        private void DisconnectFromServer()
        {
            cancellationTokenSource?.Cancel();
            stream?.Close();
            client?.Close();
        }

        private void fillOpponent(Game game)
        {
            textBox18.Text = game.Name;
            textBox17.Text = game.City;
            textBox16.Text = game.Country;
            textBox15.Text = game.River;
            textBox14.Text = game.Mountain;
            textBox13.Text = game.Animal;
            textBox12.Text = game.Plant;
            textBox11.Text = game.Thing;
            textBox10.Text = Convert.ToString(game.Points);
        }

        private void ClientView_Load(object sender, EventArgs e)
        {
            ConnectToServer();
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }
        private void timerLabelFormat(int timeLeft)
        {
            label2.Text = new TimeSpan(0, timeLeft / 60, timeLeft % 60).ToString("mm':'ss");
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            timeLeft--;
            if (timeLeft <= 0)
            {
                timer1.Stop();
                if(!clientDone)
                { 
                    Game game = new Game(textBox1.Text, textBox2.Text, textBox3.Text, textBox4.Text, textBox5.Text, textBox6.Text, textBox7.Text, textBox8.Text, 0);
                    games.Add(game);
                    SendDataToServer(1, "", game);
                }
                clientDone = true;
                if (serverDone)
                {
                    fillOpponent(serverGame);
                    this.Height = 500;
                    SendDataToServer(0, "clientDone");
                }
            }
            timerLabelFormat(timeLeft);
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            timer1.Stop();
            clientDone = true;
            Game game = new Game(textBox1.Text, textBox2.Text, textBox3.Text, textBox4.Text, textBox5.Text, textBox6.Text, textBox7.Text, textBox8.Text, 0);
            games.Add(game);
            SendDataToServer(1, "", game);
            if (serverDone)
            {
                fillOpponent(serverGame);
                this.Height = 500;
                SendDataToServer(0, "clientDone");
            }
        }
    }
}
