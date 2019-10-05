using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Collections;
using System.Runtime.InteropServices;

namespace GFBattleTester
{
    public partial class battleResultViewer : Form
    {
        int lastfcount = 0;
        public delegate void FormSendDataHandler(JObject json);
        public event FormSendDataHandler FormSendEvent;
        JObject enemyInfo = new JObject();
        JArray Record = new JArray();
        List<string> files = new List<string>();
        static int[] gun_position = { -1, 7, 12, 17, 8, 13, 18, 9, 14, 19 };
        static int[] gun_pos_in_record = { -1, 1, 4, 7, 2, 5, 8, 3, 6, 9 };
        
        private JObject Form2_value;
        public JObject Passvalue
        {
            get { return this.Form2_value; }
            set { this.Form2_value = value; }
        }
        public battleResultViewer()
        {
            InitializeComponent();
        }

        private void BattleResultViewer_Load(object sender, EventArgs e)
        {
            this.Text = Form1.frm.lang_data["brv_title"].ToString();
            listView2.Columns[0].Text = Form1.frm.lang_data["brv_date_writed"].ToString();
            listView2.Columns[1].Text = Form1.frm.lang_data["brv_leader"].ToString();
            listView2.Columns[2].Text = Form1.frm.lang_data["brv_MVP"].ToString();
            listView2.Columns[3].Text = Form1.frm.lang_data["brv_battle_status"].ToString();
            listView2.Columns[4].Text = Form1.frm.lang_data["brv_sally_num"].ToString();
            listView2.Columns[5].Text = Form1.frm.lang_data["brv_battle_time"].ToString();
            listView2.Columns[6].Text = Form1.frm.lang_data["brv_enemy_leader"].ToString();
            listView2.Columns[7].Text = Form1.frm.lang_data["brv_enemy_id"].ToString();
            listView2.Columns[8].Text = Form1.frm.lang_data["brv_max_damage_to_enemy"].ToString();
            listView2.Columns[9].Text = Form1.frm.lang_data["brv_max_damage_from_enemy"].ToString();
            listView2.Columns[10].Text = Form1.frm.lang_data["brv_total_damage_to_enemy"].ToString();
            listView2.Columns[11].Text = Form1.frm.lang_data["brv_filename"].ToString();
            listView1.Columns[0].Text = Form1.frm.lang_data["brv_time"].ToString();
            listView1.Columns[1].Text = Form1.frm.lang_data["brv_frame"].ToString();
            listView1.Columns[2].Text = Form1.frm.lang_data["brv_target"].ToString();
            listView1.Columns[3].Text = Form1.frm.lang_data["brv_action"].ToString();
            listView1.Columns[4].Text = Form1.frm.lang_data["brv_current_pos"].ToString();
            listView3.Columns[0].Text = Form1.frm.lang_data["brv_start_pos"].ToString();
            listView3.Columns[1].Text = Form1.frm.lang_data["brv_gun_name"].ToString();
            listView3.Columns[2].Text = Form1.frm.lang_data["brv_hp"].ToString();
            listView3.Columns[3].Text = Form1.frm.lang_data["brv_hp_after"].ToString();
            listView3.Columns[4].Text = Form1.frm.lang_data["brv_hp_damaged"].ToString();
            label1.Text = Form1.frm.lang_data["brv_formation_info"].ToString();
            enemyInfo = JObject.Parse(File.ReadAllText(@"data/json/enemy_character_type_info.json"));
            timer1.Start();
            LoadFile();

        }
        void LoadFile()
        {
            listView2.Items.Clear();
            string[] brvfFiles = Directory.GetFiles(@"BattleLog", "*.brvf");
            lastfcount = brvfFiles.Length;
            foreach (string fname in brvfFiles)
            {
                JObject brvf = JObject.Parse(File.ReadAllText(@"BattleLog/" + Path.GetFileName(fname)));
                JArray guns = new JArray((JArray)brvf["gun_info"]);
                string date = ConvertFromUnixTimestamp(double.Parse(brvf["battleEndtime"].ToString())).ToString("yyyy-MM-dd HH:mm:ss");
                string leader = Form1.frm.gunName[Form1.frm.gun_id.IndexOf(int.Parse(guns[0]["gunid"].ToString()))];
                string mvp = Form1.frm.gunName[Form1.frm.gun_id.IndexOf(int.Parse(guns[int.Parse(brvf["mvp"].ToString()) - 1]["gunid"].ToString()))]; 
                string battleresult = bool.Parse( brvf["enemyDie"].ToString()) ? Form1.frm.lang_data["brv_win"].ToString() : Form1.frm.lang_data["brv_lose"].ToString();
                int battledNum = 0;
                string groupID = brvf["EnemyGroupID"].ToString();
                string enemyLeader = brvf["EnemyLeaderID"].ToString() == "0"?"-" : enemyInfo[brvf["EnemyLeaderID"].ToString()]["code"].ToString();
                string maxDamage = brvf["MaxDamageToEnemy"].ToString();
                string totalDamageFromEnemy = brvf["totalDamageFromEnemy"].ToString();
                string totalDamageToEnemy = brvf["totalDamageToEnemy"].ToString();
                string filename = Path.GetFileName(fname);
                string battletime = brvf["battleTime"].ToString() + Form1.frm.lang_data["brv_second"].ToString();
                
                foreach(var u in guns)
                {
                    
                    if (u["isUsed"].ToString() == "1")
                    {
                        
                        battledNum++;
                    }
                }
                string[] add = {date,leader,mvp,battleresult,battledNum.ToString(),battletime,enemyLeader,groupID,maxDamage,totalDamageFromEnemy,totalDamageToEnemy,filename };
                ListViewItem itm = new ListViewItem(add);
                listView2.Items.Add(itm);
            } 
            
            


        }
        static DateTime ConvertFromUnixTimestamp(double timestamp)
        {
            DateTime origin = new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return origin.AddSeconds(timestamp);
        }
        private void ListBox1_Click(object sender, EventArgs e)
        {
           
        }

