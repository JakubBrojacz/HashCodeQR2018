using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Self_driving_rides
{
    static class Backpropagation
    {
        public static int Task(string path)
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

            //rides.PrintList(r => r.i.ToString());
            global_max = 0;
            SolveRecurrent(board, rides.ToArray(), 0, rides.Count, cars.ToArray(), 0, cars.Count, false, 0);
            SolveRecurrent(board, rides.ToArray(), 0, rides.Count, cars.ToArray(), 0, cars.Count, true, 0);
            Console.WriteLine("points: {0}", global_max);//
            //Console.ReadKey();
            return global_max;
        }

        static int global_max;
        static List<int> assigment;


        public static void SolveRecurrent(Board board, Ride[] rides, int actual_ride,int max_ride, Car[] cars,int actual_car,int max_car,bool dismiss,int actual_score)
        {
            //if(dismiss)
            //    Console.WriteLine($"Exx true {actual_ride}");
            //else
            //    Console.WriteLine($"Exx {actual_car} {actual_ride}");
            int points_for_ride = 0;
            int past_t=0;
            int past_x=0;
            int past_y=0;
            if(dismiss)
            {
                //rides[actual_ride].car = null;
            }
            else
            {
                var c = cars[actual_car];
                var r = rides[actual_ride];
                if (c.WhenCanYouGetThere(r.a, r.b) + r.t > r.f)
                {
                    //Console.WriteLine($"Dupa {actual_car} {actual_ride}");
                    return;
                }
                c.schedule.Add(r);
                if (c.WhenCanYouGetThere(r.a, r.b) <= r.s)
                    points_for_ride += board.B;
                points_for_ride += r.t;
                past_t = c.t;
                past_x = c.x;
                past_y = c.y;
                c.t = c.WhenCanYouGetThere(r.a, r.b) + r.t;
                c.x = r.x;
                c.y = r.y;
            }

            if(actual_ride<max_ride-1)
            {
                int tmp_x = -1;
                int tmp_y = -1;
                int tmp_t = -1;
                for(int i=0;i<max_car;i++)
                {
                    if(cars[i].x!=tmp_x || cars[i].y!=tmp_y || cars[i].t!=tmp_t)
                    {
                        SolveRecurrent(board, rides, actual_ride + 1, max_ride, cars, i, max_car, false, actual_score + points_for_ride);
                        tmp_x = cars[i].x;
                        tmp_y = cars[i].y;
                        tmp_t = cars[i].t;
                    }
                }
                SolveRecurrent(board, rides, actual_ride + 1, max_ride, cars, 0, max_car, true, actual_score + points_for_ride);
            }
            else
            {
                Console.WriteLine($"Score: {actual_score}");
                if (actual_score + points_for_ride > global_max)
                {
                    global_max = actual_score + points_for_ride;
                    assigment = new List<int>(board.N + board.F * 2);
                    foreach (var c in cars)
                    {
                        assigment.Add(c.schedule.Count);
                        foreach (var r in c.schedule)
                            assigment.Add(r.i);
                        assigment.Add(-1);
                    }
                }
            }
            

            //return to initial state 
            if (!dismiss)
            {
                var c = cars[actual_car];
                c.schedule.RemoveAt(c.schedule.Count - 1);
                c.t = past_t;
                c.x = past_x;
                c.y = past_y;
            }
        }
    }
}
