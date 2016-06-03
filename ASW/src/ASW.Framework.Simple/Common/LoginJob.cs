using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Specialized;
using ASW.Framework.Core;
namespace ASW.Framework.Simple
{
    [Serializable]
    public sealed class LoginHomeJob : WebRequestJob
    {
        public LoginHomeJob(string username) : base(username, CommonJobCodes.LoginHomePage)
        {
            Url = @"https://onlineservices.immigration.govt.nz/secure/status.htm";
            Method = Methods.GET;
            JobType = WebRequestJobType.Retrieving;
            RetrievingProfile = CreateRetrievingProfile();
        }

        private WebContentRetrievingProfile CreateRetrievingProfile()
        {
            var profile = new WebContentRetrievingProfile(@"<form name=""Login"".*action=""(?<url>.*?)""");
            profile.AddExpression(@"<input.*name=""__EVENTTARGET"".*value=""(?<eventTarget>.*?)""");
            profile.AddExpression(@"<input.*name=""__EVENTARGUMENT"".*value=""(?<eventArgument>.*?)""");
            profile.AddExpression(@"<input.*name=""__VIEWSTATE"".*value=""(?<viewstate>.*?)""");
            profile.AddExpression(@"<input.*name=""__VIEWSTATEGENERATOR"".*value=""(?<viewstateGenerator>.*?)""");
            return profile;
        }
    }

    [Serializable]
    public sealed class LoginSubmitJob : WebRequestJob
    {
        public LoginSubmitJob(string username, string password, NameValueCollection preJobData) : base(username, CommonJobCodes.LoginSubmit)
        {
            //Url = homePage.Url;
            //Url = @"https://onlineservices.immigration.govt.nz/Templates/Secure/Login.aspx?NRMODE=Published&NRNODEGUID=%7b31097FC2-72ED-4420-BF02-6755811E2916%7d&NRORIGINALURL=%2fsecure%2fstatus%2ehtm&NRCACHEHINT=Guest";
            Method = Methods.POST;
            JobType = WebRequestJobType.Attampt;
            //header
            Headers.Add("Content-Type", "application/x-www-form-urlencoded");
            //data
            //Data.Add("__EVENTTARGET", homePage.EventTarget);
            //Data.Add("__EVENTARGUMENT", homePage.EventArgument);
            //Data.Add("__VIEWSTATE", homePage.ViewState);
            //Data.Add("__VIEWSTATEGENERATOR", homePage.ViewStateGenerator);
            Data.Add("OnlineServicesLoginStealth:VisaLoginControl:userNameTextBox", username);
            Data.Add("OnlineServicesLoginStealth:VisaLoginControl:passwordTextBox", password);
            //success match
            SuccessProfile = new WebContentMatchingProfile(@"LastLoggedInTime_LastLoggedInTimeLabel", ExpressionCombinationTypes.AND);
            SuccessProfile.AddExpression(@"<span class=""username"" >.*- <\/span>");
            //fail match
            FailProfile = new WebContentMatchingProfile("body");
        }
    }
}
