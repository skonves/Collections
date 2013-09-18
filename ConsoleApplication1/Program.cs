using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Konves.Collections;

namespace ConsoleApplication1
{
    class Program
    {
        static void Main(string[] args)
        {
            IntervalDictionary<int, string> dict = new IntervalDictionary<int, string>();

            dict.Add(1, 1, "one");
            dict.Add(2, 2, "two");
            dict.Add(3, 3, "three");
            dict.Add(4, 4, "four");
            dict.Add(5, 5, "five");
            dict.Add(6, 6, "six");
            dict.Add(7, 7, "seven");
            dict.Add(8, 8, "eight");
            dict.Add(9, 9, "nine");
            dict.Add(10,10, "ten");
            dict.Add(11,11, "eleven");
            dict.Add(12, 12, "zwulf");
            dict.Add(13, 13, "thirteen");
            dict.Add(14, 14, "fourteen");
            dict.Add(15, 15, "fifteen");


            dict.Remove(12);
            dict.Remove(1);

            //dict.Remove(4);

            dict.Remove(2);

            return;

            var test = new IntervalDictionary<int, string>();
            var dic = new Dictionary<int, string>();

            var c = 1e6; //Math.Pow(2,16)-1;
            var n = 1;

            DateTime start = DateTime.Now;

            for(int i = 0; i < c; i += n)
            {
                //test.Add(i, i + n - 1, i.ToString());
                dic.Add(i, i.ToString());
            }

            DateTime end = DateTime.Now;

            int count = test.Count;

            var t = end.Subtract(start).TotalMilliseconds;

            double elapsed = end.Subtract(start).TotalMilliseconds * n / c;



            IntervalDictionary<int,string> months = new IntervalDictionary<int, string>();

            months.Add(1, 31, "January");
            months.Add(32, 59, "February");
            months.Add(60, 90, "March");
            months.Add(91, 120, "April");
            months.Add(121, 151, "May");
            months.Add(152, 181, "June");
            months.Add(182, 212, "July");
            months.Add(213, 243, "August");
            months.Add(244, 273, "September");
            months.Add(274, 304, "October");
            months.Add(305, 334, "November");
            months.Add(335, 365, "December");

            string month = months[264];

            var x = months.First();

        }
    }
}
