using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace ASW.Framework.Core
{
    public class CookieWebClient : WebClient
    {
        private CookieContainer _cookieContainer = new CookieContainer();
        public void AddCookie(Cookie cookie)
        {
            _cookieContainer.Add(cookie);
        }

        protected override WebRequest GetWebRequest(Uri address)
        {
            var request = base.GetWebRequest(address) as HttpWebRequest;
            if (request == null)
            {
                return base.GetWebRequest(address);
            }
            request.Credentials = CredentialCache.DefaultCredentials;
            request.CookieContainer = _cookieContainer;
            request.AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip;
            foreach (var h in request.Headers.AllKeys)
            {
                Console.WriteLine(string.Format("{0}:{1}", h, request.Headers[h]));
            }


            return request;
        }

        //protected override WebResponse GetWebResponse(WebRequest request)
        //{
        //    //WebResponse response = base.GetWebResponse(request);
        //    //Console.WriteLine(response.ResponseUri.AbsoluteUri);
        //    //return response;
        //}

        //POST
        public Task<string> PostAsync(string url, Dictionary<string, string> data, Dictionary<string, string> headers = null,
                                             IList<ASWCookie> cookies = null)
        {
            //string dataStr = HttpUtility.UrlEncode(BuildQueryString(data));
            SetHeader(headers);
            SetCookies(ConvertCookies(cookies));
            var respBytes = UploadData(url, "POST", GetDataBytes(data));
            return Task.FromResult(Encoding.UTF8.GetString(respBytes));
        }

        //GET
        public async Task<string> GetAsync(string url, Dictionary<string, string> data = null, Dictionary<string, string> headers = null,
                                            IList<ASWCookie> cookies = null)
        {
            SetHeader(headers);
            SetCookies(ConvertCookies(cookies));
            if (data != null && data.Count > 0)
            {
                url = url + "?" + BuildQueryString(data);
            }
            return await DownloadStringTaskAsync(new Uri(url));
        }

        private void SetHeader(Dictionary<string, string> headers)
        {
            //Headers.Clear();
            Headers.Set("User-Agent", "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/50.0.2661.102 Safari/537.36");
            Headers.Set("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8");
            Headers.Set("Accept-Language", "en-NZ,en;q=0.8,zh-CN;q=0.6,zh;q=0.4");
            Headers.Set("Content-Type", "application/x-www-form-urlencoded");
            Headers.Set("Accept-Encoding", "gzip, deflate");
            if (headers != null)
            {
                foreach (var header in headers)
                {
                    Headers.Set(header.Key, header.Value);
                }
            }
        }

        private void SetCookies(CookieCollection cookies)
        {
            if (cookies != null)
            {
                _cookieContainer.Add(cookies);
            }
        }

        private static string BuildQueryString(Dictionary<string, string> datas)
        {
            if (datas == null)
            {
                return "";
            }

            var stringBuilder = new StringBuilder();
            string prefix = "";
            foreach (var data in datas)
            {
                string str = null;
                str = data.Key + "=" + data.Value;

                stringBuilder.Append(prefix + str);
                prefix = "&";
            }

            return stringBuilder.ToString();
        }

        private static byte[] GetDataBytes(Dictionary<string, string> datas)
        {
            var str = BuildQueryString(datas);
            byte[] postArray = Encoding.UTF8.GetBytes(str);
            return postArray;
        }

        private static CookieCollection ConvertCookies(IList<ASWCookie> aswcookies)
        {
            var result = new CookieCollection();
            if (aswcookies != null)
            {
                foreach (var c in aswcookies)
                {
                    result.Add(ConvertCookie(c));
                }
            }
            return result;
        }
        private static Cookie ConvertCookie(ASWCookie aswcookie)
        {
            if (aswcookie != null)
            {
                return new Cookie(aswcookie.Name, aswcookie.Value, aswcookie.Path, aswcookie.Domain);
            }
            else
            {
                return null;
            }
        }
    }
}
