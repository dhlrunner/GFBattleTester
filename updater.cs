using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GFBattleTester
{
    public partial class updater : Form
    {
        Form opener;

        public updater(Form parentForm)
        {
            InitializeComponent();
            opener = parentForm;
        }

        private void updater_Load(object senders, EventArgs e)
        {
            List<string> updatefilelist = new List<string>(); 
            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            bool isLatest = true;
            WebClient updatercl = new WebClient();
            byte[] updatelist = updatercl.DownloadData("http://dhlrunner.github.io/gfbtfiles/update_list.txt");
            string[] list = Encoding.UTF8.GetString(updatelist).Split(new[] { Environment.NewLine }, StringSplitOptions.None);
            foreach (string a in list)
            {
                if (a != "")
                {
                    string filepath = a.Split(',')[0];
                    string filehash = a.Split(',')[1];
                    if (getFileSizeMD5(filepath) != filehash)
                    {
                        updatefilelist.Add(filepath);
                        isLatest = false;
                    }
                        
                }
            }
            if (!isLatest)
            {
               
                for(int i=0; i<updatefilelist.Count; i++)
                {
                    label1.Text = "다운로드 중... (" + (i + 1).ToString() + "/" + updatefilelist.Count.ToString() + ")";
                    label2.Text = updatefilelist[i];
                    using (WebClient wc = new WebClient())
                    {     
                        progressBar1.Style = ProgressBarStyle.Blocks;
                        progressBar1.Value = 0;
                        // wc.DownloadFileAsync(new Uri("https://dhlrunner.github.io/gfbtfiles/" + updatefilelist[i]), "tempfile.temp");
                        byte[] d = wc.DownloadData(new Uri("https://dhlrunner.github.io/gfbtfiles/" + updatefilelist[i]));
                        File.Delete(@updatefilelist[i]);
                        File.WriteAllText(updatefilelist[i], Encoding.UTF8.GetString( d));
                        wc.DownloadProgressChanged += (sender, args) =>
                        {
                            if (args.TotalBytesToReceive > 0)
                            {
                                
                                    progressBar1.Value = args.ProgressPercentage;
                                
                                   
                            }
                                    
                        };
                        wc.DownloadFileCompleted += (sender, args) =>
                        {
                            if(args.Error == null)
                            {
                                File.Delete(@updatefilelist[i]);
                                File.Move("tempfile.temp", @updatefilelist[i]);
                            }                           
                        };
                    }
                }
                label1.Text = "다운로드 완료. 창을 닫아 프로그램을 재시작 해주세요.";

            }
            else
            {
                Close();
                opener.Opacity = 100;
            }
        }
        string getFileSizeMD5(string path)
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

        private void updater_FormClosing(object sender, FormClosingEventArgs e)
        {
            opener.Close();
        }
        void Downloadprogersschanged(object sender, DownloadProgressChangedEventArgs args)
        {

        }
        void Downloadcompleted(object sender, DownloadDataCompletedEventArgs args)
        {

        }
    }
}
