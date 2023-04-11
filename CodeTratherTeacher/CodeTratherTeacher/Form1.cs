using System.Windows.Forms;
using System.IO;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace CodeTratherTeacher
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        public static string filePath = Environment.GetEnvironmentVariable("USERPROFILE") + @"\" + "Downloads/TratherLogs/";
        public static string unitTestFilePath = "unitTest.py";
        public static string inputFilePath = "";
        List<string> assignments = new List<string>();
        List<string> grades = new List<string>();

        private void getAssignments()
        {
            using (var fbd = new FolderBrowserDialog())
            {
                DialogResult result = fbd.ShowDialog();

                if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(fbd.SelectedPath))
                {
                    string[] folders = Directory.GetDirectories(fbd.SelectedPath);

                    //go throuh each folder and find the assigment file
                    foreach (string folder in folders)
                    {
                        string[] files = Directory.GetFiles(folder);
                        foreach (string file in files)
                        {
                            if (file.Contains("assignment.py"))
                            {
                                assignments.Add(file);
                            }
                        }
                    }
                }
            }
        }

        private OpenFileDialog openFileDialog;

        private void uploadUnitTest()
        {
            // Set up the open file dialog
            openFileDialog = new OpenFileDialog();
            openFileDialog.Title = "Upload Unit Test";
            openFileDialog.Filter = "Python Files (*.py)|*.py";

            //wait for file to be selected
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                //Get the path of specified file and save it in the output directory 
                string unitTestFile = openFileDialog.FileName;
                System.IO.File.WriteAllText(unitTestFilePath, System.IO.File.ReadAllText(unitTestFile));
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
            uploadUnitTest();
            getAssignments();
            foreach (string testFile in assignments)
            {
                inputFilePath = testFile;
                string res = runUnitTest();
                if (res.Contains("error"))
                {
                    grades.Add("0");
                }
                else
                {
                    grades.Add(res);
                }
            }
        }
    }
}