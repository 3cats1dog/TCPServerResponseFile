
namespace TCPServerResponseFile
{
    partial class Form2
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
            this.txtLogs = new System.Windows.Forms.RichTextBox();
            this.txtPortStart = new System.Windows.Forms.TextBox();
            this.txtPortEnd = new System.Windows.Forms.TextBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.btnStartServer = new System.Windows.Forms.Button();
            this.txtIP = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // txtLogs
            // 
            this.txtLogs.Dock = System.Windows.Forms.DockStyle.Right;
            this.txtLogs.Location = new System.Drawing.Point(323, 0);
            this.txtLogs.Name = "txtLogs";
            this.txtLogs.Size = new System.Drawing.Size(591, 552);
            this.txtLogs.TabIndex = 0;
            this.txtLogs.Text = "";
            // 
            // txtPortStart
            // 
            this.txtPortStart.Location = new System.Drawing.Point(82, 58);
            this.txtPortStart.Name = "txtPortStart";
            this.txtPortStart.Size = new System.Drawing.Size(74, 23);
            this.txtPortStart.TabIndex = 1;
            this.txtPortStart.Text = "52000";
            // 
            // txtPortEnd
            // 
            this.txtPortEnd.Location = new System.Drawing.Point(181, 58);
            this.txtPortEnd.Name = "txtPortEnd";
            this.txtPortEnd.Size = new System.Drawing.Size(74, 23);
            this.txtPortEnd.TabIndex = 2;
            this.txtPortEnd.Text = "54000";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.txtIP);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.btnStartServer);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.txtPortEnd);
            this.panel1.Controls.Add(this.txtPortStart);
            this.panel1.Location = new System.Drawing.Point(12, 12);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(280, 322);
            this.panel1.TabIndex = 3;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(162, 61);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(12, 15);
            this.label1.TabIndex = 3;
            this.label1.Text = "-";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(18, 61);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(29, 15);
            this.label2.TabIndex = 4;
            this.label2.Text = "Port";
            // 
            // btnStartServer
            // 
            this.btnStartServer.Location = new System.Drawing.Point(161, 97);
            this.btnStartServer.Name = "btnStartServer";
            this.btnStartServer.Size = new System.Drawing.Size(94, 35);
            this.btnStartServer.TabIndex = 5;
            this.btnStartServer.Text = "Start";
            this.btnStartServer.UseVisualStyleBackColor = true;
            this.btnStartServer.Click += new System.EventHandler(this.btnStartServer_Click);
            // 
            // txtIP
            // 
            this.txtIP.Location = new System.Drawing.Point(82, 26);
            this.txtIP.Name = "txtIP";
            this.txtIP.Size = new System.Drawing.Size(100, 23);
            this.txtIP.TabIndex = 7;
            this.txtIP.Text = "192.168.1.100";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(18, 29);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(17, 15);
            this.label3.TabIndex = 6;
            this.label3.Text = "IP";
            // 
            // Form2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(914, 552);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.txtLogs);
            this.Name = "Form2";
            this.Text = "Form2";
            this.Load += new System.EventHandler(this.Form2_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.RichTextBox txtLogs;
        private System.Windows.Forms.TextBox txtPortStart;
        private System.Windows.Forms.TextBox txtPortEnd;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnStartServer;
        private System.Windows.Forms.TextBox txtIP;
        private System.Windows.Forms.Label label3;
    }
}