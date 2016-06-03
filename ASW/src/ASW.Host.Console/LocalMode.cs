using System;
using System.Threading.Tasks;

using Orleans;
using Orleans.Runtime.Configuration;

using ASW.Framework.Core;
using ASW.Framework.Simple;
using System.Collections.Generic;
using System.Net;
using System.Collections.Specialized;

namespace ASW.Host
{
    /// <summary>
    /// Orleans test silo host
    /// </summary>
    public class Program3
    {
        static SimpleCenter _center = new SimpleCenter();
        private static ApplicationCase _appCase;
        private static ASWWorker _worker;

        static void Main(string[] args)
        {
            Logger.Info("ASW Start");
            ASWClient client = new ASWClient(new Guid("1312aca2-f973-4965-863b-d87d15cc08c3"), _center);
            client.Start();

            Console.ReadLine();
        }
    }
}
