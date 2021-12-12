namespace SmartUpdater
{
    partial class dlg_new_prog
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
            this.label1 = new System.Windows.Forms.Label();
            this.tb_uid = new System.Windows.Forms.TextBox();
            this.cb_visible = new System.Windows.Forms.CheckBox();
            this.cb_autoupdate = new System.Windows.Forms.CheckBox();
            this.label2 = new System.Windows.Forms.Label();
            this.tb_name = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.tb_description = new System.Windows.Forms.TextBox();
            this.tb_path_server = new System.Windows.Forms.TextBox();
            this.tb_exe = new System.Windows.Forms.TextBox();
            this.tb_install = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.cb_combine = new System.Windows.Forms.CheckBox();
            this.tb_icon_path = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 37);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(26, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "UID";
            // 
            // tb_uid
            // 
            this.tb_uid.Location = new System.Drawing.Point(80, 34);
            this.tb_uid.Name = "tb_uid";
            this.tb_uid.Size = new System.Drawing.Size(216, 20);
            this.tb_uid.TabIndex = 2;
            // 
            // cb_visible
            // 
            this.cb_visible.AutoSize = true;
            this.cb_visible.Checked = true;
            this.cb_visible.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cb_visible.Location = new System.Drawing.Point(15, 195);
            this.cb_visible.Name = "cb_visible";
            this.cb_visible.Size = new System.Drawing.Size(71, 17);
            this.cb_visible.TabIndex = 7;
            this.cb_visible.Text = "Видимая";
            this.cb_visible.UseVisualStyleBackColor = true;
            // 
            // cb_autoupdate
            // 
            this.cb_autoupdate.AutoSize = true;
            this.cb_autoupdate.Checked = true;
            this.cb_autoupdate.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cb_autoupdate.Location = new System.Drawing.Point(92, 195);
            this.cb_autoupdate.Name = "cb_autoupdate";
            this.cb_autoupdate.Size = new System.Drawing.Size(110, 17);
            this.cb_autoupdate.TabIndex = 8;
            this.cb_autoupdate.Text = "Автообновление";
            this.cb_autoupdate.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 9);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(57, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "Название";
            // 
            // tb_name
            // 
            this.tb_name.Location = new System.Drawing.Point(80, 6);
            this.tb_name.Name = "tb_name";
            this.tb_name.Size = new System.Drawing.Size(216, 20);
            this.tb_name.TabIndex = 1;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(11, 221);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(91, 13);
            this.label3.TabIndex = 7;
            this.label3.Text = "Путь на сервере";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(8, 248);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(94, 13);
            this.label4.TabIndex = 8;
            this.label4.Text = "Путь к exe файлу";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(8, 277);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(156, 13);
            this.label5.TabIndex = 9;
            this.label5.Text = "Папка установки без слешей";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(12, 70);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(57, 13);
            this.label6.TabIndex = 10;
            this.label6.Text = "Описание";
            // 
            // tb_description
            // 
            this.tb_description.AcceptsReturn = true;
            this.tb_description.AcceptsTab = true;
            this.tb_description.Location = new System.Drawing.Point(14, 88);
            this.tb_description.Multiline = true;
            this.tb_description.Name = "tb_description";
            this.tb_description.Size = new System.Drawing.Size(387, 86);
            this.tb_description.TabIndex = 3;
            // 
            // tb_path_server
            // 
            this.tb_path_server.Location = new System.Drawing.Point(120, 218);
            this.tb_path_server.Name = "tb_path_server";
            this.tb_path_server.Size = new System.Drawing.Size(281, 20);
            this.tb_path_server.TabIndex = 4;
            // 
            // tb_exe
            // 
            this.tb_exe.Location = new System.Drawing.Point(120, 248);
            this.tb_exe.Name = "tb_exe";
            this.tb_exe.Size = new System.Drawing.Size(281, 20);
            this.tb_exe.TabIndex = 5;
            // 
            // tb_install
            // 
            this.tb_install.Location = new System.Drawing.Point(170, 277);
            this.tb_install.Name = "tb_install";
            this.tb_install.Size = new System.Drawing.Size(231, 20);
            this.tb_install.TabIndex = 6;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(272, 338);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(129, 32);
            this.button1.TabIndex = 10;
            this.button1.Text = "Сохранить";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // cb_combine
            // 
            this.cb_combine.AutoSize = true;
            this.cb_combine.Checked = true;
            this.cb_combine.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cb_combine.Location = new System.Drawing.Point(14, 345);
            this.cb_combine.Name = "cb_combine";
            this.cb_combine.Size = new System.Drawing.Size(201, 17);
            this.cb_combine.TabIndex = 9;
            this.cb_combine.Text = "Совместить с конфигами сервера";
            this.cb_combine.UseVisualStyleBackColor = true;
            // 
            // tb_icon_path
            // 
            this.tb_icon_path.Location = new System.Drawing.Point(120, 303);
            this.tb_icon_path.Name = "tb_icon_path";
            this.tb_icon_path.Size = new System.Drawing.Size(281, 20);
            this.tb_icon_path.TabIndex = 11;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(8, 303);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(79, 13);
            this.label7.TabIndex = 12;
            this.label7.Text = "Путь к иконке";
            // 
            // dlg_new_prog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(434, 396);
            this.Controls.Add(this.tb_icon_path);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.cb_combine);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.tb_install);
            this.Controls.Add(this.tb_exe);
            this.Controls.Add(this.tb_path_server);
            this.Controls.Add(this.tb_description);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.tb_name);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.cb_autoupdate);
            this.Controls.Add(this.cb_visible);
            this.Controls.Add(this.tb_uid);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "dlg_new_prog";
            this.Text = "Создание конфига";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tb_uid;
        private System.Windows.Forms.CheckBox cb_visible;
        private System.Windows.Forms.CheckBox cb_autoupdate;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox tb_name;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox tb_description;
        private System.Windows.Forms.TextBox tb_path_server;
        private System.Windows.Forms.TextBox tb_exe;
        private System.Windows.Forms.TextBox tb_install;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.CheckBox cb_combine;
        private System.Windows.Forms.TextBox tb_icon_path;
        private System.Windows.Forms.Label label7;
    }
}