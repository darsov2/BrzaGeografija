using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BrzaGeografija
{
    public partial class ResultBoxBG : Form
    {
        public ResultBoxBG(int type, int server, int client)
        {
            InitializeComponent();
            if(type == 0)
            {
                if(server >= client)
                {
                    label1.Text = "Честитки!";
                }
                else
                {
                    label1.Text = "Повеќе среќа следниот пат";
                }
            }
            else if(type == 1)
            {
                if (client >= server)
                {
                    label1.Text = "Честитки!";
                }
                else
                {
                    label1.Text = "Повеќе среќа следниот пат";
                }
            }
            label4.Text = Convert.ToString(server);
            label5.Text = Convert.ToString(client);
        }

        private void ResultBoxBG_Load(object sender, EventArgs e)
        {

        }
    }
}
