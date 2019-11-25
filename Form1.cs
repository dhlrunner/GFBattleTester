using System;
using System.Collections.Generic;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json.Linq;
using System.Net;
using System.Web;
using System.Threading;
using Newtonsoft.Json;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.IO.Compression;
//using SimpleHttpServer;


namespace GFBattleTester
{
    public partial class Form1 : Form
    {
        string battlespot = "983";
        public bool getUserinfoFromServer = false;
        public static Form1 frm;
        serveraccess serv;
        static string signkey = "????";
        string _token = string.Empty;
        bool gunsaved = false;
        int[,] gun_user_id = { { 1, 2, 3, 4, 5 }, { 6, 7, 8, 9, 10 }, { 11, 12, 13, 14, 15 }, { 16, 17, 18, 19, 20 }, { 21, 22, 23, 24, 25 }, { 26, 27, 28, 29, 30 } };
        string[] Equippos = { "11", "12", "13", "21", "22", "23", "31", "32", "33", "41", "42", "43", "51", "52", "53" };
        string equip = string.Empty;
        JObject gun_info_array = JObject.Parse(@"{'0':[],'1':[],'2':[],'3':[],'4':[],'5':[]}");
        JObject gun_info_json_1 = new JObject();
        JObject gun_info_json_2 = new JObject();
        JObject gun_info_json_3 = new JObject();
        JObject gun_info_json_4 = new JObject();
        JObject gun_info_json_5 = new JObject();
        JArray gun_eh_array = new JArray();
        JArray Squad_info = new JArray();
        List<string> equipID = new List<string>();
        List<string> equipDest = new List<string>();
        List<string> equipStatDest = new List<string>();
        List<string> equipName = new List<string>();
        List<string> enemyGrpID = new List<string>();
        public List<string> gun_info = new List<string>();
        List<string> mission_name = new List<string>();
        public List<int> gun_id = new List<int>();
        public List<string> gunName = new List<string>();
        JObject equip_e = new JObject();
        HttpListener listener = new HttpListener();
        Thread thread = new Thread(new ParameterizedThreadStart(WorkerThread));
        public JObject lang_data = new JObject();

        ManualResetEvent _pauseEvent = new ManualResetEvent(false);
        static JObject userinfo = new JObject();
        JObject enemy_team_info = new JObject();
        JObject enemy_character_info = new JObject();
        JObject Theater_data = new JObject();
        JArray gunstatdata = new JArray();
        JObject attribute = new JObject();
        JObject grow = new JObject();
        JObject missionactinfo = new JObject();
        JArray spotactinfo = new JArray();
        JObject setting = new JObject();
        public JObject theater_enemy_info = new JObject();
        //JObject test_userinfo = JObject.Parse(File.ReadAllText("data/json/userinfo_test.json"));
        long[] gun_exp_table = {0,100,300,400,600,1000,1500,2100,2800,3600,4500,5500,6600,7800,9100,10500,
        12000,13600,15300,17100,19000,21000,23000,25300,27600,30000,32500,35100,37900,41000,44400,48600,
        53200,58200,63600,69400,75700,82400,89600,97300,105500,114300,123600,133500,144000,155100,166900,179400,
        192500,206400,221000,236400,252500,269400,287100,305700,325200,345600,366900,389200,412500,436800,462100,
        488400,515800,544300,573900,604700,636700,669900,704300,749400,796200,844800,895200,947400,1001400,1057300,
        1115200,1175000,1236800,1300700,1366700,1434800,1505100,1577700,1652500,1729600,1809100,1891000,1975300,
        2087900,2204000,2323500,2446600,2573300,2703700,2837800,2975700,3117500,3263200,3363200,3483200,3623200,3783200,
        3963200,4163200,4383200,4623200,4983200,5463200,6103200,7003200,8203200,9803200,12803200,16803200,21803200,27803200,32803200};

        long[] sqd_exp_table = { 0, 500,1400,2700,4500,6700,9400,12600,16200,20200,24700,29700,35100,40900,47200,54000,61200,68800,77100,86100,
        95900,106500,118500,132000,147000,163500,181800,201900,223900,247900,274200,302500,333300,366600,402400,441000,482400,526600,574000,624600,
        678400,735700,796500,861000,929200,1001500,1077900,1158400,1243300,1332700,1426800,1525600,1629400,1738300,1852300,1971800,2096700,
        2227200,2363500,2505900,2654400,2809000,2970100,3137800,3312300,3493800,3682300,3877800,4080800,4291400,4509600,4735800,4970000,
        5212500,5463300,5722800,5990800,6267800,6553800,6849300,7154000,7468500,7792500,8127000,8471000,8826000,9191000,9567000,9954000,
        10352000,10761000,11182000,11614000,12058000,12514000,12983000,13464000,13957000,14463000,15000000};
        int[] sqdID = { 227, 7615, 7138, 24369, 28011, 99999 };
        //HttpServer httpServer = new HttpServer(8080, Routes.GET);
        int[] gun_position = { -1, 7, 12, 17, 8, 13, 18, 9, 14, 19 }; //??? = ??? ??, ?= ?????? ?
        //Thread thread_http = new Thread(new ThreadStart(frm.httpServer.Listen));
        //string[] squad_names = { ""};
        int[] squad_BGM71_defaultstat = { 52, 135, 118, 28 };
        int[] squad_AT4_defaultstat = { 38, 88, 96, 45 };
        int[] squad_AGS30_defaultstat = { 27, 49, 67, 130 };
        int[] squad_2B14_defaultstat = { 51, 20, 46, 54 };
        int[] squad_M2_defaultstat = { 38, 17, 40, 61 };
        int[] squad_QLZ04_defaultstat = { 26, 46, 63, 112 };
        int ??? = 0, ??? = 1, ??? = 2, ?? = 3;
        int battleStarttime = 0;
        public string reset_error = "error:3";

        public class guns
        {
            public string gun_with_user_id { get; set; }
            public string exp { get; set; }
        }
        public class gun_life
        {

        }
        public class squads
        {

        }
        public class empty
        {

        }
        private void AddLog(string text)
        {
            if (!nolog_checkbox.Checked)
                LogTextBox.AppendText(string.Format("[{0}]{1}" + Environment.NewLine, DateTime.Now.ToString("yyyy.MM.dd HH:mm:ss"), text));

        }

