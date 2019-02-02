using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Self_driving_rides
{
    static class Kuba
    {
        static int Task(string path)
        {
            List<Ride> rides;
            Board board;
            Input.ReadData(path, out rides, out board);
            foreach (var item in rides)
            {
                item.t = Tools.InfiniteMetric(item.a, item.b, item.x, item.y);
            }

            List<Car> cars = new List<Car>();
            for (int i = 0; i < board.F; i++)
            {
                Car car = new Car();
                car.i = i;
                car.x = 0;
                car.y = 0;
                car.t = 0;
                cars.Add(car);
            }

            rides.Sort((x, y) => (x.s.CompareTo(y.s)));

            //int points = 0;//
            //int dismissedRides = 0;//
            //int completedRides = 0;//
            //int ridesWithBonuses = 0;//
            //int waitedCycles = 0;//

            return max(SolveRecurrent(board, rides, 0, rides.Count, cars, i, cars.Count, false / true));
        }

        public static int SolveRecurrent(Board board, Ride[] rides, int actual_ride,int max_ride, Car[] cars,int actual_car,int max_car,bool dismiss)
        {
            if (actual_ride > max_ride)
                return 0;
            int points_for_ride = 0;
            if(dismiss)
            {
                rides[actual_ride].car = null;
            }
            else
            {
                var c = cars[actual_car];
                var r = rides[actual_ride];
                if (c.WhenCanYouGetThere(r.a, r.b) + r.t > r.f)
                    return 0;
                r.car = c;
                c.schedule.Add(r);
                if (c.WhenCanYouGetThere(r.a, r.b) <= r.s)
                    points_for_ride += board.B;
                points_for_ride += r.t;
                c.t = c.WhenCanYouGetThere(r.a, r.b) + r.t;
                c.x = r.x;
                c.y = r.y;
            }
                
            int max = 0;
            for(int i=0;i<max_car;i++)
            {
                max = Math.Max(max, SolveRecurrent(board,rides, actual_ride + 1, max_ride, cars, i, max_car,false));
            }
            max = Math.Max(max, SolveRecurrent(board,rides, actual_ride + 1, max_ride, cars, 0, max_car,true));

            //return to initial state - clear c, r 

            return max + points_for_ride;//somehow remember which car was assigned to which ride when points are maxed
        }
    }
}
