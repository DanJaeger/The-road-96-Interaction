INCLUDE Globals.ink

VAR talkAnimationValue = 0
VAR audioValue = 4

->Phase_1
=== Phase_1 ===
    ~ talkAnimationValue = 0
{HaveYouMetAlex: I see you met alex}
* [Hi]
    ~ audioValue = 0
    Hello there, traveler
    ->Phase_2
* [And who are you?]
    ~ audioValue = 1
    I'm Rick 
    Just an Additional character for the demo
    ->DONE
-> END

=== Phase_2 ===
* [Who are you?]
    ~ audioValue = 1
    I'm Rick
    ->DONE
* [....]
    ~ audioValue = 3
    I hope you're having a good time
    ->DONE
-> END