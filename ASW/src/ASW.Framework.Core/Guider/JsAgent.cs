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

namespace ASW.Framework.Core
{
    public static partial class Guider
    {
        internal class JsAgent
        {
            private IWebDriver _web = new PhantomJSDriver();
            private object _locker = new object();
            private Stopwatch _stopWatch = new Stopwatch();
            public JsAgent()
            {
            }

            public string GetPostData(string url, TimeSpan timeout, out CookieCollection cookies)
            {
                lock (_locker)
                {
                    _stopWatch.Start();
                    cookies = null;

                    try
                    {
                        _web.Manage().Cookies.DeleteAllCookies();
                        _web.Navigate().GoToUrl(url);
                        var wait = new WebDriverWait(_web, timeout);
                        IWebElement preElement = wait.Until(d => d.FindElement(By.TagName("pre")));
                        cookies = RepackCookie(_web.Manage().Cookies.AllCookies);
                        return preElement.Text;
                    }
                    catch
                    {
                        return null;
                    }
                    finally
                    {
                        _stopWatch.Stop();
                    }
                }
            }

            private CookieCollection RepackCookie(ReadOnlyCollection<OpenQA.Selenium.Cookie> cookies)
            {
                var pack = new CookieCollection();
                foreach (var c in cookies)
                {
                    pack.Add(new System.Net.Cookie(c.Name, c.Value, c.Path, c.Domain));
                }

                return pack;
            }

            public void Quit()
            {
                lock (_locker)
                {
                    _web.Quit();
                }
            }
        }
    }
}
