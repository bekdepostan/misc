﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ASW.Framework.Core;
using System.Net;
using System.Collections.Specialized;
using System.Web;
using Newtonsoft.Json;

namespace ASW.Framework.Simple
{
    public static class CommonJobNames
    {
        public const string JsResultNeeded = "JS_RESULT_NEEDED";
        public const string LoginHomePage = "LOGIN_HOMEPAGE";
        public const string LoginSubmit = "LOGIN_SUBMIT";
        public const string CardType = "CARD_TYPE";
        public const string CardDetail = "CARD_DETAIL";
        public const string Pay = "PAY";
    }

    public static class CommonJobs
    {
        public static WebRequestJob CreateJsResultJob(ApplicationCase appCase, WebRequestJob currentJob)
        {
            var postData = JaAgent2.GetPostData(currentJob.ResponseContent);
            currentJob.SetParameter(ParseDataFromString(postData));
            currentJob.Method = RestSharp.Method.POST;
            currentJob.JobResult = JobResults.None;
            return currentJob;
        }
        
        //login homepage
        public static WebRequestJob CreateLoginHomeJob(ApplicationCase appCase)
        {
            var job = new WebRequestJob(CommonJobNames.LoginHomePage, appCase.AppId)
            {
                Method = RestSharp.Method.GET,
                JobType = JobTypes.Normal
            };

            //url
            job.BaseUrl = @"https://onlineservices.immigration.govt.nz/";
            job.Resource = @"secure/status.htm";

            //matcher
            job.AddMatcher(new WebContentMatcher(JobResults.Succeed,
                                                new string[] { @"<form name=""Login"" method=""post""" }));
            //retrieving
            job.AddRetriever(new WebContentRetriever(@"<form name=""Login"".*action=""(?<url>.*?)"""));
            job.AddRetriever(new WebContentRetriever(@"<input.*name=""__EVENTTARGET"".*value=""(?<eventTarget>.*?)"""));
            job.AddRetriever(new WebContentRetriever(@"<input.*name=""__EVENTARGUMENT"".*value=""(?<eventArgument>.*?)"""));
            job.AddRetriever(new WebContentRetriever(@"<input.*name=""__VIEWSTATE"".*value=""(?<viewstate>.*?)"""));
            job.AddRetriever(new WebContentRetriever(@"<input.*name=""__VIEWSTATEGENERATOR"".*value=""(?<viewstateGenerator>.*?)"""));

            return job;
        }

