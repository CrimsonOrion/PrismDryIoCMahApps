using System.Collections.ObjectModel;

using ControlzEx.Theming;

using Prism.Mvvm;

namespace ThemeSelect.Module.ViewModels
{
    public class SelectThemeViewModel : BindableBase
    {
        private ObservableCollection<Theme> _themes = new();
        public ObservableCollection<Theme> Themes
        {
            get => _themes;
            set => SetProperty(ref _themes, value);
        }

        private Theme _selectedTheme;
        public Theme SelectedTheme
        {
            get => _selectedTheme;
            set
            {
                SetProperty(ref _selectedTheme, value);
                ThemeManager.Current.ChangeTheme(System.Windows.Application.Current, SelectedTheme);
            }
        }

        public SelectThemeViewModel() => Themes = new(ThemeManager.Current.Themes);
    }
}
