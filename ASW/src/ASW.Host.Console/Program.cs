using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ASW.Framework.Core;
namespace ASW.Host
{
    public class Program
    {
        static void Main(string[] args)
        {
            ASWHost.Init("orleansconfiguration.xml");
            ASWHost.Start();
            Console.ReadLine();
        }
    }
}
