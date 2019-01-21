using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathNet.Numerics.Statistics;
using Newtonsoft.Json;
using Outliers.Models;

namespace Outliers
{
    class Program
    {
        static void Main(string[] args)
        {

            Work();
            Console.Write("done");
        }

        static void Work()
        {
            var dict = new Dictionary<string, List<VaderClassification>>();

            var files = Directory.GetFiles("Data");


            foreach (var file in files)
            {
                using (StreamReader sr = File.OpenText(file))
                {
                    var j = new JsonSerializer();
                    var s = j.Deserialize(sr, typeof(List<VaderClassification>));

                    dict.Add(file.Split('\\')[1], (List<VaderClassification>)s);
                }
            }


            CheckValues(dict);
            
        }

        /// <summary>
        /// check the values in each file for onward outlier detection
        /// </summary>
        /// <param name="keyValuePairs"></param>
        static void CheckValues(Dictionary<string, List<VaderClassification>> keyValuePairs)
        {

            for (int i = 0; i < 1564; i++)
            {
                string word = "";
                List<double> vals = new List<double>();
                foreach (var list in keyValuePairs.Values)
                {
                    word = list[i].Word;
                    vals.Add(Convert.ToDouble(list[i].Value));
                }

                GetQuartilesCheck(vals.ToArray(), word);

            }
        }

        /// <summary>
        /// Check for the interquartile range and also detect outliers
        /// </summary>
        /// <param name="data"></param>
        /// <param name="word"></param>
        static void  GetQuartilesCheck(double[] data, string word)
        {
            Array.Sort(data);

            var result = new double[5];
            result = SortedArrayStatistics.FiveNumberSummary(data);

            var min = result[0];
            var q1 = result[1];
            var median = result[2];
            var q3 = result[3];
            var max = result[4];

            var iqr = q3 - q1;

            var c1 = q1 - (1.5 * iqr);

            var c2 = q3 + (1.5 * iqr);

            foreach (var number in data)
            {
                if (number < c1 || number > c2)
                {
                    Console.WriteLine($"outlier detected. {word}" );
                }
            }
           
        }
    }
}
