using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace PowerBallNZ
{
    public class NZCityHelper
    {

        const string REQUEST_URL = "http://home.nzcity.co.nz/lotto/lotto.aspx";
       
        public static string RetrieveLottoResultHtml(int drawNumber, ref string msg)
        {
            try {
                var request = (HttpWebRequest)WebRequest.Create(REQUEST_URL);

                var postData = string.Format("draw={0}&draw2={1}", Draw.LastDrawNumber, drawNumber);
                var data = Encoding.ASCII.GetBytes(postData);

                request.Method = "POST";
                request.ContentType = "application/x-www-form-urlencoded";
                request.ContentLength = data.Length;

                using (var stream = request.GetRequestStream())
                {
                    stream.Write(data, 0, data.Length);
                }

                var response = (HttpWebResponse)request.GetResponse();

                var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();
                return responseString;

            }catch(Exception ex)
            {
                msg = ex.Message;
                return null;
            }
        }

        public static int? GetDrawNumber(string html, ref string msg)
        {
            //<label id="ctl00_ContentPlaceHolder1_LottoDrawNumber">1534</label>
            var rgx = new Regex(@"<label id=""ctl00_ContentPlaceHolder1_LottoDrawNumber"">(?<number>\d+)</label>");
            var match = rgx.Match(html);
            if(match.Success)
            {
                return int.Parse(match.Groups["number"].Value);
            }
            else
            {
                msg = "failed to match draw number.";
                return null;
            }
        }

        public static DateTime? GetDrawDate(string html, ref string msg)
        {
            //<label id="ctl00_ContentPlaceHolder1_LottoDrawDate">Saturday, 16 Apr 2016</label>
            var rgx = new Regex(@"<label id=""ctl00_ContentPlaceHolder1_LottoDrawDate"">(?<date>.+)</label>");
            var match = rgx.Match(html);
            if (match.Success)
            {
                return DateTime.Parse(match.Groups["date"].Value);
            }
            else
            {
                msg = "failed to match draw date.";
                return null;
            }
        }

        public static int[] GetLottos(string html, ref string msg)
        {
            //<img src="http://images.nzcity.co.nz/nz/lotto/balls/medium/3.gif" id="ctl00_ContentPlaceHolder1_ball1" width="50" height="50" alt="3" />
            var rgx = new Regex(@"<img src=""http://images.nzcity.co.nz/nz/lotto/balls/medium/(?<ball>\d+).gif"" id=""ctl00_ContentPlaceHolder1_ball(?<idx>\d)"" width=""50"" height=""50"" alt=""(?<ball>\d+)"" />");
            var matches = rgx.Matches(html);
            var len = matches.Count;
            if (len != 6)
            {
                msg = "number of lotto balls is not 6";
                return null;
            }

            var lottos = new int[6];
            for(int i = 0; i < len; i++)
            {
                int idx = int.Parse(matches[i].Groups["idx"].Value);
                int ballNumber = int.Parse(matches[i].Groups["ball"].Value);
                lottos[idx-1] = ballNumber;
            }

            return lottos;
        }

        public static string GetBonus(string html, ref string msg)
        {
            //<img src="http://images.nzcity.co.nz/nz/lotto/balls/medium/25.gif" id="ctl00_ContentPlaceHolder1_bonus1" width="50" height="50" alt="Bonus number 25" />
            var rgx = new Regex(@"<img src=""http://images.nzcity.co.nz/nz/lotto/balls/medium/(?<bonus>\d+).gif"" id=""ctl00_ContentPlaceHolder1_bonus1"" width=""50"" height=""50"" alt=""Bonus number (?<bonus>\d+)"" />");
            var match = rgx.Match(html);
            if (match.Success)
            {
                return match.Groups["bonus"].Value;
            }
            else
            {
                msg = "failed to match bonus number.";
                return null;
            }
        }

        public static int[] GetStrikes(string html, ref string msg)
        {
            //<img src="http://images.nzcity.co.nz/nz/lotto/balls/medium/4.gif" id="ctl00_ContentPlaceHolder1_strike1" width="50" height="50" alt="4" />
            var rgx = new Regex(@"<img src=""http://images.nzcity.co.nz/nz/lotto/balls/medium/(?<ball>\d+).gif"" id=""ctl00_ContentPlaceHolder1_strike(?<idx>\d)"" width=""50"" height=""50"" alt=""(?<ball>\d+)"" />");
            var matches = rgx.Matches(html);
            var len = matches.Count;
            if (len != 4)
            {
                msg = "number of strike is not 4";
                return null;
            }

            var strikes = new int[4];
            for (int i = 0; i < len; i++)
            {
                int idx = int.Parse(matches[i].Groups["idx"].Value);
                int ballNumber = int.Parse(matches[i].Groups["ball"].Value);
                strikes[idx-1] = ballNumber;
            }

            return strikes;
        }

        public static int? GetPowerBall(string html, ref string msg)
        {
            //<img src="http://images.nzcity.co.nz/nz/lotto/balls/medium/4.gif" id="ctl00_ContentPlaceHolder1_powerball" width="50" height="50" alt="4" />
            var rgx = new Regex(@"<img src=""http://images.nzcity.co.nz/nz/lotto/balls/medium/(?<powerball>\d+).gif"" id=""ctl00_ContentPlaceHolder1_powerball"" width=""50"" height=""50"" alt=""(?<powerball>\d+)"" />");
            var match = rgx.Match(html);
            if (match.Success)
            {
                return int.Parse(match.Groups["powerball"].Value);
            }
            else
            {
                msg = "failed to match powerball number.";
                return null;
            }
        }
    }
}
