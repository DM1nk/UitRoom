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
    public partial class Server : Form
    {
        public Server()
        {
            InitializeComponent();
            htUsers = new Hashtable(20);
            htConnections = new Hashtable(20);
        }
        private Thread thdListen;
        private TcpListener tListener;
        private Hashtable htUsers;
        private Hashtable htConnections;
        private void Server_Load(object sender, EventArgs e)
        {
            this.StartListen();
            


        }
        
        // bắt đầu lắng nghe kết nối
        private void StartListen()
        {
            tListener = new TcpListener(IPAddress.Parse("127.0.0.1"), 8080);
            tListener.Start();

            thdListen = new Thread(new ThreadStart(KeepListen));
            thdListen.Start();
        }

        private Thread thdClient;
        private TcpClient tClient;

        // lắng nghe và chấp nhận kết nối mới 
        private void KeepListen()
        {
            while (true)
            {
                tClient = tListener.AcceptTcpClient();
                thdClient = new Thread(AcceptUser);
                thdClient.Start();
            }
        }

        //thêm user vào hashTable để lưu trữ
        private void AddUser(string userName, TcpClient tClient)
        {
            htUsers.Add(userName, tClient);
            htConnections.Add(tClient, userName);
        }

        // gửi message tới tất cả user
        private void SendMessageToAll( string mess)
        {
            TcpClient[] clients = new TcpClient[htUsers.Count];
            htUsers.Values.CopyTo(clients, 0);

            StreamWriter swSend;
            for (int i = 0; i < clients.Length; i++)
            {
                if (mess == "" || clients[i] == null)
                {
                    continue;
                }
                else
                {
                    swSend = new StreamWriter(clients[i].GetStream());
                    swSend.Write(mess);
                    swSend.Flush();
                }
            }
        }

        private delegate void delInfoMessage(string mess);

        
        private void AcceptUser()
        {
            StreamReader srReceive = new StreamReader(tClient.GetStream());
            StreamWriter swSend = new StreamWriter(tClient.GetStream());

            string currentUser = srReceive.ReadLine();
            if (currentUser == "")
            {
                return;
            }

            

            this.AddUser(currentUser, tClient);


            InfoMessage("New connection from " + tClient.Client.RemoteEndPoint.ToString() + "\n");


            swSend.WriteLine("1");
            swSend.Flush();
            try
            {
                string mess = srReceive.ReadLine();
                 
                while ((mess = srReceive.ReadLine()) != null)
                {
                    
                    


                    string[] lenh= mess.Split(' ');
                    InfoMessage(mess + "\n");

                    //lenh 0 la lenh dang nhap
                    if (lenh[0] == "0")
                    {
                        if(Login(lenh[1], lenh[2]))
                        {
                           
                            swSend.WriteLine("0.1");
                            swSend.Flush();
                        }
                        else
                        {
                            
                            swSend.WriteLine("0.0");
                            swSend.Flush();
                        }
                    }

                    //lenh 1 la lenh dang ki phong
                    if(lenh[0]=="1")
                    {

                    }

                    //lenh 2 la lenh huy phong
                    if (lenh[0] == "2")
                    {

                    }

                }
            }
            catch
            {
                htUsers.Remove(currentUser);
            }
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {

        }
       
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
        bool Login(string username,string password)
        {
            string query = "SELECT * FROM dbo.Account WHERE username = N'"+username+"' AND pass = N'"+password+"' ";
            DataTable result = DataProvider.Instance.ExecuteQuery(query);
            return result.Rows.Count > 0;
        }
        
        private void InfoMessage(string mess)
        {
            if (a.InvokeRequired)
            {
                delInfoMessage delReceiveMess = new delInfoMessage(InfoMessage);
                a.Invoke(delReceiveMess, new object[] { mess });
            }
            else
            {
                a.Items.Add(mess + "\n");
            }
        }
    }
}
