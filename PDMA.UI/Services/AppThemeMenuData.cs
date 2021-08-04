using System.Windows;

using ControlzEx.Theming;

namespace PDMA.UI.Services
{
    public class AppThemeMenuData : AccentColorMenuData
    {
        protected override void DoChangeTheme(string? name)
        {
            if (name is not null)
            {
                ThemeManager.Current.ChangeThemeBaseColor(Application.Current, name);
            }
        }
    }
}