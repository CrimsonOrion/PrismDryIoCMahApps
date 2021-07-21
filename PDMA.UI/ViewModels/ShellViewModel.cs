using System;
using System.Diagnostics;
using System.IO;
using System.Windows;

using Prism.Commands;
using Prism.Mvvm;

namespace PDMA.UI.ViewModels
{
    public class ShellViewModel : BindableBase
    {

        private string _title = "Prism DryIoC / MahApps Demo";
        public string Title
        {
            get => _title;
            set => SetProperty(ref _title, value);
        }

        public DelegateCommand<string> LaunchGitHubSiteCommand { get; }

        public ShellViewModel() => LaunchGitHubSiteCommand = new(LaunchGitHubSite);

        private void LaunchGitHubSite(string browser)
        {
            var uri = "https://github.com/CrimsonOrion/PrismDryIoCMahApps";
            var chromePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles), "Google", "Chrome", "Application", "chrome.exe");
            var edgePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86), "Microsoft", "Edge", "Application", "msedge.exe");
            var selectedBrowser = browser == "Chrome" ? chromePath : edgePath;
            try
            {
                Process.Start(selectedBrowser, uri);
            }
            catch (System.ComponentModel.Win32Exception noBrowser)
            {
                if (noBrowser.ErrorCode == -2147467259)
                {
                    MessageBox.Show(noBrowser.Message);
                }

            }
            catch (Exception other)
            {
                MessageBox.Show(other.Message);
            }
        }
    }
}