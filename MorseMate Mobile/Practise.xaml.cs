using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;

namespace MorseMate_Mobile;

public partial class Practise : ContentPage, INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;
    protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
    public Practise()
    {
        InitializeComponent();
        keyLogic = new KeyLogic();
        translator = new MorseTextTranslator();
        stopwatch = new Stopwatch();
        BindingContext = this;
    }
    private string _practiseTextOutput;
    private string _practiseMorseOutput;

    public string PractiseTextOutput
    {
        get => _practiseTextOutput;
        set
        {
            if (_practiseTextOutput != value) //does this work with the default null placeholder defined in the XAML?
            {
                _practiseTextOutput = value;
                OnPropertyChanged();
            }
        }
    }
    public string PractiseMorseOutput
    {
        get => _practiseMorseOutput;
        set
        {
            if (_practiseMorseOutput != value)
            {
                _practiseMorseOutput = value;
                OnPropertyChanged();
            }
        }
    }

    KeyLogic keyLogic;
    MorseTextTranslator translator;
    Stopwatch stopwatch;
    private int pressedTimeInMs = 0;
    private KeyLogic.MorseCharacterType morseCharacterType;

    private async void OnBtnDown(object sender, EventArgs e)
    {
        //keyLogic.GetMorseKeyTime();
        Debug.WriteLine($"Pressed key");
        stopwatch.Reset();
        stopwatch.Start();
    }

    private async void OnBtnUp(object sender, EventArgs e)
    {
        //keyLogic.GetMorseKeyTime();
        Debug.WriteLine($"Released key");
        stopwatch.Stop();
        TimeSpan elapsedTime = stopwatch.Elapsed;
        pressedTimeInMs = (int)elapsedTime.TotalMilliseconds;
        stopwatch.Reset();
    }

    private async void OnButtonClicked(object sender, EventArgs e)
    {
        Debug.WriteLine($"Button clicked");
        //await DisplayAlert("Alert", "You clicked the button!", "OK");
        morseCharacterType = keyLogic.GetMorseCharacterType(pressedTimeInMs);
        WriteOutputToUI(morseCharacterType);
    }

    private void WriteOutputToUI(KeyLogic.MorseCharacterType characterType)
    {
        if (characterType == KeyLogic.MorseCharacterType.Dot)
        {
            PractiseMorseOutput += ".";
        }
        else if (characterType == KeyLogic.MorseCharacterType.Dash)
        {
            PractiseMorseOutput += "-";
        }
        else if (characterType == KeyLogic.MorseCharacterType.Invalid)
        {
            PractiseMorseOutput += "Invalid input";
        }
        else if (characterType == KeyLogic.MorseCharacterType.Space)
        {
            PractiseMorseOutput += " / ";
        }

        PractiseTextOutput = translator.TranslateMorseToText(PractiseMorseOutput);
    }
}