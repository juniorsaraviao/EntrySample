using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace EntrySample;

public partial class MainPage : ContentPage, INotifyPropertyChanged
{
    bool canSetIsDirty;
    public bool IsDirty { get; set; }

    public MainPage()
	{
		InitializeComponent();
        BindingContext = this;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await Task.Delay(1500);

        Text = "Updated text in OnAppearing";
    }

    string text;
    public string Text
    {
        get => text;
        set => SetProperty(ref text, value);
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual bool SetProperty<T>(ref T backingStore, T value, [CallerMemberName] string propertyName = "", Action? onChanged = null, Func<T, T, bool>? validateValue = null)
    {
        if (EqualityComparer<T>.Default.Equals(backingStore, value))
        {
            return false;
        }

        if (validateValue != null && !validateValue!(backingStore, value))
        {
            return false;
        }

        backingStore = value;
        onChanged?.Invoke();
        OnPropertyChanged(propertyName);

        if (canSetIsDirty)
        {
            IsDirty = true;
            OnPropertyChanged(nameof(IsDirty));
        }

        return true;
    }

    protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = "")
    {
        this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}