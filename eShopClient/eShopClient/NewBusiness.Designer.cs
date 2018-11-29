namespace eShopClient
{
    partial class NewBusiness
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
            this.submitBtn = new System.Windows.Forms.Button();
            this.businessAddressTxt = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.businessNameTxt = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.businessIdTxt = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.ownerIdTxt = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.submitBtn);
            this.groupBox1.Controls.Add(this.businessAddressTxt);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.businessNameTxt);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.businessIdTxt);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.ownerIdTxt);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(213, 171);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Enter below your business\'s information :";
            // 
            // submitBtn
            // 
            this.submitBtn.Location = new System.Drawing.Point(67, 142);
            this.submitBtn.Name = "submitBtn";
            this.submitBtn.Size = new System.Drawing.Size(75, 23);
            this.submitBtn.TabIndex = 8;
            this.submitBtn.Text = "Submit";
            this.submitBtn.UseVisualStyleBackColor = true;
            this.submitBtn.Click += new System.EventHandler(this.submitBtn_Click);
            // 
            // businessAddressTxt
            // 
            this.businessAddressTxt.Location = new System.Drawing.Point(98, 106);
            this.businessAddressTxt.Name = "businessAddressTxt";
            this.businessAddressTxt.Size = new System.Drawing.Size(100, 20);
            this.businessAddressTxt.TabIndex = 7;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 109);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(51, 13);
            this.label4.TabIndex = 6;
            this.label4.Text = "Address :";
            // 
            // businessNameTxt
            // 
            this.businessNameTxt.Location = new System.Drawing.Point(98, 80);
            this.businessNameTxt.Name = "businessNameTxt";
            this.businessNameTxt.Size = new System.Drawing.Size(100, 20);
            this.businessNameTxt.TabIndex = 5;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 83);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(86, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "Business Name :";
            // 
            // businessIdTxt
            // 
            this.businessIdTxt.Location = new System.Drawing.Point(98, 54);
            this.businessIdTxt.Name = "businessIdTxt";
            this.businessIdTxt.Size = new System.Drawing.Size(100, 20);
            this.businessIdTxt.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 57);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(67, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Business Id :";
            // 
            // ownerIdTxt
            // 
            this.ownerIdTxt.Location = new System.Drawing.Point(98, 28);
            this.ownerIdTxt.Name = "ownerIdTxt";
            this.ownerIdTxt.Size = new System.Drawing.Size(100, 20);
            this.ownerIdTxt.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 31);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(63, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Owner\'s Id :";
            // 
            // button1
            // 
            this.button1.AutoEllipsis = true;
            this.button1.Location = new System.Drawing.Point(250, 59);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 9;
            this.button1.Text = "check id";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // NewBusiness
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(417, 210);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.groupBox1);
            this.Name = "NewBusiness";
            this.Text = "Register a new business";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox businessAddressTxt;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox businessNameTxt;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox businessIdTxt;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox ownerIdTxt;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button submitBtn;
        private System.Windows.Forms.Button button1;
    }
}