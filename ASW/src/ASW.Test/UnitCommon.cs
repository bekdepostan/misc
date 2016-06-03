using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using ASW.Framework.Core;
using ASW.Framework.Simple;
using System.Threading.Tasks;

namespace ASW.Test
{
    [TestClass]
    public class UnitCommon
    {
        private static ASWWorker _worker;
        private ApplicationCase _appCase;

        [TestInitialize]
        public void Init()
        {
            //Guider.Start();
            //Console.WriteLine("Guider starting ...");
            //while (!Guider.IsRunning)
            //{
            //    Task.Delay(1000);
            //}
            Console.WriteLine("Guider started");
            _appCase = new ApplicationCase()
            {
                Id = "4f890d2b-7d1e-4932-8585-118254e0f375",
                Username = "mango_003",
                Password = "Godman2016"
            };
            _worker = new ASWWorker(_appCase.Id, null);
        }
    }
}
