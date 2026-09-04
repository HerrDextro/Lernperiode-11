using MorseMate_Mobile;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;

public partial class KeyLogic
{ 

    Stopwatch stopwatch = new Stopwatch();
    bool registerTime = false;

    int unitTimeInMs = 130;

    double pressedTimeInMs = 0;
    /*public async Task GetMorseKeyTime()
    {
        if (!registerTime)
        {
            stopwatch.Start();
            registerTime = true;

        }
        else if (registerTime)
        {
            stopwatch.Stop();
            TimeSpan elapsedTime = stopwatch.Elapsed;
            pressedTimeInMs = elapsedTime.TotalMilliseconds;
            stopwatch.Reset();
            registerTime = false;
        }

    }*/

    public string GetMorseCharacter1(int elapsedKeyPressTimeMS, TimeRecordState recordState)
    {
        //decide on morse timings from wpm setting...
        // dit is 1x, dash is 3x, space is 7x, One unit time is the time it takes to send a single dot. A dash is 3 unit times.
        //The space between symbols is 1 unit time, between letters is 3 unit times, and between words is 7 unit times.
        //WPM PARIS check later
        if (elapsedKeyPressTimeMS <= unitTimeInMs && recordState == TimeRecordState.RecordingInterKeySpace)
        {
            //dot
            return ".";
        }
        else if (elapsedKeyPressTimeMS > unitTimeInMs && elapsedKeyPressTimeMS <= 3 * unitTimeInMs && recordState == TimeRecordState.RecordingInterKeySpace)
        {
            //dash
            return "-";
        }
        else if (elapsedKeyPressTimeMS <= 3 * unitTimeInMs && elapsedKeyPressTimeMS >= unitTimeInMs &&recordState == TimeRecordState.RecordingKeyPress)
        {
            //space between letters
            return " ";
        }
        else if (elapsedKeyPressTimeMS > 3 * unitTimeInMs && elapsedKeyPressTimeMS <= 7 * unitTimeInMs && recordState == TimeRecordState.RecordingKeyPress)
        {
            //space
            return " / ";
        }
        else
        {
            return "";
        }
    }

    internal int DoParisCheck()
    {
        //paris would be .--. .- .-. .. ...
        //so thats 10 dots, 4 dashes, 9 units of space between symbols, 4x3 units of spaces between letters, plus space on trailing end is 7 units
        int totalUnits = 10 + (4 * 3) + 9 + (4 * 3) + 7;
        int totalMorseMs = totalUnits * unitTimeInMs;
        int MsInMin = 60 * 1000;
        int wpm = MsInMin / totalMorseMs;

        return wpm; //test using unit time of 130ms should return 9wpm
    }

    public string GetMorseCharacter2(int elapsedKeyPressTimeMS, TimeRecordState recordState) //change this to not return?
    {
        int unitTimeInMs = 130;
        //decide on morse timings from wpm setting...
        // dit is 1x, dash is 3x, space is 7x, One unit time is the time it takes to send a single dot. A dash is 3 unit times.
        //The space between symbols is 1 unit time, between letters is 3 unit times, and between words is 7 unit times.
        //WPM PARIS check later

        //new idea: short press is dot everything else is dash as long as state is recording press
        if (elapsedKeyPressTimeMS <= unitTimeInMs && recordState == TimeRecordState.RecordingKeyPress)
        {
            //dot
            return ".";
        }
        else if (elapsedKeyPressTimeMS > unitTimeInMs /*&& elapsedKeyPressTimeMS <= 3 * unitTimeInMs*/ && recordState == TimeRecordState.RecordingKeyPress)
        {
            //dash
            return "-";
        }
        else if (elapsedKeyPressTimeMS <= 3 * unitTimeInMs && elapsedKeyPressTimeMS >= 2.5 * unitTimeInMs && recordState == TimeRecordState.RecordingInterKeySpace)
        {
            //space between letters
            return " ";
        }
        else if (elapsedKeyPressTimeMS > 3 * unitTimeInMs && /*elapsedKeyPressTimeMS <= 7 * unitTimeInMs && */ recordState == TimeRecordState.RecordingInterKeySpace)
        {
            //space
            return " / ";
        }
        return null;
    }

    public enum MorseCharacterType //why even do this, Neo....
    {
        Dot,
        Dash,
        Space,
        Invalid
    }

    public enum TimeRecordState
    {
        RecordingKeyPress,
        RecordingInterKeySpace,
        Emtpty
    }

}
