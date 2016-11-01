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
    public partial class Form7 : Form
    {

        private string conStr = "Data Source=liuc-pc/orcl;User ID =interbasindb;Password=interbasin;";

        //public int selectIndex;//指示该电站在流域电站数组里面的序号
        public string stationID;//获取该电站的ID
        public string stationName;//获取该电站的ID


        //-----------Basic-----------------
        private DataTable dt_Basic;
        private BindingSource bs_Basic;
        private OracleDataAdapter da_Basic;
        //-----------UpZV-----------------
        private DataTable dt_UpZV;
        private BindingSource bs_UpZV;
        private OracleDataAdapter da_UpZV;
        //-----------Down_RZ-----------------
        private DataTable dt_DownRZ;
        private BindingSource bs_DownRZ;
        private OracleDataAdapter da_DownRZ;
        //-----------Plant------------------
        private DataTable dt_Plant;
        private BindingSource bs_Plant;
        private OracleDataAdapter da_Plant;
        //-----------PlantType--------------
        private DataTable dt_PlantType;
        private BindingSource bs_PlantType;
        private OracleDataAdapter da_PlantType;

        DataIO dataIO = new DataIO();
        DataTable dt1 = new DataTable("Datas");
        DataTable dt2 = new DataTable("Datas");
        public Form7()
        {
            InitializeComponent();
      
            
            //AdjustType.Items.Clear();
            //for (int i = 1; i < 72; i++)
            //{
            //    cb1.Items.Add("第" + i.ToString() + "年");
            //}
            //cb1.SelectedIndex = 0;


            dt_Basic = new DataTable();
            bs_Basic = new BindingSource();
            da_Basic = new OracleDataAdapter();
            //考虑到没有csv数据时，增加空行的问题，对每个表增加相应的列名
            dt_UpZV = new DataTable();
            bs_UpZV = new BindingSource();
            da_UpZV = new OracleDataAdapter();

            //绑定水位库容 bs、dt、dgv
            bs_UpZV.DataSource = dt_UpZV;
            dgv_UpZV.DataSource = bs_UpZV;
            dt_UpZV.Columns.Add("Z");
            dt_UpZV.Columns.Add("V");
            //dt_UpZV.Columns.Add("STATIONID");
            //dt_UpZV.Columns.Add("ID");
            //dgv_UpZV.Columns["STATIONID"].Visible = false;
            //dgv_UpZV.Columns["ID"].Visible = false;

            dt_DownRZ = new DataTable();
            bs_DownRZ = new BindingSource();
            da_DownRZ = new OracleDataAdapter();

            //绑定下游水位和下泄 bs、dt、dgv
            bs_DownRZ.DataSource = dt_DownRZ;
            dgv_DownRZ.DataSource = bs_DownRZ;
            dt_DownRZ.Columns.Add("R");
            dt_DownRZ.Columns.Add("Z");
            //dt_DownRZ.Columns.Add("STATIONID");
            //dt_DownRZ.Columns.Add("ID");

            //  机组Q H R
            dt_Plant = new DataTable();
            bs_Plant = new BindingSource();
            da_Plant = new OracleDataAdapter();
            //绑定bs、dt、dgv
            bs_Plant.DataSource = dt_Plant;
            dgv_Plant.DataSource = bs_Plant;
            dt_Plant.Columns.Add("H");
            dt_Plant.Columns.Add("Q");
            dt_Plant.Columns.Add("N");
            bs_Plant.DataSource = dt_Plant;
            dgv_Plant.DataSource = bs_Plant;


            dt_PlantType = new DataTable();
            bs_PlantType = new BindingSource();
            da_PlantType = new OracleDataAdapter();
            //绑定bs、dt、dgv
            bs_PlantType.DataSource = dt_PlantType;
            dgv_PlantType.DataSource = bs_PlantType;
            dt_PlantType.Columns.Add("机组名称");
            dt_PlantType.Columns.Add("机组类型");


        }

        

        public void GetBasiciInfo ()
      {
         //得到电站ID

            


            DataTable dt_Basic = new DataTable();
            //string dtName = "dt";
            string sql = "select * from STATIONINFO where stationid='" + stationID + "' ";
            OracleConnection con = new OracleConnection(conStr);
            con.Open();//打开数据库连接
            OracleDataAdapter da_Basic = new OracleDataAdapter();
            da_Basic = new OracleDataAdapter(sql, con);
            da_Basic.Fill(dt_Basic);
            con.Close();//关闭数据库连接
           //taForm_c = new New_station();




            string sql_1 = "select * from UPZV where stationid='" + stationID + "'";
            OracleConnection con_1 = new OracleConnection(conStr);
            con.Open();//打开数据库连接
            OracleDataAdapter da_UpZV = new OracleDataAdapter();
            da_UpZV = new OracleDataAdapter(sql_1, con_1);
            da_UpZV.Fill(dt_UpZV);
            con.Close();//关闭数据库连接


            string sql_2 = "select * from DOWNRZ where  stationid='" + stationID + "'" ;
            OracleConnection con_2 = new OracleConnection(conStr);
            con.Open();//打开数据库连接
            OracleDataAdapter   da_DownRZ = new OracleDataAdapter();
            da_DownRZ = new OracleDataAdapter(sql_2, con_2);
            da_DownRZ.Fill(dt_DownRZ);
            con.Close();//关闭数据库连接

          
            //StationNamecb.Text = stationName;
           
            stationName1.Text = dt_Basic.Rows[0]["STATIONNAME"].ToString();
            Checkz.Text = dt_Basic.Rows[0]["CHECKFLOODZ"].ToString();
            Designz.Text = dt_Basic.Rows[0]["DESIGNFLOODZ"].ToString();
            Floodmaxz.Text = dt_Basic.Rows[0]["FLOODMAXZ"].ToString();
            Floodmaxz.Text = dt_Basic.Rows[0]["FLOODMAXZ"].ToString();
            Normalz.Text = dt_Basic.Rows[0]["NORMALZ"].ToString();
            Floodlimitz.Text = dt_Basic.Rows[0]["FLOODLIMITZ"].ToString();
            Deadz.Text = dt_Basic.Rows[0]["DEADZ"].ToString();
            Totalv.Text = dt_Basic.Rows[0]["TOTALV"].ToString();
            Activev.Text = dt_Basic.Rows[0]["ACTIVEV"].ToString();
            Deadv.Text = dt_Basic.Rows[0]["DEADV"].ToString();
            Guaranteedn.Text = dt_Basic.Rows[0]["GUARANTEEDN"].ToString();
            Plantsmaxq.Text = dt_Basic.Rows[0]["PLANTSMAXQ"].ToString();
            Stationmaxr.Text = dt_Basic.Rows[0]["STATIONMAXR"].ToString();
            AdjustType.SelectedIndex = Convert.ToInt32(dt_Basic.Rows[0]["ADJUSTTYPE"]);
            XishuN.Text = Convert.ToString(8);


            bs_UpZV.Sort = "Z";                                  //画图 chart
            chart_UpZV.Series[0].Points.Clear();
            chart_UpZV.Series[0].XValueMember = "V";
            chart_UpZV.Series[0].YValueMembers = "Z";
            chart_UpZV.DataSource = dt_UpZV;


            bs_DownRZ.Sort = "Z";
            chart_DownRZ.Series[0].Points.Clear();
            chart_DownRZ.Series[0].XValueMember = "R";
            chart_DownRZ.Series[0].YValueMembers = "Z";
            chart_DownRZ.DataSource = dt_DownRZ;



                
      }

        private void Form7_Load(object sender, EventArgs e)
        {
           
            //taForm_c = new New_station();

            Dictionary<string, int> A = new Dictionary<string, int>();

            DataTable dt_StationInfo = new DataTable();
            string sql = "select stationname from STATIONINFO ";//where stationid='" + stationID + "' and inuse=1";
            OracleConnection con = new OracleConnection(conStr);
            con.Open();//打开数据库连接
            OracleDataAdapter da_StationInfo = new OracleDataAdapter();
            da_StationInfo = new OracleDataAdapter(sql, con);
            da_StationInfo.Fill(dt_StationInfo);
            con.Close();//关闭数据库连接

            StationNamecb.Items.Clear();
            for (int i = 0; i < dt_StationInfo.Rows.Count; i++)
            {
                A.Add(dt_StationInfo.Rows[i][0].ToString(), i);
                StationNamecb.Items.Add(dt_StationInfo.Rows[i][0]);
            }
            StationNamecb.SelectedIndex = A[stationName];

        }


        private void StationNamecb_SelectionChangeCommitted(object sender, EventArgs e)
        {
            DataTable dt = new DataTable();
            stationName = StationNamecb.SelectedItem.ToString();
            //string dtName = "dt";
            string sql = "select stationid from STATIONINFO where stationname='" + stationName + "' ";
            OracleConnection con = new OracleConnection(conStr);
            con.Open();//打开数据库连接
            OracleDataAdapter da = new OracleDataAdapter();
            da = new OracleDataAdapter(sql, con);
            da.Fill(dt);
            con.Close();//关闭数据库连

            stationID = dt.Rows[0]["STATIONID"].ToString();
            GetBasiciInfo();
        }
        


    }
}
