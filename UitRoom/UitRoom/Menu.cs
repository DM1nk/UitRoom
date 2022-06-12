using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UitRoom
{
    public partial class Menu : Form
    {
        public Menu()
        {
            InitializeComponent();
        }

      
        private void button2_Click(object sender, EventArgs e)
        {
            Login app = new Login();
            app.Show();
        }

        private void Menu_Load(object sender, EventArgs e)
        {
            Server quanly = new Server();
            quanly.Show();
        }
    }
}
