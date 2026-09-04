using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;

namespace MorseMate_Mobile;
public enum TimeRecordState
{
    RecordingKeyPress,
    RecordingInterKeySpace,
    Empty
}

public partial class Practise : ContentPage, INotifyPropertyChanged
{
    public Practise()
    {
        InitializeComponent();
        translator = new MorseTextTranslator();
        BindingContext = this;
        Task.Run(IdleTimerLoop);
    }

    protected void OnTextChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
    public event PropertyChangedEventHandler? PropertyChanged;
    MorseTextTranslator translator;
    Stopwatch SpaceStopwatch = new();
    Stopwatch KeyStopWatch = new();
    private bool letterSpaceAdded = false;
    private bool wordSpaceAdded = false;
    private bool addWordSpace = false;
    private bool rmdPlaceHolder = false;
    private static int UnitTimeMs = 130;
    private int LetterSpaceMs = UnitTimeMs * 3; // 390ms
    private int WordSpaceMs = UnitTimeMs * 7;   // 910ms
    private TimeRecordState _recordState = TimeRecordState.Empty;
    public TimeRecordState RecordState
    {
        get => _recordState;
        set
        {
            if (_recordState != value)
            {
                _recordState = value;
                //OnStateChanged();
            }
        }
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
                OnTextChanged();
              
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
                OnTextChanged();
                PractiseTextOutput = translator.TranslateMorseToText(PractiseMorseOutput);
            }
        }
    }

    private async void OnBtnDown(object sender, EventArgs e) 
    {
        SpaceStopwatch.Stop();
        RecordState = TimeRecordState.RecordingKeyPress;
        if (rmdPlaceHolder == false)
        {
            PractiseMorseOutput = "";
            rmdPlaceHolder = true;
        }
        if (PractiseMorseOutput.Length == 0) //what is this && PractiseMorseOutput[^1] == ' '
        {
            addWordSpace = false;
        }
        if (addWordSpace && PractiseMorseOutput.Length > 0) //avoids a / in empty textbox after prev. text manual delete
        {
            PractiseMorseOutput += " / ";
            addWordSpace = false;
        }
        PractiseMorseOutput += ".";
        KeyStopWatch.Restart();

        letterSpaceAdded = false;
        wordSpaceAdded = false;

        await Task.Delay(UnitTimeMs);

        if (RecordState == TimeRecordState.RecordingKeyPress && PractiseMorseOutput.Length > 0)
        {
            PractiseMorseOutput = PractiseMorseOutput.Remove(PractiseMorseOutput.Length - 1) + "-";
        }
    }

    private async void OnBtnUp(object sender, EventArgs e)
    {
        KeyStopWatch.Stop();
        RecordState = TimeRecordState.RecordingInterKeySpace;
        SpaceStopwatch.Restart();

    }

    public async Task IdleTimerLoop()
    {
        while (true)
        {
            await Task.Delay(10);
            
            if (RecordState == TimeRecordState.RecordingInterKeySpace)
            {
                long elapsed = SpaceStopwatch.ElapsedMilliseconds;

                if (elapsed >= LetterSpaceMs && elapsed < WordSpaceMs && !letterSpaceAdded)
                {
                    PractiseMorseOutput += " ";
                    letterSpaceAdded = true;
                }
                else if (elapsed >= WordSpaceMs && !wordSpaceAdded) //never ever triggers somehow
                {
                    addWordSpace = true;
                    wordSpaceAdded = true;
                    SpaceStopwatch.Stop();
                    RecordState = TimeRecordState.Empty;
                }

            }
        }
    }
}