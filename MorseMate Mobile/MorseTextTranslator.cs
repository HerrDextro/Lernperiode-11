using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Linq;
//using static Android.Text.Style.TtsSpan;

namespace MorseMate_Mobile
{
    public class MorseTextTranslator : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        private Dictionary<char, string> _morseCodeMap = new Dictionary<char, string>
            {
                { 'A', ".-" }, { 'B', "-..." }, { 'C', "-.-." }, { 'D', "-.." },
                { 'E', "." }, { 'F', "..-." }, { 'G', "--." }, { 'H', "...." },
                { 'I', ".." }, { 'J', ".---" }, { 'K', "-.-" }, { 'L', ".-.." },
                { 'M', "--" }, { 'N', "-." }, { 'O', "---" }, { 'P', ".--." },
                { 'Q', "--.-" }, { 'R', ".-." }, { 'S', "..." }, { 'T', "-" },
                { 'U', "..-" }, { 'V', "...-" }, { 'W', ".--" }, { 'X', "-..-" },
                { 'Y', "-.--" }, { 'Z', "--.." },
                { '0', "-----" }, { '1', ".----" }, { '2', "..---" },
                { '3', "...--" }, { '4', "....-" }, { '5', "....." },
                { '6', "-...." }, { '7', "--..." }, { '8', "---.." },
                { '9', "----." },
                // Add more mappings for punctuation and special characters if needed (or be cool and make the binary tree thing)
            };

        private TranslationDirection _translationDirection;
        private bool _supressRecursion = false;
        private string _textToTranslate = string.Empty;
        private string _translationOutput = string.Empty;
        public string TextToTranslate
        {
            get => _textToTranslate;
            set
            {
                if (_textToTranslate != value)
                {
                    _textToTranslate = value.ToUpper();
                    OnPropertyChanged();

                    DetectMorse(_textToTranslate);
                    if (_translationDirection == TranslationDirection.MorseToText)
                    {
                        TranslationOutput = TranslateMorseToText(_textToTranslate);
                    }
                    else
                    {
                        TranslationOutput = TranslateTextToMorse(_textToTranslate);
                    }
                }
            }
        }
        public string TranslationOutput
        {
            get => _translationOutput;
            set
            {
                if (_translationOutput != value.ToUpper())
                {
                    _translationOutput = value.ToUpper();
                    OnPropertyChanged();
                    DetectMorse(TranslationOutput); //update this to also adhere to non recursion flag
                    RewriteInputAfterOutputChange(); //or keep flag in if statement and put these two in, but thats even less safe
                    _supressRecursion = false;
                }
            }
        }

        public void DetectMorse(string input)
        {
            string lowerInput = input.ToLower();
            char[] inputArray = lowerInput.ToArray();

            //check if the input contains only dots, dashes, and spaces (Morse code)
            bool isMorseCode = inputArray.All(C => C == '.' || C == '-' || C == ' ' || C == '/');
            if (isMorseCode)
            {
                _translationDirection = TranslationDirection.MorseToText;

            }
            else
            {
                _translationDirection = TranslationDirection.TextToMorse;
            }
        }

        public void RewriteInputAfterOutputChange() //works but editors text does not update
        {
            if (_translationDirection == TranslationDirection.MorseToText && !_supressRecursion)
            {
                _textToTranslate = TranslateMorseToText(_translationOutput);
                OnPropertyChanged(nameof(TextToTranslate));
            }
            else if (_translationDirection == TranslationDirection.TextToMorse && !_supressRecursion)
            {
                _textToTranslate = TranslateTextToMorse(_translationOutput); //doesnt trigger property change for some reason
                OnPropertyChanged(nameof(TextToTranslate));    
            }
        }

        //redefining the translation rules: morse words will now be separated by a / and regular text spaces will appear as / between morse words

        public string TranslateMorseToText(string morseInput) //input looks like --. / .. -- //spaces not yet functioning
        {
            _supressRecursion = true;
            StringBuilder morseBuilder = new StringBuilder();

            string[] morseChars = morseInput.Split(' ');
            foreach (string morseChar in morseChars)
            {
                if (_morseCodeMap.ContainsValue(morseChar))
                {
                    morseBuilder.Append(_morseCodeMap.FirstOrDefault(x => x.Value == morseChar).Key);
                }
                else if(morseChar == "/")
                {
                    morseBuilder.Append(" "); // Add a space for word separation
                }
            }
            return morseBuilder.ToString();
        }
        public string TranslateTextToMorse(string textInput)
        {
            _supressRecursion = true;
            // Convert the text to uppercase for consistency
            textInput = textInput.ToUpper();
            // Create a dictionary to map characters to Morse code
            
            StringBuilder morseBuilder = new StringBuilder();
            foreach (char c in textInput)
            {
                if (_morseCodeMap.ContainsKey(c))
                {
                    morseBuilder.Append(_morseCodeMap[c] + " ");
                }
                else if (c == ' ')
                {
                    morseBuilder.Append(" / "); // Add slash for word separation
                }
            }
            return morseBuilder.ToString().Trim();
        }
    }

    enum TranslationDirection
    {
        TextToMorse,
        MorseToText
    }
}
