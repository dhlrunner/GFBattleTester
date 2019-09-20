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
            this.SuspendLayout();
            // 
            // listView1
            // 
            this.listView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.time,
            this.fps,
            this.target,
            this.Event,
            this.currentPos,
            this.remain});
            this.listView1.Dock = System.Windows.Forms.DockStyle.Left;
            this.listView1.Location = new System.Drawing.Point(0, 515);
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(485, 242);
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
            this.listView2.Dock = System.Windows.Forms.DockStyle.Top;
            this.listView2.Location = new System.Drawing.Point(0, 0);
            this.listView2.Name = "listView2";
            this.listView2.Size = new System.Drawing.Size(1190, 515);
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
            this.result.Width = 65;
            // 
            // memberNum
            // 
            this.memberNum.Text = "출격인원";
            this.memberNum.Width = 65;
            // 
            // battleTime
            // 
            this.battleTime.Text = "전투시간";
            this.battleTime.Width = 70;
            // 
            // enemyLeader
            // 
            this.enemyLeader.Text = "적 리더";
            this.enemyLeader.Width = 130;
            // 
            // enemyGroupID
            // 
            this.enemyGroupID.Text = "적 그룹ID";
            this.enemyGroupID.Width = 75;
            // 
            // maxDamage
            // 
            this.maxDamage.Text = "최대 딜";
            this.maxDamage.Width = 65;
            // 
            // totalDamagefromenemy
            // 
            this.totalDamagefromenemy.Text = "총 받은 데미지";
            this.totalDamagefromenemy.Width = 86;
            // 
            // totalDamagetoEnemy
            // 
            this.totalDamagetoEnemy.Text = "총 딜량";
            this.totalDamagetoEnemy.Width = 65;
            // 
            // fileName
            // 
            this.fileName.Text = "기록 파일명";
            this.fileName.Width = 200;
            // 
            // listView3
            // 
            this.listView3.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.pos,
            this.name,
            this.hp,
            this.hpafterbattle,
            this.totaldamage});
            this.listView3.Dock = System.Windows.Forms.DockStyle.Right;
            this.listView3.Location = new System.Drawing.Point(761, 515);
            this.listView3.Name = "listView3";
            this.listView3.Size = new System.Drawing.Size(429, 242);
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
            // battleResultViewer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1190, 757);
            this.Controls.Add(this.listView3);
            this.Controls.Add(this.listView1);
            this.Controls.Add(this.listView2);
            this.Name = "battleResultViewer";
            this.Text = "battleResultViewer";
            this.Load += new System.EventHandler(this.BattleResultViewer_Load);
            this.ResumeLayout(false);

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
    }
}