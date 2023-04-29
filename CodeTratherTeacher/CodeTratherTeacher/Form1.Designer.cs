namespace CodeTratherTeacher
{
    partial class Teacher_App
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
            button3 = new Button();
            decryptBTN = new Button();
            SuspendLayout();
            // 
            // button3
            // 
            button3.Location = new Point(473, 194);
            button3.Name = "button3";
            button3.Size = new Size(115, 73);
            button3.TabIndex = 3;
            button3.Text = "Grade with Unit Test";
            button3.UseVisualStyleBackColor = true;
            button3.Click += gradeUnitTest;
            // 
            // decryptBTN
            // 
            decryptBTN.Location = new Point(183, 194);
            decryptBTN.Name = "decryptBTN";
            decryptBTN.Size = new Size(115, 73);
            decryptBTN.TabIndex = 4;
            decryptBTN.Text = "Decrypt";
            decryptBTN.UseVisualStyleBackColor = true;
            decryptBTN.Click += decryptBTN_Click;
            // 
            // Teacher_App
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(decryptBTN);
            Controls.Add(button3);
            Name = "Teacher_App";
            Text = "Teacher App";
            ResumeLayout(false);
        }

        #endregion
        private Button button3;
        private Button decryptBTN;
    }
}