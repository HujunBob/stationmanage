using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Configuration;

using Oracle.ManagedDataAccess;
using Oracle.ManagedDataAccess.Client;
using Oracle.ManagedDataAccess.Types;

namespace _01第一页向导
{
    //form1为向导页
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        //添加水电站按键的标识符
        int a=0;
        private void button1_Click(object sender, EventArgs e)
        {
            a = 1;
        }

        /// <summary>
        ///鼠标在画布上移动触发,先添加按键操作再在画布上添加点
        /// 照片从图片\\bullet_green3.png中获取
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {

            if (a == 1)
            {
                a=0;
                PictureBox pic = new PictureBox();
                pic.Height = 20;
                pic.Width = 20;
                pic.Image = Image.FromFile("..\\图片\\bullet_green3.png");
                pic.SizeMode = PictureBoxSizeMode.StretchImage;
                //将图片添加到画布上
                pic.Parent = pictureBox1;
                //获取鼠标点的坐标相对于工作区域
                Point formPoint = pictureBox1.PointToClient(Control.MousePosition);
             //   int i = formPoint.X;
             //   int j = formPoint.Y;
               // pic.Location = new Point(i, j); //显示位置
                pic.Location = formPoint;
                pictureBox1.Controls.Add(pic);
                //触发画布双击事件
                pic.DoubleClick += new System.EventHandler(btn_Doubleclick);

                DialogResult dr = MessageBox.Show("确定在此处添加电站？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (dr == DialogResult.Yes)
                {
                    PictureBox b1 = (PictureBox)sender;
                    Form2 fm2 = new Form2();
                    fm2.Show();

                }
                else
                {
                    //pic.Visible = false;
                    this.Controls.Remove(pic);
                    //MessageBox.Show(pic.Name);
                }
               
            }
            
        }
        private void btn_Doubleclick(object sender, System.EventArgs e)      // 设为查看按钮
        {
            Form7 fm7=new Form7();
            fm7.Show();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            DialogResult dr = MessageBox.Show("是否关闭窗体", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (dr == DialogResult.Yes)
            {
                e.Cancel = false;

            }
            else
            {
                e.Cancel = true;
            }
        }
        private void button2_Click(object sender, EventArgs e)
        {
            //20311744
            string conStr = "Data Source=liuc-pc/orcl;User ID =interbasindb;Password=interbasin;";
            using (OracleConnection con = new OracleConnection(conStr))//给对象con赋值，建立数据库连接
            {
                con.Open();
                //sql="delete from stationinfo where x="+x+"and y="+y;//(x,y)通过x,y坐标删除
                OracleCommand cmd = new OracleCommand();
                cmd.CommandText= "delete from stationinfo where stationid=20311744";
                cmd.Connection = con;
                cmd.ExecuteNonQuery();
                con.Close();//关闭数据库连接
            }
            MessageBox.Show("success");
        }

    }
}
