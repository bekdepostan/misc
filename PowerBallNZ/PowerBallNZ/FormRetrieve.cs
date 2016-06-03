using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;

namespace PowerBallNZ
{
    public partial class FormRetrieve : Form
    {
        public FormRetrieve()
        {
            InitializeComponent();
        }

        private void btnRetrieve_Click(object sender, EventArgs e)
        {
            Thread backgroundThread = new Thread(format);
            backgroundThread.Start();
        }

        private void SetStatus(string general, int idx, int ttl, string detail)
        {
            if (statusStrip.InvokeRequired)
            {
                Invoke((MethodInvoker)delegate
                {
                    staGeneral.Text = general;
                    staCount.Text = string.Format("{0}/{1}", idx, ttl);
                    progRetrieve.Value = idx * 100 / ttl;
                    staDetail.Text = detail;
                    txtMain.Text = string.Format("{0}\r\n", detail) + txtMain.Text;
                    numFrom.Value = idx;
                });

            }
            else
            {
                staGeneral.Text = general;
                staCount.Text = string.Format("{0}/{1}", idx, ttl);
                progRetrieve.Value = idx / ttl;
                staDetail.Text = detail;
                txtMain.Text = string.Format("{0}\n", detail) + txtMain.Text;
                numFrom.Value = idx;
            }
        }
        private void Retrieve()
        {
            var from = Convert.ToInt32(numFrom.Value);
            var to = Convert.ToInt32(numTo.Value);
            var rnd = new Random();
            string msg = null;
            SetStatus("Retrieve...", 1, to - from + 1, "Readay");

            using (var db = new Entites())
            {
                for (int i = from; i <= to; i++)
                {
                    var d = Draw.Retrieve(i, ref msg);
                    if(d == null)
                    {
                        MessageBox.Show(msg);
                        break;
                    }

                    if (db.Draws.SingleOrDefault(x => x.Number == d.Number) == null)
                    {
                        db.Draws.Add(d);
                        db.SaveChanges();
                    }

                    //update
                    SetStatus("Retrieve...", i - from + 1, to - from + 1,
                        string.Format("{0}:{1}", i, d == null ? msg : d.ToString()));

                    Task.Delay(rnd.Next(5,20)*1000).Wait();
                }
            }

            SetStatus("Finish", to - from + 1, to - from + 1, "Finish");
        }
        private void format()
        {
            using(var db = new Entites())
            {
                var draws = db.Draws.ToList();
                foreach(var d in draws)
                {
                    if(d.Strike1 == null)
                    {
                        continue;
                    }
         
                    d.StrikeStr = string.Format("{0},{1},{2},{3}",d.Strike1, d.Strike2, d.Strike3, d.Strike4);

                    db.Entry(d).State = System.Data.Entity.EntityState.Modified;
                }

                db.SaveChanges();
            }
        }
        private void FormRetrieve_Load(object sender, EventArgs e)
        {
            numFrom.Value = Draw.LastDrawNumber;
            numTo.Value = Draw.LastDrawNumber;
        }
    }
}
