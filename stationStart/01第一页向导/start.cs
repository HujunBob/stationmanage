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
    public partial class start : Form
    {
        //连接数据库的钥匙
        private string conStr = "Data Source=liuc-pc/orcl;User ID =interbasindb;Password=interbasin;";


        //定义一个station类
        public class Station
        {
            public String stationid;  //电站编号
            public String stationname; //电站名称
            //电站相对位置坐标
            public double pox;
            public double poy;
            //电站在界面上的展示
            public PictureBox picturebox;
            public Label lab_name;
        }

        public class River  //存储流域信息
        {
            //流域编号及名称
            public String riverid;
            public String rivername;
            //流域所包含的电站和上下游关系
            public List<Station> stations;
            //public List<LinkLine> linkline;

        };

        New_station staForm_c = null;
        List<Station> myStations;//定义电站数组

        //List<PictureBox> pic;
        //List<Label> lab;
        int position_i = 0;
        int position_j = 0;
        int status_k = 0;
        public start()
        {
            InitializeComponent();
            
            staForm_c = new New_station();//实例化b窗体
            staForm_c.MyEvent += new New_station.MyDelegate(c_MyEvent);//监听b窗体事件

            DataTable dt = new DataTable();
            OracleConnection con = new OracleConnection(conStr);//给对象con赋值，建立数据库连接
            con.Open();//打开数据库连接
            //todo:默认只读取湘江流域的可用电站
            string sql = "select * from STATIONINFO t where t.inuse=1 and t.riverid=140222 order by t.stationid";
            OracleDataAdapter adapter = new OracleDataAdapter(sql, con);
            adapter.Fill(dt);
            con.Close();//关闭数据库连接

            myStations = new List<Station>();

            for (int i = 0; i < dt.Rows.Count; i++)
            {

                //Label lb = new Label();
                //PictureBox pt = new PictureBox();

                //lab.Add(lb);
                //pic.Add(pt);
                Station tempstation = new Station();

                tempstation.stationid = dt.Rows[i]["stationid"].ToString();
                tempstation.stationname = dt.Rows[i]["stationname"].ToString();

                tempstation.lab_name = new Label();
                tempstation.lab_name.Text = dt.Rows[i]["stationname"].ToString();

                tempstation.picturebox = new PictureBox();
                tempstation.picturebox.Height = 10;
                tempstation.picturebox.Width = 10;
                tempstation.picturebox.Image = Image.FromFile("..\\图片\\bullet_green3.png");
                tempstation.picturebox.SizeMode = PictureBoxSizeMode.StretchImage;

                tempstation.pox = Convert.ToDouble(dt.Rows[i]["relativex"].ToString());
                tempstation.poy = Convert.ToDouble(dt.Rows[i]["relativey"].ToString());

                tempstation.picturebox.Left = Convert.ToInt32(tempstation.pox * pictureBox1.Width - tempstation.picturebox.Width / 2.0);
                tempstation.picturebox.Top = Convert.ToInt32(tempstation.poy * pictureBox1.Height - tempstation.picturebox.Height / 2.0);
                tempstation.lab_name.Left = Convert.ToInt32(tempstation.pox * pictureBox1.Width - tempstation.picturebox.Width / 2.0);
                tempstation.lab_name.Top = Convert.ToInt32(tempstation.poy * pictureBox1.Height - tempstation.picturebox.Height / 2.0)-6;
                tempstation.lab_name.BackColor = Color.Transparent;
                tempstation.picturebox.Parent = pictureBox1;    //这是什么？
                tempstation.lab_name.Parent = pictureBox1;

                tempstation.picturebox.DoubleClick += new System.EventHandler(btn_Doubleclick);
                tempstation.picturebox.MouseMove += new System.Windows.Forms.MouseEventHandler(pic_Mousemove);
                tempstation.picturebox.MouseLeave += new System.EventHandler(pic_Mouseleave);
                tempstation.picturebox.Click += new System.EventHandler(pic_mouseclick);

                myStations.Add(tempstation);
            }

        }

        /// <summary>
        /// 删除水库
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void pic_mouseclick(object sender, EventArgs e)
        {
            if (status_k == 2)
            {

                DataTable dt = new DataTable();
                OracleConnection con = new OracleConnection(conStr);//给对象con赋值，建立数据库连接
                con.Open();//打开数据库连接
                //todo:默认只读取湘江流域的可用电站
                string sql = "select * from STATIONINFO t where t.inuse=1 and t.riverid=140222 order by t.stationid";
                OracleDataAdapter adapter = new OracleDataAdapter(sql, con);
                adapter.Fill(dt);
                con.Close();//关闭数据库连接


                Form7 d1 = new Form7();
                for (int i = 0; i < myStations.Count; i++)
                {
                    if (myStations[i].picturebox == sender)   //遍历
                    {
                        d1.stationID = myStations[i].stationid;
                       
                    }
                }

               // string conStr = "Data Source=liuc-pc/orcl;User ID =interbasindb;Password=interbasin;";
                // string conStr = "Data Source=liuc-pc/orcl;User ID =interbasindb;Password=interbasin;";
                OracleConnection con_0 = new OracleConnection(conStr);//给对象con赋值，建立数据库连接
                con_0.Open();//打开数据库连接
                string sql_0 = "delete from stationinfo where stationid=d1.stationID";
                OracleCommand cmd_0 = new OracleCommand(sql_0, con_0);
                cmd_0 = new OracleCommand(sql_0, con_0);
                cmd_0.ExecuteNonQuery();
                con_0.Close();//关闭数据库连接

                for (int i = 0; i < myStations.Count; i++)
                {
                    if (myStations[i].picturebox == sender)   //遍历
                    {

                        pictureBox1.Controls.Remove(myStations[i].picturebox);
                        pictureBox1.Controls.Remove(myStations[i].lab_name);
                    }
                   
                }
            }
            status_k = 0;
           // MessageBox.Show("success");
        }

        private DataTable InputCSV(string fileName)
        {
            DataTable dt = new DataTable();
            FileStream fs = new FileStream(fileName, System.IO.FileMode.Open, System.IO.FileAccess.Read);
            StreamReader sr = new StreamReader(fs, System.Text.Encoding.Default);
            //记录每次读取的一行记录
            string strLine = "";
            //记录每行记录中的各字段内容
            string[] aryLine;
            //标示列数
            int columnCount = 0;
            //标示是否是读取的第一行
            bool IsFirst = true;
            //逐行读取CSV中的数据
            while ((strLine = sr.ReadLine()) != null)
            {
                aryLine = strLine.Split(',');
                if (IsFirst == true)
                {
                    IsFirst = false;
                    columnCount = aryLine.Length;
                    //创建列
                    for (int i = 0; i < columnCount; i++)
                    {
                        DataColumn dc = new DataColumn(aryLine[i]);
                        dt.Columns.Add(dc);
                    }
                }
                else
                {
                    DataRow dr = dt.NewRow();
                    for (int j = 0; j < columnCount; j++)
                    {
                        dr[j] = aryLine[j];
                    }
                    dt.Rows.Add(dr);
                }
            }
            sr.Close();
            fs.Close();
            return dt;
        }

        //在newstation窗口点击“完成”后，触发的事件，用于在地图上显示添加电站图标
        void c_MyEvent()
        {
            Station tempstation = new Station();

            tempstation.stationid = staForm_c.stationId.ToString();
            tempstation.stationname = staForm_c.stationName.ToString();

            tempstation.lab_name = new Label();
            tempstation.lab_name.Text = staForm_c.stationName.ToString();

            tempstation.picturebox = new PictureBox();
            tempstation.picturebox.Height = 10;
            tempstation.picturebox.Width = 10;
            tempstation.picturebox.Image = Image.FromFile("..\\图片\\bullet_green3.png");
            tempstation.picturebox.SizeMode = PictureBoxSizeMode.StretchImage;

            tempstation.pox = 1.0 * position_i / pictureBox1.Width;
            tempstation.poy = 1.0 * position_j / pictureBox1.Width;

            tempstation.picturebox.Left = position_i;
            tempstation.picturebox.Top = position_j;
            tempstation.lab_name.Left = position_i;
            tempstation.lab_name.Top = position_j - 6;
            tempstation.lab_name.BackColor = Color.Transparent;
            tempstation.picturebox.Parent = pictureBox1;
            tempstation.lab_name.Parent = pictureBox1;

            tempstation.picturebox.DoubleClick += new System.EventHandler(btn_Doubleclick);
            tempstation.picturebox.MouseMove += new System.Windows.Forms.MouseEventHandler(pic_Mousemove);
            tempstation.picturebox.MouseLeave += new System.EventHandler(pic_Mouseleave);

            myStations.Add(tempstation);

            //把位置信息补充到新的电站信息里面DB
            DataTable dt = new DataTable();
            OracleConnection con = new OracleConnection(conStr);//给对象con赋值，建立数据库连接
            con.Open();//打开数据库连接
            //todo:默认只读取湘江流域的可用电站
            string sql = "update stationinfo t set t.relativex = " + (tempstation.pox).ToString() +
                        ", t.relativey = " + (tempstation.poy).ToString() + "where t.stationid= " + tempstation.stationid;
            OracleCommand cmd = new OracleCommand(sql, con);
            cmd.ExecuteNonQuery();
            con.Close();//关闭数据库连接

            status_k = 0;
        }
        private void OutputCSV(DataTable dt, string fileName)
        {
            FileInfo fi = new FileInfo(fileName);
            if (!fi.Directory.Exists)
            {
                fi.Directory.Create();
            }
            FileStream fs = new FileStream(fileName, System.IO.FileMode.Create, System.IO.FileAccess.Write);
            StreamWriter sw = new StreamWriter(fs, System.Text.Encoding.UTF8);
            string data = "";
            //写出列名称
            for (int i = 0; i < dt.Columns.Count; i++)
            {
                data += dt.Columns[i].ColumnName.ToString();
                if (i < dt.Columns.Count - 1)
                {
                    data += ",";
                }
            }
            sw.WriteLine(data);
            //写出各行数据
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                data = "";
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    string str = dt.Rows[i][j].ToString();
                    str = str.Replace("\"", "\"\"");//替换英文冒号 英文冒号需要换成两个冒号
                    if (str.Contains(',') || str.Contains('"') || str.Contains('\r') || str.Contains('\n')) //含逗号 冒号 换行符的需要放到引号中
                    {
                        str = string.Format("\"{0}\"", str);
                    }

                    data += str;
                    if (j < dt.Columns.Count - 1)
                    {
                        data += ",";
                    }
                }
                sw.WriteLine(data);
            }
            sw.Close();
            fs.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            status_k = 1;
        }

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            if (status_k == 1)
            {

                Point formPoint = pictureBox1.PointToClient(Control.MousePosition);//获取相对于picturebox1的鼠标坐标 
                position_i = formPoint.X;
                position_j = formPoint.Y;
                staForm_c.ShowDialog();
            
            }
            status_k = 0;
        }
        private void btn_Doubleclick(object sender, System.EventArgs e)      // 设为查看按钮
        {
            Form7 d = new Form7();
            for (int i = 0; i < myStations.Count; i++)
            {
                if (myStations[i].picturebox==sender)   //遍历
                {
                    d.stationID = myStations[i].stationid;
                    d.stationName = myStations[i].stationname;
                    
                }
            }
            d.Show();

            d.GetBasiciInfo();

            //stationId = dt.Rows[0]["stationid"].ToString();
            //stationName = dt.Rows[0]["stationname"].ToString();

        }

        //针对已有电站，被选中电站变红
        private void pic_Mousemove(object sender, System.EventArgs e)//生成鼠标移入事件
        {
            for (int i = 0; i < myStations.Count; i++)
            {
                if (myStations[i].picturebox == sender)//pic被选定
                {
                    myStations[i].picturebox.Image = Image.FromFile("..\\图片\\bullet_red3.png");
                }

            }
        }

        //不选择，所有电站变绿
        private void pic_Mouseleave(object sender, System.EventArgs e)//生成鼠标离开事件
        {
            for (int i = 0; i < myStations.Count; i++)
            {
                myStations[i].picturebox.Image = Image.FromFile("..\\图片\\bullet_green3.png");
            }

        }


        //关闭窗体
        private void start_FormClosing(object sender, FormClosingEventArgs e)
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

        //删除电站
        private void button2_Click(object sender, EventArgs e)
        {

            status_k = 2;

        }

    }
}
