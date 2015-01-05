KeyStates
=========

A Keyboard State Watcher for C# and .NET

##Usage:##

`using KeyStates;`

You can subscribe to any of these events:
* `KeyboardMonitor.KeyDown`, fired when any key is depressed.
* `KeyboardMonitor.KeyUp`, fired when any key is released.
* `KeyboardMonitor.KeyPressed`, fired when an alpha-numeric or symbol key is released.

Begin monitoring for keyboard changes with `KeyboardMonitor.Start()` and end with `KeyboardMonitor.Stop()`. You can also change the polling frequency with `KeyboardMonitor.Interval = <double milliseconds>;`.
