namespace SmartUpdater
{
    partial class dlg_deploy
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
            this.button1 = new System.Windows.Forms.Button();
            this.tb_path = new System.Windows.Forms.TextBox();
            this.treeView1 = new System.Windows.Forms.TreeView();
            this.cb_ignore = new System.Windows.Forms.CheckBox();
            this.gb_file_config = new System.Windows.Forms.GroupBox();
            this.lbl_full_path = new System.Windows.Forms.Label();
            this.tb_hash = new System.Windows.Forms.TextBox();
            this.lbl_size = new System.Windows.Forms.Label();
            this.bb = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.button3 = new System.Windows.Forms.Button();
            this.cb_clearall = new System.Windows.Forms.CheckBox();
            this.tb_ver = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.button2 = new System.Windows.Forms.Button();
            this.lbl_full_size = new System.Windows.Forms.Label();
            this.tb_min_upd = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.tb_dir_build = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.cb_onlychange = new System.Windows.Forms.CheckBox();
            this.cb_req = new System.Windows.Forms.CheckBox();
            this.tb_changelist = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.gb_file_config.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(412, 23);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(185, 23);
            this.button1.TabIndex = 0;
            this.button1.Text = "Выбрать папку с программой";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // tb_path
            // 
            this.tb_path.Location = new System.Drawing.Point(12, 25);
            this.tb_path.Name = "tb_path";
            this.tb_path.ReadOnly = true;
            this.tb_path.Size = new System.Drawing.Size(394, 20);
            this.tb_path.TabIndex = 1;
            // 
            // treeView1
            // 
            this.treeView1.Location = new System.Drawing.Point(12, 51);
            this.treeView1.Name = "treeView1";
            this.treeView1.Size = new System.Drawing.Size(297, 462);
            this.treeView1.TabIndex = 2;
            this.treeView1.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeView1_AfterSelect);
            // 
            // cb_ignore
            // 
            this.cb_ignore.AutoSize = true;
            this.cb_ignore.Location = new System.Drawing.Point(9, 21);
            this.cb_ignore.Name = "cb_ignore";
            this.cb_ignore.Size = new System.Drawing.Size(98, 17);
            this.cb_ignore.TabIndex = 4;
            this.cb_ignore.Text = "Игнорировать";
            this.cb_ignore.UseVisualStyleBackColor = true;
            this.cb_ignore.CheckedChanged += new System.EventHandler(this.cb_ignore_CheckedChanged);
            // 
            // gb_file_config
            // 
            this.gb_file_config.Controls.Add(this.lbl_full_path);
            this.gb_file_config.Controls.Add(this.tb_hash);
            this.gb_file_config.Controls.Add(this.lbl_size);
            this.gb_file_config.Controls.Add(this.bb);
            this.gb_file_config.Controls.Add(this.cb_ignore);
            this.gb_file_config.Location = new System.Drawing.Point(315, 52);
            this.gb_file_config.Name = "gb_file_config";
            this.gb_file_config.Size = new System.Drawing.Size(294, 114);
            this.gb_file_config.TabIndex = 5;
            this.gb_file_config.TabStop = false;
            this.gb_file_config.Text = "Настройка выбранного файла";
            // 
            // lbl_full_path
            // 
            this.lbl_full_path.AutoSize = true;
            this.lbl_full_path.Location = new System.Drawing.Point(6, 85);
            this.lbl_full_path.Name = "lbl_full_path";
            this.lbl_full_path.Size = new System.Drawing.Size(34, 13);
            this.lbl_full_path.TabIndex = 9;
            this.lbl_full_path.Text = "Путь:";
            // 
            // tb_hash
            // 
            this.tb_hash.Location = new System.Drawing.Point(35, 38);
            this.tb_hash.Name = "tb_hash";
            this.tb_hash.ReadOnly = true;
            this.tb_hash.Size = new System.Drawing.Size(253, 20);
            this.tb_hash.TabIndex = 8;
            // 
            // lbl_size
            // 
            this.lbl_size.AutoSize = true;
            this.lbl_size.Location = new System.Drawing.Point(4, 60);
            this.lbl_size.Name = "lbl_size";
            this.lbl_size.Size = new System.Drawing.Size(55, 13);
            this.lbl_size.TabIndex = 6;
            this.lbl_size.Text = "Размер: -";
            // 
            // bb
            // 
            this.bb.AutoSize = true;
            this.bb.Location = new System.Drawing.Point(6, 41);
            this.bb.Name = "bb";
            this.bb.Size = new System.Drawing.Size(34, 13);
            this.bb.TabIndex = 5;
            this.bb.Text = "Хеш: ";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.button3);
            this.groupBox2.Controls.Add(this.cb_clearall);
            this.groupBox2.Controls.Add(this.tb_ver);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.button2);
            this.groupBox2.Controls.Add(this.lbl_full_size);
            this.groupBox2.Controls.Add(this.tb_min_upd);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.comboBox1);
            this.groupBox2.Controls.Add(this.tb_dir_build);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Controls.Add(this.cb_onlychange);
            this.groupBox2.Controls.Add(this.cb_req);
            this.groupBox2.Location = new System.Drawing.Point(315, 170);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(294, 343);
            this.groupBox2.TabIndex = 5;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Настройки билда";
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(5, 298);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(279, 29);
            this.button3.TabIndex = 11;
            this.button3.Text = "Публикация в папку";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click_1);
            // 
            // cb_clearall
            // 
            this.cb_clearall.AutoSize = true;
            this.cb_clearall.Location = new System.Drawing.Point(5, 107);
            this.cb_clearall.Name = "cb_clearall";
            this.cb_clearall.Size = new System.Drawing.Size(207, 17);
            this.cb_clearall.TabIndex = 10;
            this.cb_clearall.Text = "Очищать папку перед обновлением";
            this.cb_clearall.UseVisualStyleBackColor = true;
            // 
            // tb_ver
            // 
            this.tb_ver.Location = new System.Drawing.Point(7, 223);
            this.tb_ver.Name = "tb_ver";
            this.tb_ver.ReadOnly = true;
            this.tb_ver.Size = new System.Drawing.Size(84, 20);
            this.tb_ver.TabIndex = 9;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(2, 207);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(106, 13);
            this.label4.TabIndex = 8;
            this.label4.Text = "Версия публикации";
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(6, 263);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(279, 29);
            this.button2.TabIndex = 6;
            this.button2.Text = "Публикация на сервер";
            this.button2.UseVisualStyleBackColor = true;
            // 
            // lbl_full_size
            // 
            this.lbl_full_size.AutoSize = true;
            this.lbl_full_size.Location = new System.Drawing.Point(4, 246);
            this.lbl_full_size.Name = "lbl_full_size";
            this.lbl_full_size.Size = new System.Drawing.Size(92, 13);
            this.lbl_full_size.TabIndex = 7;
            this.lbl_full_size.Text = "Общий размер: -";
            // 
            // tb_min_upd
            // 
            this.tb_min_upd.Location = new System.Drawing.Point(5, 178);
            this.tb_min_upd.Name = "tb_min_upd";
            this.tb_min_upd.Size = new System.Drawing.Size(84, 20);
            this.tb_min_upd.TabIndex = 7;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(2, 162);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(266, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "Минимальная версия SmartUpdate для обновления";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 16);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(66, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "Программа";
            // 
            // comboBox1
            // 
            this.comboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(7, 35);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(253, 21);
            this.comboBox1.TabIndex = 4;
            this.comboBox1.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
            // 
            // tb_dir_build
            // 
            this.tb_dir_build.Location = new System.Drawing.Point(7, 141);
            this.tb_dir_build.Name = "tb_dir_build";
            this.tb_dir_build.Size = new System.Drawing.Size(231, 20);
            this.tb_dir_build.TabIndex = 3;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(4, 125);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(149, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Папка с билдом на сервере";
            // 
            // cb_onlychange
            // 
            this.cb_onlychange.AutoSize = true;
            this.cb_onlychange.Checked = true;
            this.cb_onlychange.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cb_onlychange.Location = new System.Drawing.Point(6, 84);
            this.cb_onlychange.Name = "cb_onlychange";
            this.cb_onlychange.Size = new System.Drawing.Size(223, 17);
            this.cb_onlychange.TabIndex = 1;
            this.cb_onlychange.Text = "Обновлять только измененные файлы";
            this.cb_onlychange.UseVisualStyleBackColor = true;
            // 
            // cb_req
            // 
            this.cb_req.AutoSize = true;
            this.cb_req.Checked = true;
            this.cb_req.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cb_req.Location = new System.Drawing.Point(6, 61);
            this.cb_req.Name = "cb_req";
            this.cb_req.Size = new System.Drawing.Size(162, 17);
            this.cb_req.TabIndex = 0;
            this.cb_req.Text = "Обязательное обновление";
            this.cb_req.UseVisualStyleBackColor = true;
            // 
            // tb_changelist
            // 
            this.tb_changelist.AcceptsReturn = true;
            this.tb_changelist.AcceptsTab = true;
            this.tb_changelist.Location = new System.Drawing.Point(627, 71);
            this.tb_changelist.Multiline = true;
            this.tb_changelist.Name = "tb_changelist";
            this.tb_changelist.Size = new System.Drawing.Size(448, 410);
            this.tb_changelist.TabIndex = 6;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(627, 52);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(103, 13);
            this.label8.TabIndex = 7;
            this.label8.Text = "Список изменений";
            // 
            // dlg_deploy
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1105, 525);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.tb_changelist);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.gb_file_config);
            this.Controls.Add(this.treeView1);
            this.Controls.Add(this.tb_path);
            this.Controls.Add(this.button1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "dlg_deploy";
            this.Text = "Сброщик";
            this.gb_file_config.ResumeLayout(false);
            this.gb_file_config.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox tb_path;
        private System.Windows.Forms.TreeView treeView1;
        private System.Windows.Forms.CheckBox cb_ignore;
        private System.Windows.Forms.GroupBox gb_file_config;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.CheckBox cb_req;
        private System.Windows.Forms.CheckBox cb_onlychange;
        private System.Windows.Forms.TextBox tb_dir_build;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.TextBox tb_min_upd;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label lbl_size;
        private System.Windows.Forms.Label bb;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Label lbl_full_size;
        private System.Windows.Forms.TextBox tb_changelist;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox tb_ver;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox tb_hash;
        private System.Windows.Forms.Label lbl_full_path;
        private System.Windows.Forms.CheckBox cb_clearall;
        private System.Windows.Forms.Button button3;
    }
}