using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GFBattleTester
{
    public partial class updater : Form
    {
        Form opener;
        List<string> updatefilelist = new List<string>();
        int downloadfilecount = 0;
        int currentcount = 0;
        bool isLatest = true;
        public updater(Form parentForm)
        {
            InitializeComponent();
            opener = parentForm;
        }
        //label1.Text = "다운로드 완료. 창을 닫아 프로그램을 재시작 해주세요.";
        private void updater_Load(object senders, EventArgs e)
        {
            
            //ServicePointManager.Expect100Continue = true;
            //ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                    
            backgroundWorker1.RunWorkerAsync();
           
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
        public string GetMD5HashFromFile(string filepath)
        {
            MD5 md5 = MD5.Create();
            FileStream stream = File.OpenRead(filepath);
            string hash = BitConverter.ToString(md5.ComputeHash(stream)).Replace("-", string.Empty).ToLower();
            stream.Close();
            return hash;
        }

        private void updater_FormClosing(object sender, FormClosingEventArgs e)
        {
            opener.Close();
        }
        void Downloadprogresschanged(object sender, DownloadProgressChangedEventArgs args)
        {
            if (args.TotalBytesToReceive > 0)
            {
                //progressBar1.Value = args.ProgressPercentage;
            }
        }
        void Downloadcompleted(object sender, DownloadDataCompletedEventArgs args)
        {
            if (args.Error == null)
            {              
                File.Delete(@updatefilelist[currentcount]);
                File.WriteAllBytes(@updatefilelist[currentcount], args.Result);
                currentcount++;
            }
        }

        private void BackgroundWorker1_DoWork(object o, DoWorkEventArgs e)
        {
            WebClient updatercl = new WebClient();
            byte[] updatelist = updatercl.DownloadData("https://dhlrunner.github.io/GFBT_update_data_files/update_files.txt");
            string[] list = Encoding.UTF8.GetString(updatelist).Split(Environment.NewLine.ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            foreach (string a in list)
            {
                if (a != "")
                {
                    string filepath = a.Split(',')[0];
                    string filehash = a.Split(',')[1];
                    if (GetMD5HashFromFile(filepath) != filehash)
                    {
                        updatefilelist.Add(filepath);
                        downloadfilecount++;
                        isLatest = false;
                    }
                }
            }
            if (!isLatest)
            {
                backgroundWorker2.RunWorkerAsync();
            }
            else
            {
                Close();
                opener.Opacity = 100;
            }
        }

        private void BackgroundWorker2_DoWork(object sender, DoWorkEventArgs e)
        {
            for(int i=0; i<updatefilelist.Count; i++)
            {
                WebClient wc = new WebClient();
                label1.Text = "다운로드 중... (" + (i + 1).ToString() + "/" + downloadfilecount.ToString() + ")";
                label2.Text = updatefilelist[i];
                byte[] data = wc.DownloadData(new Uri("https://dhlrunner.github.io/GFBT_update_data_files/" + updatefilelist[i]));
                File.WriteAllBytes(updatefilelist[i], data);
                /*using (WebClient wc = new WebClient())
                {
                    progressBar1.Style = ProgressBarStyle.Blocks;
                    progressBar1.Value = 0;
                    wc.DownloadDataCompleted += Downloadcompleted;
                    wc.DownloadProgressChanged += Downloadprogresschanged;
                    wc.DownloadDataAsync(new Uri("https://dhlrunner.github.io/GFBT_update_data_files/" + updatefilelist[i]));
                    Uri a = new Uri("https://dhlrunner.github.io/GFBT_update_data_files/" + updatefilelist[i]);
                }*/
            }
            label1.Text = "다운로드 완료. 창을 닫아 프로그램을 재시작 해주세요.";
            //progressBar1.Style = ProgressBarStyle.Continuous;
            //progressBar1.Value = 100;
            //opener.Close();
            //Close();
        }
    }
}
