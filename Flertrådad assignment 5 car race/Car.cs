using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flertrådad_assignment_5_car_race
{
    public enum Status
    {
        NotStarted,
        Running,
        Paused,
        OutOfGas,
        Leaving,
        Restarted
    }
    public class Car
    {
        public string id;
        public double fuel;
        public double tank;
        public int index;

        string ownDistance = "0";
        public Status status;

        public Car(string id, double fuel, double tank, int index)
        {
            this.id = id;
            this.fuel = fuel;
            this.tank = tank;
            this.index = index;
            status = Status.NotStarted;
        }

        public string GetId()
        {
            return id;
        }

        public void IncreaseDis(double distance)
        {
            ownDistance = distance.ToString();
        }
        public string GetDis()
        {
            return ownDistance;      
        }
    }
}
