using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading;
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
            reload();
        }
        private void ReceiveFromServer()
        {

            Stream remess = srReceive.BaseStream;

            

            while (remess != null)
            {

                string mess;
                
                while ((mess = srReceive.ReadLine()) != null)
                {
                    
                    if (mess == "1.0.")
                    {
                        CheckForIllegalCrossThreadCalls = false;
                        MessageBox.Show("Booking don't valid!");
                        //InfoMessage("This username is not available!\n");
                        CheckForIllegalCrossThreadCalls = true;
                    }
                    if (mess == "2.0")
                    {

                        CheckForIllegalCrossThreadCalls = false;
                        MessageBox.Show("Canceling don't valid!");
                        //InfoMessage("This username is not available!\n");
                        CheckForIllegalCrossThreadCalls = true;
                    }
                    if (mess == "Reload")
                    {
                        reload();
                       
                    }
                    string[]lenh=mess.Split('.');
                    if(lenh[0]=="data")
                    {
                        List<room> roomdata = new List<room>();
                        
                        
                        for (int i = 1; i < lenh.Length; i++)
                        {
                            string[] phong = lenh[i].Split('@');
                            room a= new room();
                            a.idroom = phong[0];
                            
                            a.stat = phong[1];
                            a.day  =   phong[2];
                            roomdata.Add(a);
                        }
                        loaddata(roomdata);
                    }
                }

            }
        }
        public void loaddata(List<room> roomdata)
        {
            
            data.DataSource = roomdata;
            
            data.Refresh();
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

        public void reload()
        {
            SendMessage("3");

        }

        private void UserNamebt_Click(object sender, EventArgs e)
        {
            SendMessage("4");
            book.Text = "Cancel";

        }

        
        string temp;
        private void data_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int rowIndex = e.RowIndex;
            DataGridViewRow row = data.Rows[rowIndex];
            if (row.Cells[1].Value.ToString() == "Occupied")
            {



                temp = "2 " + row.Cells[0].Value.ToString() + " " + row.Cells[2].Value.ToString();
            }
            else
            {
                temp = "1 " + row.Cells[0].Value.ToString() + " " + row.Cells[2].Value.ToString();
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
            book.Text = "Book";
        }
    }
}
