using Orleans;
using Orleans.Runtime.Configuration;
using Orleans.Runtime.Host;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASW.Framework.Core
{
    public class ASWHost
    {
        public bool Debug
        {
            get { return siloHost != null && siloHost.Debug; }
            set { siloHost.Debug = value; }
        }


        private SiloHost siloHost;
        private ASWHost(string configFile)
        {
            _init(configFile);
        }

        private void _init(string configFile)
        {
            siloHost = new SiloHost(System.Net.Dns.GetHostName());
            siloHost.ConfigFileName = configFile;
        }

        private bool _start()
        {
            bool ok = false;

            try
            {
                siloHost.InitializeOrleansSilo();

                ok = siloHost.StartOrleansSilo();

                if (ok)
                {
                    Console.WriteLine(string.Format("Successfully started Orleans silo '{0}' as a {1} node.", siloHost.Name, siloHost.Type));
                    Logger.Info(string.Format("Successfully started Orleans silo '{0}' as a {1} node.", siloHost.Name, siloHost.Type));
                }
                else
                {
                    throw new SystemException(string.Format("Failed to start Orleans silo '{0}' as a {1} node.", siloHost.Name, siloHost.Type));
                }

                //Guider.Start();
                //if (Guider.IsRunning)
                //{
                //    Console.WriteLine("Guider started");
                //}
                //else
                //{
                //    throw new SystemException(string.Format("Failed to start Guider"));
                //}                
            }
            catch (Exception exc)
            {
                siloHost.ReportStartupError(exc);
                var msg = string.Format("{0}:\n{1}\n{2}", exc.GetType().FullName, exc.Message, exc.StackTrace);
                Console.WriteLine(msg);
                Logger.Error(msg);
            }

            return ok;
        }

        private bool _stop()
        {
            bool ok = false;

            try
            {
                siloHost.StopOrleansSilo();

                Console.WriteLine(string.Format("Orleans silo '{0}' shutdown.", siloHost.Name));
                Logger.Error(string.Format("Orleans silo '{0}' shutdown.", siloHost.Name));
            }
            catch (Exception exc)
            {
                siloHost.ReportStartupError(exc);
                var msg = string.Format("{0}:\n{1}\n{2}", exc.GetType().FullName, exc.Message, exc.StackTrace);
                Logger.Error(string.Format("{0}:\n{1}\n{2}", exc.GetType().FullName, exc.Message, exc.StackTrace));
            }

            return ok;
        }

        private static ASWHost _instance;
        private static object _locker = new object();

        public static void Init(string configFile)
        {
            lock (_locker)
            {
                _instance = new ASWHost(configFile);
            }
        }

        public static bool Start()
        {
            lock (_locker)
            {
                if (_instance != null)
                {
                    return _instance._start();
                }
                else
                {
                    return false;
                }
            }
        }

        public static bool Stop()
        {
            lock (_locker)
            {
                if (_instance != null)
                {
                    return _instance._stop();
                }
                else
                {
                    return false;
                }
            }
        }
    }
}
