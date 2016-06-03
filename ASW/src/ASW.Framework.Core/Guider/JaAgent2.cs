using OpenQA.Selenium;
using OpenQA.Selenium.PhantomJS;
using System.Collections.ObjectModel;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium.Support.UI;
using System.Net;
using NReco.PhantomJS;
using System.IO;

namespace ASW.Framework.Core
{
    public static class JaAgent2
    {
        private static PhantomJS _phantomJS;
        private static bool _finished = false;
        private static string _result = null;
        private static bool _hasError = false;
        private static object _locker = new object();
        private static Stopwatch _stopWatch = new Stopwatch();
        private static TimeSpan _timeout = new TimeSpan(0, 0, 20);

        static JaAgent2()
        {
            _phantomJS = new PhantomJS();
            _phantomJS.OutputReceived += PhantomJSOutputReceived;
            _phantomJS.ErrorReceived += PhantomJSErrorReceived;

        }

        static void PhantomJSOutputReceived(object sender, DataReceivedEventArgs e)
        {
            if (!string.IsNullOrEmpty(e.Data))
            {
                _result = e.Data;
            }
            _finished = true;
        }
        static void PhantomJSErrorReceived(object sender, DataReceivedEventArgs e)
        {
            _hasError = true;
        }

        public static string GetPostData(string content)
        {
            lock (_locker)
            {
                _stopWatch.Start();

                try
                {
                    File.WriteAllText("secure_temp.html", content);
                    _result = null;
                    _hasError = false;
                    _finished = false;
                    _phantomJS.Run("secure_calculator.js", new string[] { });
                    //_phantomJS.Run("2.js", new string[] { });
                    while (!_hasError && !_finished && _stopWatch.Elapsed < _timeout)
                    {
                        Task.Delay(100);
                    }
                }
                catch
                {
                    return null;
                }
                finally
                {
                    _stopWatch.Stop();
                }

                if (_hasError || !_finished)
                {
                    return null;
                }
                else
                {
                    return _result;
                }
            }
        }
    }
}
