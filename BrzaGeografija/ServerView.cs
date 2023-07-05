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
    public partial class ServerView : Form
    {
        private int port;
        private List<Game> games;
        private Game clientGame;
        private TcpListener server;
        private TcpClient tcpClient;
        private NetworkStream stream;
        private Random random = new Random();
        private char letter;
        private int timeLeft = 300;
        private bool serverDone;
        private bool clientDone;
        private List<char> usedLetters;
        private readonly string alphabet = "АБВГДЃЕЖЗЅИЈКЛЉМНОПРСТЌУФХЦЧЏШ";
        public ServerView(TcpListener server, TcpClient client, NetworkStream stream, int _port)
        {
            this.server = server;
            tcpClient = client;
            this.stream = stream;
            port = _port;
            games = new List<Game>();
            usedLetters = new List<char>();
            InitializeComponent();
            serverDone = false;
            clientDone = false;
        }

        private async Task AcceptClients()
        {
            try
            {
                _ = ProcessClient(tcpClient);
            }
            catch(Exception ex)
            {
                MessageBox.Show("Проблем со стартување поврзување со клиентот!\n\n" + ex.Message);
            }
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

        private async Task ProcessClient(TcpClient client)
        {
            try
            {

                while (true)
                {
                    byte[] receivedData = new byte[client.ReceiveBufferSize];
                    int bytesRead = await stream.ReadAsync(receivedData, 0, client.ReceiveBufferSize);
                    string receivedObject = Encoding.UTF8.GetString(receivedData, 0, bytesRead);
                    try
                    {
                        Game deserializedObject = JsonConvert.DeserializeObject<Game>(receivedObject);
                        clientGame = deserializedObject;
                        clientDone = true;
                        timeLeft = Math.Min(timeLeft, 10);
                        if (serverDone)
                        {
                            fillOpponent(clientGame);
                            this.Height = 500;
                            label3.Hide();
                        }
                        MessageBox.Show(deserializedObject.ToString());
                        timeLeft = Math.Min(10, timeLeft);
                        label3.Show();
                    }
                    catch (Exception ex)
                    {
                        if (receivedObject.Equals("clientDone"))
                        {
                            label3.Show();
                            fillOpponent(clientGame);
                            this.Height = 500;
                        }
                    }

                    // Send response back to the client
                    string responseData = "Response from server";
                    byte[] responseBytes = Encoding.UTF8.GetBytes(responseData);
                    await stream.WriteAsync(responseBytes, 0, responseBytes.Length);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Проблем со процесирање на податоците од клиентска страна!\n\n" + ex.Message);
            }
            finally
            {
                tcpClient.Close();
                tcpClient = null;
            }
        }

        private async void SendDataToClients(int type, string text, Game game = null)
        {
            string dataToSend = text;

            try
            {
                NetworkStream stream = tcpClient.GetStream();
                if(type == 0)
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
                MessageBox.Show("Проблем со испраќање на податоците на клиентска страна!\n\n" + ex.Message);
            }
        }

        private void StopServer()
        {
            tcpClient.Close();
            server.Stop();
        }

        private void ServerView_Load(object sender, EventArgs e)
        {
            //StartServer();
            disableTextBoxes();
            AcceptClients();
            letter = randomLetter();
            usedLetters.Add(letter);
            SendDataToClients(0, "letter:" + Convert.ToString(letter));
            label1.Text = Convert.ToString(letter);
            enableTextBoxes();
            textBox1.Focus();
            timer1.Start();
            this.Height = 300;
        }

        private char randomLetter()
        {
            while(true)
            {
                char rnd = alphabet[random.Next(alphabet.Length)];
                if(!usedLetters.Contains(rnd))
                {
                    return rnd;
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            SendDataToClients(0, Convert.ToString(letter));
        }

        private void timerLabelFormat(int timeLeft)
        {
            label2.Text = new TimeSpan(0, timeLeft / 60, timeLeft % 60).ToString("mm':'ss");
        }

        private void disableTextBoxes()
        {
            textBox1.ReadOnly = true;
            textBox2.ReadOnly = true;
            textBox3.ReadOnly = true;
            textBox4.ReadOnly = true;
            textBox5.ReadOnly = true;
            textBox6.ReadOnly = true;
            textBox7.ReadOnly = true;
            textBox8.ReadOnly = true;
            textBox9.ReadOnly = true;
        }

        private void enableTextBoxes()
        {
            textBox1.ReadOnly = false;
            textBox2.ReadOnly = false;
            textBox3.ReadOnly = false;
            textBox4.ReadOnly = false;
            textBox5.ReadOnly = false;
            textBox6.ReadOnly = false;
            textBox7.ReadOnly = false;
            textBox8.ReadOnly = false;
            textBox9.ReadOnly = false;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            timeLeft--;
            if(timeLeft <= 0)
            {
                timer1.Stop();
                if(!serverDone)
                {
                    serverDone = true;
                    Game game = new Game(textBox1.Text, textBox2.Text, textBox3.Text, textBox4.Text, textBox5.Text, textBox6.Text, textBox7.Text, textBox8.Text, 0);
                    games.Add(game);
                    SendDataToClients(1, "", game);
                }
                if (clientDone)
                {
                    fillOpponent(clientGame);
                    this.Height = 500;
                    SendDataToClients(0, "serverDone");
                }
            }
            timerLabelFormat(timeLeft);
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            timer1.Stop();
            serverDone = true;
            if(clientDone)
            {
                fillOpponent(clientGame);
                this.Height = 500;
                label3.Hide();
            }
            Game game = new Game(textBox1.Text, textBox2.Text, textBox3.Text, textBox4.Text, textBox5.Text, textBox6.Text, textBox7.Text, textBox8.Text, 0);
            games.Add(game);
            SendDataToClients(1, "", game);
        }
    }
}
