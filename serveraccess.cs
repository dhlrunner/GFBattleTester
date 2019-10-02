using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using System.Windows.Forms;
using System.IO;

namespace GFBattleTester
{
    public partial class serveraccess : Form
    {
        Form1 form1;
        List<string> gun_path = new List<string>();
        List<string> equip_path = new List<string>();
        public serveraccess(Form1 form1)
        {
            InitializeComponent();
            this.form1 = form1;
        }

        private void Serveraccess_Load(object sender, EventArgs e)
        {
            AddLog("클라이언트 접속 대기 중..");
        }
        public void getUserdataFromServer(JObject data)
        {
            AddLog("유저 정보 취득 성공.");
            File.WriteAllText(@"userdata/userinfo/" + DateTime.Now.ToString("yyyyMMdd_HHmmss_") + data["user_info"]["name"].ToString() + ".json", data.ToString());
            JObject guninfo = new JObject();

            JObject sqd = new JObject();
            for (int i = 1; i <= 10; i++)
            {
                string gun_filename = "userdata/presets/" + DateTime.Now.ToString("yyyyMMdd_HHmmss_") + data["user_info"]["name"].ToString() + "_e" + i.ToString()+".gun";
                string eq_filename = "userdata/presets/" + DateTime.Now.ToString("yyyyMMdd_HHmmss_") + data["user_info"]["name"].ToString() + "_e" + i.ToString()+".equip";
                JObject o = new JObject();
                JObject eq = new JObject();
                foreach (var guns in (JArray)data["gun_with_user_info"])
                {

                    if (guns["team_id"].ToString() == i.ToString())
                    {
                        if (guns["location"].ToString() != "0")
                        {
                            List<string> gun_equip_ids = new List<string>();
                            JObject n = new JObject((JObject)guns);

                            n["id"] = guns["location"].ToString();
                            n["team_id"] = "1";
                            n["equip1"] = "0";
                            n["equip2"] = "0";
                            n["equip3"] = "0";

                            for (int uu = 1; uu <= 3; uu++)
                            {
                                if (guns["equip" + uu.ToString()].ToString() != "0")
                                {
                                    gun_equip_ids.Add(guns["equip" + uu.ToString()].ToString());
                                }
                            }
                            o.Add("gun" + guns["location"].ToString(), n);
                            foreach (var keys in data["equip_with_user_info"])
                            {
                                for (int lis = 0; lis < gun_equip_ids.Count; lis++)
                                {
                                    if (gun_equip_ids[lis] == ((JProperty)keys).Name.ToString())
                                    {
                                        JObject t = new JObject((JObject)data["equip_with_user_info"][gun_equip_ids[lis]]);
                                        t["id"] = guns["location"].ToString() + (lis + 1).ToString();
                                        t["gun_with_user_id"] = guns["location"].ToString();
                                        eq.Add(guns["location"].ToString() + (lis + 1).ToString(), t);
                                    }
                                }
                            }

                        }
                    }
                }

                string[] add = { i.ToString(),
                                o["gun1"] !=null ? Form1.frm.gunName[Form1.frm.gun_id.IndexOf(int.Parse(o["gun1"]["gun_id"].ToString()))] : "-",
                                o["gun2"] !=null ? Form1.frm.gunName[Form1.frm.gun_id.IndexOf(int.Parse(o["gun2"]["gun_id"].ToString()))] : "-",
                                 o["gun3"] !=null ? Form1.frm.gunName[Form1.frm.gun_id.IndexOf(int.Parse(o["gun3"]["gun_id"].ToString()))] : "-",
                                  o["gun4"] !=null ? Form1.frm.gunName[Form1.frm.gun_id.IndexOf(int.Parse(o["gun4"]["gun_id"].ToString()))] : "-",
                                   o["gun5"] !=null ? Form1.frm.gunName[Form1.frm.gun_id.IndexOf(int.Parse(o["gun5"]["gun_id"].ToString()))] : "-",
                               Path.GetFileName(gun_filename), o.ToString()
                            };
                ListViewItem itm = new ListViewItem(add);
                listView1.Items.Add(itm);
                gun_path.Add(gun_filename);
                equip_path.Add(eq_filename);
                File.WriteAllText(gun_filename, o.ToString());
                File.WriteAllText(eq_filename, eq.ToString());
            }
            foreach (var keys in data["squad_with_user_info"])
            {
                JObject t = new JObject((JObject)data["squad_with_user_info"][((JProperty)keys).Name.ToString()]);
                t["ammo"] = "1000";
                t["mre"] = "1000";
                sqd.Add("Squad" + data["squad_with_user_info"][((JProperty)keys).Name.ToString()]["squad_id"].ToString(), t);
            }
            sqd.Add("switch1", "1");
            sqd.Add("switch2", "1");
            sqd.Add("switch3", "1");
            sqd.Add("switch4", "1");
            sqd.Add("switch5", "1");
            sqd.Add("switch6", "1");
            File.WriteAllText(@"userdata/presets/" + DateTime.Now.ToString("yyyyMMdd_HHmmss_") + data["user_info"]["name"].ToString() + ".sqd", sqd.ToString());
            AddLog("유저 정보로부터 데이터를 모두 불러왔습니다.");
        }
        public void AddLog(string text)
        {
            logTextbox.AppendText(string.Format("[{0}]{1}" + Environment.NewLine, DateTime.Now.ToString("yyyy.MM.dd HH:mm:ss"), text));

        }

        private void Serveraccess_FormClosed(object sender, FormClosedEventArgs e)
        {

        }

        private void Serveraccess_FormClosing(object sender, CancelEventArgs e)
        {
            form1.getUserinfoFromServer = false;
            form1.button23.Enabled = true;
            e.Cancel = true;
            this.Hide();
        }

        private void ListView1_DoubleClick(object sender, EventArgs e)
        {
            ListView.SelectedListViewItemCollection items = listView1.SelectedItems;
            ListViewItem subitem = items[0];
            Form1.frm.Load_Gun_Info_fromfilepath(gun_path[int.Parse(subitem.SubItems[0].Text)-1]);
            if (checkBox1.Checked)
            {
                Form1.frm.Load_Equip_Info_From_File(equip_path[int.Parse(subitem.SubItems[0].Text) - 1]);
            }
            Close();
        }
    }
}
