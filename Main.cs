using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Self_driving_rides
{
    static class Program
    {
        static int Main()
        {
            int points = 0;
            string path = "a_example.in";
            Console.WriteLine(path);
            points+=Task(path);
            Console.WriteLine("________");
            path = "b_should_be_easy.in";
            Console.WriteLine(path);
            points += Task(path);
            Console.WriteLine("________");
            path = "c_no_hurry.in";
            Console.WriteLine(path);
            points += Task(path);
            Console.WriteLine("________");
            path = "d_metropolis.in";
            Console.WriteLine(path);
            points += Task(path);
            Console.WriteLine("________");
            path = "e_high_bonus.in";
            Console.WriteLine(path);
            points += Task(path);
            Console.WriteLine("________");
            Console.WriteLine("Points " + points);
            Console.ReadKey();
            return 0;
        }
        static int Task(string path)
        {
            List<Ride> rides;
            Board board;
            Input.ReadData(path, out rides, out board);
            foreach (var item in rides)
            {
                item.t = Tools.InfiniteMetric(item.a,item.b,item.x,item.y);
            }
            //Tools.DisplayStruct(board);
            //Tools.DisplayStructList(rides);
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
            //Tools.DisplayStructList(cars);
            rides.Sort((x, y) => (x.s.CompareTo(y.s)));

            int points = 0;//
            int dismissedRides = 0;//
            int completedRides = 0;//
            int ridesWithBonuses = 0;//
            int waitedCycles = 0;//

            foreach (var ride in rides)
            {
                Car chosen = FindBestCar(ride, cars);
                if (chosen == null)
                {
                    dismissedRides++;//
                    continue;
                }
                completedRides++;//
                if (chosen.WhenCanYouGetThere(ride.x, ride.y) <= ride.s)//
                {//
                    points += board.B + ride.t;//
                    ridesWithBonuses++;//
                    waitedCycles += ride.s - chosen.WhenCanYouGetThere(ride.x, ride.y);//
                }//
                else//
                {//
                    points += ride.t;//
                }//

                chosen.schedule.Add(ride);
                chosen.t = Tools.Max2(chosen.WhenCanYouGetThere(ride.a, ride.b), ride.s) + ride.t;
                chosen.x = ride.x;
                chosen.y = ride.y;
            }
            Output.WriteData(path.Substring(0, path.Length-3) + "_result.in", cars);
            Console.WriteLine("points: {0}", points);//
            Console.WriteLine("dismissedRides: {0}", dismissedRides);//
            Console.WriteLine("completedRides: {0}", completedRides);//
            Console.WriteLine("ridesWithBonuses: {0}", ridesWithBonuses);//
            Console.WriteLine("waitedCycles: {0}", waitedCycles);//
            return points;
        }
        static Car FindBestCar(Ride ride, List<Car> cars)
        {
            Car bestCar = cars[0];
            foreach (var car in cars)
            {
                int whenBest = bestCar.WhenCanYouGetThere(ride.a, ride.b);
                int whenThis = car.WhenCanYouGetThere(ride.a, ride.b);
                if (whenBest == ride.s)
                    break;
                if (whenBest > ride.s)
                    if (whenThis < whenBest)
                        bestCar = car;
                else if(whenBest < ride.s)
                    if(whenThis > whenBest && whenThis <= ride.s)
                        bestCar = car;
            }

            //int debug = bestCar.WhenCanYouGetThere(ride.a, ride.b);
            //if (debug + ride.t > ride.f)
            //    Console.WriteLine($"Ride {ride.i} nie obsłużony na czas");
            //else if (debug == ride.s)
            //    Console.WriteLine($"Ride {ride.i}  obsłużony idealnie");
            //else if (debug > ride.s)
            //    Console.WriteLine($"Ride {ride.i}  obsłużony z {debug - ride.s} poślizgiem");
            //else if (debug < ride.s)
            //    Console.WriteLine($"Ride {ride.i}  obsłużony, musiał czekać {ride.s - debug}");

            if (Tools.Max2(bestCar.WhenCanYouGetThere(ride.a, ride.b), ride.s) + ride.t > ride.f)
                return null;
            return bestCar;
        }
    }
}
