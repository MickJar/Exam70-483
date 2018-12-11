using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
        public Form1()
        {

            InitializeComponent();
            this.button1.Click += new EventHandler(button1_Click);
            button1.Text = "Start WaitAny Race";
            this.button2.Click += new EventHandler(button2_Click);
            button2.Text = "Start WaitAll Race";
        }

        public static int DoWork(Form1 form, int no, Random rng, CancellationToken ct)
        {
            int count = 0;
            while (count < 20 & !ct.IsCancellationRequested) {
                Thread.Sleep(rng.Next(0, 2000));
                form.incrementProgressBar(no);

                count++;
            }
            return no;
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            resetProgressBars();
            int noThreads = 4;
            Random rng = new Random();
            Task<int>[] tasks = new Task<int>[noThreads];
            var tokenSource = new CancellationTokenSource();
            var token = tokenSource.Token;


            for (int i = 0; i < noThreads; i++)
            {
                int currentIndex = i + 1; // Make a local copy of the loop counter so that the
                                          // correct task number is passed into the lambda function
                tasks[i] = new Task<int>(() => DoWork(this, currentIndex, rng, token));
            }

            foreach (Task task in tasks)
            {
                task.Start();
            }
            //foreach(Task task in tasks)
            //{
            //    task.Wait();
            //}
            var winner = await Task.WhenAny(tasks);
            tokenSource.Cancel();
            MessageBox.Show("The Winner is: " + winner.Result, "Congratulations", MessageBoxButtons.OK, MessageBoxIcon.Information);

        }
        private async void button2_Click(object sender, EventArgs e)
        {
            resetProgressBars();
            int noThreads = 4;
            Random rng = new Random();
            Task<int>[] tasks = new Task<int>[noThreads];
            var tokenSource = new CancellationTokenSource();
            var token = tokenSource.Token;


            for (int i = 0; i < noThreads; i++)
            {
                int currentIndex = i + 1; // Make a local copy of the loop counter so that the
                                          // correct task number is passed into the lambda function
                tasks[i] = new Task<int>(() => DoWork(this, currentIndex, rng, token));
            }

            foreach (Task task in tasks)
            {
                task.Start();
            }
            //foreach(Task task in tasks)
            //{
            //    task.Wait();
            //}
            var winner = await Task.WhenAll(tasks);
            tokenSource.Cancel();
            MessageBox.Show("The Winner is: " + "Everyone is a winner", "Congratulations", MessageBoxButtons.OK, MessageBoxIcon.Information);

        }


        private void resetProgressBars()
        {
            progressBar1.Value = 0;
            progressBar2.Value = 0;
            progressBar3.Value = 0;
            progressBar4.Value = 0;
        }
        public void incrementProgressBar(int identifier)
        {
            ProgressBar progBar = null;
            switch (identifier)
            {
                case 1:
                    progBar = progressBar1;
                    break;
                case 2:
                    progBar = progressBar2;
                    break;
                case 3:
                    progBar = progressBar3;
                    break;
                case 4:
                    progBar = progressBar4;
                    break;
            }

            if (this.InvokeRequired)
            {
                this.EndInvoke(this.BeginInvoke(new MethodInvoker(delegate () { progBar.Value+=5; })));
            }
            else
            {
                progBar.Value+=5;
            }
        }
    }
}
