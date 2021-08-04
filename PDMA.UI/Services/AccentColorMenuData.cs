using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

using ControlzEx.Theming;

using Prism.Commands;

namespace PDMA.UI.Services
{
    public class AccentColorMenuData
    {
        public string? Name { get; set; }

        public Brush? BorderColorBrush { get; set; }

        public Brush? ColorBrush { get; set; }

        public AccentColorMenuData() => ChangeAccentCommand = new DelegateCommand<string?>(DoChangeTheme, _ => true);

        public ICommand ChangeAccentCommand { get; }

        protected virtual void DoChangeTheme(string? name)
        {
            if (name is not null)
            {
                ThemeManager.Current.ChangeThemeColorScheme(Application.Current, name);
            }
        }
    }
}