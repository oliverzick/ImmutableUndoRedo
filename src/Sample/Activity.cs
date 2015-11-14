#region Copyright and license
// <copyright file="Activity.cs" company="Oliver Zick">
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

    internal struct Activity : IActivity
    {
        private readonly int id;

        private readonly int doCalls;

        private readonly int undoCalls;

        public Activity(int id)
            : this()
        {
            this.id = id;
        }

        private Activity(int id, int doCalls, int undoCalls)
            : this(id)
        {
            this.doCalls = doCalls;
            this.undoCalls = undoCalls;
        }

        public IActivity Do()
        {
            Console.WriteLine("Do activity #{0}", this.id);

            return new Activity(this.id, this.doCalls + 1, this.undoCalls);
        }

        public IActivity Undo()
        {
            Console.WriteLine("Undo activity #{0}", this.id);

            return new Activity(this.id, this.doCalls, this.undoCalls + 1);
        }

        public override string ToString()
        {
            return $"Activity {{Id = {this.id}, #Do calls = {this.doCalls}, #Undo calls = {this.undoCalls}}}";
        }
    }
}