namespace GFBattleTester
{
    partial class serveraccess
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.listView1 = new System.Windows.Forms.ListView();
            this.echelon = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.gun1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.gun2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.gun3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.gun4 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.gun5 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.filename = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.logTextbox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // listView1
            // 
            this.listView1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.echelon,
            this.gun1,
            this.gun2,
            this.gun3,
            this.gun4,
            this.gun5,
            this.filename});
            this.listView1.FullRowSelect = true;
            this.listView1.HideSelection = false;
            this.listView1.Location = new System.Drawing.Point(12, 12);
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(698, 214);
            this.listView1.TabIndex = 0;
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.View = System.Windows.Forms.View.Details;
            this.listView1.DoubleClick += new System.EventHandler(this.ListView1_DoubleClick);
            // 
            // echelon
            // 
            this.echelon.Text = "제대";
            this.echelon.Width = 80;
            // 
            // gun1
            // 
            this.gun1.Text = "1번 인형";
            this.gun1.Width = 100;
            // 
            // gun2
            // 
            this.gun2.Text = "2번 인형";
            this.gun2.Width = 100;
            // 
            // gun3
            // 
            this.gun3.Text = "3번 인형";
            this.gun3.Width = 100;
            // 
            // gun4
            // 
            this.gun4.Text = "4번 인형";
            this.gun4.Width = 100;
            // 
            // gun5
            // 
            this.gun5.Text = "5번 인형";
            this.gun5.Width = 100;
            // 
            // filename
            // 
            this.filename.Text = "파일명";
            this.filename.Width = 120;
            // 
            // logTextbox
            // 
            this.logTextbox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.logTextbox.Location = new System.Drawing.Point(12, 381);
            this.logTextbox.Multiline = true;
            this.logTextbox.Name = "logTextbox";
            this.logTextbox.Size = new System.Drawing.Size(698, 144);
            this.logTextbox.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 238);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(593, 12);
            this.label1.TabIndex = 2;
            this.label1.Text = "제대가 불러와진 후, 불러오고 싶은 제대의 행을 더블 클릭하면 해당 제대가 자동으로 프로그램에 로딩됩니다.";
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Location = new System.Drawing.Point(12, 264);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(224, 16);
            this.checkBox1.TabIndex = 3;
            this.checkBox1.Text = "해당 제대의 장비도 같이 불러옵니다.";
            this.checkBox1.UseVisualStyleBackColor = true;
            // 
            // serveraccess
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(722, 537);
            this.Controls.Add(this.checkBox1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.logTextbox);
            this.Controls.Add(this.listView1);
            this.Name = "serveraccess";
            this.ShowIcon = false;
            this.Text = "서버 접속기";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Serveraccess_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Serveraccess_FormClosed);
            this.Load += new System.EventHandler(this.Serveraccess_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListView listView1;
        private System.Windows.Forms.TextBox logTextbox;
        private System.Windows.Forms.ColumnHeader echelon;
        private System.Windows.Forms.ColumnHeader gun1;
        private System.Windows.Forms.ColumnHeader gun2;
        private System.Windows.Forms.ColumnHeader gun3;
        private System.Windows.Forms.ColumnHeader gun4;
        private System.Windows.Forms.ColumnHeader gun5;
        private System.Windows.Forms.ColumnHeader filename;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox checkBox1;
    }
}