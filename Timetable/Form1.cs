﻿using System;
using System.IO;
using System.Windows.Forms;

namespace Timetable
{
    public partial class Form1 : Form
    {
        private FileInfo _filePathToStudents;
        private FileInfo _filePathToTeachers;

        public Form1()
        {
            InitializeComponent();
        }

        private void btnLoad_Click(object sender, EventArgs e)
        {
            var ofd = new OpenFileDialog
            {
                Filter = Settings.ExcelFilter
            };
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                progressBar1.Minimum = 0;
                progressBar1.Maximum = 100;
                progressBar1.Value = 0;
                progressBar1.Step = 1;
                
                var fiFrom = new FileInfo(ofd.FileName);
                _filePathToStudents = new FileInfo(txtPath.Text + Settings.StudentsResultFileName);
                _filePathToTeachers = new FileInfo(txtPath.Text + Settings.TeachersResultFileName);

                DeleteFilesIfExist();
                progressBar1.Increment(10);
                progressBar1.Increment(7);

                try
                {
                    Converter.ConvertTemplateToResult(fiFrom, _filePathToStudents, _filePathToTeachers, progressBar1);

                    MessageBox.Show(Settings.SuccessConvertationMessage);
                    btnOpenStudents.Enabled = true;
                    btnOpenTeachers.Enabled = true;
                }
                catch (IOException)
                {
                    MessageBox.Show(Settings.FailedConvertationMessage);
                }
            }
        }

        private void DeleteFilesIfExist()
        {
            if (File.Exists(_filePathToStudents.FullName))
                File.Delete(_filePathToStudents.FullName);
            if (File.Exists(_filePathToTeachers.FullName))
                File.Delete(_filePathToTeachers.FullName);
        }


        private void btnChoosePath_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            if (fbd.ShowDialog() == DialogResult.OK)
            {
                txtPath.Text = fbd.SelectedPath;
            }
        }

        private void txtPath_TextChanged(object sender, EventArgs e)
        {
            btnLoad.Enabled = !string.IsNullOrEmpty(txtPath.Text);
        }

        private void btnOpenStudents_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start(_filePathToStudents.FullName);
        }

        private void btnOpenTeachers_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start(_filePathToTeachers.FullName);
        }
    }
}