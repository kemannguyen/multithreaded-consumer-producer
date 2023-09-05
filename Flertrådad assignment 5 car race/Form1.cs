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
    public partial class Form1 : Form
    {
        public static int index = 0;

        Thread update;
        public static List<Task> tasks = new List<Task>();
        public static List<Car> cars = new List<Car>();


        delegate void uppt();
        delegate void writedele();

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            dataGridView1.ColumnCount = 3;
            dataGridView1.Columns[0].Name = "Car Id";
            dataGridView1.Columns[1].Name = "Distance (km)";
            dataGridView1.Columns[2].Name = "Status";   

            //updates
            update = new Thread(UppdateT);
            update.Start();

            //adds fuel consumtion to the list
            for (double i = 0.4; i <= 1.2; i += 0.1)
            {
                string temp = (i + "");
                comboBox1.Items.Add(temp);
                Console.WriteLine(i);
            }

            //adds tank volume to the list
            for (int i = 20; i <= 50; i += 5)
            {
                string temp = (i + "");
                comboBox2.Items.Add(temp);
                Console.WriteLine(temp);
            }
        }

        private void UppdateT()
        {
            while (true)
            {
                Thread.Sleep(100);
                Invoke(new uppt(Uppdate));
            }
        }

        //updates the datagrid cells
        private void Uppdate()
        {
            //What do i wanna update?
            //The report text for the cars
            foreach(Car c in cars)
            { 
                dataGridView1.Rows[c.index].Cells[1].Value = c.GetDis();
                dataGridView1.Rows[c.index].Cells[2].Value = c.status.ToString();
            }
        }
        private void Label2_Click(object sender, EventArgs e)
        {

        }

        //add car button, has an index saved so that the car, carform and row can be matched
        private void Button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text != "" && comboBox1.SelectedItem != null && comboBox2.SelectedItem != null)
            {
                string carId = textBox1.Text;
                double fuel = double.Parse(comboBox1.SelectedItem.ToString());
                double tank = double.Parse(comboBox2.SelectedItem.ToString());
                CarForm c1 = new CarForm(carId, fuel, tank, index);
                Car temp = new Car(carId, fuel, tank, index);
                cars.Add(temp);
                string[] temp2 = { carId, "0", "Not Started"};
                dataGridView1.Rows.Add(temp2);
                c1.Text = "Car: " + carId + " " + index;
                c1.Show();
                index++;
            }
            else if (textBox1.Text != "" && comboBox1.SelectedItem != null && comboBox2.SelectedItem == null)
            {
                MessageBox.Show("Select:  Tank Volume");
            }
            else if (textBox1.Text != "" && comboBox1.SelectedItem == null && comboBox2.SelectedItem != null)
            {
                MessageBox.Show("Select:  Fuel Consumtion");
            }
            else if (textBox1.Text != "" && comboBox1.SelectedItem == null && comboBox2.SelectedItem == null)
            {
                MessageBox.Show("Select:  Fuel Consumtion  \n             Tank Volume");
            }
            else if (textBox1.Text == "" && comboBox1.SelectedItem != null && comboBox2.SelectedItem == null)
            {
                MessageBox.Show("Select:  ID \n             Tank Volume");
            }
            else if (textBox1.Text == "" && comboBox1.SelectedItem == null && comboBox2.SelectedItem != null)
            {
                MessageBox.Show("Select:  ID \n             Fuel Consumtion");
            }
            else if (textBox1.Text == "" && comboBox1.SelectedItem == null && comboBox2.SelectedItem == null)
            {
                MessageBox.Show("Select:  ID \n             Fuel Consumtion\n             Tank Volume");
            }
            else
            {
                MessageBox.Show("You need to select ID");
            }
        }
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            if (update != null)
            {
                update.Abort();
            }

            Console.WriteLine("Closing app");
            base.OnFormClosing(e);
        }

       
    }
}