        //login submit
        public static WebRequestJob CreateLoginSubmitJob(ApplicationCase appCase, WebRequestJob currentJob)
        {
            if (currentJob == null || !currentJob.HasResultData)
            {
                return null;
            }

            var job = new WebRequestJob(CommonJobNames.LoginSubmit, appCase.AppId)
            {
                Method = RestSharp.Method.POST,
                JobType = JobTypes.Normal
            };

            //url
            job.BaseUrl = @"https://onlineservices.immigration.govt.nz/";
            job.Resource = @"Templates/Secure/Login.aspx?NRMODE=Published&NRNODEGUID=%7b31097FC2-72ED-4420-BF02-6755811E2916%7d&NRORIGINALURL=%2fsecure%2fstatus%2ehtm&NRCACHEHINT=Guest";

            //post data
            job.SetParameter("__EVENTTARGET", currentJob.RetrievedData["eventTarget"]);
            job.SetParameter("__EVENTARGUMENT", currentJob.RetrievedData["eventArgument"]);
            //job.SetParameter("__VIEWSTATE", preJobData["viewstate"]);
            job.SetParameter("__VIEWSTATE", "dDwxMDA1ODA0MzMwO3Q8O2w8aTwwPjtpPDE+Oz47bDx0PDtsPGk8Mz47PjtsPHQ8cDxsPFZpc2libGU7PjtsPG88Zj47Pj47Oz47Pj47dDw7bDxpPDA+O2k8Mj47aTw0PjtpPDg+O2k8MTI+O2k8MTQ+O2k8MTY+O2k8MTg+O2k8MjA+Oz47bDx0PDtsPGk8MD47aTwyPjtpPDM+Oz47bDx0PHA8bDxWaXNpYmxlOz47bDxvPGY+Oz4+O2w8aTwxPjs+O2w8dDw7bDxpPDE+O2k8Mz47aTw1PjtpPDc+O2k8OT47aTwxMT47PjtsPHQ8cDxsPFRleHQ7PjtsPCAtIDs+Pjs7Pjt0PHA8bDxWaXNpYmxlOz47bDxvPGY+Oz4+Ozs+O3Q8O2w8aTwxPjs+O2w8dDxwPGw8dGl0bGU7b25jbGljaztpbm5lcmh0bWw7PjtsPExpbmsgb3BlbnMgaW4gYSBuZXcgd2luZG93O3dpbmRvdy5vcGVuKCcvcmVnaXN0cmF0aW9uL21hbmFnZXN1aXRjYXNlLmFzcHgnLCdtYW5hZ2VzdWl0Y2FzZXBvcHVwJywndG9vbGJhcj1ubyxsb2NhdGlvbj1ubyxzdGF0dXM9eWVzLG1lbnViYXI9bm8sc2Nyb2xsYmFycz15ZXMscmVzaXphYmxlPXllcyx3aWR0aD04NDAsaGVpZ2h0PTUyMCcpXDsgcmV0dXJuIGZhbHNlO01hbmFnZSBGYXZvdXJpdGVzOz4+Ozs+Oz4+O3Q8O2w8aTwxPjs+O2w8dDxwPGw8dGl0bGU7b25jbGljaztpbm5lcmh0bWw7PjtsPExpbmsgb3BlbnMgaW4gYSBuZXcgd2luZG93O3dpbmRvdy5vcGVuKCcvcmVnaXN0cmF0aW9uL3doYXRzaGFwcGVuaW5nLmFzcHgnLCd3aGF0c2hhcHBlbmluZ3BvcHVwJywndG9vbGJhcj1ubyxsb2NhdGlvbj1ubyxzdGF0dXM9eWVzLG1lbnViYXI9bm8sc2Nyb2xsYmFycz15ZXMscmVzaXphYmxlPXllcyx3aWR0aD04NDAsaGVpZ2h0PTUyMCcpXDsgcmV0dXJuIGZhbHNlO015IEFwcGxpY2F0aW9uOz4+Ozs+Oz4+O3Q8cDxsPGhyZWY7PjtsPC9SZWdpc3RyYXRpb24vTG9nT3V0LmFzcHg7Pj47Oz47dDxwPGw8VGV4dDs+O2w8XDxhIGhyZWY9Imh0dHA6Ly93d3cuaW1taWdyYXRpb24uZ292dC5uei9taWdyYW50L2dlbmVyYWwvYWJvdXRuemlzL2NvbnRhY3R1cy9kZWZhdWx0Lmh0bSJcPkNvbnRhY3QgVXNcPC9hXD47Pj47Oz47Pj47Pj47dDw7bDxpPDA+Oz47bDx0PHA8bDxWaXNpYmxlOz47bDxvPGY+Oz4+Ozs+Oz4+O3Q8O2w8aTwwPjs+O2w8dDxwPGw8VGV4dDs+O2w8XDxhIGhyZWY9Ii8iXD5Ib21lXDwvYVw+ICZndFw7IFw8YSBocmVmPSIvc2VjdXJlLyJcPkxvZ2luXDwvYVw+Oz4+Ozs+Oz4+Oz4+O3Q8O2w8aTwwPjs+O2w8dDxwPGw8VmlzaWJsZTs+O2w8bzxmPjs+Pjs7Pjs+Pjt0PDtsPGk8MD47PjtsPHQ8cDxsPFZpc2libGU7PjtsPG88Zj47Pj47Oz47Pj47dDx0PHA8bDxEYXRhVGV4dEZpZWxkO0RhdGFWYWx1ZUZpZWxkOz47bDxLZXk7VmFsdWU7Pj47dDxpPDQ+O0A8Q2hlY2sgdGhlIHN0YXR1cyBvZiB5b3VyIGFwcGxpY2F0aW9uO1NraWxsZWQgTWlncmFudCAtIGV4cHJlc3Npb24gb2YgaW50ZXJlc3Q7U2lsdmVyIEZlcm4gSm9iIFNlYXJjaCB2aXNhO1dvcmtpbmcgSG9saWRheSB2aXNhOz47QDwvc2VjdXJlL3N0YXR1cy5odG07L3NlY3VyZS9Mb2dpbitTa2lsbGVkK01pZ3JhbnQuaHRtOy9zZWN1cmUvTG9naW4rU2lsdmVyK0Zlcm4uaHRtOy9zZWN1cmUvTG9naW4rV29ya2luZytIb2xpZGF5Lmh0bTs+Pjs+Ozs+O3Q8cDxsPFZpc2libGU7PjtsPG88Zj47Pj47bDxpPDE+Oz47bDx0PDtsPGk8MD47PjtsPHQ8cDxsPFZpc2libGU7PjtsPG88Zj47Pj47bDxpPDE+Oz47bDx0PHA8cDxsPFRleHQ7PjtsPFNlbGVjdCB3aGljaCB0eXBlIG9mIGxvZ2luIHRvIGRpc3BsYXksIGlmIGFueTs+Pjs+Ozs+Oz4+Oz4+Oz4+O3Q8O2w8aTwxPjs+O2w8dDw7bDxpPDA+Oz47bDx0PDtsPGk8MT47aTwzPjtpPDU+O2k8OT47aTwxMT47aTwxMz47PjtsPHQ8cDxsPG9uY2xpY2s7aW5uZXJodG1sO3RpdGxlOz47bDx3aW5kb3cub3BlbignL3JlZ2lzdHJhdGlvbi9DcmVhdGVBY2NvdW50LmFzcHgnLCdyZWdpc3RlcicsJ3Rvb2xiYXI9bm8sbG9jYXRpb249bm8sc3RhdHVzPXllcyxtZW51YmFyPW5vLHNjcm9sbGJhcnM9eWVzLHJlc2l6YWJsZT15ZXMsd2lkdGg9NzIwLGhlaWdodD01MjAnKVw7IHJldHVybiBmYWxzZTtjcmVhdGUgDQoJCQkJCWFuIGFjY291bnQ7TGluayBvcGVucyBpbiBhIG5ldyB3aW5kb3c7Pj47Oz47dDxwPGw8b25rZXlwcmVzczs+O2w8ZmlyZURlZmF1bHRCdXR0b24oJ09ubGluZVNlcnZpY2VzTG9naW5TdGVhbHRoX1Zpc2FMb2dpbkNvbnRyb2xfbG9naW5JbWFnZUJ1dHRvbicpOz4+Ozs+O3Q8cDxsPG9ua2V5cHJlc3M7PjtsPGZpcmVEZWZhdWx0QnV0dG9uKCdPbmxpbmVTZXJ2aWNlc0xvZ2luU3RlYWx0aF9WaXNhTG9naW5Db250cm9sX2xvZ2luSW1hZ2VCdXR0b24nKTs+Pjs7Pjt0PDtsPGk8MT47PjtsPHQ8cDxsPFRleHQ7VmlzaWJsZTs+O2w8O288Zj47Pj47Oz47Pj47dDxwPGw8b25jbGljaztpbm5lcmh0bWw7dGl0bGU7PjtsPHdpbmRvdy5vcGVuKCcvUmVnaXN0cmF0aW9uL0ZvcmdvdFBhc3N3b3JkU3RlcDEuYXNweCcsJ1JlZ2lzdHJhdGlvbicsJ3Rvb2xiYXI9bm8sbG9jYXRpb249bm8sc3RhdHVzPXllcyxtZW51YmFyPW5vLHNjcm9sbGJhcnM9eWVzLHJlc2l6YWJsZT15ZXMsd2lkdGg9NjAwLGhlaWdodD00MzQnKVw7IHJldHVybiBmYWxzZTtGb3Jnb3R0ZW4gcGFzc3dvcmQ/O0xpbmsgb3BlbnMgaW4gYSBuZXcgd2luZG93Oz4+Ozs+O3Q8cDxsPG9uY2xpY2s7aW5uZXJodG1sO3RpdGxlOz47bDx3aW5kb3cub3BlbignaHR0cDovL2Zvcm1zaGVscC5pbW1pZ3JhdGlvbi5nb3Z0Lm56L1NraWxsZWRNaWdyYW50L0V4cHJlc3Npb25PZkludGVyZXN0L0NBTG9naW4uaHRtJywnbmV3cG9wdXAnLCd0b29sYmFyPW5vLGxvY2F0aW9uPW5vLHN0YXR1cz15ZXMsbWVudWJhcj1ubyxzY3JvbGxiYXJzPXllcyxyZXNpemFibGU9eWVzLHdpZHRoPTYwMCxoZWlnaHQ9NDM0JylcOyByZXR1cm4gZmFsc2U7Q2FuJ3QgbG9naW4/O0xpbmsgb3BlbnMgaW4gYSBuZXcgd2luZG93Oz4+Ozs+Oz4+Oz4+Oz4+O3Q8cDxsPFZpc2libGU7PjtsPG88Zj47Pj47Oz47dDw7bDxpPDA+Oz47bDx0PHA8bDxWaXNpYmxlOz47bDxvPGY+Oz4+Ozs+Oz4+O3Q8O2w8aTwwPjtpPDI+Oz47bDx0PHA8cDxsPFZpc2libGU7PjtsPG88dD47Pj47PjtsPGk8MD47aTwyPjtpPDQ+O2k8Nj47aTw4PjtpPDEwPjtpPDEyPjtpPDE0PjtpPDE2PjtpPDE4PjtpPDIyPjtpPDI0PjtpPDI2PjtpPDI4Pjs+O2w8dDw7bDxpPDA+Oz47bDx0PHA8bDxWaXNpYmxlOz47bDxvPHQ+Oz4+O2w8aTwxPjs+O2w8dDxwPGw8VGV4dDs+O2w8MDQgSnVsIDIwMTQ7Pj47Oz47Pj47Pj47dDw7bDxpPDE+Oz47bDx0PHA8bDxWaXNpYmxlOz47bDxvPGY+Oz4+Ozs+Oz4+O3Q8O2w8aTwwPjtpPDI+Oz47bDx0PHA8bDxWaXNpYmxlOz47bDxvPGY+Oz4+Ozs+O3Q8cDxsPFZpc2libGU7PjtsPG88Zj47Pj47Oz47Pj47dDw7bDxpPDA+Oz47bDx0PHA8bDxWaXNpYmxlOz47bDxvPGY+Oz4+Ozs+Oz4+O3Q8O2w8aTwwPjs+O2w8dDxwPGw8VmlzaWJsZTs+O2w8bzxmPjs+PjtsPGk8MT47PjtsPHQ8O2w8aTwwPjs+O2w8dDxwPGw8VmlzaWJsZTs+O2w8bzxmPjs+Pjs7Pjs+Pjs+Pjs+Pjt0PHA8cDxsPFZpc2libGU7PjtsPG88Zj47Pj47PjtsPGk8MT47PjtsPHQ8O2w8aTw5Pjs+O2w8dDxwPHA8bDxFbmFibGVkO1Zpc2libGU7PjtsPG88Zj47bzxmPjs+Pjs+Ozs+Oz4+Oz4+O3Q8cDxwPGw8VmlzaWJsZTs+O2w8bzxmPjs+Pjs+O2w8aTwwPjs+O2w8dDxwPGw8VmlzaWJsZTs+O2w8bzxmPjs+PjtsPGk8Nz47PjtsPHQ8QDA8Ozs7Ozs7Ozs7Oz47Oz47Pj47Pj47dDxwPHA8bDxWaXNpYmxlOz47bDxvPGY+Oz4+Oz47bDxpPDE+O2k8Mz47aTw1Pjs+O2w8dDw7bDxpPDE+O2k8Mz47PjtsPHQ8cDxsPFRleHQ7PjtsPFw8bGFiZWwgZm9yPSJGb290ZXJfbmV3Rm9vdGVyX1ZpZXdJbmZvRm9yTGlua3NfVmlld0luZm9Gb3JMaW5rc0xpc3QiXD47Pj47Oz47dDxwPGw8VGV4dDs+O2w8XDwvbGFiZWxcPjs+Pjs7Pjs+Pjt0PHA8bDxWaXNpYmxlOz47bDxvPGY+Oz4+Ozs+O3Q8dDxwPHA8bDxEYXRhVGV4dEZpZWxkO0RhdGFWYWx1ZUZpZWxkOz47bDxrZXk7dmFsdWU7Pj47Pjt0PGk8Mj47QDxcZTtcZTs+O0A8XGU7XGU7Pj47Pjs7Pjs+Pjt0PHA8cDxsPFZpc2libGU7PjtsPG88Zj47Pj47PjtsPGk8MT47aTwzPjtpPDU+Oz47bDx0PDtsPGk8MT47PjtsPHQ8cDxsPFRleHQ7PjtsPFw8bGFiZWwgZm9yPSJGb290ZXJfbmV3Rm9vdGVyX0lXYW50VG9MaW5rc19JV2FudFRvTGlua3NMaXN0Ilw+Oz4+Ozs+Oz4+O3Q8cDxsPFZpc2libGU7PjtsPG88Zj47Pj47bDxpPDA+Oz47bDx0PHA8bDxUZXh0Oz47bDxcPGxhYmVsIGZvcj0iRm9vdGVyX25ld0Zvb3Rlcl9JV2FudFRvTGlua3NfSVdhbnRUb0xpbmtzTGlzdCJcPjs+Pjs7Pjs+Pjt0PHQ8cDxwPGw8RGF0YVRleHRGaWVsZDtEYXRhVmFsdWVGaWVsZDs+O2w8a2V5O3ZhbHVlOz4+Oz47dDxpPDI+O0A8XGU7XGU7PjtAPFxlO1xlOz4+Oz47Oz47Pj47dDw7bDxpPDI+Oz47bDx0PHA8bDxWaXNpYmxlOz47bDxvPGY+Oz4+O2w8aTwzPjs+O2w8dDxwPGw8VmlzaWJsZTs+O2w8bzxmPjs+Pjs7Pjs+Pjs+Pjt0PDtsPGk8MD47PjtsPHQ8cDxsPFZpc2libGU7PjtsPG88Zj47Pj47Oz47Pj47dDw7bDxpPDA+Oz47bDx0PHA8bDxWaXNpYmxlOz47bDxvPGY+Oz4+Ozs+Oz4+O3Q8cDxsPFZpc2libGU7PjtsPG88Zj47Pj47Oz47dDw7bDxpPDA+O2k8Mj47aTw0PjtpPDY+Oz47bDx0PDtsPGk8MT47aTwzPjtpPDU+Oz47bDx0PHA8bDxUZXh0Oz47bDxcPEEgaHJlZj0iaHR0cDovL3d3dy5uZXd6ZWFsYW5kLmdvdnQubnovIlw+XDxJTUcgYm9yZGVyPSIwIiBhbHQ9IkdvIHRvIHd3dy5uZXd6ZWFsYW5kLmdvdnQubnouICIgc3JjPSIvTlIvcmRvbmx5cmVzL0Y3QTIxNjQ0LTQ1QjMtNEMzNS1BNTFGLTI5RTdERDY4N0U5Ny8wL256Z292dGxvZ28ucG5nIlw+XDwvQVw+Jm5ic3BcOyZuYnNwXDs7Pj47Oz47dDxwPGw8VGV4dDs+O2w8XGU7Pj47Oz47dDxwPGw8VGV4dDs+O2w8XGU7Pj47Oz47Pj47dDw7bDxpPDA+Oz47bDx0PHA8bDxWaXNpYmxlOz47bDxvPGY+Oz4+O2w8aTwwPjtpPDE+O2k8Mj47PjtsPHQ8O2w8aTwwPjs+O2w8dDxwPGw8VmlzaWJsZTs+O2w8bzxmPjs+Pjs7Pjs+Pjt0PDtsPGk8MD47PjtsPHQ8cDxsPFZpc2libGU7PjtsPG88Zj47Pj47Oz47Pj47dDw7bDxpPDA+Oz47bDx0PHA8bDxWaXNpYmxlOz47bDxvPGY+Oz4+Ozs+Oz4+Oz4+Oz4+O3Q8O2w8aTwwPjs+O2w8dDxwPGw8VmlzaWJsZTs+O2w8bzxmPjs+Pjs7Pjs+Pjt0PDtsPGk8MD47PjtsPHQ8cDxwPGw8VmlzaWJsZTs+O2w8bzxmPjs+Pjs+Ozs+Oz4+Oz4+Oz4+O3Q8O2w8aTwxPjtpPDM+O2k8Nz47aTw5PjtpPDExPjtpPDEzPjtpPDE1PjtpPDE3PjtpPDE5PjtpPDIxPjtpPDIzPjtpPDI5Pjs+O2w8dDxwPGw8VmlzaWJsZTs+O2w8bzxmPjs+Pjs7Pjt0PDtsPGk8MD47PjtsPHQ8cDxsPFZpc2libGU7PjtsPG88dD47Pj47bDxpPDE+Oz47bDx0PHA8bDxUZXh0Oz47bDwwNCBKdWwgMjAxNDs+Pjs7Pjs+Pjs+Pjt0PHA8bDxWaXNpYmxlOz47bDxvPGY+Oz4+O2w8aTwxPjs+O2w8dDw7bDxpPDE+O2k8Mz47aTw1Pjs+O2w8dDw7bDxpPDE+Oz47bDx0PHA8bDxUZXh0Oz47bDxcPGxhYmVsIGZvcj0iRm9vdGVyX01pZ3JhbnRTaXRlU3RlYWx0aF9JV2FudFRvTGlua3NfSVdhbnRUb0xpbmtzTGlzdCJcPjs+Pjs7Pjs+Pjt0PHA8bDxWaXNpYmxlOz47bDxvPGY+Oz4+O2w8aTwwPjs+O2w8dDxwPGw8VGV4dDs+O2w8XDxsYWJlbCBmb3I9IkZvb3Rlcl9NaWdyYW50U2l0ZVN0ZWFsdGhfSVdhbnRUb0xpbmtzX0lXYW50VG9MaW5rc0xpc3QiXD47Pj47Oz47Pj47dDx0PHA8cDxsPERhdGFUZXh0RmllbGQ7RGF0YVZhbHVlRmllbGQ7PjtsPGtleTt2YWx1ZTs+Pjs+O3Q8aTwyPjtAPFxlO1xlOz47QDxcZTtcZTs+Pjs+Ozs+Oz4+Oz4+O3Q8cDxsPFZpc2libGU7PjtsPG88Zj47Pj47bDxpPDE+Oz47bDx0PDtsPGk8MT47aTwzPjtpPDU+Oz47bDx0PDtsPGk8MT47aTwzPjs+O2w8dDxwPGw8VGV4dDs+O2w8XDxsYWJlbCBmb3I9IkZvb3Rlcl9Db21tdW5pdHlTaXRlU3RlYWx0aF9WaWV3SW5mb0ZvckxpbmtzX1ZpZXdJbmZvRm9yTGlua3NMaXN0Ilw+Oz4+Ozs+O3Q8cDxsPFRleHQ7PjtsPFw8L2xhYmVsXD47Pj47Oz47Pj47dDxwPGw8VmlzaWJsZTs+O2w8bzxmPjs+Pjs7Pjt0PHQ8cDxwPGw8RGF0YVRleHRGaWVsZDtEYXRhVmFsdWVGaWVsZDs+O2w8a2V5O3ZhbHVlOz4+Oz47dDxpPDI+O0A8XGU7XGU7PjtAPFxlO1xlOz4+Oz47Oz47Pj47Pj47dDw7bDxpPDE+Oz47bDx0PHA8cDxsPFZpc2libGU7PjtsPG88Zj47Pj47PjtsPGk8NT47aTw3PjtpPDk+Oz47bDx0PDtsPGk8MT47PjtsPHQ8O2w8aTwxPjs+O2w8dDw7bDxpPDA+Oz47bDx0PDtsPGk8MT47PjtsPHQ8QDA8Ozs7Ozs7Ozs7Oz47Oz47Pj47Pj47Pj47Pj47dDw7bDxpPDE+Oz47bDx0PDtsPGk8MT47PjtsPHQ8O2w8aTwwPjs+O2w8dDw7bDxpPDE+Oz47bDx0PEAwPDs7Ozs7Ozs7Ozs+Ozs+Oz4+Oz4+Oz4+Oz4+O3Q8O2w8aTwxPjs+O2w8dDw7bDxpPDE+Oz47bDx0PDtsPGk8MD47PjtsPHQ8O2w8aTwxPjs+O2w8dDxAMDw7Ozs7Ozs7Ozs7Pjs7Pjs+Pjs+Pjs+Pjs+Pjs+Pjs+Pjt0PHA8cDxsPFZpc2libGU7PjtsPG88Zj47Pj47PjtsPGk8MT47PjtsPHQ8O2w8aTwxPjtpPDM+Oz47bDx0PHA8bDxocmVmO3RpdGxlO29uY2xpY2s7PjtsPGh0dHA6Ly9mb3Jtc2hlbHAuaW1taWdyYXRpb24uZ292dC5uei9vbmxpbmVzZXJ2aWNlcy9oZWxwL29ubGluZXNlcnZpY2VzLmh0bTtPcGVucyBhIG5ldyB3aW5kb3c7d2luZG93Lm9wZW4oJ2h0dHA6Ly9mb3Jtc2hlbHAuaW1taWdyYXRpb24uZ292dC5uei9vbmxpbmVzZXJ2aWNlcy9oZWxwL29ubGluZXNlcnZpY2VzLmh0bScsJycsJ3Rvb2xiYXI9bm8sbG9jYXRpb249bm8sc3RhdHVzPW5vLG1lbnViYXI9bm8sc2Nyb2xsYmFycz15ZXMscmVzaXphYmxlPXllcyx3aWR0aD03NTAsaGVpZ2h0PTQ1MCcpXDsgcmV0dXJuIGZhbHNlOz4+Ozs+O3Q8cDxwPGw8VmlzaWJsZTs+O2w8bzxmPjs+Pjs+O2w8aTwxPjtpPDM+O2k8NT47PjtsPHQ8cDxsPHRpdGxlO2hyZWY7b25jbGljaztpbm5lcmh0bWw7PjtsPExpbmsgb3BlbnMgaW4gYSBuZXcgd2luZG93Oy9jc3NtL1VzZXJzL0hvbWUuYXNweDt3aW5kb3cub3BlbignL2Nzc20vVXNlcnMvSG9tZS5hc3B4JywnQ1NTTScsJ3Rvb2xiYXI9bm8sbG9jYXRpb249bm8sc3RhdHVzPXllcyxtZW51YmFyPW5vLHNjcm9sbGJhcnM9eWVzLHJlc2l6YWJsZT15ZXMsd2lkdGg9ODQwLGhlaWdodD01MjAnKVw7IHJldHVybiBmYWxzZTtFeHByZXNzaW9uIG9mIEludGVyZXN0Oz4+Ozs+O3Q8cDxsPGhyZWY7dGl0bGU7b25jbGljaztpbm5lcmh0bWw7PjtsPC9TaWx2ZXJGZXJuL0hvbWUvSW5kZXg7TGluayBvcGVucyBpbiBhIG5ldyB3aW5kb3c7d2luZG93Lm9wZW4oJy9TaWx2ZXJGZXJuL0hvbWUvSW5kZXgnLCdTaWx2ZXJGZXJuJywndG9vbGJhcj1ubyxsb2NhdGlvbj1ubyxzdGF0dXM9eWVzLG1lbnViYXI9bm8sc2Nyb2xsYmFycz15ZXMscmVzaXphYmxlPXllcyx3aWR0aD04NDAsaGVpZ2h0PTUyMCcpXDsgcmV0dXJuIGZhbHNlO1NpbHZlciBGZXJuIEpvYiBTZWFyY2g7Pj47Oz47dDxwPGw8dGl0bGU7aHJlZjtvbmNsaWNrO2lubmVyaHRtbDs+O2w8TGluayBvcGVucyBpbiBhIG5ldyB3aW5kb3c7L1dvcmtpbmdIb2xpZGF5Lzt3aW5kb3cub3BlbignL1dvcmtpbmdIb2xpZGF5LycsJ1dIUycsJ3Rvb2xiYXI9bm8sbG9jYXRpb249bm8sc3RhdHVzPXllcyxtZW51YmFyPW5vLHNjcm9sbGJhcnM9eWVzLHJlc2l6YWJsZT15ZXMsd2lkdGg9ODQwLGhlaWdodD01MjAnKVw7IHJldHVybiBmYWxzZTtXb3JraW5nIEhvbGlkYXk7Pj47Oz47Pj47Pj47Pj47dDxwPHA8bDxWaXNpYmxlOz47bDxvPGY+Oz4+Oz47bDxpPDE+Oz47bDx0PDtsPGk8MT47PjtsPHQ8cDxsPHRpdGxlO29uY2xpY2s7aW5uZXJodG1sOz47bDxMaW5rIG9wZW5zIGluIGEgbmV3IHdpbmRvdzt3aW5kb3cub3BlbignaHR0cDovL2Zvcm1zaGVscC5pbW1pZ3JhdGlvbi5nb3Z0Lm56L29ubGluZXNlcnZpY2VzL2hlbHAvbWVzc2FnZXMuaHRtJywnbmV3cG9wdXAnLCd0b29sYmFyPW5vLGxvY2F0aW9uPW5vLHN0YXR1cz15ZXMsbWVudWJhcj1ubyxzY3JvbGxiYXJzPXllcyxyZXNpemFibGU9eWVzLHdpZHRoPTYwMCxoZWlnaHQ9NDM0JylcOyByZXR1cm4gZmFsc2U7XGU7Pj47Oz47Pj47Pj47dDw7bDxpPDI+Oz47bDx0PHA8bDxWaXNpYmxlOz47bDxvPGY+Oz4+O2w8aTwzPjs+O2w8dDxwPGw8VmlzaWJsZTs+O2w8bzxmPjs+Pjs7Pjs+Pjs+Pjt0PDtsPGk8MD47PjtsPHQ8cDxsPFZpc2libGU7PjtsPG88dD47Pj47Oz47Pj47dDxwPHA8bDxWaXNpYmxlOz47bDxvPGY+Oz4+Oz47bDxpPDE+Oz47bDx0PDtsPGk8MT47PjtsPHQ8cDxwPGw8VGV4dDs+O2w8MDEgSmFuIDAwMDEsIDAwOjAwOz4+Oz47Oz47Pj47Pj47dDw7bDxpPDE+O2k8Mz47aTw1Pjs+O2w8dDxwPGw8VGV4dDs+O2w8XDxBIGhyZWY9Imh0dHA6Ly93d3cubmV3emVhbGFuZC5nb3Z0Lm56LyJcPlw8SU1HIGJvcmRlcj0iMCIgYWx0PSJHbyB0byB3d3cubmV3emVhbGFuZC5nb3Z0Lm56LiAiIHNyYz0iL05SL3Jkb25seXJlcy9GN0EyMTY0NC00NUIzLTRDMzUtQTUxRi0yOUU3REQ2ODdFOTcvMC9uemdvdnRsb2dvLnBuZyJcPlw8L0FcPiZuYnNwXDsmbmJzcFw7Oz4+Ozs+O3Q8cDxsPFRleHQ7PjtsPFxlOz4+Ozs+O3Q8cDxsPFRleHQ7PjtsPFxlOz4+Ozs+Oz4+O3Q8O2w8aTwwPjs+O2w8dDxwPHA8bDxWaXNpYmxlOz47bDxvPGY+Oz4+Oz47Oz47Pj47Pj47Pj47Pj47Pj47bDxIZWFkZXJDb21tdW5pdHlIb21lcGFnZTpTZWFyY2hDb250cm9sOmJ0bkdvO09ubGluZVNlcnZpY2VzTG9naW5TdGVhbHRoOlZpc2FMb2dpbkNvbnRyb2w6bG9naW5JbWFnZUJ1dHRvbjs+Pj0AvwD8KvhDCG9+lrMjTdEZdaRc");
            job.SetParameter("__VIEWSTATEGENERATOR", (currentJob.RetrievedData["viewstateGenerator"]));
            job.SetParameter("OnlineServicesLoginStealth:VisaLoginControl:userNameTextBox", appCase.Username);
            job.SetParameter("OnlineServicesLoginStealth:VisaLoginControl:passwordTextBox", appCase.Password);
            job.SetParameter("OnlineServicesLoginStealth:VisaLoginControl:loginImageButton.x", "57");
            job.SetParameter("OnlineServicesLoginStealth:VisaLoginControl:loginImageButton.y", "10");

            //success match
            job.AddMatcher(new WebContentMatcher(JobResults.Succeed, 
                                            new string[]{
                                                @"LastLoggedInTime_LastLoggedInTimeLabel",
                                                @"<span class=""username"" >.*- <\/span>"}));

            return job;
        }

