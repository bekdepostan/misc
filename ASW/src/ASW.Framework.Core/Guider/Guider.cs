using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ASW.Framework.Core
{
    public static partial class Guider
    {
        private static object _locker = new object();

        private static bool _isRunning = false;

        //puppet
        private static Puppet _puppet;
        private const string _prefiex = "http://localhost:51300/";
        private static readonly SortedList<string, string> _contentStore = new SortedList<string, string>();
        private static readonly TimeSpan _puppetStartTimeout = new TimeSpan(0, 1, 0);

        //JsAgent
        private static JsAgent _jsAgent;
        private static TimeSpan _redirectTimeout = new TimeSpan(0, 0, 2);

        static Guider()
        {
            _puppet = new Puppet(_prefiex, PuppetProcessor);
        }

        public static string GetPostData(string content, out CookieCollection cookies)
        {
            lock (_locker)
            {

                cookies = null;

                if (!IsRunning)
                {
                    return null;
                }

                try
                {
                    //add content
                    var contentId = Guid.NewGuid().ToString();

                    _contentStore.Add(contentId, content);

                    //doRequest
                    var url = string.Format("{0}?id={1}", _prefiex, contentId);
                    var postData = _jsAgent.GetPostData(url, _redirectTimeout, out cookies);

                    //remove content
                    _contentStore.Remove(contentId);

                    //get content
                    return postData;

                }
                catch (Exception ex)
                {
                    return null;
                }
            }
        }

        private static bool PuppetProcessor(HttpListenerRequest request, HttpListenerResponse response)
        {
            string content = null;
            if (request.HttpMethod == "GET")
            {
                var contentId = request.QueryString.Count > 0 ? request.QueryString[0] : null;
                if (string.IsNullOrEmpty(contentId) || !_contentStore.ContainsKey(contentId))
                {
                    response.StatusCode = (int)HttpStatusCode.NotFound;
                    return false;
                }

                content = _contentStore[contentId]??"";
            }
            else
            {
                if (request.HasEntityBody)
                {
                    using (var sr = new StreamReader(request.InputStream))
                    {
                        content = sr.ReadToEnd();
                    }
                }
                //response.ContentType = "application/json";
            }

            byte[] buf = Encoding.UTF8.GetBytes(content);
            response.ContentLength64 = buf.Length;
            response.OutputStream.Write(buf, 0, buf.Length);
            return true;
        }

        public static void Start()
        {
            lock (_locker)
            {
                try
                {
                    var stopwatch = new Stopwatch();
                    _puppet.Start();
                    _jsAgent = new JsAgent();
                    stopwatch.Restart();
                    while (stopwatch.Elapsed < _puppetStartTimeout)
                    {
                        if (_puppet.IsRunning)
                        {
                            stopwatch.Stop();
                            break;
                        }
                        Task.Delay(1000);
                    }
                    _isRunning = true;
                }
                catch (Exception ex)
                {

                }
            }
        }

        public static bool IsRunning
        {
            get
            {
                return _isRunning;
            }
        }

        public static void Stop()
        {
            lock (_locker)
            {
                _puppet.Stop();
                _jsAgent.Quit();
                _isRunning = false;
            }
        }
    }
}
