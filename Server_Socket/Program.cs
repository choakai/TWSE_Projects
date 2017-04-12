using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Threading;
using Server_Socket.Utilities;
using System.Data.SQLite;
namespace Server_Socket
{
    class Program
    {
        static void Main(string[] args)
        {
            while ((true))
            {
                try
                {
                    
                    System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();//引用stopwatch物件
                    sw.Reset();//碼表歸零
                    sw.Start();//碼表開始計時
                    openTcpListener();
                    sw.Stop();//碼錶停止
                              //印出所花費的總豪秒數
                    string result1 = (sw.Elapsed.TotalMilliseconds/1000).ToString();
                    Console.WriteLine(result1);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            
        }

        public static void openTcpListener()
        {
            Int32 intPort = 0;
            Int32.TryParse(Setting.SERVER_PORT.ToString(), out intPort);

            Utilities.Schedule Schedule = new Utilities.Schedule();
            bool bolSch = Schedule.Schedule_Start();

            TcpListener serverSocket = new TcpListener(System.Net.IPAddress.Parse(Setting.SERVER_IP.ToString()),intPort);
            int requestCount = 0;
            TcpClient clientSocket = default(TcpClient);
            serverSocket.Start();
            Console.WriteLine(" >> Server Started");
            clientSocket = serverSocket.AcceptTcpClient();
            Console.WriteLine(" >> Accept connection from client");
            requestCount = 0;

            while ((true))
            {
                try
                {
                    requestCount = requestCount + 1;
                    NetworkStream networkStream = clientSocket.GetStream();
                    byte[] bytesFrom = new byte[10025];
                    networkStream.Read(bytesFrom, 0, (int)clientSocket.ReceiveBufferSize);
                    string dataFromClient = System.Text.Encoding.ASCII.GetString(bytesFrom);
                    dataFromClient = dataFromClient.Substring(0, dataFromClient.IndexOf("$"));
                    Console.WriteLine(" >> Data from client - " + dataFromClient);
                    string serverResponse = "Last Message from client" + dataFromClient;
                    Byte[] sendBytes = Encoding.ASCII.GetBytes(serverResponse);
                    networkStream.Write(sendBytes, 0, sendBytes.Length);
                    networkStream.Flush();
                    Console.WriteLine("============================================================");
                    Console.WriteLine(" >> " + serverResponse);
                    Console.WriteLine("============================================================");
                    //Thread.Sleep(100);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                    break;
                }
                break;
            }

            //clientSocket.Close();
            clientSocket = null;
            serverSocket.Stop();
            Console.WriteLine(" >> exit");
            //Console.ReadLine();
        }
    }
}

