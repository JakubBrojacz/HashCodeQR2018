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
            List<Ride> ridesList;
            Input.ReadData(path, out ridesList, out board);
            foreach (var item in ridesList)
            {
                item.t = Tools.InfiniteMetric(item.a, item.b, item.x, item.y);
            }

            List<Car> carsList = new List<Car>();
            for (int i = 0; i < board.F; i++)
            {
                Car car = new Car();
                car.i = i;
                car.x = 0;
                car.y = 0;
                car.t = 0;
                carsList.Add(car);
            }

            ridesList.Sort((x, y) => (x.s.CompareTo(y.s)));
            rides = ridesList.ToArray();
            max_ride = ridesList.Count;
            {
                Car sentiel = new Car();
                sentiel.i = -1;
                sentiel.x = -1;
                sentiel.y = -1;
                sentiel.t = board.T+1;
                carsList.Add(sentiel);
            }
            cars = carsList.ToArray();
            max_car = carsList.Count - 1;
            cleanups = new RideCleanup[ridesList.Count];

            //rides.PrintList(r => r.i.ToString());
            cant_be_better = ridesList.Sum(r => r.t + board.B);
            global_max = 0;
            already_lost = 0;
            SolveRecurrent(0, 0, false, 0);
            SolveRecurrent(0, 0, true, 0);
            Console.WriteLine("points: {0}", global_max);//
            //Console.ReadKey();
            return global_max;
        }

        static int already_lost;
        static int cant_be_better;
        static int global_max;
        static List<int> assigment;
        struct RideCleanup
        {
            public int points_for_ride;
            public int past_t;
            public int past_x;
            public int past_y;
            public int temporary_lost;
            public int actual_car;
            public bool dismiss;
            public int i;
            public void zero(bool dis, int car)
            {
                points_for_ride = 0;
                past_t = 0;
                past_x = 0;
                past_y = 0;
                temporary_lost = 0;
                dismiss = dis;
                actual_car = car;
            }
        }
        static RideCleanup[] cleanups;
        static Board board;
        static Ride[] rides;
        static Car[] cars;
        static int max_ride;
        static int max_car;



        public static void SolveRecurrent(int actual_ride,int actual_car,bool dismiss,int actual_score)
        {
        Start:
            cleanups[actual_ride].zero(dismiss,actual_car);
            if (cleanups[actual_ride].dismiss)
            {
                cleanups[actual_ride].temporary_lost += rides[actual_ride].t + board.B;
                //rides[actual_ride].car = null;
            }
            else
            {
                var c = cars[cleanups[actual_ride].actual_car];
                var r = rides[actual_ride];
                if (c.WhenCanYouGetThere(r.a, r.b) + r.t > r.f)
                {
                    //Console.WriteLine($"Dupa {actual_car} {actual_ride}");
                    actual_ride--;
                    goto Load;
                }
                c.schedule.Add(r);
                if (c.WhenCanYouGetThere(r.a, r.b) <= r.s)
                    cleanups[actual_ride].points_for_ride += board.B;
                else
                    cleanups[actual_ride].temporary_lost += board.B;
                cleanups[actual_ride].points_for_ride += r.t;
                cleanups[actual_ride].past_t = c.t;
                cleanups[actual_ride].past_x = c.x;
                cleanups[actual_ride].past_y = c.y;
                c.t = c.WhenCanYouGetThere(r.a, r.b) + r.t;
                c.x = r.x;
                c.y = r.y;

                actual_score += cleanups[actual_ride].points_for_ride;
            }
            already_lost += cleanups[actual_ride].temporary_lost;


            cleanups[actual_ride].i = -1;
        Load:
            //the only return statement
            if (actual_ride < 0)
                return;

            //check if it is even possible to improve max
            if (cant_be_better - already_lost <= global_max)
                goto Cleanup;

            //all cars were calculated
            if (cleanups[actual_ride].i == max_car)
                goto Cleanup;

            //recurrent
            if (actual_ride<max_ride-1)
            {
            SkipCar:
                cleanups[actual_ride].i+=1;
                actual_car = cleanups[actual_ride].i;
                if (cleanups[actual_ride].i <max_car)
                {
                    if (cars[cleanups[actual_ride].i].x == cars[cleanups[actual_ride].i + 1].x && cars[cleanups[actual_ride].i].y == cars[cleanups[actual_ride].i + 1].y && cars[cleanups[actual_ride].i].t >= cars[cleanups[actual_ride].i + 1].t)
                        goto SkipCar;
                    //SolveRecurrent(actual_ride + 1, cleanups[actual_ride].i, false, actual_score + cleanups[actual_ride].points_for_ride);
                    dismiss = false;
                }else
                {
                    //SolveRecurrent(actual_ride + 1, 0, true, actual_score + cleanups[actual_ride].points_for_ride);
                    dismiss = true;
                }
                actual_ride++;
                goto Start;
                
            }
            else
            {
                Console.WriteLine($"Score: {actual_score}");
                if (actual_score > global_max)
                {
                    global_max = actual_score;
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

        Cleanup:
            //return to initial state 
            already_lost -= cleanups[actual_ride].temporary_lost;
            actual_score -= cleanups[actual_ride].points_for_ride;
            if (!cleanups[actual_ride].dismiss)
            {
                var c = cars[cleanups[actual_ride].actual_car];
                c.schedule.RemoveAt(c.schedule.Count - 1);
                c.t = cleanups[actual_ride].past_t;
                c.x = cleanups[actual_ride].past_x;
                c.y = cleanups[actual_ride].past_y;
            }
            actual_ride--;
            goto Load;
        }
    }
}
