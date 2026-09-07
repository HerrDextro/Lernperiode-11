using Plugin.Maui.Audio;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace MorseMate_Mobile;

public partial class Learn : ContentPage, INotifyPropertyChanged
{
	public Learn()
	{
		InitializeComponent();
        _morseUtilities = new MorseUtilities();
        InitAudioPlayer(); //works without this somehow?? 
    }
    public enum ColorHex
    {
        Normal = 0x808080, // Gray
        Blink = 0x00FF00  // Green
    }
    private MorseUtilities _morseUtilities;
    private IAudioPlayer _audioPlayer;
    private int _userFrequency = 607; // Default frequency in Hz
    public event PropertyChangedEventHandler? PropertyChanged;
    private CancellationTokenSource? _audioCts;
    //private CancellationToken _cancellationToken;
    protected override bool OnBackButtonPressed() //No AI for this btw //disposes but back button no longer works
    {
        _audioCts.Cancel();
        _audioCts.Dispose();
        _audioCts = null; //program needs to know its dead, if we kill it by dispose it leaves the audioplayer in zombie state
        return base.OnBackButtonPressed();// Indicate that the back button press has been handled
    }
    protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
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
    private string _levelTitle;
    public string LevelTitle
    {
        get { return _levelTitle; } 
        set 
        { 
            if(_levelTitle != value)
            {
                _levelTitle = value;
                OnPropertyChanged();
            }

        }
    }
    private ColorHex _blinkBoxColor;
    public ColorHex BlinkBoxColor
    {
        get => _blinkBoxColor;
        set
        {
            if (_blinkBoxColor != value)
            {
                _blinkBoxColor = value;
                OnPropertyChanged(nameof(BlinkBoxColor));
            }
        }
    }
    private void ShowHints(bool punctuation)
    {
        MorseLabel.IsVisible = !punctuation;
        CharLabel.IsVisible = !punctuation;
    }
    private async void OnPlayAudioBtnClicked(object sender, EventArgs e)
    {
        _audioCts?.Cancel();
        _audioCts?.Dispose();
        _audioCts = null;

        _audioCts = new CancellationTokenSource(); //first time ever using ts

        try
        {
            await _morseUtilities.PlayMorseAudio("Hello World", _audioCts.Token);
        }
        catch (TaskCanceledException)
        {
            // Expected exception when we stop audio early—safely ignore it! Nice
        }
    }
    private async void OnShowHintsBtnClicked(object sender, EventArgs e)
    {
        ShowHints(true);
        await Task.Delay(3000);
        ShowHints(false);
    }
}