﻿using MultiMiner.Engine;
using MultiMiner.Utility.Forms;
using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
using MultiMiner.Win.Extensions;
using System.Reflection;

namespace MultiMiner.Win.Forms
{
    public partial class AboutForm : MessageBoxFontForm
    {
        public AboutForm()
        {
            InitializeComponent();
        }

        private void AboutForm_Load(object sender, EventArgs e)
        {
            PopulateCopyright();
            PopulateAppVersions();
        }

        private void PopulateCopyright()
        {
            DateTime compileDate = Assembly.GetExecutingAssembly().GetCompileDate();
            const string source = "(C) 2013";
            licenseTextBox.Text = licenseTextBox.Text.Replace(source, String.Format("{0} - {1}", source, compileDate.Year));
        }

        private void PopulateAppVersions()
        {
            string multiMinerVersion = Engine.Installer.GetInstalledMinerVersion();
            multiMinerLabel.Text = "MultiMiner " + multiMinerVersion;

            PopulateXgminerVersion(bfgminerLabel);
        }

        private static void PopulateXgminerVersion(Label targetLabel)
        {
            string xgminerName = MinerPath.GetMinerName();
            string xgminerPath = MinerPath.GetPathToInstalledMiner();
            string xgminerVersion = String.Empty;

            if (File.Exists(xgminerPath))
                xgminerVersion = Xgminer.Installer.GetInstalledMinerVersion(xgminerPath);

            if (string.IsNullOrEmpty(xgminerVersion))
                targetLabel.Text = String.Format("{0} not installed", xgminerName);
            else
                targetLabel.Text = String.Format("{0} {1} installed", xgminerName, xgminerVersion);
        }

        private void multiMinerLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("http://multiminerapp.com/");
        }
    }
}
