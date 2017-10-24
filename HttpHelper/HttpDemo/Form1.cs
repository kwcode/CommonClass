using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Windows.Forms;

namespace HttpDemo
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            while (true)
            {
                try
                {

                    string url = "http://www.zk71.com/";
                    //CookieContainer cookieContainer = new CookieContainer();
                    string a = HttpHelper.HttpGet(url);
                    //string b = HttpHelper.HttpGet(url, cookieContainer, Encoding.Default);
                }
                catch (Exception ex)
                {
                    throw;
                }
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
