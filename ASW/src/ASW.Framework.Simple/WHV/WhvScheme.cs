using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ASW.Framework.Core;
using System.Collections.Specialized;

namespace ASW.Framework.Simple
{
    public static class WhvScheme
    {
        private static object _locker = new object();

        public static WebRequestJob GetNextJob(ApplicationCase appCase, ASWJob currentJob, SimpleCenter center)
        {
            if (appCase == null || string.IsNullOrEmpty(appCase.Id) || currentJob == null)
            {
                return null;
            }
            var job = currentJob as WebRequestJob;
            if (job.HasResponseContent && job.ResponseContent.Contains("<noscript>Please enable JavaScript to view the page content.</noscript>"))
            {
                //need js result
                return CommonJobs.CreateJsResultJob(appCase, job);
            }
            else if (string.IsNullOrEmpty(job.Name) || job.JobResult == JobResults.None
                       || job.JobResult == JobResults.Excepted)
            {
                //login home page
                return CommonJobs.CreateLoginHomeJob(appCase);
            }
            else if (CommonJobNames.LoginHomePage.Equals(job.Name) && job.JobResult == JobResults.Succeed && job.HasResultData)
            {
                //login submit
                return CommonJobs.CreateLoginSubmitJob(appCase, job);
            }
            else if (job.JobResult == JobResults.Succeed &&
                   (CommonJobNames.LoginSubmit.Equals(job.Name)))
            {
                //login succeed
                return WhvJobs.CreateGetStatusJob(appCase, job);
            }
            else if (WhvJobNames.GetStatus.Equals(job.Name) && job.JobResult == JobResults.Succeed)
            {
                //after get status
                var appId = currentJob.JobResultData["refNum"];
                if (!string.IsNullOrEmpty(appCase.AppId) && !appCase.AppId.Equals(appId))
                {
                    //differetn appId
                    return CommonJobs.CreateLoginHomeJob(appCase);
                }

                //after get status
                appCase = UpdateApplicationCaseStatus(appCase, job, center);
                if (appCase.CanSubmit)
                {
                    return WhvJobs.CreateSubmissionJob(appCase, job);
                }
                else if (appCase.CanPay)
                {
                    return CommonJobs.CreateCardTypeJob(appCase, job);
                }
                else if (appCase.HasPaid)
                {
                    return null;
                }
                else
                {
                    return null;
                }
            }
            else if (WhvJobNames.Submission.Equals(job.Name) && job.JobResult == JobResults.Succeed)
            {
                //submitted
                //select card type
                return CommonJobs.CreateCardTypeJob(appCase, job);
            }
            else if (CommonJobNames.CardType.Equals(job.Name) && job.JobResult == JobResults.Succeed)
            {
                //card type selected
                //fill detial
                return CommonJobs.CreateCardDetailJob(appCase, job);
            }
            else if (CommonJobNames.CardDetail.Equals(job.Name) && job.JobResult == JobResults.Succeed)
            {
                //pay
                return CommonJobs.CreatePayNowJob(appCase, job);
            }
            else if (CommonJobNames.Pay.Equals(job.Name))
            {
                return WhvJobs.CreateGetStatusJob(appCase, job);
            }
            else
            {
                Logger.Error(string.Format("Not matched action:{0},{1},{2}", job.Name, job.JobResult, job.ResponseContent));
                Logger.Error(job.ResponseContent);
                Task.Delay(10000);
                //return null;
                return CommonJobs.CreateLoginHomeJob(appCase);
            }
        }

        //update case status
        private static ApplicationCase UpdateApplicationCaseStatus(ApplicationCase appCase, ASWJob currentJob, SimpleCenter center)
        {
            if (!WhvJobNames.GetStatus.Equals(currentJob.Name) || currentJob.JobResult != JobResults.Succeed)
            {
                return appCase;
            }

            //update
            var appId = currentJob.JobResultData["refNum"];
            var stateNew = currentJob.JobResultData["app_status"];
            var payStateNew = currentJob.JobResultData["pay_status"];
            if ((!appId.Equals(appCase.AppId) && !string.IsNullOrEmpty(appCase.AppId)) ||
                (stateNew.Equals(appCase.State) && payStateNew.Equals(appCase.PayState)))
            {
                //appId changed
                //or state not changed
                return appCase;
            }
            appCase.State = currentJob.JobResultData["app_status"];
            appCase.PayState = currentJob.JobResultData["pay_status"];
            if (string.IsNullOrEmpty(appCase.AppId))
            {
                appCase.AppId = appId;
            }
            if (string.IsNullOrEmpty(appCase.InitState))
            {
                appCase.InitState = appCase.State;
            }
            if (string.IsNullOrEmpty(appCase.InitPayState))
            {
                appCase.InitPayState = appCase.PayState;
            }
            appCase.UpdatedTime = DateTime.Now;

            //
            return center.UpdateApplication(appCase);
        }
    }
}
