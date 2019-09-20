using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json.Linq;
using System.IO;

namespace GFBattleTester
{
    public partial class EquipSelecter : Form
    {
        string equip = string.Empty;
        int equipCount = 0;
        int selectedEquipID = 0;
        List<string> equipID = new List<string>();
        List<string> equipDest = new List<string>();
        List<string> equipStatDest = new List<string>();
        List<string> equipName = new List<string>();
        JArray equipInfoJson = JArray.Parse(File.ReadAllText(@"data/json/equip.json"));
        JObject resultJson = new JObject();
        public delegate void FormSendDataHandler(JObject json, string name, bool clear);
        public event FormSendDataHandler FormSendEvent;
        private string Form2_value;
        public string Passvalue
        {
            get { return this.Form2_value; }
            set { this.Form2_value = value; }
        }

        public EquipSelecter()
        {
            InitializeComponent();

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

                    if (Convert.ToInt32(((ListViewItem)x).SubItems[col].Text) > Convert.ToInt32(((ListViewItem)y).SubItems[col].Text))
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
        private void listView1_ColumnClick(object sender, ColumnClickEventArgs e)
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

        private void EquipSelecter_Load(object sender, EventArgs e)
        {
            //MessageBox.Show(Passvalue);
            Text = Passvalue.Substring(0, 1) + "번 인형 " + Passvalue.Substring(1, 1) + "번 장비 선택중";
            equip = Encoding.UTF8.GetString(Convert.FromBase64String(File.ReadAllText("data/info_texts/equip.b64")));
            string[] temp = equip.Split(new[] { Environment.NewLine }, StringSplitOptions.None);
            foreach (string a in temp)
            {
                if (a.Split(',')[0].Split('-')[1].StartsWith("1"))
                {
                    equipID.Add(int.Parse(a.Split(',')[0].Split('-')[1].Substring(5)).ToString());
                    equipName.Add(a.Split(',')[1]);
                }
                else if (a.Split(',')[0].Split('-')[1].StartsWith("3"))
                {
                    string o = a.Split(',')[1].Replace("//c", ",");
                    equipDest.Add(o);
                }
                else if (a.Split(',')[0].Split('-')[1].StartsWith("2"))
                {
                    string o = a.Split(',')[1].Replace("//n", Environment.NewLine).Replace("$", Environment.NewLine).Replace("//c", ",");
                    equipStatDest.Add(o);
                }
            }
            equipCount = equipID.Count;
            for (int i = 0; i < equipCount; i++)
            {
                string rank = string.Empty;
                string type = string.Empty;
                string only = string.Empty;
                foreach (var a in equipInfoJson)
                {
                    JObject tmp = new JObject((JObject)a);
                    if (tmp["id"].ToString() == equipID[i])
                    {
                        rank = getRank(tmp["rank"].ToString());
                        type = tmp["type"].ToString();
                        if (tmp.ContainsKey("fitGuns"))
                        {
                            JArray fit = JArray.Parse(tmp["fitGuns"].ToString());
                            for (int j = 0; j < fit.Count; j++)
                            {
                                if (j == 0)
                                    only += getGunName(int.Parse(fit[j].ToString()));
                                else
                                    only = only + ", " + getGunName(int.Parse(fit[j].ToString()));
                            }
                        }
                    }
                }
                if (type == string.Empty)
                    type = "정보없음";
                if (rank == string.Empty)
                    rank = "정보없음";

                string[] equipItems = { equipID[i], equipName[i], type, rank, only };
                ListViewItem itm = new ListViewItem(equipItems);
                listView1.Items.Add(itm);
            }
            foreach (string a in equipName)
            {
                if (a == Passvalue.Split(';')[1])
                {
                    listView1.Items[equipName.IndexOf(a)].Focused = true;
                    listView1.Items[equipName.IndexOf(a)].Selected = true;
                    listView1.EnsureVisible(equipName.IndexOf(a));
                    break;
                }
            }
        }
        string getRank(string rank)
        {
            if (rank == "2")
                return "★★";
            else if (rank == "3")
                return "★★★";
            else if (rank == "4")
                return "★★★★";
            else if (rank == "5")
                return "★★★★★";
            else
                return "-";
        }
        string getGunName(int id)
        {
            for (int i = 0; i < Form1.frm.gun_id.Count; i++)
            {
                if (id == Form1.frm.gun_id[i])
                {
                    if (id / 20000 < 2 && id / 20000 >= 1)
                        return "개조 " + Form1.frm.gunName[i];
                    else
                        return Form1.frm.gunName[i];
                }
            }
            return "-";
        }
        private void button1_Click(object sender, EventArgs e)
        {
            JObject ret = new JObject();
            int selectedIDindex = Array.IndexOf(equipID.ToArray(), selectedEquipID.ToString());
            ret.Add("id", Passvalue.Split(';')[0]);
            ret.Add("user_id", "20139");
            ret.Add("gun_with_user_id", "0");
            ret.Add("equip_id", selectedEquipID.ToString());
            ret.Add("equip_exp", "0");
            ret.Add("equip_level", numericUpDown1.Value.ToString());
            ret.Add("pow", (numericUpDown2.Value * 100).ToString());
            ret.Add("hit", (numericUpDown2.Value * 100).ToString());
            ret.Add("dodge", (numericUpDown2.Value * 100).ToString());
            ret.Add("speed", (numericUpDown2.Value * 100).ToString());
            ret.Add("rate", (numericUpDown2.Value * 100).ToString());
            ret.Add("critical_harm_rate", (numericUpDown2.Value * 100).ToString());
            ret.Add("critical_percent", (numericUpDown2.Value * 100).ToString());
            ret.Add("armor_piercing", (numericUpDown2.Value * 100).ToString());
            ret.Add("armor", (numericUpDown2.Value * 100).ToString());
            ret.Add("shield", (numericUpDown2.Value * 100).ToString());
            ret.Add("damage_amplify", (numericUpDown2.Value * 100).ToString());
            ret.Add("damage_reduction", (numericUpDown2.Value * 100).ToString());
            ret.Add("night_view_percent", (numericUpDown2.Value * 100).ToString());
            ret.Add("bullet_number_up", (numericUpDown2.Value * 100).ToString());
            ret.Add("adjust_count", "0");
            ret.Add("is_locked", "0");
            ret.Add("last_adjust", "");
            this.FormSendEvent(ret, equipName[selectedIDindex] + ";" + Passvalue, false);
            Close();
        }
        private void eqClear(object sender, EventArgs e)
        {
            JObject ret = new JObject();       
            ret.Add("id", Passvalue.Split(';')[0]);
            ret.Add("user_id", "20139");
            ret.Add("gun_with_user_id", "0");
            ret.Add("equip_id", "1");
            ret.Add("equip_exp", "0");
            ret.Add("equip_level", "0");
            ret.Add("pow", "0");
            ret.Add("hit", "0");
            ret.Add("dodge", "0");
            ret.Add("speed", "0");
            ret.Add("rate", "0");
            ret.Add("critical_harm_rate", "0");
            ret.Add("critical_percent", "0");
            ret.Add("armor_piercing", "0");
            ret.Add("armor", "0");
            ret.Add("shield", "0");
            ret.Add("damage_amplify", "0");
            ret.Add("damage_reduction", "0");
            ret.Add("night_view_percent", "0");
            ret.Add("bullet_number_up", "0");
            ret.Add("adjust_count", "0");
            ret.Add("is_locked", "0");
            ret.Add("last_adjust", "");
            FormSendEvent(ret, ";" + Passvalue, true);
            Close();
        }
       
        private void listView1_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count == 1)
            {
                ListView.SelectedListViewItemCollection items = listView1.SelectedItems;
                ListViewItem subitem = items[0];
                groupBox1.Text = subitem.SubItems[1].Text;
                button1.Enabled = true;
                selectedEquipID = int.Parse(subitem.SubItems[0].Text);
                for (int i = 0; i < equipID.Count; i++)
                {
                    if (equipID[i].ToString() == subitem.SubItems[0].Text)
                    {
                        label4.Text = equipDest[i];
                        label5.Text = equipStatDest[i];
                        break;
                    }
                }
                for (int i = 0; i < equipInfoJson.Count; i++)
                {
                    if (equipInfoJson[i]["id"].ToString() == subitem.SubItems[0].Text)
                    {
                        label5.Text = getStatDataFromString(label5.Text, (JObject)equipInfoJson[i]["stats"]);
                        break;
                    }
                }
            }
        }
        string getStatDataFromString(string s, JObject stats)
        {
            string result = s;
            int lv = (int)numericUpDown1.Value;
            //foreach(var a in stats.Properties())
            {
                if (s.Contains("<hit>"))
                {
                    int min = int.Parse(stats["hit"]["min"].ToString());
                    int max = int.Parse(stats["hit"]["max"].ToString());
                    int upgrade = stats["hit"].SelectToken("upgrade") != null ? int.Parse(stats["hit"]["upgrade"].ToString()) : 0;
                    s = s.Replace("<hit>", " " + calcStats(lv, min, upgrade) + "~" + calcStats(lv, max, upgrade));
                }
                if (s.Contains("<dodge>"))
                {
                    int min = int.Parse(stats["dodge"]["min"].ToString());
                    int max = int.Parse(stats["dodge"]["max"].ToString());
                    int upgrade = stats["dodge"].SelectToken("upgrade") != null ? int.Parse(stats["dodge"]["upgrade"].ToString()) : 0;
                    s = s.Replace("<dodge>", " " + calcStats(lv, min, upgrade) + "~" + calcStats(lv, max, upgrade));
                }
                if (s.Contains("<pow>"))
                {
                    int min = int.Parse(stats["pow"]["min"].ToString());
                    int max = int.Parse(stats["pow"]["max"].ToString());
                    int upgrade = stats["pow"].SelectToken("upgrade") != null ? int.Parse(stats["pow"]["upgrade"].ToString()) : 1;
                    s = s.Replace("<pow>", " " + calcStats(lv, min, upgrade) + "~" + calcStats(lv, max, upgrade));
                }
                if (s.Contains("<rate>"))
                {
                    int min = int.Parse(stats["rate"]["min"].ToString());
                    int max = int.Parse(stats["rate"]["max"].ToString());
                    int upgrade = stats["rate"].SelectToken("upgrade") != null ? int.Parse(stats["rate"]["upgrade"].ToString()) : 0;
                    s = s.Replace("<rate>", " " + calcStats(lv, min, upgrade) + "~" + calcStats(lv, max, upgrade));
                }
                if (s.Contains("<armor_piercing>"))
                {
                    int min = int.Parse(stats["armorPiercing"]["min"].ToString());
                    int max = int.Parse(stats["armorPiercing"]["max"].ToString());
                    int upgrade = stats["armorPiercing"].SelectToken("upgrade") != null ? int.Parse(stats["armorPiercing"]["upgrade"].ToString()) : 0;
                    s = s.Replace("<armor_piercing>", " " + calcStats(lv, min, upgrade) + "~" + calcStats(lv, max, upgrade));
                }
                if (s.Contains("<night_view_percent>"))
                {
                    int min = int.Parse(stats["nightview"]["min"].ToString());
                    int max = int.Parse(stats["nightview"]["max"].ToString());
                    int upgrade = stats["nightview"].SelectToken("upgrade") != null ? int.Parse(stats["nightview"]["upgrade"].ToString()) : 0;
                    s = s.Replace("<night_view_percent>", " " + calcStats(lv, min, upgrade) + "~" + calcStats(lv, max, upgrade));
                }
                if (s.Contains("<critical_percent>"))
                {
                    int min = int.Parse(stats["criticalPercent"]["min"].ToString());
                    int max = int.Parse(stats["criticalPercent"]["max"].ToString());
                    int upgrade = stats["criticalPercent"].SelectToken("upgrade") != null ? int.Parse(stats["criticalPercent"]["upgrade"].ToString()) : 0;
                    s = s.Replace("<critical_percent>", " " + calcStats(lv, min, upgrade) + "~" + calcStats(lv, max, upgrade));
                }
                if (s.Contains("<critical_harm_rate>"))
                {
                    int min = int.Parse(stats["criticalHarmRate"]["min"].ToString());
                    int max = int.Parse(stats["criticalHarmRate"]["max"].ToString());
                    int upgrade = stats["criticalHarmRate"].SelectToken("upgrade") != null ? int.Parse(stats["criticalHarmRate"]["upgrade"].ToString()) : 0;
                    s = s.Replace("<critical_harm_rate>", " " + calcStats(lv, min, upgrade) + "~" + calcStats(lv, max, upgrade));
                }
                if (s.Contains("<armor_piercing>"))
                {
                    int min = int.Parse(stats["armorPiercing"]["min"].ToString());
                    int max = int.Parse(stats["armorPiercing"]["max"].ToString());
                    int upgrade = stats["armorPiercing"].SelectToken("upgrade") != null ? int.Parse(stats["armorPiercing"]["upgrade"].ToString()) : 0;
                    s = s.Replace("<armor_piercing>", " " + calcStats(lv, min, upgrade) + "~" + calcStats(lv, max, upgrade));
                }
                if (s.Contains("<armor>"))
                {
                    int min = int.Parse(stats["armor"]["min"].ToString());
                    int max = int.Parse(stats["armor"]["max"].ToString());
                    int upgrade = stats["armor"].SelectToken("upgrade") != null ? int.Parse(stats["armor"]["upgrade"].ToString()) : 0;
                    s = s.Replace("<armor>", " " + calcStats(lv, min, upgrade) + "~" + calcStats(lv, max, upgrade));
                }
                if (s.Contains("<bullet_number_up>"))
                {
                    int min = int.Parse(stats["bullet"]["min"].ToString());
                    int max = int.Parse(stats["bullet"]["max"].ToString());
                    int upgrade = stats["bullet"].SelectToken("upgrade") != null ? int.Parse(stats["bullet"]["upgrade"].ToString()) : 0;
                    s = s.Replace("<bullet_number_up>", " " + calcStats(lv, min, upgrade) + "~" + calcStats(lv, max, upgrade));
                }
                if (s.Contains("<speed>"))
                {
                    int min = int.Parse(stats["speed"]["min"].ToString());
                    int max = int.Parse(stats["speed"]["max"].ToString());
                    int upgrade = stats["speed"].SelectToken("upgrade") != null ? int.Parse(stats["speed"]["upgrade"].ToString()) : 0;
                    s = s.Replace("<speed>", " " + calcStats(lv, min, upgrade) + "~" + calcStats(lv, max, upgrade));
                }
                return s;
            }
        }
        string calcStats(int lv, int stat, int upgrade)
        {
            return Math.Floor((decimal)stat * (10000 + (decimal)lv * (decimal)upgrade) / 10000).ToString();
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {

        }
    }
}
