# Lernperiode 11

14.8 bis 11.9.2024

## Planning

The Idea: A Morse learning app that you can practise individual letters with, and also transmit or decode signals.

Technical requirements:
- Storage: user preferences API und FS API
- Outputs: Audio API, haptic feedback, flashlight

The challenge will lie in the correct use of these APIs and compatibility with different phones.
Non technical aspects: I want to try and design a highly ergonomic app that would never frustrate a user.

How is this project different from M335? I cant know yet, since we are not far enough in 335. I will use MAUI here, so if the option comes in M335 I will use Avalonia there.

## 14.8

Planned features:
- [x] As the user I want to be able to view a morse alphabet to quickly see what a character is
- [x] As the user I want to be able to translate morse into text and vice versa to quickly see what a string of morse means, or what a string of text would be in morse
- [ ] As the user I want to be able to practise decoding morse characters
- [ ] As the user I want to be able to practise decoding morse words/sentences
- [x] As the user I want to be able to use the screen to practise keying morse characters
- [x] As the user I want to be able to use the screen to key morse words/sentences
- [x] As the user I want to have an auditory feedback when keying Morse so that I can learn to decode by sound
- [x] As the user I want to be able to set the WPM speed and save it to customize the practising
- [ ] As the user I want to have an accessible Q-Code list to understand which is appropriate for the situation

Additional feature ideas:
- [ ] As the user I want to be able to play an audio of the typed morse to be able to practise recognising my text
- [ ] As the user I want to be able to see my WPM progress in decoding morse to see how much I have improved
- [ ] As the user I want to be able to see my WPM progress in keying morse to see how much I have improved

Goals 14.08.2026
- [x] Create a .NET MAUI project
- [x] Research MAUI and learn how to bind buttons and add pages
- [x] Create UI layout drawing

Today I planned out the app and all the features I want. I decided to keep most of the features optional (as in: not dependent on each other) so I can get the essentials working within the 5 week timeframe. I installed the MAUI workload onto my VS and started a project, with which I played around a bit trying to understand how it works and what were dealing with, this means just changing some buttons, adding new pages, trying out paddings and layouts. Because my phone broke, I will not be adding a photo of the sketch yet.
Also: No code is committed, since at the closing time it did not compile


## 21.8

Goals 21.08.26
- [x] As the user I want to be able to view a morse alphabet to quickly see what a character is
- [x] As the user I want to be able to translate morse into text and vice versa to quickly see what a string of morse means, or what a string of text would be in morse

(No third user story, I wouldnt get it finished)

Today I did a crash course on MAUI XAML, mainly learning the types of basic layout like <VerticalLayout> and <HorizontalLayout> and elements like <label> and <editor>. Then I wrote the boilerplate needed to connect a "backend" class to the UI by binding its properties to elements in the XAML. Once that was done I started writing simple Text to Morse and vice versa methods. I first pass the text into a morse detector that checks if the string consists of only dot dash and space and returns a true if yes. Then according to that bool the correct translation method is called and given the input string. The code works nicely, except that I didnt know what official good practise was for written morse delimiting, which turns out to be a / in between words. The code doesnt implement this yet though.

Note: after some more effort outside of school time the morse slash spaces and two way translation work well. Now its possible to input text/morse in the top bar and edit the result to retranslate it in reverse, which allowes for fixing mistakes in an existing text.



https://github.com/user-attachments/assets/345c945f-7664-4de3-8e28-6a7d93b18341




## 28.8

Goals 28.08.2026
- [x] As the user I want to be able to use the screen to practise keying morse characters
- [x] As the user I want to be able to use the screen to key morse words/sentences 
- [x] As the user I want to be able to set the WPM speed and save it to customize the practising

Today I added a page with 2 text outputs and a button, which allows you to practise keying morse and getting the timings right. For this I created a class containing logic for determining what press timings are what morse characters, and also calculating WPM so the user knows exactly what their WPM is set to. I had to do some research as to how the timings work exactly and also how to calculate the WPM, where I settled on the "PARIS check" which is just using whatever morse unit time is set to key the word "paris" with a trailing space. It took me alot of time to figure out how exactly to get working key pressed and released events in MAUI, since there were several ways and the NuGet I needed for the previous method I was using had alot of issues. As of right now, the key functionality does work but is not very smooth at all, and the characters dont really behave leading me to believe there must be a bug with the timing logic or the timing to morse method. I also did implement the wpm functionality but I didnt add any settings for that yet on the settings page.

Note: turns out I forgot like the most important thing about the code and ignored it when I decided to change the structure. Yes, its bugged.
I fixed it by adding an async timer for the space timings, and made some small functional changes to make the experience smoother.

## Goals 04.09

Goals 04.09
- [x] As the user I want to have an auditory feedback when keying Morse so that I can learn to decode by sound
- [x] As the user I want to be able to set the WPM speed and save it to customize the practising
- [ ] As the user I want to be able to practise decoding morse characters
(Note: the decode feature required extensive research on how to structure morse lessons, and is also a very big feature to implement

Today I started working on the learning (decoding) features for morse. First I had to do research about how morse can be learned, and then I had to make a bunch of decisions and draft up what the experience should be like. This took quite some time since ive learnt to really define it in detail before coding (otherwise we get a situation like the previous translation feature or even worse: the keying feature) I went ahead and added a page called "Learn" and let AI make a JSON containing the levels which look like this:
```
{
  "LevelNumber": 1,
  "Title": "Opposed Pair: E & T",
  "Description": "Learn the two fundamental Morse elements: a single dot and a single dash.",
  "Characters": {
    "E": ".",
    "T": "-"
  }
}
```
The two main learning methods, "Opposed" and "Koch" are the ones im going to use, I will later add a toggle to choose where it recommends the Koch method, however for now ill use the opposed method since its how I learned. 




https://github.com/user-attachments/assets/ca8023d9-6663-4552-9d98-f7bb7684f8b1


# 11.09

Goals 11.09
- [ ] As the user I want to be able to practise decoding morse words/sentences
- [ ] As the user I want to have an accessible Q-Code list to understand which is appropriate for the situation
(Note: the first is a very big thing to implement, so I might only get that done)
