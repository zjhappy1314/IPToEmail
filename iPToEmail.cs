using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Net;
using System.Text.RegularExpressions;
using System.Net.Mail;
using System.Collections.Specialized;

namespace iPToEmail
{
    public partial class iPToEmail : Form
    {
        string oldIP = null;
        string newIP = NetHelper.GetIP();

        public iPToEmail()
        {
            InitializeComponent();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            //this.oldIP = this.newIP;
            this.newIP = NetHelper.GetIP();
            this.richTextBox1.Text = "刷新当前IP>> 开始刷新 \r\n" +
                "\t刷新时间: " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " \r\n" +
                //"\t历史  IP: " + this.oldIP + " \r\n" +
                "\t当前  IP: " + this.newIP + " \r\n" +
                //"\t是否变更: " + (this.newIP == this.oldIP ? "IP未变更" : "IP已变更") + " \r\n" +
                "\t刷新完成 \r\n\r\n" +
                this.richTextBox1.Text;
            this.timer1.Interval = Convert.ToInt32 ( this.numericUpDown1.Value ) * 1000;
            this.timer1.Enabled = true;
            this.groupBox1.Enabled = false;
        }

        void toEmail(string oldIP,string newIP)
        {
            //this.oldIP = oldIP;//this.newIP;
            //this.newIP = newIP;//NetHelper.GetIP();
            string newInfor = "开始发送IP>> 开始刷新 \r\n" +
                "\t刷新时间: " + DateTime.Now.ToString ( "yyyy-MM-dd HH:mm:ss" ) + " \r\n" +
                "\t历史  IP: " + oldIP + " \r\n" +
                "\t当前  IP: " + newIP + " \r\n" +
                "\t是否变更: " + ( newIP == oldIP ? "IP未变更" : "IP已变更" ) + " \r\n" +
                "\t刷新完成 \r\n";
                //"\t开始发送>> \r\n" +
                //"\t代理邮箱: " + this.textBox1.Text + " \r\n" +
                //"\t接受邮箱: " + this.textBox2.Text + " \r\n" +
                //"\t发送时间: " + DateTime.Now.ToString ( "yyyy-MM-dd HH:mm:ss" ) + " \r\n" +
                //"\t发送主题: " + "IP地址变更信息" + " \r\n" +
                //"\t发送内容: " + this.newIP + " \r\n"+
                //"\t发送完成 \r\n\r\n";                
            this.richTextBox1.Text = newInfor + this.richTextBox1.Text;
            string MessageSubject = "IP地址变更信息";        //邮件主题
            string outMsg = null;
            if (!NetHelper.Send(this.textBox3.Text.Trim(), Convert.ToInt32(this.textBox4.Text), this.textBox1.Text, this.textBox5.Text, MessageSubject, NetHelper.GetHtml(newInfor), this.textBox2.Text, out outMsg))
            {
                MessageBox.Show(outMsg);
                this.groupBox1.Enabled = true;
                this.timer1.Enabled = false;
            }            
        }

        //开始发送按钮，暂时不用。
        private void button2_Click(object sender, EventArgs e)
        {
            if (!Regex.IsMatch(this.textBox1.Text, @"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"))
            {
                MessageBox.Show("请输入正确的发送代理邮箱");
            }
            else if (!Regex.IsMatch(this.textBox2.Text, @"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"))
            {
                MessageBox.Show("请输入正确的接收邮箱");
            }
            else if (this.textBox5.Text.Trim() == "")
            {
                MessageBox.Show("请输入正确的发送邮箱密码");
            }
            else if (this.textBox5.Text.Trim() == "")
            {
                MessageBox.Show("请输入正确的发送邮箱密码");
            }
            else if (this.textBox3.Text.Trim() == "" || this.textBox4.Text.Trim() == "")
            {
                MessageBox.Show("请输入正确的依赖项\"SMTP\"和\"端口号\"");
            }
            else
            {
                this.timer1.Interval = Convert.ToInt32(this.numericUpDown1.Value) * 1000;
                this.timer1.Enabled = true;
                this.groupBox1.Enabled = false;
            }
        }

        //停止获取IP按钮
        private void button1_Click(object sender, EventArgs e)
        {
            this.groupBox1.Enabled = true;
            this.timer1.Enabled = false;
        }

        //定时器
        private void timer1_Tick(object sender, EventArgs e)
        {
            this.oldIP = this.newIP;//在获取新的IP前，先保存之前的IP地址
            this.newIP = NetHelper.GetIP (); //获取新的IP地址            
            if (this.oldIP != this.newIP)
            {
                toEmail (this.oldIP,this.newIP);//如果不同则发送邮件到指定的用户
            }              
        }

        private void iPToEmail_Load ( object sender, EventArgs e )
        {

        }

        
    }
}
