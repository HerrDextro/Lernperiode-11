using Plugin.Maui.Audio;
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
        InitAudioPlayer();
        Task.Run(IdleTimerLoop);
    }

    protected void OnTextChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
    private void InitAudioPlayer()
    {
        // 1. Generate 1-second sine wave stream at user's desired Hz
        var audioStream = AudioGenerator.GenerateSineWaveStream(_userFrequency, durationMs: 1000);

        // 2. Load into MAUI Audio Player
        _audioPlayer = AudioManager.Current.CreatePlayer(audioStream);
        _audioPlayer.Loop = true; // Set infinite looping
    }
    private IAudioPlayer? _audioPlayer;
    private double _userFrequency = 600.0; // User setting in Hz
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

        // Start tone
        if (_audioPlayer != null && !_audioPlayer.IsPlaying)
        {
            _audioPlayer.Play();
        }

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
        // Stop tone instantly
        if (_audioPlayer != null && _audioPlayer.IsPlaying)
        {
            _audioPlayer.Pause();
        }

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