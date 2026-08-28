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

    public MorseCharacterType GetMorseCharacterType(int elapsedTimeInMs)
    {
        //decide on morse timings from wpm setting...
        // dit is 1x, dash is 3x, space is 7x, One unit time is the time it takes to send a single dot. A dash is 3 unit times.
        //The space between symbols is 1 unit time, between letters is 3 unit times, and between words is 7 unit times.
        //WPM PARIS check later
        if (elapsedTimeInMs <= unitTimeInMs)
        {
            //dot
            return MorseCharacterType.Dot;
        }
        else if (elapsedTimeInMs > unitTimeInMs && elapsedTimeInMs <= 3 * unitTimeInMs)
        {
            //dash
            return MorseCharacterType.Dash;
        }
        else if (elapsedTimeInMs > 3 * unitTimeInMs && elapsedTimeInMs <= 7 * unitTimeInMs)
        {
            //space
            return MorseCharacterType.Dash;
        }
        else
        {
            //invalid input
            return MorseCharacterType.Invalid;
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

    public enum MorseCharacterType
    {
        Dot,
        Dash,
        Space,
        Invalid
    }

}
