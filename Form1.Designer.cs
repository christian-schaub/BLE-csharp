namespace MultipleServCharac
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
            this.components = new System.ComponentModel.Container();
            this.txtLog = new System.Windows.Forms.TextBox();
            this.comboPorts = new System.Windows.Forms.ComboBox();
            this.serialAPI = new System.IO.Ports.SerialPort(this.components);
            this.Characteristic2 = new System.Windows.Forms.Panel();
            this.sensor_moving_value_label = new System.Windows.Forms.Label();
            this.sensor_moving_raw_label = new System.Windows.Forms.Label();
            this.raw_val_label1 = new System.Windows.Forms.Label();
            this.moveDectLabel = new System.Windows.Forms.Label();
            this.sensorLabel1 = new System.Windows.Forms.Label();
            this.Characteristic1 = new System.Windows.Forms.Panel();
            this.sensor_distance_value_label = new System.Windows.Forms.Label();
            this.sensor_distance_raw_label = new System.Windows.Forms.Label();
            this.distanceCmLabel = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.distanceLabel = new System.Windows.Forms.Label();
            this.PlattformService = new System.Windows.Forms.GroupBox();
            this.activityLabel = new System.Windows.Forms.Label();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.btnAttach = new System.Windows.Forms.Button();
            this.btnReset = new System.Windows.Forms.Button();
            this.btnGo = new System.Windows.Forms.Button();
            this.lblPorts = new System.Windows.Forms.Label();
            this.separatorTop = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.attributList = new System.Windows.Forms.ListBox();
            this.readButton = new System.Windows.Forms.Button();
            this.bitsCheckBox = new System.Windows.Forms.CheckBox();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.Characteristic2.SuspendLayout();
            this.Characteristic1.SuspendLayout();
            this.PlattformService.SuspendLayout();
            this.SuspendLayout();
            // 
            // txtLog
            // 
            this.txtLog.BackColor = System.Drawing.SystemColors.Window;
            this.txtLog.Location = new System.Drawing.Point(12, 312);
            this.txtLog.Multiline = true;
            this.txtLog.Name = "txtLog";
            this.txtLog.ReadOnly = true;
            this.txtLog.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtLog.Size = new System.Drawing.Size(541, 238);
            this.txtLog.TabIndex = 3;
            // 
            // comboPorts
            // 
            this.comboPorts.DropDownHeight = 200;
            this.comboPorts.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboPorts.IntegralHeight = false;
            this.comboPorts.ItemHeight = 14;
            this.comboPorts.Items.AddRange(new object[] {
            "Refresh"});
            this.comboPorts.Location = new System.Drawing.Point(64, 14);
            this.comboPorts.Name = "comboPorts";
            this.comboPorts.Size = new System.Drawing.Size(155, 22);
            this.comboPorts.TabIndex = 6;
            this.comboPorts.SelectedIndexChanged += new System.EventHandler(this.comboPorts_SelectedIndexChanged);
            // 
            // Characteristic2
            // 
            this.Characteristic2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Characteristic2.Controls.Add(this.sensor_moving_value_label);
            this.Characteristic2.Controls.Add(this.sensor_moving_raw_label);
            this.Characteristic2.Controls.Add(this.raw_val_label1);
            this.Characteristic2.Controls.Add(this.moveDectLabel);
            this.Characteristic2.Location = new System.Drawing.Point(35, 99);
            this.Characteristic2.Name = "Characteristic2";
            this.Characteristic2.Size = new System.Drawing.Size(431, 30);
            this.Characteristic2.TabIndex = 0;
            // 
            // sensor_moving_value_label
            // 
            this.sensor_moving_value_label.AutoSize = true;
            this.sensor_moving_value_label.Location = new System.Drawing.Point(323, 7);
            this.sensor_moving_value_label.Name = "sensor_moving_value_label";
            this.sensor_moving_value_label.Size = new System.Drawing.Size(65, 14);
            this.sensor_moving_value_label.TabIndex = 6;
            this.sensor_moving_value_label.Text = "not moved";
            this.sensor_moving_value_label.Click += new System.EventHandler(this.label10_Click);
            // 
            // sensor_moving_raw_label
            // 
            this.sensor_moving_raw_label.AutoSize = true;
            this.sensor_moving_raw_label.Location = new System.Drawing.Point(254, 7);
            this.sensor_moving_raw_label.Name = "sensor_moving_raw_label";
            this.sensor_moving_raw_label.Size = new System.Drawing.Size(19, 14);
            this.sensor_moving_raw_label.TabIndex = 5;
            this.sensor_moving_raw_label.Text = "----";
            // 
            // raw_val_label1
            // 
            this.raw_val_label1.AutoSize = true;
            this.raw_val_label1.Location = new System.Drawing.Point(185, 7);
            this.raw_val_label1.Name = "raw_val_label1";
            this.raw_val_label1.Size = new System.Drawing.Size(63, 14);
            this.raw_val_label1.TabIndex = 3;
            this.raw_val_label1.Text = "RAW Value";
            // 
            // moveDectLabel
            // 
            this.moveDectLabel.AutoSize = true;
            this.moveDectLabel.Location = new System.Drawing.Point(14, 7);
            this.moveDectLabel.Name = "moveDectLabel";
            this.moveDectLabel.Size = new System.Drawing.Size(104, 14);
            this.moveDectLabel.TabIndex = 2;
            this.moveDectLabel.Text = "Moving Detection";
            // 
            // sensorLabel1
            // 
            this.sensorLabel1.AutoSize = true;
            this.sensorLabel1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.sensorLabel1.Location = new System.Drawing.Point(32, 28);
            this.sensorLabel1.Name = "sensorLabel1";
            this.sensorLabel1.Size = new System.Drawing.Size(46, 13);
            this.sensorLabel1.TabIndex = 0;
            this.sensorLabel1.Text = "Sensor";
            this.sensorLabel1.Click += new System.EventHandler(this.sensorLabel1_Click);
            // 
            // Characteristic1
            // 
            this.Characteristic1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Characteristic1.Controls.Add(this.sensor_distance_value_label);
            this.Characteristic1.Controls.Add(this.sensor_distance_raw_label);
            this.Characteristic1.Controls.Add(this.distanceCmLabel);
            this.Characteristic1.Controls.Add(this.label1);
            this.Characteristic1.Controls.Add(this.distanceLabel);
            this.Characteristic1.Location = new System.Drawing.Point(35, 56);
            this.Characteristic1.Name = "Characteristic1";
            this.Characteristic1.Size = new System.Drawing.Size(431, 28);
            this.Characteristic1.TabIndex = 1;
            this.Characteristic1.Paint += new System.Windows.Forms.PaintEventHandler(this.Characteristic1_Paint);
            // 
            // sensor_distance_value_label
            // 
            this.sensor_distance_value_label.AutoSize = true;
            this.sensor_distance_value_label.Location = new System.Drawing.Point(392, 4);
            this.sensor_distance_value_label.Name = "sensor_distance_value_label";
            this.sensor_distance_value_label.Size = new System.Drawing.Size(38, 14);
            this.sensor_distance_value_label.TabIndex = 5;
            this.sensor_distance_value_label.Text = "---- cm";
            // 
            // sensor_distance_raw_label
            // 
            this.sensor_distance_raw_label.AutoSize = true;
            this.sensor_distance_raw_label.Location = new System.Drawing.Point(254, 4);
            this.sensor_distance_raw_label.Name = "sensor_distance_raw_label";
            this.sensor_distance_raw_label.Size = new System.Drawing.Size(19, 14);
            this.sensor_distance_raw_label.TabIndex = 4;
            this.sensor_distance_raw_label.Text = "----";
            this.sensor_distance_raw_label.Click += new System.EventHandler(this.label4_Click);
            // 
            // distanceCmLabel
            // 
            this.distanceCmLabel.AutoSize = true;
            this.distanceCmLabel.Location = new System.Drawing.Point(323, 4);
            this.distanceCmLabel.Name = "distanceCmLabel";
            this.distanceCmLabel.Size = new System.Drawing.Size(69, 14);
            this.distanceCmLabel.TabIndex = 3;
            this.distanceCmLabel.Text = "Value in cm";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(185, 4);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(63, 14);
            this.label1.TabIndex = 2;
            this.label1.Text = "RAW Value";
            this.label1.Click += new System.EventHandler(this.label1_Click);
            // 
            // distanceLabel
            // 
            this.distanceLabel.AutoSize = true;
            this.distanceLabel.Location = new System.Drawing.Point(14, 4);
            this.distanceLabel.Name = "distanceLabel";
            this.distanceLabel.Size = new System.Drawing.Size(55, 14);
            this.distanceLabel.TabIndex = 1;
            this.distanceLabel.Text = "Distance";
            // 
            // PlattformService
            // 
            this.PlattformService.AutoSize = true;
            this.PlattformService.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.PlattformService.Controls.Add(this.readButton);
            this.PlattformService.Controls.Add(this.bitsCheckBox);
            this.PlattformService.Controls.Add(this.textBox1);
            this.PlattformService.Controls.Add(this.Characteristic1);
            this.PlattformService.Controls.Add(this.Characteristic2);
            this.PlattformService.Controls.Add(this.sensorLabel1);
            this.PlattformService.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.PlattformService.Font = new System.Drawing.Font("Lucida Sans", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.PlattformService.Location = new System.Drawing.Point(46, 80);
            this.PlattformService.Name = "PlattformService";
            this.PlattformService.Size = new System.Drawing.Size(472, 210);
            this.PlattformService.TabIndex = 0;
            this.PlattformService.TabStop = false;
            this.PlattformService.Text = "PlattformService";
            this.PlattformService.Enter += new System.EventHandler(this.PlattformService_Enter);
            // 
            // activityLabel
            // 
            this.activityLabel.AutoSize = true;
            this.activityLabel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.activityLabel.Image = global::MultipleServCharac.Properties.Resources.Gnome_Network_Offline_32;
            this.activityLabel.Location = new System.Drawing.Point(516, 6);
            this.activityLabel.Name = "activityLabel";
            this.activityLabel.Size = new System.Drawing.Size(49, 42);
            this.activityLabel.TabIndex = 8;
            this.activityLabel.Text = "              \r\n\r\n\r\n";
            this.activityLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnRefresh
            // 
            this.btnRefresh.BackgroundImage = global::MultipleServCharac.Properties.Resources.Gnome_View_Refresh_32;
            this.btnRefresh.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnRefresh.FlatAppearance.BorderSize = 0;
            this.btnRefresh.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRefresh.Location = new System.Drawing.Point(229, 9);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(36, 36);
            this.btnRefresh.TabIndex = 7;
            this.btnRefresh.UseVisualStyleBackColor = true;
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // btnAttach
            // 
            this.btnAttach.FlatAppearance.BorderSize = 0;
            this.btnAttach.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAttach.Image = global::MultipleServCharac.Properties.Resources.Dialog_Apply_32;
            this.btnAttach.Location = new System.Drawing.Point(266, 8);
            this.btnAttach.Name = "btnAttach";
            this.btnAttach.Size = new System.Drawing.Size(36, 36);
            this.btnAttach.TabIndex = 5;
            this.btnAttach.UseVisualStyleBackColor = true;
            this.btnAttach.Click += new System.EventHandler(this.btnAttach_Click);
            // 
            // btnReset
            // 
            this.btnReset.BackColor = System.Drawing.SystemColors.ControlLight;
            this.btnReset.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnReset.Enabled = false;
            this.btnReset.FlatAppearance.BorderSize = 0;
            this.btnReset.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnReset.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnReset.Image = global::MultipleServCharac.Properties.Resources.Gnome_Process_Stop_64;
            this.btnReset.Location = new System.Drawing.Point(463, 8);
            this.btnReset.Name = "btnReset";
            this.btnReset.Size = new System.Drawing.Size(36, 36);
            this.btnReset.TabIndex = 4;
            this.btnReset.UseVisualStyleBackColor = false;
            this.btnReset.Click += new System.EventHandler(this.btnReset_Click);
            // 
            // btnGo
            // 
            this.btnGo.BackColor = System.Drawing.SystemColors.ControlLight;
            this.btnGo.Enabled = false;
            this.btnGo.FlatAppearance.BorderSize = 0;
            this.btnGo.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnGo.Image = global::MultipleServCharac.Properties.Resources.Gnome_Network_Wireless_32;
            this.btnGo.Location = new System.Drawing.Point(421, 9);
            this.btnGo.Name = "btnGo";
            this.btnGo.Size = new System.Drawing.Size(36, 36);
            this.btnGo.TabIndex = 5;
            this.btnGo.UseVisualStyleBackColor = false;
            this.btnGo.Click += new System.EventHandler(this.btnGo_Click);
            // 
            // lblPorts
            // 
            this.lblPorts.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lblPorts.Image = global::MultipleServCharac.Properties.Resources.Gnome_Preferences_System_Network_64;
            this.lblPorts.ImageAlign = System.Drawing.ContentAlignment.BottomRight;
            this.lblPorts.Location = new System.Drawing.Point(10, 9);
            this.lblPorts.Name = "lblPorts";
            this.lblPorts.Size = new System.Drawing.Size(54, 36);
            this.lblPorts.TabIndex = 0;
            // 
            // separatorTop
            // 
            this.separatorTop.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.separatorTop.Location = new System.Drawing.Point(304, -1);
            this.separatorTop.Name = "separatorTop";
            this.separatorTop.Size = new System.Drawing.Size(7, 57);
            this.separatorTop.TabIndex = 10;
            this.separatorTop.Text = "\r\n\r\n";
            // 
            // label2
            // 
            this.label2.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.label2.Location = new System.Drawing.Point(-1, 52);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(566, 4);
            this.label2.TabIndex = 11;
            this.label2.Click += new System.EventHandler(this.label2_Click);
            // 
            // label3
            // 
            this.label3.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.label3.Location = new System.Drawing.Point(408, -1);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(7, 57);
            this.label3.TabIndex = 12;
            this.label3.Text = "\r\n\r\n";
            // 
            // label4
            // 
            this.label4.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.label4.Location = new System.Drawing.Point(505, -1);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(7, 57);
            this.label4.TabIndex = 11;
            this.label4.Text = "\r\n\r\n";
            // 
            // attributList
            // 
            this.attributList.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.attributList.FormattingEnabled = true;
            this.attributList.ItemHeight = 14;
            this.attributList.Items.AddRange(new object[] {
            "Atrributes"});
            this.attributList.Location = new System.Drawing.Point(81, 224);
            this.attributList.Name = "attributList";
            this.attributList.Size = new System.Drawing.Size(83, 58);
            this.attributList.TabIndex = 13;
            // 
            // readButton
            // 
            this.readButton.Enabled = false;
            this.readButton.ForeColor = System.Drawing.SystemColors.ControlText;
            this.readButton.Location = new System.Drawing.Point(136, 168);
            this.readButton.Name = "readButton";
            this.readButton.Size = new System.Drawing.Size(75, 23);
            this.readButton.TabIndex = 14;
            this.readButton.Text = "Read";
            this.readButton.UseVisualStyleBackColor = true;
            this.readButton.Click += new System.EventHandler(this.readButton_Click);
            // 
            // bitsCheckBox
            // 
            this.bitsCheckBox.AutoSize = true;
            this.bitsCheckBox.Location = new System.Drawing.Point(136, 144);
            this.bitsCheckBox.Name = "bitsCheckBox";
            this.bitsCheckBox.Size = new System.Drawing.Size(60, 18);
            this.bitsCheckBox.TabIndex = 15;
            this.bitsCheckBox.Text = "16Bits";
            this.bitsCheckBox.UseVisualStyleBackColor = true;
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(248, 158);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(205, 20);
            this.textBox1.TabIndex = 16;
            this.textBox1.Text = "Read Attribute:\r\n\r\n";
            this.textBox1.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            // 
            // Form1
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.AutoSize = true;
            this.BackColor = System.Drawing.SystemColors.ControlLight;
            this.ClientSize = new System.Drawing.Size(565, 576);
            this.Controls.Add(this.attributList);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.separatorTop);
            this.Controls.Add(this.activityLabel);
            this.Controls.Add(this.PlattformService);
            this.Controls.Add(this.btnRefresh);
            this.Controls.Add(this.comboPorts);
            this.Controls.Add(this.btnAttach);
            this.Controls.Add(this.btnReset);
            this.Controls.Add(this.txtLog);
            this.Controls.Add(this.btnGo);
            this.Controls.Add(this.lblPorts);
            this.Font = new System.Drawing.Font("Lucida Sans", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.Text = "Bluetooth Smart Sensorplattform Control";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.Characteristic2.ResumeLayout(false);
            this.Characteristic2.PerformLayout();
            this.Characteristic1.ResumeLayout(false);
            this.Characteristic1.PerformLayout();
            this.PlattformService.ResumeLayout(false);
            this.PlattformService.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblPorts;
        private System.Windows.Forms.Button btnGo;
        private System.Windows.Forms.TextBox txtLog;
        private System.Windows.Forms.Button btnReset;
        private System.Windows.Forms.Button btnAttach;
        private System.Windows.Forms.ComboBox comboPorts;
        private System.IO.Ports.SerialPort serialAPI;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.Panel Characteristic2;
        private System.Windows.Forms.Panel Characteristic1;
        private System.Windows.Forms.Label raw_val_label1;
        private System.Windows.Forms.Label sensorlabel2;
        private System.Windows.Forms.GroupBox PlattformService;
        private System.Windows.Forms.Label distanceCmLabel;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label sensor_distance_value_label;
        private System.Windows.Forms.Label sensor_distance_raw_label;
        private System.Windows.Forms.Label sensor_moving_value_label;
        private System.Windows.Forms.Label sensor_moving_raw_label;
        private System.Windows.Forms.Label distanceLabel;
        private System.Windows.Forms.Label moveDectLabel;
        private System.Windows.Forms.Label sensorLabel1;
        private System.Windows.Forms.Label activityLabel;
        private System.Windows.Forms.Label separatorTop;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ListBox attributList;
        private System.Windows.Forms.Button readButton;
        private System.Windows.Forms.CheckBox bitsCheckBox;
    }
}
