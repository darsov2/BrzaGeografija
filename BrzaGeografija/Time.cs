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
    public partial class Time : Form
    {
        public Time(List<string> lista)
        {
            InitializeComponent();
            listBox1.DataSource = lista;
        }

        private void Time_Load(object sender, EventArgs e)
        {

        }
    }
}
