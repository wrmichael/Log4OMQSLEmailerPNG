
namespace Log4OMQSLEmailer
{
    partial class Form1
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
            this.txtStart = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txtEnd = new System.Windows.Forms.TextBox();
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.btnQuery = new System.Windows.Forms.Button();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.settingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aDIFToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.lookupInfoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.layoytSettingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.button1 = new System.Windows.Forms.Button();
            this.lstlog = new System.Windows.Forms.ListBox();
            this.label3 = new System.Windows.Forms.Label();
            this.button2 = new System.Windows.Forms.Button();
            this.txtADIFFile = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // txtStart
            // 
            this.txtStart.Location = new System.Drawing.Point(8, 108);
            this.txtStart.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.txtStart.Name = "txtStart";
            this.txtStart.Size = new System.Drawing.Size(175, 22);
            this.txtStart.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(8, 89);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(174, 17);
            this.label1.TabIndex = 1;
            this.label1.Text = "Start Date (YYYY-MM-DD)";
            this.label1.Click += new System.EventHandler(this.label1_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(201, 89);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(169, 17);
            this.label2.TabIndex = 3;
            this.label2.Text = "End Date (YYYY-MM-DD)";
            // 
            // txtEnd
            // 
            this.txtEnd.Location = new System.Drawing.Point(201, 108);
            this.txtEnd.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.txtEnd.Name = "txtEnd";
            this.txtEnd.Size = new System.Drawing.Size(183, 22);
            this.txtEnd.TabIndex = 2;
            // 
            // listBox1
            // 
            this.listBox1.FormattingEnabled = true;
            this.listBox1.ItemHeight = 16;
            this.listBox1.Location = new System.Drawing.Point(8, 158);
            this.listBox1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.listBox1.Name = "listBox1";
            this.listBox1.ScrollAlwaysVisible = true;
            this.listBox1.Size = new System.Drawing.Size(776, 100);
            this.listBox1.TabIndex = 4;
            // 
            // btnQuery
            // 
            this.btnQuery.Location = new System.Drawing.Point(643, 108);
            this.btnQuery.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnQuery.Name = "btnQuery";
            this.btnQuery.Size = new System.Drawing.Size(147, 30);
            this.btnQuery.TabIndex = 5;
            this.btnQuery.Text = "Email";
            this.btnQuery.UseVisualStyleBackColor = true;
            this.btnQuery.Click += new System.EventHandler(this.btnQuery_Click);
            // 
            // menuStrip1
            // 
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Padding = new System.Windows.Forms.Padding(5, 2, 0, 2);
            this.menuStrip1.Size = new System.Drawing.Size(800, 28);
            this.menuStrip1.TabIndex = 6;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.settingsToolStripMenuItem,
            this.aDIFToolStripMenuItem,
            this.layoytSettingsToolStripMenuItem,
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(46, 24);
            this.fileToolStripMenuItem.Text = "File";
            this.fileToolStripMenuItem.Click += new System.EventHandler(this.fileToolStripMenuItem_Click);
            // 
            // settingsToolStripMenuItem
            // 
            this.settingsToolStripMenuItem.Name = "settingsToolStripMenuItem";
            this.settingsToolStripMenuItem.Size = new System.Drawing.Size(192, 26);
            this.settingsToolStripMenuItem.Text = "Settings";
            this.settingsToolStripMenuItem.Click += new System.EventHandler(this.settingsToolStripMenuItem_Click);
            // 
            // aDIFToolStripMenuItem
            // 
            this.aDIFToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lookupInfoToolStripMenuItem});
            this.aDIFToolStripMenuItem.Name = "aDIFToolStripMenuItem";
            this.aDIFToolStripMenuItem.Size = new System.Drawing.Size(224, 26);
            this.aDIFToolStripMenuItem.Text = "ADIF";
            // 
            // lookupInfoToolStripMenuItem
            // 
            this.lookupInfoToolStripMenuItem.Name = "lookupInfoToolStripMenuItem";
            this.lookupInfoToolStripMenuItem.Size = new System.Drawing.Size(224, 26);
            this.lookupInfoToolStripMenuItem.Text = "Lookup Info";
            this.lookupInfoToolStripMenuItem.Click += new System.EventHandler(this.lookupInfoToolStripMenuItem_Click);
            // 
            // layoytSettingsToolStripMenuItem
            // 
            this.layoytSettingsToolStripMenuItem.Name = "layoytSettingsToolStripMenuItem";
            this.layoytSettingsToolStripMenuItem.Size = new System.Drawing.Size(192, 26);
            this.layoytSettingsToolStripMenuItem.Text = "Layoyt Settings";
            this.layoytSettingsToolStripMenuItem.Click += new System.EventHandler(this.layoytSettingsToolStripMenuItem_Click);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(192, 26);
            this.exitToolStripMenuItem.Text = "Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(711, 263);
            this.button1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 7;
            this.button1.Text = "Refresh";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // lstlog
            // 
            this.lstlog.FormattingEnabled = true;
            this.lstlog.ItemHeight = 16;
            this.lstlog.Location = new System.Drawing.Point(8, 288);
            this.lstlog.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.lstlog.Name = "lstlog";
            this.lstlog.ScrollAlwaysVisible = true;
            this.lstlog.Size = new System.Drawing.Size(776, 196);
            this.lstlog.TabIndex = 8;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(8, 137);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(152, 17);
            this.label3.TabIndex = 9;
            this.label3.Text = "Select Template Image";
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(737, 49);
            this.button2.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(48, 28);
            this.button2.TabIndex = 10;
            this.button2.Text = "...";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // txtADIFFile
            // 
            this.txtADIFFile.Location = new System.Drawing.Point(8, 49);
            this.txtADIFFile.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtADIFFile.Name = "txtADIFFile";
            this.txtADIFFile.Size = new System.Drawing.Size(720, 22);
            this.txtADIFFile.TabIndex = 11;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(8, 30);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(790, 17);
            this.label4.TabIndex = 12;
            this.label4.Text = "ADIF File (When selected will ignore dates and will not use LOG4OM2 DB Info -> Wi" +
    "ll need to lookup email from qrzcq or qrz)";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 498);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.txtADIFFile);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.lstlog);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.btnQuery);
            this.Controls.Add(this.listBox1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtEnd);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtStart);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "Form1";
            this.Text = "QSL Emailer";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtStart;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtEnd;
        private System.Windows.Forms.ListBox listBox1;
        private System.Windows.Forms.Button btnQuery;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem settingsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.ListBox lstlog;
        private System.Windows.Forms.ToolStripMenuItem layoytSettingsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aDIFToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem lookupInfoToolStripMenuItem;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.TextBox txtADIFFile;
        private System.Windows.Forms.Label label4;
    }
}

