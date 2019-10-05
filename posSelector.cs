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
    public partial class posSelector : Form
    {
        int selpos = 0;
        int num = 0;
        public posSelector(Control c, int p, int n)
        {
            InitializeComponent();

            this.Text = string.Format(Form1.frm.lang_data["posselector_title"].ToString(), n.ToString());
            selpos = p;
            num = n;

        }
        
        
        private void PosSelector_Load(object sender, EventArgs e)
        {
            for(int i=1; i <= 9; i++)
            {
                if(i == selpos)
                {
                    Button button = (Button)Controls.Find("button" + i.ToString(), true)[0];
                    button.BackColor = ColorTranslator.FromHtml("#ff6542");
                    button.Select();
                }
                
            }
        }
      
        private void Button_Click(object sender, EventArgs e)
        {
            Control _sender = (Control)sender;
            for (int i = 1; i <= 5; i++)
            {
                if(num == i)
                {
                    NumericUpDown Num = (NumericUpDown)Form1.frm.Controls.Find("gunpos_" + i.ToString() + "_number", true)[0];
                    Num.Value = decimal.Parse(_sender.Name.Replace("button", ""));
                }
                
            }
            Close();
        }

    }
}
