using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace MorseMate_Mobile;

public partial class Settings : ContentPage, INotifyPropertyChanged
{
	public Settings()
	{
		InitializeComponent();
        BindingContext = this;
    }
    // Backing event required by INotifyPropertyChanged so the class can raise PropertyChanged.
    public event PropertyChangedEventHandler? PropertyChanged;
    protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}