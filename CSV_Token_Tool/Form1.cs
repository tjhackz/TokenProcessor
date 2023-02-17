using CSVTokenOperations;
using System.Windows.Forms;

namespace CSV_Token_Tool
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            folderBrowserDialog1.ShowNewFolderButton = false;
            folderBrowserDialog2.ShowNewFolderButton = false;
        }

        public void AddToLogs(string logData)
        {
            textBox5.Text += Environment.NewLine + logData;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {

                if (Directory.Exists(folderBrowserDialog1.SelectedPath))
                {
                    textBox1.Text = folderBrowserDialog1.SelectedPath;
                    DirectoryInfo dir_info = new DirectoryInfo(folderBrowserDialog1.SelectedPath);
                    string directory = dir_info.Name;
                    textBox3.Text = directory;

                    AddToLogs("Master Directory Set.");
                }
                else
                {
                    
                }
            }
            EnableDisableRun();
        }

        private void button2_Click(object sender, EventArgs e)
        {

            if (folderBrowserDialog2.ShowDialog() == DialogResult.OK)
            {

                if (Directory.Exists(folderBrowserDialog2.SelectedPath))
                {
                    textBox2.Text = folderBrowserDialog2.SelectedPath;
                    DirectoryInfo dir_info = new DirectoryInfo(folderBrowserDialog2.SelectedPath);
                    string directory = dir_info.Name;
                    textBox4.Text = directory;
                    
                    AddToLogs("Instrument Directory Set.");
                }
                else
                {

                }
            }
            EnableDisableRun();
        }

        private void EnableDisableRun()
        {
            if(CheckIfShouldRun())
            {
                button3.Enabled = true;
                SetStatusStripMessage("Click on Run to start the process.");
                AddToLogs("Ready to Process the files.");
            }
            else
            {
                button3.Enabled = false;
            }
        }


        private bool CheckIfShouldRun()
        {

            if ( !string.IsNullOrEmpty(textBox1.Text)
                && !string.IsNullOrWhiteSpace(textBox1.Text)
                && !string.IsNullOrEmpty(textBox2.Text)
                && !string.IsNullOrWhiteSpace(textBox2.Text))
            {
                return true;
            }

            return false;
        }
        private void button3_Click(object sender, EventArgs e)
        {
            FileHelper fh = new FileHelper();

            string MasterDirPath = textBox1.Text;
            string InstrumentDirPath = textBox2.Text;
            
            AddToLogs("Getting Master & Instrument Dir Files...");

            var InstrumentDir = fh.GetFileNames(InstrumentDirPath);
            var MasterDir = fh.GetFileNames(MasterDirPath);

            int Current = 0;
            int TotalFiles = MasterDir?.Count() ?? 0;
            int Moved_Master_count = 0;
            int Moved_Insturment_count = 0;

            if (MasterDir?.Count > 0)
            {
                foreach (string item in MasterDir)
                {
                    AddToLogs($"Processing File {++Current} of {TotalFiles} ");

                    if (MainOperations.CheckIfDeleteFlagValid(MainOperations.GetDeleteFlagValueFromMasterModel(MainOperations.ReadMasterCSV(MasterDirPath, item))))
                    {

                        AddToLogs($"Found File with Valid Delete Flag: {item}. Moving to /deleted directory...");

                        MainOperations.MoveCSVFile(MasterDirPath, System.IO.Path.Combine(MasterDirPath, MainOperations.DELETED_DIR_NAME), item);

                        Moved_Master_count++;
                        AddToLogs($"Moved to MasterDir/deleted successfully.");

                        AddToLogs("Checking for the same file inside Instrument Folder...");

                        if (MainOperations.CheckIfFileExistInDirectory(item, InstrumentDir))
                        {
                            AddToLogs("Found the same file inside Instrument Folder.");
                            MainOperations.MoveCSVFile(InstrumentDirPath, System.IO.Path.Combine(InstrumentDirPath, MainOperations.DELETED_DIR_NAME), item);
                            AddToLogs($"Moved to InstrumentDir/deleted successfully.");
                            Moved_Insturment_count++;
                        }
                        else
                        {
                            AddToLogs("Not Found inside Instrument Folder.");
                        }
                    }
                    else
                    {
                        AddToLogs("Valid Delete Flag not found. Skipping processing for this record...");
                    }
                }
                AddToLogs($"Operation Comepleted.");
                AddToLogs($"Total Master Directory Files Processed : {TotalFiles}");
                AddToLogs($"Total Files moved to Master Dir/deleted : {Moved_Master_count}");
                AddToLogs($"Total Files moved to Instruments Dir/deleted : {Moved_Insturment_count}");

                SetStatusStripMessage("Task completed!");

            }
            else
            {
                AddToLogs("Not found any CSV inside MASTER dir. Please check the selection.");
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            MessageBox.Show("Created by Jay for Upwork.");
            SetStatusStripMessage("Application Ready.");
            EnableDisableRun();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Bye! Created by Jay for Upwork.");
            this.Close();
        }
        private void SetStatusStripMessage(string msg)
        {
            toolStripStatusLabel1.Text = msg;
        }

        private void ResetWindow()
        {
            textBox1.Text = string.Empty;
            textBox2.Text = string.Empty;
            textBox3.Text = string.Empty;
            textBox4.Text = string.Empty;
            textBox5.Text = string.Empty;
            folderBrowserDialog1.Dispose();
            folderBrowserDialog2.Dispose();
        }

        private void resetToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ResetWindow();
        }
    }
}