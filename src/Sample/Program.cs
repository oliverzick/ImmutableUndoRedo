#region Copyright and license
// <copyright file="Program.cs" company="Oliver Zick">
//     Copyright (c) 2015 Oliver Zick. All rights reserved.
// </copyright>
// <author>Oliver Zick</author>
// <license>
//     Licensed under the Apache License, Version 2.0 (the "License");
//     you may not use this file except in compliance with the License.
//     You may obtain a copy of the License at
// 
//     http://www.apache.org/licenses/LICENSE-2.0
// 
//     Unless required by applicable law or agreed to in writing, software
//     distributed under the License is distributed on an "AS IS" BASIS,
//     WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//     See the License for the specific language governing permissions and
//     limitations under the License.
// </license>
#endregion

namespace ImmutableUndoRedo
{
    using System;

    class Program
    {
        static void Main(string[] args)
        {
            // Create history instance without any done or undone events
            var history = History.Create();

            // Current history:
            // Done:    (none)
            // Undone:  (none)

            // Perform 'Do' operation of event #1 and
            // use resulting event as next event to undo.
            // Empty undone events.
            history = history.Do(new Event(1));

            // Current history:
            // Done:    #1
            // Undone:  (none)

            // Perform 'Do' operation of event #2 and
            // use resulting event as next event to undo.
            // Empty undone events.
            history = history.Do(new Event(2));

            // Current history:
            // Done:    #2 -> #1
            // Undone:  (none)

            // Perform 'Do' operation of event #3 and
            // use resulting event as next event to undo.
            // Empty undone events.
            history = history.Do(new Event(3));

            // Current history:
            // Done:    #3 -> #2 -> #1
            // Undone:  (none)

            // Perform 'Undo' operation of event #3 and
            // use resulting event as next event to redo.
            history = history.Undo();

            // Current history:
            // Done:    #2 -> #1
            // Undone:  #3

            // Perform 'Undo' operation of event #2 and
            // use resulting event as next event to redo.
            history = history.Undo();

            // Current history:
            // Done:    #1
            // Undone:  #2 -> #3

            // Perform 'Do' operation of event #2 and
            // use resulting event as next event to undo.
            history = history.Redo();

            // Current history:
            // Done:    #2 -> #1
            // Undone:  #3

            // Perform 'Do' operation of event #4 and 
            // use resulting event as next event to undo.
            // Empties undone events.
            history = history.Do(new Event(4));

            // Current history:
            // Done:    #4 -> #2 -> #1
            // Undone:  (none)

            Console.ReadLine();
        }
    }
}