        private void ListBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void ListView2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listView2.SelectedItems.Count == 1)
            {
                ListView.SelectedListViewItemCollection items = listView2.SelectedItems;
                ListViewItem subitem = items[0];
                listView1.Items.Clear();
                listView3.Items.Clear();
                for(int i = 1; i <= 9; i++)
                {
                    Button pbtn = (Button)Controls.Find("p"+i.ToString(),true)[0];
                    ProgressBar hp = (ProgressBar)Controls.Find("hpbar" + i.ToString(), true)[0];
                    pbtn.Enabled = false;
                    pbtn.BackColor = Color.Transparent;
                    pbtn.Text = string.Empty;
                    hp.Value = 0;

                }

                JObject brvf = JObject.Parse(File.ReadAllText(@"BattleLog/" + subitem.SubItems[11].Text));
                JArray rec = new JArray((JArray)brvf["rec"]);
                foreach(var z in brvf["gun_info"])
                {
                    if(z["isUsed"].ToString() == "1")
                    {
                        string[] items2 = {Array.IndexOf(gun_position,int.Parse(z["pos"].ToString())).ToString(), Form1.frm.gunName[Form1.frm.gun_id.IndexOf(int.Parse(z["gunid"].ToString()))]
                        ,z["lifeBefore"].ToString(),z["life"].ToString(),(int.Parse(z["lifeBefore"].ToString())-int.Parse(z["life"].ToString())).ToString()};
                        ListViewItem itms = new ListViewItem(items2);
                        listView3.Items.Add(itms);
                        Button posbtn = (Button)Controls.Find("p"+items2[0],true)[0];
                        ProgressBar hpbar = (ProgressBar)Controls.Find("hpbar" + items2[0], true)[0];
                        hpbar.Maximum = int.Parse(z["lifeBefore"].ToString()) != -1 ? int.Parse(z["lifeBefore"].ToString()) : 0;
                        try
                        {
                            hpbar.Value = int.Parse(z["life"].ToString()) != -1 ? int.Parse(z["life"].ToString()) : 0;
                        }
                        catch
                        {
                            hpbar.Value = hpbar.Maximum;
                        }
                        posbtn.Text = items2[1]+Environment.NewLine+"HP: "+ items2[2] +Environment.NewLine + Form1.frm.lang_data["brv_hp_damaged"].ToString() + ": "+ items2[4];
                        double damage_percent = double.Parse(items2[4]) / double.Parse(items2[2]) * 100.0;
                        if (damage_percent < 50.0 && damage_percent >= 0.0)
                        {                           
                            posbtn.BackColor = ColorTranslator.FromHtml("#00aeff");
                            //ModifyProgressBarColor.SetState(hpbar, 1);
                        }
                        else if(damage_percent > 50.0 && damage_percent <75.0)
                        {
                            posbtn.BackColor = ColorTranslator.FromHtml("#ffcc00");
                            //ModifyProgressBarColor.SetState(hpbar, 2);
                        }
                        else if(damage_percent >= 75.0 && damage_percent < 100.0)
                        {
                            posbtn.BackColor = ColorTranslator.FromHtml("#ff4d00");
                           // ModifyProgressBarColor.SetState(hpbar, 3);
                        }
                        else
                        {
                            posbtn.BackColor = Color.Red;
                        }
                    }
                }
                foreach (var a in rec)
                {
                    int[] currentpos = new int[5];
                    int remaingun = 0;
                    foreach (var p in brvf["gun_info"])
                    {
                        if (p["isUsed"].ToString() == "1")
                        {

                           
                            remaingun++;
                        }

                    }
                    string record = a.ToString();
                    string[] item = new string[6];
                    int currentGun = 0;
                    item[0] = Convert.ToDouble(double.Parse(record.Split(',')[0]) / 27.7).ToString("F3");
                    item[1] = record.Split(',')[0].ToString();

                    //Console.Write("Time: " + Convert.ToDouble(double.Parse(record.Split(',')[0]) / 26.4).ToString("F3"));
                    if (record.Split(',')[1] == "0")
                    {
                        item[2] = "-";
                        if (record.Split(',')[2] == "2")
                        {
                            if (record.Split(',')[3] == "0")
                            {
                                //Console.WriteLine(",Toggle AutoSkill Off");
                                item[3] = Form1.frm.lang_data["brv_autoskill_off"].ToString();

                            }

                            else
                            {
                                //Console.WriteLine(",Toggle AutoSkill On");
                                item[3] = Form1.frm.lang_data["brv_autoskill_on"].ToString();
                            }
                            item[4] = "-";
                        }
                        else
                        {
                            item[2] = "-";
                            item[3] = Form1.frm.lang_data["brv_unknown_action"].ToString();
                            item[4] = "-";

                            Console.WriteLine(",Unknown Op");
                        }
                    }
                    else
                    {


                        //Console.Write(",Target: " + Array.IndexOf(gun_pos_in_record, int.Parse(record.Split(',')[1])));
                        foreach (var temp in brvf["gun_info"])
                        {
                            if (temp["id"].ToString() == record.Split(',')[1])
                            {
                                currentGun = int.Parse(temp["id"].ToString()) - 1;
                                currentpos[currentGun] = Array.IndexOf(gun_pos_in_record, int.Parse(record.Split(',')[1]));
                                item[2] = Form1.frm.gunName[Form1.frm.gun_id.IndexOf(int.Parse(temp["gunid"].ToString()))];
                                item[4] = currentpos[currentGun].ToString();
                                break;
                            }
                        }

                        if (record.Split(',')[2] == "1")
                        {
                            item[3] = Form1.frm.lang_data["brv_retire"].ToString();
                            item[4] = "-";
                            remaingun--;
                            // Console.WriteLine(",Retire");
                        }
                        else if (record.Split(',')[2] == "3")
                        {
                            item[3] = Form1.frm.lang_data["brv_use_skill"].ToString();
                            item[4] = currentpos[currentGun].ToString();
                            //Console.WriteLine(", Use Skill");
                        }
                        else if (record.Split(',')[2] == "0")
                        {
                            item[3] = string.Format(Form1.frm.lang_data["brv_pos_move"].ToString(), Array.IndexOf(gun_pos_in_record, int.Parse(record.Split(',')[3])).ToString());
                            currentpos[currentGun] = Array.IndexOf(gun_pos_in_record, int.Parse(record.Split(',')[3]));
                            item[4] = currentpos[currentGun].ToString();
                            //Console.WriteLine(",Moveto: " + Array.IndexOf(gun_pos_in_record, int.Parse(record.Split(',')[3])));
                        }
                    }
                    item[5] = remaingun.ToString();
                    ListViewItem i = new ListViewItem(item);
                    listView1.Items.Add(i);
                }
            }
        }

        private void Timer1_Tick(object sender, EventArgs e)
        {
            string[] brvfFiles = Directory.GetFiles(@"BattleLog", "*.brvf");
            if(lastfcount != brvfFiles.Length)
            {
                LoadFile();
            }
        }

        private void ListView1_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            for (int i = 0; i < listView1.Columns.Count; i++)
            {
                listView1.Columns[i].Text = listView1.Columns[i].Text.Replace(" ▼", "");
                listView1.Columns[i].Text = listView1.Columns[i].Text.Replace(" ▲", "");
            }

            if (listView1.Sorting == SortOrder.Ascending || listView1.Sorting == SortOrder.None)
            {
                listView1.ListViewItemSorter = new ListViewItemComparer(e.Column, "desc");
                listView1.Sorting = SortOrder.Descending;
                listView1.Columns[e.Column].Text = listView1.Columns[e.Column].Text + " ▼";

            }
            else
            {
                listView1.ListViewItemSorter = new ListViewItemComparer(e.Column, "asc");
                listView1.Sorting = SortOrder.Ascending;
                listView1.Columns[e.Column].Text = listView1.Columns[e.Column].Text + " ▲";
            }
            listView1.Sort();
        }

        private void ListView3_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            for (int i = 0; i < listView3.Columns.Count; i++)
            {
                listView3.Columns[i].Text = listView3.Columns[i].Text.Replace(" ▼", "");
                listView3.Columns[i].Text = listView3.Columns[i].Text.Replace(" ▲", "");
            }

            if (listView3.Sorting == SortOrder.Ascending || listView3.Sorting == SortOrder.None)
            {
                listView3.ListViewItemSorter = new ListViewItemComparer(e.Column, "desc");
                listView3.Sorting = SortOrder.Descending;
                listView3.Columns[e.Column].Text = listView3.Columns[e.Column].Text + " ▼";

            }
            else
            {
                listView3.ListViewItemSorter = new ListViewItemComparer(e.Column, "asc");
                listView3.Sorting = SortOrder.Ascending;
                listView3.Columns[e.Column].Text = listView3.Columns[e.Column].Text + " ▲";
            }
            listView3.Sort();
        }

        private void ListView2_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            for (int i = 0; i < listView2.Columns.Count; i++)
            {
                listView2.Columns[i].Text = listView2.Columns[i].Text.Replace(" ▼", "");
                listView2.Columns[i].Text = listView2.Columns[i].Text.Replace(" ▲", "");
            }

            if (listView2.Sorting == SortOrder.Ascending || listView2.Sorting == SortOrder.None)
            {
                listView2.ListViewItemSorter = new ListViewItemComparer(e.Column, "desc");
                listView2.Sorting = SortOrder.Descending;
                listView2.Columns[e.Column].Text = listView2.Columns[e.Column].Text + " ▼";

            }
            else
            {
                listView2.ListViewItemSorter = new ListViewItemComparer(e.Column, "asc");
                listView2.Sorting = SortOrder.Ascending;
                listView2.Columns[e.Column].Text = listView2.Columns[e.Column].Text + " ▲";
            }
            listView2.Sort();
        }
        class ListViewItemComparer : IComparer
        {
            private int col;
            public string sort = "asc";
            public ListViewItemComparer()
            {
                col = 0;
            }
            public ListViewItemComparer(int column, string sort)
            {
                col = column;
                this.sort = sort;
            }
            public int Compare(object x, object y)
            {
                int chk = 1;
                try
                {
                    if (sort == "asc")
                        chk = 1;
                    else
                        chk = -1;

                    if (Convert.ToDecimal(((ListViewItem)x).SubItems[col].Text.Replace("초","")) > Convert.ToDecimal(((ListViewItem)y).SubItems[col].Text.Replace("초","")))
                        return chk;
                    else
                        return -chk;

                }
                catch (Exception)
                {
                    if (sort == "asc")
                        return String.Compare(((ListViewItem)x).SubItems[col].Text, ((ListViewItem)y).SubItems[col].Text);
                    else
                        return String.Compare(((ListViewItem)y).SubItems[col].Text, ((ListViewItem)x).SubItems[col].Text);
                }
            }
        }
    }
    public static class ModifyProgressBarColor
    {
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = false)]
        static extern IntPtr SendMessage(IntPtr hWnd, uint Msg, IntPtr w, IntPtr l);
        public static void SetState(this ProgressBar pBar, int state)
        {
            SendMessage(pBar.Handle, 1040, (IntPtr)state, IntPtr.Zero);
        }
    }
}
