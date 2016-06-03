using RestSharp;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace ASW.Framework.Core
{
    public class ASWWorker
    {
        private string _caseId;

        private ASWClient _aswClient;
        private RestClient _restClient;
        private WebRequestJob _preJob = WebRequestJob.EmptyJob;
        private WebRequestJob _currentJob = WebRequestJob.EmptyJob;
        private Stopwatch _stopWatch = new Stopwatch();

        public ASWWorker(string caseId, ASWClient aswClient)
        {
            _caseId = caseId;
            _aswClient = aswClient;
            _restClient = new RestClient();
            _restClient.CookieContainer = new CookieContainer();
            _restClient.UserAgent = "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/50.0.2661.102 Safari/537.36";
            _restClient.Timeout = 15000;
        }

        public int Index { get; set; }

        public void Start()
        {
            RunAsync();
            Console.WriteLine(string.Format("start case [{0}]", _caseId));
        }

        public void Stop()
        {
            Console.WriteLine(string.Format("stop case [{0}]", _caseId));
        }

        private async void RunAsync()
        {
            Console.WriteLine(string.Format("start run case [{0}]", _caseId));
            if (_aswClient == null || _restClient == null)
            {
                return;
            }

            while (true)
            {
                _currentJob = await _aswClient.GetJob(_caseId, _preJob);
                if (_currentJob == null)
                {
                    _aswClient.Report(_caseId);
                    return;
                }

                while (_currentJob.JobType != JobTypes.Finish)
                {
                    //Console.WriteLine(string.Format("[{0} {1}]", _currentJob.Name, _currentJob.Code));
                    _currentJob.JobResult = DoJob(_currentJob);
                    _preJob = _currentJob;

                    //update
                    //_aswClient.Report(_caseId);

                    if (_currentJob.JobResult != JobResults.RepeatNeeded)
                    {
                        //out of loop
                        Console.WriteLine(string.Format("{0:HH:mm:ss}-{1}[{2}]:{3}|{4}", 
                                                DateTime.Now, _currentJob.Code, Index,
                                                _currentJob.Name, _currentJob.JobResult));
                        break;
                    }

                    //
                    //await Task.Delay(_currentJob.RepeatInterval);
                    Console.WriteLine(string.Format("{0:HH:mm:ss}-{1}[{2}]:{3}|{4}",
                                            DateTime.Now, _currentJob.Code, Index,
                                            _currentJob.Name, _currentJob.JobResult));
                }
            }
        }

        public JobResults DoJob(WebRequestJob job)
        {
            _restClient.BaseUrl = new Uri(job.BaseUrl);
            var request = new RestRequest(job.Resource, job.Method);

            string respContent = null;
            try
            {
                //header
                foreach (var h in job.Headers)
                {
                    request.AddHeader(h.Key, h.Value);
                }

                //parameters
                foreach (var p in job.Parameters)
                {
                    request.AddParameter(p.Key, p.Value);
                }

                _stopWatch.Restart();
                IRestResponse response = _restClient.Execute(request);
                _stopWatch.Stop();
                Logger.Req(string.Format("REQUEST SPEND {0}ms, {1}", _stopWatch.ElapsedMilliseconds,
                    response.StatusDescription));
                if (response.ResponseStatus == ResponseStatus.TimedOut)
                {
                    Logger.Info("Response Time Out");
                    return JobResults.RepeatNeeded;
                }
                else if(response.ResponseStatus == ResponseStatus.Error)
                {
                    Console.WriteLine("Response Error, retry after 5 secs");
                    Logger.Info("Response Error, retry after 5 secs");
                    Task.Delay(5000);
                    return JobResults.RepeatNeeded;
                }
                else
                {
                    respContent = response.Content;
                }

                return job.Do(respContent);

            }
            catch (Exception ex)
            {
                Logger.Error(ex.Message);
                return JobResults.Excepted;
            }
        }
    }
}
