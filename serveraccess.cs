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
            
            
                for(int i=1; i <= 10; i++)
                {
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
                            for(int uu = 1; uu <= 3; uu++)
                            {
                                if(guns["equip" + uu.ToString()].ToString() != "0")
                                {
                                    gun_equip_ids.Add(guns["equip" + uu.ToString()].ToString());
                                }                              
                            }
                            o.Add("gun" + guns["location"].ToString(), n);
                            foreach (var keys in data["equip_with_user_info"])
                            {
                                for(int lis = 0; lis<gun_equip_ids.Count;lis++)
                                {
                                    if (gun_equip_ids[lis] == ((JProperty)keys).Name.ToString())
                                    {
                                        JObject t = new JObject((JObject)data["equip_with_user_info"][gun_equip_ids[lis]]);
                                        t["id"] = guns["location"].ToString() + (lis+1).ToString();
                                        t["gun_with_user_id"] = guns["location"].ToString();
                                        eq.Add(guns["location"].ToString() + (lis + 1).ToString(), t);
                                    }
                                }                               
                            }
                        }
                        }
                    }

                File.WriteAllText(@"userdata/presets/" + DateTime.Now.ToString("yyyyMMdd_HHmmss_") + data["user_info"]["name"].ToString() + "_e" + i.ToString() + ".equip", eq.ToString());
                File.WriteAllText(@"userdata/presets/" + DateTime.Now.ToString("yyyyMMdd_HHmmss_") + data["user_info"]["name"].ToString() + "_e" + i.ToString()+".gun", o.ToString());
                }
            
        }
        public void AddLog(string text)
        {
            logTextbox.AppendText(string.Format("[{0}]{1}" + Environment.NewLine, DateTime.Now.ToString("yyyy.MM.dd HH:mm:ss"), text));

        }

        private void Serveraccess_FormClosed(object sender, FormClosedEventArgs e)
        {
            form1.getUserinfoFromServer = false;
        }
    }
}
