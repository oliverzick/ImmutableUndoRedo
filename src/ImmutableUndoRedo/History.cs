#region Copyright and license
// <copyright file="History.cs" company="Oliver Zick">
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

    /// <summary>
    /// Represents a history of events that supports doing, undoing and redoing of events.
    /// </summary>
    /// <remarks>
    /// This class is implemented as immutable object with value semantics.
    /// </remarks>
    public sealed class History : IEquatable<History>
    {
        private readonly IEventNode done;

        private readonly IEventNode undone;

        /// <summary>
        /// Initializes a new instance of the <see cref="History"/> class
        /// with the specified <paramref name="done"/> and <paramref name="undone"/> events.
        /// </summary>
        /// <param name="done">The done events.</param>
        /// <param name="undone">The undone events.</param>
        internal History(IEventNode done, IEventNode undone)
        {
            this.done = done;
            this.undone = undone;
        }

        /// <summary>
        /// Determines whether two specified histories represent the same history of events.
        /// </summary>
        /// <param name="left">The first history to compare.</param>
        /// <param name="right">The second history to compare.</param>
        /// <returns>
        /// <c>true</c> if the <paramref name="left"/> history represents the same history as the <paramref name="right"/> history; otherwise, <c>false</c>.
        /// </returns>
        public static bool operator ==(History left, History right)
        {
            return object.Equals(left, right);
        }

        /// <summary>
        /// Determines whether two specified histories do not represent the same history of events.
        /// </summary>
        /// <param name="left">The first history to compare.</param>
        /// <param name="right">The second history to compare.</param>
        /// <returns>
        /// <c>true</c> if the <paramref name="left"/> history does not represent the same history as the <paramref name="right"/> history; otherwise, <c>false</c>.
        /// </returns>
        public static bool operator !=(History left, History right)
        {
            return !(left == right);
        }

        /// <summary>
        /// Creates a new history instance that is empty and contains neither done nor undone events.
        /// </summary>
        /// <returns>
        /// A new history instance that contains neither done nor undone events.
        /// </returns>
        public static History CreateEmpty()
        {
            return new History(
                new NullEventNode(), 
                new NullEventNode());
        }

        /// <summary>
        /// Determines whether the specified <see cref="System.Object" /> is equal to this instance.
        /// </summary>
        /// <param name="obj">The <see cref="System.Object" /> to compare with this instance.</param>
        /// <returns>
        /// <c>true</c> if the specified <see cref="System.Object" /> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        public override bool Equals(object obj)
        {
            return this.Equals(obj as History);
        }

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns>
        /// <c>true</c> if the current object is equal to the <paramref name="other" /> parameter; otherwise, <c>false</c>.
        /// </returns>
        public bool Equals(History other)
        {
            if (object.ReferenceEquals(other, null))
            {
                return false;
            }

            if (object.ReferenceEquals(this, other))
            {
                return true;
            }

            return object.Equals(this.done, other.done)
                   && object.Equals(this.undone, other.undone);
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
        /// </returns>
        public override int GetHashCode()
        {
            return Hash.Calculate(this.done.GetHashCode(), this.undone.GetHashCode());
        }

        /// <summary>
        /// Returns a new history after performing the 'Do' operation of the specified <paramref name="event"/>
        /// that contains the result of this operation as the next event to undo
        /// without any events to redo.
        /// </summary>
        /// <returns>
        /// A new history that contains the resulting event of the 'Do' operation
        /// of the specified <paramref name="event"/> as next event to undo
        /// without any events to redo.
        /// </returns>
        public History Do(IEvent @event)
        {
            return new History(
                new EventNode(@event, this.done).Do(),
                new NullEventNode());
        }

        /// <summary>
        /// Returns a new history after performing the 'Undo' operation of the next event to undo
        /// that contains the result of this operation as next event to redo.
        /// </summary>
        /// <returns>
        /// A new history that contains the resulting event of the 'Undo' operation
        /// of the next event to undo as next event to redo.
        /// </returns>
        public History Undo()
        {
            return new History(
                this.done.Next(),
                this.done.Undo().Concat(this.undone));
        }

        /// <summary>
        /// Returns a new history after performing the 'Do' operation of the next event to redo
        /// that contains the result of this operation as next event to undo.
        /// </summary>
        /// <returns>
        /// A new history that contains the resulting event of the 'Do' operation
        /// of the next event to redo as next event to undo.
        /// </returns>
        public History Redo()
        {
            return new History(
                this.undone.Do().Concat(this.done),
                this.undone.Next());
        }
    }
}
