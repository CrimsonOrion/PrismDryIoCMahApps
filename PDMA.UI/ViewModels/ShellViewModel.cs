using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

using MahApps.Metro.Controls.Dialogs;

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

        private readonly IDialogCoordinator _dialogCoordinator;
        public DelegateCommand<string> LaunchGitHubSiteCommand { get; }
        //public DelegateCommand ChangeThemeCommand { get; }

        public ShellViewModel(IDialogCoordinator dialogCoordinator)
        {
            _dialogCoordinator = dialogCoordinator;
            LaunchGitHubSiteCommand = new(LaunchGitHubSite);
            //ChangeThemeCommand = new(ChangeTheme);
        }

        private async void LaunchGitHubSite(string browser)
        {
            var uri = "https://github.com/CrimsonOrion/PrismDryIoCMahApps";
            //var chromePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles), "Google", "Chrome", "Application", "chrome.exe");
            //var edgePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86), "Microsoft", "Edge", "Application", "msedge.exe");
            //var selectedBrowser = browser == "Chrome" ? chromePath : edgePath;
            ProgressDialogController controller = await _dialogCoordinator.ShowProgressAsync(this, "Opening", "Opening Browser");

            try
            {
                controller.SetIndeterminate();
                await Task.Run(() => Thread.Sleep(1000));
                Process.Start(new ProcessStartInfo { FileName = uri, UseShellExecute = true });
            }
            catch (System.ComponentModel.Win32Exception noBrowser)
            {
                if (noBrowser.ErrorCode == -2147467259)
                {
                    // need a width of 700 to see all 4 selections, 535 to see 3, 355 to see 2.
                    MessageDialogResult result = await _dialogCoordinator.ShowMessageAsync(this, "No Default Browser Set", noBrowser.Message, MessageDialogStyle.AffirmativeAndNegativeAndSingleAuxiliary,
                        new MetroDialogSettings { AffirmativeButtonText = "Yes", NegativeButtonText = "No", FirstAuxiliaryButtonText = "Maybe", SecondAuxiliaryButtonText = "So" });

                }
            }
            catch (Exception other)
            {
                await _dialogCoordinator.ShowMessageAsync(this, "Error", other.Message);
            }
            await controller.CloseAsync();
        }

        //private void ChangeTheme() => ThemeManager.Current.ChangeTheme(Application.Current, "Dark.Green");
    }
}