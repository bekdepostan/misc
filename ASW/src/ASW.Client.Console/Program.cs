using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using ASW.Framework.Core;
namespace ASW.Client
{
    /// <summary>
    /// Orleans test silo host
    /// </summary>
    public class Program
    {
        static void Main(string[] args)
        {
            ASWClient client = new ASWClient(new Guid("1312aca2-f973-4965-863b-d87d15cc08c3"), "ClientConfiguration.xml");
            client.Start();
            Console.ReadLine();
        }
    }
}
