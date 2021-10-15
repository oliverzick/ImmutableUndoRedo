# ImmutableUndoRedo library
Copyright (c) 2015-2021 Oliver Zick. All rights reserved.

---

## Version 2.1.0:

* #15: Migrate to .NET Standard 2.0
* #17: Include LICENSE file in packages
* #19: Transform CHANGELOG into markdown

## Version 2.0.1:

* #14: Improve naming of History class's generic type parameter, rename to TActivity

## Version 2.0.0:

* #9: Enable History class to be specialized with a custom implementation of the IActivity interface
* #11: Remove obsolete History.CreateEmpty method
* #13: Rename IEvent interface to IActivity

## Version 1.2.1:

* #7: Enable NuGet package to support .NET Framework version 4.6
* #8: Enable NuGet package to support .NET Framework version 4.5.2
* #12: Correct output folders of .NET 4.5.1 project regarding debug and release build

## Version 1.2.0:

* #2: Enable History to tell done and undone events
* #4: Drop support for .NET Framework version 3.5
* #5: Remove .NET Framework 3.5 files from NuGet package
* #6: Improve change history by adding issue numbers
* #10: Unit tests for CopyXXXTo methods of History now assert for correct order

## Version 1.1.1:

* #3: Enable NuGet package to support .NET Framework versions 3.5, 4.0, 4.5 and 4.5.1

## Version 1.1.0:

* #1: Enable History class to clear done and undone events

## Version 1.0.1:

* Exclude PDB file from NuGet package

## Version 1.0.0:

* Add implementation of history that supports do, undo and redo operations
* Add IEvent interface that represents an event and supports do and undo operations