        //card type selected
        public static WebRequestJob CreateCardTypeJob(ApplicationCase appCase, WebRequestJob currentJob)
        {
            var job = new WebRequestJob(CommonJobNames.CardType, appCase.AppId)
            {
                Method = RestSharp.Method.POST,
                JobType = JobTypes.Normal
            };

            //url
            job.BaseUrl = @"https://onlineservices.immigration.govt.nz/";
            job.Resource = string.Format(@"PaymentGateway/OnLinePayment.aspx?SourceUrl=https://onlineservices.immigration.govt.nz/WorkingHoliday/Application/SubmitConfirmation.aspx?ApplicationId={0}&ApplicationId={0}&ProductId=2", appCase.AppId);
            job.Parameters.Add("__EVENTTARGET", "");
            job.Parameters.Add("__EVENTARGUMENT", "");
            job.Parameters.Add("__VIEWSTATE", "/wEPDwULLTEyMDY4Njc4MDkPZBYCZg9kFgICAw9kFgICAw9kFgICBw8WCB4FY2xhc3MFCXNpdGV0YWJsZR4LY2VsbHNwYWNpbmcFATAeBmJvcmRlcgUBMB4LY2VsbHBhZGRpbmcFATIWAgIBDxYCHgZ2YWxpZ24FA3RvcBYCAgIPZBYCAgEPDxYCHglNYXhMZW5ndGgCZGRkGAEFHl9fQ29udHJvbHNSZXF1aXJlUG9zdEJhY2tLZXlfXxYCBSdjdGwwMCRDb250ZW50UGxhY2VIb2xkZXIxJG9rSW1hZ2VCdXR0b24FK2N0bDAwJENvbnRlbnRQbGFjZUhvbGRlcjEkY2FuY2VsSW1hZ2VCdXR0b27cKn7Ve9p8wZAjvx+vD5veCXC0nw==");
            job.Parameters.Add("__VIEWSTATEGENERATOR", "64EC2CD5");
            job.Parameters.Add("__EVENTVALIDATION", "/wEWBAKbnrrRDQLsqKiNDgLAhozUDwL5l44EvLqaWXWqwKlxNHR20Ez1IFDyuaU=");
            job.Parameters.Add("ctl00$ContentPlaceHolder1$payorNameTextBox", appCase.Card.Holder);
            job.Parameters.Add("ctl00$ContentPlaceHolder1$okImageButton.x", "40");
            job.Parameters.Add("ctl00$ContentPlaceHolder1$okImageButton.y", "8");

            //
            job.AddRetriever(new WebContentRetriever(@"<form.*payment_type_selection.*action=""(?<form_action>.+?)"">"));
            job.AddRetriever(new WebContentRetriever(@"<input.*name=""hk"".*value=""(?<hk>.+)"">"));

            //
            job.AddMatcher(new WebContentMatcher(JobResults.Succeed, new string[] { @"1: Select your preferred payment method" }));

            return job;
        }

