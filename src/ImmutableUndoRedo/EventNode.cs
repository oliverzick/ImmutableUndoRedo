#region Copyright and license
// <copyright file="EventNode.cs" company="Oliver Zick">
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

    internal sealed class EventNode : IEventNode, IEquatable<EventNode>
    {
        private readonly IEvent @event;

        private readonly IEventNode next;

        public EventNode(IEvent @event, IEventNode next)
        {
            this.@event = @event;
            this.next = next;
        }

        public static bool operator ==(EventNode left, EventNode right)
        {
            return object.Equals(left, right);
        }

        public static bool operator !=(EventNode left, EventNode right)
        {
            return !(left == right);
        }

        public override bool Equals(object obj)
        {
            return this.Equals(obj as EventNode);
        }

        public bool Equals(EventNode other)
        {
            if (object.ReferenceEquals(other, null))
            {
                return false;
            }

            if (object.ReferenceEquals(this, other))
            {
                return true;
            }

            return object.Equals(this.@event, other.@event)
                   && object.Equals(this.next, other.next);
        }

        public override int GetHashCode()
        {
            return Hash.Calculate(this.@event.GetHashCode(), this.next.GetHashCode());
        }

        public IEventNode Concat(IEventNode newNext)
        {
            return new EventNode(this.@event, newNext);
        }

        public IEventNode Do()
        {
            return new EventNode(this.@event.Do(), this.next);
        }

        public IEventNode Next()
        {
            return this.next;
        }

        public IEventNode Undo()
        {
            return new EventNode(this.@event.Undo(), this.next);
        }
    }
}