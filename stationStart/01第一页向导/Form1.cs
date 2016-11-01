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
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        int a=0;
        private void button1_Click(object sender, EventArgs e)
        {
            a = 1;
        }

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
                //pic.BackgroundImageLayout = ImageLayout.Stretch;
                
                pic.Parent = pictureBox1;
                Point formPoint = pictureBox1.PointToClient(Control.MousePosition);
                int i = formPoint.X;
                int j = formPoint.Y;
                pic.Location = new Point(i, j); //显示位置
                pictureBox1.Controls.Add(pic);
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

        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            //20311744
            string conStr = "Data Source=liuc-pc/orcl;User ID =interbasindb;Password=interbasin;";
            ////插入电站基础信息
            //OracleConnection con = new OracleConnection(conStr);//给对象con赋值，建立数据库连接
            //con.Open();//打开数据库连接
            //string sql;//插入电站的sql语句
            //sql = "delete from UPZV t where t.stationid=20312153";
            //OracleCommand cmd = new OracleCommand(sql, con);
            //cmd.ExecuteNonQuery();
            //con.Close();//关闭数据库连接

            //con.Open();//打开数据库连接
            //sql = "delete from downrz where stationid=20312153";
            //cmd = new OracleCommand(sql, con);
            //cmd.ExecuteNonQuery();
            //con.Close();//关闭数据库连接

            OracleConnection con = new OracleConnection(conStr);//给对象con赋值，建立数据库连接
            con.Open();//打开数据库连接
            string sql;
            sql = "delete from stationinfo where stationid=20311744";
            OracleCommand cmd = new OracleCommand(sql, con);
            cmd = new OracleCommand(sql, con);
            cmd.ExecuteNonQuery();
            con.Close();//关闭数据库连接

            MessageBox.Show("success");
        }

    }
}
