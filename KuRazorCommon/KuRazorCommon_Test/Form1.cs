
using KuRazorCommon;
using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Dynamic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web.Razor;
using System.Windows.Forms;
using Tuple的用法.Template;

namespace Tuple的用法
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //List<Tuple<int, string>> list = new List<Tuple<int, string>>();

            //Tuple<int, string> obj = new Tuple<int, string>(1, "ss");

            //list.Add(obj);
        }



        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                dynamic model = new { UserID = 5016 };
                string path = @"E:\T_开文\公共类\KuRazorCommon\KuRazorCommon_Test\demo.txt";
                string sourceCode = System.IO.File.ReadAllText(path, System.Text.Encoding.GetEncoding("GBK"));
                KuRazor.IsDebug = false;
                KuRazor.CreateDLL<dynamic>(sourceCode, model, "ddd", "dll/");

                MessageBox.Show("生成成功！");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }


        private void Read_Click(object sender, EventArgs e)
        {
            try
            {
                dynamic model = new { UserID = 5016, IsAdmin = true, Name = "B" };
                string dllPath = AppDomain.CurrentDomain.BaseDirectory + "dll/ddd.dll";
                string html = KuRazor.GetRazorHtml<dynamic>(model, dllPath, true);
                MessageBox.Show(html);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                dynamic model = new { UserID = 5016, IsAdmin = true, Name = "凯文" };
                string dllPath = AppDomain.CurrentDomain.BaseDirectory + "dll/ddd.dll";
                string html = KuRazor.GetRazorHtml<dynamic>(model, dllPath, true);
                MessageBox.Show(html);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

            //try
            //{
            //    dynamic model = new { IsAdmin = true, Name = "name_1" };
            //    ITemplate instance = new KuRazorCommon.Dynamic.ddd();
            //    instance.SetModel(model);
            //    using (var writer = new System.IO.StringWriter())
            //    {
            //        instance.Run(writer);
            //        string html = writer.ToString();
            //    }
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show(ex.ToString());
            //}
        }
    }
}
