namespace ConsoleApplication1
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
            this.searchText = new System.Windows.Forms.TextBox();
            this.searchButton = new System.Windows.Forms.Button();
            this.outputText = new System.Windows.Forms.RichTextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.yStopWords = new System.Windows.Forms.CheckBox();
            this.yStemming = new System.Windows.Forms.CheckBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.outputMsg = new System.Windows.Forms.Label();
            this.evalButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // searchText
            // 
            this.searchText.Location = new System.Drawing.Point(82, 173);
            this.searchText.Name = "searchText";
            this.searchText.Size = new System.Drawing.Size(182, 20);
            this.searchText.TabIndex = 0;
            // 
            // searchButton
            // 
            this.searchButton.Location = new System.Drawing.Point(346, 173);
            this.searchButton.Name = "searchButton";
            this.searchButton.Size = new System.Drawing.Size(88, 22);
            this.searchButton.TabIndex = 1;
            this.searchButton.Text = "Search";
            this.searchButton.UseVisualStyleBackColor = true;
            this.searchButton.Click += new System.EventHandler(this.searchButton_Click);
            // 
            // outputText
            // 
            this.outputText.Location = new System.Drawing.Point(82, 222);
            this.outputText.Name = "outputText";
            this.outputText.Size = new System.Drawing.Size(182, 96);
            this.outputText.TabIndex = 2;
            this.outputText.Text = "";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(79, 78);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(107, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Include Stop Words?";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(89, 105);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(97, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Include Stemming?";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(89, 206);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(42, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "Results";
            // 
            // yStopWords
            // 
            this.yStopWords.AutoSize = true;
            this.yStopWords.Location = new System.Drawing.Point(208, 77);
            this.yStopWords.Name = "yStopWords";
            this.yStopWords.Size = new System.Drawing.Size(42, 17);
            this.yStopWords.TabIndex = 6;
            this.yStopWords.Text = "yes";
            this.yStopWords.UseVisualStyleBackColor = true;
            // 
            // yStemming
            // 
            this.yStemming.AutoSize = true;
            this.yStemming.Location = new System.Drawing.Point(208, 101);
            this.yStemming.Name = "yStemming";
            this.yStemming.Size = new System.Drawing.Size(42, 17);
            this.yStemming.TabIndex = 7;
            this.yStemming.Text = "yes";
            this.yStemming.UseVisualStyleBackColor = true;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(79, 157);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(161, 13);
            this.label4.TabIndex = 8;
            this.label4.Text = "Enter term to search for in CACM";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 26.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(111, 9);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(243, 39);
            this.label5.TabIndex = 9;
            this.label5.Text = "Search Engine";
            // 
            // outputMsg
            // 
            this.outputMsg.AutoSize = true;
            this.outputMsg.Location = new System.Drawing.Point(309, 222);
            this.outputMsg.Name = "outputMsg";
            this.outputMsg.Size = new System.Drawing.Size(35, 13);
            this.outputMsg.TabIndex = 10;
            this.outputMsg.Text = "label6";
            // 
            // evalButton
            // 
            this.evalButton.Location = new System.Drawing.Point(274, 318);
            this.evalButton.Name = "evalButton";
            this.evalButton.Size = new System.Drawing.Size(182, 23);
            this.evalButton.TabIndex = 11;
            this.evalButton.Text = "Run Eval";
            this.evalButton.UseVisualStyleBackColor = true;
            this.evalButton.Click += new System.EventHandler(this.evalButton_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.ClientSize = new System.Drawing.Size(459, 353);
            this.Controls.Add(this.evalButton);
            this.Controls.Add(this.outputMsg);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.yStemming);
            this.Controls.Add(this.yStopWords);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.outputText);
            this.Controls.Add(this.searchButton);
            this.Controls.Add(this.searchText);
            this.Name = "Form1";
            this.Text = "Search Engine";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox searchText;
        private System.Windows.Forms.Button searchButton;
        private System.Windows.Forms.RichTextBox outputText;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.CheckBox yStopWords;
        private System.Windows.Forms.CheckBox yStemming;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label outputMsg;
        private System.Windows.Forms.Button evalButton;
    }
}

