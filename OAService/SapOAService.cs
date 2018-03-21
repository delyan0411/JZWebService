using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Core.Common;

namespace OAService
{
    partial class SapOAService : ServiceBase
    {
        public SapOAService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            Logger.Log("服务开始");
            // TODO: 在此处添加代码以启动服务。
            Thread t1 = new Thread(new ThreadStart(this.Thread141));            
            t1.IsBackground = true;
            t1.Start();


            Thread t2 = new Thread(new ThreadStart(this.Thread142));
            t2.IsBackground = true;
            t2.Start();


            Thread t3 = new Thread(new ThreadStart(this.Thread145));
            t3.IsBackground = true;
            t3.Start();

            Thread t4 = new Thread(new ThreadStart(this.Thread146));
            t4.IsBackground = true;
            t4.Start();

            Thread t5 = new Thread(new ThreadStart(this.Thread147));
            t5.IsBackground = true;
            t5.Start();

            Thread t6 = new Thread(new ThreadStart(this.Thread149));
            t6.IsBackground = true;
            t6.Start();

            Thread t7 = new Thread(new ThreadStart(this.Thread154));
            t7.IsBackground = true;
            t7.Start();

            Thread t8 = new Thread(new ThreadStart(this.Thread168));
            t8.IsBackground = true;
            t8.Start();

            Thread t9 = new Thread(new ThreadStart(this.Thread169));
            t9.IsBackground = true;
            t9.Start();

            Thread t10 = new Thread(new ThreadStart(this.Thread173));
            t10.IsBackground = true;
            t10.Start();

            Thread t11 = new Thread(new ThreadStart(this.Thread175));
            t11.IsBackground = true;
            t11.Start();

            Thread t12 = new Thread(new ThreadStart(this.Thread177));
            t12.IsBackground = true;
            t12.Start();

            Thread t13 = new Thread(new ThreadStart(this.Thread178));
            t13.IsBackground = true;
            t13.Start();

            Thread t14 = new Thread(new ThreadStart(this.Thread179));
            t14.IsBackground = true;
            t14.Start();

            Thread t15 = new Thread(new ThreadStart(this.Thread180));
            t15.IsBackground = true;
            t15.Start();

            Thread t16 = new Thread(new ThreadStart(this.Thread181));
            t16.IsBackground = true;
            t16.Start();

        }

        protected override void OnStop()
        {
            Logger.Log("服务结束");
            // TODO: 在此处添加代码以执行停止服务所需的关闭操作。
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
        private void Thread175()
        {
            new Timer.Timer175();
        }
        private void Thread177()
        {
            new Timer.Timer177();
        }
        private void Thread178()
        {
            new Timer.Timer178();
        }
        private void Thread179()
        {
            new Timer.Timer179();
        }
        private void Thread180()
        {
            new Timer.Timer180();
        }
        private void Thread181()
        {
            new Timer.Timer181();
        }
    }
}
