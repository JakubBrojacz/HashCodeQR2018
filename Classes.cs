using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Self_driving_rides
{
    public class Ride
    {
        public int a, b;    //start point
        public int x, y;    //finish point
        public int s;       //earliest start
        public int f;       //latest finish
        public int t;       //duration
        public int i;       //no of ride
        //public Car car;     //assigned car
    }
    public class Board
    {
        public int R, C;    //size
        public int F;       //number of cars
        public int N;       //number of rides
        public int B;       //per ride bonus for earliest start
        public int T;       //max timestep
    }
    public class Car
    {
        public int i;       //no of car
        public int x, y;    //position
        public int t;       //time when it will be free
        public List<Ride> schedule = new List<Ride>();    //list of rides(?)
        public int WhenCanYouGetThere(int a, int b)
        {
            return Tools.InfiniteMetric(a, b, x, y) + t;
        }
    }
}
