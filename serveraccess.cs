using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

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
        public void getUserdataFromServer()
        {
            MessageBox.Show("get");
        }
        private void AddLog(string text)
        {
            logTextbox.AppendText(string.Format("[{0}]{1}" + Environment.NewLine, DateTime.Now.ToString("yyyy.MM.dd HH:mm:ss"), text));

        }
    }
}
