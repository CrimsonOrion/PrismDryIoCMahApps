using System.Windows;

using PDMA.UI.Views;

using Prism.Ioc;

namespace PDMA.UI
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        protected override Window CreateShell() => Container.Resolve<ShellView>();

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {

        }
    }
}