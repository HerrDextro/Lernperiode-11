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

        private string _textToTranslate = string.Empty;
        private string _translationOutput = string.Empty;
        public string TextToTranslate
        {
            get => _textToTranslate;
            set
            {
                if (_textToTranslate != value)
                {
                    _textToTranslate = value;
                    OnPropertyChanged();

                    if (DetectMorse(_textToTranslate))
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
                if (_translationOutput != value)
                {
                    _translationOutput = value;
                    OnPropertyChanged();
                }
            }
        }

        public bool DetectMorse(string input)
        {
            string lowerInput = input.ToLower();
            char[] inputArray = lowerInput.ToArray();

            //check if the input contains only dots, dashes, and spaces (Morse code)
            bool isMorseCode = inputArray.All(C => C == '.' || C == '-' || C == ' ');

            return isMorseCode;
        }

        //redefining the translation rules: morse words will now be separated by a / and regular text spaces will appear as / between morse words

        public string TranslateMorseToText(string morseInput) //input looks like --. / .. -- //spaces not yet functioning
        {
            StringBuilder morseBuilder = new StringBuilder();

            string[] morseWords = morseInput.Split('/'); //now looks lîke --. and  .. -- (with the whitespaces still around them
            string[] morseChars = [];
            foreach (string word in morseWords)
            {
                morseChars = word.Split(' ');
                morseBuilder.Append(" "); //add a space between words

                foreach (string morseChar in morseChars)
                {
                    if (_morseCodeMap.ContainsValue(morseChar))
                    {
                        morseBuilder.Append(_morseCodeMap.FirstOrDefault(x => x.Value == morseChar).Key);
                    }
                }
            }

            

            return morseBuilder.ToString().Trim();
        }
        public string TranslateTextToMorse(string textInput)
        {
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
                    morseBuilder.Append("  "); // Add extra space for word separation
                }
            }
            return morseBuilder.ToString().Trim();
        }
    }
}
