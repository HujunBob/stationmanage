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
    public partial class Form2 : Form
    {

        DataIO dataIO = new DataIO();
        DataTable dt0 = new DataTable();
        int S = 0;
        int M = 0;
        int L = 0;
        public Form2()
        {
            InitializeComponent();
           
        }
        private void button1_Click(object sender, EventArgs e)
        {

            New_station station = new New_station();
            station.Show();
            


            //20311744
            string conStr = "Data Source=liuc-pc/orcl;User ID =interbasindb;Password=interbasin;";
            //插入电站基础信息
            OracleConnection con = new OracleConnection(conStr);//给对象con赋值，建立数据库连接
            con.Open();//打开数据库连接
            string sql;//插入电站的sql语句
           // sql = "delete from UPZV t where t.stationid=20312153";
            sql = "update stationinfo t set t.shortcanuse="+S.ToString()+" where t.stationid=20312153";
            OracleCommand cmd = new OracleCommand(sql, con);
            cmd.ExecuteNonQuery();
            con.Close();//关闭数据库连接
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            dt0 = dataIO.InputCSV("..\\数据\\2016-10-18水库基础数据.csv");
            comboBox1.Items.Clear();
            for (int i = 1; i < dt0.Rows.Count; i++)
            {
                comboBox1.Items.Add(dt0.Rows[i][0].ToString() );
            }
            comboBox1.SelectedIndex = 0;

            comboBox2.Items.Clear();
            for (int i = 1; i < dt0.Rows.Count; i++)
            {
                comboBox2.Items.Add(dt0.Rows[i][0].ToString());
            }
            comboBox2.SelectedIndex = 0;

           
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            S = 1;
        }

        private void checkBox4_CheckedChanged(object sender, EventArgs e)
        {
            M = 1;
        }

        private void checkBox5_CheckedChanged(object sender, EventArgs e)
        {
            L = 1;
        }


    }
}
