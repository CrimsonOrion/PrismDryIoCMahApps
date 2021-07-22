using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;

using ThemeSelect.Module.Views;

namespace ThemeSelect.Module
{
    public class ThemeSelectModule : IModule
    {
        private readonly IRegionManager _regionManager;

        public ThemeSelectModule(IRegionManager regionManager) => _regionManager = regionManager;

        public void OnInitialized(IContainerProvider containerProvider) => _regionManager.RequestNavigate("ContentRegion", "SelectThemeView");

        public void RegisterTypes(IContainerRegistry containerRegistry) => containerRegistry.RegisterForNavigation<SelectThemeView>();
    }
}