        //card detail
        public static WebRequestJob CreateCardDetailJob(ApplicationCase appCase, WebRequestJob currentJob)
        {
            var job = new WebRequestJob(CommonJobNames.CardDetail, appCase.AppId)
            {
                Method = RestSharp.Method.POST,
                JobType = JobTypes.Normal
            };

            //url
            job.BaseUrl = @"https://webcomm.paymark.co.nz/";
            job.Resource = string.Format(@"hosted/{0}", currentJob.JobResultData["form_action"]);
            job.Parameters.Add("hk", currentJob.JobResultData["hk"]);
            job.Parameters.Add("hosted_responsive_format", "N");
            job.Parameters.Add(string.Format("card_type_{0}.x", appCase.Card.CardTypeName), "44"); //card_type_VISA.x:"36"
            job.Parameters.Add(string.Format("card_type_{0}.y", appCase.Card.CardTypeName), "18"); //card_type_VISA.y:"36"
            job.Parameters.Add("processingStage", "card_entry");
            job.Parameters.Add("future_pay", "");
            job.Parameters.Add("future_pay_save_only", "");

            //
            job.AddRetriever(new WebContentRetriever(@"<form action=""(?<form_action>.+?)"".*credit_card_entry"" >"));
            job.AddRetriever(new WebContentRetriever(@"<input.*name=""hk"".*value=""(?<hk>.+)"">"));

            //matcher
            job.AddMatcher(new WebContentMatcher(JobResults.Succeed, new string[] { @"2: Enter Your Card Details" }));

            return job;
        }

