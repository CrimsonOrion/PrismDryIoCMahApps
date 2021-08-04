using System.Collections;
using System.Globalization;
using System.Windows;
using System.Windows.Media;

using ControlzEx.Theming;

namespace PDMA.UI.Models
{
    public class ThemeResource
    {
        public Theme Theme { get; }

        public LibraryTheme LibraryTheme { get; }

        public string Source { get; }

        public string? Key { get; }

        public Brush? Value { get; }

        public string? StringValue { get; }

        public ThemeResource(Theme theme, LibraryTheme libraryTheme, ResourceDictionary resourceDictionary, DictionaryEntry dictionaryEntry)
            : this(theme, libraryTheme, resourceDictionary, dictionaryEntry.Key.ToString(), dictionaryEntry.Value)
        {
        }

        public ThemeResource(Theme theme, LibraryTheme libraryTheme, ResourceDictionary resourceDictionary, string? key, object? value)
        {
            Theme = theme;
            LibraryTheme = libraryTheme;

            Source = (resourceDictionary.Source?.ToString() ?? "Runtime").ToLower();
            Source = CultureInfo.InstalledUICulture.TextInfo.ToTitleCase(Source)
                                     .Replace("Pack", "pack")
                                     .Replace("Application", "application")
                                     .Replace("Xaml", "xaml");

            Key = key;

            Value = value switch
            {
                Color color => new SolidColorBrush(color),
                Brush brush => brush,
                _ => null
            };

            StringValue = value?.ToString();
        }

    }
}