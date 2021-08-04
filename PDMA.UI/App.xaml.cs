using System.Windows;

using ControlzEx.Theming;

using MahApps.Metro.Controls.Dialogs;

using PDMA.UI.Views;

using Prism.Ioc;
using Prism.Modularity;

using ThemeSelect.Module;

namespace PDMA.UI
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        protected override Window CreateShell() => Container.Resolve<ShellView>();

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            //ThemeManager.Current.ThemeSyncMode = ThemeSyncMode.SyncAll;
            //ThemeManager.Current.ChangeTheme(this, "Light.Purple");
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry) => containerRegistry
                .RegisterInstance<IDialogCoordinator>(new DialogCoordinator())
                ;

        protected override void ConfigureModuleCatalog(IModuleCatalog moduleCatalog) => moduleCatalog
                .AddModule<ThemeSelectModule>()
                ;
    }
}