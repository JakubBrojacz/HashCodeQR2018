using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Self_driving_rides
{

    public static class Output
    {
        public static void WriteData(string path, List<Car> list)
        {
            using (System.IO.StreamWriter file =
            new System.IO.StreamWriter(path))
            {
                foreach (var v1 in list)
                {
                    file.Write(v1.schedule.Count + " ");
                    foreach(var v2 in v1.schedule)
                    {
                        file.Write(v2 + " ");
                    }
                    file.WriteLine("\n");
                }
            }
        }
    }
}
