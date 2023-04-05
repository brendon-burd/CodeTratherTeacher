namespace CodeTratherTeacher
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.createExecButton = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // createExecButton
            // 
            this.createExecButton.Location = new System.Drawing.Point(112, 200);
            this.createExecButton.Name = "createExecButton";
            this.createExecButton.Size = new System.Drawing.Size(115, 73);
            this.createExecButton.TabIndex = 0;
            this.createExecButton.Text = "Create Executive Overview ";
            this.createExecButton.UseVisualStyleBackColor = true;
            this.createExecButton.Click += new System.EventHandler(this.createExec);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(268, 200);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(115, 73);
            this.button3.TabIndex = 3;
            this.button3.Text = "Grade with Unit Test";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.gradeUnitTest);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.createExecButton);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);

        }

        #endregion

        private Button createExecButton;
        private Button button3;
    }
}