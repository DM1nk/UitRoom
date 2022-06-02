using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using UitRoom.DAO;

namespace UitRoom
{

    public partial class Main : Form
    {
        public Main()
        {
            InitializeComponent();
        }
        private TcpClient client;
        private Thread thdReceiveMessage;
        private StreamReader srReceive;
        private StreamWriter swSend;
        public void NhanTk(TextBox text)
        {
            UserNamebt.Text = text.Text;
        }
        
        private void Main_Load(object sender, EventArgs e)
        {
            ConnectToServer();
            reload();
        }
        void ConnectToServer()
        {
            client = new TcpClient();
            client.Connect(IPAddress.Parse("127.0.0.1"), 8080);
            srReceive = new StreamReader(client.GetStream());
            swSend = new StreamWriter(client.GetStream());
            SendMessage(UserNamebt.Text);

            thdReceiveMessage = new Thread(new ThreadStart(ReceiveFromServer));
            thdReceiveMessage.Start();
        }
        private void ReceiveFromServer()
        {

            Stream remess = srReceive.BaseStream;



            while (remess != null)
            {

                string mess;
                while ((mess = srReceive.ReadLine()) != null)
                {
                    if (mess == "1.0."+UserNamebt.Text)
                    {
                        CheckForIllegalCrossThreadCalls = false;
                        MessageBox.Show("This acount is not exist or your password is incorect!");
                        //InfoMessage("This username is not available!\n");
                        CheckForIllegalCrossThreadCalls = true;
                    }
                    if (mess == "1.1")
                    {
                        
                        
                    }
                }

            }
        }
        
        private void SendMessage(string mess)
        {

            swSend.WriteLine(mess);
            swSend.Flush();
        }

        private void logout_Click(object sender, EventArgs e)
        {
            Login dangnhap = new Login();
            dangnhap.Show();
            this.Close();

        }

        public void reload ()
        {
            string query = "SELECT * FROM dbo.Room WHERE stat = N'Available'";
            DataTable result = DataProvider.Instance.ExecuteQuery(query);
            data.DataSource = result;
            book.Text = "Booking";

        }

        private void UserNamebt_Click(object sender, EventArgs e)
        {
            string query = "SELECT roomid, ngay FROM dbo.Booked WHERE username = N'"+UserNamebt.Text+"'";
            DataTable result = DataProvider.Instance.ExecuteQuery(query);
            data.DataSource = result;
            book.Text = "Canceling";
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            
        }
        string temp;
        private void data_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int rowIndex = e.RowIndex;
            DataGridViewRow row = data.Rows[rowIndex];
            if (row.Cells.Count < 4)
            {



                temp = "2 " + row.Cells[0].Value.ToString() + " " + row.Cells[1].Value.ToString();
            }
            else
            {
                temp = "1 " + row.Cells[1].Value.ToString() + " " + row.Cells[3].Value.ToString();
            }
        }
       

        private void button2_Click(object sender, EventArgs e)
        {
            
            SendMessage(temp);
            temp = "";
        }

        private void mainbtx_Click(object sender, EventArgs e)
        {
            reload();
        }
    }
}
