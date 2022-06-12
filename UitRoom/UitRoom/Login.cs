using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UitRoom
{
    public partial class Login : Form
    {

        public Login()
        {
            InitializeComponent();
        }
        private TcpClient client;
        private Thread thdReceiveMessage;
        private StreamReader srReceive;
        private StreamWriter swSend;
        public delegate void ChuyenTk(TextBox text);
        
        private void logi_Click(object sender, EventArgs e)
        {
            srReceive = new StreamReader(client.GetStream());
            swSend = new StreamWriter(client.GetStream());
            SendMessage("0 " + Username.Text +" "+ Password.Text);

        }
        void ConnectToServer()
        {
            client = new TcpClient();
            client.Connect(IPAddress.Parse("127.0.0.1"), 8080);
            srReceive = new StreamReader(client.GetStream());
            swSend = new StreamWriter(client.GetStream());
            SendMessage("Login");
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
                    if (mess == "0.1")
                    {
                        CheckForIllegalCrossThreadCalls = false;
                        Main chinh = new Main();
                        ChuyenTk chuyenname = new ChuyenTk(chinh.NhanTk);

                        chuyenname(this.Username);
                        //this.Visible=false;
                        
                        chinh.ShowDialog();
                      //this.Visible = true;
                        Username.Clear();
                        Password.Clear();
                    }
                    if (mess == "0.0")
                    {

                        CheckForIllegalCrossThreadCalls = false;
                        MessageBox.Show("This acount is not exist or your password is incorect!");
                        //InfoMessage("This username is not available!\n");
                        CheckForIllegalCrossThreadCalls = true;
                    }
                    
                    else { }
                }

            }
        }
        private void SendMessage(string mess)
        {

            swSend.WriteLine(mess);
            swSend.Flush();
        }


        private void Login_Load(object sender, EventArgs e)
        {
            ConnectToServer();
        }
        
    }
}
