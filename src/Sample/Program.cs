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
    using System.Collections.Generic;

    class Program
    {
        static void Main(string[] args)
        {
            var history = History.Create()
                                 .Do(new Event(1))
                                 .Do(new Event(2))
                                 .Do(new Event(3))
                                 .Do(new Event(4))
                                 .Do(new Event(5))
                                 .Undo()
                                 .Undo()
                                 .Undo()
                                 .Redo();

            Console.WriteLine("\n\nDone events:");
            List<IEvent> doneEvents = new List<IEvent>();
            history.CopyDoneTo(doneEvents);
            doneEvents.ForEach(Console.WriteLine);

            Console.WriteLine("\n\nUndone events:");
            List<IEvent> undoneEvents = new List<IEvent>();
            history.CopyUndoneTo(undoneEvents);
            undoneEvents.ForEach(Console.WriteLine);

            Console.ReadLine();
        }
    }
}
