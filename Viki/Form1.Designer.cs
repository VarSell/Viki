namespace Viki
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.LogBox = new System.Windows.Forms.RichTextBox();
            this.vikiLink = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.keyPair = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.releaseName = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.headerMeta = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.releaseYear = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.languageISO = new System.Windows.Forms.TextBox();
            this.button2 = new System.Windows.Forms.Button();
            this.label7 = new System.Windows.Forms.Label();
            this.audioUrl = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.videoUrl = new System.Windows.Forms.TextBox();
            this.LogBox1 = new System.Windows.Forms.RichTextBox();
            this.themoviedb = new System.Windows.Forms.Label();
            this.tmdb = new System.Windows.Forms.TextBox();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.panel3 = new System.Windows.Forms.Panel();
            this.panel4 = new System.Windows.Forms.Panel();
            this.panel5 = new System.Windows.Forms.Panel();
            this.panel6 = new System.Windows.Forms.Panel();
            this.backDrop = new System.Windows.Forms.PictureBox();
            this.button1 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.backDrop)).BeginInit();
            this.SuspendLayout();
            // 
            // LogBox
            // 
            this.LogBox.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.LogBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.LogBox.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.LogBox.Location = new System.Drawing.Point(10, 107);
            this.LogBox.Name = "LogBox";
            this.LogBox.Size = new System.Drawing.Size(244, 285);
            this.LogBox.TabIndex = 0;
            this.LogBox.Text = "";
            // 
            // vikiLink
            // 
            this.vikiLink.BackColor = System.Drawing.SystemColors.InactiveCaption;
            this.vikiLink.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.vikiLink.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.vikiLink.Location = new System.Drawing.Point(42, 20);
            this.vikiLink.Name = "vikiLink";
            this.vikiLink.Size = new System.Drawing.Size(274, 23);
            this.vikiLink.TabIndex = 3;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.ForeColor = System.Drawing.SystemColors.ActiveCaption;
            this.label1.Location = new System.Drawing.Point(7, 24);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(29, 15);
            this.label1.TabIndex = 4;
            this.label1.Text = "Link";
            // 
            // keyPair
            // 
            this.keyPair.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.keyPair.Location = new System.Drawing.Point(554, 129);
            this.keyPair.Name = "keyPair";
            this.keyPair.Size = new System.Drawing.Size(227, 23);
            this.keyPair.TabIndex = 5;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.ForeColor = System.Drawing.SystemColors.ActiveCaption;
            this.label2.Location = new System.Drawing.Point(500, 133);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(48, 15);
            this.label2.TabIndex = 6;
            this.label2.Text = "KID:KEY";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.label3.ForeColor = System.Drawing.SystemColors.ActiveCaption;
            this.label3.Location = new System.Drawing.Point(491, 24);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(55, 15);
            this.label3.TabIndex = 8;
            this.label3.Text = "Filename";
            // 
            // releaseName
            // 
            this.releaseName.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.releaseName.Location = new System.Drawing.Point(554, 20);
            this.releaseName.Name = "releaseName";
            this.releaseName.Size = new System.Drawing.Size(227, 23);
            this.releaseName.TabIndex = 7;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.label4.ForeColor = System.Drawing.SystemColors.GrayText;
            this.label4.Location = new System.Drawing.Point(571, 165);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(192, 21);
            this.label4.TabIndex = 10;
            this.label4.Text = "https://github.com/VarSell";
            this.label4.Click += new System.EventHandler(this.label4_Click);
            // 
            // headerMeta
            // 
            this.headerMeta.BackColor = System.Drawing.SystemColors.InactiveCaption;
            this.headerMeta.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.headerMeta.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.headerMeta.Location = new System.Drawing.Point(560, 200);
            this.headerMeta.Name = "headerMeta";
            this.headerMeta.Size = new System.Drawing.Size(221, 23);
            this.headerMeta.TabIndex = 9;
            this.headerMeta.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.ForeColor = System.Drawing.SystemColors.ActiveCaption;
            this.label8.Location = new System.Drawing.Point(505, 203);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(45, 15);
            this.label8.TabIndex = 17;
            this.label8.Text = "Header";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.ForeColor = System.Drawing.SystemColors.ActiveCaption;
            this.label5.Location = new System.Drawing.Point(525, 232);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(29, 15);
            this.label5.TabIndex = 19;
            this.label5.Text = "Year";
            // 
            // releaseYear
            // 
            this.releaseYear.BackColor = System.Drawing.SystemColors.InactiveCaption;
            this.releaseYear.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.releaseYear.Enabled = false;
            this.releaseYear.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.releaseYear.ForeColor = System.Drawing.SystemColors.InfoText;
            this.releaseYear.Location = new System.Drawing.Point(560, 229);
            this.releaseYear.Name = "releaseYear";
            this.releaseYear.Size = new System.Drawing.Size(82, 23);
            this.releaseYear.TabIndex = 18;
            this.releaseYear.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.ForeColor = System.Drawing.SystemColors.ActiveCaption;
            this.label6.Location = new System.Drawing.Point(497, 261);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(57, 15);
            this.label6.TabIndex = 21;
            this.label6.Text = "ISO 639-1";
            // 
            // languageISO
            // 
            this.languageISO.BackColor = System.Drawing.SystemColors.InactiveCaption;
            this.languageISO.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.languageISO.Enabled = false;
            this.languageISO.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.languageISO.Location = new System.Drawing.Point(560, 258);
            this.languageISO.Name = "languageISO";
            this.languageISO.Size = new System.Drawing.Size(82, 23);
            this.languageISO.TabIndex = 20;
            this.languageISO.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // button2
            // 
            this.button2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button2.ForeColor = System.Drawing.SystemColors.ActiveCaption;
            this.button2.Location = new System.Drawing.Point(12, 65);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 26);
            this.button2.TabIndex = 22;
            this.button2.Text = "Start";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.ForeColor = System.Drawing.SystemColors.ActiveCaption;
            this.label7.Location = new System.Drawing.Point(487, 57);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(63, 15);
            this.label7.TabIndex = 24;
            this.label7.Text = "Audio URL";
            // 
            // audioUrl
            // 
            this.audioUrl.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.audioUrl.Location = new System.Drawing.Point(554, 53);
            this.audioUrl.Name = "audioUrl";
            this.audioUrl.Size = new System.Drawing.Size(227, 23);
            this.audioUrl.TabIndex = 23;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.ForeColor = System.Drawing.SystemColors.ActiveCaption;
            this.label9.Location = new System.Drawing.Point(487, 95);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(61, 15);
            this.label9.TabIndex = 26;
            this.label9.Text = "Video URL";
            // 
            // videoUrl
            // 
            this.videoUrl.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.videoUrl.Location = new System.Drawing.Point(554, 91);
            this.videoUrl.Name = "videoUrl";
            this.videoUrl.Size = new System.Drawing.Size(227, 23);
            this.videoUrl.TabIndex = 25;
            // 
            // LogBox1
            // 
            this.LogBox1.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.LogBox1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.LogBox1.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.LogBox1.Location = new System.Drawing.Point(260, 107);
            this.LogBox1.Name = "LogBox1";
            this.LogBox1.Size = new System.Drawing.Size(191, 285);
            this.LogBox1.TabIndex = 28;
            this.LogBox1.Text = "";
            // 
            // themoviedb
            // 
            this.themoviedb.AutoSize = true;
            this.themoviedb.ForeColor = System.Drawing.SystemColors.ActiveCaption;
            this.themoviedb.Location = new System.Drawing.Point(515, 290);
            this.themoviedb.Name = "themoviedb";
            this.themoviedb.Size = new System.Drawing.Size(39, 15);
            this.themoviedb.TabIndex = 30;
            this.themoviedb.Text = "TMDB";
            // 
            // tmdb
            // 
            this.tmdb.BackColor = System.Drawing.SystemColors.InactiveCaption;
            this.tmdb.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tmdb.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.tmdb.Location = new System.Drawing.Point(560, 287);
            this.tmdb.Name = "tmdb";
            this.tmdb.Size = new System.Drawing.Size(82, 23);
            this.tmdb.TabIndex = 29;
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(7, 398);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.progressBar1.Size = new System.Drawing.Size(444, 23);
            this.progressBar1.TabIndex = 36;
            this.progressBar1.Tag = "";
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(44)))), ((int)(((byte)(44)))), ((int)(((byte)(44)))));
            this.panel1.Location = new System.Drawing.Point(0, 421);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(461, 11);
            this.panel1.TabIndex = 38;
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(44)))), ((int)(((byte)(44)))), ((int)(((byte)(44)))));
            this.panel2.Location = new System.Drawing.Point(0, 97);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(10, 334);
            this.panel2.TabIndex = 39;
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(44)))), ((int)(((byte)(44)))), ((int)(((byte)(44)))));
            this.panel3.Location = new System.Drawing.Point(451, 99);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(10, 326);
            this.panel3.TabIndex = 40;
            // 
            // panel4
            // 
            this.panel4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(44)))), ((int)(((byte)(44)))), ((int)(((byte)(44)))));
            this.panel4.Location = new System.Drawing.Point(254, 103);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(6, 300);
            this.panel4.TabIndex = 41;
            // 
            // panel5
            // 
            this.panel5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(44)))), ((int)(((byte)(44)))), ((int)(((byte)(44)))));
            this.panel5.Location = new System.Drawing.Point(10, 392);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(449, 11);
            this.panel5.TabIndex = 39;
            // 
            // panel6
            // 
            this.panel6.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(44)))), ((int)(((byte)(44)))), ((int)(((byte)(44)))));
            this.panel6.Location = new System.Drawing.Point(10, 97);
            this.panel6.Name = "panel6";
            this.panel6.Size = new System.Drawing.Size(451, 10);
            this.panel6.TabIndex = 39;
            // 
            // backDrop
            // 
            this.backDrop.Location = new System.Drawing.Point(649, 232);
            this.backDrop.Name = "backDrop";
            this.backDrop.Size = new System.Drawing.Size(132, 188);
            this.backDrop.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.backDrop.TabIndex = 42;
            this.backDrop.TabStop = false;
            // 
            // button1
            // 
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.button1.ForeColor = System.Drawing.Color.DodgerBlue;
            this.button1.Location = new System.Drawing.Point(181, 65);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(82, 26);
            this.button1.TabIndex = 1;
            this.button1.Text = "Downloads";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button3
            // 
            this.button3.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button3.ForeColor = System.Drawing.Color.DodgerBlue;
            this.button3.Location = new System.Drawing.Point(93, 65);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(82, 26);
            this.button3.TabIndex = 27;
            this.button3.Text = "Settings";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // button4
            // 
            this.button4.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.button4.ForeColor = System.Drawing.Color.DodgerBlue;
            this.button4.Location = new System.Drawing.Point(322, 20);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(82, 26);
            this.button4.TabIndex = 43;
            this.button4.Text = "Start (Beta)";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Visible = false;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(56)))), ((int)(((byte)(59)))));
            this.ClientSize = new System.Drawing.Size(793, 432);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.backDrop);
            this.Controls.Add(this.panel4);
            this.Controls.Add(this.panel6);
            this.Controls.Add(this.panel5);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.themoviedb);
            this.Controls.Add(this.tmdb);
            this.Controls.Add(this.LogBox1);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.videoUrl);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.audioUrl);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.languageISO);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.releaseYear);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.headerMeta);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.releaseName);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.keyPair);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.vikiLink);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.LogBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Viki";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.backDrop)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private RichTextBox LogBox;
        private TextBox vikiLink;
        private Label label1;
        private TextBox keyPair;
        private Label label2;
        private Label label3;
        private TextBox releaseName;
        private Label label4;
        private TextBox headerMeta;
        private Label label8;
        private Label label5;
        private TextBox releaseYear;
        private Label label6;
        private TextBox languageISO;
        private Button button2;
        private Label label7;
        private TextBox audioUrl;
        private Label label9;
        private TextBox videoUrl;
        private RichTextBox LogBox1;
        private Label themoviedb;
        private TextBox tmdb;
        private ProgressBar progressBar1;
        private Panel panel1;
        private Panel panel2;
        private Panel panel3;
        private Panel panel4;
        private Panel panel5;
        private Panel panel6;
        private PictureBox backDrop;
        private Button button1;
        private Button button3;
        public Button button4;
    }
}