        public Form1()
        {
            InitializeComponent();
            frm = this;
            CheckForIllegalCrossThreadCalls = false;
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            echelon_select.SelectedIndex = 0;
            try
            {
                setting = JObject.Parse(File.ReadAllText(@"data/json/settings.json"));
                showdetailLog_checkbox.Checked = bool.Parse(setting["showDetailLog"].ToString());
                updatecheck_checkbox.Checked = bool.Parse(setting["checkUpdate"].ToString());
                nolog_checkbox.Checked = bool.Parse(setting["disableLog"].ToString());
                if (!File.Exists(@"data/lang/" + setting["language"].ToString() + ".json"))
                {
                    MessageBox.Show("Cannot find setted language file.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Close();
                }
                lang_data = JObject.Parse(File.ReadAllText(@"data/lang/" + setting["language"].ToString() + ".json"));
            }
            catch
            {
                MessageBox.Show("Failed to load setting data", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            if (updatecheck_checkbox.Checked)
                updatecheck();
            try
            {
                string guninfofile = File.ReadAllText(@"data/info_texts/guns.b64");
                string mission = File.ReadAllText("data/info_texts/mission.b64");

                System.IO.DirectoryInfo di = new System.IO.DirectoryInfo(@"data/lang");
                System.IO.FileInfo[] fi = di.GetFiles("*.json");
                foreach (var a in fi)
                {
                    languageList_Combobox.Items.Add(Path.GetFileNameWithoutExtension(a.FullName));
                }
                languageList_Combobox.SelectedItem = setting["language"].ToString();
                Set_Lang();
                JObject spotinfo = JObject.Parse(File.ReadAllText(@"data/json/spot_info.json"));
                guninfofile = Encoding.UTF8.GetString(Convert.FromBase64String(guninfofile));
                mission = Encoding.UTF8.GetString(Convert.FromBase64String(mission));

                mission_name = mission.Split('\n').ToList();

                enemy_team_info = JObject.Parse(File.ReadAllText(@"data/json/enemy_team_info.json"));
                userinfo = JObject.Parse(File.ReadAllText(@"data/json/userinfo.json"));
                Theater_data = JObject.Parse(File.ReadAllText(@"data/json/theater_data.json"));
                missionactinfo = new JObject((JObject)userinfo["mission_act_info"]);
                spotactinfo = new JArray((JArray)userinfo["spot_act_info"]);
                userinfo["mission_act_info"] = null;
                userinfo["spot_act_info"] = null;
                enemy_character_info = JObject.Parse(File.ReadAllText(@"data/json/enemy_character_type_info.json"));
                string[] guninfofile_split = guninfofile.Split(new[] { Environment.NewLine }, StringSplitOptions.None);
                gun_eh_array = JArray.Parse(File.ReadAllText(@"data/json/gun_e.json"));
                gun_info_json_1 = JObject.Parse(File.ReadAllText(@"data/json/default_gun.json"));
                gun_info_json_2 = JObject.Parse(File.ReadAllText(@"data/json/default_gun.json"));
                gun_info_json_3 = JObject.Parse(File.ReadAllText(@"data/json/default_gun.json"));
                gun_info_json_4 = JObject.Parse(File.ReadAllText(@"data/json/default_gun.json"));
                gun_info_json_5 = JObject.Parse(File.ReadAllText(@"data/json/default_gun.json"));
                equip_e = JObject.Parse(File.ReadAllText(@"data/json/equip_e.json"));
                gunstatdata = JArray.Parse(File.ReadAllText(@"data/json/doll.json"));
                attribute = JObject.Parse(File.ReadAllText(@"data/json/dollAttribute.json"));
                grow = JObject.Parse(File.ReadAllText(@"data/json/dollGrow.json"));

                portnum.Value = int.Parse(File.ReadAllText(@"data/port"));
                enemy_team_id_combobox.Text = File.ReadAllText(@"data/last_enemyID");
                Boss_HP_textbox.Value = int.Parse(File.ReadAllText(@"data/last_bossHP"));
                theater_enemy_info = JObject.Parse(File.ReadAllText(@"data/json/theater_enemy_info.json"));
                ChangeEnemyGroupID();
                ChangeEnemyBossHP();
                equip = Encoding.UTF8.GetString(Convert.FromBase64String(File.ReadAllText("data/info_texts/equip.b64")));
                string[] temp = equip.Split(new[] { Environment.NewLine }, StringSplitOptions.None);
                commander_name.Text = userinfo["user_info"]["name"].ToString();
                commander_exp.Text = userinfo["user_info"]["experience"].ToString();
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
                for (int i = 0; i < guninfofile_split.Length; i++)
                {
                    if (guninfofile_split[i].Split(',')[0].Split('-')[1].StartsWith("1"))
                    {
                        gun_info.Add(guninfofile_split[i].Substring(5));
                        gunName.Add(guninfofile_split[i].Split(',')[1]);
                    }
                }
                for (int i = 0; i < gun_info.Count; i++)
                {
                    string info = string.Empty;
                    if ((gun_info[i].Split(',')[0]).ToString().Substring(2, 1) == "2")
                    {
                        info = "(改)" + gun_info[i].Split(',')[1] + "(" + Convert.ToInt16(gun_info[i].Split(',')[0]).ToString() + ")";
                    }
                    else
                    {
                        info = gun_info[i].Split(',')[1] + "(" + Convert.ToInt16(gun_info[i].Split(',')[0]).ToString() + ")";
                    }
                    gun_id.Add(int.Parse(gun_info[i].Split(',')[0]));
                    gunid_1_combobox.Items.Add(info);
                    gunid_2_combobox.Items.Add(info);
                    gunid_3_combobox.Items.Add(info);
                    gunid_4_combobox.Items.Add(info);
                    gunid_5_combobox.Items.Add(info);
                }


                foreach (var item in enemy_team_info)
                {
                    string enemyID = enemy_team_info[item.Key]["id"].ToString();
                    string enemyLeaderID = enemy_team_info[enemyID]["enemy_leader"].ToString();

                    //string membernames = string.Empty;
                    JArray a = JArray.Parse(enemy_team_info[enemyID]["member_ids"].ToString());
                    if (enemyLeaderID != "0")
                    {
                        /*
                        for(int i=0; i<a.Count; i++)
                        {
                            string id = a[i].ToString();
                            if(enemy_character_info.ContainsKey(id))
                            {
                                if (i == a.Count - 1)
                                    membernames += enemy_character_info[id]["code"].ToString();
                                else
                                    membernames += enemy_character_info[id]["code"].ToString() + ",";
                            }
                            else
                            {
                                if (i == a.Count - 1)
                                    membernames += id;
                                else
                                    membernames += id + ",";
                            }

                        }*/
                        string missionspotname = string.Empty;
                        if (int.Parse(enemy_team_info[enemyID]["spot_id"].ToString()) > 0)
                        {
                            missionspotname = CodeToMissionName(int.Parse(spotinfo[enemy_team_info[enemyID]["spot_id"].ToString()]["mission_id"].ToString()));
                        }
                        else
                            missionspotname = "-";
                        string[] enemyinfo = { enemyID, enemy_character_info[enemyLeaderID]["code"].ToString(), enemy_character_info[enemyLeaderID]["boss_hp"].ToString(),
                         enemy_character_info[enemyLeaderID]["maxlife"].ToString(),missionspotname/*membernames*/};
                        ListViewItem itm = new ListViewItem(enemyinfo);
                        listView1.Items.Add(itm);
                    }
                    enemyGrpID.Add(enemyID);
                    enemy_team_id_combobox.Items.Add(enemyID);


                }
                gunid_1_combobox.SelectedIndex = 0;
                gunid_2_combobox.SelectedIndex = 1;
                gunid_3_combobox.SelectedIndex = 2;
                gunid_4_combobox.SelectedIndex = 3;
                gunid_5_combobox.SelectedIndex = 4;

                {
                    sqd_1_damage.Minimum = squad_BGM71_defaultstat[???];
                    sqd_1_break.Minimum = squad_BGM71_defaultstat[???];
                    sqd_1_hit.Minimum = squad_BGM71_defaultstat[???];
                    sqd_1_reload.Minimum = squad_BGM71_defaultstat[??];

                    sqd_2_damage.Minimum = squad_AGS30_defaultstat[???];
                    sqd_2_break.Minimum = squad_AGS30_defaultstat[???];
                    sqd_2_hit.Minimum = squad_AGS30_defaultstat[???];
                    sqd_2_reload.Minimum = squad_AGS30_defaultstat[??];

                    sqd_3_damage.Minimum = squad_2B14_defaultstat[???];
                    sqd_3_break.Minimum = squad_2B14_defaultstat[???];
                    sqd_3_hit.Minimum = squad_2B14_defaultstat[???];
                    sqd_3_reload.Minimum = squad_2B14_defaultstat[??];

                    sqd_4_damage.Minimum = squad_M2_defaultstat[???];
                    sqd_4_break.Minimum = squad_M2_defaultstat[???];
                    sqd_4_hit.Minimum = squad_M2_defaultstat[???];
                    sqd_4_reload.Minimum = squad_M2_defaultstat[??];

                    sqd_5_damage.Minimum = squad_AT4_defaultstat[???];
                    sqd_5_break.Minimum = squad_AT4_defaultstat[???];
                    sqd_5_hit.Minimum = squad_AT4_defaultstat[???];
                    sqd_5_reload.Minimum = squad_AT4_defaultstat[??];

                    sqd_6_damage.Minimum = squad_QLZ04_defaultstat[???];
                    sqd_6_break.Minimum = squad_QLZ04_defaultstat[???];
                    sqd_6_hit.Minimum = squad_QLZ04_defaultstat[???];
                    sqd_6_reload.Minimum = squad_QLZ04_defaultstat[??];
                }
                setSquadInfo(false); //??? ???
                SetGunInfo(false, 0); //?? ???
                {
                    sqd_1_damage.Minimum = squad_BGM71_defaultstat[???];
                    sqd_1_break.Minimum = squad_BGM71_defaultstat[???];
                    sqd_1_hit.Minimum = squad_BGM71_defaultstat[???];
                    sqd_1_reload.Minimum = squad_BGM71_defaultstat[??];

                    sqd_2_damage.Minimum = squad_AGS30_defaultstat[???];
                    sqd_2_break.Minimum = squad_AGS30_defaultstat[???];
                    sqd_2_hit.Minimum = squad_AGS30_defaultstat[???];
                    sqd_2_reload.Minimum = squad_AGS30_defaultstat[??];

                    sqd_3_damage.Minimum = squad_2B14_defaultstat[???];
                    sqd_3_break.Minimum = squad_2B14_defaultstat[???];
                    sqd_3_hit.Minimum = squad_2B14_defaultstat[???];
                    sqd_3_reload.Minimum = squad_2B14_defaultstat[??];

                    sqd_4_damage.Minimum = squad_M2_defaultstat[???];
                    sqd_4_break.Minimum = squad_M2_defaultstat[???];
                    sqd_4_hit.Minimum = squad_M2_defaultstat[???];
                    sqd_4_reload.Minimum = squad_M2_defaultstat[??];

                    sqd_5_damage.Minimum = squad_AT4_defaultstat[???];
                    sqd_5_break.Minimum = squad_AT4_defaultstat[???];
                    sqd_5_hit.Minimum = squad_AT4_defaultstat[???];
                    sqd_5_reload.Minimum = squad_AT4_defaultstat[??];

                    sqd_6_damage.Minimum = squad_QLZ04_defaultstat[???];
                    sqd_6_break.Minimum = squad_QLZ04_defaultstat[???];
                    sqd_6_hit.Minimum = squad_QLZ04_defaultstat[???];
                    sqd_6_reload.Minimum = squad_QLZ04_defaultstat[??];
                }
                enable_set_sqd();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), lang_data["fileload_failed_msg"].ToString(), MessageBoxButtons.OK, MessageBoxIcon.Stop);
                Close();
            }
            theater_area.SelectedIndex = 0;
            theater_area_num.SelectedIndex = 0;
            UpdatePosTile(null, null);

        }
        private void Set_Lang()
        {
            for (int i = 1; i <= 5; i++)
            {
                Label gunid = (Label)Controls.Find("gunid_" + i.ToString(), true)[0];
                gunid.Text = lang_data["gun_id"].ToString();
                Label gunskin = (Label)Controls.Find("gunskin_" + i.ToString(), true)[0];
                gunskin.Text = lang_data["skin"].ToString();
                Label gunlv = (Label)Controls.Find("gunlv_" + i.ToString(), true)[0];
                gunlv.Text = lang_data["Level"].ToString();
                Label gunhp = (Label)Controls.Find("gunhp_" + i.ToString(), true)[0];
                gunhp.Text = lang_data["Health"].ToString();
                Label gunfire = (Label)Controls.Find("gunfire_" + i.ToString(), true)[0];
                gunfire.Text = lang_data["fire"].ToString();
                Label gundodge = (Label)Controls.Find("gundodge_" + i.ToString(), true)[0];
                gundodge.Text = lang_data["dodge"].ToString();
                Label gunfirespeed = (Label)Controls.Find("gunfirespeed_" + i.ToString(), true)[0];
                gunfirespeed.Text = lang_data["fire_speed"].ToString();
                Label gunhit = (Label)Controls.Find("gunhit_" + i.ToString(), true)[0];
                gunhit.Text = lang_data["hit"].ToString();
                Label gundummy = (Label)Controls.Find("gundummy_" + i.ToString(), true)[0];
                gundummy.Text = lang_data["gun_dummy"].ToString();
                Label gunskill1 = (Label)Controls.Find("gunskill1_" + i.ToString(), true)[0];
                gunskill1.Text = lang_data["skill_1_lv"].ToString();
                Label gunskill2 = (Label)Controls.Find("gunskill2_" + i.ToString(), true)[0];
                gunskill2.Text = lang_data["skill_2_lv"].ToString();
                Label gunfavor = (Label)Controls.Find("gunfavor_" + i.ToString(), true)[0];
                gunfavor.Text = lang_data["favor"].ToString();
                Label gunpos = (Label)Controls.Find("gunpos_" + i.ToString(), true)[0];
                gunpos.Text = lang_data["position"].ToString();
                Button setbtn = (Button)Controls.Find("set_gunpos_btn_" + i.ToString(), true)[0];
                setbtn.Text = lang_data["set"].ToString();
                CheckBox gunoath = (CheckBox)Controls.Find("gunoath_" + i.ToString() + "_checkbox", true)[0];
                gunoath.Text = lang_data["oath"].ToString();
                Button resetstat = (Button)Controls.Find("gun_resetstat_" + i.ToString(), true)[0];
                resetstat.Text = lang_data["reset_default_stat"].ToString();
                CheckBox enable = (CheckBox)Controls.Find("enable_" + i.ToString(), true)[0];
                enable.Text = lang_data["doll_no" + i.ToString()].ToString();
                GroupBox gun_gb = (GroupBox)Controls.Find("groupBox" + i.ToString(), true)[0];
                gun_gb.Text = lang_data["doll_no" + i.ToString()].ToString();
                GroupBox equip_gb = (GroupBox)Controls.Find("groupBox_equip_" + i.ToString(), true)[0];
                equip_gb.Text = lang_data["doll_no" + i.ToString()].ToString();
                Button setequip_1 = (Button)Controls.Find("setEquip_" + i.ToString() + "1", true)[0];
                setequip_1.Text = lang_data["none_equip"].ToString();
                Button setequip_2 = (Button)Controls.Find("setEquip_" + i.ToString() + "2", true)[0];
                setequip_2.Text = lang_data["none_equip"].ToString();
                Button setequip_3 = (Button)Controls.Find("setEquip_" + i.ToString() + "3", true)[0];
                setequip_3.Text = lang_data["none_equip"].ToString();

            }
            for (int i = 1; i <= 6; i++)
            {
                Label sqdlv = (Label)Controls.Find("sqdlv_" + i.ToString(), true)[0];
                sqdlv.Text = lang_data["Level"].ToString();
                Label sqddam = (Label)Controls.Find("sqddamage_" + i.ToString(), true)[0];
                sqddam.Text = lang_data["sqd_damage"].ToString();
                Label sqdbrk = (Label)Controls.Find("sqdbreak_" + i.ToString(), true)[0];
                sqdbrk.Text = lang_data["sqd_break"].ToString();
                Label sqdhit = (Label)Controls.Find("sqdhit_" + i.ToString(), true)[0];
                sqdhit.Text = lang_data["sqd_hit"].ToString();
                Label sqdreload = (Label)Controls.Find("sqdreload_" + i.ToString(), true)[0];
                sqdreload.Text = lang_data["sqd_reload"].ToString();
                Label sqdskill_1 = (Label)Controls.Find("sqdskill1_" + i.ToString(), true)[0];
                sqdskill_1.Text = lang_data["skill_1_lv"].ToString();
                Label sqdskill_2 = (Label)Controls.Find("sqdskill2_" + i.ToString(), true)[0];
                sqdskill_2.Text = lang_data["skill_2_lv"].ToString();
                Label sqdskill_3 = (Label)Controls.Find("sqdskill3_" + i.ToString(), true)[0];
                sqdskill_3.Text = lang_data["skill_3_lv"].ToString();

            }
            #region page1
            tabControl1.TabPages[0].Text = lang_data["set_guns"].ToString();
            groupBox6.Text = lang_data["settings"].ToString();
            gun_save.Text = lang_data["save"].ToString();
            gun_load.Text = lang_data["load"].ToString();
            gun_loadfromserver.Text = lang_data["loadfromserver"].ToString();
            gun_setfire.Text = lang_data["sally"].ToString();
            gun_previewpos.Text = lang_data["preview_position"].ToString();
            gun_apply.Text = lang_data["decide"].ToString();
            #endregion

            #region page2
            tabControl1.TabPages[1].Text = lang_data["set_fire_suppert_unit"].ToString();
            groupBox13.Text = lang_data["set"].ToString();
            sqd_save.Text = lang_data["save"].ToString();
            sqd_load.Text = lang_data["load"].ToString();
            set_fire.Text = lang_data["set_fire"].ToString();
            sqd_apply.Text = lang_data["decide"].ToString();
            #endregion

            #region page3
            tabControl1.TabPages[2].Text = lang_data["set_equips"].ToString();
            groupBox_equip_setting.Text = lang_data["set"].ToString();
            equip_save.Text = lang_data["save"].ToString();
            equip_load.Text = lang_data["load"].ToString();
            equip_removeall.Text = lang_data["remove_all_equip"].ToString();
            equip_autoapply_desc.Text = lang_data["equip_autoapply_desc"].ToString();
            #endregion

            #region page4
            tabControl1.TabPages[3].Text = lang_data["set_battle"].ToString();
            server_applyID.Text = lang_data["apply"].ToString();
            server_viewBattlelog.Text = lang_data["view_battleLog"].ToString();
            server_referenceID.Text = lang_data["enemy_id_reference"].ToString();
            server_information.Text = lang_data["server_info"].ToString();
            server_hideip.Text = lang_data["hide_ip"].ToString();
            server_log.Text = lang_data["server_log"].ToString();
            server_mode.Text = lang_data["server_mode"].ToString();
            normalbattle.Text = lang_data["server_mode_normal"].ToString();
            theaterbattle.Text = lang_data["server_mode_theater"].ToString();
            server_setport.Text = lang_data["port_setting"].ToString();
            server_start.Text = lang_data["start_server"].ToString();
            clearlog.Text = lang_data["clear_log"].ToString();
            listView1.Columns[0].Text = lang_data["group"].ToString();
            listView1.Columns[1].Text = lang_data["leader"].ToString();
            listView1.Columns[2].Text = lang_data["list_boss_hp"].ToString();
            listView1.Columns[3].Text = lang_data["list_leader_hp"].ToString();
            listView1.Columns[4].Text = lang_data["list_mission_name"].ToString();
            server_state.Text = lang_data["server_state_stopped"].ToString();
            server_settedID.Text = lang_data["server_setted_id"].ToString();
            server_ip.Text = lang_data["server_ip"].ToString();
            server_port.Text = lang_data["server_port"].ToString();
            enemy_group.Text = lang_data["enemy_group"].ToString();
            boss_hp.Text = lang_data["boss_hp"].ToString();
            #endregion

            #region page5
            for (int i = 1; i <= 8; i++)
            {
                theater_area_num.Items.Add(string.Format(lang_data["theater_area_num"].ToString(), i.ToString()));
            }
            tabControl1.TabPages[4].Text = lang_data["set_theater_tap"].ToString();
            theater_area_gb.Text = lang_data["set_theater_tap"].ToString();
            theater_area_occ_gb.Text = lang_data["theater_area_occupation"].ToString();
            theater_area_bigginer_occ.Text = lang_data["theater_area_bigginer"].ToString();
            theater_area_mid_occ.Text = lang_data["theater_area_mid"].ToString();
            theater_area_adv_occ.Text = lang_data["theater_area_adv"].ToString();
            theater_area_core_occ.Text = lang_data["theater_area_core"].ToString();
            theater_enemy_setting.Text = lang_data["theater_enemy_setting"].ToString();
            theater_enemy_random_btn.Text = lang_data["theater_enemy_random"].ToString();
            theater_enemy_preview.Text = lang_data["theater_enemy_pre"].ToString();
            #endregion

            #region page6
            tabControl1.TabPages[5].Text = lang_data["set_etc"].ToString();
            etc_basicinformation.Text = lang_data["basic_information"].ToString();
            com_name.Text = lang_data["commander_name"].ToString();
            com_exp.Text = lang_data["commander_exp"].ToString();
            etc_progsetting.Text = lang_data["program_setting"].ToString();
            showdetailLog_checkbox.Text = lang_data["show_detailLog"].ToString();
            updatecheck_checkbox.Text = lang_data["chech_update"].ToString();
            nolog_checkbox.Text = lang_data["no_log"].ToString();
            client_reset_type.Text = lang_data["client_force_reset_type"].ToString();
            error_error3.Text = lang_data["type_error3"].ToString();
            error_error2.Text = lang_data["type_error2"].ToString();
            error_error1.Text = lang_data["type_error1"].ToString();
            error_error0.Text = lang_data["type_error0"].ToString();
            #endregion




        }

        string getFileSizeMD5(string path)
        {
            try
            {
                var info = new FileInfo(path);
                long size = info.Length;
                using (System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create())
                {
                    byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(size.ToString());
                    byte[] hashBytes = md5.ComputeHash(inputBytes);
                    StringBuilder sb = new StringBuilder();
                    for (int i = 0; i < hashBytes.Length; i++)
                    {
                        sb.Append(hashBytes[i].ToString("X2"));
                    }
                    return sb.ToString();
                }
            }
            catch (FileNotFoundException)
            {
                return "0";
            }
        }
        public string GetMD5HashFromFile(string filepath)
        {
            MD5 md5 = MD5.Create();
            FileStream stream = File.OpenRead(filepath);
            string hash = BitConverter.ToString(md5.ComputeHash(stream)).Replace("-", string.Empty).ToLower();
            stream.Close();
            return hash;
        }
        void updatecheck()
        {
            try
            {
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                bool isLatest = true;
                WebClient updatercl = new WebClient();
                byte[] updatelist = updatercl.DownloadData("https://dhlrunner.github.io/GFBT_update_data_files/update_files.txt");
                string[] list = Encoding.UTF8.GetString(updatelist).Split(Environment.NewLine.ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                foreach (string a in list)
                {
                    if (a != "")
                    {
                        string filepath = a.Split(',')[0];
                        string filehash = a.Split(',')[1];
                        string localhash = string.Empty;
                        if (File.Exists(filepath))
                        {
                            localhash = GetMD5HashFromFile(filepath);
                            if (localhash != filehash)
                                isLatest = false;
                        }
                        else
                            isLatest = false;

                    }
                }
                if (!isLatest)
                {
                    if (MessageBox.Show(lang_data["update_available_msg"].ToString(), lang_data["update_available"].ToString(), MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        //this.Opacity = 0;
                        updater updform = new updater(this);
                        updform.StartPosition = FormStartPosition.Manual;
                        updform.Left = 500;
                        updform.Top = 500;
                        updform.ShowDialog();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(lang_data["update_check_failed_msg"].ToString() + Environment.NewLine + ex.ToString(), lang_data["error"].ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        string CodeToMissionName(int code)
        {
            string name = string.Empty;
            for (int i = 0; i < mission_name.Count(); i++)
            {
                string temp = mission_name[i].Split('-')[1];

                if (temp.Substring(0, 1) != "2")
                {
                    int c = int.Parse(temp.Split(',')[0].Substring(1));
                    if (c == code)
                    {
                        name = temp.Split(',')[1];
                        return name;
                    }
                }

            }
            return "-";
        }
        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {

        }

        private void gunid_1_combobox_SelectedIndexChanged(object sender, EventArgs e)
        {
            eq1_gun_name.Text = gunid_1_combobox.Text;
            setdefaultgunstat(1, int.Parse(gun_info[gunid_1_combobox.SelectedIndex].Split(',')[0]), Convert.ToInt16(gundummy_1_number.Value));
            if (!check_ifmod(1))
            {
                gunskill2_1_number.Maximum = 0;
                gunlv_1_number.Maximum = 100;
            }
            else
            {
                gunskill2_1_number.Maximum = 10;
                gunlv_1_number.Maximum = 120;
            }

        }

        private void gunid_2_combobox_SelectedIndexChanged(object sender, EventArgs e)
        {
            eq2_gun_name.Text = gunid_2_combobox.Text;
            setdefaultgunstat(2, int.Parse(gun_info[gunid_2_combobox.SelectedIndex].Split(',')[0]), Convert.ToInt16(gundummy_2_number.Value));
            if (!check_ifmod(2))
            {
                gunskill2_2_number.Maximum = 0;
                gunlv_2_number.Maximum = 100;
            }
            else
            {
                gunskill2_2_number.Maximum = 10;
                gunlv_2_number.Maximum = 120;
            }
        }

        private void gunid_3_combobox_SelectedIndexChanged(object sender, EventArgs e)
        {
            eq3_gun_name.Text = gunid_3_combobox.Text;
            setdefaultgunstat(3, int.Parse(gun_info[gunid_3_combobox.SelectedIndex].Split(',')[0]), Convert.ToInt16(gundummy_3_number.Value));
            if (!check_ifmod(3))
            {
                gunskill2_3_number.Maximum = 0;
                gunlv_3_number.Maximum = 100;
            }
            else
            {
                gunskill2_3_number.Maximum = 10;
                gunlv_3_number.Maximum = 120;
            }
        }

        private void gunid_4_combobox_SelectedIndexChanged(object sender, EventArgs e)
        {
            eq4_gun_name.Text = gunid_4_combobox.Text;
            setdefaultgunstat(4, int.Parse(gun_info[gunid_4_combobox.SelectedIndex].Split(',')[0]), Convert.ToInt16(gundummy_4_number.Value));
            if (!check_ifmod(4))
            {
                gunskill2_4_number.Maximum = 0;
                gunlv_4_number.Maximum = 100;
            }
            else
            {
                gunskill2_4_number.Maximum = 10;
                gunlv_4_number.Maximum = 120;
            }
        }

        private void gunid_5_combobox_SelectedIndexChanged(object sender, EventArgs e)
        {
            eq5_gun_name.Text = gunid_5_combobox.Text;
            setdefaultgunstat(5, int.Parse(gun_info[gunid_5_combobox.SelectedIndex].Split(',')[0]), Convert.ToInt16(gundummy_5_number.Value));
            if (!check_ifmod(5))
            {
                gunskill2_5_number.Maximum = 0;
                gunlv_5_number.Maximum = 100;
            }
            else
            {
                gunskill2_5_number.Maximum = 10;
                gunlv_5_number.Maximum = 120;
            }
        }
        bool check_ifmod(int num)
        {
            switch (num)
            {
                case 1:
                    int id = int.Parse(gun_info[gunid_1_combobox.SelectedIndex].Split(',')[0]);
                    if (id / 20000 < 2 && id / 20000 >= 1)
                        return true;
                    return false;
                case 2:
                    int id2 = int.Parse(gun_info[gunid_2_combobox.SelectedIndex].Split(',')[0]);
                    if (id2 / 20000 < 2 && id2 / 20000 >= 1)
                        return true;
                    return false;
                case 3:
                    int id3 = int.Parse(gun_info[gunid_3_combobox.SelectedIndex].Split(',')[0]);
                    if (id3 / 20000 < 2 && id3 / 20000 >= 1)
                        return true;
                    return false;
                case 4:
                    int id4 = int.Parse(gun_info[gunid_4_combobox.SelectedIndex].Split(',')[0]);
                    if (id4 / 20000 < 2 && id4 / 20000 >= 1)
                        return true;
                    return false;
                case 5:
                    int id5 = int.Parse(gun_info[gunid_5_combobox.SelectedIndex].Split(',')[0]);
                    if (id5 / 20000 < 2 && id5 / 20000 >= 1)
                        return true;
                    return false;
                default:
                    return false;
            }
        }
        void setdefaultgunstat(int num, int id, int gunNum)
        {
            JObject stat = new JObject(getGunDefaultStat(gunNum, id, 1));
            switch (num)
            {
                case 1:
                    //gunhp_1_number.Value = int.Parse(stat["hp"].ToString());
                    gunpow_1_number.Minimum = int.Parse(stat["pow"].ToString());
                    gundodge_1_number.Minimum = int.Parse(stat["dodge"].ToString());
                    gunrate_1_number.Minimum = int.Parse(stat["rate"].ToString());
                    gunhit_1_number.Minimum = int.Parse(stat["hit"].ToString());
                    break;
                case 2:
                    //gunhp_2_number.Value = int.Parse(stat["hp"].ToString());
                    gunpow_2_number.Minimum = int.Parse(stat["pow"].ToString());
                    gundodge_2_number.Minimum = int.Parse(stat["dodge"].ToString());
                    gunrate_2_number.Minimum = int.Parse(stat["rate"].ToString());
                    gunhit_2_number.Minimum = int.Parse(stat["hit"].ToString());
                    break;
                case 3:
                    //gunhp_3_number.Value = int.Parse(stat["hp"].ToString());
                    gunpow_3_number.Minimum = int.Parse(stat["pow"].ToString());
                    gundodge_3_number.Minimum = int.Parse(stat["dodge"].ToString());
                    gunrate_3_number.Minimum = int.Parse(stat["rate"].ToString());
                    gunhit_3_number.Minimum = int.Parse(stat["hit"].ToString());
                    break;
                case 4:
                    //gunhp_4_number.Value = int.Parse(stat["hp"].ToString());
                    gunpow_4_number.Minimum = int.Parse(stat["pow"].ToString());
                    gundodge_4_number.Minimum = int.Parse(stat["dodge"].ToString());
                    gunrate_4_number.Minimum = int.Parse(stat["rate"].ToString());
                    gunhit_4_number.Minimum = int.Parse(stat["hit"].ToString());
                    break;
                case 5:
                    //gunhp_5_number.Value = int.Parse(stat["hp"].ToString());
                    gunpow_5_number.Minimum = int.Parse(stat["pow"].ToString());
                    gundodge_5_number.Minimum = int.Parse(stat["dodge"].ToString());
                    gunrate_5_number.Minimum = int.Parse(stat["rate"].ToString());
                    gunhit_5_number.Minimum = int.Parse(stat["hit"].ToString());
                    break;
            }
        }
        void load_with_userinfo(JObject json, int num)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {

        }

        private void button5_Click(object sender, EventArgs e)
        {
            run_server();
        }
        bool run_server()
        {
            if (enemy_team_id_combobox.Text == string.Empty)
            {
                MessageBox.Show(lang_data["select_enemygroup_msg"].ToString(), lang_data["error"].ToString(), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            else if (!gunsaved)
            {
                MessageBox.Show(lang_data["set_gun_first_msg"].ToString(), lang_data["error"].ToString(), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            else
            {
                if (!IsTcpPortAvailable(Convert.ToInt16(portnum.Value)))
                {
                    MessageBox.Show(string.Format(lang_data["port_already_using_msg"].ToString(), portnum.Value.ToString()), lang_data["error"].ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
                else
                {
                    try
                    {
                        int i = 0;
                        for (i = 0; i < userinfo["spot_act_info"].Count(); i++)
                        {
                            if (userinfo["spot_act_info"][i]["spot_id"].ToString() == battlespot)
                            {
                                userinfo["spot_act_info"][i]["enemy_team_id"] = enemy_team_id_combobox.Text;
                                userinfo["spot_act_info"][i]["boss_hp"] = Boss_HP_textbox.Value.ToString();
                                break;
                            }
                        }

                        //if (enemy_team_info[])              
                        listener.Prefixes.Add(string.Format("http://*:{0}/", portnum.Value.ToString()));
                        //listener.Prefixes.Add("https://+/");
                        listener.AuthenticationSchemes = AuthenticationSchemes.Anonymous;
                        listener.Start();
                        if (!thread.IsAlive)
                        {
                            thread.SetApartmentState(ApartmentState.STA);
                            thread.Start(listener);
                        }


                        server_state.Text = lang_data["server_state_running"].ToString();
                        if (!server_hideip.Checked)
                            server_ip.Text = lang_data["server_ip"].ToString() + GetLocalIP();
                        server_port.Text = lang_data["server_port"].ToString() + portnum.Value.ToString();
                        AddLog(string.Format(lang_data["server_started_log_msg"].ToString(), server_hideip.Checked ? "***.***.***.***" : GetLocalIP(), portnum.Value.ToString(), spotactinfo[i]["enemy_team_id"].ToString()));


                        File.WriteAllText(@"data/port", portnum.Value.ToString());
                        server_start.Enabled = false;
                        portnum.Enabled = false;
                        return true;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.ToString(), "Error while opening server", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }
                }


            }
        }
        private string GetLocalIP()
        {
            string myIP = "";
            IPHostEntry host = Dns.GetHostEntry(Dns.GetHostName());

            foreach (IPAddress ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    myIP = ip.ToString();
                }
            }

            return myIP;
        }
        public static bool IsTcpPortAvailable(int tcpPort)
        {
            var ipgp = IPGlobalProperties.GetIPGlobalProperties();
            // ?? ??
            TcpConnectionInformation[] conns = ipgp.GetActiveTcpConnections();
            foreach (var cn in conns)
            {
                if (cn.State == TcpState.Listen)
                {
                    return false;
                }
            }

            IPEndPoint[] endpoints = ipgp.GetActiveTcpListeners();
            foreach (var ep in endpoints)
            {
                if (ep.Port == tcpPort)
                {
                    return false;
                }
            }

            return true;
        }
        private static void ProcessRequest(HttpListenerContext ctx)
        {
            string uri = ctx.Request.RawUrl;
            byte[] Client_Req_data = Encoding.UTF8.GetBytes(new StreamReader(ctx.Request.InputStream).ReadToEnd());
            string Outdatacode = Encoding.UTF8.GetString(Client_Req_data);
            if (Outdatacode.Contains("outdatacode"))
                Outdatacode = HttpUtility.UrlDecode(Outdatacode.Split('&')[1].Split('=')[1]);
            if (uri.Contains("favicon.ico"))
            {
                ResponceProcessBinary(ctx, File.ReadAllBytes(@"data/icon.ico"), false, false);
            }
            else if (ctx.Request.Url.LocalPath == "/")
            {
                string html = "<!doctype html><html lang=\"ko\"><head><meta charset=\"utf-8\"><title>GFHelper</title></head><body>Server is working</br><a href=\"data/Cert/GFBattleTester.cer/\">??? ????</a></body></html>";
                ResponceProcessBinary(ctx, Encoding.UTF8.GetBytes(html), false, true);
            }
            else if (ctx.Request.Url.LocalPath == "/data/Cert/GFBattleTester.cer/")
            {
                byte[] cert = File.ReadAllBytes(@"data/Cert/Cert.pfx");
                ctx.Response.ContentType = "other/certificate";
                ctx.Response.Headers.Add("Content-Disposition", "form-data; name=\"my_file\"; filename=\"GFBattleTester.pfx\"");
                ResponceProcessBinary(ctx, cert, false, true);
            }
            frm.AddLog("ClientIP: " + ctx.Request.RemoteEndPoint.Address.ToString() + "; Request: " + uri);
            if (frm.showdetailLog_checkbox.Checked && ctx.Request.HttpMethod.ToString() == "POST")
                frm.AddLog("Client_Request: " + Encoding.UTF8.GetString(Client_Req_data));

            if (!frm.getUserinfoFromServer) //!????
            {
                if (ctx.Request.Url.LocalPath == GF_URLs_Kr.index_version)
                {
                    string json = File.ReadAllText(@"data/json/version.json");
                    JObject o = JObject.Parse(json);
                    o["now"] = Packet.GetCurrentTimeStamp().ToString();
                    o["tomorrow_zero"] = (Packet.GetCurrentTimeStamp() + (86400 - Packet.GetCurrentTimeStamp() % 86400)).ToString();
                    ResponceProcessBinary(ctx, Encoding.UTF8.GetBytes(o.ToString()), false, false);
                }
                else if (ctx.Request.Url.LocalPath == GF_URLs_Kr.getToken)
                {
                    Packet.init();
                    string data = Packet.Encode(File.ReadAllText(@"data/json/token.json"), "yundoudou");
                    ResponceProcessBinary(ctx, Encoding.UTF8.GetBytes(data), false, false);
                }
                else if (ctx.Request.Url.LocalPath == GF_URLs_Kr.getuserinfo)
                {
                    frm.AddLog(frm.lang_data["get_userinfo_log"].ToString());
                    Clipboard.SetText(userinfo.ToString());
                    if (frm.normalbattle.Checked)
                    {
                        userinfo["mission_act_info"] = new JObject(frm.missionactinfo);
                        userinfo["spot_act_info"] = new JArray(frm.spotactinfo);
                    }
                    else
                    {
                        userinfo["mission_act_info"] = null;
                        userinfo["spot_act_info"] = null;
                    }
                    ResponceProcessBinary(ctx, Encoding.UTF8.GetBytes(userinfo.ToString()), true, false);
                }
                else if (ctx.Request.Url.LocalPath == GF_URLs_Kr.GetTheaterData)
                {
                    ResponceProcessBinary(ctx, Encoding.UTF8.GetBytes(frm.Theater_data.ToString()), true, false);
                }
                else if (ctx.Request.Url.LocalPath == GF_URLs_Kr.GetTheaterBalance)
                {
                    string data = "{\"incident_pt\":0}";
                    ResponceProcessBinary(ctx, Encoding.UTF8.GetBytes(data), true, false);
                }
                else if (ctx.Request.Url.LocalPath == GF_URLs_Kr.GetTheaterPrize)
                {
                    string data = "[{ \"type\":2,\"occupied_ids\":52}]";
                    ResponceProcessBinary(ctx, Encoding.UTF8.GetBytes(data), true, false);
                }
                else if (ctx.Request.Url.LocalPath == GF_URLs_Kr.startTheaterBattle)
                {
                    JObject clientdata = JObject.Parse(Packet.Decode(Outdatacode, signkey));
                    frm.AddLog(string.Format(frm.lang_data["echelon_out"].ToString(), clientdata["team_id"].ToString()));
                    ResponceProcessBinary(ctx, new byte[1] { 0x31 }/*"1"*/, false, false);
                }
                else if (ctx.Request.Url.LocalPath == GF_URLs_Kr.battlefinish)
                {
                    Random random = new Random();
                    JObject clientdata = JObject.Parse(Packet.Decode(Outdatacode, signkey));
                    //Clipboard.SetText(clientdata.ToString());
                    JObject saveJson = new JObject();
                    JArray guninfo = new JArray();
                    int index = 0;
                    for (index = 0; index < userinfo["spot_act_info"].Count(); index++)
                    {
                        if (userinfo["spot_act_info"][index]["spot_id"].ToString() == frm.battlespot)
                        {
                            break;
                        }
                    }
                    guninfo.Add(JObject.Parse("{" + string.Format(@"'id':'1','gunid':'{0}','life':'{1}','lifeBefore':'{4}','pos':'{2}','isUsed':'{3}'", frm.gun_info_json_1["gun_id"].ToString(), clientdata["guns"][0]["life"].ToString(), frm.gun_info_json_1["position"].ToString(), frm.gun_info_json_1["team_id"].ToString(), frm.gun_info_json_1["life"].ToString()) + "}"));
                    guninfo.Add(JObject.Parse("{" + string.Format(@"'id':'2','gunid':'{0}','life':'{1}','lifeBefore':'{4}','pos':'{2}','isUsed':'{3}'", frm.gun_info_json_2["gun_id"].ToString(), clientdata["guns"].Count() > 1 ? clientdata["guns"][1]["life"].ToString() : "-1", frm.gun_info_json_2["position"].ToString(), frm.gun_info_json_2["team_id"].ToString(), frm.gun_info_json_2["life"].ToString()) + "}"));
                    guninfo.Add(JObject.Parse("{" + string.Format(@"'id':'3','gunid':'{0}','life':'{1}','lifeBefore':'{4}','pos':'{2}','isUsed':'{3}'", frm.gun_info_json_3["gun_id"].ToString(), clientdata["guns"].Count() > 2 ? clientdata["guns"][2]["life"].ToString() : "-1", frm.gun_info_json_3["position"].ToString(), frm.gun_info_json_3["team_id"].ToString(), frm.gun_info_json_3["life"].ToString()) + "}"));
                    guninfo.Add(JObject.Parse("{" + string.Format(@"'id':'4','gunid':'{0}','life':'{1}','lifeBefore':'{4}','pos':'{2}','isUsed':'{3}'", frm.gun_info_json_4["gun_id"].ToString(), clientdata["guns"].Count() > 3 ? clientdata["guns"][3]["life"].ToString() : "-1", frm.gun_info_json_4["position"].ToString(), frm.gun_info_json_4["team_id"].ToString(), frm.gun_info_json_4["life"].ToString()) + "}"));
                    guninfo.Add(JObject.Parse("{" + string.Format(@"'id':'5','gunid':'{0}','life':'{1}','lifeBefore':'{4}','pos':'{2}','isUsed':'{3}'", frm.gun_info_json_5["gun_id"].ToString(), clientdata["guns"].Count() > 4 ? clientdata["guns"][4]["life"].ToString() : "-1", frm.gun_info_json_5["position"].ToString(), frm.gun_info_json_5["team_id"].ToString(), frm.gun_info_json_5["life"].ToString()) + "}"));
                    saveJson.Add("spot", clientdata["spot_id"].ToString());
                    saveJson.Add("enemyDie", clientdata["if_enemy_die"]);
                    saveJson.Add("battleEndtime", clientdata["current_time"].ToString());
                    saveJson.Add("bossHP", clientdata["boss_hp"].ToString());
                    saveJson.Add("mvp", clientdata["mvp"].ToString());
                    saveJson.Add("sqd_skill", clientdata["use_skill_squads"]);
                    saveJson.Add("gun_info", guninfo);
                    saveJson.Add("rec", JObject.Parse(clientdata["user_rec"].ToString())["record"]);
                    saveJson.Add("battleTime", clientdata["1000"]["27"].ToString());
                    saveJson.Add("totalDamageFromEnemy", clientdata["1000"]["18"].ToString());
                    saveJson.Add("totalDamageToEnemy", clientdata["1000"]["24"].ToString());
                    saveJson.Add("MaxDamageToEnemy", clientdata["1000"]["41"].ToString());
                    saveJson.Add("EnemyLeaderID", clientdata["1000"]["33"].ToString());
                    saveJson.Add("EnemyGroupID", frm.spotactinfo[index]["enemy_team_id"].ToString());
                    saveJson.Add("unknown", Xxtea.XXTEA.EncryptToBase64String(Encoding.UTF8.GetBytes(clientdata["1000"].ToString()), Encoding.UTF8.GetBytes("????")));
                    File.WriteAllText(@"BattleLog/" + DateTime.Now.ToString("yyyyMMdd_HHmmss") + "_battlelog.brvf", saveJson.ToString());
                    JArray gun_ids = JArray.Parse(clientdata["guns"].ToString());
                    int fight_gun = gun_ids.Count;
                    string[] gunid = new string[fight_gun];
                    for (int i = 0; i < fight_gun; i++)
                    {
                        gunid[i] = gun_ids[i]["id"].ToString();
                    }
                    var sd = new
                    {
                        user_exp = "50",
                        /*
                        gun_exp = new List<guns>()
                         {                        
                             new guns(){gun_with_user_id = gunid[0], exp = "0"},
                            new guns(){gun_with_user_id = gunid[1], exp = "0"},
                            new guns(){gun_with_user_id = gunid[2], exp = "0"},
                            new guns(){gun_with_user_id = gunid[3], exp = "0"},
                            new guns(){gun_with_user_id = gunid[4], exp = "0"}
                        },*/
                        fairy_exp = "0",
                        gun_life = new List<gun_life>()
                        {
                        },
                        squad_exp = new List<squads>()
                        {

                        },
                        battle_rank = 5,
                        free_exp = "999999",
                        change_belong = new List<empty>()
                        {

                        },
                        building_defender_change = new List<empty>()
                        {
                        },
                        mission_win_result = new List<empty>()
                        {

                        },
                        seed = "2801",
                        type5_score = "0",
                        ally_instance_transform = new List<empty>() { },
                        ally_instance_betray = new List<empty>() { },
                        mission_control = new
                        {
                        }
                    };
                    var senddata = JsonConvert.SerializeObject(sd, Formatting.Indented);
                    JObject a = JObject.Parse(senddata.ToString());
                    JObject fav = new JObject();
                    for (int i = 0; i < fight_gun; i++)
                    {
                        fav.Add(gunid[i], "500");
                    }
                    //var favor = JObject.Parse("{\"" + gunid[0] + "\":500,\"" + gunid[1] + "\":150,\"" + gunid[2] + "\":150,\"" + gunid[3] + "\":500,\"" + gunid[4] + "\":450}");
                    a.Add("favor_change", fav);
                    JArray array = new JArray();
                    for (int i = 0; i < fight_gun; i++)
                    {
                        JObject k = new JObject();
                        k.Add("gun_with_user_id", gunid[i]);
                        k.Add("exp", random.Next(1, 23442393).ToString());
                        array.Add(k);
                        //a.Add("gun_exp", array);
                    }
                    a.Add("gun_exp", array);
                    string outtdata = a.ToString();
                    outtdata = outtdata.Replace("\n", String.Empty);
                    outtdata = outtdata.Replace("\r", String.Empty);
                    ResponceProcessBinary(ctx, Encoding.UTF8.GetBytes(a.ToString()), true, false);

                }
                else if (ctx.Request.Url.LocalPath == GF_URLs_Kr.endTheaterBattle)
                {
                    string data = "{\"theater_end_exercise\":[],\"next_enemy_no\":\"2\"}";
                    ResponceProcessBinary(ctx, Encoding.UTF8.GetBytes(data), true, false);
                }
                else if (ctx.Request.Url.LocalPath == GF_URLs_Kr.TheaterBossBattle)
                {
                    Random rdm = new Random();
                    int score = rdm.Next(1, 999999);
                    string data = "{\"boss_score\":" + rdm.Next(1, 999999).ToString() + ",\"battle_pt\":" + score.ToString() + ",\"material_num\":1844}";
                    ResponceProcessBinary(ctx, Encoding.UTF8.GetBytes(data), true, false);
                }
                else if (ctx.Request.Url.LocalPath == GF_URLs_Kr.downloadsucsess)
                {
                    ResponceProcessBinary(ctx, Encoding.ASCII.GetBytes("1"), false, false);
                }
                //wc.Headers.Add("Accept","*/*");
                /*else if (ctx.Request.Url.AbsoluteUri.Contains("sn-list.girlfrontline.co.kr") && ctx.Request.Url.LocalPath.Contains(".txt"))
                {
                    WebClient wc = new WebClient();

                    wc.Headers.Add("Accept-Encoding", "gzip, deflate");
                    wc.Headers.Add("Accept-Language", "en-us");
                    wc.Headers.Add("User-Agent", ctx.Request.UserAgent);
                    wc.Headers.Add("X-Unity-Version", ctx.Request.Headers.GetValues("X-Unity-Version")[0]);
                    byte[] unitydata = wc.DownloadData("http://"+ctx.Request.Url.Host+ctx.Request.Url.AbsolutePath);

                    ResponceProcessBinary(ctx, unitydata, false,true);
                }*/
                else if (ctx.Request.Url.LocalPath == GF_URLs_Kr.crashreport)
                {
                    JObject temp = JObject.Parse(Packet.Decode(Outdatacode, signkey));
                    File.AppendAllText("CrashLog/client_crashlog_" + DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".txt", temp.ToString());
                    ResponceProcessBinary(ctx, Encoding.ASCII.GetBytes("1"), false, false);
                    frm.AddLog("(CrashReport)" + frm.lang_data["crashlog_log_msg"].ToString());
                }
                else if (ctx.Request.Url.LocalPath == GF_URLs_Kr.abortmission)
                {
                    frm.AddLog(frm.lang_data["client_force_reset_log_msg"].ToString());
                    ResponceProcessBinary(ctx, Encoding.ASCII.GetBytes("error:1"), false, false);
                }
                else if (ctx.Request.Url.LocalPath == GF_URLs_Kr.teammove || ctx.Request.Url.LocalPath == GF_URLs_Kr.squadMove)
                {
                    frm.AddLog(frm.lang_data["move_echelon"].ToString() + Packet.Decode(Outdatacode, signkey));
                    ResponceProcessBinary(ctx, Encoding.ASCII.GetBytes("1"), false, false);
                }
                else if (ctx.Request.Url.LocalPath == GF_URLs_Kr.squadSwitchChange)
                {
                    frm.AddLog(frm.lang_data["fire_unit_switch_change"].ToString() + Packet.Decode(Outdatacode, signkey));
                    ResponceProcessBinary(ctx, Encoding.ASCII.GetBytes("1"), false, false);
                }
                /*
                else if (ctx.Request.Url.LocalPath == GF_URLs_Kr.home)
                {
                    ResponceProcessBinary(ctx, Encoding.UTF8.GetBytes(File.ReadAllText(@"data/json/home.json")), true, false);
                }
                else if (ctx.Request.Url.LocalPath == GF_URLs_Kr.statictables)
                {
                    ResponceProcessBinary(ctx, Encoding.UTF8.GetBytes(File.ReadAllText(@"data/json/mail.json")), true, false);
                }
                else if (ctx.Request.Url.LocalPath == GF_URLs_Kr.getdorminfo)
                {
                    ResponceProcessBinary(ctx, Encoding.UTF8.GetBytes(File.ReadAllText(@"data/json/dorminfo.json")), true, false);
                }*/
                else if (ctx.Request.RawUrl.Contains("gf-game.girlfrontline.co.kr"))
                {
                    frm.AddLog(frm.lang_data["client_force_reset_log_msg"].ToString());
                    ResponceProcessBinary(ctx, Encoding.ASCII.GetBytes(frm.reset_error), false, false);
                }
                else
                {
                    //ResponceProcessBinary(ctx, Encoding.UTF8.GetBytes("??? ?? ??? ???"), false,false);
                    try
                    {
                        //uri = "http://gf-game.girlfrontline.co.kr" + uri;
                        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
                        request.Method = ctx.Request.HttpMethod;
                        request.KeepAlive = true;
                        request.UserAgent = ctx.Request.UserAgent;
                        request.ContentType = ctx.Request.ContentType;
                        request.Host = ctx.Request.Headers.GetValues("Host")[0];
                        for (int i = 0; i < ctx.Request.Headers.Count; i++)
                        {
                            if (ctx.Request.Headers.GetKey(i) != "Content-Length" &&
                                ctx.Request.Headers.GetKey(i) != "Content-Type" &&
                                ctx.Request.Headers.GetKey(i) != "Connection" &&
                                ctx.Request.Headers.GetKey(i) != "Transfer-Encoding" &&
                                ctx.Request.Headers.GetKey(i) != "Accept-Encoding" &&
                                ctx.Request.Headers.GetKey(i) != "Content-Length")
                                ctx.Response.Headers.Add(ctx.Request.Headers.GetKey(i), ctx.Request.Headers.GetValues(i)[0]);
                        }
                        //request.Headers.Add("X-Unity-Version", ctx.Request.Headers.GetValues("X-Unity-Version")[0]);
                        //request.Headers.Add("Accept-Encoding", "none");
                        //byte[] data = Encoding.UTF8.GetBytes(new StreamReader(ctx.Request.InputStream).ReadToEnd());
                        request.ContentLength = Client_Req_data.Length;
                        if (ctx.Request.HttpMethod == "POST")
                        {
                            using (Stream reqStream = request.GetRequestStream())
                            {
                                reqStream.Write(Client_Req_data, 0, Client_Req_data.Length);
                            }
                        }
                        //if(ctx.Request.HttpMethod == "POST")
                        //{
                        try
                        {
                            using (WebResponse resp = request.GetResponse())
                            {
                                byte[] b = null;
                                Stream respStream = resp.GetResponseStream();
                                using (MemoryStream ms = new MemoryStream())
                                {
                                    int count = 0;
                                    do
                                    {
                                        byte[] buf = new byte[1024];
                                        count = respStream.Read(buf, 0, 1024);
                                        ms.Write(buf, 0, count);
                                    } while (respStream.CanRead && count > 0);
                                    b = ms.ToArray();
                                }

                                for (int i = 0; i < resp.Headers.Count; i++)
                                {
                                    if (resp.Headers.GetKey(i) != "Content-Length" &&
                                        resp.Headers.GetKey(i) != "Content-Type" &&
                                        resp.Headers.GetKey(i) != "Connection" &&
                                        resp.Headers.GetKey(i) != "Transfer-Encoding" &&
                                        resp.Headers.GetKey(i) != "Content-Length" &&
                                        resp.Headers.GetKey(i) != "Content-Length")
                                        ctx.Response.Headers.Add(resp.Headers.GetKey(i), resp.Headers.GetValues(i)[0]);
                                }
                                ctx.Response.KeepAlive = true;
                                ctx.Response.ContentType = resp.ContentType;
                                ResponceProcessBinary(ctx, b, false, true);
                                //frm.textBox1.AppendText(packet+"\n");
                            }

                        }
                        catch (Exception ex)
                        {
                            frm.AddLog("Error: " + ex.ToString());
                        }
                        //  }
                        //ctx.Response.Headers.


                    }

                    catch (Exception ex)
                    {
                        frm.AddLog("Error: " + ex.ToString());
                    }
                }
            }
            else
            {
                // Packet.init();
                try
                {
                    //uri = "http://gf-game.girlfrontline.co.kr" + uri;
                    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
                    request.Method = ctx.Request.HttpMethod;
                    request.KeepAlive = true;
                    request.UserAgent = ctx.Request.UserAgent;
                    request.ContentType = ctx.Request.ContentType;
                    request.Host = ctx.Request.Headers.GetValues("Host")[0];
                    for (int i = 0; i < ctx.Request.Headers.Count; i++)
                    {
                        if (ctx.Request.Headers.GetKey(i) != "Content-Length" &&
                            ctx.Request.Headers.GetKey(i) != "Content-Type" &&
                            ctx.Request.Headers.GetKey(i) != "Connection" &&
                            ctx.Request.Headers.GetKey(i) != "Transfer-Encoding" &&
                            ctx.Request.Headers.GetKey(i) != "Accept-Encoding" &&
                            ctx.Request.Headers.GetKey(i) != "Content-Length")
                            ctx.Response.Headers.Add(ctx.Request.Headers.GetKey(i), ctx.Request.Headers.GetValues(i)[0]);
                    }
                    //request.Headers.Add("X-Unity-Version", ctx.Request.Headers.GetValues("X-Unity-Version")[0]);
                    //request.Headers.Add("Accept-Encoding", "none");
                    //byte[] data = Encoding.UTF8.GetBytes(new StreamReader(ctx.Request.InputStream).ReadToEnd());
                    request.ContentLength = Client_Req_data.Length;
                    if (ctx.Request.HttpMethod == "POST")
                    {
                        using (Stream reqStream = request.GetRequestStream())
                        {
                            reqStream.Write(Client_Req_data, 0, Client_Req_data.Length);
                        }
                    }
                    //if(ctx.Request.HttpMethod == "POST")
                    //{
                    try
                    {


                        using (WebResponse resp = request.GetResponse())
                        {
                            byte[] b = null;
                            Stream respStream = resp.GetResponseStream();
                            using (MemoryStream ms = new MemoryStream())
                            {
                                int count = 0;
                                do
                                {
                                    byte[] buf = new byte[1024];
                                    count = respStream.Read(buf, 0, 1024);
                                    ms.Write(buf, 0, count);
                                } while (respStream.CanRead && count > 0);
                                b = ms.ToArray();
                            }

                            for (int i = 0; i < resp.Headers.Count; i++)
                            {
                                if (resp.Headers.GetKey(i) != "Content-Length" &&
                                    resp.Headers.GetKey(i) != "Content-Type" &&
                                    resp.Headers.GetKey(i) != "Connection" &&
                                    resp.Headers.GetKey(i) != "Transfer-Encoding" &&
                                    resp.Headers.GetKey(i) != "Content-Length" &&
                                    resp.Headers.GetKey(i) != "Content-Length")
                                    ctx.Response.Headers.Add(resp.Headers.GetKey(i), resp.Headers.GetValues(i)[0]);
                            }
                            ctx.Response.KeepAlive = true;
                            ctx.Response.ContentType = resp.ContentType;

                            string Serverpacket = Encoding.UTF8.GetString(b);
                            if (ctx.Request.Url.LocalPath == GF_URLs_Kr.index_version)
                            {
                                frm.serv.AddLog(frm.lang_data["get_versioninfo_log"].ToString());
                            }
                            else if (ctx.Request.Url.LocalPath == GF_URLs_Kr.getToken)
                            {
                                Packet.init();
                                if (Serverpacket.Contains("error"))
                                {
                                    frm.serv.AddLog(frm.lang_data["login_log"].ToString());
                                }
                                else
                                {
                                    JObject tok = JObject.Parse(Packet.Decode(Serverpacket, "yundoudou"));
                                    frm._token = tok["sign"].ToString();
                                    frm.serv.AddLog(frm.lang_data["get_token_success_log"].ToString());
                                }
                            }
                            else if (ctx.Request.Url.LocalPath == GF_URLs_Kr.getuserinfo)
                            {
                                JObject uinfo = JObject.Parse(Packet.Decode(Serverpacket, frm._token));
                                frm.serv.getUserdataFromServer(uinfo);
                            }
                            ResponceProcessBinary(ctx, b, false, true);

                        }

                    }
                    catch (Exception ex)
                    {
                        frm.serv.AddLog("Error: " + ex.ToString());
                    }
                    //  }
                    //ctx.Response.Headers.


                }

                catch (Exception ex)
                {
                    frm.serv.AddLog("Error: " + ex.ToString());
                }

            }

            ctx.Response.Close();
        }
        private static void WorkerThread(object arg)
        {

            //HttpListener listener = (HttpListener)arg;

            try
            {
                while (frm.listener.IsListening)
                {
                    HttpListenerContext ctx = frm.listener.GetContext();
                    ProcessRequest(ctx);
                }
            }
            catch (ThreadAbortException)
            {
                //frm.textBox1.AppendText("Normal Stopping Service");
            }
            //catch (Exception ex)
            //{
            //   frm.textBox1.AppendText(string.Format("Exception !!! Stop Service...\n\n{0}", ex.ToString()));
            //}

        }

        private static void ResponceProcessBinary(HttpListenerContext ctx, byte[] data, bool encrypt, bool isHeaderSettedAlready)
        {
            if (encrypt)
                data = Encoding.UTF8.GetBytes(Packet.EncodeWithGzip(Encoding.UTF8.GetString(data), signkey));
            //HttpListenerRequest request = ctx.Request;
            HttpListenerResponse response = ctx.Response;
            //?? ??
            if (!isHeaderSettedAlready)
            {
                response.Headers.Add("Accept-Encoding", "none"); //gzip ???? ????? ???
                response.Headers.Add("Content-Type", "text/html; charset=UTF-8");
                response.Headers.Add("Server", "ngnix");
                response.Headers.Add("X-Powered-By", " PHP/5.6.21");
                response.Headers.Add("X-Upstream", " 127.0.0.1:8080");
            }
            //??? ??
            response.ContentLength64 = data.Length; //??? ?? ??
            Stream output = response.OutputStream;
            output.Write(data, 0, data.Length);
            if (frm.showdetailLog_checkbox.Checked)
                frm.AddLog("Responce: " + Encoding.UTF8.GetString(data));
        }

        private void button6_Click(object sender, EventArgs e)
        {

            listener.Stop();
            Thread.Sleep(100);
            /* if (thread.IsAlive)
             {
                 thread.Abort();
                 thread.Join();
             }
             */
            //listener.Close();
        }
        void gun_info_refresh()
        {

        }
        private void enemy_team_id_combobox_SelectedIndexChanged(object sender, EventArgs e)
        {
            //ChangeEnemyGroupID();
        }
        private void enemy_team_id_combobox_TextUpdate(object sender, EventArgs e)
        {
            //ChangeEnemyGroupID();
        }
        void ChangeEnemyGroupID()
        {
            int bosshp = 0;
            try
            {
                if (int.TryParse(enemy_team_id_combobox.Text, out bosshp))
                {
                    bosshp = int.Parse(enemy_character_info[enemy_team_info[enemy_team_id_combobox.Text]["enemy_leader"].ToString()]["boss_hp"].ToString());
                    Boss_HP_textbox.Value = bosshp;
                    for (int i = 0; i < spotactinfo.Count(); i++)
                    {
                        if (spotactinfo[i]["spot_id"].ToString() == battlespot)
                        {
                            spotactinfo[i]["enemy_team_id"] = enemy_team_id_combobox.Text;
                            spotactinfo[i]["boss_hp"] = Boss_HP_textbox.Value.ToString();
                            AddLog(lang_data["changed_groupID"].ToString() + spotactinfo[i]["enemy_team_id"].ToString());
                            break;
                        }
                    }
                    JObject p = new JObject();
                    for (int k = 0; k < spotactinfo.Count(); k++)
                    {
                        p.Add(spotactinfo[k]["spot_id"].ToString(), spotactinfo[k]);
                    }
                    missionactinfo["spot"].Replace(JsonConvert.SerializeObject(p, Formatting.None));
                    //MessageBox.Show(userinfo["mission_act_info"]["spot"].ToString());
                    server_settedID.Text = lang_data["server_setted_id"].ToString() + enemy_team_id_combobox.Text;
                    File.WriteAllText(@"data/last_enemyID", enemy_team_id_combobox.Text);
                }
                else
                    MessageBox.Show(lang_data["enter_only_number_msg"].ToString(), lang_data["error"].ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
            catch (NullReferenceException)
            {
                if (MessageBox.Show(lang_data["db_notfound_msg"].ToString(), lang_data["id_cannotfound"].ToString(), MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
                {
                    bosshp = 0;
                    Boss_HP_textbox.Value = bosshp;
                    for (int i = 0; i < spotactinfo.Count(); i++)
                    {
                        if (spotactinfo[i]["spot_id"].ToString() == battlespot)
                        {
                            spotactinfo[i]["enemy_team_id"] = enemy_team_id_combobox.Text;
                            spotactinfo[i]["boss_hp"] = Boss_HP_textbox.Value.ToString();
                            AddLog(lang_data["changed_groupID"].ToString() + spotactinfo[i]["enemy_team_id"].ToString());
                            break;
                        }
                    }
                    server_settedID.Text = lang_data["server_setted_id"].ToString() + enemy_team_id_combobox.Text;

                }
            }
        }
        void ChangeEnemyBossHP()
        {
            for (int i = 0; i < spotactinfo.Count(); i++)
            {
                if (spotactinfo[i]["spot_id"].ToString() == battlespot)
                {
                    spotactinfo[i]["boss_hp"] = Boss_HP_textbox.Value.ToString();
                    AddLog(lang_data["changed_bossHP"].ToString() + spotactinfo[i]["boss_hp"].ToString());
                    break;
                }
            }
            File.WriteAllText(@"data/last_bossHP", Boss_HP_textbox.Value.ToString());
        }
        bool check_gun(int o)
        {
            switch (o)
            {
                case 1:
                    if (enable_1.Checked)
                        return true;
                    return false;
                case 2:
                    if (enable_2.Checked)
                        return true;
                    return false;
                case 3:
                    if (enable_3.Checked)
                        return true;
                    return false;
                case 4:
                    if (enable_4.Checked)
                        return true;
                    return false;
                case 5:
                    if (enable_5.Checked)
                        return true;
                    return false;
                default:
                    return false;
            }
        }
        private void button4_Click(object sender, EventArgs e)
        {
            SetGunInfo(true, echelon_select.SelectedIndex);

        }
        void SetGunInfo(bool showMessageBox, int echelon)
        {
            bool duplicated = false;
            for (int i = 1; i <= 5; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    List<int> n = new List<int>() { 1, 2, 3, 4, 5 };
                    n.Remove(i);
                    NumericUpDown n1 = (NumericUpDown)Controls.Find("gunpos_" + i.ToString() + "_number", true)[0];
                    NumericUpDown n2 = (NumericUpDown)Controls.Find("gunpos_" + n[j].ToString() + "_number", true)[0];
                    if (n1.Value == n2.Value)
                    {
                        CheckBox c1 = (CheckBox)Controls.Find("enable_" + i.ToString(), true)[0];
                        CheckBox c2 = (CheckBox)Controls.Find("enable_" + n[j].ToString(), true)[0];
                        if (c1.Checked)
                        {
                            if (c2.Checked)
                                duplicated = true;
                        }

                    }
                }
            }
            if (/*gunid_1_combobox.SelectedIndex == gunid_2_combobox.SelectedIndex ||
               gunid_1_combobox.SelectedIndex == gunid_3_combobox.SelectedIndex ||
               gunid_1_combobox.SelectedIndex == gunid_4_combobox.SelectedIndex ||
               gunid_1_combobox.SelectedIndex == gunid_5_combobox.SelectedIndex ||
               gunid_2_combobox.SelectedIndex == gunid_1_combobox.SelectedIndex ||
              gunid_2_combobox.SelectedIndex == gunid_3_combobox.SelectedIndex ||
              gunid_2_combobox.SelectedIndex == gunid_4_combobox.SelectedIndex ||
              gunid_2_combobox.SelectedIndex == gunid_5_combobox.SelectedIndex ||
              gunid_3_combobox.SelectedIndex == gunid_2_combobox.SelectedIndex ||
              gunid_3_combobox.SelectedIndex == gunid_1_combobox.SelectedIndex ||
              gunid_3_combobox.SelectedIndex == gunid_4_combobox.SelectedIndex ||
              gunid_3_combobox.SelectedIndex == gunid_5_combobox.SelectedIndex ||
              gunid_4_combobox.SelectedIndex == gunid_2_combobox.SelectedIndex ||
              gunid_4_combobox.SelectedIndex == gunid_3_combobox.SelectedIndex ||
              gunid_4_combobox.SelectedIndex == gunid_1_combobox.SelectedIndex ||
              gunid_4_combobox.SelectedIndex == gunid_5_combobox.SelectedIndex ||
              gunid_5_combobox.SelectedIndex == gunid_2_combobox.SelectedIndex ||
              gunid_5_combobox.SelectedIndex == gunid_3_combobox.SelectedIndex ||
              gunid_5_combobox.SelectedIndex == gunid_4_combobox.SelectedIndex ||
              gunid_5_combobox.SelectedIndex == gunid_1_combobox.SelectedIndex*/
              false)
            {
                MessageBox.Show("?? ??? ??? ??? ? ????.", "??", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if (duplicated)
            {
                MessageBox.Show(lang_data["duplicate_formation_msg"].ToString(), lang_data["error"].ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            else if (gunid_1_combobox.SelectedIndex == -1 || gunid_2_combobox.SelectedIndex == -1
                || gunid_3_combobox.SelectedIndex == -1 || gunid_4_combobox.SelectedIndex == -1
                || gunid_5_combobox.SelectedIndex == -1)
            {
                MessageBox.Show(lang_data["gun_id_error_msg"].ToString(), lang_data["error"].ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                if (gunid_1_combobox.SelectedIndex == gunid_2_combobox.SelectedIndex ||
               gunid_1_combobox.SelectedIndex == gunid_3_combobox.SelectedIndex ||
               gunid_1_combobox.SelectedIndex == gunid_4_combobox.SelectedIndex ||
               gunid_1_combobox.SelectedIndex == gunid_5_combobox.SelectedIndex ||
               gunid_2_combobox.SelectedIndex == gunid_1_combobox.SelectedIndex ||
              gunid_2_combobox.SelectedIndex == gunid_3_combobox.SelectedIndex ||
              gunid_2_combobox.SelectedIndex == gunid_4_combobox.SelectedIndex ||
              gunid_2_combobox.SelectedIndex == gunid_5_combobox.SelectedIndex ||
              gunid_3_combobox.SelectedIndex == gunid_2_combobox.SelectedIndex ||
              gunid_3_combobox.SelectedIndex == gunid_1_combobox.SelectedIndex ||
              gunid_3_combobox.SelectedIndex == gunid_4_combobox.SelectedIndex ||
              gunid_3_combobox.SelectedIndex == gunid_5_combobox.SelectedIndex ||
              gunid_4_combobox.SelectedIndex == gunid_2_combobox.SelectedIndex ||
              gunid_4_combobox.SelectedIndex == gunid_3_combobox.SelectedIndex ||
              gunid_4_combobox.SelectedIndex == gunid_1_combobox.SelectedIndex ||
              gunid_4_combobox.SelectedIndex == gunid_5_combobox.SelectedIndex ||
              gunid_5_combobox.SelectedIndex == gunid_2_combobox.SelectedIndex ||
              gunid_5_combobox.SelectedIndex == gunid_3_combobox.SelectedIndex ||
              gunid_5_combobox.SelectedIndex == gunid_4_combobox.SelectedIndex ||
              gunid_5_combobox.SelectedIndex == gunid_1_combobox.SelectedIndex)
                    MessageBox.Show(lang_data["gun_duplicate_skill_error_warning_msg"].ToString(), lang_data["warning"].ToString(), MessageBoxButtons.OK, MessageBoxIcon.Information);
                JArray gun_user_info = JArray.Parse(userinfo["gun_with_user_info"].ToString());
                JArray gun_info_in_theater = JArray.Parse(userinfo["gun_in_theater_info"].ToString());
                gun_user_info.RemoveAll();
                gun_info_in_theater.RemoveAll();

                for (int i = 1; i <= 5; i++)
                {
                    Get_user_gun_info_json(i, echelon);
                    JObject o = new JObject();
                    for (int k = 0; k < gun_eh_array.Count(); k++)
                    {
                        o = new JObject((JObject)gun_eh_array[k]);
                        if (gun_eh_array[k]["id"].ToString() == o["id"].ToString())
                        {
                            gun_eh_array[k].Replace(o);
                        }
                    }
                    gun_user_info.Add(o);

                }
                foreach (var a in gun_eh_array)
                {
                    JObject n = new JObject();
                    n.Add("user_id", "20139");
                    n.Add("team_id", a["team_id"]);
                    n.Add("location", a["location"]);
                    n.Add("gun_with_user_id", a["id"]);
                    n.Add("position", a["position"]);
                    n.Add("life", a["life"]);
                    gun_info_in_theater.Add(n);
                }


                // gun_user_info.Add(gun_info_array);

                userinfo["gun_with_user_info"].Replace(gun_eh_array);
                userinfo["gun_in_theater_info"].Replace(gun_info_in_theater);
                //MessageBox.Show(Get_user_gun_info_json(1).ToString());
                if (showMessageBox)
                    MessageBox.Show(lang_data["saved_msg"].ToString(), lang_data["alert"].ToString(), MessageBoxButtons.OK, MessageBoxIcon.Information);
                gunsaved = true;
                //Clipboard.SetText(gun_eh_array.ToString());
            }
        }
        string getModState(int lv)
        {
            if (lv < 110)
                return "1";
            else if (lv < 115)
                return "2";
            else if (lv <= 120)
                return "3";
            else
                return "0";
        }

        void Get_user_gun_info_json(int num, int e)
        {
            JObject defStat = new JObject();
            for (int k = 0; k < gun_eh_array.Count(); k++)
            {
                if (gun_eh_array[k]["id"].ToString() == gun_user_id[e, num - 1].ToString())
                {
                    gun_eh_array[k]["gun_id"] = int.Parse(gun_info[((ComboBox)Controls.Find("gunid_" + num.ToString() + "_combobox", true)[0]).SelectedIndex].Split(',')[0]).ToString();
                    gun_eh_array[k]["gun_level"] = ((NumericUpDown)Controls.Find("gunlv_" + num.ToString() + "_number", true)[0]).Value.ToString();
                    gun_eh_array[k]["gun_exp"] = gun_exp_table[Convert.ToInt16(((NumericUpDown)Controls.Find("gunlv_" + num.ToString() + "_number", true)[0]).Value - 1)].ToString();
                    gun_eh_array[k]["location"] = num.ToString();
                    gun_eh_array[k]["position"] = gun_position[Convert.ToInt16(((NumericUpDown)Controls.Find("gunpos_" + num.ToString() + "_number", true)[0]).Value)].ToString();
                    gun_eh_array[k]["if_modification"] = check_ifmod(num) ? getModState((int)((NumericUpDown)Controls.Find("gunlv_" + num.ToString() + "_number", true)[0]).Value) : "0";
                    gun_eh_array[k]["life"] = int.Parse(((NumericUpDown)Controls.Find("gunhp_" + num.ToString() + "_number", true)[0]).Value.ToString());
                    gun_eh_array[k]["pow"] = (((NumericUpDown)Controls.Find("gunpow_" + num.ToString() + "_number", true)[0]).Value - ((NumericUpDown)Controls.Find("gunpow_" + num.ToString() + "_number", true)[0]).Minimum).ToString();
                    gun_eh_array[k]["hit"] = (((NumericUpDown)Controls.Find("gunhit_" + num.ToString() + "_number", true)[0]).Value - ((NumericUpDown)Controls.Find("gunhit_" + num.ToString() + "_number", true)[0]).Minimum).ToString();
                    gun_eh_array[k]["dodge"] = (((NumericUpDown)Controls.Find("gundodge_" + num.ToString() + "_number", true)[0]).Value - ((NumericUpDown)Controls.Find("gundodge_" + num.ToString() + "_number", true)[0]).Minimum).ToString();
                    gun_eh_array[k]["rate"] = (((NumericUpDown)Controls.Find("gunrate_" + num.ToString() + "_number", true)[0]).Value - ((NumericUpDown)Controls.Find("gunrate_" + num.ToString() + "_number", true)[0]).Minimum).ToString();
                    gun_eh_array[k]["skill1"] = ((NumericUpDown)Controls.Find("gunskill1_" + num.ToString() + "_number", true)[0]).Value.ToString();
                    gun_eh_array[k]["skill2"] = ((NumericUpDown)Controls.Find("gunskill2_" + num.ToString() + "_number", true)[0]).Value.ToString(); //?? ???? 0
                    gun_eh_array[k]["number"] = ((NumericUpDown)Controls.Find("gundummy_" + num.ToString() + "_number", true)[0]).Value.ToString();
                    gun_eh_array[k]["favor"] = (((NumericUpDown)Controls.Find("gunfavor_" + num.ToString() + "_number", true)[0]).Value * 10000).ToString();
                    gun_eh_array[k]["soul_bond"] = ((CheckBox)Controls.Find("gunoath_" + num.ToString() + "_checkbox", true)[0]).Checked ? "1" : "0";
                    gun_eh_array[k]["skin"] = "0";
                    gun_eh_array[k]["team_id"] = ((CheckBox)Controls.Find("enable_" + num.ToString(), true)[0]).Checked ? getTeamIDfromID(gun_eh_array[k]["id"].ToString()).ToString() : "0";
                }
            }
            /*
            if (num == 1)
            {
                defStat = new JObject(getGunDefaultStat(1, int.Parse(gun_info[gunid_1_combobox.SelectedIndex].Split(',')[0].ToString()), 1));
                gun_info_json_1["id"] = gun_user_id[e, num - 1].ToString();
                gun_info_json_1["gun_id"] = int.Parse(gun_info[gunid_1_combobox.SelectedIndex].Split(',')[0]).ToString();
                gun_info_json_1["gun_exp"] = gun_exp_table[Convert.ToInt16(gunlv_1_number.Value - 1)].ToString();
                gun_info_json_1["gun_level"] = gunlv_1_number.Value.ToString();
                gun_info_json_1["team_id"] = check_gun(num) ? (e+1).ToString() : "0";
                gun_info_json_1["if_modification"] = check_ifmod(1) ? getModState((int)gunlv_1_number.Value) : "0";
                gun_info_json_1["location"] = num.ToString();
                gun_info_json_1["position"] = gun_position[Convert.ToInt16(gunpos_1_number.Value)].ToString();
                gun_info_json_1["life"] = gunhp_1_number.Value.ToString();
                gun_info_json_1["pow"] = (gunpow_1_number.Value - gunpow_1_number.Minimum).ToString();
                gun_info_json_1["hit"] = (gunhit_1_number.Value - gunhit_1_number.Minimum).ToString();
                gun_info_json_1["dodge"] = (gundodge_1_number.Value - gundodge_1_number.Minimum).ToString();
                gun_info_json_1["rate"] = (gunrate_1_number.Value - gunrate_1_number.Minimum).ToString();
                gun_info_json_1["skill1"] = gunskill1_1_number.Value.ToString();
                gun_info_json_1["skill2"] = gunskill2_1_number.Value.ToString();
                gun_info_json_1["number"] = gundummy_1_number.Value.ToString();
                //gun_info_json_1["equip1"] = "6";
                //gun_info_json_1["equip2"] = "7";
                //gun_info_json_1["equip3"] = "0";
                gun_info_json_1["favor"] = (gunfavor_1_number.Value * 10000).ToString();
                gun_info_json_1["max_favor"] = "2000000";
                gun_info_json_1["favor_toplimit"] = "2000000";
                gun_info_json_1["soul_bond"] = (gunoath_1_checkbox.Checked ? "1" : "0");
                gun_info_json_1["skin"] = "0";
                return gun_info_json_1;
            }
            else if (num == 2)
            {
                defStat = new JObject(getGunDefaultStat(1, int.Parse(gun_info[gunid_2_combobox.SelectedIndex].Split(',')[0].ToString()), 1));
                gun_info_json_2["id"] = gun_user_id[e, num - 1].ToString();
                gun_info_json_2["gun_id"] = int.Parse(gun_info[gunid_2_combobox.SelectedIndex].Split(',')[0]).ToString();
                gun_info_json_2["gun_exp"] = gun_exp_table[Convert.ToInt16(gunlv_2_number.Value - 1)].ToString();
                gun_info_json_2["gun_level"] = gunlv_2_number.Value.ToString();
                gun_info_json_2["team_id"] = check_gun(num) ? (e+1).ToString() : "0";
                gun_info_json_2["if_modification"] = check_ifmod(2) ? getModState((int)gunlv_2_number.Value) : "0";
                gun_info_json_2["location"] = num.ToString();
                gun_info_json_2["position"] = gun_position[Convert.ToInt16(gunpos_2_number.Value)].ToString();
                gun_info_json_2["life"] = gunhp_2_number.Value.ToString();
                gun_info_json_2["pow"] = (gunpow_2_number.Value - gunpow_2_number.Minimum).ToString();
                gun_info_json_2["hit"] = (gunhit_2_number.Value - gunhit_2_number.Minimum).ToString();
                gun_info_json_2["dodge"] = (gundodge_2_number.Value - gundodge_2_number.Minimum).ToString();
                gun_info_json_2["rate"] = (gunrate_2_number.Value - gunrate_2_number.Minimum).ToString();
                gun_info_json_2["skill1"] = gunskill1_2_number.Value.ToString();
                gun_info_json_2["skill2"] = gunskill1_2_number.Value.ToString();
                gun_info_json_2["number"] = gundummy_2_number.Value.ToString();
                //gun_info_json_2["equip1"] = "0";
                //gun_info_json_2["equip2"] = "0";
                //gun_info_json_2["equip3"] = "0";
                gun_info_json_2["favor"] = (gunfavor_2_number.Value * 10000).ToString();
                gun_info_json_2["max_favor"] = "2000000";
                gun_info_json_2["favor_toplimit"] = "2000000";
                gun_info_json_2["soul_bond"] = (gunoath_2_checkbox.Checked ? "1" : "0");
                gun_info_json_2["skin"] = "0";
                return gun_info_json_2;
            }
            else if (num == 3)
            {
                defStat = new JObject(getGunDefaultStat(1, int.Parse(gun_info[gunid_3_combobox.SelectedIndex].Split(',')[0].ToString()), 1));
                gun_info_json_3["id"] = gun_user_id[e, num - 1].ToString();
                gun_info_json_3["gun_id"] = int.Parse(gun_info[gunid_3_combobox.SelectedIndex].Split(',')[0]).ToString();
                gun_info_json_3["gun_exp"] = gun_exp_table[Convert.ToInt16(gunlv_3_number.Value - 1)].ToString();
                gun_info_json_3["gun_level"] = gunlv_3_number.Value.ToString();
                gun_info_json_3["team_id"] = check_gun(num) ? (e+1).ToString() : "0";
                gun_info_json_3["if_modification"] = check_ifmod(3) ? getModState((int)gunlv_3_number.Value) : "0";
                gun_info_json_3["location"] = num.ToString();
                gun_info_json_3["position"] = gun_position[Convert.ToInt16(gunpos_3_number.Value)].ToString();
                gun_info_json_3["life"] = gunhp_3_number.Value.ToString();
                gun_info_json_3["pow"] = (gunpow_3_number.Value - gunpow_3_number.Minimum).ToString();
                gun_info_json_3["hit"] = (gunhit_3_number.Value - gunhit_3_number.Minimum).ToString();
                gun_info_json_3["dodge"] = (gundodge_3_number.Value - gundodge_3_number.Minimum).ToString();
                gun_info_json_3["rate"] = (gunrate_3_number.Value - gunrate_3_number.Minimum).ToString();
                gun_info_json_3["skill1"] = gunskill1_3_number.Value.ToString();
                gun_info_json_3["skill2"] = gunskill1_3_number.Value.ToString();
                gun_info_json_3["number"] = gundummy_3_number.Value.ToString();
                //gun_info_json_3["equip1"] = "0";
                //gun_info_json_3["equip2"] = "0";
                //gun_info_json_3["equip3"] = "0";
                gun_info_json_3["favor"] = (gunfavor_3_number.Value * 10000).ToString();
                gun_info_json_3["max_favor"] = "2000000";
                gun_info_json_3["favor_toplimit"] = "2000000";
                gun_info_json_3["soul_bond"] = (gunoath_3_checkbox.Checked ? "1" : "0");
                gun_info_json_3["skin"] = "0";
                return gun_info_json_3;
            }
            else if (num == 4)
            {
                defStat = new JObject(getGunDefaultStat(1, int.Parse(gun_info[gunid_4_combobox.SelectedIndex].Split(',')[0].ToString()), 1));
                gun_info_json_4["id"] = gun_user_id[e, num - 1].ToString();
                gun_info_json_4["gun_id"] = int.Parse(gun_info[gunid_4_combobox.SelectedIndex].Split(',')[0]).ToString();
                gun_info_json_4["gun_exp"] = gun_exp_table[Convert.ToInt16(gunlv_4_number.Value - 1)].ToString();
                gun_info_json_4["gun_level"] = gunlv_4_number.Value.ToString();
                gun_info_json_4["team_id"] = check_gun(num) ? (e+1).ToString() : "0";
                gun_info_json_4["if_modification"] = check_ifmod(4) ? getModState((int)gunlv_4_number.Value) : "0";
                gun_info_json_4["location"] = num.ToString();
                gun_info_json_4["position"] = gun_position[Convert.ToInt16(gunpos_4_number.Value)].ToString();
                gun_info_json_4["life"] = gunhp_4_number.Value.ToString();
                gun_info_json_4["pow"] = (gunpow_4_number.Value - gunpow_4_number.Minimum).ToString();
                gun_info_json_4["hit"] = (gunhit_4_number.Value - gunhit_4_number.Minimum).ToString();
                gun_info_json_4["dodge"] = (gundodge_4_number.Value - gundodge_4_number.Minimum).ToString();
                gun_info_json_4["rate"] = (gunrate_4_number.Value - gunrate_4_number.Minimum).ToString();
                gun_info_json_4["skill1"] = gunskill1_4_number.Value.ToString();
                gun_info_json_4["skill2"] = gunskill1_4_number.Value.ToString();
                gun_info_json_4["number"] = gundummy_4_number.Value.ToString();
                //gun_info_json_4["equip1"] = "0";
                //gun_info_json_4["equip2"] = "0";
                //gun_info_json_4["equip3"] = "0";
                gun_info_json_4["favor"] = (gunfavor_4_number.Value * 10000).ToString();
                gun_info_json_4["max_favor"] = "2000000";
                gun_info_json_4["favor_toplimit"] = "2000000";
                gun_info_json_4["soul_bond"] = (gunoath_4_checkbox.Checked ? "1" : "0");
                gun_info_json_4["skin"] = "0";
                return gun_info_json_4;
            }
            else if (num == 5)
            {
                defStat = new JObject(getGunDefaultStat(1, int.Parse(gun_info[gunid_5_combobox.SelectedIndex].Split(',')[0].ToString()), 1));
                gun_info_json_5["id"] = gun_user_id[e, num - 1].ToString();
                gun_info_json_5["gun_id"] = int.Parse(gun_info[gunid_5_combobox.SelectedIndex].Split(',')[0]).ToString();
                gun_info_json_5["gun_exp"] = gun_exp_table[Convert.ToInt16(gunlv_5_number.Value - 1)].ToString();
                gun_info_json_5["gun_level"] = gunlv_5_number.Value.ToString();
                gun_info_json_5["team_id"] = check_gun(num) ? (e+1).ToString() : "0";
                gun_info_json_5["if_modification"] = check_ifmod(5) ? getModState((int)gunlv_5_number.Value) : "0";
                gun_info_json_5["location"] = num.ToString();
                gun_info_json_5["position"] = gun_position[Convert.ToInt16(gunpos_5_number.Value)].ToString();
                gun_info_json_5["life"] = gunhp_5_number.Value.ToString();
                gun_info_json_5["pow"] = (gunpow_5_number.Value - gunpow_5_number.Minimum).ToString();
                gun_info_json_5["hit"] = (gunhit_5_number.Value - gunhit_5_number.Minimum).ToString();
                gun_info_json_5["dodge"] = (gundodge_5_number.Value - gundodge_5_number.Minimum).ToString();
                gun_info_json_5["rate"] = (gunrate_5_number.Value - gunrate_5_number.Minimum).ToString();
                gun_info_json_5["skill1"] = gunskill1_5_number.Value.ToString();
                gun_info_json_5["skill2"] = gunskill1_5_number.Value.ToString();
                gun_info_json_5["number"] = gundummy_5_number.Value.ToString();
                //gun_info_json_5["equip1"] = "0";
                //gun_info_json_5["equip2"] = "0";
                //gun_info_json_5["equip3"] = "0";
                gun_info_json_5["favor"] = (gunfavor_5_number.Value * 10000).ToString();
                gun_info_json_5["max_favor"] = "2000000";
                gun_info_json_5["favor_toplimit"] = "2000000";
                gun_info_json_5["soul_bond"] = (gunoath_5_checkbox.Checked ? "1" : "0");
                gun_info_json_5["skin"] = "0";
                return gun_info_json_5;
            }
            else
                return null;*/
        }
        private void enable_1_CheckedChanged(object sender, EventArgs e)
        {
            enable_set(1);
            UpdatePosTile(null, null);
        }

        private void enable_2_CheckedChanged(object sender, EventArgs e)
        {
            enable_set(2);
            UpdatePosTile(null, null);
        }

        private void enable_3_CheckedChanged(object sender, EventArgs e)
        {
            enable_set(3);
            UpdatePosTile(null, null);
        }

        private void enable_4_CheckedChanged(object sender, EventArgs e)
        {
            enable_set(4);
            UpdatePosTile(null, null);
        }

        private void enable_5_CheckedChanged(object sender, EventArgs e)
        {
            enable_set(5);
            UpdatePosTile(null, null);
        }
        void enable_set(int num)
        {
            if (enable_1.Checked)
                groupBox1.Enabled = true;
            else
                groupBox1.Enabled = false;

            if (enable_2.Checked)
                groupBox2.Enabled = true;
            else
                groupBox2.Enabled = false;

            if (enable_3.Checked)
                groupBox3.Enabled = true;
            else
                groupBox3.Enabled = false;

            if (enable_4.Checked)
                groupBox4.Enabled = true;
            else
                groupBox4.Enabled = false;

            if (enable_5.Checked)
                groupBox5.Enabled = true;
            else
                groupBox5.Enabled = false;

            if (!enable_1.Checked && !enable_2.Checked && !enable_3.Checked && !enable_4.Checked && !enable_5.Checked)
            {
                switch (num)
                {
                    case 1:
                        enable_1.Checked = true;
                        break;
                    case 2:
                        enable_2.Checked = true;
                        break;
                    case 3:
                        enable_3.Checked = true;
                        break;
                    case 4:
                        enable_4.Checked = true;
                        break;
                    case 5:
                        enable_5.Checked = true;
                        break;
                }
            }
        }
        void enable_set_sqd()
        {
            if (sqdswitch_1.Checked)
                groupBox8.Enabled = true;
            else
                groupBox8.Enabled = false;

            if (sqdswitch_2.Checked)
                groupBox9.Enabled = true;
            else
                groupBox9.Enabled = false;

            if (sqdswitch_3.Checked)
                groupBox10.Enabled = true;
            else
                groupBox10.Enabled = false;

            if (sqdswitch_4.Checked)
                groupBox11.Enabled = true;
            else
                groupBox11.Enabled = false;

            if (sqdswitch_5.Checked)
                groupBox12.Enabled = true;
            else
                groupBox12.Enabled = false;

            if (sqdswitch_6.Checked)
                groupBox21.Enabled = true;
            else
                groupBox21.Enabled = false;
        }
        private void button1_Click(object sender, EventArgs e)
        {
            saveFileDialog1.ShowDialog();
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("https://gf.underseaworld.net/ko/maps/");
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            JObject sett = new JObject();
            sett.Add("showDetailLog", showdetailLog_checkbox.Checked);
            sett.Add("checkUpdate", updatecheck_checkbox.Checked);
            sett.Add("disableLog", nolog_checkbox.Checked);
            sett.Add("language", languageList_Combobox.SelectedItem.ToString());
            File.WriteAllText(@"data/json/settings.json", sett.ToString());

            listener.Abort();
            if (thread.IsAlive)
            {
                thread.Abort();
                thread.Join();
            }
        }

        private void Boss_HP_textbox_ValueChanged(object sender, EventArgs e)
        {
            ChangeEnemyBossHP();
        }

        private void listView1_MouseClick(object sender, MouseEventArgs e)
        {
            if (listView1.SelectedItems.Count == 1)
            {
                ListView.SelectedListViewItemCollection items = listView1.SelectedItems;
                ListViewItem subitem = items[0];
                enemy_team_id_combobox.Text = subitem.SubItems[0].Text;

            }
        }

        private void numericUpDown1_ValueChanged_1(object sender, EventArgs e)
        {

        }

        private void saveFileDialog1_FileOk(object sender, CancelEventArgs e)
        {
            JObject savefile = new JObject();
            for (int i = 1; i <= 5; i++)
            {
                JObject o = new JObject();
                for (int k = 0; k < gun_eh_array.Count(); k++)
                {
                    if (gun_eh_array[k]["id"].ToString() == gun_user_id[echelon_select.SelectedIndex, i - 1].ToString())
                    {
                        o = new JObject(gun_eh_array[k]);
                        o["equip1"] = "0";
                        o["equip2"] = "0";
                        o["equip3"] = "0";
                    }
                }
                savefile.Add("gun" + i.ToString(), o);
            }
            //savefile.Add("enemyGroupID", enemy_team_id_combobox.Text);
            // savefile.Add("BossHP", Boss_HP_textbox.Value.ToString());
            File.WriteAllText(saveFileDialog1.FileName, savefile.ToString());
        }

        private void button2_Click(object sender, EventArgs e)
        {
            openFileDialog1.ShowDialog();
        }

        private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
        {
            Load_Gun_Info_fromfilepath(openFileDialog1.FileName);
        }
        public void Load_Gun_Info_fromfilepath(string path)
        {
            try
            {
                JObject file = JObject.Parse(File.ReadAllText(path));
                loadinfofromfile(file, echelon_select.SelectedIndex);
                label115.Text = Path.GetFileName(path);
                SetGunInfo(false, echelon_select.SelectedIndex);
            }
            catch (Exception ex)
            {
                MessageBox.Show(lang_data["file_load_error_msg"].ToString() + Environment.NewLine + ex.ToString(), lang_data["load_failed"].ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        void loadinfofromfile(JObject json, int ech)
        {
            int[] gunid = gun_id.ToArray();
            #region LoadJsonData
            for (int i = 1; i <= 5; i++)
            {
                for (int k = 0; k < gun_eh_array.Count(); k++)
                {
                    if (gun_eh_array[k]["id"].ToString() == gun_user_id[ech, i - 1].ToString())
                    {
                        if (json.SelectToken("gun" + i.ToString()) != null)
                        {
                            gun_eh_array[k].Replace(json["gun" + i.ToString()]);
                            gun_eh_array[k]["id"] = gun_user_id[ech, i - 1].ToString();
                        }
                        else if (json.SelectToken("gun" + i.ToString()) == null && i != 1)
                        {
                            CheckBox e = (CheckBox)Controls.Find("enable_" + i.ToString(), true)[0];
                            gun_eh_array[k]["team_id"] = "0";
                            e.Checked = false;
                        }
                    }
                }
            }
            /*
            if(json.SelectToken("gun1") != null)
            {
                gun_info_json_1["id"] = gun_user_id[ech, 0].ToString(); //json["gun1"]["id"].ToString();
                gun_info_json_1["gun_id"] =   json["gun1"]["gun_id"].ToString();
                gun_info_json_1["gun_exp"] = json["gun1"]["gun_exp"].ToString();
                gun_info_json_1["gun_level"] = json["gun1"]["gun_level"].ToString();
                gun_info_json_1["team_id"] = json["gun1"]["team_id"].ToString();
                gun_info_json_1["if_modification"] = json["gun1"]["if_modification"].ToString();
                gun_info_json_1["location"] = json["gun1"]["location"].ToString();
                gun_info_json_1["position"] = json["gun1"]["position"].ToString();
                gun_info_json_1["life"] = json["gun1"]["life"].ToString();
                gun_info_json_1["pow"] = json["gun1"]["pow"].ToString();
                gun_info_json_1["hit"] = json["gun1"]["hit"].ToString();
                gun_info_json_1["dodge"] = json["gun1"]["dodge"].ToString();
                gun_info_json_1["rate"] = json["gun1"]["rate"].ToString();
                gun_info_json_1["skill1"] = json["gun1"]["skill1"].ToString();
                gun_info_json_1["skill2"] = json["gun1"]["skill2"].ToString();
                gun_info_json_1["number"] = json["gun1"]["number"].ToString();
                //gun_info_json_1["equip1"] = json["gun1"]["equip1"].ToString();
                //gun_info_json_1["equip2"] = json["gun1"]["equip2"].ToString();
                //gun_info_json_1["equip3"] = json["gun1"]["equip3"].ToString();
                gun_info_json_1["favor"] = json["gun1"]["favor"].ToString();
                gun_info_json_1["max_favor"] = json["gun1"]["max_favor"].ToString();
                gun_info_json_1["favor_toplimit"] = json["gun1"]["favor_toplimit"].ToString();
                gun_info_json_1["soul_bond"] = json["gun1"]["soul_bond"].ToString();
                gun_info_json_1["skin"] = json["gun1"]["skin"].ToString();
            }


            if (json.SelectToken("gun2") != null)
            {
                gun_info_json_2["id"] = gun_user_id[ech, 1].ToString();//json["gun2"]["id"].ToString();
                gun_info_json_2["gun_id"] = json["gun2"]["gun_id"].ToString();
                gun_info_json_2["gun_exp"] = json["gun2"]["gun_exp"].ToString();
                gun_info_json_2["gun_level"] = json["gun2"]["gun_level"].ToString();
                gun_info_json_2["team_id"] = json["gun2"]["team_id"].ToString();
                gun_info_json_2["if_modification"] = json["gun2"]["if_modification"].ToString();
                gun_info_json_2["location"] = json["gun2"]["location"].ToString();
                gun_info_json_2["position"] = json["gun2"]["position"].ToString();
                gun_info_json_2["life"] = json["gun2"]["life"].ToString();
                gun_info_json_2["pow"] = json["gun2"]["pow"].ToString();
                gun_info_json_2["hit"] = json["gun2"]["hit"].ToString();
                gun_info_json_2["dodge"] = json["gun2"]["dodge"].ToString();
                gun_info_json_2["rate"] = json["gun2"]["rate"].ToString();
                gun_info_json_2["skill1"] = json["gun2"]["skill1"].ToString();
                gun_info_json_2["skill2"] = json["gun2"]["skill2"].ToString();
                gun_info_json_2["number"] = json["gun2"]["number"].ToString();
                //gun_info_json_2["equip1"] = json["gun2"]["equip1"].ToString();
                //gun_info_json_2["equip2"] = json["gun2"]["equip2"].ToString();
                //gun_info_json_2["equip3"] = json["gun2"]["equip3"].ToString();
                gun_info_json_2["favor"] = json["gun2"]["favor"].ToString();
                gun_info_json_2["max_favor"] = json["gun2"]["max_favor"].ToString();
                gun_info_json_2["favor_toplimit"] = json["gun2"]["favor_toplimit"].ToString();
                gun_info_json_2["soul_bond"] = json["gun2"]["soul_bond"].ToString();
                gun_info_json_2["skin"] = json["gun2"]["skin"].ToString();
            }
            else
            {
                enable_2.Checked = false;
                gun_info_json_2["team_id"] = "0";
            }
                

            if(json.SelectToken("gun3")!= null)
            {
                gun_info_json_3["id"] = gun_user_id[ech, 2].ToString(); //json["gun3"]["id"].ToString();
                gun_info_json_3["gun_id"] = json["gun3"]["gun_id"].ToString();
                gun_info_json_3["gun_exp"] = json["gun3"]["gun_exp"].ToString();
                gun_info_json_3["gun_level"] = json["gun3"]["gun_level"].ToString();
                gun_info_json_3["team_id"] = json["gun3"]["team_id"].ToString();
                gun_info_json_3["if_modification"] = json["gun3"]["if_modification"].ToString();
                gun_info_json_3["location"] = json["gun3"]["location"].ToString();
                gun_info_json_3["position"] = json["gun3"]["position"].ToString();
                gun_info_json_3["life"] = json["gun3"]["life"].ToString();
                gun_info_json_3["pow"] = json["gun3"]["pow"].ToString();
                gun_info_json_3["hit"] = json["gun3"]["hit"].ToString();
                gun_info_json_3["dodge"] = json["gun3"]["dodge"].ToString();
                gun_info_json_3["rate"] = json["gun3"]["rate"].ToString();
                gun_info_json_3["skill1"] = json["gun3"]["skill1"].ToString();
                gun_info_json_3["skill2"] = json["gun3"]["skill2"].ToString();
                gun_info_json_3["number"] = json["gun3"]["number"].ToString();
                //gun_info_json_3["equip1"] = json["gun3"]["equip1"].ToString();
                //gun_info_json_3["equip2"] = json["gun3"]["equip2"].ToString();
                //gun_info_json_3["equip3"] = json["gun3"]["equip3"].ToString();
                gun_info_json_3["favor"] = json["gun3"]["favor"].ToString();
                gun_info_json_3["max_favor"] = json["gun3"]["max_favor"].ToString();
                gun_info_json_3["favor_toplimit"] = json["gun3"]["favor_toplimit"].ToString();
                gun_info_json_3["soul_bond"] = json["gun3"]["soul_bond"].ToString();
                gun_info_json_3["skin"] = json["gun3"]["skin"].ToString();
            }
            else
            {
                enable_3.Checked = false;
                gun_info_json_3["team_id"] = "0";
            }
                

            if (json.SelectToken("gun4") != null)
            {
                gun_info_json_4["id"] = gun_user_id[ech, 3].ToString();//json["gun4"]["id"].ToString();
                gun_info_json_4["gun_id"] = json["gun4"]["gun_id"].ToString();
                gun_info_json_4["gun_exp"] = json["gun4"]["gun_exp"].ToString();
                gun_info_json_4["gun_level"] = json["gun4"]["gun_level"].ToString();
                gun_info_json_4["team_id"] = json["gun4"]["team_id"].ToString();
                gun_info_json_4["if_modification"] = json["gun4"]["if_modification"].ToString();
                gun_info_json_4["location"] = json["gun4"]["location"].ToString();
                gun_info_json_4["position"] = json["gun4"]["position"].ToString();
                gun_info_json_4["life"] = json["gun4"]["life"].ToString();
                gun_info_json_4["pow"] = json["gun4"]["pow"].ToString();
                gun_info_json_4["hit"] = json["gun4"]["hit"].ToString();
                gun_info_json_4["dodge"] = json["gun4"]["dodge"].ToString();
                gun_info_json_4["rate"] = json["gun4"]["rate"].ToString();
                gun_info_json_4["skill1"] = json["gun4"]["skill1"].ToString();
                gun_info_json_4["skill2"] = json["gun4"]["skill2"].ToString();
                gun_info_json_4["number"] = json["gun4"]["number"].ToString();
                //gun_info_json_4["equip1"] = json["gun4"]["equip1"].ToString();
                //gun_info_json_4["equip2"] = json["gun4"]["equip2"].ToString();
                //gun_info_json_4["equip3"] = json["gun4"]["equip3"].ToString();
                gun_info_json_4["favor"] = json["gun4"]["favor"].ToString();
                gun_info_json_4["max_favor"] = json["gun4"]["max_favor"].ToString();
                gun_info_json_4["favor_toplimit"] = json["gun4"]["favor_toplimit"].ToString();
                gun_info_json_4["soul_bond"] = json["gun4"]["soul_bond"].ToString();
                gun_info_json_4["skin"] = json["gun4"]["skin"].ToString();
            }
            else
            {
                enable_4.Checked = false;
                gun_info_json_4["team_id"] = "0";
            }

            if (json.SelectToken("gun5") != null)
            {
                gun_info_json_5["id"] = gun_user_id[ech, 4].ToString();//json["gun5"]["id"].ToString();
                gun_info_json_5["gun_id"] = json["gun5"]["gun_id"].ToString();
                gun_info_json_5["gun_exp"] = json["gun5"]["gun_exp"].ToString();
                gun_info_json_5["gun_level"] = json["gun5"]["gun_level"].ToString();
                gun_info_json_5["team_id"] = json["gun5"]["team_id"].ToString();
                gun_info_json_5["if_modification"] = json["gun5"]["if_modification"].ToString();
                gun_info_json_5["location"] = json["gun5"]["location"].ToString();
                gun_info_json_5["position"] = json["gun5"]["position"].ToString();
                gun_info_json_5["life"] = json["gun5"]["life"].ToString();
                gun_info_json_5["pow"] = json["gun5"]["pow"].ToString();
                gun_info_json_5["hit"] = json["gun5"]["hit"].ToString();
                gun_info_json_5["dodge"] = json["gun5"]["dodge"].ToString();
                gun_info_json_5["rate"] = json["gun5"]["rate"].ToString();
                gun_info_json_5["skill1"] = json["gun5"]["skill1"].ToString();
                gun_info_json_5["skill2"] = json["gun5"]["skill2"].ToString();
                gun_info_json_5["number"] = json["gun5"]["number"].ToString();
                //gun_info_json_5["equip1"] = json["gun5"]["equip1"].ToString();
                //gun_info_json_5["equip2"] = json["gun5"]["equip2"].ToString();
                //gun_info_json_5["equip3"] = json["gun5"]["equip3"].ToString();
                gun_info_json_5["favor"] = json["gun5"]["favor"].ToString();
                gun_info_json_5["max_favor"] = json["gun5"]["max_favor"].ToString();
                gun_info_json_5["favor_toplimit"] = json["gun5"]["favor_toplimit"].ToString();
                gun_info_json_5["soul_bond"] = json["gun5"]["soul_bond"].ToString();
                gun_info_json_5["skin"] = json["gun5"]["skin"].ToString();
            }
            else
            {
                enable_5.Checked = false;
                gun_info_json_5["team_id"] = "0";
            }
           */
            #endregion

            #region SetJsonData
            //gun_info_json_1["id"]
            //gun_info_json_1["equip1"] = "0";
            //gun_info_json_1["equip2"] = "0";
            //gun_info_json_1["equip3"] = "0";
            //gun_info_json_1["skin"] = "0";
            //gun_info_json_1["if_modification"] = "0";
            //gun_info_json_1["location"] = num.ToString();

            for (int i = 1; i <= 5; i++)
            {
                for (int k = 0; k < gun_eh_array.Count(); k++)
                {
                    if (gun_eh_array[k]["id"].ToString() == gun_user_id[ech, i - 1].ToString())
                    {
                        JObject defStat = new JObject(getGunDefaultStat(1, int.Parse(gun_eh_array[k]["gun_id"].ToString()), 1));
                        ((ComboBox)Controls.Find("gunid_" + i.ToString() + "_combobox", true)[0]).SelectedIndex = Array.IndexOf(gunid, int.Parse(gun_eh_array[k]["gun_id"].ToString()));
                        ((NumericUpDown)Controls.Find("gunlv_" + i.ToString() + "_number", true)[0]).Value = !check_ifmod(i) && int.Parse(gun_eh_array[k]["gun_level"].ToString()) > 100 ? 100 : int.Parse(gun_eh_array[k]["gun_level"].ToString());
                        ((NumericUpDown)Controls.Find("gunpos_" + i.ToString() + "_number", true)[0]).Value = Array.IndexOf(gun_position, int.Parse(gun_eh_array[k]["position"].ToString()));
                        ((NumericUpDown)Controls.Find("gunhp_" + i.ToString() + "_number", true)[0]).Value = int.Parse(gun_eh_array[k]["life"].ToString());
                        ((NumericUpDown)Controls.Find("gunpow_" + i.ToString() + "_number", true)[0]).Value = int.Parse(gun_eh_array[k]["pow"].ToString()) + int.Parse(defStat["pow"].ToString());
                        ((NumericUpDown)Controls.Find("gunhit_" + i.ToString() + "_number", true)[0]).Value = int.Parse(gun_eh_array[k]["hit"].ToString()) + int.Parse(defStat["hit"].ToString());
                        ((NumericUpDown)Controls.Find("gundodge_" + i.ToString() + "_number", true)[0]).Value = int.Parse(gun_eh_array[k]["dodge"].ToString()) + int.Parse(defStat["dodge"].ToString());
                        ((NumericUpDown)Controls.Find("gunrate_" + i.ToString() + "_number", true)[0]).Value = int.Parse(gun_eh_array[k]["rate"].ToString()) + int.Parse(defStat["rate"].ToString());
                        ((NumericUpDown)Controls.Find("gunskill1_" + i.ToString() + "_number", true)[0]).Value = int.Parse(gun_eh_array[k]["skill1"].ToString());
                        ((NumericUpDown)Controls.Find("gunskill2_" + i.ToString() + "_number", true)[0]).Value = check_ifmod(i) ? int.Parse(gun_eh_array[k]["skill2"].ToString()) : 0; //?? ???? 0
                        ((NumericUpDown)Controls.Find("gundummy_" + i.ToString() + "_number", true)[0]).Value = int.Parse(gun_eh_array[k]["number"].ToString());
                        ((NumericUpDown)Controls.Find("gunfavor_" + i.ToString() + "_number", true)[0]).Value = int.Parse(gun_eh_array[k]["favor"].ToString()) / 10000;
                        ((CheckBox)Controls.Find("gunoath_" + i.ToString() + "_checkbox", true)[0]).Checked = Convert.ToBoolean(int.Parse(gun_eh_array[k]["soul_bond"].ToString()));
                        if (i != 1)
                        {
                            ((CheckBox)Controls.Find("enable_" + i.ToString(), true)[0]).Checked = Convert.ToBoolean(int.Parse(gun_eh_array[k]["team_id"].ToString()));
                        }
                    }
                }
            }
            /*
             JObject defStat_1 = new JObject(getGunDefaultStat(1, int.Parse(gun_info_json_1["gun_id"].ToString()), 1));
             JObject defStat_2 = new JObject(getGunDefaultStat(1, int.Parse(gun_info_json_2["gun_id"].ToString()), 1));
             JObject defStat_3 = new JObject(getGunDefaultStat(1, int.Parse(gun_info_json_3["gun_id"].ToString()), 1));
             JObject defStat_4 = new JObject(getGunDefaultStat(1, int.Parse(gun_info_json_4["gun_id"].ToString()), 1));
             JObject defStat_5 = new JObject(getGunDefaultStat(1, int.Parse(gun_info_json_5["gun_id"].ToString()), 1));

             gunid_1_combobox.SelectedIndex = Array.IndexOf(gunid, int.Parse(gun_info_json_1["gun_id"].ToString()));
             gunlv_1_number.Value = !check_ifmod(1) && int.Parse(gun_info_json_1["gun_level"].ToString()) > 100 ? 100 : int.Parse(gun_info_json_1["gun_level"].ToString());
             gunpos_1_number.Value = Array.IndexOf(gun_position, int.Parse(gun_info_json_1["position"].ToString()));
             gunhp_1_number.Value = int.Parse(gun_info_json_1["life"].ToString());
             gunpow_1_number.Value = int.Parse(gun_info_json_1["pow"].ToString()) + int.Parse(defStat_1["pow"].ToString());
             gunhit_1_number.Value = int.Parse(gun_info_json_1["hit"].ToString()) + int.Parse(defStat_1["hit"].ToString());
             gundodge_1_number.Value = int.Parse(gun_info_json_1["dodge"].ToString()) + int.Parse(defStat_1["dodge"].ToString());
             gunrate_1_number.Value = int.Parse(gun_info_json_1["rate"].ToString()) + int.Parse(defStat_1["rate"].ToString());
             gunskill1_1_number.Value = int.Parse(gun_info_json_1["skill1"].ToString());
             gunskill2_1_number.Value = check_ifmod(1) ? int.Parse(gun_info_json_1["skill2"].ToString()) : 0; //?? ???? 0
             gundummy_1_number.Value = int.Parse(gun_info_json_1["number"].ToString());
             gunfavor_1_number.Value = int.Parse(gun_info_json_1["favor"].ToString()) / 10000;
             gunoath_1_checkbox.Checked = Convert.ToBoolean(int.Parse(gun_info_json_1["soul_bond"].ToString()));


             gunid_2_combobox.SelectedIndex = Array.IndexOf(gunid, int.Parse(gun_info_json_2["gun_id"].ToString()));
             gunlv_2_number.Value = !check_ifmod(2) && int.Parse(gun_info_json_2["gun_level"].ToString()) > 100 ? 100 : int.Parse(gun_info_json_2["gun_level"].ToString());
             gunpos_2_number.Value = Array.IndexOf(gun_position, int.Parse(gun_info_json_2["position"].ToString()));
             gunhp_2_number.Value = int.Parse(gun_info_json_2["life"].ToString());
             gunpow_2_number.Value = int.Parse(gun_info_json_2["pow"].ToString()) + int.Parse(defStat_2["pow"].ToString());
             gunhit_2_number.Value = int.Parse(gun_info_json_2["hit"].ToString()) + int.Parse(defStat_2["hit"].ToString());
             gundodge_2_number.Value = int.Parse(gun_info_json_2["dodge"].ToString()) + int.Parse(defStat_2["dodge"].ToString());
             gunrate_2_number.Value = int.Parse(gun_info_json_2["rate"].ToString()) + int.Parse(defStat_2["rate"].ToString());
             gunskill1_2_number.Value = int.Parse(gun_info_json_2["skill1"].ToString());
             gunskill2_2_number.Value = check_ifmod(2) ? int.Parse(gun_info_json_2["skill2"].ToString()) : 0;
             gundummy_2_number.Value = int.Parse(gun_info_json_2["number"].ToString());
             gunfavor_2_number.Value = int.Parse(gun_info_json_2["favor"].ToString()) / 10000;
             gunoath_2_checkbox.Checked = Convert.ToBoolean(int.Parse(gun_info_json_2["soul_bond"].ToString()));
             enable_2.Checked = Convert.ToBoolean(int.Parse(gun_info_json_2["team_id"].ToString()));

             gunid_3_combobox.SelectedIndex = Array.IndexOf(gunid, int.Parse(gun_info_json_3["gun_id"].ToString()));
             gunlv_3_number.Value = !check_ifmod(3) && int.Parse(gun_info_json_3["gun_level"].ToString()) > 100 ? 100 : int.Parse(gun_info_json_3["gun_level"].ToString());
             gunpos_3_number.Value = Array.IndexOf(gun_position, int.Parse(gun_info_json_3["position"].ToString()));
             gunhp_3_number.Value = int.Parse(gun_info_json_3["life"].ToString());
             gunpow_3_number.Value = int.Parse(gun_info_json_3["pow"].ToString()) + int.Parse(defStat_3["pow"].ToString());
             gunhit_3_number.Value = int.Parse(gun_info_json_3["hit"].ToString()) + int.Parse(defStat_3["hit"].ToString());
             gundodge_3_number.Value = int.Parse(gun_info_json_3["dodge"].ToString()) + int.Parse(defStat_3["dodge"].ToString());
             gunrate_3_number.Value = int.Parse(gun_info_json_3["rate"].ToString()) + int.Parse(defStat_3["rate"].ToString());
             gunskill1_3_number.Value = int.Parse(gun_info_json_3["skill1"].ToString());
             gunskill2_3_number.Value = check_ifmod(3) ? int.Parse(gun_info_json_3["skill2"].ToString()) : 0;
             gundummy_3_number.Value = int.Parse(gun_info_json_3["number"].ToString());
             gunfavor_3_number.Value = int.Parse(gun_info_json_3["favor"].ToString()) / 10000;
             gunoath_3_checkbox.Checked = Convert.ToBoolean(int.Parse(gun_info_json_3["soul_bond"].ToString()));
             enable_3.Checked = Convert.ToBoolean(int.Parse(gun_info_json_3["team_id"].ToString()));

             gunid_4_combobox.SelectedIndex = Array.IndexOf(gunid, int.Parse(gun_info_json_4["gun_id"].ToString()));
             gunlv_4_number.Value = !check_ifmod(4) && int.Parse(gun_info_json_4["gun_level"].ToString()) > 100 ? 100 : int.Parse(gun_info_json_4["gun_level"].ToString());
             gunpos_4_number.Value = Array.IndexOf(gun_position, int.Parse(gun_info_json_4["position"].ToString()));
             gunhp_4_number.Value = int.Parse(gun_info_json_4["life"].ToString());
             gunpow_4_number.Value = int.Parse(gun_info_json_4["pow"].ToString()) + int.Parse(defStat_4["pow"].ToString());
             gunhit_4_number.Value = int.Parse(gun_info_json_4["hit"].ToString()) + int.Parse(defStat_4["hit"].ToString());
             gundodge_4_number.Value = int.Parse(gun_info_json_4["dodge"].ToString()) + int.Parse(defStat_4["dodge"].ToString());
             gunrate_4_number.Value = int.Parse(gun_info_json_4["rate"].ToString()) + int.Parse(defStat_4["rate"].ToString());
             gunskill1_4_number.Value = int.Parse(gun_info_json_4["skill1"].ToString());
             gunskill2_4_number.Value = check_ifmod(4) ? int.Parse(gun_info_json_4["skill2"].ToString()) : 0;
             gundummy_4_number.Value = int.Parse(gun_info_json_4["number"].ToString());
             gunfavor_4_number.Value = int.Parse(gun_info_json_4["favor"].ToString()) / 10000;
             gunoath_4_checkbox.Checked = Convert.ToBoolean(int.Parse(gun_info_json_4["soul_bond"].ToString()));
             enable_4.Checked = Convert.ToBoolean(int.Parse(gun_info_json_4["team_id"].ToString()));

             gunid_5_combobox.SelectedIndex = Array.IndexOf(gunid, int.Parse(gun_info_json_5["gun_id"].ToString()));
             gunlv_5_number.Value = !check_ifmod(5) && int.Parse(gun_info_json_5["gun_level"].ToString()) > 100 ? 100 : int.Parse(gun_info_json_5["gun_level"].ToString());
             gunpos_5_number.Value = Array.IndexOf(gun_position, int.Parse(gun_info_json_5["position"].ToString()));
             gunhp_5_number.Value = int.Parse(gun_info_json_5["life"].ToString());
             gunpow_5_number.Value = int.Parse(gun_info_json_5["pow"].ToString()) + int.Parse(defStat_5["pow"].ToString());
             gunhit_5_number.Value = int.Parse(gun_info_json_5["hit"].ToString()) + int.Parse(defStat_5["hit"].ToString());
             gundodge_5_number.Value = int.Parse(gun_info_json_5["dodge"].ToString()) + int.Parse(defStat_5["dodge"].ToString());
             gunrate_5_number.Value = int.Parse(gun_info_json_5["rate"].ToString()) + int.Parse(defStat_5["rate"].ToString());
             gunskill1_5_number.Value = int.Parse(gun_info_json_5["skill1"].ToString());
             gunskill2_5_number.Value = check_ifmod(5) ? int.Parse(gun_info_json_5["skill2"].ToString()) : 0;
             gundummy_5_number.Value = int.Parse(gun_info_json_5["number"].ToString());
             gunfavor_5_number.Value = int.Parse(gun_info_json_5["favor"].ToString()) / 10000;
             gunoath_5_checkbox.Checked = Convert.ToBoolean(int.Parse(gun_info_json_5["soul_bond"].ToString()));
             enable_5.Checked = Convert.ToBoolean(int.Parse(gun_info_json_5["team_id"].ToString()));

             */
            #endregion

            //enemy_team_id_combobox.Text = json["enemyGroupID"].ToString();

        }
        int getTeamIDfromID(int id)
        {
            if (id == 1 || id == 2 || id == 3 || id == 4 || id == 5)
            {
                return 1;
            }
            else if (id == 6 || id == 7 || id == 8 || id == 9 || id == 10)
            {
                return 2;
            }
            else if (id == 11 || id == 12 || id == 13 || id == 14 || id == 15)
            {
                return 3;
            }
            else if (id == 16 || id == 17 || id == 18 || id == 19 || id == 20)
            {
                return 4;
            }
            else if (id == 21 || id == 22 || id == 23 || id == 24 || id == 25)
            {
                return 5;
            }
            else if (id == 26 || id == 27 || id == 28 || id == 29 || id == 30)
            {
                return 6;
            }
            else
                return 0;
        }
        int getTeamIDfromID(string id)
        {
            return getTeamIDfromID(int.Parse(id));
        }
        private void button8_Click(object sender, EventArgs e)
        {

            setSquadInfo(true);
            //MessageBox.Show(userinfo["mission_act_info"]["squad_info"].ToString());
        }
        void setSquadInfo(bool showConfirmMessage)
        {

            JArray sqd = new JArray();
            JObject temp = new JObject();
            JObject sqdSwitch = JObject.Parse(missionactinfo["squad_info"].ToString());

            #region SetData
            sqdSwitch["1"]["battleskill_switch"] = Convert.ToInt16(sqdswitch_1.Checked);
            sqdSwitch["2"]["battleskill_switch"] = Convert.ToInt16(sqdswitch_2.Checked);
            sqdSwitch["3"]["battleskill_switch"] = Convert.ToInt16(sqdswitch_3.Checked);
            sqdSwitch["4"]["battleskill_switch"] = Convert.ToInt16(sqdswitch_4.Checked);
            sqdSwitch["5"]["battleskill_switch"] = Convert.ToInt16(sqdswitch_5.Checked);
            sqdSwitch["6"]["battleskill_switch"] = Convert.ToInt16(sqdswitch_6.Checked);

            missionactinfo["squad_info"].Replace(JsonConvert.SerializeObject(sqdSwitch, Formatting.None));
            for (int i = 0; i < 6; i++)
            {
                sqd.Add(userinfo["squad_with_user_info"][sqdID[i].ToString()]);
                switch (i)
                {
                    case 0:
                        sqd[i]["squad_id"] = (i + 1).ToString();
                        sqd[i]["squad_exp"] = sqd_exp_table[Convert.ToInt16(sqd_1_lv.Value) - 1].ToString();
                        sqd[i]["squad_level"] = sqd_1_lv.Value.ToString();
                        sqd[i]["assist_damage"] = (sqd_1_damage.Value - squad_BGM71_defaultstat[???]).ToString();
                        sqd[i]["assist_reload"] = (sqd_1_reload.Value - squad_BGM71_defaultstat[??]).ToString();
                        sqd[i]["assist_hit"] = (sqd_1_hit.Value - squad_BGM71_defaultstat[???]).ToString();
                        sqd[i]["assist_def_break"] = (sqd_1_break.Value - squad_BGM71_defaultstat[???]).ToString();
                        sqd[i]["skill1"] = sqd_1_skill1.Value.ToString();
                        sqd[i]["skill2"] = sqd_1_skill2.Value.ToString();
                        sqd[i]["skill3"] = sqd_1_skill3.Value.ToString();
                        break;
                    case 1:
                        sqd[i]["squad_id"] = (i + 1).ToString();
                        sqd[i]["squad_exp"] = sqd_exp_table[Convert.ToInt16(sqd_2_lv.Value) - 1].ToString();
                        sqd[i]["squad_level"] = sqd_2_lv.Value.ToString();
                        sqd[i]["assist_damage"] = (sqd_2_damage.Value - squad_AGS30_defaultstat[???]).ToString();
                        sqd[i]["assist_reload"] = (sqd_2_reload.Value - squad_AGS30_defaultstat[??]).ToString();
                        sqd[i]["assist_hit"] = (sqd_2_hit.Value - squad_AGS30_defaultstat[???]).ToString();
                        sqd[i]["assist_def_break"] = (sqd_2_break.Value - squad_AGS30_defaultstat[???]).ToString();
                        sqd[i]["skill1"] = sqd_2_skill1.Value.ToString();
                        sqd[i]["skill2"] = sqd_2_skill2.Value.ToString();
                        sqd[i]["skill3"] = sqd_2_skill3.Value.ToString();
                        break;
                    case 2:
                        sqd[i]["squad_id"] = (i + 1).ToString();
                        sqd[i]["squad_exp"] = sqd_exp_table[Convert.ToInt16(sqd_3_lv.Value) - 1].ToString();
                        sqd[i]["squad_level"] = sqd_3_lv.Value.ToString();
                        sqd[i]["assist_damage"] = (sqd_3_damage.Value - squad_2B14_defaultstat[???]).ToString();
                        sqd[i]["assist_reload"] = (sqd_3_reload.Value - squad_2B14_defaultstat[??]).ToString();
                        sqd[i]["assist_hit"] = (sqd_3_hit.Value - squad_2B14_defaultstat[???]).ToString();
                        sqd[i]["assist_def_break"] = (sqd_3_break.Value - squad_2B14_defaultstat[???]).ToString();
                        sqd[i]["skill1"] = sqd_3_skill1.Value.ToString();
                        sqd[i]["skill2"] = sqd_3_skill2.Value.ToString();
                        sqd[i]["skill3"] = sqd_3_skill3.Value.ToString();
                        break;
                    case 3:
                        sqd[i]["squad_id"] = (i + 1).ToString();
                        sqd[i]["squad_exp"] = sqd_exp_table[Convert.ToInt16(sqd_4_lv.Value) - 1].ToString();
                        sqd[i]["squad_level"] = sqd_4_lv.Value.ToString();
                        sqd[i]["assist_damage"] = (sqd_4_damage.Value - squad_M2_defaultstat[???]).ToString();
                        sqd[i]["assist_reload"] = (sqd_4_reload.Value - squad_M2_defaultstat[??]).ToString();
                        sqd[i]["assist_hit"] = (sqd_4_hit.Value - squad_M2_defaultstat[???]).ToString();
                        sqd[i]["assist_def_break"] = (sqd_4_break.Value - squad_M2_defaultstat[???]).ToString();
                        sqd[i]["skill1"] = sqd_4_skill1.Value.ToString();
                        sqd[i]["skill2"] = sqd_4_skill2.Value.ToString();
                        sqd[i]["skill3"] = sqd_4_skill3.Value.ToString();
                        break;
                    case 4:
                        sqd[i]["squad_id"] = (i + 1).ToString();
                        sqd[i]["squad_exp"] = sqd_exp_table[Convert.ToInt16(sqd_5_lv.Value) - 1].ToString();
                        sqd[i]["squad_level"] = sqd_5_lv.Value.ToString();
                        sqd[i]["assist_damage"] = (sqd_5_damage.Value - squad_AT4_defaultstat[???]).ToString();
                        sqd[i]["assist_reload"] = (sqd_5_reload.Value - squad_AT4_defaultstat[??]).ToString();
                        sqd[i]["assist_hit"] = (sqd_5_hit.Value - squad_AT4_defaultstat[???]).ToString();
                        sqd[i]["assist_def_break"] = (sqd_5_break.Value - squad_AT4_defaultstat[???]).ToString();
                        sqd[i]["skill1"] = sqd_5_skill1.Value.ToString();
                        sqd[i]["skill2"] = sqd_5_skill2.Value.ToString();
                        sqd[i]["skill3"] = sqd_5_skill3.Value.ToString();
                        break;
                    case 5:
                        sqd[i]["squad_id"] = (i + 1).ToString();
                        sqd[i]["squad_exp"] = sqd_exp_table[Convert.ToInt16(sqd_6_lv.Value) - 1].ToString();
                        sqd[i]["squad_level"] = sqd_6_lv.Value.ToString();
                        sqd[i]["assist_damage"] = (sqd_6_damage.Value - squad_QLZ04_defaultstat[???]).ToString();
                        sqd[i]["assist_reload"] = (sqd_6_reload.Value - squad_QLZ04_defaultstat[??]).ToString();
                        sqd[i]["assist_hit"] = (sqd_6_hit.Value - squad_QLZ04_defaultstat[???]).ToString();
                        sqd[i]["assist_def_break"] = (sqd_6_break.Value - squad_QLZ04_defaultstat[???]).ToString();
                        sqd[i]["skill1"] = sqd_6_skill1.Value.ToString();
                        sqd[i]["skill2"] = sqd_6_skill2.Value.ToString();
                        sqd[i]["skill3"] = sqd_6_skill3.Value.ToString();
                        break;
                }
                temp.Add(sqdID[i].ToString(), sqd[i]);
            }
            userinfo["squad_with_user_info"].Replace(temp);
            //Clipboard.SetText(userinfo["squad_with_user_info"].ToString());
            if (showConfirmMessage)
                MessageBox.Show(lang_data["saved_msg"].ToString(), lang_data["alert"].ToString(), MessageBoxButtons.OK, MessageBoxIcon.Information);
            #endregion

        }
        void SaveSquadInfo(string path)
        {
            JObject savefile = new JObject();
            try
            {
                for (int i = 0; i < 6; i++)
                {
                    savefile.Add("Squad" + (i + 1).ToString(), userinfo["squad_with_user_info"][sqdID[i].ToString()]);
                }
                savefile.Add("switch1", Convert.ToInt16(sqdswitch_1.Checked));
                savefile.Add("switch2", Convert.ToInt16(sqdswitch_2.Checked));
                savefile.Add("switch3", Convert.ToInt16(sqdswitch_3.Checked));
                savefile.Add("switch4", Convert.ToInt16(sqdswitch_4.Checked));
                savefile.Add("switch5", Convert.ToInt16(sqdswitch_5.Checked));
                savefile.Add("switch6", Convert.ToInt16(sqdswitch_6.Checked));
                File.WriteAllText(path, savefile.ToString());
            }
            catch (Exception ex)
            {
                MessageBox.Show(lang_data["file_save_error_msg"].ToString() + Environment.NewLine + ex.ToString(), lang_data["error"].ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        void loadSquadInfoFromFile(JObject json)
        {
            try
            {
                #region LoadSqdDataFromJson
                if (json["Squad1"] != null)
                {
                    json["Squad1"]["id"] = sqdID[0].ToString();
                    sqd_1_lv.Value = int.Parse(json["Squad1"]["squad_level"].ToString());
                    sqd_1_damage.Value = int.Parse(json["Squad1"]["assist_damage"].ToString()) + squad_BGM71_defaultstat[???];
                    sqd_1_reload.Value = int.Parse(json["Squad1"]["assist_reload"].ToString()) + squad_BGM71_defaultstat[??];
                    sqd_1_hit.Value = int.Parse(json["Squad1"]["assist_hit"].ToString()) + squad_BGM71_defaultstat[???];
                    sqd_1_break.Value = int.Parse(json["Squad1"]["assist_def_break"].ToString()) + squad_BGM71_defaultstat[???];
                    sqd_1_skill1.Value = int.Parse(json["Squad1"]["skill1"].ToString());
                    sqd_1_skill2.Value = int.Parse(json["Squad1"]["skill2"].ToString());
                    sqd_1_skill3.Value = int.Parse(json["Squad1"]["skill3"].ToString());
                }
                /*else
                {
                    sqd_1_lv.Value = 1;
                    sqd_1_damage.Value = sqd_1_damage.Minimum;
                    sqd_1_reload.Value = sqd_1_reload.Minimum;
                    sqd_1_hit.Value = sqd_1_reload.Minimum;
                    sqd_1_break.Value = sqd_1_break.Minimum;
                    sqd_1_skill1.Value = 1;
                    sqd_1_skill2.Value = 1;
                    sqd_1_skill3.Value = 1;
                }*/

                if (json["Squad2"] != null)
                {
                    json["Squad2"]["id"] = sqdID[1].ToString();
                    sqd_2_lv.Value = int.Parse(json["Squad2"]["squad_level"].ToString());
                    sqd_2_damage.Value = int.Parse(json["Squad2"]["assist_damage"].ToString()) + squad_AGS30_defaultstat[???];
                    sqd_2_reload.Value = int.Parse(json["Squad2"]["assist_reload"].ToString()) + squad_AGS30_defaultstat[??];
                    sqd_2_hit.Value = int.Parse(json["Squad2"]["assist_hit"].ToString()) + squad_AGS30_defaultstat[???];
                    sqd_2_break.Value = int.Parse(json["Squad2"]["assist_def_break"].ToString()) + squad_AGS30_defaultstat[???];
                    sqd_2_skill1.Value = int.Parse(json["Squad2"]["skill1"].ToString());
                    sqd_2_skill2.Value = int.Parse(json["Squad2"]["skill2"].ToString());
                    sqd_2_skill3.Value = int.Parse(json["Squad2"]["skill3"].ToString());
                }
                /*else
                {
                    sqd_2_lv.Value = 1;
                    sqd_2_damage.Value = sqd_2_damage.Minimum;
                    sqd_2_reload.Value = sqd_2_reload.Minimum;
                    sqd_2_hit.Value = sqd_2_reload.Minimum;
                    sqd_2_break.Value = sqd_2_break.Minimum;
                    sqd_2_skill1.Value = 1;
                    sqd_2_skill2.Value = 1;
                    sqd_2_skill3.Value = 1;
                }*/

                if (json["Squad3"] != null)
                {
                    json["Squad3"]["id"] = sqdID[2].ToString();
                    sqd_3_lv.Value = int.Parse(json["Squad3"]["squad_level"].ToString());
                    sqd_3_damage.Value = int.Parse(json["Squad3"]["assist_damage"].ToString()) + squad_2B14_defaultstat[???];
                    sqd_3_reload.Value = int.Parse(json["Squad3"]["assist_reload"].ToString()) + squad_2B14_defaultstat[??];
                    sqd_3_hit.Value = int.Parse(json["Squad3"]["assist_hit"].ToString()) + squad_2B14_defaultstat[???];
                    sqd_3_break.Value = int.Parse(json["Squad3"]["assist_def_break"].ToString()) + squad_2B14_defaultstat[???];
                    sqd_3_skill1.Value = int.Parse(json["Squad3"]["skill1"].ToString());
                    sqd_3_skill2.Value = int.Parse(json["Squad3"]["skill2"].ToString());
                    sqd_3_skill3.Value = int.Parse(json["Squad3"]["skill3"].ToString());
                }
                /* else
                 {
                     sqd_3_lv.Value = 1;
                     sqd_3_damage.Value = sqd_3_damage.Minimum;
                     sqd_3_reload.Value = sqd_3_reload.Minimum;
                     sqd_3_hit.Value = sqd_3_reload.Minimum;
                     sqd_3_break.Value = sqd_3_break.Minimum;
                     sqd_3_skill1.Value = 1;
                     sqd_3_skill2.Value = 1;
                     sqd_3_skill3.Value = 1;
                 }*/

                if (json["Squad4"] != null)
                {
                    json["Squad4"]["id"] = sqdID[3].ToString();
                    sqd_4_lv.Value = int.Parse(json["Squad4"]["squad_level"].ToString());
                    sqd_4_damage.Value = int.Parse(json["Squad4"]["assist_damage"].ToString()) + squad_M2_defaultstat[???];
                    sqd_4_reload.Value = int.Parse(json["Squad4"]["assist_reload"].ToString()) + squad_M2_defaultstat[??];
                    sqd_4_hit.Value = int.Parse(json["Squad4"]["assist_hit"].ToString()) + squad_M2_defaultstat[???];
                    sqd_4_break.Value = int.Parse(json["Squad4"]["assist_def_break"].ToString()) + squad_M2_defaultstat[???];
                    sqd_4_skill1.Value = int.Parse(json["Squad4"]["skill1"].ToString());
                    sqd_4_skill2.Value = int.Parse(json["Squad4"]["skill2"].ToString());
                    sqd_4_skill3.Value = int.Parse(json["Squad4"]["skill3"].ToString());
                }
                /*else
                {
                    sqd_4_lv.Value = 1;
                    sqd_4_damage.Value = sqd_4_damage.Minimum;
                    sqd_4_reload.Value = sqd_4_reload.Minimum;
                    sqd_4_hit.Value = sqd_4_reload.Minimum;
                    sqd_4_break.Value = sqd_4_break.Minimum;
                    sqd_4_skill1.Value = 1;
                    sqd_4_skill2.Value = 1;
                    sqd_4_skill3.Value = 1;
                }*/

                if (json["Squad5"] != null)
                {
                    json["Squad5"]["id"] = sqdID[4].ToString();
                    sqd_5_lv.Value = int.Parse(json["Squad5"]["squad_level"].ToString());
                    sqd_5_damage.Value = int.Parse(json["Squad5"]["assist_damage"].ToString()) + squad_AT4_defaultstat[???];
                    sqd_5_reload.Value = int.Parse(json["Squad5"]["assist_reload"].ToString()) + squad_AT4_defaultstat[??];
                    sqd_5_hit.Value = int.Parse(json["Squad5"]["assist_hit"].ToString()) + squad_AT4_defaultstat[???];
                    sqd_5_break.Value = int.Parse(json["Squad5"]["assist_def_break"].ToString()) + squad_AT4_defaultstat[???];
                    sqd_5_skill1.Value = int.Parse(json["Squad5"]["skill1"].ToString());
                    sqd_5_skill2.Value = int.Parse(json["Squad5"]["skill2"].ToString());
                    sqd_5_skill3.Value = int.Parse(json["Squad5"]["skill3"].ToString());
                }
                /*else
                {
                    sqd_5_lv.Value = 1;
                    sqd_5_damage.Value = sqd_5_damage.Minimum;
                    sqd_5_reload.Value = sqd_5_reload.Minimum;
                    sqd_5_hit.Value = sqd_5_reload.Minimum;
                    sqd_5_break.Value = sqd_5_break.Minimum;
                    sqd_5_skill1.Value = 1;
                    sqd_5_skill2.Value = 1;
                    sqd_5_skill3.Value = 1;
                }*/

                if (json["Squad6"] != null)
                {
                    json["Squad6"]["id"] = sqdID[5].ToString();
                    sqd_6_lv.Value = int.Parse(json["Squad6"]["squad_level"].ToString());
                    sqd_6_damage.Value = int.Parse(json["Squad6"]["assist_damage"].ToString()) + squad_QLZ04_defaultstat[???];
                    sqd_6_reload.Value = int.Parse(json["Squad6"]["assist_reload"].ToString()) + squad_QLZ04_defaultstat[??];
                    sqd_6_hit.Value = int.Parse(json["Squad6"]["assist_hit"].ToString()) + squad_QLZ04_defaultstat[???];
                    sqd_6_break.Value = int.Parse(json["Squad6"]["assist_def_break"].ToString()) + squad_QLZ04_defaultstat[???];
                    sqd_6_skill1.Value = int.Parse(json["Squad6"]["skill1"].ToString());
                    sqd_6_skill2.Value = int.Parse(json["Squad6"]["skill2"].ToString());
                    sqd_6_skill3.Value = int.Parse(json["Squad6"]["skill3"].ToString());
                }
                /*else
                {
                    sqd_6_lv.Value = 1;
                    sqd_6_damage.Value = sqd_6_damage.Minimum;
                    sqd_6_reload.Value = sqd_6_reload.Minimum;
                    sqd_6_hit.Value = sqd_6_reload.Minimum;
                    sqd_6_break.Value = sqd_6_break.Minimum;
                    sqd_6_skill1.Value = 1;
                    sqd_6_skill2.Value = 1;
                    sqd_6_skill3.Value = 1;
                }*/


                for (int i = 0; i < 6; i++)
                {
                    if (json["switch" + (i + 1).ToString()] != null)
                    {
                        CheckBox sw = (CheckBox)Controls.Find("sqdswitch_" + (i + 1).ToString(), true)[0];
                        sw.Checked = Convert.ToBoolean(int.Parse(json["switch" + (i + 1)].ToString()));
                    }


                }
                /*
                sqdswitch_1.Checked = Convert.ToBoolean(int.Parse(json["switch1"].ToString()));
                sqdswitch_2.Checked = Convert.ToBoolean(int.Parse(json["switch2"].ToString()));
                sqdswitch_3.Checked = Convert.ToBoolean(int.Parse(json["switch3"].ToString()));
                sqdswitch_4.Checked = Convert.ToBoolean(int.Parse(json["switch4"].ToString()));
                sqdswitch_5.Checked = Convert.ToBoolean(int.Parse(json["switch5"].ToString()));
                sqdswitch_6.Checked = Convert.ToBoolean(int.Parse(json["switch6"].ToString()));*/

                for (int i = 0; i < 6; i++)
                {
                    if (json["Squad" + (i + 1).ToString()] != null)
                    {

                        userinfo["squad_with_user_info"][sqdID[i].ToString()].Replace(json["Squad" + (i + 1).ToString()]);

                    }
                    else
                    {
                        JObject t = JObject.Parse("{'id': '" + sqdID[i].ToString() + "','squad_id': '" + (i + 1).ToString() + "','squad_exp': '0','squad_level': '1','rank': '1','advanced_level': '0','life': '100','cur_def': '0','ammo': '1000','mre': '1000','assist_damage': '0','assist_reload': '0','assist_hit': '0','assist_def_break': '0','damage': '0','atk_speed': '0','hit': '0','def': '0','skill1': '1','skill2': '1','skill3': '1'}");
                        userinfo["squad_with_user_info"][sqdID[i].ToString()] = t;
                    }
                }
                setSquadInfo(false);
                #endregion
                label116.Text = Path.GetFileName(openFileDialog2.FileName);
            }
            catch (Exception ex)
            {
                MessageBox.Show(lang_data["file_load_error_msg"].ToString() + Environment.NewLine + ex.ToString(), lang_data["error"].ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void button6_Click_1(object sender, EventArgs e)
        {
            saveFileDialog2.ShowDialog();
        }

        private void saveFileDialog2_FileOk(object sender, CancelEventArgs e)
        {
            SaveSquadInfo(saveFileDialog2.FileName);
        }

        private void button7_Click(object sender, EventArgs e)
        {
            openFileDialog2.ShowDialog();
        }

        private void openFileDialog2_FileOk(object sender, CancelEventArgs e)
        {
            loadSquadInfoFromFile(JObject.Parse(File.ReadAllText(openFileDialog2.FileName)));

        }

        private void sqdswitch_1_CheckedChanged(object sender, EventArgs e)
        {
            enable_set_sqd();
        }

        private void sqdswitch_2_CheckedChanged(object sender, EventArgs e)
        {
            enable_set_sqd();
        }

        private void sqdswitch_3_CheckedChanged(object sender, EventArgs e)
        {
            enable_set_sqd();
        }

        private void button1_MouseHover(object sender, EventArgs e)
        {
            showTooltip((Control)sender, lang_data["gun_save_tooltip"].ToString());
        }

        private void sqdswitch_4_CheckedChanged(object sender, EventArgs e)
        {
            enable_set_sqd();
        }

        private void sqdswitch_5_CheckedChanged(object sender, EventArgs e)
        {
            enable_set_sqd();
        }

        private void button2_MouseHover(object sender, EventArgs e)
        {
            showTooltip((Control)sender, lang_data["gun_load_tooltip"].ToString());
        }

        private void button4_MouseHover(object sender, EventArgs e)
        {
            showTooltip((Control)sender, lang_data["gun_apply_tooltip"].ToString());
        }

        private void button9_Click(object sender, EventArgs e)
        {
            setgundefaultstatbyinput(1);
        }
        void setgundefaultstatbyinput(int num)
        {
            JObject s = new JObject();

            try
            {
                if (num == 1)
                {
                    int id = 0;
                    if (check_ifmod(1))
                        id = int.Parse(gun_info[gunid_1_combobox.SelectedIndex].Split(',')[0]) - 20000;
                    else
                        id = int.Parse(gun_info[gunid_1_combobox.SelectedIndex].Split(',')[0]);
                    s = new JObject(getGunDefaultStat((int)gundummy_1_number.Value, id, (int)gunlv_1_number.Value));
                    gunhp_1_number.Value = Convert.ToInt16(s["hp"]);
                    gunpow_1_number.Value = Convert.ToInt16(s["pow"]);
                    gunhit_1_number.Value = Convert.ToInt16(s["hit"]);
                    gundodge_1_number.Value = Convert.ToInt16(s["dodge"]);
                    gunrate_1_number.Value = Convert.ToInt16(s["rate"]);
                }
                else if (num == 2)
                {
                    int id = 0;
                    if (check_ifmod(2))
                        id = int.Parse(gun_info[gunid_2_combobox.SelectedIndex].Split(',')[0]) - 20000;
                    else
                        id = int.Parse(gun_info[gunid_2_combobox.SelectedIndex].Split(',')[0]);
                    s = new JObject(getGunDefaultStat((int)gundummy_2_number.Value, id, (int)gunlv_2_number.Value));
                    gunhp_2_number.Value = Convert.ToInt16(s["hp"]);
                    gunpow_2_number.Value = Convert.ToInt16(s["pow"]);
                    gunhit_2_number.Value = Convert.ToInt16(s["hit"]);
                    gundodge_2_number.Value = Convert.ToInt16(s["dodge"]);
                    gunrate_2_number.Value = Convert.ToInt16(s["rate"]);
                }
                else if (num == 3)
                {
                    int id = 0;
                    if (check_ifmod(3))
                        id = int.Parse(gun_info[gunid_3_combobox.SelectedIndex].Split(',')[0]) - 20000;
                    else
                        id = int.Parse(gun_info[gunid_3_combobox.SelectedIndex].Split(',')[0]);
                    s = new JObject(getGunDefaultStat((int)gundummy_3_number.Value, id, (int)gunlv_3_number.Value));
                    gunhp_3_number.Value = Convert.ToInt16(s["hp"]);
                    gunpow_3_number.Value = Convert.ToInt16(s["pow"]);
                    gunhit_3_number.Value = Convert.ToInt16(s["hit"]);
                    gundodge_3_number.Value = Convert.ToInt16(s["dodge"]);
                    gunrate_3_number.Value = Convert.ToInt16(s["rate"]);
                }
                else if (num == 4)
                {
                    int id = 0;
                    if (check_ifmod(4))
                        id = int.Parse(gun_info[gunid_4_combobox.SelectedIndex].Split(',')[0]) - 20000;
                    else
                        id = int.Parse(gun_info[gunid_4_combobox.SelectedIndex].Split(',')[0]);
                    s = new JObject(getGunDefaultStat((int)gundummy_4_number.Value, id, (int)gunlv_4_number.Value));
                    gunhp_4_number.Value = Convert.ToInt16(s["hp"]);
                    gunpow_4_number.Value = Convert.ToInt16(s["pow"]);
                    gunhit_4_number.Value = Convert.ToInt16(s["hit"]);
                    gundodge_4_number.Value = Convert.ToInt16(s["dodge"]);
                    gunrate_4_number.Value = Convert.ToInt16(s["rate"]);
                }
                else if (num == 5)
                {
                    int id = 0;
                    if (check_ifmod(5))
                        id = int.Parse(gun_info[gunid_5_combobox.SelectedIndex].Split(',')[0]) - 20000;
                    else
                        id = int.Parse(gun_info[gunid_5_combobox.SelectedIndex].Split(',')[0]);
                    s = new JObject(getGunDefaultStat((int)gundummy_5_number.Value, id, (int)gunlv_5_number.Value));
                    gunhp_5_number.Value = Convert.ToInt16(s["hp"]);
                    gunpow_5_number.Value = Convert.ToInt16(s["pow"]);
                    gunhit_5_number.Value = Convert.ToInt16(s["hit"]);
                    gundodge_5_number.Value = Convert.ToInt16(s["dodge"]);
                    gunrate_5_number.Value = Convert.ToInt16(s["rate"]);
                }
                else
                    throw new Exception("Error: Invaild control number: " + num.ToString());
            }
            catch (ArgumentOutOfRangeException)
            {
                MessageBox.Show(lang_data["stat_calc_error_msg"].ToString(), lang_data["stat_calc_error"].ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }
        private void button9_MouseHover(object sender, EventArgs e)
        {
            showTooltip((Control)sender, lang_data["stat_calc_tooltip"].ToString() + Environment.NewLine + lang_data["stat_calc_tooltip_line2"].ToString());
        }

        private void button10_Click(object sender, EventArgs e)
        {
            setgundefaultstatbyinput(2);
        }

        private void button11_Click(object sender, EventArgs e)
        {
            setgundefaultstatbyinput(3);
        }

        private void button12_Click(object sender, EventArgs e)
        {
            setgundefaultstatbyinput(4);
        }

        private void button13_Click(object sender, EventArgs e)
        {
            setgundefaultstatbyinput(5);
        }

        private void label117_Click(object sender, EventArgs e)
        {

        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (server_hideip.Checked)
                server_ip.Text = lang_data["server_ip"].ToString() + "******";
            else
            {
                if (listener.IsListening)
                    server_ip.Text = lang_data["server_ip"].ToString() + GetLocalIP();
                else
                    server_ip.Text = lang_data["server_ip"].ToString();
            }

        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            LogTextBox.Clear();
        }

        private void button14_Click(object sender, EventArgs e)
        {
            ChangeEnemyGroupID();
            ChangeEnemyBossHP();
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


        void OpenEquipSelector(string value)
        {
            EquipSelecter equipselector = new EquipSelecter();
            equipselector.FormSendEvent += new EquipSelecter.FormSendDataHandler(getEquipInfoFromForm2);
            equipselector.Passvalue = value;
            equipselector.Show();
        }
        void getEquipInfoFromForm2(JObject json, string name, bool clear)
        {
            string pos = name.Split(';')[1];
            string ename = name.Split(';')[0];
            if (pos == "11")
            {
                if (clear)
                {
                    gun_info_json_1["equip1"] = "0";
                    setEquip_11.Text = lang_data["none_equip"].ToString();
                    userinfo["equip_with_user_info"]["11"] = new JObject(json);
                }
                else
                {
                    json["gun_with_user_id"] = "1";
                    userinfo["equip_with_user_info"]["11"] = new JObject(json);
                    gun_info_json_1["equip1"] = "11";
                    setEquip_11.Text = ename;
                }

            }
            else if (pos == "12")
            {
                if (clear)
                {

                    gun_info_json_1["equip2"] = "0";
                    setEquip_12.Text = lang_data["none_equip"].ToString();
                    userinfo["equip_with_user_info"]["12"] = new JObject(json);
                }
                else
                {
                    json["gun_with_user_id"] = "1";
                    userinfo["equip_with_user_info"]["12"] = new JObject(json);
                    gun_info_json_1["equip2"] = "12";
                    setEquip_12.Text = ename;
                }

            }
            else if (pos == "13")
            {
                if (clear)
                {
                    gun_info_json_1["equip3"] = "0";
                    setEquip_13.Text = lang_data["none_equip"].ToString();
                    userinfo["equip_with_user_info"]["13"] = new JObject(json);
                }
                else
                {
                    json["gun_with_user_id"] = "1";
                    userinfo["equip_with_user_info"]["13"] = new JObject(json);
                    gun_info_json_1["equip3"] = "13";
                    setEquip_13.Text = ename;
                }

            }
            else if (pos == "21")
            {
                if (clear)
                {
                    gun_info_json_2["equip1"] = "0";
                    setEquip_21.Text = lang_data["none_equip"].ToString();
                    userinfo["equip_with_user_info"]["21"] = new JObject(json);
                }
                else
                {
                    json["gun_with_user_id"] = "2";
                    userinfo["equip_with_user_info"]["21"] = new JObject(json);
                    gun_info_json_2["equip1"] = "21";
                    setEquip_21.Text = ename;
                }

            }
            else if (pos == "22")
            {
                if (clear)
                {
                    gun_info_json_2["equip2"] = "0";
                    setEquip_22.Text = lang_data["none_equip"].ToString();
                    userinfo["equip_with_user_info"]["22"] = new JObject(json);
                }
                else
                {
                    json["gun_with_user_id"] = "2";
                    userinfo["equip_with_user_info"]["22"] = new JObject(json);
                    gun_info_json_2["equip2"] = "22";
                    setEquip_22.Text = ename;
                }

            }
            else if (pos == "23")
            {
                if (clear)
                {
                    gun_info_json_2["equip3"] = "0";
                    setEquip_23.Text = lang_data["none_equip"].ToString();
                    userinfo["equip_with_user_info"]["23"] = new JObject(json);
                }
                else
                {
                    json["gun_with_user_id"] = "2";
                    userinfo["equip_with_user_info"]["23"] = new JObject(json);
                    gun_info_json_2["equip3"] = "23";
                    setEquip_23.Text = ename;
                }

            }
            else if (pos == "31")
            {
                if (clear)
                {
                    gun_info_json_3["equip1"] = "0";
                    setEquip_31.Text = lang_data["none_equip"].ToString();
                    userinfo["equip_with_user_info"]["31"] = new JObject(json);
                }
                else
                {
                    json["gun_with_user_id"] = "3";
                    userinfo["equip_with_user_info"]["31"] = new JObject(json);
                    gun_info_json_3["equip1"] = "31";
                    setEquip_31.Text = ename;
                }

            }
            else if (pos == "32")
            {
                if (clear)
                {
                    gun_info_json_3["equip2"] = "0";
                    setEquip_32.Text = lang_data["none_equip"].ToString();
                    userinfo["equip_with_user_info"]["32"] = new JObject(json);
                }
                else
                {
                    json["gun_with_user_id"] = "3";
                    userinfo["equip_with_user_info"]["32"] = new JObject(json);
                    gun_info_json_3["equip2"] = "32";
                    setEquip_32.Text = ename;
                }

            }
            else if (pos == "33")
            {
                if (clear)
                {
                    gun_info_json_3["equip3"] = "0";
                    setEquip_33.Text = lang_data["none_equip"].ToString();
                    userinfo["equip_with_user_info"]["33"] = new JObject(json);
                }
                else
                {
                    json["gun_with_user_id"] = "3";
                    userinfo["equip_with_user_info"]["33"] = new JObject(json);
                    gun_info_json_3["equip3"] = "33";
                    setEquip_33.Text = ename;
                }

            }
            else if (pos == "41")
            {
                if (clear)
                {
                    gun_info_json_4["equip1"] = "0";
                    setEquip_41.Text = lang_data["none_equip"].ToString();
                    userinfo["equip_with_user_info"]["41"] = new JObject(json);
                }
                else
                {
                    json["gun_with_user_id"] = "4";
                    userinfo["equip_with_user_info"]["41"] = new JObject(json);
                    gun_info_json_4["equip1"] = "41";
                    setEquip_41.Text = ename;
                }

            }
            else if (pos == "42")
            {
                if (clear)
                {
                    gun_info_json_4["equip2"] = "0";
                    setEquip_42.Text = lang_data["none_equip"].ToString();
                    userinfo["equip_with_user_info"]["42"] = new JObject(json);
                }
                else
                {
                    json["gun_with_user_id"] = "4";
                    userinfo["equip_with_user_info"]["42"] = new JObject(json);
                    gun_info_json_4["equip2"] = "42";
                    setEquip_42.Text = ename;
                }

            }
            else if (pos == "43")
            {
                if (clear)
                {
                    gun_info_json_4["equip3"] = "0";
                    setEquip_43.Text = lang_data["none_equip"].ToString();
                    userinfo["equip_with_user_info"]["43"] = new JObject(json);
                }
                else
                {
                    json["gun_with_user_id"] = "4";
                    userinfo["equip_with_user_info"]["43"] = new JObject(json);
                    gun_info_json_4["equip3"] = "43";
                    setEquip_43.Text = ename;
                }

            }
            else if (pos == "51")
            {
                if (clear)
                {
                    gun_info_json_5["equip1"] = "0";
                    setEquip_51.Text = lang_data["none_equip"].ToString();
                    userinfo["equip_with_user_info"]["51"] = new JObject(json);
                }
                else
                {
                    json["gun_with_user_id"] = "5";
                    userinfo["equip_with_user_info"]["51"] = new JObject(json);
                    gun_info_json_5["equip1"] = "51";
                    setEquip_51.Text = ename;
                }

            }
            else if (pos == "52")
            {
                if (clear)
                {
                    gun_info_json_5["equip2"] = "0";
                    setEquip_52.Text = lang_data["none_equip"].ToString();
                    userinfo["equip_with_user_info"]["52"] = new JObject(json);
                }
                else
                {
                    json["gun_with_user_id"] = "5";
                    userinfo["equip_with_user_info"]["52"] = new JObject(json);
                    gun_info_json_5["equip2"] = "52";
                    setEquip_52.Text = ename;
                }

            }
            else if (pos == "53")
            {
                if (clear)
                {
                    gun_info_json_5["equip3"] = "0";
                    setEquip_53.Text = lang_data["none_equip"].ToString();
                    userinfo["equip_with_user_info"]["53"] = new JObject(json);
                }
                else
                {
                    json["gun_with_user_id"] = "5";
                    userinfo["equip_with_user_info"]["53"] = new JObject(json);
                    gun_info_json_5["equip3"] = "53";
                    setEquip_53.Text = ename;
                }

            }
            SetGunInfo(false, echelon_select.SelectedIndex);
        }
        #region showEquipselect
        private void button15_Click(object sender, EventArgs e)
        {
            OpenEquipSelector("11;" + setEquip_11.Text);
        }
        private void setEquip_12_Click(object sender, EventArgs e)
        {
            OpenEquipSelector("12;" + setEquip_12.Text);
        }
        private void setEquip_13_Click(object sender, EventArgs e)
        {
            OpenEquipSelector("13;" + setEquip_13.Text);
        }
        private void setEquip_21_Click(object sender, EventArgs e)
        {
            OpenEquipSelector("21;" + setEquip_21.Text);
        }

        private void setEquip_22_Click(object sender, EventArgs e)
        {
            OpenEquipSelector("22;" + setEquip_22.Text);
        }

        private void setEquip_23_Click(object sender, EventArgs e)
        {
            OpenEquipSelector("23;" + setEquip_23.Text);
        }
        private void setEquip_31_Click(object sender, EventArgs e)
        {
            OpenEquipSelector("31;" + setEquip_31.Text);
        }

        private void setEquip_32_Click(object sender, EventArgs e)
        {
            OpenEquipSelector("32;" + setEquip_32.Text);
        }

        private void setEquip_33_Click(object sender, EventArgs e)
        {
            OpenEquipSelector("33;" + setEquip_33.Text);
        }
        private void setEquip_41_Click(object sender, EventArgs e)
        {
            OpenEquipSelector("41;" + setEquip_41.Text);
        }

        private void setEquip_42_Click(object sender, EventArgs e)
        {
            OpenEquipSelector("42;" + setEquip_42.Text);
        }

        private void setEquip_43_Click(object sender, EventArgs e)
        {
            OpenEquipSelector("43;" + setEquip_43.Text);
        }
        private void setEquip_51_Click(object sender, EventArgs e)
        {
            OpenEquipSelector("51;" + setEquip_51.Text);
        }

        private void setEquip_52_Click(object sender, EventArgs e)
        {
            OpenEquipSelector("52;" + setEquip_52.Text);
        }

        private void setEquip_53_Click(object sender, EventArgs e)
        {
            OpenEquipSelector("53;" + setEquip_53.Text);
        }
        #endregion

        private void button15_Click_1(object sender, EventArgs e)
        {
            saveFileDialog3.ShowDialog();
        }

        private void saveFileDialog3_FileOk(object sender, CancelEventArgs e)
        {

            JObject sav = new JObject((JObject)userinfo["equip_with_user_info"]);
            //JObject sw = new JObject();
            File.WriteAllText(saveFileDialog3.FileName, sav.ToString());

        }

        private void button16_Click(object sender, EventArgs e)
        {
            openFileDialog3.ShowDialog();

        }

        private void openFileDialog3_FileOk(object sender, CancelEventArgs e)
        {
            Load_Equip_Info_From_File(openFileDialog3.FileName, echelon_select.SelectedIndex);
        }
        public void Load_Equip_Info_From_File(string path, int echelon)
        {
            string eh = (echelon + 1).ToString();
            JObject eq = JObject.Parse(File.ReadAllText(path));
            foreach (string a in Equippos)
            {

                if (eq[a] != null)
                {
                    this.Controls.Find("setEquip_" + a, true)[0].Text = equipName[equipID.IndexOf(eq[a]["equip_id"].ToString())];

                    /*
                    for(int i=1; i <= 5; i++)
                    {
                        for(int k=1; k<=3; k++)
                        {
                            int cn = int.Parse(i.ToString() + k.ToString());
                            
                        }
                    }*/



                    if (a == "11")
                    {
                        for(int i=0;i< gun_eh_array.Count(); i++)
                        {
                            if(gun_eh_array[i]["id"].ToString() == gun_user_id[echelon, 0].ToString())
                            {
                                 gun_eh_array[i]["equip1"] = a;
                            }
                        }
                       
                    }
                    else if (a == "12")
                    {
                        gun_eh_array[i]["equip2"] = a;
                    }
                    else if (a == "13")
                    {
                        gun_eh_array[i]["equip3"] = a;
                    }

                    if (a == "21")
                    {
                        gun_eh_array[i]["equip1"] = a;
                    }
                    else if (a == "22")
                    {
                        gun_eh_array[i]["equip2"] = a;
                    }
                    else if (a == "23")
                    {
                        gun_eh_array[i]["equip3"] = a;
                    }



                    if (a == "31")
                    {
                        gun_eh_array[i]["equip1"] = a;
                    }
                    else if (a == "32")
                    {
                        gun_eh_array[i]["equip2"] = a;
                    }
                    else if (a == "33")
                    {
                        gun_eh_array[i]["equip3"] = a;
                    }



                    if (a == "41")
                    {
                        gun_eh_array[i]["equip1"] = a;
                    }
                    else if (a == "42")
                    {
                        gun_eh_array[i]["equip2"] = a;
                    }
                    else if (a == "43")
                    {
                        gun_eh_array[i]["equip3"] = a;
                    }


                    if (a == "51")
                    {
                        gun_eh_array[i]["equip1"] = a;
                    }
                    else if (a == "52")
                    {
                        gun_eh_array[i]["equip2"] = a;
                    }
                    else if (a == "53")
                    {
                        gun_eh_array[i]["equip3"] = a;
                    }




                }
            }
            userinfo["equip_with_user_info"].Replace(eq);
            label121.Text = Path.GetFileName(path);
            SetGunInfo(false, echelon);
        }
        private void button17_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show(lang_data["removeall_equip_confirm_msg"].ToString(), lang_data["alert"].ToString(), MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation) == DialogResult.OK)
            {
                for (int i = 1; i <= 5; i++)
                {
                    for (int j = 1; j <= 3; j++)
                    {
                        foreach (string s in Equippos)
                        {
                            Controls.Find("setEquip_" + s, true)[0].Text = lang_data["none_equip"].ToString();
                        }
                        if (i == 1)
                        {
                            gun_info_json_1["equip" + j.ToString()] = "0";
                        }
                        else if (i == 2)
                        {
                            gun_info_json_2["equip" + j.ToString()] = "0";
                        }
                        else if (i == 3)
                        {
                            gun_info_json_3["equip" + j.ToString()] = "0";
                        }
                        else if (i == 4)
                        {
                            gun_info_json_4["equip" + j.ToString()] = "0";
                        }
                        else if (i == 5)
                        {
                            gun_info_json_5["equip" + j.ToString()] = "0";
                        }
                    }
                }
                SetGunInfo(false, echelon_select.SelectedIndex);
                JObject def = JObject.Parse(File.ReadAllText(@"data/json/userinfo.json"));
                userinfo["equip_with_user_info"].Replace(def["equip_with_user_info"]);
            }
        }


        void applyEquip(JObject a)
        {

        }

        private void button18_Click(object sender, EventArgs e)
        {

        }



        private void linkLabel3_LinkClicked_1(object sender, LinkLabelLinkClickedEventArgs e)
        {
            //Clipboard.SetText(userinfo.ToString());
        }

        private void Button18_Click_1(object sender, EventArgs e)
        {
            battleResultViewer BRV = new battleResultViewer();
            BRV.Show();
        }
        void showBRV(JObject o)
        {
            battleResultViewer BRV = new battleResultViewer();
            BRV.Passvalue = o;
            BRV.Show();
        }

        private void Gunpow_1_number_ValueChanged(object sender, EventArgs e)
        {

        }

        private void Button6_MouseHover(object sender, EventArgs e)
        {
            showTooltip((Control)sender, lang_data["sqd_save_tooltip"].ToString());
        }

        private void Button7_MouseHover(object sender, EventArgs e)
        {
            showTooltip((Control)sender, lang_data["sqd_load_tooltip"].ToString());
        }

        private void Button8_MouseHover(object sender, EventArgs e)
        {
            showTooltip((Control)sender, lang_data["sqd_apply_tooltip"].ToString());
        }

        private void Button5_MouseHover(object sender, EventArgs e)
        {
            showTooltip((Control)sender, lang_data["server_start_tooltip"].ToString());
        }

        private void CheckBox1_MouseHover(object sender, EventArgs e)
        {
            showTooltip((Control)sender, lang_data["server_hideip_tooltip"].ToString());
        }

        private void Button3_Click_1(object sender, EventArgs e)
        {
            ShowposSelector(1, (int)gunpos_1_number.Value, (Control)sender);
        }

        private void Button19_Click(object sender, EventArgs e)
        {
            ShowposSelector(2, (int)gunpos_2_number.Value, (Control)sender);
        }

        private void Button20_Click(object sender, EventArgs e)
        {
            ShowposSelector(3, (int)gunpos_3_number.Value, (Control)sender);
        }

        private void Button21_Click(object sender, EventArgs e)
        {
            ShowposSelector(4, (int)gunpos_4_number.Value, (Control)sender);
        }

        private void Button22_Click(object sender, EventArgs e)
        {
            ShowposSelector(5, (int)gunpos_5_number.Value, (Control)sender);
        }
        void ShowposSelector(int n, int selpos, Control c)
        {
            foreach (Form f in Application.OpenForms) //???? ??
            {
                if (f.Name == "posSelector")
                {
                    f.Activate();
                    System.Media.SystemSounds.Asterisk.Play();
                    return;
                }
            }
            GroupBox gbox = (GroupBox)Controls.Find("groupBox" + n.ToString(), true)[0];
            posSelector posselector = new posSelector(c, selpos, n);
            posselector.StartPosition = FormStartPosition.Manual;
            posselector.Location = new Point(c.Location.X + gbox.Location.X + 100, c.Location.Y + gbox.Location.Y + 100);
            //posselector.FormSendEvent += new EquipSelecter.FormSendDataHandler(getEquipInfoFromForm2);
            //posselector.Passvalue = value;
            posselector.Show();
        }
        JObject getGunDefaultStat(int gunNum, int gunID, int lev)
        {
            int level = lev;
            int growratio = 0;
            JObject stats = new JObject();
            JObject basicstats = new JObject();
            JObject growStats = new JObject();
            JObject newStats = new JObject();
            string type = string.Empty;
            string name = string.Empty;
            double favorRatio = 0.0;
            /*
            if (favor < 10)
                favorRatio = -0.05;
            else if (favor < 90)
                favorRatio = 0;
            else if (favor < 140)
                favorRatio = 0.05;
            else if (favor < 190)
                favorRatio = 0.1;
            else
                favorRatio = 0.15;*/

            try
            {
                for (int i = 0; i < gunstatdata.Count; i++)
                {
                    string t = gunstatdata[i]["id"].ToString();
                    if (t == gunID.ToString())
                    {
                        type = gunstatdata[i]["type"].ToString();
                        name = gunstatdata[i]["name"].ToString();
                        growratio = int.Parse(gunstatdata[i]["grow"].ToString());
                        stats = (JObject)gunstatdata[i]["stats"];
                        if (level <= 100)
                        {
                            basicstats = (JObject)grow["normal"]["basic"];
                            growStats = (JObject)grow["normal"]["grow"];
                        }
                        else
                        {
                            basicstats = (JObject)grow["after100"]["basic"];
                            growStats = (JObject)grow["after100"]["grow"];
                        }
                        break;
                    }
                }
                newStats = new JObject(stats);
                foreach (var keys in basicstats.Properties())
                {

                    newStats[keys.Name] = basicstats[keys.Name].Count() > 1 ?
                        Math.Ceiling((Convert.ToDecimal(basicstats[keys.Name][0]) + (Convert.ToDecimal(level - 1) * Convert.ToDecimal(basicstats[keys.Name][1]))) * Convert.ToDecimal(attribute[type][keys.Name]) * Convert.ToDecimal(stats[keys.Name]) / 100) :
                        Math.Ceiling(Convert.ToDecimal(basicstats[keys.Name][0]) * Convert.ToDecimal(attribute[type][keys.Name]) * Convert.ToDecimal(stats[keys.Name]) / 100);

                    //newStat[key] = math.ceil((basicStats[key][0] + ((self.level - 1) * basicStats[key][1])) * attribute[self.type][key] * stats[key] / 100) if len(basicStats[key]) > 1 
                    //else math.ceil(basicStats[key][0] * attribute[self.type][key] * stats[key] / 100)
                }

                foreach (var keys in growStats.Properties())
                {
                    newStats[keys.Name] = Math.Ceiling(Convert.ToDecimal(newStats[keys.Name]) + (Convert.ToDecimal(growStats[keys.Name][1]) + (Convert.ToDecimal(level - 1) * Convert.ToDecimal(growStats[keys.Name][0])) * Convert.ToDecimal(attribute[type][keys.Name]) * Convert.ToDecimal(stats[keys.Name]) * Convert.ToDecimal(growratio) / 100 / 100));
                    //newStat[key] += math.ceil(growStats[key][1] + ((self.level - 1) * growStats[key][0]) * attribute[self.type][key] * stats[key] * growRatio / 100 / 100)

                }

                newStats["hp"] = int.Parse(newStats["hp"].ToString()) * gunNum;
                return newStats;
            }
            catch
            {
                return JObject.Parse("{'hp':0,'pow':0,'hit':0,'dodge':0,'speed':0,'rate':0,'armorPiercing':0,'crit':0}");
            }

        }
        public void showTooltip(Control sender, string caption)
        {
            toolTip1.ToolTipTitle = "";
            toolTip1.SetToolTip(sender, caption);
        }

        private void Button23_Click(object sender, EventArgs e)
        {
            serv = new serveraccess(this);
            if (MessageBox.Show(lang_data["get_userinfo_confrim_msg"].ToString(), lang_data["get_userinfo_mode"].ToString(), MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
            {
                if (!listener.IsListening)
                {
                    if (MessageBox.Show(lang_data["server_not_started_msg"].ToString(), lang_data["server_not_started"].ToString(), MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
                    {
                        if (run_server())
                        {
                            MessageBox.Show(lang_data["server_started_msg"].ToString(), lang_data["server_started"].ToString(), MessageBoxButtons.OK, MessageBoxIcon.Information);
                            serv.Show();
                            frm.getUserinfoFromServer = true;
                            gun_loadfromserver.Enabled = false;
                        }
                        else
                        {
                            MessageBox.Show(lang_data["server_start_failed_msg"].ToString(), lang_data["server_start_failed"].ToString(), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            MessageBox.Show(lang_data["get_userinfo_failed_cause_serv_run_failed_msg"].ToString(), lang_data["server_start_failed"].ToString(), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }

                    }
                    else
                    {
                        MessageBox.Show(lang_data["get_userinfo_failed_cause_serv_run_failed_msg"].ToString(), lang_data["server_start_failed"].ToString(), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
                else
                {
                    serv.Show();

                    frm.getUserinfoFromServer = true;
                    gun_loadfromserver.Enabled = false;
                }
            }

        }
        public bool CheckIfFormIsOpen(string formname)
        {
            bool formOpen = Application.OpenForms.Cast<Form>().Any(form => form.Name == formname);
            return formOpen;
        }
        void showTooltip(Control sender, string caption, string title)
        {
            toolTip1.ToolTipTitle = title;
            toolTip1.SetToolTip(sender, caption);
        }

        private void Sqdswitch_6_CheckedChanged(object sender, EventArgs e)
        {
            enable_set_sqd();
        }

        private void Nolog_checkbox_CheckedChanged(object sender, EventArgs e)
        {

        }
        private void programSettingChanged(object sender, EventArgs e)
        {
            CheckBox chkbox = (CheckBox)sender;
            if (chkbox.Name == "nolog_checkbox")
            {
                if (chkbox.Checked)
                {
                    showdetailLog_checkbox.Checked = false;
                    showdetailLog_checkbox.Enabled = false;
                }
                else
                    showdetailLog_checkbox.Enabled = true;
            }
        }

        private void Commander_name_TextChanged(object sender, EventArgs e)
        {
            userinfo["user_info"]["name"] = commander_name.Text;

        }

        private void Commander_exp_TextChanged(object sender, EventArgs e)
        {
            userinfo["user_info"]["experience"] = commander_exp.Text;
        }

        private void Normalbattle_CheckedChanged(object sender, EventArgs e)
        {
            if (theaterbattle.Checked)
            {

            }
        }



        private void TextBox2_TextChanged(object sender, EventArgs e)
        {
            userinfo["theater_exercise_info"]["enemy_teams"] = theater_enemyid_set.Text;
            preview_theater_enemy_set();
        }

        private void Error_error1_CheckedChanged(object sender, EventArgs e)
        {
            if (error_error0.Checked)
                reset_error = "error:0";
            else if (error_error1.Checked)
                reset_error = "error:1";
            else if (error_error2.Checked)
                reset_error = "error:2";
            else if (error_error3.Checked)
                reset_error = "error:3";
        }

        private void Theater_area_num_SelectedIndexChanged(object sender, EventArgs e)
        {
            userinfo["theater_exercise_info"]["theater_area_id"] = "5" + (theater_area.SelectedIndex + 1).ToString() + (theater_area_num.SelectedIndex == -1 ? 1 : (theater_area_num.SelectedIndex + 1)).ToString();
            theater_enemy_random_set();
        }
        void preview_theater_enemy_set()
        {
            theater_enemy_preview_listbox.Items.Clear();
            string[] list = theater_enemyid_set.Text.Split(',');
            string currentid = userinfo["theater_exercise_info"]["theater_area_id"].ToString();
            foreach (var a in (JArray)theater_enemy_info[currentid.Substring(0, 2)][currentid.Substring(2, 1)][list[(int)theater_wave_set.Value - 1]])
            {
                ListViewItem itm = new ListViewItem(a.ToString(), 7);
                theater_enemy_preview_listbox.Items.Add(itm);
                //theater_enemy_preview_listbox.Items.Add(a.ToString());
            }

        }
        private void Theater_enemy_random_btn_Click(object sender, EventArgs e)
        {
            theater_enemy_random_set();
        }
        void theater_enemy_random_set()
        {
            string a = "";
            string currentid = userinfo["theater_exercise_info"]["theater_area_id"].ToString();
            int max = theater_enemy_info[currentid.Substring(0, 2)][currentid.Substring(2, 1)].Count();
            Random random = new Random();
            for (int i = 0; i < 10; i++)
            {
                a += random.Next(0, max);
                if (i != 9)
                    a += ",";
            }
            theater_enemyid_set.Text = a;
        }

        private void Theater_wave_set_ValueChanged(object sender, EventArgs e)
        {
            preview_theater_enemy_set();
        }

        private void Theater_area_core_occ_CheckedChanged(object sender, EventArgs e)
        {
            if (theater_area_bigginer_occ.Checked)
            {
                foreach (var a in (JArray)Theater_data["theater_info"])
                {
                    if (a["theater_id"].ToString() == "51")
                    {
                        a["battle_pt"] = "195000000";
                    }
                }
            }
            else
            {
                foreach (var a in (JArray)Theater_data["theater_info"])
                {
                    if (a["theater_id"].ToString() == "51")
                    {
                        a["battle_pt"] = "10";
                    }
                }
            }
            if (theater_area_mid_occ.Checked)
            {
                foreach (var a in (JArray)Theater_data["theater_info"])
                {
                    if (a["theater_id"].ToString() == "52")
                    {
                        a["battle_pt"] = "465000000";
                    }
                }
            }
            else
            {
                foreach (var a in (JArray)Theater_data["theater_info"])
                {
                    if (a["theater_id"].ToString() == "52")
                    {
                        a["battle_pt"] = "10";
                    }
                }
            }
            if (theater_area_adv_occ.Checked)
            {
                foreach (var a in (JArray)Theater_data["theater_info"])
                {
                    if (a["theater_id"].ToString() == "53")
                    {
                        a["battle_pt"] = "1640000000";
                    }
                }
            }
            else
            {
                foreach (var a in (JArray)Theater_data["theater_info"])
                {
                    if (a["theater_id"].ToString() == "53")
                    {
                        a["battle_pt"] = "10";
                    }
                }
            }
            if (theater_area_core_occ.Checked)
            {
                foreach (var a in (JArray)Theater_data["theater_info"])
                {
                    if (a["theater_id"].ToString() == "54")
                    {
                        a["battle_pt"] = "9999000000";
                    }
                }
            }
            else
            {
                foreach (var a in (JArray)Theater_data["theater_info"])
                {
                    if (a["theater_id"].ToString() == "54")
                    {
                        a["battle_pt"] = "10";
                    }
                }
            }
        }

        private void Theater_sqd_6_CheckedChanged(object sender, EventArgs e)
        {
            Control last_cb = (Control)sender;
            int check_num = 0;
            int cnt = 0;
            string sqd = "";
            for (int i = 1; i <= 6; i++)
            {
                CheckBox cb = (CheckBox)Controls.Find("theater_sqd_" + i.ToString(), true)[0];
                if (cb.Checked) check_num++;
            }
            if (check_num > 3)
            {
                ((CheckBox)last_cb).Checked = false;
            }
            for (int i = 1; i <= 6; i++)
            {
                CheckBox cb = (CheckBox)Controls.Find("theater_sqd_" + i.ToString(), true)[0];
                if (cb.Checked) { sqd += i.ToString(); cnt++; if (cnt != check_num) { sqd += ","; } }

            }
            userinfo["theater_exercise_info"]["theater_squads"] = sqd;
        }

        private void Echelon_select_SelectedIndexChanged(object sender, EventArgs e)
        {
            int[] gunid = gun_id.ToArray();
            for (int i = 1; i <= 5; i++)
            {
                for (int k = 0; k < gun_eh_array.Count(); k++)
                {
                    if (gun_eh_array[k]["id"].ToString() == gun_user_id[echelon_select.SelectedIndex, i - 1].ToString())
                    {
                        JObject defStat = new JObject(getGunDefaultStat(1, int.Parse(gun_eh_array[k]["gun_id"].ToString()), 1));
                        ((ComboBox)Controls.Find("gunid_" + i.ToString() + "_combobox", true)[0]).SelectedIndex = Array.IndexOf(gunid, int.Parse(gun_eh_array[k]["gun_id"].ToString()));
                        ((NumericUpDown)Controls.Find("gunlv_" + i.ToString() + "_number", true)[0]).Value = !check_ifmod(i) && int.Parse(gun_eh_array[k]["gun_level"].ToString()) > 100 ? 100 : int.Parse(gun_eh_array[k]["gun_level"].ToString());
                        ((NumericUpDown)Controls.Find("gunpos_" + i.ToString() + "_number", true)[0]).Value = Array.IndexOf(gun_position, int.Parse(gun_eh_array[k]["position"].ToString()));
                        ((NumericUpDown)Controls.Find("gunhp_" + i.ToString() + "_number", true)[0]).Value = int.Parse(gun_eh_array[k]["life"].ToString());
                        ((NumericUpDown)Controls.Find("gunpow_" + i.ToString() + "_number", true)[0]).Value = int.Parse(gun_eh_array[k]["pow"].ToString()) + int.Parse(defStat["pow"].ToString());
                        ((NumericUpDown)Controls.Find("gunhit_" + i.ToString() + "_number", true)[0]).Value = int.Parse(gun_eh_array[k]["hit"].ToString()) + int.Parse(defStat["hit"].ToString());
                        ((NumericUpDown)Controls.Find("gundodge_" + i.ToString() + "_number", true)[0]).Value = int.Parse(gun_eh_array[k]["dodge"].ToString()) + int.Parse(defStat["dodge"].ToString());
                        ((NumericUpDown)Controls.Find("gunrate_" + i.ToString() + "_number", true)[0]).Value = int.Parse(gun_eh_array[k]["rate"].ToString()) + int.Parse(defStat["rate"].ToString());
                        ((NumericUpDown)Controls.Find("gunskill1_" + i.ToString() + "_number", true)[0]).Value = int.Parse(gun_eh_array[k]["skill1"].ToString());
                        ((NumericUpDown)Controls.Find("gunskill2_" + i.ToString() + "_number", true)[0]).Value = check_ifmod(i) ? int.Parse(gun_eh_array[k]["skill2"].ToString()) : 0; //?? ???? 0
                        ((NumericUpDown)Controls.Find("gundummy_" + i.ToString() + "_number", true)[0]).Value = int.Parse(gun_eh_array[k]["number"].ToString());
                        ((NumericUpDown)Controls.Find("gunfavor_" + i.ToString() + "_number", true)[0]).Value = int.Parse(gun_eh_array[k]["favor"].ToString()) / 10000;
                        ((CheckBox)Controls.Find("gunoath_" + i.ToString() + "_checkbox", true)[0]).Checked = Convert.ToBoolean(int.Parse(gun_eh_array[k]["soul_bond"].ToString()));
                        if (i != 1)
                        {
                            ((CheckBox)Controls.Find("enable_" + i.ToString(), true)[0]).Checked = Convert.ToBoolean(int.Parse(gun_eh_array[k]["team_id"].ToString()));
                        }
                    }
                }
            }
        }

        private void UpdatePosTile(object sender, EventArgs e)
        {
            string[] pos = Enumerable.Repeat("0", 9).ToArray();
            for (int i = 0; i < 5; i++)
            {
                CheckBox checkBox = (CheckBox)Controls.Find("enable_" + (i + 1).ToString(), true)[0];
                if (checkBox.Checked)
                {
                    NumericUpDown Num = (NumericUpDown)Controls.Find("gunpos_" + (i + 1).ToString() + "_number", true)[0];
                    pos[(int)Num.Value - 1] = "1," + (i + 1).ToString();
                }
                //else
                //pos[(int)Num.Value - 1] = "0";
            }
            for (int i = 0; i < 9; i++)
            {
                Button button = (Button)Controls.Find("postile_" + (i + 1).ToString(), true)[0];
                if (pos[i].StartsWith("1"))
                {
                    button.BackColor = ColorTranslator.FromHtml("#00aeff");
                    button.Text = pos[i].Substring(2);
                }
                else if (pos[i].StartsWith("0"))
                {
                    button.BackColor = Color.Transparent;
                    button.Text = string.Empty;
                }

            }

        }
        private void PosTile_Hover(object sender, EventArgs e)
        {
            Control control = (Control)sender;
            if (control.Text != "")
            {
                ComboBox comboBox = (ComboBox)Controls.Find("gunid_" + control.Text + "_combobox", true)[0];
                showTooltip(control, comboBox.Text);
            }
        }
        byte[] DecompressGzip(byte[] data)
        {
            try
            {
                using (var compressedStream = new MemoryStream(data))
                using (var zipStream = new GZipStream(compressedStream, CompressionMode.Decompress))
                using (var resultStream = new MemoryStream())
                {
                    zipStream.CopyTo(resultStream);
                    return resultStream.ToArray();
                }
            }
            catch
            {
                return data;
            }
        }
    }
}
