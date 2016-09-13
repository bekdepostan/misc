using ASW.Framework.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASW.Framework.Simple
{
    public static class WhvJobNames
    {
        public const string GetStatus = "WHV_GET_STATUS";
        public const string Submission = "WHV_SUBMISSION";
    }

    public static class WhvJobs
    {
        public static WebRequestJob CreateGetStatusJob(ApplicationCase appCase, WebRequestJob currentJob)
        {
            
            var job = new WebRequestJob(WhvJobNames.GetStatus, appCase.AppId)
            {
                Method = RestSharp.Method.GET,
                JobType = JobTypes.Normal
            };

            //usl
            job.BaseUrl = @"https://onlineservices.immigration.govt.nz/";
            job.Resource = @"WorkingHoliday";

            //retrieving
            job.AddRetriever(new WebContentRetriever(@"<span.*referenceNumber.*>(?<refNum>\d+)<\/span>"));
            job.AddRetriever(new WebContentRetriever(@"<span.*dateCreated.*>(?<dateCreated>.*)<\/span>"));
            job.AddRetriever(new WebContentRetriever(@"<span.*status.*>(?<app_status>.*)<\/span>"));
            job.AddRetriever(new WebContentRetriever(@"<span.*paymentStatus.*>(?<pay_status>.*)<\/span>"));

            //
            job.AddMatcher(new WebContentMatcher(JobResults.Succeed, new string[]{
                                                @"<p>Welcome to the Working Holiday Schemes Online homepage.</p>"}));

            return job;
        }

        public static WebRequestJob CreateSubmissionJob(ApplicationCase appCase, WebRequestJob currentJob)
        {
            var job = new WebRequestJob(WhvJobNames.Submission, appCase.AppId)
            {
                Method = RestSharp.Method.POST,
                JobType = JobTypes.Normal
            };

            //url
            job.BaseUrl = @"https://onlineservices.immigration.govt.nz/";
            job.Resource = string.Format(@"WorkingHoliday/Application/Submit.aspx?ApplicationId={0}", appCase.AppId);

            job.Parameters.Add("__VIEWSTATE", "/wEPDwUKMTU5Mjg5NDA1Mg9kFgJmD2QWAgIDD2QWAgIDD2QWBAIDD2QWCgIBDxYIHgRocmVmBURodHRwOi8vZ2xvc3NhcnkuaW1taWdyYXRpb24uZ292dC5uei9GYWxzZW9ybWlzbGVhZGluZ2luZm9ybWF0aW9uLmh0bR4Hb25jbGljawXNAXdpbmRvdy5vcGVuKCdodHRwOi8vZ2xvc3NhcnkuaW1taWdyYXRpb24uZ292dC5uei9GYWxzZW9ybWlzbGVhZGluZ2luZm9ybWF0aW9uLmh0bScsJ25ld3BvcHVwJywndG9vbGJhcj1ubyxsb2NhdGlvbj1ubyxzdGF0dXM9eWVzLG1lbnViYXI9bm8sc2Nyb2xsYmFycz15ZXMscmVzaXphYmxlPXllcyx3aWR0aD02MDAsaGVpZ2h0PTQzNCcpOyByZXR1cm4gZmFsc2UeCWlubmVyaHRtbAUfZmFsc2Ugb3IgbWlzbGVhZGluZyBpbmZvcm1hdGlvbh4FdGl0bGUFGkxpbmsgb3BlbnMgaW4gYSBuZXcgd2luZG93ZAIDDxYIHwAFNWh0dHA6Ly9nbG9zc2FyeS5pbW1pZ3JhdGlvbi5nb3Z0Lm56L1Zpc2FhbmRQZXJtaXQuaHRtHwEFvgF3aW5kb3cub3BlbignaHR0cDovL2dsb3NzYXJ5LmltbWlncmF0aW9uLmdvdnQubnovVmlzYWFuZFBlcm1pdC5odG0nLCduZXdwb3B1cCcsJ3Rvb2xiYXI9bm8sbG9jYXRpb249bm8sc3RhdHVzPXllcyxtZW51YmFyPW5vLHNjcm9sbGJhcnM9eWVzLHJlc2l6YWJsZT15ZXMsd2lkdGg9NjAwLGhlaWdodD00MzQnKTsgcmV0dXJuIGZhbHNlHwIFBHZpc2EfAwUaTGluayBvcGVucyBpbiBhIG5ldyB3aW5kb3dkAgsPFggfAAU1aHR0cDovL2dsb3NzYXJ5LmltbWlncmF0aW9uLmdvdnQubnovR29vZGNoYXJhY3Rlci5odG0fAQW+AXdpbmRvdy5vcGVuKCdodHRwOi8vZ2xvc3NhcnkuaW1taWdyYXRpb24uZ292dC5uei9Hb29kY2hhcmFjdGVyLmh0bScsJ25ld3BvcHVwJywndG9vbGJhcj1ubyxsb2NhdGlvbj1ubyxzdGF0dXM9eWVzLG1lbnViYXI9bm8sc2Nyb2xsYmFycz15ZXMscmVzaXphYmxlPXllcyx3aWR0aD02MDAsaGVpZ2h0PTQzNCcpOyByZXR1cm4gZmFsc2UfAgUaZ29vZCANCgkJCQkJCQkJCQljaGFyYWN0ZXIfAwUaTGluayBvcGVucyBpbiBhIG5ldyB3aW5kb3dkAhUPFggfAAVAaHR0cDovL2dsb3NzYXJ5LmltbWlncmF0aW9uLmdvdnQubnovT2NjdXBhdGlvbmFscmVnaXN0cmF0aW9uLmh0bR8BBckBd2luZG93Lm9wZW4oJ2h0dHA6Ly9nbG9zc2FyeS5pbW1pZ3JhdGlvbi5nb3Z0Lm56L09jY3VwYXRpb25hbHJlZ2lzdHJhdGlvbi5odG0nLCduZXdwb3B1cCcsJ3Rvb2xiYXI9bm8sbG9jYXRpb249bm8sc3RhdHVzPXllcyxtZW51YmFyPW5vLHNjcm9sbGJhcnM9eWVzLHJlc2l6YWJsZT15ZXMsd2lkdGg9NjAwLGhlaWdodD00MzQnKTsgcmV0dXJuIGZhbHNlHwIFDHJlZ2lzdHJhdGlvbh8DBRpMaW5rIG9wZW5zIGluIGEgbmV3IHdpbmRvd2QCIQ9kFgICAQ8WCB8ABRZodHRwOi8vd3d3LmlhYS5nb3Z0Lm56HwEFnwF3aW5kb3cub3BlbignaHR0cDovL3d3dy5pYWEuZ292dC5ueicsJ25ld3BvcHVwJywndG9vbGJhcj1ubyxsb2NhdGlvbj1ubyxzdGF0dXM9eWVzLG1lbnViYXI9bm8sc2Nyb2xsYmFycz15ZXMscmVzaXphYmxlPXllcyx3aWR0aD02MDAsaGVpZ2h0PTQzNCcpOyByZXR1cm4gZmFsc2UfAgUPd3d3LmlhYS5nb3Z0Lm56HwMFGkxpbmsgb3BlbnMgaW4gYSBuZXcgd2luZG93ZAIFDw8WAh4HVmlzaWJsZWhkFgYCAQ8WAh8ABSx+L0FwcGxpY2F0aW9uL1BheS5hc3B4P0FwcGxpY2F0aW9uSWQ9MTE3MTEzM2QCAw8WAh8ABRwvV29ya2luZ0hvbGlkYXkvZGVmYXVsdC5hc3B4ZAIFDw8WAh8EaGRkGAEFHl9fQ29udHJvbHNSZXF1aXJlUG9zdEJhY2tLZXlfXxYNBTBjdGwwMCRDb250ZW50UGxhY2VIb2xkZXIxJGZhbHNlU3RhdGVtZW50Q2hlY2tCb3gFJ2N0bDAwJENvbnRlbnRQbGFjZUhvbGRlcjEkbm90ZXNDaGVja0JveAUvY3RsMDAkQ29udGVudFBsYWNlSG9sZGVyMSRjaXJjdW1zdGFuY2VzQ2hlY2tCb3gFKmN0bDAwJENvbnRlbnRQbGFjZUhvbGRlcjEkd2FycmFudHNDaGVja0JveAUtY3RsMDAkQ29udGVudFBsYWNlSG9sZGVyMSRpbmZvcm1hdGlvbkNoZWNrQm94BShjdGwwMCRDb250ZW50UGxhY2VIb2xkZXIxJGhlYWx0aENoZWNrQm94BShjdGwwMCRDb250ZW50UGxhY2VIb2xkZXIxJGFkdmljZUNoZWNrQm94BS5jdGwwMCRDb250ZW50UGxhY2VIb2xkZXIxJHJlZ2lzdHJhdGlvbkNoZWNrQm94BS1jdGwwMCRDb250ZW50UGxhY2VIb2xkZXIxJGVudGl0bGVtZW50Q2hlY2tib3gFLmN0bDAwJENvbnRlbnRQbGFjZUhvbGRlcjEkcGVybWl0RXhwaXJ5Q2hlY2tCb3gFMmN0bDAwJENvbnRlbnRQbGFjZUhvbGRlcjEkbWVkaWNhbEluc3VyYW5jZUNoZWNrQm94BStjdGwwMCRDb250ZW50UGxhY2VIb2xkZXIxJHN1Ym1pdEltYWdlQnV0dG9uBStjdGwwMCRDb250ZW50UGxhY2VIb2xkZXIxJGNhbmNlbEltYWdlQnV0dG9uCu3sqFrVnQmj9kUcrt6smJ6ALKk=");
            job.Parameters.Add("__VIEWSTATEGENERATOR", "8150357E");
            job.Parameters.Add("__EVENTVALIDATION", "/wEWDgLBnIteAv/x1m4CwdaLwgQCmMmxHQKHpuXxBALl3beGCgKzv5nJAwLL4JyXDwK6zeOZCgKL7YLlDQKii5i8BgKH0/XCCALByIr9DAL5l44EhtguqqZAjv6b+nhYqK6JiWDjDi8=");
            job.Parameters.Add("ctl00$ContentPlaceHolder1$falseStatementCheckBox", "on");
            job.Parameters.Add("ctl00$ContentPlaceHolder1$notesCheckBox", "on");
            job.Parameters.Add("ctl00$ContentPlaceHolder1$circumstancesCheckBox", "on");
            job.Parameters.Add("ctl00$ContentPlaceHolder1$warrantsCheckBox", "on");
            job.Parameters.Add("ctl00$ContentPlaceHolder1$informationCheckBox", "on");
            job.Parameters.Add("ctl00$ContentPlaceHolder1$healthCheckBox", "on");
            job.Parameters.Add("ctl00$ContentPlaceHolder1$adviceCheckBox", "on");
            job.Parameters.Add("ctl00$ContentPlaceHolder1$permitExpiryCheckBox", "on");
            job.Parameters.Add("ctl00$ContentPlaceHolder1$medicalInsuranceCheckBox", "on");
            job.Parameters.Add("ctl00$ContentPlaceHolder1$registrationCheckBox", "on");
            job.Parameters.Add("ctl00$ContentPlaceHolder1$entitlementCheckbox", "on");
            job.Parameters.Add("ctl00$ContentPlaceHolder1$submitImageButton.x", "49");
            job.Parameters.Add("ctl00$ContentPlaceHolder1$submitImageButton.y", "11");

            //
            job.AddMatcher(new WebContentMatcher(JobResults.RepeatNeeded,
                                    new string[]{@"Scheme unavailable because"}));

            return job;
        }

    }
}