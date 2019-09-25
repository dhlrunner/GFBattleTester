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
            this.logTextbox = new System.Windows.Forms.TextBox();
            this.listView2 = new System.Windows.Forms.ListView();
            this.SuspendLayout();
            // 
            // listView1
            // 
            this.listView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.echelon,
            this.gun1,
            this.gun2,
            this.gun3,
            this.gun4,
            this.gun5});
            this.listView1.FullRowSelect = true;
            this.listView1.HideSelection = false;
            this.listView1.Location = new System.Drawing.Point(12, 12);
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(663, 214);
            this.listView1.TabIndex = 0;
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.View = System.Windows.Forms.View.Details;
            // 
            // echelon
            // 
            this.echelon.Text = "제대";
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
            // logTextbox
            // 
            this.logTextbox.Location = new System.Drawing.Point(12, 475);
            this.logTextbox.Multiline = true;
            this.logTextbox.Name = "logTextbox";
            this.logTextbox.Size = new System.Drawing.Size(663, 135);
            this.logTextbox.TabIndex = 1;
            // 
            // listView2
            // 
            this.listView2.HideSelection = false;
            this.listView2.Location = new System.Drawing.Point(12, 241);
            this.listView2.Name = "listView2";
            this.listView2.Size = new System.Drawing.Size(331, 213);
            this.listView2.TabIndex = 2;
            this.listView2.UseCompatibleStateImageBehavior = false;
            this.listView2.View = System.Windows.Forms.View.Details;
            // 
            // serveraccess
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(687, 622);
            this.Controls.Add(this.listView2);
            this.Controls.Add(this.logTextbox);
            this.Controls.Add(this.listView1);
            this.Name = "serveraccess";
            this.Text = "serveraccess";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Serveraccess_FormClosed);
            this.Load += new System.EventHandler(this.Serveraccess_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListView listView1;
        private System.Windows.Forms.TextBox logTextbox;
        private System.Windows.Forms.ListView listView2;
        private System.Windows.Forms.ColumnHeader echelon;
        private System.Windows.Forms.ColumnHeader gun1;
        private System.Windows.Forms.ColumnHeader gun2;
        private System.Windows.Forms.ColumnHeader gun3;
        private System.Windows.Forms.ColumnHeader gun4;
        private System.Windows.Forms.ColumnHeader gun5;
    }
}