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

            ServerView serverView = new ServerView(server, client, stream, port);
            serverView.Show();
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            if (textBox2.Text.Length < 1)
            {
                MessageBox.Show("За да се поврзере на игра мора да внесете порта!");
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

            ClientView clientView = new ClientView(client, stream);
            clientView.Show();

        }
    }
}
