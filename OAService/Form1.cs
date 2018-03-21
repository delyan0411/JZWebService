using Core;
using Core.Common;
using Core.Model;
using System;
using System.Threading;
using System.Windows.Forms;

namespace OAService
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //写成服务 定时调用 多线程调用 
            //读取json配置
            //RuleInfo rules = Settings.GetRuleList;
            //if (rules == null)
            //{
            //    Logger.Error("规则文件不存在或者为空");
            //}
            //else
            //{
            //Thread t1 = new Thread(new ThreadStart(this.Thread141));
            //t1.IsBackground = true;
            //t1.Start();
            //new Timer.Timer141();
            //}
            //Thread t1 = new Thread(new ThreadStart(this.Thread141));
            //t1.IsBackground = true;
            //t1.Start();
            //Thread t2 = new Thread(new ThreadStart(this.Thread142));
            //t2.IsBackground = true;
            //t2.Start();
            //Thread t3 = new Thread(new ThreadStart(this.Thread145));
            //t3.IsBackground = true;
            //t3.Start();
            //new Timer.Timer142();
            //new Timer.test();
            //new Timer.Timer146();
            //new Timer.Timer154();
            //new Timer.Timer147();
            new Timer.Timer173();
        }
        private void Thread141()
        {
            new Timer.Timer141();
        }
        private void Thread142()
        {
            new Timer.Timer142();
        }
        private void Thread145()
        {
            new Timer.Timer145();
        }
        private void Thread146()
        {
            new Timer.Timer146();
        }
        private void Thread147()
        {
            new Timer.Timer147();
        }
        private void Thread149()
        {
            new Timer.Timer149();
        }
        private void Thread154()
        {
            new Timer.Timer154();
        }
        private void Thread168()
        {
            new Timer.Timer168();
        }
        private void Thread169()
        {
            new Timer.Timer169();
        }
        private void Thread173()
        {
            new Timer.Timer173();
        }
        private void Thread180()
        {
            new Timer.Timer180();
        }
        private void Thread179()
        {
            new Timer.Timer179();
        }
        private void Thread175()
        {
            new Timer.Timer175();
        }
        private void Thread177()
        {
            new Timer.Timer177();
        }
        private void Thread181()
        {
            new Timer.Timer181();
        }
        private void Thread178()
        {
            new Timer.Timer178();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            DateTime dt = DateTime.Now;
           string b= string.Format("{0:yyyyMMddHHmmssffff}", dt);
            //int a = (5088 / 800) + 1;
           // int a = (5588 / 800) + 1;
            //double b = 5588 / 800;
            MessageBox.Show(b.ToString());
        }
    }
}
