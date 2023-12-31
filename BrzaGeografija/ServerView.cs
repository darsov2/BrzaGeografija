﻿using System;
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
        private Game actualGame;
        private TcpListener server;
        private TcpClient tcpClient;
        private NetworkStream stream;
        private Random random = new Random();
        private char letter;
        private int clientTotalPoints;
        private bool clientTotal;
        private int timeLeft = 300;
        private bool serverDone;
        private bool opponentFilled;
        private bool clientDone;
        private List<char> usedLetters;
        private bool newGame;
        private bool stopGame;
        private readonly string alphabet = "АБВГДЕЖЗИЈКЛЉМНОПРСТЌУФХЦЧЏШ";
        public ServerView(TcpListener server, TcpClient client, NetworkStream stream, int _port)
        {
            this.server = server;
            tcpClient = client;
            this.stream = stream;
            port = _port;
            games = new List<Game>();
            usedLetters = new List<char>();
            InitializeComponent();
            clientTotal = false;
            serverDone = false;
            stopGame = false;
            clientTotalPoints = 0;
            this.DoubleBuffered = true;
            clientDone = false;
            newGame = true;
            opponentFilled = false;
            label12.Text = Convert.ToString(games.Sum(x => x.Points));
        }

        private async Task AcceptClients()
        {
            try
            {
                _ = ProcessClient(tcpClient);
            }
            catch(Exception ex)
            {
                MessageBox.Show("Проблем со стартување/поврзување со клиентот!\n\n" + ex.Message);
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

                actualGame.Letter = letter;
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

                textBox9.Text = Convert.ToString(points.Sum());
                actualGame.Points = points.Sum();
                games.Add(actualGame);
                label12.Text = Convert.ToString(games.Sum(x => x.Points));
            }
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
                        else if(receivedObject.Equals("resultsShown"))
                        { 
                            newGame = true;
                        }
                        else if(receivedObject.StartsWith("totalPoints"))
                        {
                            clientTotalPoints = Convert.ToInt32(receivedObject.Split(':')[1]);
                            clientTotal = true;
                            if (!stopGame)
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
            startNewGame();
        }

        private void startNewGame()
        {
            newGame = false;
            letter = randomLetter();
            serverDone = false;
            clientDone = false;
            usedLetters.Add(letter);
            SendDataToClients(0, "letter:" + Convert.ToString(letter));
            label1.Text = Convert.ToString(letter);
            enableTextBoxes();
            textBox1.Focus();
            timeLeft = 300;
            timer1.Start();
            this.Height = 300;
            hidePoints();
            clearTextBoxes();
            label3.Hide();
            opponentFilled = false;
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
                    actualGame = new Game(textBox1.Text, textBox2.Text, textBox3.Text, textBox4.Text, textBox5.Text, textBox6.Text, textBox7.Text, textBox8.Text, 0);
                    SendDataToClients(1, "", actualGame);
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
            }
            actualGame = new Game(textBox1.Text, textBox2.Text, textBox3.Text, textBox4.Text, textBox5.Text, textBox6.Text, textBox7.Text, textBox8.Text, 0);
            actualGame.Letter = letter;
            SendDataToClients(1, "", actualGame);
        }

        private void ServerView_FormClosing(object sender, FormClosingEventArgs e)
        {
            StopServer();
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            if(newGame)
            {
                startNewGame();
            }
        }

        private void endGame()
        {
            ResultBoxBG resultBoxBG = new ResultBoxBG(0, games.Sum(x => x.Points), clientTotalPoints);
            resultBoxBG.Show();
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            timer1.Stop();
            stopGame = true;
            if(clientTotal)
            {
                endGame();
            }
            SendDataToClients(0, "totalPoints:" + games.Sum(x => x.Points));
        }
    }
}
