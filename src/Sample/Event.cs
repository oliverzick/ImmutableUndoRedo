#region Copyright and license
// <copyright file="Event.cs" company="Oliver Zick">
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

namespace ImmutablUndoRedo
{
    using System;
    using ImmutableUndoRedo;

    struct Event : IEvent
    {
        private readonly int id;

        private readonly int totalDo;

        private readonly int totalUndo;

        public Event(int id)
            : this()
        {
            this.id = id;
        }

        private Event(int id, int totalDo, int totalUndo)
            : this(id)
        {
            this.totalDo = totalDo;
            this.totalUndo = totalUndo;
        }

        public IEvent Do()
        {
            Console.WriteLine("Do   event #{0} (total 'Do': {1} | total 'Undo': {2})", this.id, this.totalDo + 1, this.totalUndo);

            return new Event(this.id, this.totalDo + 1, this.totalUndo);
        }

        public IEvent Undo()
        {
            Console.WriteLine("Undo event #{0} (total 'Do': {1} | total 'Undo': {2})", this.id, this.totalDo, this.totalUndo + 1);

            return new Event(this.id, this.totalDo, this.totalUndo + 1);
        }
    }
}