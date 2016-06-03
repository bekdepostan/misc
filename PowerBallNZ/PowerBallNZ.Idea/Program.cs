using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerBallNZ
{
    class Program
    {
        static void Main(string[] args)
        {
            InsertCombine();
            Console.WriteLine("finished");
            Console.ReadLine();
        }

        static void InsertCombine()
        {
            using (var db = new Entites())
            {
                int maxDrawNum = 1535;
                if (db.Combines.Count() > 0)
                {
                    db.Combines.Max(x => x.DrawNumber);
                }
                var draws = db.Draws.ToList();
                foreach (var d in draws)
                {
                    if (d.Number <= maxDrawNum) { continue; }
                    //
                    Console.WriteLine(d.Number);
                    var arr = GetCombines(d.LottoStr);
                    foreach (var s in arr)
                    {
                        db.Combines.Add(new Combine()
                        {
                            CombineStr = s,
                            DrawNumber = d.Number
                        });
                    }
                    db.SaveChanges();
                }
            }
        }

        static IList<string> GetCombines(string str)
        {
            var result = new List<string>();
            var arr = str.Split(',');
            var numItem = arr.Length;

            for (int i = 1; i <= Math.Pow(2, numItem); i++)
            {
                string temp = null;
                BitArray b = new BitArray(new int[] { i });
                for (int j = 0; j < numItem; j++)
                {
                    if (b[j])
                    {
                        if (temp != null)
                        {
                            temp += ",";
                        }

                        temp += arr[j];
                    }
                }

                if (!string.IsNullOrEmpty(temp))
                {
                    result.Add(temp);
                }
            }

            return result;
        }
    }
}
