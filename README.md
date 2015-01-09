KeyStates
=========

A Keyboard State Watcher for C# and .NET

##Usage:##

`using KeyStates;`

There are two KeyboardMonitor classes, ActiveKeyboardMonitor and PassiveKeyboardMonitor. The Active monitor constantly polls the system for keypresses, while the Passive monitor hooks in to the system message loop, but cannot be used in console apps.

You can subscribe to any of these events:
* `xxxKeyboardMonitor.KeyDown`, fired when any key is depressed.
* `xxxKeyboardMonitor.KeyUp`, fired when any key is released.
* `xxxKeyboardMonitor.KeyPressed`, fired when an alpha-numeric or symbol key is released.

Begin monitoring for keyboard changes with `xxxKeyboardMonitor.Start()` and end with `xxxKeyboardMonitor.Stop()`. When using the ActiveKeyboardMonitor, you can also change the polling frequency with `ActiveKeyboardMonitor.Interval = <double milliseconds>;`.
