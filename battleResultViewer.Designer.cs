namespace GFBattleTester
{
    partial class battleResultViewer
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
            this.components = new System.ComponentModel.Container();
            this.listView1 = new System.Windows.Forms.ListView();
            this.time = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.fps = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.target = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.Event = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.currentPos = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.remain = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.listView2 = new System.Windows.Forms.ListView();
            this.date = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.leader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.mvp = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.result = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.memberNum = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.battleTime = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.enemyLeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.enemyGroupID = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.maxDamage = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.totalDamagefromenemy = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.totalDamagetoEnemy = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.fileName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.listView3 = new System.Windows.Forms.ListView();
            this.pos = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.name = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.hp = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.hpafterbattle = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.totaldamage = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.p2 = new System.Windows.Forms.Button();
            this.p3 = new System.Windows.Forms.Button();
            this.p4 = new System.Windows.Forms.Button();
            this.p5 = new System.Windows.Forms.Button();
            this.p6 = new System.Windows.Forms.Button();
            this.p7 = new System.Windows.Forms.Button();
            this.p8 = new System.Windows.Forms.Button();
            this.p9 = new System.Windows.Forms.Button();
            this.p1 = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.hpbar7 = new System.Windows.Forms.ProgressBar();
            this.hpbar1 = new System.Windows.Forms.ProgressBar();
            this.hpbar2 = new System.Windows.Forms.ProgressBar();
            this.hpbar3 = new System.Windows.Forms.ProgressBar();
            this.hpbar6 = new System.Windows.Forms.ProgressBar();
            this.hpbar9 = new System.Windows.Forms.ProgressBar();
            this.hpbar5 = new System.Windows.Forms.ProgressBar();
            this.hpbar8 = new System.Windows.Forms.ProgressBar();
            this.hpbar4 = new System.Windows.Forms.ProgressBar();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // listView1
            // 
            this.listView1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.listView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.time,
            this.fps,
            this.target,
            this.Event,
            this.currentPos,
            this.remain});
            this.listView1.FullRowSelect = true;
            this.listView1.HideSelection = false;
            this.listView1.Location = new System.Drawing.Point(0, 498);
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(494, 362);
            this.listView1.TabIndex = 0;
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.View = System.Windows.Forms.View.Details;
            this.listView1.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.ListView1_ColumnClick);
            // 
            // time
            // 
            this.time.Text = "시간(초)";
            this.time.Width = 70;
            // 
            // fps
            // 
            this.fps.Text = "프레임";
            this.fps.Width = 55;
            // 
            // target
            // 
            this.target.Text = "대상";
            this.target.Width = 97;
            // 
            // Event
            // 
            this.Event.Text = "동작";
            this.Event.Width = 144;
            // 
            // currentPos
            // 
            this.currentPos.Text = "현제 위치";
            this.currentPos.Width = 75;
            // 
            // remain
            // 
            this.remain.Text = "남은인원";
            this.remain.Width = 0;
            // 
            // listView2
            // 
            this.listView2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listView2.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.date,
            this.leader,
            this.mvp,
            this.result,
            this.memberNum,
            this.battleTime,
            this.enemyLeader,
            this.enemyGroupID,
            this.maxDamage,
            this.totalDamagefromenemy,
            this.totalDamagetoEnemy,
            this.fileName});
            this.listView2.FullRowSelect = true;
            this.listView2.HideSelection = false;
            this.listView2.Location = new System.Drawing.Point(0, 0);
            this.listView2.Name = "listView2";
            this.listView2.Size = new System.Drawing.Size(1388, 499);
            this.listView2.TabIndex = 5;
            this.listView2.UseCompatibleStateImageBehavior = false;
            this.listView2.View = System.Windows.Forms.View.Details;
            this.listView2.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.ListView2_ColumnClick);
            this.listView2.SelectedIndexChanged += new System.EventHandler(this.ListView2_SelectedIndexChanged);
            // 
            // date
            // 
            this.date.Text = "기록된 날짜";
            this.date.Width = 161;
            // 
            // leader
            // 
            this.leader.Text = "리더";
            this.leader.Width = 120;
            // 
            // mvp
            // 
            this.mvp.Text = "MVP";
            this.mvp.Width = 120;
            // 
            // result
            // 
            this.result.Text = "전투상태";
            this.result.Width = 85;
            // 
            // memberNum
            // 
            this.memberNum.Text = "출격인원";
            this.memberNum.Width = 85;
            // 
            // battleTime
            // 
            this.battleTime.Text = "전투시간";
            this.battleTime.Width = 90;
            // 
            // enemyLeader
            // 
            this.enemyLeader.Text = "적 리더";
            this.enemyLeader.Width = 130;
            // 
            // enemyGroupID
            // 
            this.enemyGroupID.Text = "적 그룹ID";
            this.enemyGroupID.Width = 90;
            // 
            // maxDamage
            // 
            this.maxDamage.Text = "최대 딜";
            this.maxDamage.Width = 80;
            // 
            // totalDamagefromenemy
            // 
            this.totalDamagefromenemy.Text = "총 받은 데미지";
            this.totalDamagefromenemy.Width = 120;
            // 
            // totalDamagetoEnemy
            // 
            this.totalDamagetoEnemy.Text = "총 딜량";
            this.totalDamagetoEnemy.Width = 85;
            // 
            // fileName
            // 
            this.fileName.Text = "기록 파일명";
            this.fileName.Width = 200;
            // 
            // listView3
            // 
            this.listView3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.listView3.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.pos,
            this.name,
            this.hp,
            this.hpafterbattle,
            this.totaldamage});
            this.listView3.FullRowSelect = true;
            this.listView3.HideSelection = false;
            this.listView3.Location = new System.Drawing.Point(865, 498);
            this.listView3.Name = "listView3";
            this.listView3.Size = new System.Drawing.Size(523, 362);
            this.listView3.TabIndex = 6;
            this.listView3.UseCompatibleStateImageBehavior = false;
            this.listView3.View = System.Windows.Forms.View.Details;
            this.listView3.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.ListView3_ColumnClick);
            // 
            // pos
            // 
            this.pos.Text = "시작위치";
            this.pos.Width = 70;
            // 
            // name
            // 
            this.name.Text = "이름";
            this.name.Width = 120;
            // 
            // hp
            // 
            this.hp.Text = "HP";
            // 
            // hpafterbattle
            // 
            this.hpafterbattle.Text = "전투후HP";
            this.hpafterbattle.Width = 75;
            // 
            // totaldamage
            // 
            this.totaldamage.Text = "피해량";
            this.totaldamage.Width = 75;
            // 
            // timer1
            // 
            this.timer1.Interval = 1000;
            this.timer1.Tick += new System.EventHandler(this.Timer1_Tick);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.tableLayoutPanel1.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Single;
            this.tableLayoutPanel1.ColumnCount = 3;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel1.Controls.Add(this.p2, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.p3, 2, 2);
            this.tableLayoutPanel1.Controls.Add(this.p4, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.p5, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.p6, 2, 1);
            this.tableLayoutPanel1.Controls.Add(this.p7, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.p8, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.p9, 2, 0);
            this.tableLayoutPanel1.Controls.Add(this.p1, 0, 2);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(522, 553);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(318, 283);
            this.tableLayoutPanel1.TabIndex = 7;
            // 
            // p2
            // 
            this.p2.Enabled = false;
            this.p2.Location = new System.Drawing.Point(109, 192);
            this.p2.Name = "p2";
            this.p2.Size = new System.Drawing.Size(98, 87);
            this.p2.TabIndex = 1;
            this.p2.UseVisualStyleBackColor = true;
            // 
            // p3
            // 
            this.p3.Enabled = false;
            this.p3.Location = new System.Drawing.Point(214, 192);
            this.p3.Name = "p3";
            this.p3.Size = new System.Drawing.Size(100, 87);
            this.p3.TabIndex = 2;
            this.p3.UseVisualStyleBackColor = true;
            // 
            // p4
            // 
            this.p4.Enabled = false;
            this.p4.Location = new System.Drawing.Point(4, 98);
            this.p4.Name = "p4";
            this.p4.Size = new System.Drawing.Size(98, 86);
            this.p4.TabIndex = 3;
            this.p4.UseVisualStyleBackColor = true;
            // 
            // p5
            // 
            this.p5.Enabled = false;
            this.p5.Location = new System.Drawing.Point(109, 98);
            this.p5.Name = "p5";
            this.p5.Size = new System.Drawing.Size(98, 86);
            this.p5.TabIndex = 4;
            this.p5.UseVisualStyleBackColor = true;
            // 
            // p6
            // 
            this.p6.Enabled = false;
            this.p6.Location = new System.Drawing.Point(214, 98);
            this.p6.Name = "p6";
            this.p6.Size = new System.Drawing.Size(100, 86);
            this.p6.TabIndex = 5;
            this.p6.UseVisualStyleBackColor = true;
            // 
            // p7
            // 
            this.p7.Enabled = false;
            this.p7.Location = new System.Drawing.Point(4, 4);
            this.p7.Name = "p7";
            this.p7.Size = new System.Drawing.Size(98, 87);
            this.p7.TabIndex = 6;
            this.p7.UseVisualStyleBackColor = true;
            // 
            // p8
            // 
            this.p8.Enabled = false;
            this.p8.Location = new System.Drawing.Point(109, 4);
            this.p8.Name = "p8";
            this.p8.Size = new System.Drawing.Size(98, 86);
            this.p8.TabIndex = 7;
            this.p8.UseVisualStyleBackColor = true;
            // 
            // p9
            // 
            this.p9.Enabled = false;
            this.p9.Location = new System.Drawing.Point(214, 4);
            this.p9.Name = "p9";
            this.p9.Size = new System.Drawing.Size(100, 86);
            this.p9.TabIndex = 8;
            this.p9.UseVisualStyleBackColor = true;
            // 
            // p1
            // 
            this.p1.Enabled = false;
            this.p1.Location = new System.Drawing.Point(4, 192);
            this.p1.Name = "p1";
            this.p1.Size = new System.Drawing.Size(98, 87);
            this.p1.TabIndex = 0;
            this.p1.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(520, 538);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(57, 12);
            this.label1.TabIndex = 8;
            this.label1.Text = "진형 정보";
            // 
            // hpbar7
            // 
            this.hpbar7.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.hpbar7.Location = new System.Drawing.Point(527, 629);
            this.hpbar7.Name = "hpbar7";
            this.hpbar7.Size = new System.Drawing.Size(96, 14);
            this.hpbar7.TabIndex = 9;
            // 
            // hpbar1
            // 
            this.hpbar1.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.hpbar1.Location = new System.Drawing.Point(527, 817);
            this.hpbar1.Name = "hpbar1";
            this.hpbar1.Size = new System.Drawing.Size(96, 14);
            this.hpbar1.TabIndex = 10;
            // 
            // hpbar2
            // 
            this.hpbar2.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.hpbar2.Location = new System.Drawing.Point(632, 817);
            this.hpbar2.Name = "hpbar2";
            this.hpbar2.Size = new System.Drawing.Size(96, 14);
            this.hpbar2.TabIndex = 11;
            // 
            // hpbar3
            // 
            this.hpbar3.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.hpbar3.Location = new System.Drawing.Point(737, 817);
            this.hpbar3.Name = "hpbar3";
            this.hpbar3.Size = new System.Drawing.Size(98, 14);
            this.hpbar3.TabIndex = 12;
            // 
            // hpbar6
            // 
            this.hpbar6.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.hpbar6.Location = new System.Drawing.Point(737, 722);
            this.hpbar6.Name = "hpbar6";
            this.hpbar6.Size = new System.Drawing.Size(98, 14);
            this.hpbar6.TabIndex = 13;
            // 
            // hpbar9
            // 
            this.hpbar9.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.hpbar9.Location = new System.Drawing.Point(737, 628);
            this.hpbar9.Name = "hpbar9";
            this.hpbar9.Size = new System.Drawing.Size(98, 14);
            this.hpbar9.TabIndex = 14;
            // 
            // hpbar5
            // 
            this.hpbar5.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.hpbar5.Location = new System.Drawing.Point(632, 722);
            this.hpbar5.Name = "hpbar5";
            this.hpbar5.Size = new System.Drawing.Size(96, 14);
            this.hpbar5.TabIndex = 15;
            // 
            // hpbar8
            // 
            this.hpbar8.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.hpbar8.Location = new System.Drawing.Point(632, 628);
            this.hpbar8.Name = "hpbar8";
            this.hpbar8.Size = new System.Drawing.Size(96, 14);
            this.hpbar8.TabIndex = 16;
            // 
            // hpbar4
            // 
            this.hpbar4.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.hpbar4.Location = new System.Drawing.Point(527, 722);
            this.hpbar4.Name = "hpbar4";
            this.hpbar4.Size = new System.Drawing.Size(96, 14);
            this.hpbar4.TabIndex = 17;
            // 
            // battleResultViewer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1388, 860);
            this.Controls.Add(this.listView3);
            this.Controls.Add(this.listView1);
            this.Controls.Add(this.hpbar4);
            this.Controls.Add(this.hpbar8);
            this.Controls.Add(this.hpbar5);
            this.Controls.Add(this.hpbar9);
            this.Controls.Add(this.hpbar6);
            this.Controls.Add(this.hpbar3);
            this.Controls.Add(this.hpbar2);
            this.Controls.Add(this.hpbar1);
            this.Controls.Add(this.hpbar7);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.listView2);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "battleResultViewer";
            this.Text = "battleResultViewer";
            this.Load += new System.EventHandler(this.BattleResultViewer_Load);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListView listView1;
        private System.Windows.Forms.ColumnHeader time;
        private System.Windows.Forms.ColumnHeader fps;
        private System.Windows.Forms.ColumnHeader target;
        private System.Windows.Forms.ColumnHeader Event;
        private System.Windows.Forms.ColumnHeader currentPos;
        private System.Windows.Forms.ColumnHeader remain;
        private System.Windows.Forms.ListView listView2;
        private System.Windows.Forms.ColumnHeader date;
        private System.Windows.Forms.ColumnHeader leader;
        private System.Windows.Forms.ColumnHeader mvp;
        private System.Windows.Forms.ColumnHeader memberNum;
        private System.Windows.Forms.ColumnHeader battleTime;
        private System.Windows.Forms.ColumnHeader enemyLeader;
        private System.Windows.Forms.ColumnHeader maxDamage;
        private System.Windows.Forms.ColumnHeader totalDamagefromenemy;
        private System.Windows.Forms.ColumnHeader totalDamagetoEnemy;
        private System.Windows.Forms.ColumnHeader fileName;
        private System.Windows.Forms.ColumnHeader result;
        private System.Windows.Forms.ColumnHeader enemyGroupID;
        private System.Windows.Forms.ListView listView3;
        private System.Windows.Forms.ColumnHeader pos;
        private System.Windows.Forms.ColumnHeader name;
        private System.Windows.Forms.ColumnHeader hp;
        private System.Windows.Forms.ColumnHeader hpafterbattle;
        private System.Windows.Forms.ColumnHeader totaldamage;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Button p1;
        private System.Windows.Forms.Button p2;
        private System.Windows.Forms.Button p3;
        private System.Windows.Forms.Button p4;
        private System.Windows.Forms.Button p5;
        private System.Windows.Forms.Button p6;
        private System.Windows.Forms.Button p7;
        private System.Windows.Forms.Button p8;
        private System.Windows.Forms.Button p9;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ProgressBar hpbar7;
        private System.Windows.Forms.ProgressBar hpbar1;
        private System.Windows.Forms.ProgressBar hpbar2;
        private System.Windows.Forms.ProgressBar hpbar3;
        private System.Windows.Forms.ProgressBar hpbar6;
        private System.Windows.Forms.ProgressBar hpbar9;
        private System.Windows.Forms.ProgressBar hpbar5;
        private System.Windows.Forms.ProgressBar hpbar8;
        private System.Windows.Forms.ProgressBar hpbar4;
    }
}