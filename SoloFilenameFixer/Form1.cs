﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace SoloFilenameFixer
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void buttonLocation_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog folder = new FolderBrowserDialog();
            folder.ShowDialog();
            textBoxFolderPath.Text = folder.SelectedPath;
            loadFiles();
        }
        private void loadFiles()
        {
            fileListBox.Items.Clear();
            fileListBox.Items.Add("Parent Directory");
            try
            {
                string[] fileNames = Directory.GetFiles(textBoxFolderPath.Text);
                foreach (String name in fileNames)
                {
                    fileListBox.Items.Add(name);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error:");
                fileListBox.Items.Clear();
            }
        }

        private void buttonRefresh_Click(object sender, EventArgs e)
        {
            loadFiles();
        }

        // Strip a path to find only the file name following the final slash.
        private string findFilename(string path)
        {
            string ret = "";
            int finalSlash = path.LastIndexOf('\\');
            if (finalSlash > -1 && finalSlash + 1 != path.Length)
            {
                ret = path.Substring(finalSlash + 1);
            }
            return ret;
        }
        // Strip a path to find only the path preceeding final slash.
        private string findPath(string path)
        {
            string ret = "";
            int finalSlash = path.LastIndexOf('\\');
            if (finalSlash > -1)
            {
                ret = path.Substring(0, finalSlash);
            }
            return ret;
        }

        private void fileListBox_DoubleClick(object sender, EventArgs e)
        {
            int index = fileListBox.SelectedIndex;
            if (index == 0)
            {
                textBoxFolderPath.Text = findPath(textBoxFolderPath.Text);
                loadFiles();
            }
        }
    }
}
