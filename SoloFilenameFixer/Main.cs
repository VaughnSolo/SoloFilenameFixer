using System;
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
    public partial class Main : Form
    {
        List<string> tagsToRemove = new List<string>();
        public Main()
        {
            InitializeComponent();
        }

        private void LoadFirstChangeItem()
        {
            LoadChangeItem(FirstFileIndex());
        }
        private void LoadChangeItem(int pos)
        {
            int fileStart = 1;
            
        }
        private int FirstFileIndex()
        {
            int ret = 0;
            foreach (object item in fileListBox.Items)
            {
                string text = item.ToString();
                if (text[text.Length - 1] != '\\' || text[text.Length - 1] != ']')
                {
                    return ret;
                }
                ret++;
            }
            return 0;
        }
        private void buttonLocation_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog folder = new FolderBrowserDialog();
            folder.ShowDialog();
            if (folder.SelectedPath != "")
            {
                textBoxFolderPath.Text = folder.SelectedPath;
                loadFiles();
            }
        }
        private void loadFiles()
        {
            fileListBox.Items.Clear();
            fileListBox.Items.Add("[Parent Directory]");
            try
            {
                string[] folderNames = Directory.GetDirectories(textBoxFolderPath.Text);
                foreach (String name in folderNames)
                {
                    fileListBox.Items.Add(FindFilename(name) + '\\');
                }
                string[] fileNames = Directory.GetFiles(textBoxFolderPath.Text);
                foreach (String name in fileNames)
                {
                    fileListBox.Items.Add(FindFilename(name));
                }
                LoadFirstChangeItem();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error:");
                fileListBox.Items.Clear();
            }
            if (fileListBox.Items.Count > 0)
            {
                int startOfFileIndex = 1;
                while (fileListBox.Items[startOfFileIndex].ToString().Contains("\\"))
                {
                    startOfFileIndex++;
                    if (startOfFileIndex == fileListBox.Items.Count)
                    {
                        startOfFileIndex = 0;
                        break;
                    }
                }
                if (startOfFileIndex > 0)
                {
                    fileListBox.SelectedIndex = startOfFileIndex;
                }
            }
        }

        private void buttonRefresh_Click(object sender, EventArgs e)
        {
            loadFiles();
        }

        // Strip a path to find only the file name following the final slash.
        private string FindFilename(string path)
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
        private string StripPathToSlash(string path)
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
                textBoxFolderPath.Text = StripPathToSlash(textBoxFolderPath.Text);
                loadFiles();
            }
            else if (SelectedIsFolder())
            {
                string fileFolderName = fileListBox.Items[fileListBox.SelectedIndex].ToString();
                textBoxFolderPath.Text += '\\' + fileFolderName.Substring(0,fileFolderName.Length-1);
                loadFiles();
            }
        }
        private bool SelectedIsFolder()
        {
            string fileFolderName = fileListBox.Items[fileListBox.SelectedIndex].ToString();
            char finalChar = fileFolderName[fileFolderName.Length-1];
            if (finalChar == '\\')
            {

                return true;
            }
            return false;
        }

        private void fileListBox_DrawItem(object sender, DrawItemEventArgs e)
        {
            e.DrawBackground();

            bool isItemSelected = ((e.State & DrawItemState.Selected) == DrawItemState.Selected);
            int itemIndex = e.Index;
            if (itemIndex >= 0 && itemIndex < fileListBox.Items.Count)
            {
                Graphics g = e.Graphics;

                // Background Color
                SolidBrush backgroundColorBrush = new SolidBrush((isItemSelected) ? Color.Red : Color.White);
                g.FillRectangle(backgroundColorBrush, e.Bounds);

                // Set text color
                string itemText = fileListBox.Items[itemIndex].ToString();

                SolidBrush itemTextColorBrush = (isItemSelected) ? new SolidBrush(Color.White) : new SolidBrush(Color.Black);
                g.DrawString(itemText, e.Font, itemTextColorBrush, fileListBox.GetItemRectangle(itemIndex).Location);

                // Clean up
                backgroundColorBrush.Dispose();
                itemTextColorBrush.Dispose();
            }

            e.DrawFocusRectangle();
        }

        private void fileListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadRenamerWithFile(fileListBox.SelectedItem.ToString());
        }
        private void LoadRenamerWithFile(string filename)
        {
            textBox2.Text = filename;
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            RunFilenameChange();
        }
        // Complicated Filename Fixer Code
        private void RunFilenameChange()
        {
            string newName = textBox2.Text;
            newName = RunCheckboxedActions(newName);
            newName = RunTagRemover(newName);
            textBox3.Text = newName;
        }
        private string RunCheckboxedActions(string preName)
        {
            string newName = preName;
            
            return newName;
        }
        private string RunTagRemover(string preName)
        {
            string newName = preName;
            foreach (var item in tagsToRemove)
            {
                newName = newName.Replace(item.ToString(), "");
            }
            return newName;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            MoveSelectedIndex(1);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            MoveSelectedIndex(-1);
        }
        private void MoveSelectedIndex(int amt)
        {
            if (fileListBox.Items.Count > 0 && fileListBox.SelectedIndex >= 0)
            {
                int newIndex = fileListBox.SelectedIndex;
                newIndex += amt;
                if (newIndex >= fileListBox.Items.Count)
                {
                    fileListBox.SelectedIndex = fileListBox.Items.Count -1;
                    return;
                }
                if (newIndex < 0)
                {
                    fileListBox.SelectedIndex = 0;
                    return;
                }
                else
                {
                    fileListBox.SelectedIndex = newIndex;
                }
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            if (!checkedListBox1.Items.Contains(textBox1.Text))
            {
                checkedListBox1.Items.Add(textBox1.Text);
            }
            RunFilenameChange();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            if (checkedListBox1.Items.Count > 0 && checkedListBox1.SelectedIndex >= 0)
            {
                tagsToRemove.Remove(checkedListBox1.SelectedItem.ToString());
                checkedListBox1.Items.RemoveAt(checkedListBox1.SelectedIndex);
            }
            RunFilenameChange();
        }

        private void checkedListBox1_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            tagsToRemove = new List<string>();
            string changedItem = checkedListBox1.Items[e.Index].ToString();
            foreach (var item in checkedListBox1.CheckedItems)
            {
                tagsToRemove.Add(item.ToString());
            }
            bool newVal = (e.CurrentValue == CheckState.Checked) ? false : true;
            if (newVal)
            {
                if (!tagsToRemove.Contains(changedItem))
                {
                    tagsToRemove.Add(changedItem);
                }
            }
            else
            {
                tagsToRemove.Remove(changedItem);
            }
            RunFilenameChange();
        }
    }
}