        //pay
        public static WebRequestJob CreatePayNowJob(ApplicationCase appCase, WebRequestJob currentJob)
        {
            var job = new WebRequestJob(CommonJobNames.Pay, appCase.AppId)
            {
                Method = RestSharp.Method.POST,
                JobType = JobTypes.Normal
            };

            //url
            job.BaseUrl = @"https://webcomm.paymark.co.nz/";
            job.Resource = string.Format(@"hosted/{0}", currentJob.JobResultData["form_action"]);

            //
            job.Parameters.Add("hk", currentJob.JobResultData["hk"]);

            job.Parameters.Add("use_card_security_code", "Y");
            job.Parameters.Add("enforce_card_security_code", "N");
            job.Parameters.Add("enforce_card_security_code_3party", "Y");
            job.Parameters.Add("enforce_card_security_code_futurepay", "Y");
            job.Parameters.Add("future_pay", "N");
            job.Parameters.Add("future_pay_save_only", "");
            job.Parameters.Add("hosted_responsive_format", "N");

            var card = appCase.Card;
            job.Parameters.Add("cardtype", card.CardTypeName);
            job.Parameters.Add("cardnumber", card.CardNumber);
            job.Parameters.Add("cardverificationcode", card.SecureCode);
            job.Parameters.Add("expirymonth", card.ExpiryMonth);
            job.Parameters.Add("expiryyear", card.ExpiryYear);
            job.Parameters.Add("cardholder", card.Holder);
            job.Parameters.Add("pay_now", "Pay+Now");

            //
            job.AddMatcher(new WebContentMatcher(JobResults.Succeed, new string[] { @"Your payment was successful" }));
            job.AddMatcher(new WebContentMatcher(JobResults.Failed, new string[] { @"Your payment was not successful" }));

            return job;
        }

        private static Dictionary<string, string> ParseDataFromString(string str)
        {
            var result = new Dictionary<string, string>();
            dynamic paris = JsonConvert.DeserializeObject(str);
            foreach (var pair in paris)
            {
                string key = Convert.ToString(pair.Name.Value);
                if (key.EndsWith("_ct") || key.EndsWith("_rf"))
                {
                    result.Add(key, "0");
                }
                else
                {
                    result.Add(key, pair.Value.Value);
                }
            }

            return result;
        }
    }
}
