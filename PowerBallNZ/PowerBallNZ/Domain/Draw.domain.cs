using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerBallNZ
{
    public partial class Draw
    {
        public static int LastDrawNumber
        {
            get
            {
                int baseNumber = 1534;
                DateTime baseDate = new DateTime(2016, 4, 17);
                return baseNumber + (DateTime.Today.Subtract(baseDate).Days % 7);
            }
        }

        public static Draw Retrieve(int drawNumber,ref string msg)
        {
            try {
                var html = NZCityHelper.RetrieveLottoResultHtml(drawNumber, ref msg);
                if (string.IsNullOrEmpty(html))
                {
                    msg = "failed to get the response." + msg;
                    return null;
                }
                var draw = new Draw();

                //number
                var number = NZCityHelper.GetDrawNumber(html, ref msg);
                if(number == null)
                {
                    msg = "failed to get draw number." + msg;
                    return null;
                }
                else
                {
                    draw.Number = number.Value;
                }

                //date
                var date = NZCityHelper.GetDrawDate(html, ref msg);
                if(date == null) {
                    msg = "failed to get draw date." + msg;
                    return null;
                }
                else
                {
                    draw.Date = date.Value;
                }

                //lottos
                var lottos = NZCityHelper.GetLottos(html, ref msg);
                if(lottos == null)
                {
                    msg = "failed to get lotto balls." + msg;
                    return null;
                }
                else
                {
                    draw.LottoStr = string.Format("{0},{1},{2},{3},{4},{5}", lottos[0], lottos[1], lottos[2], 
                                                                            lottos[3], lottos[4], lottos[5]);
                    draw.Lotto1 = lottos[0];
                    draw.Lotto2 = lottos[1];
                    draw.Lotto3 = lottos[2];
                    draw.Lotto4 = lottos[3];
                    draw.Lotto5 = lottos[4];
                    draw.Lotto6 = lottos[5];
                }

                //bonus
                var bonus = NZCityHelper.GetBonus(html, ref msg);
                if (bonus == null)
                {
                    msg = "failed to get draw number." + msg;
                    return null;
                }
                else
                {
                    draw.Bonus = bonus;
                }


                //strikes
                var strikes = NZCityHelper.GetStrikes(html, ref msg);
                if (strikes != null)
                {
                    draw.StrikeStr = string.Format("{0},{1},{2},{3}", strikes[0], strikes[1], strikes[2], strikes[3]);
                    draw.Strike1 = strikes[0];
                    draw.Strike2 = strikes[1];
                    draw.Strike3 = strikes[2];
                    draw.Strike4 = strikes[3];
                }

                //powerball
                var power = NZCityHelper.GetPowerBall(html, ref msg);
                if (power != null)
                {
                    draw.PowerBall = power.Value;
                }

                return draw;

            }
            catch(Exception ex)
            {
                msg = ex.Message;
            }
            return null;
        }
    }
}
