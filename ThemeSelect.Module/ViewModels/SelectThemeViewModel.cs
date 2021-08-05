
using ControlzEx.Theming;

using Prism.Mvvm;

namespace ThemeSelect.Module.ViewModels
{
    public class SelectThemeViewModel : BindableBase
    {
        private string _currentTheme;
        public string CurrentTheme
        {
            get => _currentTheme;
            set => SetProperty(ref _currentTheme, value);
        }

        public SelectThemeViewModel() => ThemeManager.Current.ThemeChanged += Current_ThemeChanged;

        ~SelectThemeViewModel()
        {
            ThemeManager.Current.ThemeChanged -= Current_ThemeChanged;
        }

        private void Current_ThemeChanged(object sender, ThemeChangedEventArgs e) => CurrentTheme = ThemeManager.Current.DetectTheme(System.Windows.Application.Current).DisplayName;
    }
}
