namespace CloudlogCAT
{
    partial class ConnectionForm
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
            System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
            System.Windows.Forms.Label label2;
            System.Windows.Forms.Label label1;
            System.Windows.Forms.Label label3;
            System.Windows.Forms.Label label4;
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ConnectionForm));
            this.m_RadioType = new System.Windows.Forms.ComboBox();
            this.m_SerialPort = new System.Windows.Forms.ComboBox();
            this.m_EnableDTR = new System.Windows.Forms.CheckBox();
            this.m_EnableRTS = new System.Windows.Forms.CheckBox();
            this.m_Connect = new System.Windows.Forms.Button();
            this.m_Speed = new System.Windows.Forms.TextBox();
            tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            label2 = new System.Windows.Forms.Label();
            label1 = new System.Windows.Forms.Label();
            label3 = new System.Windows.Forms.Label();
            label4 = new System.Windows.Forms.Label();
            tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            tableLayoutPanel1.ColumnCount = 2;
            tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            tableLayoutPanel1.Controls.Add(label4, 0, 2);
            tableLayoutPanel1.Controls.Add(label3, 0, 3);
            tableLayoutPanel1.Controls.Add(label1, 0, 1);
            tableLayoutPanel1.Controls.Add(this.m_SerialPort, 1, 1);
            tableLayoutPanel1.Controls.Add(label2, 0, 0);
            tableLayoutPanel1.Controls.Add(this.m_RadioType, 1, 0);
            tableLayoutPanel1.Controls.Add(this.m_EnableDTR, 1, 3);
            tableLayoutPanel1.Controls.Add(this.m_EnableRTS, 1, 4);
            tableLayoutPanel1.Controls.Add(this.m_Connect, 1, 5);
            tableLayoutPanel1.Controls.Add(this.m_Speed, 1, 2);
            tableLayoutPanel1.Location = new System.Drawing.Point(12, 12);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 6;
            tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            tableLayoutPanel1.Size = new System.Drawing.Size(296, 210);
            tableLayoutPanel1.TabIndex = 0;
            // 
            // label2
            // 
            label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            label2.AutoSize = true;
            label2.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            label2.Location = new System.Drawing.Point(3, 11);
            label2.Name = "label2";
            label2.Size = new System.Drawing.Size(66, 13);
            label2.TabIndex = 2;
            label2.Text = "&Radio type:";
            // 
            // m_RadioType
            // 
            this.m_RadioType.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.m_RadioType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.m_RadioType.FormattingEnabled = true;
            this.m_RadioType.Location = new System.Drawing.Point(75, 7);
            this.m_RadioType.Name = "m_RadioType";
            this.m_RadioType.Size = new System.Drawing.Size(218, 21);
            this.m_RadioType.TabIndex = 3;
            // 
            // label1
            // 
            label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            label1.AutoSize = true;
            label1.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            label1.Location = new System.Drawing.Point(3, 46);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(66, 13);
            label1.TabIndex = 4;
            label1.Text = "&Serial port:";
            // 
            // m_SerialPort
            // 
            this.m_SerialPort.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.m_SerialPort.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.m_SerialPort.FormattingEnabled = true;
            this.m_SerialPort.Location = new System.Drawing.Point(75, 42);
            this.m_SerialPort.Name = "m_SerialPort";
            this.m_SerialPort.Size = new System.Drawing.Size(218, 21);
            this.m_SerialPort.TabIndex = 5;
            // 
            // m_EnableDTR
            // 
            this.m_EnableDTR.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.m_EnableDTR.AutoSize = true;
            this.m_EnableDTR.Location = new System.Drawing.Point(75, 114);
            this.m_EnableDTR.Name = "m_EnableDTR";
            this.m_EnableDTR.Size = new System.Drawing.Size(218, 17);
            this.m_EnableDTR.TabIndex = 6;
            this.m_EnableDTR.Text = "Enable &DTR";
            this.m_EnableDTR.UseVisualStyleBackColor = true;
            // 
            // label3
            // 
            label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            label3.AutoSize = true;
            label3.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            label3.Location = new System.Drawing.Point(3, 116);
            label3.Name = "label3";
            label3.Size = new System.Drawing.Size(66, 13);
            label3.TabIndex = 7;
            label3.Text = "Options:";
            // 
            // m_EnableRTS
            // 
            this.m_EnableRTS.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.m_EnableRTS.AutoSize = true;
            this.m_EnableRTS.Location = new System.Drawing.Point(75, 149);
            this.m_EnableRTS.Name = "m_EnableRTS";
            this.m_EnableRTS.Size = new System.Drawing.Size(218, 17);
            this.m_EnableRTS.TabIndex = 8;
            this.m_EnableRTS.Text = "Enable R&TS";
            this.m_EnableRTS.UseVisualStyleBackColor = true;
            // 
            // m_Connect
            // 
            this.m_Connect.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.m_Connect.Location = new System.Drawing.Point(218, 181);
            this.m_Connect.Name = "m_Connect";
            this.m_Connect.Size = new System.Drawing.Size(75, 23);
            this.m_Connect.TabIndex = 9;
            this.m_Connect.Text = "Connect";
            this.m_Connect.UseVisualStyleBackColor = true;
            this.m_Connect.Click += new System.EventHandler(this.m_Connect_Click);
            // 
            // label4
            // 
            label4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            label4.AutoSize = true;
            label4.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            label4.Location = new System.Drawing.Point(3, 81);
            label4.Name = "label4";
            label4.Size = new System.Drawing.Size(66, 13);
            label4.TabIndex = 10;
            label4.Text = "S&peed:";
            // 
            // m_Speed
            // 
            this.m_Speed.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.m_Speed.Location = new System.Drawing.Point(75, 76);
            this.m_Speed.Name = "m_Speed";
            this.m_Speed.Size = new System.Drawing.Size(218, 22);
            this.m_Speed.TabIndex = 11;
            this.m_Speed.Text = "4800";
            // 
            // ConnectionForm
            // 
            this.AcceptButton = this.m_Connect;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(320, 234);
            this.Controls.Add(tableLayoutPanel1);
            this.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ConnectionForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Connect to radio";
            this.Load += new System.EventHandler(this.ConnectionForm_Load);
            tableLayoutPanel1.ResumeLayout(false);
            tableLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ComboBox m_SerialPort;
        private System.Windows.Forms.ComboBox m_RadioType;
        private System.Windows.Forms.CheckBox m_EnableDTR;
        private System.Windows.Forms.CheckBox m_EnableRTS;
        private System.Windows.Forms.Button m_Connect;
        private System.Windows.Forms.TextBox m_Speed;
    }
}