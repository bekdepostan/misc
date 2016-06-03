using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;


namespace DeCode
{
    class Program
    {
        static void Main(string[] args)
        {
            var iI = "08fab73d718318014f6cd2ed3567035fa901732eb3a311efecf6f9da388bffcdb2dbc5d041c0436231d9f4db391c36bc24ff06fbbe55809e73481f414da73eb1d6dea6892e9ba4d6463af18e78560e33443923a95b31bbd160071f7eb1435a2f06584cbfc9deed46032a02f58369cf4e49d34ae599f83dd3213807a46f1008bcf548d06553c1ec76c927f503d8e0f34740138efe7e705a8b832a0e62eb5b369983028de117debe3a52caf4bb1fb654f68b8251ead81c3515036afb152e3be8ea2cd65851f05e7f5d8df2efc29e602015815e1e1e20cc0f59ad9257b421eb4523452dd03027958133040fe803083344a83349671e2d9fb848f918350ae4a1dfcf96fb71efc9856b12bf7d85028610a1d461467c8f92bbc4866be11832369817dc";
            var step1 = mySecuremsg.Step1(iI);
            Console.WriteLine(step1);


            var str = securemsg.securemsg_sJ(iI);
            Console.WriteLine(str);
            Console.ReadLine();
        }
    }

    static class securemsg
    {
        //securemsg.SJ
        public static string securemsg_sJ(string str)
        {
            var list = securemsg__j(0, str.Length, 2);
            IList<char> result = new List<char>();
            StringBuilder sb = new StringBuilder();
            foreach (int i in list)
            {
                int dec = Convert.ToInt32(str.Substring(i, 2), 16);
                sb.Append((char)dec);
            }
            return sb.ToString();
        }

        //securemsg._j
        static IList<int> securemsg__j(int S, int J, int l)
        {
            var list = new List<int>();
            for (; S < J; S += l)
            {
                list.Add(S);
            }

            return list;
        }

        public static string securemsg_oj()
        {

        }
        public static string securemsg_js(int S, int J)
        {
            string l = "";
            string O = "0" + S.ToString("X");
            var z = O.Length;
            for (; z > 0; z -= 2)
            {
                //String["fromCharCode"](parseInt(O['slice'](z - 2, z), 16)));
                l += Convert.ToString((char)Convert.ToInt32(O.Substring(z - 2, z), 16));
            }

            J = J > l.Length ? J : l.Length;
            return l; 
        }
    }

    static class mySecuremsg
    {
        //Convert window.sI.iI to char dictionary
        public static string Step1(string iI)
        {
            var len = iI.Length / 2;
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < len; i++)
            {
                var hexStr = iI.Substring(i * 2, 2);
                int dec = Convert.ToInt32(hexStr, 16);
                sb.Append((char)dec);
            }

            return sb.ToString();
        }
    }
}