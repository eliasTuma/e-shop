namespace eShopClient
{
    partial class BusinessInfo
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.businessNameTxt = new System.Windows.Forms.TextBox();
            this.businessAddressTxt = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.submitBtn = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.submitBtn);
            this.groupBox1.Controls.Add(this.businessAddressTxt);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.businessNameTxt);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(3, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(272, 135);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(37, 28);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(86, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Business Name :";
            // 
            // businessNameTxt
            // 
            this.businessNameTxt.Location = new System.Drawing.Point(141, 25);
            this.businessNameTxt.Name = "businessNameTxt";
            this.businessNameTxt.Size = new System.Drawing.Size(100, 20);
            this.businessNameTxt.TabIndex = 1;
            // 
            // businessAddressTxt
            // 
            this.businessAddressTxt.Location = new System.Drawing.Point(141, 51);
            this.businessAddressTxt.Name = "businessAddressTxt";
            this.businessAddressTxt.Size = new System.Drawing.Size(100, 20);
            this.businessAddressTxt.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(37, 54);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(96, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Business Address :";
            // 
            // submitBtn
            // 
            this.submitBtn.Location = new System.Drawing.Point(90, 77);
            this.submitBtn.Name = "submitBtn";
            this.submitBtn.Size = new System.Drawing.Size(97, 23);
            this.submitBtn.TabIndex = 4;
            this.submitBtn.Text = "Save Changes";
            this.submitBtn.UseVisualStyleBackColor = true;
            this.submitBtn.Click += new System.EventHandler(this.submitBtn_Click);
            // 
            // BusinessInfo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(279, 139);
            this.Controls.Add(this.groupBox1);
            this.Name = "BusinessInfo";
            this.Text = "Edit Business Information";
            this.Load += new System.EventHandler(this.BusinessInfo_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button submitBtn;
        private System.Windows.Forms.TextBox businessAddressTxt;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox businessNameTxt;
    }
}