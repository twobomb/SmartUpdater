namespace SmartUpdater
{
    partial class dlg_install
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
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.rb_current = new System.Windows.Forms.RadioButton();
            this.rb_all = new System.Windows.Forms.RadioButton();
            this.cb_desk = new System.Windows.Forms.CheckBox();
            this.button2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.cb_autostart = new System.Windows.Forms.CheckBox();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(399, 36);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(80, 23);
            this.button1.TabIndex = 0;
            this.button1.Text = "Обзор";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // tb_path
            // 
            this.tb_path.Location = new System.Drawing.Point(12, 38);
            this.tb_path.Name = "tb_path";
            this.tb_path.Size = new System.Drawing.Size(381, 20);
            this.tb_path.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 22);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(86, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Путь установки";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.rb_current);
            this.groupBox1.Controls.Add(this.rb_all);
            this.groupBox1.Location = new System.Drawing.Point(12, 64);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(199, 47);
            this.groupBox1.TabIndex = 3;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Установить для пользователя";
            // 
            // rb_current
            // 
            this.rb_current.AutoSize = true;
            this.rb_current.Location = new System.Drawing.Point(84, 19);
            this.rb_current.Name = "rb_current";
            this.rb_current.Size = new System.Drawing.Size(97, 17);
            this.rb_current.TabIndex = 1;
            this.rb_current.Text = "Для текущего";
            this.rb_current.UseVisualStyleBackColor = true;
            this.rb_current.CheckedChanged += new System.EventHandler(this.rb_current_CheckedChanged);
            // 
            // rb_all
            // 
            this.rb_all.AutoSize = true;
            this.rb_all.Checked = true;
            this.rb_all.Location = new System.Drawing.Point(6, 19);
            this.rb_all.Name = "rb_all";
            this.rb_all.Size = new System.Drawing.Size(72, 17);
            this.rb_all.TabIndex = 0;
            this.rb_all.TabStop = true;
            this.rb_all.Text = "Для всех";
            this.rb_all.UseVisualStyleBackColor = true;
            this.rb_all.CheckedChanged += new System.EventHandler(this.rb_all_CheckedChanged);
            // 
            // cb_desk
            // 
            this.cb_desk.AutoSize = true;
            this.cb_desk.Checked = true;
            this.cb_desk.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cb_desk.Location = new System.Drawing.Point(12, 133);
            this.cb_desk.Name = "cb_desk";
            this.cb_desk.Size = new System.Drawing.Size(196, 17);
            this.cb_desk.TabIndex = 4;
            this.cb_desk.Text = "Создать ярлык на рабочем столе";
            this.cb_desk.UseVisualStyleBackColor = true;
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(214, 114);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(153, 36);
            this.button2.TabIndex = 5;
            this.button2.Text = "Установить";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(376, 114);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(118, 36);
            this.button3.TabIndex = 6;
            this.button3.Text = "Отмена";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // cb_autostart
            // 
            this.cb_autostart.AutoSize = true;
            this.cb_autostart.Checked = true;
            this.cb_autostart.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cb_autostart.Location = new System.Drawing.Point(12, 114);
            this.cb_autostart.Name = "cb_autostart";
            this.cb_autostart.Size = new System.Drawing.Size(190, 17);
            this.cb_autostart.TabIndex = 7;
            this.cb_autostart.Text = "Автостарт при запуске Windows";
            this.cb_autostart.UseVisualStyleBackColor = true;
            // 
            // dlg_install
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(506, 162);
            this.Controls.Add(this.cb_autostart);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.cb_desk);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.tb_path);
            this.Controls.Add(this.button1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "dlg_install";
            this.Text = "Установка";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox tb_path;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton rb_current;
        private System.Windows.Forms.RadioButton rb_all;
        private System.Windows.Forms.CheckBox cb_desk;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.CheckBox cb_autostart;
    }
}