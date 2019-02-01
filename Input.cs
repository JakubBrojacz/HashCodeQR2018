using System.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Self_driving_rides
{
    public class Ride
    {
        public int a, b, x, y, s, f, t, i;
    }
    public class Board
    {
        public int R, C, F, N, B, T;
    }
    public static class Input
    {
        public static void ReadData(string path, out List<Ride> rides, out Board board)
        {
            rides = new List<Ride>();
            board = new Board();
            var lines = File.ReadLines(path);
            string firstLine = lines.First();
            string[] v = firstLine.Split(' ');
            board.R = int.Parse(v[0]);
            board.C = int.Parse(v[1]);
            board.F = int.Parse(v[2]);
            board.N = int.Parse(v[3]);
            board.B = int.Parse(v[4]);
            board.T = int.Parse(v[5]);
            lines = lines.Skip(1);
            int i = 0;
            foreach (string line in lines)
            {
                string[] vs = line.Split(' ');
                Ride ride = new Ride();
                ride.a = int.Parse(vs[0]);
                ride.b = int.Parse(vs[1]);
                ride.x = int.Parse(vs[2]);
                ride.y = int.Parse(vs[3]);
                ride.s = int.Parse(vs[4]);
                ride.f = int.Parse(vs[5]);
                ride.i = i;
                i++;
                rides.Add(ride);
            }
        }
    }
}
