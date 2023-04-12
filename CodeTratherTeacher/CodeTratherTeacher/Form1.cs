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
        public static string unitTestFilePath = Environment.GetEnvironmentVariable("USERPROFILE") + @"\" + "Downloads/unitTest.py";
        public static string execFilPath = Environment.GetEnvironmentVariable("USERPROFILE") + @"\" + "Downloads/ExecutiveSummary.csv";
        public static string tempFilePath = Environment.GetEnvironmentVariable("USERPROFILE") + @"\" + "Downloads/tempCode.py";
        public static string inputFilePath = "";
        List<string> assignments = new List<string>();
        List<string> execSumsLocations = new List<string>();

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
                            if (file.Contains("ExecutiveSummary.csv"))
                            {
                                execSumsLocations.Add(file);
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

        private string runUnitTest(string studentCode) {
            //create temp file to store student code
            System.IO.File.WriteAllText(tempFilePath, System.IO.File.ReadAllText(studentCode));
            //get file
            System.Diagnostics.Process pProcess = new System.Diagnostics.Process();
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
        private void gradeUnitTest(object sender, EventArgs e)
        {
            string grade = "";
            uploadUnitTest();
            getAssignments();
            System.IO.File.WriteAllText(execFilPath, "Name, Grade," + Environment.NewLine);
            for (int i = 0; i < assignments.Count(); i++)
            {
                inputFilePath = assignments[i];
                string res = runUnitTest(inputFilePath);
                if (res.Contains("error"))
                {
                    grade = "0";
                }
                else
                {
                    grade = res;
                    grade = grade.Replace("\n", "").Replace("\r", "");
                }
                List<string> tokens = new List<string>(System.IO.File.ReadAllText(execSumsLocations[i]).Split(','));
                string name = tokens[0];
                tokens.RemoveAt(0);
                string remadeCSV = String.Join(", ", tokens);
                System.IO.File.AppendAllText(execFilPath, name + "," + grade + "," + remadeCSV + Environment.NewLine);
            }
            //delete the temp file
            System.IO.File.Delete(tempFilePath);
            //alert user
            MessageBox.Show("File successfully created as ExecutiveSummary.csv in downloads folder");
        }
    }
}