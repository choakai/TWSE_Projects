using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net.Sockets;
using System.Diagnostics;// 匯入System.Diagnostics 命名空間

namespace Client_Socket
{
    public partial class Form1 : Form
    {
        private PerformanceCounter ProcessorUsage = new PerformanceCounter("Processor", "% Processor Time", "_Total");//定義CPU效能計數器
        //System.Net.Sockets.TcpClient clientSocket = new System.Net.Sockets.TcpClient();

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //label1.Text = "Client Socket Program - Server Connected ...";
        }

        private void button1_Click_1(object sender, EventArgs e)

        {
            while (true)
            {
                try
                {

                    msg("Client Started");
                    System.Net.Sockets.TcpClient clientSocket = new System.Net.Sockets.TcpClient();
                    clientSocket.NoDelay = true;
                    int intBreakFlag = 0;
                    for (int i=0;i<3;i++)
                    {
                        try
                        {
                            clientSocket.Connect("127.0.0.1", 8888);
                            clientSocket.ReceiveTimeout = 1000;
                            break;
                        }
                        catch(Exception ex)
                        {
                            //Console.WriteLine("ReTry Connection..."+ex.Message);

                            if (i<3)
                            {
                                intBreakFlag = 1;
                            }
                        }
                    }
                    if (intBreakFlag == 1)
                    {
                        Console.WriteLine("ReTry 3 times , exit.");
                        break;
                    }

                    NetworkStream serverStream = clientSocket.GetStream();

                    //string szCPU = decimal.Round(decimal.Parse(ProcessorUsage.NextValue().ToString()), 0, MidpointRounding.ToEven).ToString();

                    Console.WriteLine(ProcessorUsage.NextValue());
                    System.Threading.Thread.Sleep(333);
                    string szCPU = string.Format("{0:n1}", ProcessorUsage.NextValue());
                    Console.WriteLine(szCPU);
                    //byte[] outStream = System.Text.Encoding.ASCII.GetBytes(textBox2.Text + "$");
                    byte[] outStream = System.Text.Encoding.ASCII.GetBytes(szCPU + "$");

                    serverStream.Write(outStream, 0, outStream.Length);

                    serverStream.Flush();

                    byte[] inStream = new byte[10025];

                    serverStream.Read(inStream, 0, (int)clientSocket.ReceiveBufferSize);

                    string returndata = System.Text.Encoding.ASCII.GetString(inStream);

                    msg(returndata);

                    textBox1.Text = "";

                    textBox1.Focus();
                    
                    clientSocket.Close();
                    clientSocket = null;
                    System.Threading.Thread.Sleep(333);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);

                }
            }
        }



        public void msg(string mesg)

        {

            textBox2.Text = textBox1.Text + Environment.NewLine + " >> " + mesg;

        }


    }

}