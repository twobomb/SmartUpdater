namespace SmartUpdater
{
    partial class Form1
    {
        /// <summary>
        /// Требуется переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Обязательный метод для поддержки конструктора - не изменяйте
        /// содержимое данного метода при помощи редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.командыToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.обновитьСписокПрограммToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.настройкиToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.подключениеToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.dev_menu = new System.Windows.Forms.ToolStripMenuItem();
            this.публикацияНовойВерсииНаСерверToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.показатьВсеПрограммыToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.созданиеКонфигаПрограммыToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btn_install = new System.Windows.Forms.Button();
            this.btn_update = new System.Windows.Forms.Button();
            this.btn_delete = new System.Windows.Forms.Button();
            this.lbl_installed = new System.Windows.Forms.Label();
            this.tb_path = new System.Windows.Forms.TextBox();
            this.btn_open_dir = new System.Windows.Forms.Button();
            this.lbl_cur_ver = new System.Windows.Forms.Label();
            this.btn_restore = new System.Windows.Forms.Button();
            this.btn_check_files = new System.Windows.Forms.Button();
            this.btn_select_version = new System.Windows.Forms.Button();
            this.btn_start = new System.Windows.Forms.Button();
            this.lbl_name = new System.Windows.Forms.Label();
            this.lbl_update_info = new System.Windows.Forms.Label();
            this.включитьМенюРазработчикаToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.командыToolStripMenuItem,
            this.настройкиToolStripMenuItem,
            this.dev_menu});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(732, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // командыToolStripMenuItem
            // 
            this.командыToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.обновитьСписокПрограммToolStripMenuItem});
            this.командыToolStripMenuItem.Name = "командыToolStripMenuItem";
            this.командыToolStripMenuItem.Size = new System.Drawing.Size(70, 20);
            this.командыToolStripMenuItem.Text = "Команды";
            // 
            // обновитьСписокПрограммToolStripMenuItem
            // 
            this.обновитьСписокПрограммToolStripMenuItem.Name = "обновитьСписокПрограммToolStripMenuItem";
            this.обновитьСписокПрограммToolStripMenuItem.Size = new System.Drawing.Size(230, 22);
            this.обновитьСписокПрограммToolStripMenuItem.Text = "Обновить список программ";
            this.обновитьСписокПрограммToolStripMenuItem.Click += new System.EventHandler(this.обновитьСписокПрограммToolStripMenuItem_Click);
            // 
            // настройкиToolStripMenuItem
            // 
            this.настройкиToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.подключениеToolStripMenuItem,
            this.включитьМенюРазработчикаToolStripMenuItem});
            this.настройкиToolStripMenuItem.Name = "настройкиToolStripMenuItem";
            this.настройкиToolStripMenuItem.Size = new System.Drawing.Size(79, 20);
            this.настройкиToolStripMenuItem.Text = "Настройки";
            // 
            // подключениеToolStripMenuItem
            // 
            this.подключениеToolStripMenuItem.Name = "подключениеToolStripMenuItem";
            this.подключениеToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.подключениеToolStripMenuItem.Text = "Подключение";
            this.подключениеToolStripMenuItem.Click += new System.EventHandler(this.подключениеToolStripMenuItem_Click);
            // 
            // dev_menu
            // 
            this.dev_menu.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.публикацияНовойВерсииНаСерверToolStripMenuItem,
            this.показатьВсеПрограммыToolStripMenuItem,
            this.созданиеКонфигаПрограммыToolStripMenuItem});
            this.dev_menu.Name = "dev_menu";
            this.dev_menu.Size = new System.Drawing.Size(132, 20);
            this.dev_menu.Text = "Меню разработчика";
            // 
            // публикацияНовойВерсииНаСерверToolStripMenuItem
            // 
            this.публикацияНовойВерсииНаСерверToolStripMenuItem.Name = "публикацияНовойВерсииНаСерверToolStripMenuItem";
            this.публикацияНовойВерсииНаСерверToolStripMenuItem.Size = new System.Drawing.Size(245, 22);
            this.публикацияНовойВерсииНаСерверToolStripMenuItem.Text = "Публикация новой версии";
            this.публикацияНовойВерсииНаСерверToolStripMenuItem.Click += new System.EventHandler(this.публикацияНовойВерсииНаСерверToolStripMenuItem_Click);
            // 
            // показатьВсеПрограммыToolStripMenuItem
            // 
            this.показатьВсеПрограммыToolStripMenuItem.Name = "показатьВсеПрограммыToolStripMenuItem";
            this.показатьВсеПрограммыToolStripMenuItem.Size = new System.Drawing.Size(245, 22);
            this.показатьВсеПрограммыToolStripMenuItem.Text = "Показать скрытые программы";
            this.показатьВсеПрограммыToolStripMenuItem.Click += new System.EventHandler(this.показатьВсеПрограммыToolStripMenuItem_Click);
            // 
            // созданиеКонфигаПрограммыToolStripMenuItem
            // 
            this.созданиеКонфигаПрограммыToolStripMenuItem.Name = "созданиеКонфигаПрограммыToolStripMenuItem";
            this.созданиеКонфигаПрограммыToolStripMenuItem.Size = new System.Drawing.Size(245, 22);
            this.созданиеКонфигаПрограммыToolStripMenuItem.Text = "Создание конфига программы";
            this.созданиеКонфигаПрограммыToolStripMenuItem.Click += new System.EventHandler(this.созданиеКонфигаПрограммыToolStripMenuItem_Click);
            // 
            // listBox1
            // 
            this.listBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.listBox1.FormattingEnabled = true;
            this.listBox1.ItemHeight = 20;
            this.listBox1.Location = new System.Drawing.Point(12, 47);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(251, 284);
            this.listBox1.TabIndex = 1;
            this.listBox1.SelectedIndexChanged += new System.EventHandler(this.listBox1_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 31);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(116, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Выберите программу";
            // 
            // btn_install
            // 
            this.btn_install.Location = new System.Drawing.Point(399, 47);
            this.btn_install.Name = "btn_install";
            this.btn_install.Size = new System.Drawing.Size(103, 24);
            this.btn_install.TabIndex = 3;
            this.btn_install.Text = "Установить";
            this.btn_install.UseVisualStyleBackColor = true;
            this.btn_install.Click += new System.EventHandler(this.btn_install_Click);
            // 
            // btn_update
            // 
            this.btn_update.Location = new System.Drawing.Point(508, 47);
            this.btn_update.Name = "btn_update";
            this.btn_update.Size = new System.Drawing.Size(103, 24);
            this.btn_update.TabIndex = 4;
            this.btn_update.Text = "Обновить";
            this.btn_update.UseVisualStyleBackColor = true;
            this.btn_update.Click += new System.EventHandler(this.btn_update_Click);
            // 
            // btn_delete
            // 
            this.btn_delete.Location = new System.Drawing.Point(617, 47);
            this.btn_delete.Name = "btn_delete";
            this.btn_delete.Size = new System.Drawing.Size(103, 24);
            this.btn_delete.TabIndex = 5;
            this.btn_delete.Text = "Удалить";
            this.btn_delete.UseVisualStyleBackColor = true;
            this.btn_delete.Click += new System.EventHandler(this.btn_delete_Click);
            // 
            // lbl_installed
            // 
            this.lbl_installed.AutoSize = true;
            this.lbl_installed.Location = new System.Drawing.Point(290, 143);
            this.lbl_installed.Name = "lbl_installed";
            this.lbl_installed.Size = new System.Drawing.Size(133, 13);
            this.lbl_installed.TabIndex = 6;
            this.lbl_installed.Text = "Программа установлена";
            // 
            // tb_path
            // 
            this.tb_path.Location = new System.Drawing.Point(293, 159);
            this.tb_path.Name = "tb_path";
            this.tb_path.ReadOnly = true;
            this.tb_path.Size = new System.Drawing.Size(278, 20);
            this.tb_path.TabIndex = 7;
            // 
            // btn_open_dir
            // 
            this.btn_open_dir.Location = new System.Drawing.Point(577, 155);
            this.btn_open_dir.Name = "btn_open_dir";
            this.btn_open_dir.Size = new System.Drawing.Size(103, 24);
            this.btn_open_dir.TabIndex = 8;
            this.btn_open_dir.Text = "Открыть папку";
            this.btn_open_dir.UseVisualStyleBackColor = true;
            this.btn_open_dir.Click += new System.EventHandler(this.btn_open_dir_Click);
            // 
            // lbl_cur_ver
            // 
            this.lbl_cur_ver.AutoSize = true;
            this.lbl_cur_ver.Location = new System.Drawing.Point(287, 101);
            this.lbl_cur_ver.Name = "lbl_cur_ver";
            this.lbl_cur_ver.Size = new System.Drawing.Size(153, 13);
            this.lbl_cur_ver.TabIndex = 9;
            this.lbl_cur_ver.Text = "Текущая версия программы";
            // 
            // btn_restore
            // 
            this.btn_restore.Location = new System.Drawing.Point(293, 214);
            this.btn_restore.Name = "btn_restore";
            this.btn_restore.Size = new System.Drawing.Size(210, 23);
            this.btn_restore.TabIndex = 10;
            this.btn_restore.Text = "Восстановить целостность файлов";
            this.btn_restore.UseVisualStyleBackColor = true;
            this.btn_restore.Click += new System.EventHandler(this.btn_restore_Click);
            // 
            // btn_check_files
            // 
            this.btn_check_files.Location = new System.Drawing.Point(293, 185);
            this.btn_check_files.Name = "btn_check_files";
            this.btn_check_files.Size = new System.Drawing.Size(210, 23);
            this.btn_check_files.TabIndex = 11;
            this.btn_check_files.Text = "Проверить целостность файлов";
            this.btn_check_files.UseVisualStyleBackColor = true;
            this.btn_check_files.Click += new System.EventHandler(this.btn_check_files_Click);
            // 
            // btn_select_version
            // 
            this.btn_select_version.Location = new System.Drawing.Point(293, 243);
            this.btn_select_version.Name = "btn_select_version";
            this.btn_select_version.Size = new System.Drawing.Size(210, 23);
            this.btn_select_version.TabIndex = 12;
            this.btn_select_version.Text = "Обновить до выбранной версии";
            this.btn_select_version.UseVisualStyleBackColor = true;
            this.btn_select_version.Click += new System.EventHandler(this.btn_select_version_Click);
            // 
            // btn_start
            // 
            this.btn_start.Location = new System.Drawing.Point(290, 47);
            this.btn_start.Name = "btn_start";
            this.btn_start.Size = new System.Drawing.Size(103, 24);
            this.btn_start.TabIndex = 13;
            this.btn_start.Text = "Запуск";
            this.btn_start.UseVisualStyleBackColor = true;
            this.btn_start.Click += new System.EventHandler(this.btn_start_Click);
            // 
            // lbl_name
            // 
            this.lbl_name.AutoSize = true;
            this.lbl_name.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lbl_name.Location = new System.Drawing.Point(286, 74);
            this.lbl_name.Name = "lbl_name";
            this.lbl_name.Size = new System.Drawing.Size(327, 20);
            this.lbl_name.TabIndex = 14;
            this.lbl_name.Text = "Название программы выбранной в списке";
            // 
            // lbl_update_info
            // 
            this.lbl_update_info.AutoSize = true;
            this.lbl_update_info.Location = new System.Drawing.Point(288, 120);
            this.lbl_update_info.Name = "lbl_update_info";
            this.lbl_update_info.Size = new System.Drawing.Size(43, 13);
            this.lbl_update_info.TabIndex = 15;
            this.lbl_update_info.Text = "nedupd";
            // 
            // включитьМенюРазработчикаToolStripMenuItem
            // 
            this.включитьМенюРазработчикаToolStripMenuItem.Name = "включитьМенюРазработчикаToolStripMenuItem";
            this.включитьМенюРазработчикаToolStripMenuItem.Size = new System.Drawing.Size(243, 22);
            this.включитьМенюРазработчикаToolStripMenuItem.Text = "Включить меню разработчика";
            this.включитьМенюРазработчикаToolStripMenuItem.Click += new System.EventHandler(this.включитьМенюРазработчикаToolStripMenuItem_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(732, 346);
            this.Controls.Add(this.lbl_update_info);
            this.Controls.Add(this.lbl_name);
            this.Controls.Add(this.btn_start);
            this.Controls.Add(this.btn_select_version);
            this.Controls.Add(this.btn_check_files);
            this.Controls.Add(this.btn_restore);
            this.Controls.Add(this.lbl_cur_ver);
            this.Controls.Add(this.btn_open_dir);
            this.Controls.Add(this.tb_path);
            this.Controls.Add(this.lbl_installed);
            this.Controls.Add(this.btn_delete);
            this.Controls.Add(this.btn_update);
            this.Controls.Add(this.btn_install);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.listBox1);
            this.Controls.Add(this.menuStrip1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MainMenuStrip = this.menuStrip1;
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.Text = "SmartUpdater";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem настройкиToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem подключениеToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem dev_menu;
        private System.Windows.Forms.ToolStripMenuItem публикацияНовойВерсииНаСерверToolStripMenuItem;
        private System.Windows.Forms.ListBox listBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btn_install;
        private System.Windows.Forms.Button btn_update;
        private System.Windows.Forms.Button btn_delete;
        private System.Windows.Forms.Label lbl_installed;
        private System.Windows.Forms.TextBox tb_path;
        private System.Windows.Forms.Button btn_open_dir;
        private System.Windows.Forms.Label lbl_cur_ver;
        private System.Windows.Forms.Button btn_restore;
        private System.Windows.Forms.Button btn_check_files;
        private System.Windows.Forms.Button btn_select_version;
        private System.Windows.Forms.Button btn_start;
        private System.Windows.Forms.Label lbl_name;
        private System.Windows.Forms.ToolStripMenuItem командыToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem обновитьСписокПрограммToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem показатьВсеПрограммыToolStripMenuItem;
        private System.Windows.Forms.Label lbl_update_info;
        private System.Windows.Forms.ToolStripMenuItem созданиеКонфигаПрограммыToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem включитьМенюРазработчикаToolStripMenuItem;
    }
}

