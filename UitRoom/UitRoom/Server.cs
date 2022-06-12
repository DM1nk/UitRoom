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
        int random = 0;
        private void AddUser(string userName, TcpClient tClient)
        {
            if(userName=="Login")
            {
                userName =userName+random.ToString();
                random++;
            }
            
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
        public delegate void delsrc(DataTable phong);

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



            swSend.WriteLine("1");
            swSend.Flush();
            try
            {
                string mess = srReceive.ReadLine();
                 
                while ((mess = srReceive.ReadLine()) != null)
                {
                    
                    


                    string[] lenh= mess.Split(' ');
                   

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
                       
                        string ngay = lenh[2] + " " + lenh[3] + " " + lenh[4];
                        if (CheckValidBooking(lenh[1], ngay, currentUser))
                        {

                            Booking(lenh[1], ngay, currentUser);
                            swSend.WriteLine("Reload");
                            swSend.Flush();
                        }
                        else
                        {
                            swSend.WriteLine("1.0");
                            swSend.Flush();
                        }

                    }

                    //lenh 2 la lenh huy phong
                    if (lenh[0] == "2")
                    {
                         
                        string ngay = lenh[2] + " " + lenh[3] + " " + lenh[4];
                        if (CheckValidCanceling(lenh[1], ngay, currentUser))
                        {
                            Cancelling(lenh[1], ngay, currentUser);
                            swSend.WriteLine("Reload");
                            swSend.Flush();

                        }
                        else
                        {
                            swSend.WriteLine("2.0");
                            swSend.Flush();
                        }
                    }
                    if (mess == "3")
                    {

                        string sendmess = SendData();
                        
                            swSend.WriteLine(sendmess);
                            swSend.Flush();
                       
                    }
                    if (mess == "4")
                    {
                        string sendmess = SendUserBookedData(currentUser);
                        
                        swSend.WriteLine(sendmess);
                        
                        swSend.Flush();
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
        public string SendData()
        {
            CheckForIllegalCrossThreadCalls = false;
            string query = "SELECT * FROM dbo.Room WHERE stat = N'Available'";
            DataTable result = DataProvider.Instance.ExecuteQuery(query);
           
            
            DataTb.DataSource = result;
            string send = "data";
            
           
            for (int i = 0; i < DataTb.RowCount - 1; i++)
            {

                DataGridViewRow a = DataTb.Rows[i];
                send = send + "." + a.Cells[1].Value.ToString() + "@" + a.Cells[2].Value.ToString() + "@" + a.Cells[3].Value.ToString() ;

            }
            return send;
            
        }
        public string SendUserBookedData(string username)
        {
            CheckForIllegalCrossThreadCalls = false;
            string query = "SELECT roomid, ngay FROM dbo.Booked WHERE username = N'" + username + "'";
            DataTable result = DataProvider.Instance.ExecuteQuery(query);
            
            DataTb.DataSource = result;
            string send = "data";

            for(int i=0; i<DataTb.RowCount-1; i++)
            {

                DataGridViewRow a=DataTb.Rows[i];
                send = send+"." + a.Cells[0].Value.ToString()+ "@Occupied@" + a.Cells[1].Value.ToString()  ;

            }
            if(send=="data")
            {
                send = null;
            }
            return send;
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
        
       
        bool CheckValidBooking(string roomid,string time ,string username)
        {
            string query = "SELECT * FROM dbo.Booked WHERE username = N'" + username + "' AND ngay = N'" + time + "' ";
            DataTable result = DataProvider.Instance.ExecuteQuery(query);
            if (result.Rows.Count>0)
            {
                return false;
            }
            query = "SELECT * FROM dbo.Room WHERE roomid = N'" + roomid + "' AND ngay = N'" + time + "' AND stat = N'Available'";
            result = DataProvider.Instance.ExecuteQuery(query);
            if (result.Rows.Count == 0)
            {
                return false;
            }
            return true;
        }
        bool CheckValidCanceling(string roomid, string time, string username)
        {
            string query = "SELECT * FROM dbo.Booked WHERE username = N'" + username + "' AND ngay = N'" + time + "' ";
            DataTable result = DataProvider.Instance.ExecuteQuery(query);
            if (result.Rows.Count == 0)
            {
                return false;
            }
            return true;
        }
        void Booking(string roomid,string ngay,string username)
        {
            string query = "UPDATE [dbo].[Room] SET [stat] = N'Occupied' WHERE roomid = N'" + roomid + "' AND ngay = N'" + ngay + "'";

            DataProvider.Instance.ExecuteQuery(query);
            query = "INSERT INTO [dbo].[Booked] ([username],[roomid],[ngay]) VALUES ('"+username+"','"+roomid+"','"+ngay+"')";
            DataProvider.Instance.ExecuteQuery(query);
        }
        void Cancelling(string roomid, string ngay, string username)
        {
            string query = "UPDATE dbo.Room SET stat = N'Available' WHERE roomid = N'" + roomid + "' AND ngay = N'" + ngay + "'";
            DataTable result = DataProvider.Instance.ExecuteQuery(query);
            query = "DELETE FROM [dbo].[Booked] WHERE username = N'" + username + "' AND ngay = N'" + ngay + "' AND roomid = N'"+roomid+"'";
            result = DataProvider.Instance.ExecuteQuery(query);
        }
        void Anouncechange()
        {
            SendMessageToAll(SendData());
        }
    }
}
