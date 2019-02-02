using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Self_driving_rides
{
    public static class Tools
    {
        static public void PrintList<T>(this List<T> l, System.Func<T, string> f)
        {
            string tmp = "";
            foreach (var o in l)
                tmp += f(o) + " ";
            Console.WriteLine(tmp);
        }

        public static void DisplayStructList<T>(List<T> list)
        {
            foreach (T item in list)
            {
                DisplayStruct<T>(item);
            }
        }
        public static void DisplayStruct<T>(T item)
        {
            foreach (var field in typeof(T).GetFields())
            {
                Console.Write("{0} = {1}  ", field.Name, field.GetValue(item));
            }
            Console.WriteLine("");
        }
        public static int InfiniteMetric(int a, int b, int x, int y)
        {
            return Math.Abs(x - a) + Math.Abs(y - b);
        }
        public static T Max2<T>(T v1, T v2) where T: IComparable
        {
            if (v1.CompareTo(v2) >= 0)
                return v1;
            return v2;
        }
    }
}
