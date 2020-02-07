namespace spbusTerminal
{
    partial class MainForm
    {
        /// <summary>
        /// Обязательная переменная конструктора.
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
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.SerialPortComm_groupBox = new System.Windows.Forms.GroupBox();
            this.Test_button = new System.Windows.Forms.Button();
            this.FNC_label = new System.Windows.Forms.Label();
            this.FNC_comboBox = new System.Windows.Forms.ComboBox();
            this.SAD_label = new System.Windows.Forms.Label();
            this.DAD_label = new System.Windows.Forms.Label();
            this.SAD_numericUpDown = new System.Windows.Forms.NumericUpDown();
            this.DAD_numericUpDown = new System.Windows.Forms.NumericUpDown();
            this.Send_button = new System.Windows.Forms.Button();
            this.Send_textBox = new System.Windows.Forms.TextBox();
            this.Console_textBox = new System.Windows.Forms.TextBox();
            this.ClosePort_button = new System.Windows.Forms.Button();
            this.Optiona_groupBox = new System.Windows.Forms.GroupBox();
            this.DataBits_comboBox = new System.Windows.Forms.ComboBox();
            this.DataBits_label = new System.Windows.Forms.Label();
            this.StopBits_comboBox = new System.Windows.Forms.ComboBox();
            this.StopBits_label = new System.Windows.Forms.Label();
            this.Parity_comboBox = new System.Windows.Forms.ComboBox();
            this.Parity_label = new System.Windows.Forms.Label();
            this.BaudRate_comboBox = new System.Windows.Forms.ComboBox();
            this.BaudRate_label = new System.Windows.Forms.Label();
            this.Port_comboBox = new System.Windows.Forms.ComboBox();
            this.Port_label = new System.Windows.Forms.Label();
            this.OpenPort_button = new System.Windows.Forms.Button();
            this.Mode_groupBox = new System.Windows.Forms.GroupBox();
            this.Text_radioButton = new System.Windows.Forms.RadioButton();
            this.HEX_radioButton = new System.Windows.Forms.RadioButton();
            this._serialPort = new System.IO.Ports.SerialPort(this.components);
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.btr_toolStripStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.SerialPortComm_groupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.SAD_numericUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.DAD_numericUpDown)).BeginInit();
            this.Optiona_groupBox.SuspendLayout();
            this.Mode_groupBox.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // SerialPortComm_groupBox
            // 
            this.SerialPortComm_groupBox.Controls.Add(this.Test_button);
            this.SerialPortComm_groupBox.Controls.Add(this.FNC_label);
            this.SerialPortComm_groupBox.Controls.Add(this.FNC_comboBox);
            this.SerialPortComm_groupBox.Controls.Add(this.SAD_label);
            this.SerialPortComm_groupBox.Controls.Add(this.DAD_label);
            this.SerialPortComm_groupBox.Controls.Add(this.SAD_numericUpDown);
            this.SerialPortComm_groupBox.Controls.Add(this.DAD_numericUpDown);
            this.SerialPortComm_groupBox.Controls.Add(this.Send_button);
            this.SerialPortComm_groupBox.Controls.Add(this.Send_textBox);
            this.SerialPortComm_groupBox.Controls.Add(this.Console_textBox);
            this.SerialPortComm_groupBox.Location = new System.Drawing.Point(13, 13);
            this.SerialPortComm_groupBox.Name = "SerialPortComm_groupBox";
            this.SerialPortComm_groupBox.Size = new System.Drawing.Size(652, 321);
            this.SerialPortComm_groupBox.TabIndex = 0;
            this.SerialPortComm_groupBox.TabStop = false;
            this.SerialPortComm_groupBox.Text = "Serial Port Communication";
            // 
            // Test_button
            // 
            this.Test_button.Location = new System.Drawing.Point(571, 248);
            this.Test_button.Name = "Test_button";
            this.Test_button.Size = new System.Drawing.Size(75, 23);
            this.Test_button.TabIndex = 9;
            this.Test_button.Text = "Clear";
            this.Test_button.UseVisualStyleBackColor = true;
            this.Test_button.Click += new System.EventHandler(this.Test_button_Click);
            // 
            // FNC_label
            // 
            this.FNC_label.AutoSize = true;
            this.FNC_label.Location = new System.Drawing.Point(139, 248);
            this.FNC_label.Name = "FNC_label";
            this.FNC_label.Size = new System.Drawing.Size(28, 13);
            this.FNC_label.TabIndex = 8;
            this.FNC_label.Text = "FNC";
            // 
            // FNC_comboBox
            // 
            this.FNC_comboBox.FormattingEnabled = true;
            this.FNC_comboBox.Location = new System.Drawing.Point(142, 263);
            this.FNC_comboBox.Name = "FNC_comboBox";
            this.FNC_comboBox.Size = new System.Drawing.Size(121, 21);
            this.FNC_comboBox.TabIndex = 7;
            // 
            // SAD_label
            // 
            this.SAD_label.AutoSize = true;
            this.SAD_label.Location = new System.Drawing.Point(57, 248);
            this.SAD_label.Name = "SAD_label";
            this.SAD_label.Size = new System.Drawing.Size(29, 13);
            this.SAD_label.TabIndex = 6;
            this.SAD_label.Text = "SAD";
            // 
            // DAD_label
            // 
            this.DAD_label.AutoSize = true;
            this.DAD_label.Location = new System.Drawing.Point(6, 248);
            this.DAD_label.Name = "DAD_label";
            this.DAD_label.Size = new System.Drawing.Size(30, 13);
            this.DAD_label.TabIndex = 5;
            this.DAD_label.Text = "DAD";
            // 
            // SAD_numericUpDown
            // 
            this.SAD_numericUpDown.Location = new System.Drawing.Point(60, 264);
            this.SAD_numericUpDown.Name = "SAD_numericUpDown";
            this.SAD_numericUpDown.Size = new System.Drawing.Size(48, 20);
            this.SAD_numericUpDown.TabIndex = 4;
            this.SAD_numericUpDown.Value = new decimal(new int[] {
            7,
            0,
            0,
            0});
            // 
            // DAD_numericUpDown
            // 
            this.DAD_numericUpDown.Location = new System.Drawing.Point(6, 264);
            this.DAD_numericUpDown.Name = "DAD_numericUpDown";
            this.DAD_numericUpDown.Size = new System.Drawing.Size(48, 20);
            this.DAD_numericUpDown.TabIndex = 3;
            this.DAD_numericUpDown.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // Send_button
            // 
            this.Send_button.Location = new System.Drawing.Point(571, 288);
            this.Send_button.Name = "Send_button";
            this.Send_button.Size = new System.Drawing.Size(75, 23);
            this.Send_button.TabIndex = 2;
            this.Send_button.Text = "Send";
            this.Send_button.UseVisualStyleBackColor = true;
            this.Send_button.Click += new System.EventHandler(this.Send_button_Click);
            // 
            // Send_textBox
            // 
            this.Send_textBox.Location = new System.Drawing.Point(6, 290);
            this.Send_textBox.Name = "Send_textBox";
            this.Send_textBox.Size = new System.Drawing.Size(559, 20);
            this.Send_textBox.TabIndex = 1;
            // 
            // Console_textBox
            // 
            this.Console_textBox.Location = new System.Drawing.Point(6, 19);
            this.Console_textBox.Multiline = true;
            this.Console_textBox.Name = "Console_textBox";
            this.Console_textBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.Console_textBox.Size = new System.Drawing.Size(640, 216);
            this.Console_textBox.TabIndex = 0;
            // 
            // ClosePort_button
            // 
            this.ClosePort_button.Enabled = false;
            this.ClosePort_button.Location = new System.Drawing.Point(584, 340);
            this.ClosePort_button.Name = "ClosePort_button";
            this.ClosePort_button.Size = new System.Drawing.Size(75, 23);
            this.ClosePort_button.TabIndex = 1;
            this.ClosePort_button.Text = "Close Port";
            this.ClosePort_button.UseVisualStyleBackColor = true;
            this.ClosePort_button.Click += new System.EventHandler(this.ClosePort_button_Click);
            // 
            // Optiona_groupBox
            // 
            this.Optiona_groupBox.Controls.Add(this.DataBits_comboBox);
            this.Optiona_groupBox.Controls.Add(this.DataBits_label);
            this.Optiona_groupBox.Controls.Add(this.StopBits_comboBox);
            this.Optiona_groupBox.Controls.Add(this.StopBits_label);
            this.Optiona_groupBox.Controls.Add(this.Parity_comboBox);
            this.Optiona_groupBox.Controls.Add(this.Parity_label);
            this.Optiona_groupBox.Controls.Add(this.BaudRate_comboBox);
            this.Optiona_groupBox.Controls.Add(this.BaudRate_label);
            this.Optiona_groupBox.Controls.Add(this.Port_comboBox);
            this.Optiona_groupBox.Controls.Add(this.Port_label);
            this.Optiona_groupBox.Location = new System.Drawing.Point(671, 13);
            this.Optiona_groupBox.Name = "Optiona_groupBox";
            this.Optiona_groupBox.Size = new System.Drawing.Size(119, 244);
            this.Optiona_groupBox.TabIndex = 2;
            this.Optiona_groupBox.TabStop = false;
            this.Optiona_groupBox.Text = "Options";
            // 
            // DataBits_comboBox
            // 
            this.DataBits_comboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.DataBits_comboBox.FormattingEnabled = true;
            this.DataBits_comboBox.Location = new System.Drawing.Point(6, 214);
            this.DataBits_comboBox.Name = "DataBits_comboBox";
            this.DataBits_comboBox.Size = new System.Drawing.Size(106, 21);
            this.DataBits_comboBox.TabIndex = 8;
            // 
            // DataBits_label
            // 
            this.DataBits_label.AutoSize = true;
            this.DataBits_label.Location = new System.Drawing.Point(6, 198);
            this.DataBits_label.Name = "DataBits_label";
            this.DataBits_label.Size = new System.Drawing.Size(50, 13);
            this.DataBits_label.TabIndex = 7;
            this.DataBits_label.Text = "Data Bits";
            // 
            // StopBits_comboBox
            // 
            this.StopBits_comboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.StopBits_comboBox.FormattingEnabled = true;
            this.StopBits_comboBox.Location = new System.Drawing.Point(6, 170);
            this.StopBits_comboBox.Name = "StopBits_comboBox";
            this.StopBits_comboBox.Size = new System.Drawing.Size(106, 21);
            this.StopBits_comboBox.TabIndex = 6;
            // 
            // StopBits_label
            // 
            this.StopBits_label.AutoSize = true;
            this.StopBits_label.Location = new System.Drawing.Point(6, 154);
            this.StopBits_label.Name = "StopBits_label";
            this.StopBits_label.Size = new System.Drawing.Size(49, 13);
            this.StopBits_label.TabIndex = 5;
            this.StopBits_label.Text = "Stop Bits";
            // 
            // Parity_comboBox
            // 
            this.Parity_comboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.Parity_comboBox.FormattingEnabled = true;
            this.Parity_comboBox.Location = new System.Drawing.Point(6, 126);
            this.Parity_comboBox.Name = "Parity_comboBox";
            this.Parity_comboBox.Size = new System.Drawing.Size(106, 21);
            this.Parity_comboBox.TabIndex = 6;
            // 
            // Parity_label
            // 
            this.Parity_label.AutoSize = true;
            this.Parity_label.Location = new System.Drawing.Point(6, 110);
            this.Parity_label.Name = "Parity_label";
            this.Parity_label.Size = new System.Drawing.Size(33, 13);
            this.Parity_label.TabIndex = 5;
            this.Parity_label.Text = "Parity";
            // 
            // BaudRate_comboBox
            // 
            this.BaudRate_comboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.BaudRate_comboBox.FormattingEnabled = true;
            this.BaudRate_comboBox.Location = new System.Drawing.Point(6, 82);
            this.BaudRate_comboBox.Name = "BaudRate_comboBox";
            this.BaudRate_comboBox.Size = new System.Drawing.Size(106, 21);
            this.BaudRate_comboBox.TabIndex = 3;
            // 
            // BaudRate_label
            // 
            this.BaudRate_label.AutoSize = true;
            this.BaudRate_label.Location = new System.Drawing.Point(6, 66);
            this.BaudRate_label.Name = "BaudRate_label";
            this.BaudRate_label.Size = new System.Drawing.Size(58, 13);
            this.BaudRate_label.TabIndex = 2;
            this.BaudRate_label.Text = "Baud Rate";
            // 
            // Port_comboBox
            // 
            this.Port_comboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.Port_comboBox.FormattingEnabled = true;
            this.Port_comboBox.Location = new System.Drawing.Point(6, 39);
            this.Port_comboBox.Name = "Port_comboBox";
            this.Port_comboBox.Size = new System.Drawing.Size(106, 21);
            this.Port_comboBox.TabIndex = 1;
            // 
            // Port_label
            // 
            this.Port_label.AutoSize = true;
            this.Port_label.Location = new System.Drawing.Point(6, 22);
            this.Port_label.Name = "Port_label";
            this.Port_label.Size = new System.Drawing.Size(26, 13);
            this.Port_label.TabIndex = 0;
            this.Port_label.Text = "Port";
            // 
            // OpenPort_button
            // 
            this.OpenPort_button.Location = new System.Drawing.Point(671, 340);
            this.OpenPort_button.Name = "OpenPort_button";
            this.OpenPort_button.Size = new System.Drawing.Size(75, 23);
            this.OpenPort_button.TabIndex = 3;
            this.OpenPort_button.Text = "Open Port";
            this.OpenPort_button.UseVisualStyleBackColor = true;
            this.OpenPort_button.Click += new System.EventHandler(this.OpenPort_button_Click);
            // 
            // Mode_groupBox
            // 
            this.Mode_groupBox.Controls.Add(this.Text_radioButton);
            this.Mode_groupBox.Controls.Add(this.HEX_radioButton);
            this.Mode_groupBox.Location = new System.Drawing.Point(671, 263);
            this.Mode_groupBox.Name = "Mode_groupBox";
            this.Mode_groupBox.Size = new System.Drawing.Size(119, 71);
            this.Mode_groupBox.TabIndex = 4;
            this.Mode_groupBox.TabStop = false;
            this.Mode_groupBox.Text = "Mode";
            // 
            // Text_radioButton
            // 
            this.Text_radioButton.AutoSize = true;
            this.Text_radioButton.Location = new System.Drawing.Point(6, 43);
            this.Text_radioButton.Name = "Text_radioButton";
            this.Text_radioButton.Size = new System.Drawing.Size(46, 17);
            this.Text_radioButton.TabIndex = 1;
            this.Text_radioButton.TabStop = true;
            this.Text_radioButton.Text = "Text";
            this.Text_radioButton.UseVisualStyleBackColor = true;
            // 
            // HEX_radioButton
            // 
            this.HEX_radioButton.AutoSize = true;
            this.HEX_radioButton.Location = new System.Drawing.Point(6, 20);
            this.HEX_radioButton.Name = "HEX_radioButton";
            this.HEX_radioButton.Size = new System.Drawing.Size(47, 17);
            this.HEX_radioButton.TabIndex = 0;
            this.HEX_radioButton.TabStop = true;
            this.HEX_radioButton.Text = "HEX";
            this.HEX_radioButton.UseVisualStyleBackColor = true;
            // 
            // _serialPort
            // 
            this._serialPort.DataReceived += new System.IO.Ports.SerialDataReceivedEventHandler(this._serialPort_DataReceived);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btr_toolStripStatusLabel});
            this.statusStrip1.Location = new System.Drawing.Point(0, 367);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(802, 22);
            this.statusStrip1.TabIndex = 5;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // btr_toolStripStatusLabel
            // 
            this.btr_toolStripStatusLabel.Name = "btr_toolStripStatusLabel";
            this.btr_toolStripStatusLabel.Size = new System.Drawing.Size(84, 17);
            this.btr_toolStripStatusLabel.Text = "0 bytes to read";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(802, 389);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.Mode_groupBox);
            this.Controls.Add(this.OpenPort_button);
            this.Controls.Add(this.Optiona_groupBox);
            this.Controls.Add(this.ClosePort_button);
            this.Controls.Add(this.SerialPortComm_groupBox);
            this.Name = "MainForm";
            this.Text = "spbusTerminal";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.SerialPortComm_groupBox.ResumeLayout(false);
            this.SerialPortComm_groupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.SAD_numericUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.DAD_numericUpDown)).EndInit();
            this.Optiona_groupBox.ResumeLayout(false);
            this.Optiona_groupBox.PerformLayout();
            this.Mode_groupBox.ResumeLayout(false);
            this.Mode_groupBox.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox SerialPortComm_groupBox;
        private System.Windows.Forms.Button Send_button;
        private System.Windows.Forms.TextBox Send_textBox;
        private System.Windows.Forms.TextBox Console_textBox;
        private System.Windows.Forms.Button ClosePort_button;
        private System.Windows.Forms.GroupBox Optiona_groupBox;
        private System.Windows.Forms.ComboBox DataBits_comboBox;
        private System.Windows.Forms.Label DataBits_label;
        private System.Windows.Forms.ComboBox StopBits_comboBox;
        private System.Windows.Forms.Label StopBits_label;
        private System.Windows.Forms.ComboBox Parity_comboBox;
        private System.Windows.Forms.Label Parity_label;
        private System.Windows.Forms.ComboBox BaudRate_comboBox;
        private System.Windows.Forms.Label BaudRate_label;
        private System.Windows.Forms.ComboBox Port_comboBox;
        private System.Windows.Forms.Label Port_label;
        private System.Windows.Forms.Button OpenPort_button;
        private System.Windows.Forms.GroupBox Mode_groupBox;
        private System.Windows.Forms.RadioButton Text_radioButton;
        private System.Windows.Forms.RadioButton HEX_radioButton;
        private System.IO.Ports.SerialPort _serialPort;
        private System.Windows.Forms.Label FNC_label;
        private System.Windows.Forms.ComboBox FNC_comboBox;
        private System.Windows.Forms.Label SAD_label;
        private System.Windows.Forms.Label DAD_label;
        private System.Windows.Forms.NumericUpDown SAD_numericUpDown;
        private System.Windows.Forms.NumericUpDown DAD_numericUpDown;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel btr_toolStripStatusLabel;
        private System.Windows.Forms.Button Test_button;
    }
}

