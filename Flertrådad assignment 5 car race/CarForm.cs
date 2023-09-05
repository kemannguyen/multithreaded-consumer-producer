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

namespace Flertrådad_assignment_5_car_race
{
    public partial class CarForm : Form
    {

        string state = "stop";
        delegate void update();
        CancellationTokenSource canceller { get; set; }
        private Task<object> worker { get; set; }

        int ownindex;
        string ownID;
        double fuelCon;
        double tankVol;
        double curFuel;
        double distance = 0;
        double speed = 0;
        Task<double> t;

        Car ownCar;
        public CarForm(string id, double fuel, double tank, int index)
        {
            ownID = id;
            fuelCon = fuel;
            tankVol = tank;
            ownindex = index;
            InitializeComponent();
        }

        //matches the car and the box
        private void CarForm_Load(object sender, EventArgs e)
        {
           
            button2.Enabled = false;
            curFuel = tankVol;
            label3.Text = "Fuel consumption: " + fuelCon;
            label4.Text = "Fuel: " + curFuel + "/" + tankVol;
            label5.Text = "Distance: 0";
            foreach (Car c in Form1.cars)
            {
                if (c.index == ownindex)
                {
                    ownCar = c;
                }
            }
        }

        //STOP button, cancels the task if its still running
        private void Button3_Click(object sender, EventArgs e)
        {
            ownCar.status = Status.Leaving;

            if (ownCar.status == Status.Leaving)
            {
                Console.WriteLine(ownCar.status);
                Closing();
            }
        }

        private void Closing()
        {
            state = "stop";
            if (canceller != null)
            {
                canceller.Cancel();
            }
            Thread.Sleep(3000);

            //Form1.cars.Remove(ownCar);

            this.Close();
        }


        //START button
        private void Button1_Click(object sender, EventArgs e)
        {

            state = "start";
            speed = trackBar1.Value * 5;
            if (state == "start")
            {
                Console.WriteLine("Starting car " + ownID + " " + fuelCon + " " + tankVol + " " + speed);
                ownCar.status = Status.Running;
                Start(Driving);

                Console.WriteLine(worker.Status);

                //Form1.tasks.Add(t);
                trackBar1.Enabled = false;
            }
            else
            {
                Console.WriteLine("Car already driving...");
                Console.WriteLine(worker.Status);
            }
            button1.Enabled = false;
            button2.Enabled = true;
        }

        public void Start(Func<object> DoFunc)
        {

            // start a task with a means to do a hard abort (unsafe!)
            canceller = new CancellationTokenSource();

            worker = Task.Factory.StartNew(() =>
            {
                try
                {
                    // specify this thread's Abort() as the cancel delegate
                    using (canceller.Token.Register(Thread.CurrentThread.Abort))
                    {
                        return DoFunc();
                    }
                }
                catch (ThreadAbortException)
                {
                    return false;
                }
            }, canceller.Token);
        }


        object Driving()
        {
            //Loop while exit and pause button isnt pressed(force keep the thread)
            while (state == "start")
            {
                if (curFuel > 0 && ownCar.status == Status.Restarted || curFuel > 0 && ownCar.status == Status.Running)
                {
                    Thread.Sleep(1000);
                    Thread.Sleep(1000);
                    Thread.Sleep(1000);
                    Thread.Sleep(1000);
                    Thread.Sleep(1000);

                    curFuel = curFuel - (speed* fuelCon * 0.1 * 0.5);
                    if (curFuel < 0)
                    {
                        curFuel = 0;
                    }

                    distance += speed * 0.5;
                    ownCar.IncreaseDis(distance);
                }
                else if(curFuel == 0 && ownCar.status == Status.Running)
                {

                    ownCar.status = Status.OutOfGas;
                    Thread.Sleep(1000);
                    if (MessageBox.Show("Car " + ownID + " is now refilled!") == System.Windows.Forms.DialogResult.OK)
                    {
                        ownCar.status = Status.Restarted;
                        Thread.Sleep(1000);
                    }
                }
                else
                {
                    ownCar.status = Status.Running;
                    curFuel = tankVol;
                }

                Invoke(new update(updateText));
                //debug text
                Console.WriteLine("CarID: " + ownID);
                Console.WriteLine("Fuel in tank: " + curFuel);
                Console.WriteLine("Dis: " + distance + " km\n");
            }
            button2.Enabled = true;
            return 1;
        }
        private void updateText()
        {
            label4.Text = "Fuel: " + curFuel + "/" + tankVol;
            label5.Text = "Distance: " + distance;
        }

        //PAUSE button
        private void Button2_Click(object sender, EventArgs e)
        {
            ownCar.status = Status.Paused;
            state = "paus";
            canceller.Cancel();


            trackBar1.Enabled = true;
            button1.Enabled = true;
            button2.Enabled = false;
        }
    }
}
