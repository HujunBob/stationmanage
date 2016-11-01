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
    public partial class New_station : Form
    {
        DataIO dataIO = new DataIO();

        public String riverid = "140222";//默认湘江流域
        public String stationName = "";
        public String stationId = "";

        private int Step = 0;//在不同的阶段


        //连接数据库的钥匙
        private string conStr = "Data Source=liuc-pc/orcl;User ID =interbasindb;Password=interbasin;";

        //------------Basic----------------
        //DataTable dt_Basic = new DataTable();  
        //------------UP_ZV----------------
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


       

        public New_station()
        {
            InitializeComponent();

            //dt_Basic.Columns.Add("电站名称", Type.GetType("System.String"));
            //dt_Basic.Columns.Add("死水位", Type.GetType("System.Int32"));
            //dt_Basic.Columns.Add("兴利库容", Type.GetType("System.Int32"));
            //dt_Basic.Columns.Add("总库容", Type.GetType("System.Int32"));
            //dt_Basic.Columns.Add("防洪限制水位", Type.GetType("System.Int32"));
            //dt_Basic.Columns.Add("防洪高水位", Type.GetType("System.Int32"));
            //dt_Basic.Columns.Add("设计洪水位", Type.GetType("System.Int32"));
            //dt_Basic.Columns.Add("校核洪水位", Type.GetType("System.Int32"));
            //dt_Basic.Columns.Add("死库容", Type.GetType("System.Int32"));
            //dt_Basic.Columns.Add("正常蓄水位", Type.GetType("System.Int32"));
            //dt_Basic.Columns.Add("机组过流能力", Type.GetType("System.Int32"));
            //dt_Basic.Columns.Add("电站最小下泄", Type.GetType("System.Int32"));
            //dt_Basic.Columns.Add("装机容量", Type.GetType("System.Int32"));
            //dt_Basic.Columns.Add("电站出力系数", Type.GetType("System.Int32"));
            //dt_Basic.Columns.Add("电站调节能力", Type.GetType("System.Int32"));
            //dt_Basic.Columns.Add("保证出力", Type.GetType("System.Int32"));
            //dt_Basic.Columns.Add("电站最大下泄", Type.GetType("System.Int32"));

            cbb_adjust.SelectedIndex = 0;
            panel2_basicInfo.BringToFront();
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

        /// <summary>
        /// 基础数据panel点击下一步
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button2_Click(object sender, EventArgs e)  
        {
            //dt_Basic.Rows.Add(new object[] { edt_stationName.Text.ToString(), edt_deadz.Text.ToString(), edt_activev.Text.ToString(), edt_totalv.Text.ToString(), edt_floodlimitz.Text.ToString(),
            //edt_floodmaxz.Text.ToString(),edt_designz.Text.ToString(),edt_checkz.Text.ToString(),edt_deadv.Text.ToString(),edt_normalz.Text.ToString(),edt_plantsmaxq.Text.ToString(),edt_stationminr.Text.ToString(),
            //edt_ratedn.Text.ToString(),edt_kvalue.Text.ToString(),cbb_adjust.SelectedIndex.ToString(),edt_guaranteedn.Text.ToString(),edt_stationmaxr.Text.ToString()});
            
            // dataGridView1.DataSource = dt_basic;
            //  dataIO.OutputCSV(dt_basic, @"C:\Users\Administrator\Desktop\" + DateTime.Now.ToShortDateString() + "水库基础数据" + ".csv");

            panel3_shuiku.BringToFront();
        }

        /// <summary>
        /// 取消按钮 basicInfo
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button3_Click(object sender, EventArgs e)    
        {
            this.Close();
        }

        /// <summary>
        /// 跳转到尾水panel
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button5_Click(object sender, EventArgs e)    
        {
            panel4_weishui.BringToFront();
        }

        /// <summary>
        /// 跳转到基础信息panel
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button4_Click(object sender, EventArgs e)     
        {
            panel2_basicInfo.BringToFront();
        }


        /// <summary>
        /// 导入水位库容数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button11_Click(object sender, EventArgs e)
        {
            openFileDialog1.InitialDirectory = "..\\数据";//todo...20161023:相对路径有问题   已解决
            openFileDialog1.Filter = "allfile|*.*|csv|*.csv";
            MessageBox.Show("现将第一组数据读入进去");
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                //dt_UpZV = dataIO.InputCSV(openFileDialog1.FileName);
                dataIO.InputCSV1(openFileDialog1.FileName, ref dt_UpZV);
            }
            //todo:dt更新后，竟然需要重新绑定，dgv才会更新
            //bs_UpZV.DataSource = dt_UpZV;
            //dgv_UpZV.DataSource = bs_UpZV;
            
            //liuc...20161022:排序用到BindingSource
            bs_UpZV.Sort = "Z";                                  //画图 chart
            chart_UpZV.Series[0].Points.Clear();
            chart_UpZV.Series[0].XValueMember = "V";
            chart_UpZV.Series[0].YValueMembers = "Z";
            chart_UpZV.DataSource = dt_UpZV;
        }

        /// <summary>
        /// 水位库容添加序号事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgv_UpZV_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            try
            {
                //添加行号 
                SolidBrush v_SolidBrush = new SolidBrush(dgv_UpZV.RowHeadersDefaultCellStyle.ForeColor);
                int v_LineNo = 0;
                v_LineNo = e.RowIndex + 1;

                string v_Line = v_LineNo.ToString();

                e.Graphics.DrawString(v_Line, e.InheritedRowStyle.Font, v_SolidBrush, e.RowBounds.Location.X + 15, e.RowBounds.Location.Y + 5);

            }
            catch (Exception ex)
            {
                MessageBox.Show("添加行号时发生错误，错误信息：" + ex.Message, "操作失败");
            }
        }

        //导入尾水数据
        private void button12_Click(object sender, EventArgs e)
        {
            openFileDialog2.InitialDirectory = "..\\数据";
            openFileDialog2.Filter = "allfile|*.*|csv|*.csv";
            MessageBox.Show("现将第二组数据读入进去");
            if (openFileDialog2.ShowDialog() == DialogResult.OK)
            {
                dt_DownRZ = dataIO.InputCSV(openFileDialog2.FileName);
            }
            bs_DownRZ.DataSource = dt_DownRZ;
            dgv_DownRZ.DataSource = bs_DownRZ;
            //liuc...20161022:排序用到BindingSource
            bs_DownRZ.Sort = "Z";
            chart_DownRZ.Series[0].Points.Clear();
            chart_DownRZ.Series[0].XValueMember = "R";
            chart_DownRZ.Series[0].YValueMembers = "Z";
            chart_DownRZ.DataSource = dt_DownRZ;
        }


        //机组类型的添加数据
        private void button15_Click(object sender, EventArgs e)
        {
            //openFileDialog3.InitialDirectory = "..\\数据";
            //openFileDialog3.Filter = "allfile|*.*|csv|*.csv";
            //MessageBox.Show("现将第三组数据读入进去");
            //if (openFileDialog3.ShowDialog() == DialogResult.OK)
            //{
            //    dt_Plant = dataIO.InputCSV(openFileDialog3.FileName);
            //}
            //todo:考虑到bindingsource的问题

            DataRow dr = dt_PlantType.NewRow();
            dt_PlantType.Rows.Add(dr);
            dgv_PlantType.DataSource = dt_PlantType;


        }
        //机组数据的添加数据
        private void button16_Click(object sender, EventArgs e)
        {
            openFileDialog4.InitialDirectory = "..\\数据";
            openFileDialog4.Filter = "allfile|*.*|csv|*.csv";
            MessageBox.Show("现将第四组数据读入进去");
            if (openFileDialog4.ShowDialog() == DialogResult.OK)
            {
                dt_Plant = dataIO.InputCSV(openFileDialog4.FileName);
            }
            dgv_Plant.DataSource = dt_Plant;
           

            //liuc...20161022:排序用到BindingSource
            //bs_Plant2.Sort = "H";
            //Chart_Plant.Series[0].Points.Clear();
            //Chart_Plant.Series[0].XValueMember = "H";
            //Chart_Plant.Series[0].YValueMembers = "Q";
            //Chart_Plant.Series[0].YValueMembers = "N";
            //Chart_Plant.DataSource = dt_Plant2;
            
        }


        //跳转到水位库容panel 去
        private void button7_Click(object sender, EventArgs e)
        {
            panel3_shuiku.BringToFront();
        }


        //跳转到机组panel 去
        private void button8_Click(object sender, EventArgs e)
        {
            panel5_plant.BringToFront();
        }

        /// <summary>
        /// 完成按钮，将所有检测好的数据写入DB
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public delegate void MyDelegate();
        //定义事件
        public event MyDelegate MyEvent;
        
        private void button14_Click(object sender, EventArgs e)
        {
            //OracleHelper OracleHelper = new OracleHelper();
            //-------------------------保存基础数据到DB-------------------------------

            //插入电站基础信息
            OracleConnection con = new OracleConnection(conStr);//给对象con赋值，建立数据库连接
            con.Open();//打开数据库连接
            string sql;//插入电站的sql语句
            sql = "insert into STATIONINFO t (stationname,adjusttype,normalz,deadz,riverid,plantsmaxq,stationmaxr,stationminr,ratedn,guaranteedn,kvalue,floodlimitz,floodmaxz,designfloodz,checkfloodz,deadv,activev,totalv,inuse) values ('" +
                edt_stationName.Text + "'," + (cbb_adjust.SelectedIndex).ToString() + "," + (edt_normalz.Text).ToString() +
                "," + (edt_deadz.Text).ToString() + "," + riverid + "," + (edt_plantsmaxq.Text).ToString() +
                "," + (edt_stationmaxr.Text).ToString() + "," + (edt_stationminr.Text).ToString() +
                "," + (edt_ratedn.Text).ToString() + "," + (edt_guaranteedn.Text).ToString() +
                "," + (edt_kvalue.Text).ToString() + "," + (edt_floodlimitz.Text).ToString() +
                "," + (edt_floodmaxz.Text).ToString() + "," + (edt_designz.Text).ToString() +
                "," + (edt_checkz.Text).ToString() + "," + (edt_deadv.Text).ToString() +
                "," + (edt_activev.Text).ToString() + "," + (edt_totalv.Text).ToString() + "," + (1).ToString() + ")";
            OracleCommand cmd = new OracleCommand(sql, con);
            int row = cmd.ExecuteNonQuery();
            con.Close();//关闭数据库连接

            //得到电站ID
            DataTable dt = new DataTable("dt");
            //string dtName = "dt";
            sql = "select stationid,stationname from STATIONINFO where stationname='" + edt_stationName.Text + "' and inuse=1";
            con.Open();//打开数据库连接
            da_UpZV = new OracleDataAdapter(sql, con);
            da_UpZV.Fill(dt);
            con.Close();//关闭数据库连接
            stationId = dt.Rows[0]["stationid"].ToString();
            stationName = dt.Rows[0]["stationname"].ToString();

            //-------------------------保存upZV数据到DB-------------------------------
            //得到OracleDataAdapter，OracleCommandbuilder，没有cmb不能自动生成Update里的sql
            sql = "select * from upzv where stationid = " + stationId + " order by z";
            da_UpZV = new OracleDataAdapter(sql, con);
            OracleCommandBuilder commandbuilder = new OracleCommandBuilder(da_UpZV);
            dt_UpZV.Columns.Add("STATIONID");
            dt_UpZV.Columns.Add("ID");
            for (int i = 0; i < dt_UpZV.Rows.Count; i++)
            {
                dt_UpZV.Rows[i]["STATIONID"] = stationId;
            }
            //提交
            da_UpZV.Update(dt_UpZV);
            dt_UpZV.AcceptChanges();//确定表的状态

            //刷新
            dt_UpZV.Clear();
            da_UpZV.Fill(dt_UpZV);

            //-------------------------保存DownRZ数据到DB-------------------------------
            //得到OracleDataAdapter，OracleCommandbuilder，没有cmb不能自动生成Update里的sql
            sql = "select * from DOWNRZ where stationid = " + stationId + " order by z";
            da_DownRZ = new OracleDataAdapter(sql, con);
            OracleCommandBuilder commandbuilder1 = new OracleCommandBuilder(da_DownRZ);
            dt_DownRZ.Columns.Add("STATIONID");
            dt_DownRZ.Columns.Add("ID");
            for (int i = 0; i < dt_DownRZ.Rows.Count; i++)
            {
                dt_DownRZ.Rows[i]["STATIONID"] = stationId;
            }
            //提交
            da_DownRZ.Update(dt_DownRZ);
            dt_DownRZ.AcceptChanges();//确定表的状态

            //刷新
            dt_DownRZ.Clear();
            da_DownRZ.Fill(dt_UpZV);
            //----------------------------------------------------------------------

            this.Close();//关闭窗体

            #region 给新增电站制作计划赋初值time&spatial
            ///* 初始化该电站约束信息 及初始条件*/
            //// 长期一年按月默认约束
            //for (int i = 0; i < 12; i++)
            //{
            //    sql = "insert into DEFAULTCONDITION_TIME(STATIONID,TIMENUM,PLANTYPE) values("
            //    + stationId + "," + (i + 1).ToString() + "," + (0).ToString() + ")";
            //    OracleHelper.ExecuteSql(sql);
            //}
            //// 长期一年按旬默认约束
            //for (int i = 0; i < 36; i++)
            //{
            //    sql = "insert into DEFAULTCONDITION_TIME(STATIONID,TIMENUM,PLANTYPE) values("
            //    + stationId + "," + (i + 1).ToString() + "," + (1).ToString() + ")";
            //    OracleHelper.ExecuteSql(sql);
            //}

            //// 中期调度
            //for (int i = 0; i < 1; i++)
            //{
            //    sql = "insert into DEFAULTCONDITION_TIME(STATIONID,TIMENUM,PLANTYPE) values("
            //    + stationId + "," + (i).ToString() + "," + (2).ToString() + ")";
            //    OracleHelper.ExecuteSql(sql);
            //}

            //// 短期调度
            //for (int i = 0; i < 96; i++)
            //{
            //    sql = "insert into DEFAULTCONDITION_TIME(STATIONID,TIMENUM,PLANTYPE) values("
            //    + stationId + "," + (i + 1).ToString() + "," + (3).ToString() + ")";
            //    OracleHelper.ExecuteSql(sql);
            //}

            ////设置默认约束值

            //sql = "update DEFAULTCONDITION_TIME set AREAINFLOW=0,MAXZ= " + (edt_normalz.Text).ToString()
            //+ ", MINZ= " + (edt_deadz.Text).ToString() + ",MAXR= " + (edt_stationmaxr.Text).ToString()
            //+ ", MINR= " + (edt_stationminr.Text).ToString() + ",MINN= 0 ,MAXN= " + (edt_guaranteedn.Text).ToString()
            //+ ", TYPICALN = " + (edt_guaranteedn.Text).ToString() + ", MAXDELTAZ=1, MAXDELTAR=500 where stationid = " + stationId;
            //OracleHelper.ExecuteSql(sql);

            ////设置初始条件
            ////List<double> tempstartz = new List<double>();
            ////List<double> tempendz = new List<double>();
            ////List<double> temptotale = new List<double>();
            //double[] tempstartz = new double[4];
            //double[] tempendz = new double[4];
            //double[] temptotale = new double[4];
            //for (int i = 0; i < 4; i++)
            //{
            //    if (i == 3)
            //    {
            //        temptotale[i] = double.Parse(edt_guaranteedn.Text) * 24.0;
            //    }
            //    else
            //    {
            //        temptotale[i] = 0;
            //    }

            //    if (cbb_adjust.SelectedIndex > 5)
            //    {
            //        tempstartz[i] = double.Parse(edt_normalz.Text);
            //        tempendz[i] = double.Parse(edt_normalz.Text);
            //    }
            //    else if (cbb_adjust.SelectedIndex > 3)
            //    {
            //        if (i == 0 || i == 1)
            //        {
            //            tempstartz[i] = double.Parse(edt_normalz.Text);
            //            tempendz[i] = double.Parse(edt_normalz.Text);
            //        }
            //        else if (i == 2)
            //        {
            //            tempstartz[i] = double.Parse(edt_deadz.Text);
            //            tempendz[i] = double.Parse(edt_deadz.Text);
            //        }
            //        else
            //        {
            //            tempstartz[i] = double.Parse(edt_normalz.Text) * 0.5 + double.Parse(edt_deadz.Text) * 0.5;
            //            tempendz[i] = double.Parse(edt_normalz.Text) * 0.5 + double.Parse(edt_deadz.Text) * 0.5;
            //        }
            //    }
            //    else
            //    {
            //        if (i == 0 || i == 1)
            //        {
            //            tempstartz[i] = double.Parse(edt_deadz.Text);
            //            tempendz[i] = double.Parse(edt_deadz.Text);
            //        }
            //        else
            //        {
            //            tempstartz[i] = double.Parse(edt_normalz.Text) * 0.5 + double.Parse(edt_deadz.Text) * 0.5;
            //            tempendz[i] = double.Parse(edt_normalz.Text) * 0.5 + double.Parse(edt_deadz.Text) * 0.5;
            //        }
            //    }

            //}
            //for (int i = 0; i < 4; i++)
            //{

            //    sql = "insert into DEFAULTCONDITION_SPATIAL(STATIONID,STARTZ,ENDZ,PLANTYPE,TOTALE) values("
            //    + stationId + "," + (tempstartz[i]).ToString() + "," + (tempendz[i]).ToString()
            //    + "," + (i).ToString() + "," + (temptotale[i]).ToString() + ")";
            //    OracleHelper.ExecuteSql(sql);
            //}
            #endregion

            //----------------------------------------------------------------------
            if (MyEvent != null)
                MyEvent();//引发事件,DB写完，将对应电站图标放在地图上
        }



        //水位库容增加行
        private void button6_Click(object sender, EventArgs e)
        {
            DataRow dr = dt_UpZV.NewRow();
            
           // dt_UpZV.Rows.Add(dr);
            dt_UpZV .Rows.InsertAt(dr,10);
           // dt_UpZV .Rows.
            //todo:折中
            //bs_UpZV.MoveLast();
        }

        //水位库容删除行
        private void button1_Click(object sender, EventArgs e)
        {
            if (dgv_UpZV.RowCount > 0)
            {
                    int i = dgv_UpZV.CurrentRow.Index;
                    dt_UpZV.Rows[i].Delete();
               
            }
        }

        //尾水添加序号事件
        private void dataGridView2_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            try
            {
                //添加行号 
                SolidBrush v_SolidBrush = new SolidBrush(dgv_DownRZ.RowHeadersDefaultCellStyle.ForeColor);
                int v_LineNo = 0;
                v_LineNo = e.RowIndex + 1;

                string v_Line = v_LineNo.ToString();

                e.Graphics.DrawString(v_Line, e.InheritedRowStyle.Font, v_SolidBrush, e.RowBounds.Location.X + 15, e.RowBounds.Location.Y + 5);

            }
            catch (Exception ex)
            {
                MessageBox.Show("添加行号时发生错误，错误信息：" + ex.Message, "操作失败");
            }
        }

        //尾水增加行
        private void button9_Click(object sender, EventArgs e)
        {
            BindingSource bs_DownRz = new BindingSource();
            bs_DownRz.DataSource = dt_DownRZ;
            dgv_DownRZ.DataSource = bs_DownRz;
            DataRow dr = dt_DownRZ.NewRow();
            dt_DownRZ.Rows.Add(dr);
           // dgv_DownRZ.SortedColumn = dgv_DownRZ.Columns[dgv_DownRZ.Columns[0].Name];
            //dt_DownRZ.Rows.;
            //dr["stationid"] = stationId;
           // dt_DownRZ.Rows.Add(dr);
           // dt_DownRZ.Rows.RemoveAt(5);
            //bs_DownRz.MoveLast();  
            //bs_DownRz.Remove()
        }


        //尾水删除行
        private void button10_Click(object sender, EventArgs e)
        {
            if (dgv_DownRZ.RowCount > 0)
            {
               // int i = dgv_DownRZ.SelectedCells
                dt_DownRZ .Rows .RemoveAt (dgv_DownRZ .SelectedRows [0].Index);

            }
        }


        //机组添加序号事件
        private void dataGridView4_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            try
            {
                //添加行号 
                SolidBrush v_SolidBrush = new SolidBrush(dgv_Plant.RowHeadersDefaultCellStyle.ForeColor);
                int v_LineNo = 0;
                v_LineNo = e.RowIndex + 1;

                string v_Line = v_LineNo.ToString();

                e.Graphics.DrawString(v_Line, e.InheritedRowStyle.Font, v_SolidBrush, e.RowBounds.Location.X + 15, e.RowBounds.Location.Y + 5);

            }
            catch (Exception ex)
            {
                MessageBox.Show("添加行号时发生错误，错误信息：" + ex.Message, "操作失败");
            }
        }

        private void dgv_PlantType_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
             try
            {
                //添加行号 
                SolidBrush v_SolidBrush = new SolidBrush(dgv_PlantType.RowHeadersDefaultCellStyle.ForeColor);
                int v_LineNo = 0;
                v_LineNo = e.RowIndex + 1;

                string v_Line = v_LineNo.ToString();

                e.Graphics.DrawString(v_Line, e.InheritedRowStyle.Font, v_SolidBrush, e.RowBounds.Location.X + 15, e.RowBounds.Location.Y + 5);

            }
            catch (Exception ex)
            {
                MessageBox.Show("添加行号时发生错误，错误信息：" + ex.Message, "操作失败");
            }
        }
        /*
         * 尾水添加行
         * */
        private void 添加行ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            BindingSource bs_DownRz = new BindingSource();
            bs_DownRz.DataSource = dt_DownRZ;
            dgv_DownRZ.DataSource = bs_DownRz;
            DataRow dr = dt_DownRZ.NewRow();
            dt_DownRZ.Rows.Add(dr);
        }

        private void dgv_DownRZ_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            //e.RowIndex
            // dgv_DownRZ .Rows[e.RowIndex ].// .Remove(e.RowIndex);
        }
        /*
         * 尾水删除行
         * */
        private void 删除行ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            dt_DownRZ.Rows.RemoveAt(dgv_DownRZ.SelectedRows[0].Index);
        }
       
        /*
         * 水库添加行
         * */
        private void 添加ToolStripMenuItem_Click(object sender, EventArgs e)
        {

            DataRow dr = dt_UpZV.NewRow();

            // dt_UpZV.Rows.Add(dr);
            dt_UpZV.Rows.InsertAt(dr, 10);
        }

        private void 删除ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            dt_UpZV.Rows.RemoveAt(dgv_UpZV.SelectedRows[0].Index);
        }
    }
}
