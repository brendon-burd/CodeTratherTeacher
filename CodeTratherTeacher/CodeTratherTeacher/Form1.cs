using System.Windows.Forms;
using System.IO;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace CodeTratherTeacher
{
    public partial class Teacher_App : Form
    {
        public Teacher_App()
        {
            InitializeComponent();
        }

        public static string filePath = Environment.GetEnvironmentVariable("USERPROFILE") + @"\" + "Downloads/TratherLogs/";
        public static string unitTestFilePath = Environment.GetEnvironmentVariable("USERPROFILE") + @"\" + "Downloads/unitTest.py";
        public static string execFilPath = Environment.GetEnvironmentVariable("USERPROFILE") + @"\" + "Downloads/ExecutiveSummary.csv";
        public static string tempFilePath = Environment.GetEnvironmentVariable("USERPROFILE") + @"\" + "Downloads/tempCode.py";
        public static string gradeFile = Environment.GetEnvironmentVariable("USERPROFILE") + @"\" + "Downloads/gradeCT.txt";
        public static string inputFilePath = "";
        /// <summary>
        /// List of the student assignments
        /// </summary>
        List<string> assignments = new List<string>();
        /// <summary>
        /// Locations of all the student summaries
        /// </summary>
        List<string> execSumsLocations = new List<string>();

        /// <summary>
        /// Goes through all the student folders and adds the assignment file to the assignments list 
        /// </summary>
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
                            if (file.Contains("Program.py"))
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

        /// <summary>
        /// Allows the user to upload a unit test to be tested against
        /// </summary>
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

        /// <summary>
        /// Tests the given assignment against the uploaded unit test
        /// </summary>
        /// <param name="studentCode"></param>
        /// <returns>Returns the grade that the student received </returns>
        private string runUnitTest(string studentCode)
        {
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

        /// <summary>
        /// Grades all the student's assignments then records them onto an executive summary along with other elements from the student summaries. 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gradeUnitTest(object sender, EventArgs e)
        {
            string grade = "";
            uploadUnitTest();
            getAssignments();
            System.IO.File.WriteAllText(execFilPath, "Name, Grade, Errors, Hot Keys, " + Environment.NewLine);
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
                    grade = System.IO.File.ReadAllText(gradeFile); 
                    grade = grade.Replace("\n", "").Replace("\r", "");
                }
                List<string> tokens = new List<string>(System.IO.File.ReadAllText(execSumsLocations[i]).Split(','));
                string name = tokens[0];
                tokens.RemoveAt(0);
                string remadeCSV = String.Join(", ", tokens);
                System.IO.File.AppendAllText(execFilPath, name + "," + grade + "," + remadeCSV + Environment.NewLine);
            }
            //delete the temp files
            System.IO.File.Delete(tempFilePath);
            //System.IO.File.Delete(gradeFile);
            //alert user
            MessageBox.Show("File successfully created as ExecutiveSummary.csv in downloads folder");
        }

        /// <summary>
        /// Button to decrypt student folders
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void decryptBTN_Click(object sender, EventArgs e)
        {
            Cryptog.decryptSubmit();
        }
    }
}