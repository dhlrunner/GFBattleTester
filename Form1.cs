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
        
        public bool getUserinfoFromServer = false;
        public static Form1 frm;
        serveraccess serv ;
        static string signkey = "우중비모";
        string _token = string.Empty;
        bool gunsaved = false;
        string[] Equippos = { "11", "12", "13", "21", "22", "23", "31", "32", "33", "41", "42", "43", "51", "52", "53" };
        string equip = string.Empty;
        JObject gun_info_json_1 = new JObject();
        JObject gun_info_json_2 = new JObject();
        JObject gun_info_json_3 = new JObject();
        JObject gun_info_json_4 = new JObject();
        JObject gun_info_json_5 = new JObject();
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
        HttpListener listener = new HttpListener();
        Thread thread = new Thread(new ParameterizedThreadStart(WorkerThread));
        
        ManualResetEvent _pauseEvent = new ManualResetEvent(false);
        static JObject userinfo = new JObject();
        JObject enemy_team_info = new JObject();
        JObject enemy_character_info = new JObject();
        JArray gunstatdata = new JArray();
        JObject attribute = new JObject();
        JObject grow = new JObject();
        //JObject test_userinfo = JObject.Parse(File.ReadAllText("data/json/userinfo_test.json"));
        long[] gun_exp_table = {0,100,300,400,600,1000,1500,2100,2800,3600,4500,5500,6600,7800,9100,10500,
        12000,13600,15300,17100,19000,21000,23000,25300,27600,30000,32500,35100,37900,41000,44400,48600,
        53200,58200,63600,69400,75700,82400,89600,97300,105500,114300,123600,133500,144000,155100,166900,179400,
        192500,206400,221000,236400,252500,269400,287100,305700,325200,345600,366900,389200,412500,436800,462100,
        488400,515800,544300,573900,604700,636700,669900,704300,749400,796200,844800,895200,947400,1001400,1057300,
        1115200,1175000,1236800,1300700,1366700,1434800,1505100,1577700,1652500,1729600,1809100,1891000,1975300,
        2807900,2204000,2323500,2446600,2573300,2703700,2837800,2975700,3117500,3263200,3363200,3483200,3623200,3783200,
        3963200,4163200,4383200,4623200,4983200,5463200,6103200,7003200,8203200,98032000,12803200,16803200,21803200,27803200,32803200};

        long[] sqd_exp_table = { 0, 500,1400,2700,4500,6700,9400,12600,16200,20200,24700,29700,35100,40900,47200,54000,61200,68800,77100,86100,
        95900,106500,118500,132000,147000,163500,181800,201900,223900,247900,274200,302500,333300,366600,402400,441000,482400,526600,574000,624600,
        678400,735700,796500,861000,929200,1001500,1077900,1158400,1243300,1332700,1426800,1525600,1629400,1738300,1852300,1971800,2096700,
        2227200,2363500,2505900,2654400,2809000,2970100,3137800,3312300,3493800,3682300,3877800,4080800,4291400,4509600,4735800,4970000,
        5212500,5463300,5722800,5990800,6267800,6553800,6849300,7154000,7468500,7792500,8127000,8471000,8826000,9191000,9567000,9954000,
        10352000,10761000,11182000,11614000,12058000,12514000,12983000,13464000,13957000,14463000,15000000};
        int[] sqdID = { 227, 7615, 7138, 24369, 28011 };
        //HttpServer httpServer = new HttpServer(8080, Routes.GET);
        int[] gun_position = { -1, 7, 12, 17, 8, 13, 18, 9, 14, 19 }; //인덱스 = 키패드 배열, 값= 클라이언트상 값
        //Thread thread_http = new Thread(new ThreadStart(frm.httpServer.Listen));
        //string[] squad_names = { ""};
        int[] squad_BGM71_defaultstat = { 52, 135, 118, 28 };
        int[] squad_AT4_defaultstat = { 38, 88, 96, 45 };
        int[] squad_AGS30_defaultstat = { 27, 49, 67, 130 };
        int[] squad_2B14_defaultstat = { 51, 20, 46, 54 };
        int[] squad_M2_defaultstat = { 38, 17, 40, 61 };
        int 살상력 = 0, 파쇄력 = 1, 정밀성 = 2, 장전 = 3;
        int battleStarttime = 0;

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
            
            updatecheck();
            try
            {
                string guninfofile = File.ReadAllText(@"data/info_texts/guns.b64");
                string mission = File.ReadAllText("data/info_texts/mission.b64");
                
                JObject spotinfo = JObject.Parse(File.ReadAllText(@"data/json/spot_info.json"));
                guninfofile = Encoding.UTF8.GetString(Convert.FromBase64String(guninfofile));
                mission = Encoding.UTF8.GetString(Convert.FromBase64String(mission));
                
                mission_name = mission.Split('\n').ToList();

                enemy_team_info = JObject.Parse(File.ReadAllText(@"data/json/enemy_team_info.json"));
                userinfo = JObject.Parse(File.ReadAllText(@"data/json/userinfo.json"));
                enemy_character_info = JObject.Parse(File.ReadAllText(@"data/json/enemy_character_type_info.json"));
                string[] guninfofile_split = guninfofile.Split(new[] { Environment.NewLine }, StringSplitOptions.None);
                gun_info_json_1 = JObject.Parse(File.ReadAllText(@"data/json/default_gun.json"));
                gun_info_json_2 = JObject.Parse(File.ReadAllText(@"data/json/default_gun.json"));
                gun_info_json_3 = JObject.Parse(File.ReadAllText(@"data/json/default_gun.json"));
                gun_info_json_4 = JObject.Parse(File.ReadAllText(@"data/json/default_gun.json"));
                gun_info_json_5 = JObject.Parse(File.ReadAllText(@"data/json/default_gun.json"));
                gunstatdata = JArray.Parse(File.ReadAllText(@"data/json/doll.json"));
                attribute = JObject.Parse(File.ReadAllText(@"data/json/dollAttribute.json"));
                grow = JObject.Parse(File.ReadAllText(@"data/json/dollGrow.json"));

                portnum.Value = int.Parse(File.ReadAllText(@"data/port"));
                enemy_team_id_combobox.Text = File.ReadAllText(@"data/last_enemyID");
                Boss_HP_textbox.Value = int.Parse(File.ReadAllText(@"data/last_bossHP"));
                ChangeEnemyGroupID();
                ChangeEnemyBossHP();
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

                setSquadInfo(false); //중장비 초기화
                SetGunInfo(false); //인형 초기화
                {
                    sqd_1_damage.Minimum = squad_BGM71_defaultstat[살상력];
                    sqd_1_break.Minimum = squad_BGM71_defaultstat[파쇄력];
                    sqd_1_hit.Minimum = squad_BGM71_defaultstat[정밀성];
                    sqd_1_reload.Minimum = squad_BGM71_defaultstat[장전];

                    sqd_2_damage.Minimum = squad_AGS30_defaultstat[살상력];
                    sqd_2_break.Minimum = squad_AGS30_defaultstat[파쇄력];
                    sqd_2_hit.Minimum = squad_AGS30_defaultstat[정밀성];
                    sqd_2_reload.Minimum = squad_AGS30_defaultstat[장전];

                    sqd_3_damage.Minimum = squad_2B14_defaultstat[살상력];
                    sqd_3_break.Minimum = squad_2B14_defaultstat[파쇄력];
                    sqd_3_hit.Minimum = squad_2B14_defaultstat[정밀성];
                    sqd_3_reload.Minimum = squad_2B14_defaultstat[장전];

                    sqd_4_damage.Minimum = squad_M2_defaultstat[살상력];
                    sqd_4_break.Minimum = squad_M2_defaultstat[파쇄력];
                    sqd_4_hit.Minimum = squad_M2_defaultstat[정밀성];
                    sqd_4_reload.Minimum = squad_M2_defaultstat[장전];

                    sqd_5_damage.Minimum = squad_AT4_defaultstat[살상력];
                    sqd_5_break.Minimum = squad_AT4_defaultstat[파쇄력];
                    sqd_5_hit.Minimum = squad_AT4_defaultstat[정밀성];
                    sqd_5_reload.Minimum = squad_AT4_defaultstat[장전];
                }
                enable_set_sqd();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "파일 로드 실패", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                Close();
            }
            UpdatePosTile(null, null);
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
                string[] list = Encoding.UTF8.GetString(updatelist).Split(Environment.NewLine.ToCharArray(),StringSplitOptions.RemoveEmptyEntries);
                foreach (string a in list)
                {
                    if (a != "")
                    {
                        string filepath = a.Split(',')[0];
                        string filehash = a.Split(',')[1];
                        string localhash = GetMD5HashFromFile(filepath);
                        if (localhash != filehash)
                            isLatest = false;
                    }
                }
                if (!isLatest)
                {
                    if (MessageBox.Show("데이터파일의 업데이트가 필요합니다. 업데이트를 진행할까요?", "업데이트 있음", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
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
                MessageBox.Show("업데이트 확인에 실패했습니다." + Environment.NewLine + ex.ToString(), "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                MessageBox.Show("적 그룹을 선택해주세요.", "오류", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            else if (!gunsaved)
            {
                MessageBox.Show("먼저 인형설정에서 진형을 설정 후 \"결정\" 버튼을 눌러주세요.", "오류", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            else
            {
                if (!IsTcpPortAvailable(Convert.ToInt16(portnum.Value)))
                {
                    MessageBox.Show("선택한 포트 " + portnum.Value.ToString() + "번이 현제 다른 프로세스에 의해 사용 중입니다. 다른 포트번호를 설정해 주세요.", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
                else
                {
                    try
                    {
                        
                        userinfo["spot_act_info"][2]["enemy_team_id"] = enemy_team_id_combobox.Text;
                        userinfo["spot_act_info"][2]["boss_hp"] = Boss_HP_textbox.Value.ToString();
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


                        label117.Text = "서버 상태: 실행 중";
                        if (!checkBox1.Checked)
                            label119.Text = "서버 IP: " + GetLocalIP();
                        label120.Text = "서버 포트: " + portnum.Value.ToString();
                        AddLog((checkBox1.Checked ? "***.***.***.***" : GetLocalIP()) + ":" + portnum.Value.ToString() + " 에서 서버 시작됨; 적 그룹 ID:" + userinfo["spot_act_info"][2]["enemy_team_id"].ToString());


                        File.WriteAllText(@"data/port", portnum.Value.ToString());
                        button5.Enabled = false;
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
            // 포트 체크
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
                string html = "<!doctype html><html lang=\"ko\"><head><meta charset=\"utf-8\"><title>GFHelper</title></head><body>Server is working</br><a href=\"data/Cert/GFBattleTester.cer/\">인증서 다운로드</a></body></html>";
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

            if (!frm.getUserinfoFromServer) //!유저정보
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
                    frm.AddLog("유저 정보 취득");
                    ResponceProcessBinary(ctx, Encoding.UTF8.GetBytes(userinfo.ToString()), true, false);
                }
                else if (ctx.Request.Url.LocalPath == GF_URLs_Kr.battlefinish)
                {
                    Random random = new Random();
                    JObject clientdata = JObject.Parse(Packet.Decode(Outdatacode, signkey));
                    //Clipboard.SetText(clientdata.ToString());
                    JObject saveJson = new JObject();
                    JArray guninfo = new JArray();
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
                    saveJson.Add("EnemyGroupID", userinfo["spot_act_info"][2]["enemy_team_id"].ToString());
                    saveJson.Add("unknown", Xxtea.XXTEA.EncryptToBase64String(Encoding.UTF8.GetBytes(clientdata["1000"].ToString()), Encoding.UTF8.GetBytes("우중비모")));
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
                else if (ctx.Request.Url.LocalPath == GF_URLs_Kr.downloadsucsess)
                {
                    ResponceProcessBinary(ctx, Encoding.ASCII.GetBytes("1"), false, false);
                }//wc.Headers.Add("Accept","*/*");
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
                    frm.AddLog("(CrashReport) 클라이언트 충돌 보고서가 /CrashLog 폴더에 저장되었습니다.");
                }
                else if (ctx.Request.Url.LocalPath == GF_URLs_Kr.abortmission)
                {
                    frm.AddLog("클라이언트 강제 리셋");
                    ResponceProcessBinary(ctx, Encoding.ASCII.GetBytes("error:3"), false, false);
                }
                else if (ctx.Request.Url.LocalPath == GF_URLs_Kr.teammove || ctx.Request.Url.LocalPath == GF_URLs_Kr.squadMove)
                {
                    frm.AddLog("제대 이동: " + Packet.Decode(Outdatacode, signkey));
                    ResponceProcessBinary(ctx, Encoding.ASCII.GetBytes("1"), false, false);
                }
                else if (ctx.Request.Url.LocalPath == GF_URLs_Kr.squadSwitchChange)
                {
                    frm.AddLog("화력소대 지원 스위치 조작: " + Packet.Decode(Outdatacode, signkey));
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
                    frm.AddLog("클라이언트 강제 리셋");
                    ResponceProcessBinary(ctx, Encoding.ASCII.GetBytes("error:3"), false, false);
                }
                else
                {
                    //ResponceProcessBinary(ctx, Encoding.UTF8.GetBytes("우중이 애미 하늘로 날아감"), false,false);
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
                                frm.serv.AddLog("버전 정보 취득..");                               
                            }
                            else if (ctx.Request.Url.LocalPath == GF_URLs_Kr.getToken)
                            {
                                Packet.init();
                                if (Serverpacket.Contains("error"))
                                {
                                    frm.serv.AddLog("로그인...");
                                }
                                else
                                {                                   
                                    JObject tok = JObject.Parse(Packet.Decode(Serverpacket, "yundoudou"));
                                    frm._token = tok["sign"].ToString();
                                    frm.serv.AddLog("토큰 취득 성공: "+frm._token);
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
            //헤더 설정
            if (!isHeaderSettedAlready)
            {
                response.Headers.Add("Accept-Encoding", "none"); //gzip 처리하기 귀찮으므로 비압축
                response.Headers.Add("Content-Type", "text/html; charset=UTF-8");
                response.Headers.Add("Server", "ngnix");
                response.Headers.Add("X-Powered-By", " PHP/5.6.21");
                response.Headers.Add("X-Upstream", " 127.0.0.1:8080");
            }
            //스트림 쓰기
            response.ContentLength64 = data.Length; //데이터 길이 설정
            Stream output = response.OutputStream;
            output.Write(data, 0, data.Length);
            //frm.AddLog("Responce: " + Encoding.UTF8.GetString(data));
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
                    userinfo["spot_act_info"][2]["enemy_team_id"] = enemy_team_id_combobox.Text;
                    userinfo["spot_act_info"][2]["boss_hp"] = Boss_HP_textbox.Value.ToString();
                    AddLog("적 그룹 변경: " + userinfo["spot_act_info"][2]["enemy_team_id"].ToString());
                    label118.Text = "설정된 적 그룹: " + enemy_team_id_combobox.Text;
                    File.WriteAllText(@"data/last_enemyID", enemy_team_id_combobox.Text);
                }
                else
                    MessageBox.Show("숫자만 입력해주세요.", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
            catch (NullReferenceException)
            {
                if (MessageBox.Show("적 데이터베이스에 아직 등록되어 있지 않은 그룹 번호입니다. 강제로 설정할까요?", "ID 찾을 수 없음", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
                {
                    bosshp = 0;
                    Boss_HP_textbox.Value = bosshp;
                    userinfo["spot_act_info"][2]["enemy_team_id"] = enemy_team_id_combobox.Text;
                    userinfo["spot_act_info"][2]["boss_hp"] = Boss_HP_textbox.Value.ToString();
                    label118.Text = "설정된 적 그룹: " + enemy_team_id_combobox.Text;
                    AddLog("적 그룹 변경: " + userinfo["spot_act_info"][2]["enemy_team_id"].ToString());
                }
            }
        }
        void ChangeEnemyBossHP()
        {
            userinfo["spot_act_info"][2]["boss_hp"] = Boss_HP_textbox.Value.ToString();
            AddLog("보스 HP 변경: " + userinfo["spot_act_info"][2]["boss_hp"].ToString());
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
            SetGunInfo(true);

        }
        void SetGunInfo(bool showMessageBox)
        {
            bool duplicated = false;
            for (int i = 1; i <= 5; i++)
            {
                for (int j = 0;  j < 4;j++)
                {
                    List<int> n = new List<int>() { 1,2,3,4,5};
                    n.Remove(i);
                    NumericUpDown n1 = (NumericUpDown)Controls.Find("gunpos_" + i.ToString() + "_number", true)[0];
                    NumericUpDown n2 = (NumericUpDown)Controls.Find("gunpos_" +n[j].ToString() + "_number", true)[0];
                    if (n1.Value == n2.Value)
                    {
                        CheckBox c1 = (CheckBox)Controls.Find("enable_" + i.ToString(), true)[0];
                        CheckBox c2 = (CheckBox)Controls.Find("enable_" + n[j].ToString(), true)[0];
                        if (c1.Checked)
                        {
                            if(c2.Checked)
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
                MessageBox.Show("같은 인형을 여러명 배속할 수 없습니다.", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if (duplicated)
            {
                MessageBox.Show("중복되는 진형이 있습니다.", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            
            else if (gunid_1_combobox.SelectedIndex == -1 || gunid_2_combobox.SelectedIndex == -1
                || gunid_3_combobox.SelectedIndex == -1 || gunid_4_combobox.SelectedIndex == -1
                || gunid_5_combobox.SelectedIndex == -1)
            {
                MessageBox.Show("인형 ID 입력칸에 오류가 있습니다.", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                    MessageBox.Show("같은 인형을 중복 편성할 시 해당 인형의 스킬 발동에 문제가 생길 수 있습니다.", "경고", MessageBoxButtons.OK, MessageBoxIcon.Information);
                JArray gun_user_info = JArray.Parse(userinfo["gun_with_user_info"].ToString());
                gun_user_info.RemoveAll();
                for (int i = 1; i <= 5; i++)
                {
                    gun_user_info.Add(Get_user_gun_info_json(i));
                }
                userinfo["gun_with_user_info"].Replace(gun_user_info);
                //MessageBox.Show(Get_user_gun_info_json(1).ToString());
                if (showMessageBox)
                    MessageBox.Show("저장되었습니다.", "알림", MessageBoxButtons.OK, MessageBoxIcon.Information);
                gunsaved = true;
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
        JObject Get_user_gun_info_json(int num)
        {
            JObject defStat = new JObject();
            if (num == 1)
            {
                defStat = new JObject(getGunDefaultStat(1, int.Parse(gun_info[gunid_1_combobox.SelectedIndex].Split(',')[0].ToString()), 1));
                gun_info_json_1["id"] = num.ToString();
                gun_info_json_1["gun_id"] = int.Parse(gun_info[gunid_1_combobox.SelectedIndex].Split(',')[0]).ToString();
                gun_info_json_1["gun_exp"] = gun_exp_table[Convert.ToInt16(gunlv_1_number.Value - 1)].ToString();
                gun_info_json_1["gun_level"] = gunlv_1_number.Value.ToString();
                gun_info_json_1["team_id"] = check_gun(num) ? "1" : "0";
                gun_info_json_1["if_modification"] = check_ifmod(1) ? getModState((int)gunlv_1_number.Value) : "0";
                gun_info_json_1["location"] = num.ToString();
                gun_info_json_1["position"] = gun_position[Convert.ToInt16(gunpos_1_number.Value)].ToString();
                gun_info_json_1["life"] = gunhp_1_number.Value.ToString();
                gun_info_json_1["pow"] = (gunpow_1_number.Value - gunpow_1_number.Minimum).ToString();
                gun_info_json_1["hit"] = (gunhit_1_number.Value - gunhit_1_number.Minimum).ToString();
                gun_info_json_1["dodge"] = (gundodge_1_number.Value - gundodge_1_number.Minimum).ToString();
                gun_info_json_1["rate"] = (gunrate_1_number.Value - gunrate_1_number.Minimum).ToString();
                gun_info_json_1["skill1"] = gunskill1_1_number.Value.ToString();
                gun_info_json_1["skill2"] = gunskill1_1_number.Value.ToString();
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
                gun_info_json_2["id"] = num.ToString();
                gun_info_json_2["gun_id"] = int.Parse(gun_info[gunid_2_combobox.SelectedIndex].Split(',')[0]).ToString();
                gun_info_json_2["gun_exp"] = gun_exp_table[Convert.ToInt16(gunlv_2_number.Value - 1)].ToString();
                gun_info_json_2["gun_level"] = gunlv_2_number.Value.ToString();
                gun_info_json_2["team_id"] = check_gun(num) ? "1" : "0";
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
                gun_info_json_3["id"] = num.ToString();
                gun_info_json_3["gun_id"] = int.Parse(gun_info[gunid_3_combobox.SelectedIndex].Split(',')[0]).ToString();
                gun_info_json_3["gun_exp"] = gun_exp_table[Convert.ToInt16(gunlv_3_number.Value - 1)].ToString();
                gun_info_json_3["gun_level"] = gunlv_3_number.Value.ToString();
                gun_info_json_3["team_id"] = check_gun(num) ? "1" : "0";
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
                gun_info_json_4["id"] = num.ToString();
                gun_info_json_4["gun_id"] = int.Parse(gun_info[gunid_4_combobox.SelectedIndex].Split(',')[0]).ToString();
                gun_info_json_4["gun_exp"] = gun_exp_table[Convert.ToInt16(gunlv_4_number.Value - 1)].ToString();
                gun_info_json_4["gun_level"] = gunlv_4_number.Value.ToString();
                gun_info_json_4["team_id"] = check_gun(num) ? "1" : "0";
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
                gun_info_json_5["id"] = num.ToString();
                gun_info_json_5["gun_id"] = int.Parse(gun_info[gunid_5_combobox.SelectedIndex].Split(',')[0]).ToString();
                gun_info_json_5["gun_exp"] = gun_exp_table[Convert.ToInt16(gunlv_5_number.Value - 1)].ToString();
                gun_info_json_5["gun_level"] = gunlv_5_number.Value.ToString();
                gun_info_json_5["team_id"] = check_gun(num) ? "1" : "0";
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
                return null;
        }
        private void enable_1_CheckedChanged(object sender, EventArgs e)
        {
            enable_set(1);
            UpdatePosTile(null,null);
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
                JObject o = Get_user_gun_info_json(i);
                o["equip1"] = "0";
                o["equip2"] = "0";
                o["equip3"] = "0";
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
                loadinfofromfile(file);
                label115.Text = Path.GetFileName(path);
                SetGunInfo(false);
            }
            catch (Exception ex)
            {
                MessageBox.Show("파일을 불러오는 중에 오류가 발생했습니다." + Environment.NewLine + ex.ToString(), "불러오기 실패", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        void loadinfofromfile(JObject json)
        {
            int[] gunid = gun_id.ToArray();
            #region LoadJsonData
            if(json.SelectToken("gun1") != null)
            {
                gun_info_json_1["id"] = json["gun1"]["id"].ToString();
                gun_info_json_1["gun_id"] = json["gun1"]["gun_id"].ToString();
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
                gun_info_json_2["id"] = json["gun2"]["id"].ToString();
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
                gun_info_json_3["id"] = json["gun3"]["id"].ToString();
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
                gun_info_json_4["id"] = json["gun4"]["id"].ToString();
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
                gun_info_json_5["id"] = json["gun5"]["id"].ToString();
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
            #endregion

            #region SetJsonData
            //gun_info_json_1["id"]
            //gun_info_json_1["equip1"] = "0";
            //gun_info_json_1["equip2"] = "0";
            //gun_info_json_1["equip3"] = "0";
            //gun_info_json_1["skin"] = "0";
            //gun_info_json_1["if_modification"] = "0";
            //gun_info_json_1["location"] = num.ToString();

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
            gunskill2_1_number.Value = check_ifmod(1) ? int.Parse(gun_info_json_1["skill2"].ToString()) : 0; //개조 아닐경우 0
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


            #endregion

            //enemy_team_id_combobox.Text = json["enemyGroupID"].ToString();

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
            JObject sqdSwitch = JObject.Parse(userinfo["mission_act_info"]["squad_info"].ToString());

            #region SetData
            sqdSwitch["1"]["battleskill_switch"] = Convert.ToInt16(sqdswitch_1.Checked);
            sqdSwitch["2"]["battleskill_switch"] = Convert.ToInt16(sqdswitch_2.Checked);
            sqdSwitch["3"]["battleskill_switch"] = Convert.ToInt16(sqdswitch_3.Checked);
            sqdSwitch["4"]["battleskill_switch"] = Convert.ToInt16(sqdswitch_4.Checked);
            sqdSwitch["5"]["battleskill_switch"] = Convert.ToInt16(sqdswitch_5.Checked);
            userinfo["mission_act_info"]["squad_info"] = sqdSwitch.ToString();
            for (int i = 0; i < 5; i++)
            {
                sqd.Add(userinfo["squad_with_user_info"][sqdID[i].ToString()]);
                switch (i)
                {
                    case 0:
                        sqd[i]["squad_id"] = (i + 1).ToString();
                        sqd[i]["squad_exp"] = sqd_exp_table[Convert.ToInt16(sqd_1_lv.Value) - 1].ToString();
                        sqd[i]["squad_level"] = sqd_1_lv.Value.ToString();
                        sqd[i]["assist_damage"] = (sqd_1_damage.Value - squad_BGM71_defaultstat[살상력]).ToString();
                        sqd[i]["assist_reload"] = (sqd_1_reload.Value - squad_BGM71_defaultstat[장전]).ToString();
                        sqd[i]["assist_hit"] = (sqd_1_hit.Value - squad_BGM71_defaultstat[정밀성]).ToString();
                        sqd[i]["assist_def_break"] = (sqd_1_break.Value - squad_BGM71_defaultstat[파쇄력]).ToString();
                        sqd[i]["skill1"] = sqd_1_skill1.Value.ToString();
                        sqd[i]["skill2"] = sqd_1_skill2.Value.ToString();
                        sqd[i]["skill3"] = sqd_1_skill3.Value.ToString();
                        break;
                    case 1:
                        sqd[i]["squad_id"] = (i + 1).ToString();
                        sqd[i]["squad_exp"] = sqd_exp_table[Convert.ToInt16(sqd_2_lv.Value) - 1].ToString();
                        sqd[i]["squad_level"] = sqd_2_lv.Value.ToString();
                        sqd[i]["assist_damage"] = (sqd_2_damage.Value - squad_AGS30_defaultstat[살상력]).ToString();
                        sqd[i]["assist_reload"] = (sqd_2_reload.Value - squad_AGS30_defaultstat[장전]).ToString();
                        sqd[i]["assist_hit"] = (sqd_2_hit.Value - squad_AGS30_defaultstat[정밀성]).ToString();
                        sqd[i]["assist_def_break"] = (sqd_2_break.Value - squad_AGS30_defaultstat[파쇄력]).ToString();
                        sqd[i]["skill1"] = sqd_2_skill1.Value.ToString();
                        sqd[i]["skill2"] = sqd_2_skill2.Value.ToString();
                        sqd[i]["skill3"] = sqd_2_skill3.Value.ToString();
                        break;
                    case 2:
                        sqd[i]["squad_id"] = (i + 1).ToString();
                        sqd[i]["squad_exp"] = sqd_exp_table[Convert.ToInt16(sqd_3_lv.Value) - 1].ToString();
                        sqd[i]["squad_level"] = sqd_3_lv.Value.ToString();
                        sqd[i]["assist_damage"] = (sqd_3_damage.Value - squad_2B14_defaultstat[살상력]).ToString();
                        sqd[i]["assist_reload"] = (sqd_3_reload.Value - squad_2B14_defaultstat[장전]).ToString();
                        sqd[i]["assist_hit"] = (sqd_3_hit.Value - squad_2B14_defaultstat[정밀성]).ToString();
                        sqd[i]["assist_def_break"] = (sqd_3_break.Value - squad_2B14_defaultstat[파쇄력]).ToString();
                        sqd[i]["skill1"] = sqd_3_skill1.Value.ToString();
                        sqd[i]["skill2"] = sqd_3_skill2.Value.ToString();
                        sqd[i]["skill3"] = sqd_3_skill3.Value.ToString();
                        break;
                    case 3:
                        sqd[i]["squad_id"] = (i + 1).ToString();
                        sqd[i]["squad_exp"] = sqd_exp_table[Convert.ToInt16(sqd_4_lv.Value) - 1].ToString();
                        sqd[i]["squad_level"] = sqd_4_lv.Value.ToString();
                        sqd[i]["assist_damage"] = (sqd_4_damage.Value - squad_M2_defaultstat[살상력]).ToString();
                        sqd[i]["assist_reload"] = (sqd_4_reload.Value - squad_M2_defaultstat[장전]).ToString();
                        sqd[i]["assist_hit"] = (sqd_4_hit.Value - squad_M2_defaultstat[정밀성]).ToString();
                        sqd[i]["assist_def_break"] = (sqd_4_break.Value - squad_M2_defaultstat[파쇄력]).ToString();
                        sqd[i]["skill1"] = sqd_4_skill1.Value.ToString();
                        sqd[i]["skill2"] = sqd_4_skill2.Value.ToString();
                        sqd[i]["skill3"] = sqd_4_skill3.Value.ToString();
                        break;
                    case 4:
                        sqd[i]["squad_id"] = (i + 1).ToString();
                        sqd[i]["squad_exp"] = sqd_exp_table[Convert.ToInt16(sqd_5_lv.Value) - 1].ToString();
                        sqd[i]["squad_level"] = sqd_5_lv.Value.ToString();
                        sqd[i]["assist_damage"] = (sqd_5_damage.Value - squad_AT4_defaultstat[살상력]).ToString();
                        sqd[i]["assist_reload"] = (sqd_5_reload.Value - squad_AT4_defaultstat[장전]).ToString();
                        sqd[i]["assist_hit"] = (sqd_5_hit.Value - squad_AT4_defaultstat[정밀성]).ToString();
                        sqd[i]["assist_def_break"] = (sqd_5_break.Value - squad_AT4_defaultstat[파쇄력]).ToString();
                        sqd[i]["skill1"] = sqd_5_skill1.Value.ToString();
                        sqd[i]["skill2"] = sqd_5_skill2.Value.ToString();
                        sqd[i]["skill3"] = sqd_5_skill3.Value.ToString();
                        break;
                }
                temp.Add(sqdID[i].ToString(), sqd[i]);
            }
            userinfo["squad_with_user_info"].Replace(temp);
            if (showConfirmMessage)
                MessageBox.Show("저장되었습니다.", "알림", MessageBoxButtons.OK, MessageBoxIcon.Information);
            #endregion

        }
        void SaveSquadInfo(string path)
        {
            JObject savefile = new JObject();
            try
            {
                for (int i = 0; i < 5; i++)
                {
                    savefile.Add("Squad" + (i + 1).ToString(), userinfo["squad_with_user_info"][sqdID[i].ToString()]);
                }
                savefile.Add("switch1", Convert.ToInt16(sqdswitch_1.Checked));
                savefile.Add("switch2", Convert.ToInt16(sqdswitch_2.Checked));
                savefile.Add("switch3", Convert.ToInt16(sqdswitch_3.Checked));
                savefile.Add("switch4", Convert.ToInt16(sqdswitch_4.Checked));
                savefile.Add("switch5", Convert.ToInt16(sqdswitch_5.Checked));
                File.WriteAllText(path, savefile.ToString());
            }
            catch (Exception ex)
            {
                MessageBox.Show("파일을 저장하는 중 오류가 발생했습니다." + Environment.NewLine + ex.ToString(), "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        void loadSquadInfoFromFile(JObject json)
        {
            try
            {
                #region LoadSqdDataFromJson
                sqd_1_lv.Value = int.Parse(json["Squad1"]["squad_level"].ToString());
                sqd_1_damage.Value = int.Parse(json["Squad1"]["assist_damage"].ToString()) + squad_BGM71_defaultstat[살상력];
                sqd_1_reload.Value = int.Parse(json["Squad1"]["assist_reload"].ToString()) + squad_BGM71_defaultstat[장전];
                sqd_1_hit.Value = int.Parse(json["Squad1"]["assist_hit"].ToString()) + squad_BGM71_defaultstat[정밀성];
                sqd_1_break.Value = int.Parse(json["Squad1"]["assist_def_break"].ToString()) + squad_BGM71_defaultstat[파쇄력];
                sqd_1_skill1.Value = int.Parse(json["Squad1"]["skill1"].ToString());
                sqd_1_skill2.Value = int.Parse(json["Squad1"]["skill2"].ToString());
                sqd_1_skill3.Value = int.Parse(json["Squad1"]["skill3"].ToString());

                sqd_2_lv.Value = int.Parse(json["Squad2"]["squad_level"].ToString());
                sqd_2_damage.Value = int.Parse(json["Squad2"]["assist_damage"].ToString()) + squad_AGS30_defaultstat[살상력];
                sqd_2_reload.Value = int.Parse(json["Squad2"]["assist_reload"].ToString()) + squad_AGS30_defaultstat[장전];
                sqd_2_hit.Value = int.Parse(json["Squad2"]["assist_hit"].ToString()) + squad_AGS30_defaultstat[정밀성];
                sqd_2_break.Value = int.Parse(json["Squad2"]["assist_def_break"].ToString()) + squad_AGS30_defaultstat[파쇄력];
                sqd_2_skill1.Value = int.Parse(json["Squad2"]["skill1"].ToString());
                sqd_2_skill2.Value = int.Parse(json["Squad2"]["skill2"].ToString());
                sqd_2_skill3.Value = int.Parse(json["Squad2"]["skill3"].ToString());

                sqd_3_lv.Value = int.Parse(json["Squad3"]["squad_level"].ToString());
                sqd_3_damage.Value = int.Parse(json["Squad3"]["assist_damage"].ToString()) + squad_2B14_defaultstat[살상력];
                sqd_3_reload.Value = int.Parse(json["Squad3"]["assist_reload"].ToString()) + squad_2B14_defaultstat[장전];
                sqd_3_hit.Value = int.Parse(json["Squad3"]["assist_hit"].ToString()) + squad_2B14_defaultstat[정밀성];
                sqd_3_break.Value = int.Parse(json["Squad3"]["assist_def_break"].ToString()) + squad_2B14_defaultstat[파쇄력];
                sqd_3_skill1.Value = int.Parse(json["Squad3"]["skill1"].ToString());
                sqd_3_skill2.Value = int.Parse(json["Squad3"]["skill2"].ToString());
                sqd_3_skill3.Value = int.Parse(json["Squad3"]["skill3"].ToString());

                sqd_4_lv.Value = int.Parse(json["Squad4"]["squad_level"].ToString());
                sqd_4_damage.Value = int.Parse(json["Squad4"]["assist_damage"].ToString()) + squad_M2_defaultstat[살상력];
                sqd_4_reload.Value = int.Parse(json["Squad4"]["assist_reload"].ToString()) + squad_M2_defaultstat[장전];
                sqd_4_hit.Value = int.Parse(json["Squad4"]["assist_hit"].ToString()) + squad_M2_defaultstat[정밀성];
                sqd_4_break.Value = int.Parse(json["Squad4"]["assist_def_break"].ToString()) + squad_M2_defaultstat[파쇄력];
                sqd_4_skill1.Value = int.Parse(json["Squad4"]["skill1"].ToString());
                sqd_4_skill2.Value = int.Parse(json["Squad4"]["skill2"].ToString());
                sqd_4_skill3.Value = int.Parse(json["Squad4"]["skill3"].ToString());

                sqd_5_lv.Value = int.Parse(json["Squad5"]["squad_level"].ToString());
                sqd_5_damage.Value = int.Parse(json["Squad5"]["assist_damage"].ToString()) + squad_AT4_defaultstat[살상력];
                sqd_5_reload.Value = int.Parse(json["Squad5"]["assist_reload"].ToString()) + squad_AT4_defaultstat[장전];
                sqd_5_hit.Value = int.Parse(json["Squad5"]["assist_hit"].ToString()) + squad_AT4_defaultstat[정밀성];
                sqd_5_break.Value = int.Parse(json["Squad5"]["assist_def_break"].ToString()) + squad_AT4_defaultstat[파쇄력];
                sqd_5_skill1.Value = int.Parse(json["Squad5"]["skill1"].ToString());
                sqd_5_skill2.Value = int.Parse(json["Squad5"]["skill2"].ToString());
                sqd_5_skill3.Value = int.Parse(json["Squad5"]["skill3"].ToString());

                sqdswitch_1.Checked = Convert.ToBoolean(int.Parse(json["switch1"].ToString()));
                sqdswitch_2.Checked = Convert.ToBoolean(int.Parse(json["switch2"].ToString()));
                sqdswitch_3.Checked = Convert.ToBoolean(int.Parse(json["switch3"].ToString()));
                sqdswitch_4.Checked = Convert.ToBoolean(int.Parse(json["switch4"].ToString()));
                sqdswitch_5.Checked = Convert.ToBoolean(int.Parse(json["switch5"].ToString()));

                for (int i = 0; i < 5; i++)
                {
                    userinfo["squad_with_user_info"][sqdID[i].ToString()].Replace(json["Squad" + (i + 1).ToString()]);
                }
                setSquadInfo(false);
                #endregion
                label116.Text = Path.GetFileName(openFileDialog2.FileName);
            }
            catch (Exception ex)
            {
                MessageBox.Show("파일을 불러오는 중 오류가 발생했습니다." + Environment.NewLine + ex.ToString(), "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
            showTooltip((Control)sender, "현제 입력되어 있는 인형의 수치를 프리셋 파일로 저장합니다.");
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
            showTooltip((Control)sender, "저장한 인형 프리셋 파일을 불러옵니다.");
        }

        private void button4_MouseHover(object sender, EventArgs e)
        {
            showTooltip((Control)sender, "현제 입력되어 있는 수치를 적용합니다.");
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
                MessageBox.Show("선택한 인형은 아직 스텟 데이터베이스에 존재하지 않습니다. (추후 업데이트 예정)", "스텟 계산 불가", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }
        private void button9_MouseHover(object sender, EventArgs e)
        {
            showTooltip((Control)sender, "입력된 레벨, 편제 수에 따른 능력치로 기본 스텟을 계산하여 값을 설정합니다. 현제 입력된 HP,화력,회피,명중,사속치는 초기화됩니다." + Environment.NewLine + "주) 데이터베이스에 존재하는 인형만");
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
            if (checkBox1.Checked)
                label119.Text = "서버 IP: ******";
            else
            {
                if (listener.IsListening)
                    label119.Text = "서버 IP: " + GetLocalIP();
                else
                    label119.Text = "서버 IP: ";
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
            for(int i = 0; i< listView1.Columns.Count; i++)
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
        void getEquipInfoFromForm2(JObject json, string name,bool clear)
        {
            string pos = name.Split(';')[1];
            string ename = name.Split(';')[0];
            if (pos == "11")
            {
                if (clear)
                {
                    gun_info_json_1["equip1"] = "0";
                    setEquip_11.Text = "설정된 장비 없음";
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
            else if(pos == "12")
            {
                if (clear)
                {
                    gun_info_json_1["equip2"] = "0";
                    setEquip_12.Text = "설정된 장비 없음";
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
                    setEquip_13.Text = "설정된 장비 없음";
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
                    setEquip_21.Text = "설정된 장비 없음";
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
                    setEquip_22.Text = "설정된 장비 없음";
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
                    setEquip_23.Text = "설정된 장비 없음";
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
                    setEquip_31.Text = "설정된 장비 없음";
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
                    setEquip_32.Text = "설정된 장비 없음";
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
                    setEquip_33.Text = "설정된 장비 없음";
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
                    setEquip_41.Text = "설정된 장비 없음";
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
                    setEquip_42.Text = "설정된 장비 없음";
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
                    setEquip_43.Text = "설정된 장비 없음";
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
                    setEquip_51.Text = "설정된 장비 없음";
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
                    setEquip_52.Text = "설정된 장비 없음";
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
                    setEquip_53.Text = "설정된 장비 없음";
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
            SetGunInfo(false);
        }
        #region showEquipselect
        private void button15_Click(object sender, EventArgs e)
        {
            OpenEquipSelector("11;"+setEquip_11.Text);
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
            Load_Equip_Info_From_File(openFileDialog3.FileName);
        }
        public void Load_Equip_Info_From_File(string path)
        {
            JObject eq = JObject.Parse(File.ReadAllText(path));
            foreach (string a in Equippos)
            {

                if (eq[a] != null)
                {
                    this.Controls.Find("setEquip_" + a, true)[0].Text = equipName[equipID.IndexOf(eq[a]["equip_id"].ToString())];
                    if (a == "11")
                    {
                        gun_info_json_1["equip1"] = a;
                    }
                    else if (a == "12")
                    {
                        gun_info_json_1["equip2"] = a;
                    }
                    else if (a == "13")
                    {
                        gun_info_json_1["equip3"] = a;
                    }
                    else if (a == "21")
                    {
                        gun_info_json_2["equip1"] = a;
                    }
                    else if (a == "22")
                    {
                        gun_info_json_2["equip2"] = a;
                    }
                    else if (a == "23")
                    {
                        gun_info_json_2["equip3"] = a;
                    }
                    else if (a == "31")
                    {
                        gun_info_json_3["equip1"] = a;
                    }
                    else if (a == "32")
                    {
                        gun_info_json_3["equip2"] = a;
                    }
                    else if (a == "33")
                    {
                        gun_info_json_3["equip3"] = a;
                    }
                    else if (a == "41")
                    {
                        gun_info_json_4["equip1"] = a;
                    }
                    else if (a == "42")
                    {
                        gun_info_json_4["equip2"] = a;
                    }
                    else if (a == "43")
                    {
                        gun_info_json_4["equip3"] = a;
                    }
                    else if (a == "51")
                    {
                        gun_info_json_5["equip1"] = a;
                    }
                    else if (a == "52")
                    {
                        gun_info_json_5["equip2"] = a;
                    }
                    else if (a == "53")
                    {
                        gun_info_json_5["equip3"] = a;
                    }
                }
            }
            userinfo["equip_with_user_info"].Replace(eq);
            label121.Text = Path.GetFileName(path);
            SetGunInfo(false);
        }
        private void button17_Click(object sender, EventArgs e)
        {
            if(MessageBox.Show("현제 설정되어 있는 모든 장비를 제거합니다. 이 작업은 취소할 수 없습니다.", "경고", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation) == DialogResult.OK)
            {
                for(int i=1; i<=5; i++)
                {
                    for(int j=1;j<=3;j++)
                    {
                        foreach(string s in Equippos)
                        {
                            Controls.Find("setEquip_" + s, true)[0].Text = "설정된 장비 없음";
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
                SetGunInfo(false);
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
            showTooltip((Control)sender, "현제 입력되어 있는 중장비부대의 설정값을 파일로 저장합니다.");
        }

        private void Button7_MouseHover(object sender, EventArgs e)
        {
            showTooltip((Control)sender, "저장한 설정값(.sqd) 파일을 불러옵니다.");
        }

        private void Button8_MouseHover(object sender, EventArgs e)
        {
            showTooltip((Control)sender, "현제 입력되어 있는 중장비부대의 설정값을 적용합니다.");
        }

        private void Button5_MouseHover(object sender, EventArgs e)
        {
            showTooltip((Control)sender, "입력된 포트에서 서버를 시작합니다.");
        }

        private void CheckBox1_MouseHover(object sender, EventArgs e)
        {
            showTooltip((Control)sender, "서버 IP를 숨깁니다. 로그에도 적용됩니다.");
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
        void ShowposSelector(int n ,int selpos,Control c)
        {
            foreach (Form f in Application.OpenForms) //중복로드 방지
            {
                if (f.Name == "posSelector")
                {
                    f.Activate();
                    System.Media.SystemSounds.Asterisk.Play();
                    return;                   
                }
            }
            GroupBox gbox = (GroupBox)Controls.Find("groupBox" + n.ToString(), true)[0];
            posSelector posselector = new posSelector(c,selpos ,n);            
            posselector.StartPosition = FormStartPosition.Manual;
            posselector.Location = new Point(c.Location.X+gbox.Location.X+100,c.Location.Y+gbox.Location.Y+100);
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
            if(MessageBox.Show("유저 정보 가져오기 모드를 활성화할까요? " +
                "소녀전선 클라이언트를 이용하여 전투 서버를 시뮬레이트 하는 대신 소녀전선 서버로부터 유저 정보를 가져옵니다." +
                " 유저 정보 가져오기에 성공하면 이 모드는 자동으로 비활성화 됩니다.", "유저 정보 가져오기 모드", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
            {
                if (!listener.IsListening)
                {
                    if(MessageBox.Show("서버가 아직 실행되지 않았습니다. 실행할까요?","서버 미실행",MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
                    {
                       if(run_server())
                        {
                            MessageBox.Show("서버가 실행 되었습니다.", "서버 실행됨", MessageBoxButtons.OK, MessageBoxIcon.Information);                            
                            serv.Show();
                            frm.getUserinfoFromServer = true;
                            button23.Enabled = false;
                        }
                       else
                        {
                            MessageBox.Show("서버 실행에 실패했습니다.", "서버 실행 실패", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            MessageBox.Show("서버가 실행되지 않았기 때문에 유저 정보 불러오기 모드는 활성화되지 않았습니다.", "서버 실행 실패", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                        
                    }
                    else
                    {
                        MessageBox.Show("서버가 실행되지 않았기 때문에 유저 정보 불러오기 모드는 활성화되지 않았습니다.", "서버 실행 실패", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
                else
                {
                    serv.Show();

                    frm.getUserinfoFromServer = true;
                    button23.Enabled = false;
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
           for(int i = 0; i < 9; i++)
            {                            
                Button button = (Button)Controls.Find("postile_" + (i+1).ToString(), true)[0];
                if(pos[i].StartsWith("1"))
                {
                    button.BackColor = ColorTranslator.FromHtml("#00aeff");
                    button.Text = pos[i].Substring(2);
                }
                else if(pos[i].StartsWith("0"))
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
