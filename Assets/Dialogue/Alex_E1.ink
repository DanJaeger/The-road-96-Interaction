INCLUDE Globals.ink

VAR idleAnimationValue = 0.0
VAR talkAnimationValue = 0.25
VAR audioValue = 0

-> Phase_1

=== Phase_1 ===
* [You look sad]
    ~ audioValue = 0
    I am
    ->Phase_2
* [What's wrong?]
    ~ audioValue = 1
    This is just a prototype
    ->Phase_3
-> END

=== Phase_2 ===
    ~ talkAnimationValue = 0.5
* [Why?]
    ~ audioValue = 1
    This is just a prototype
    ->Phase_3
* [....]
    ~ audioValue = 1
    This is just a prototype
    ->Phase_3
-> END

=== Phase_3 ===
    ~ talkAnimationValue = 0.75
    ~ HaveYouMetAlex = true
* [What does that mean?]
    ~ audioValue = 2
    It means this project is for learning purposes only
    ->DONE
* [So?]
    ~ audioValue = 3
    You will never see me out of this project
    ->DONE
    ~ idleAnimationValue = 0.0
-> END
