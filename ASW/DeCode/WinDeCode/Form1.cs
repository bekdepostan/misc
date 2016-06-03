using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WinDeCode
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                textBox2.Text = Regex.Unescape(textBox1.Text);
                Copy();
            }
            catch
            {

            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                var ori = textBox1.Text.Replace("I", "").Replace("(", "").Replace(")", "");
                var arr = ori.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                string str = "'";
                foreach (var v in arr)
                {
                    var asi = int.Parse(v) - 36;
                    var c = (char)(asi);
                    str += c.ToString();
                }
                str += "'";
                textBox2.Text = str;
                Copy();
            }
            catch
            {

            }
        }


        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                textBox2.Text = Base36Encode(textBox1.Text);
                Copy();
            }
            catch
            {

            }
        }

        private void Copy()
        {
            textBox1.SelectAll();
            System.Windows.Forms.Clipboard.SetText(textBox2.Text);
        }

        private string Base36Encode(string content)
        {
            var number = long.Parse(content.Replace("L(","").Replace(")",""));
            return Base36Encode(number);
        }

        private string Base36Encode(long input)
        {
            const string CharList = "0123456789abcdefghijklmnopqrstuvwxyz";
            if (input < 0) throw new ArgumentOutOfRangeException("input", input, "input cannot be negative");

            char[] clistarr = CharList.ToCharArray();
            var result = new Stack<char>();
            while (input != 0)
            {
                result.Push(clistarr[input % 36]);
                input /= 36;
            }
            return new string(result.ToArray());
        }

    }
}
