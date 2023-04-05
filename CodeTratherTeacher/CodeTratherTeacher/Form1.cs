using System.Windows.Forms;
using System.IO;

namespace CodeTratherTeacher
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        public static string filePath = Environment.GetEnvironmentVariable("USERPROFILE") + @"\" + "Downloads/TratherLogs/";
        public static string unitTestFilePath = filePath + "unitTest.py";
        public static string inputFilePath = "";

        private OpenFileDialog openFileDialog;

        private void getUnitTest()
        {
            // Set up the open file dialog
            openFileDialog = new OpenFileDialog();
            //wait for file to be selected
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                //check if derectory or single file
                if (openFileDialog.FileName.Contains(".py"))
                {
                    System.IO.File.WriteAllText(unitTestFilePath, System.IO.File.ReadAllText(openFileDialog.FileName));
                }
                else
                {
                    foreach (string file in Directory.EnumerateFiles(openFileDialog.FileName, "*.py"))
                    {
                        string contents = File.ReadAllText(file);
                        MessageBox.Show(contents);
                    }
                }
            }
        }

        private string runUnitTest() {

            //get file
            System.Diagnostics.Process pProcess = new System.Diagnostics.Process();
            //pProcess.StartInfo.CreateNoWindow = true;
            pProcess.StartInfo.UseShellExecute = false;
            pProcess.StartInfo.FileName = "cmd.exe";
            pProcess.StartInfo.Arguments = "/C python " + unitTestFilePath + " " + inputFilePath;
            // code either compiles or it doesn't
            pProcess.StartInfo.RedirectStandardOutput = true;
            pProcess.StartInfo.RedirectStandardError = true;
            // start the command prompt
            pProcess.Start();
            string output = pProcess.StandardOutput.ReadToEnd();
            string error = pProcess.StandardError.ReadToEnd();
            pProcess.WaitForExit();
            inputFilePath = "";
            return output + error;
        }

        private void createExec(object sender, EventArgs e)
        {
            
        }

        private void gradeUnitTest(object sender, EventArgs e)
        {
            getUnitTest();
        }
    }
}