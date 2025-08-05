INCLUDE Globals.ink

VAR idleAnimationValue = 0.0
VAR talkAnimationValue = 0.25
VAR audioValue = 4

->Phase_1
=== Phase_1 ===
    ~ talkAnimationValue = 0.25
    {HaveYouMetAlex: I see you met alex}
    ->Phase_2
->END

=== Phase_2 ===
* [Hi]
    ~ talkAnimationValue = 0.25
    ~ audioValue = 0
    Hello there, traveler
    ->Phase_3
* [And who are you?]
    ~ talkAnimationValue = 0.75
    ~ audioValue = 2
    I'm Rick, Just an Additional character for the demo
-> END

=== Phase_3 ===
    ~ talkAnimationValue = 0.25
* [Who are you?]
    ~ audioValue = 1
    I'm Rick
    ->DONE
* [....]
    ~ audioValue = 3
    I hope you have a good time
    ->DONE
-> END