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
        private bool serverTotal;
        private int serverTotalPoints;
        private Game actualGame;
        private bool stopGame;
        private bool serverDone;
        private bool clientDone;
        private bool opponentFilled;
        private NetworkStream stream;
        private CancellationTokenSource cancellationTokenSource;
        int timeLeft;
        public ClientView(TcpClient client, NetworkStream stream)
        {
            InitializeComponent();
            this.stream = stream;
            this.client = client;
            games = new List<Game>();
            serverTotal = false;
            stopGame = false;
            serverTotalPoints = 0;
            label12.Text = Convert.ToString(games.Sum(x => x.Points));
            opponentFilled = false;
        }

        private async void ConnectToServer()
        {
            try
            {
                stream = client.GetStream();

                cancellationTokenSource = new CancellationTokenSource();
                await ReceiveDataFromServer();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Настана грешка при поврзување со серверот:\n\n" + ex.Message);
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
                        timeLeft = Math.Min(10, timeLeft);
                        label3.Show();
                    }
                    catch(Exception ex)
                    {
                        if(receivedObject.StartsWith("letter"))
                        {
                            label1.Text = receivedObject.Substring(7);
                            actualGame = new Game(receivedObject.Substring(7).ElementAt(0));
                            timeLeft = 300;
                            serverDone = false;
                            clientDone = false;
                            timer1.Start();
                            label3.Hide();
                            clearTextBoxes();
                            this.Height = 300;
                            opponentFilled = false;
                            hidePoints();
                        }
                        else if (receivedObject.Equals("serverDone"))
                        {
                            label3.Hide();
                            timer1.Stop();
                            fillOpponent(serverGame);
                            this.Height = 500;
                        }
                        else if(receivedObject.StartsWith("totalPoints"))
                        {
                            serverTotalPoints = Convert.ToInt32(receivedObject.Split(':')[1]);
                            serverTotal = true;
                            if(!stopGame)
                            {
                               button1_Click_1(null, null);
                            }
                            else
                            {
                                endGame();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Проблем со процесирање на податоците од серверска страна!\n\n" + ex.Message);
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
                MessageBox.Show("Настана грешка при испраќање на податоците: " + ex.Message);
            }
        }
        private void clearTextBoxes()
        {
            textBox1.Clear();
            textBox2.Clear();
            textBox3.Clear();
            textBox4.Clear();
            textBox5.Clear();
            textBox6.Clear();
            textBox7.Clear();
            textBox8.Clear();
            textBox9.Clear();
            textBox11.Clear();
            textBox12.Clear();
            textBox13.Clear();
            textBox14.Clear();
            textBox15.Clear();
            textBox16.Clear();
            textBox17.Clear();
            textBox18.Clear();
        }

        private void hidePoints()
        {
            label4.Hide();
            label5.Hide();
            label6.Hide();
            label7.Hide();
            label8.Hide();
            label9.Hide();
            label10.Hide();
            label11.Hide();
        }

        private void showPoints()
        {
            label4.Show();
            label5.Show();
            label6.Show();
            label7.Show();
            label8.Show();
            label9.Show();
            label10.Show();
            label11.Show();
        }

        private void DisconnectFromServer()
        {
            cancellationTokenSource?.Cancel();
            stream?.Close();
            client?.Close();
        }

        private void fillOpponent(Game game)
        {
            if(!opponentFilled)
            {
                opponentFilled = true;
                textBox18.Text = game.Name;
                textBox17.Text = game.City;
                textBox16.Text = game.Country;
                textBox15.Text = game.River;
                textBox14.Text = game.Mountain;
                textBox13.Text = game.Animal;
                textBox12.Text = game.Plant;
                textBox11.Text = game.Thing;

                int[] points = actualGame.CheckGame(game);

                showPoints();

                label4.Text = Convert.ToString(points[0]);
                label5.Text = Convert.ToString(points[1]);
                label6.Text = Convert.ToString(points[2]);
                label7.Text = Convert.ToString(points[3]);
                label8.Text = Convert.ToString(points[4]);
                label9.Text = Convert.ToString(points[5]);
                label10.Text = Convert.ToString(points[6]);
                label11.Text = Convert.ToString(points[7]);

                actualGame.Points = points.Sum();
                games.Add(actualGame);
                label12.Text = Convert.ToString(games.Sum(x => x.Points));
                textBox9.Text = Convert.ToString(points.Sum());
                Thread.Sleep(1000);

                SendDataToServer(0, "resultsShown");
            }
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
                    game.Letter = actualGame.Letter;
                    actualGame = game;
                    SendDataToServer(1, "", actualGame);
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
            game.Letter = actualGame.Letter;
            actualGame = game;
            SendDataToServer(1, "", actualGame);
            if (serverDone)
            {
                fillOpponent(serverGame);
                this.Height = 500;
                SendDataToServer(0, "clientDone");
            }
        }

        private void ClientView_FormClosing(object sender, FormClosingEventArgs e)
        {
            DisconnectFromServer();
        }

        private void endGame()
        {
            ResultBoxBG resultBoxBG = new ResultBoxBG(1, serverTotalPoints, games.Sum(x => x.Points));
            resultBoxBG.Show();
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            stopGame = true;
            if(serverTotal)
            {
                endGame();
            }
            SendDataToServer(0, "totalPoints:" + games.Sum(x => x.Points));
        }
    }
}
