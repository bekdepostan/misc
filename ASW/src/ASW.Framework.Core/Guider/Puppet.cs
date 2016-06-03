using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ASW.Framework.Core
{
    public static partial class Guider
    {
        internal class Puppet
        {
            private string _prefix = null;
            private bool _isRunning = false;
            private HttpListener _listener = new HttpListener();
            public Func<HttpListenerRequest, HttpListenerResponse, bool> _reqProcessor;


            public Puppet(string prefix, Func<HttpListenerRequest, HttpListenerResponse, bool> processor)
            {
                _prefix = prefix;
                _reqProcessor = processor;
            }

            public void Start()
            {
                if (_isRunning)
                {
                    return;
                }

                if (!HttpListener.IsSupported)
                {
                    return;
                }

                try
                {
                    _listener = new HttpListener();
                    _listener.Prefixes.Add(_prefix);

                    _listener.Start();

                    Console.WriteLine("Listening...");
                    Run();
                }
                catch (Exception ex)
                {
                }
            }

            public void Stop()
            {
                _listener.Stop();
                _listener.Close();
                _isRunning = false;
            }

            private void Run()
            {
                ThreadPool.QueueUserWorkItem((o) =>
                {
                    _isRunning = true;
                    Console.WriteLine("Puppet is running...");
                    try
                    {
                        while (_listener.IsListening)
                        {
                            ThreadPool.QueueUserWorkItem((c) =>
                            {
                                var ctx = c as HttpListenerContext;
                                try
                                {
                                    _reqProcessor(ctx.Request, ctx.Response);
                                }
                                catch { } // suppress any exceptions
                                finally
                                {
                                    // always close the stream
                                    ctx.Response.OutputStream.Close();
                                }
                            },
                            _listener.GetContext());
                        }
                    }
                    catch { } // suppress any exceptions
                });
            }

            public bool IsRunning
            {
                get
                {
                    return _isRunning;
                }
            }
        }
    }
}