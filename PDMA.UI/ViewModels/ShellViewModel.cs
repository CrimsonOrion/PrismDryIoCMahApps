using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

using ControlzEx.Theming;

using MahApps.Metro.Controls.Dialogs;

using PDMA.UI.Models;
using PDMA.UI.Services;

using Prism.Commands;
using Prism.Mvvm;

namespace PDMA.UI.ViewModels
{
    public class ShellViewModel : BindableBase
    {
        private readonly IDialogCoordinator _dialogCoordinator;

        public static string Title => "Prism DryIoC / MahApps Demo";

        public DelegateCommand<string> LaunchGitHubSiteCommand => new(LaunchGitHubSite);
        public DelegateCommand SyncThemeNowCommand => new(SyncThemeNow);
        public DelegateCommand<ThemeSyncMode?> ChangeSyncModeCommand => new(ChangeSyncMode);

        public ObservableCollection<ThemeResource> ThemeResources { get; }
        public List<AccentColorMenuData> AccentColors { get; set; }

        public List<AppThemeMenuData> AppThemes { get; set; }

        public List<CultureInfo> CultureInfos { get; set; }

        private CultureInfo? _currentCulture = CultureInfo.CurrentCulture;

        public CultureInfo? CurrentCulture
        {
            get => _currentCulture;
            set => SetProperty(ref _currentCulture, value);
        }

        public ShellViewModel(IDialogCoordinator dialogCoordinator)
        {
            _dialogCoordinator = dialogCoordinator;
            ThemeManager.Current.ThemeSyncMode = ThemeSyncMode.SyncAll;
            ThemeManager.Current.SyncTheme();

            CultureInfos = CultureInfo.GetCultures(CultureTypes.InstalledWin32Cultures)
                .OrderBy(c => c.DisplayName)
                .ToList();

            AccentColors = ThemeManager.Current.Themes
                .GroupBy(_ => _.ColorScheme)
                .OrderBy(_ => _.Key)
                .Select(_ => new AccentColorMenuData { Name = _.Key, ColorBrush = _.First().ShowcaseBrush })
                .ToList();

            AppThemes = ThemeManager.Current.Themes
                .GroupBy(_ => _.BaseColorScheme)
                .Select(_ => _.First())
                .Select(_ => new AppThemeMenuData { Name = _.BaseColorScheme, BorderColorBrush = _.Resources["MahApps.Brushes.ThemeForeground"] as Brush, ColorBrush = _.Resources["MahApps.Brushes.ThemeBackground"] as Brush })
                .ToList();

            ThemeResources = new();
            System.ComponentModel.ICollectionView? view = CollectionViewSource.GetDefaultView(ThemeResources);
            view.SortDescriptions.Add(new System.ComponentModel.SortDescription(nameof(ThemeResource.Key), System.ComponentModel.ListSortDirection.Ascending));
            UpdateThemeResources();
        }

        private async void LaunchGitHubSite(string browser)
        {
            var uri = "https://github.com/CrimsonOrion/PrismDryIoCMahApps";
            var chromePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles), "Google", "Chrome", "Application", "chrome.exe");
            //var edgePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86), "Microsoft", "Edge", "Application", "msedge.exe");
            //var selectedBrowser = browser == "Chrome" ? chromePath : edgePath;
            ProgressDialogController controller = await _dialogCoordinator.ShowProgressAsync(this, "Opening", "Opening Browser");

            try
            {
                controller.SetIndeterminate();
                await Task.Delay(2000);

                ProcessStartInfo info = new() { FileName = uri, UseShellExecute = true, Arguments = browser == "Chrome" ? chromePath : "" };

                Process.Start(info);
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

        void ChangeSyncMode(ThemeSyncMode? themeSyncMode)
        {
            ThemeManager.Current.ThemeSyncMode = themeSyncMode!.Value;
            ThemeManager.Current.SyncTheme();
        }

        void SyncThemeNow()
        {
            ThemeManager.Current.SyncTheme();
        }

        public void UpdateThemeResources()
        {
            ThemeResources.Clear();

            if (Application.Current.MainWindow != null)
            {
                Theme? theme = ThemeManager.Current.DetectTheme(Application.Current.MainWindow);
                if (theme is not null)
                {
                    LibraryTheme? libraryTheme = theme.LibraryThemes.FirstOrDefault(x => x.Origin == "MahApps.Metro");
                    ResourceDictionary? resourceDictionary = libraryTheme?.Resources.MergedDictionaries.FirstOrDefault();

                    if (resourceDictionary != null)
                    {
                        foreach (DictionaryEntry dictionaryEntry in resourceDictionary.OfType<DictionaryEntry>())
                        {
                            ThemeResources.Add(new ThemeResource(theme, libraryTheme!, resourceDictionary, dictionaryEntry));
                        }
                    }
                }
            }
        }
    }
}