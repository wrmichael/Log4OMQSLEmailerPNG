﻿namespace Log4OMQSLEmailer
{
    partial class BySKCC
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
            this.button1 = new System.Windows.Forms.Button();
            this.txtCallSign = new System.Windows.Forms.TextBox();
            this.button2 = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.button3 = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.button4 = new System.Windows.Forms.Button();
            this.ckIgnoreQSL = new System.Windows.Forms.CheckBox();
            this.label2 = new System.Windows.Forms.Label();
            this.ckcw = new System.Windows.Forms.CheckBox();
            this.ckQSOB4 = new System.Windows.Forms.CheckBox();
            this.ckDeadBeat = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // listView1
            // 
            this.listView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listView1.HideSelection = false;
            this.listView1.Location = new System.Drawing.Point(11, 212);
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(771, 157);
            this.listView1.TabIndex = 0;
            this.listView1.UseCompatibleStateImageBehavior = false;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(166, 14);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(91, 30);
            this.button1.TabIndex = 1;
            this.button1.Text = "Search";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // txtCallSign
            // 
            this.txtCallSign.Location = new System.Drawing.Point(12, 20);
            this.txtCallSign.Name = "txtCallSign";
            this.txtCallSign.Size = new System.Drawing.Size(118, 20);
            this.txtCallSign.TabIndex = 2;
            this.txtCallSign.TextChanged += new System.EventHandler(this.txtCallSign_TextChanged);
            this.txtCallSign.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtCallSign_KeyPress);
            // 
            // button2
            // 
            this.button2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button2.Location = new System.Drawing.Point(691, 375);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(91, 30);
            this.button2.TabIndex = 3;
            this.button2.Text = "Email Selected";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Visible = false;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(11, 46);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(116, 13);
            this.label3.TabIndex = 11;
            this.label3.Text = "Select Template Image";
            // 
            // listBox1
            // 
            this.listBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listBox1.FormattingEnabled = true;
            this.listBox1.Location = new System.Drawing.Point(11, 63);
            this.listBox1.Margin = new System.Windows.Forms.Padding(2);
            this.listBox1.Name = "listBox1";
            this.listBox1.ScrollAlwaysVisible = true;
            this.listBox1.Size = new System.Drawing.Size(771, 121);
            this.listBox1.TabIndex = 10;
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(726, 188);
            this.button3.Margin = new System.Windows.Forms.Padding(2);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(56, 19);
            this.button3.TabIndex = 12;
            this.button3.Text = "Refresh";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(14, 194);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(109, 13);
            this.label1.TabIndex = 13;
            this.label1.Text = "Select QSO\'s to email";
            this.label1.Click += new System.EventHandler(this.label1_Click);
            // 
            // button4
            // 
            this.button4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button4.Location = new System.Drawing.Point(691, 411);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(91, 30);
            this.button4.TabIndex = 14;
            this.button4.Text = "Create Images";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // ckIgnoreQSL
            // 
            this.ckIgnoreQSL.AutoSize = true;
            this.ckIgnoreQSL.Location = new System.Drawing.Point(263, 20);
            this.ckIgnoreQSL.Name = "ckIgnoreQSL";
            this.ckIgnoreQSL.Size = new System.Drawing.Size(113, 17);
            this.ckIgnoreQSL.TabIndex = 15;
            this.ckIgnoreQSL.Text = "Ignore QSL Status";
            this.ckIgnoreQSL.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(10, 5);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(135, 13);
            this.label2.TabIndex = 16;
            this.label2.Text = "Callsign - leave blank for all";
            // 
            // ckcw
            // 
            this.ckcw.AutoSize = true;
            this.ckcw.Checked = true;
            this.ckcw.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ckcw.Location = new System.Drawing.Point(402, 20);
            this.ckcw.Name = "ckcw";
            this.ckcw.Size = new System.Drawing.Size(68, 17);
            this.ckcw.TabIndex = 17;
            this.ckcw.Text = "CW Only";
            this.ckcw.UseVisualStyleBackColor = true;
            // 
            // ckQSOB4
            // 
            this.ckQSOB4.AutoSize = true;
            this.ckQSOB4.Location = new System.Drawing.Point(618, 37);
            this.ckQSOB4.Name = "ckQSOB4";
            this.ckQSOB4.Size = new System.Drawing.Size(162, 17);
            this.ckQSOB4.TabIndex = 33;
            this.ckQSOB4.Text = "Ignore QSO B4 (band/mode)";
            this.ckQSOB4.UseVisualStyleBackColor = true;
            // 
            // ckDeadBeat
            // 
            this.ckDeadBeat.AutoSize = true;
            this.ckDeadBeat.Location = new System.Drawing.Point(618, 14);
            this.ckDeadBeat.Name = "ckDeadBeat";
            this.ckDeadBeat.Size = new System.Drawing.Size(111, 17);
            this.ckDeadBeat.TabIndex = 32;
            this.ckDeadBeat.Text = "Ignore Deadbeats";
            this.ckDeadBeat.UseVisualStyleBackColor = true;
            // 
            // BySKCC
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.ckQSOB4);
            this.Controls.Add(this.ckDeadBeat);
            this.Controls.Add(this.ckcw);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.ckIgnoreQSL);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.listBox1);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.txtCallSign);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.listView1);
            this.Name = "BySKCC";
            this.Text = "By SKCC Bureau";
            this.Load += new System.EventHandler(this.ByCallSign_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListView listView1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox txtCallSign;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Label label3;
        public System.Windows.Forms.ListBox listBox1;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.CheckBox ckIgnoreQSL;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.CheckBox ckcw;
        private System.Windows.Forms.CheckBox ckQSOB4;
        private System.Windows.Forms.CheckBox ckDeadBeat;
    }
}