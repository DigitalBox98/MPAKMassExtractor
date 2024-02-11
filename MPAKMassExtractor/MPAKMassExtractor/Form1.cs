using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using CamelotSharp.Pak;

namespace MPAKMassExtractor
{
    public partial class Form1 : Form
    {
        private int _fileCount = 0;
        private int _archiveCount = 0;

        public Form1()
        {
            InitializeComponent();
        }

        private void mpkFolderTextBox_Click(object sender, EventArgs e)
        {
            DialogResult res = folderBrowserDialog.ShowDialog();
            if (res == System.Windows.Forms.DialogResult.OK)
            {
                mpkFolderTextBox.Text = folderBrowserDialog.SelectedPath;
            }
        }

        private void extractFolderTextbox_Click(object sender, EventArgs e)
        {
            DialogResult res = folderBrowserDialog.ShowDialog();
            if (res == System.Windows.Forms.DialogResult.OK)
            {
                extractFolderTextbox.Text = folderBrowserDialog.SelectedPath;
            }
        }

        private void extractButton_Click(object sender, EventArgs e)
        {
            //get a list of all the mpak files in a folder
            string[] files = Directory.GetFiles(mpkFolderTextBox.Text);

            List<string> extractFileNames = new List<string>();

            foreach (string fileName in files)
            {
                string ext = Path.GetExtension(fileName);
                if (ext.ToLower() == ".mpk" || ext.ToLower() == ".npk")
                    extractFileNames.Add(fileName);
            }


            //extract the mpak file to the extract folder
            foreach (string extractFileName in extractFileNames)
            {
                PAKFile file = new PAKFile(extractFileName);
                foreach (Entry entry in file.Files)
                {
                    string destFile = extractFolderTextbox.Text + Path.DirectorySeparatorChar + entry.FileName;
                    FileStream stream = File.Create(destFile);
                    file.ExtractFile(entry.FileName, stream);
                    stream.Close();
                    _fileCount++;
                }
            }
            _archiveCount = extractFileNames.Count;
            MessageBox.Show("Extracted " + _fileCount + " files from " + _archiveCount + " archives!");
        }
    }
}
