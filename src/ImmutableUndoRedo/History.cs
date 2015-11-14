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
    using System.Collections.Generic;

    /// <summary>
    /// Represents a history of activities that supports doing, undoing and redoing of activities.
    /// </summary>
    /// <remarks>
    /// This class is implemented as immutable object with value semantics.
    /// </remarks>
    public sealed class History : IEquatable<History>, IContentEquatable<History>
    {
        private readonly Timeline<IActivity> done;

        private readonly Timeline<IActivity> undone;

        private History(Timeline<IActivity> done, Timeline<IActivity> undone)
        {
            this.done = done;
            this.undone = undone;
        }

        /// <summary>
        /// Determines whether two specified histories have the same value.
        /// </summary>
        /// <param name="left">The first history to compare.</param>
        /// <param name="right">The second history to compare.</param>
        /// <returns>
        /// <c>true</c> if the value of <paramref name="left"/> is the same as the value of <paramref name="right"/>; otherwise, <c>false</c>.
        /// </returns>
        public static bool operator ==(History left, History right)
        {
            return object.Equals(left, right);
        }

        /// <summary>
        /// Determines whether two specified histories have different values.
        /// </summary>
        /// <param name="left">The first history to compare.</param>
        /// <param name="right">The second history to compare.</param>
        /// <returns>
        /// <c>true</c> if the value of <paramref name="left"/> is different from the value of <paramref name="right"/>; otherwise, <c>false</c>.
        /// </returns>
        public static bool operator !=(History left, History right)
        {
            return !(left == right);
        }

        /// <summary>
        /// Creates a <see cref="History"/> that contains neither done nor undone activities.
        /// </summary>
        /// <returns>
        /// A <see cref="History"/> that contains neither done nor undone activities.
        /// </returns>
        public static History Create()
        {
            return new History(
                Timeline<IActivity>.Empty(),
                Timeline<IActivity>.Empty());
        }

        /// <summary>
        /// Creates a <see cref="History"/> that contains the specified
        /// <paramref name="done"/> and <paramref name="undone"/> activities.
        /// </summary>
        /// <param name="done">
        /// The done activities given in chronological order.
        /// </param>
        /// <param name="undone">
        /// The undone activities given in chronological order.
        /// </param>
        /// <returns>
        /// A <see cref="History"/> that contains the specified
        /// <paramref name="done"/> and <paramref name="undone"/> activities.
        /// </returns>
        public static History Create(IEnumerable<IActivity> done, IEnumerable<IActivity> undone)
        {
            return new History(
                Timeline<IActivity>.Create(done),
                Timeline<IActivity>.Create(undone));
        }

        /// <summary>
        /// Determines whether this instance and a specified object, which must also be a <see cref="History"/>, have the same value.
        /// </summary>
        /// <param name="obj">The <see cref="object" /> to compare to this instance.</param>
        /// <returns>
        /// <c>true</c> if <paramref name="obj"/> is a <see cref="History"/> and its value is the same as this instance; otherwise, <c>false</c>.
        /// If <paramref name="obj"/> is <c>null</c>, the method returns <c>false</c>.
        /// </returns>
        public override bool Equals(object obj)
        {
            return this.Equals(obj as History);
        }

        /// <summary>
        /// Determines whether this instance and another specified <see cref="History"/> have the same value.
        /// </summary>
        /// <param name="other">The history to compare with this instance.</param>
        /// <returns>
        /// <c>true</c> if the value of <paramref name="other"/> is the same as the value of this instance; otherwise, <c>false</c>.
        /// If <paramref name="other"/> is <c>null</c>, the method returns <c>false</c>.
        /// </returns>
        public bool Equals(History other)
        {
            return ValueSemantics.Equals(this, other);
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
        /// </returns>
        public override int GetHashCode()
        {
            return HashCode.Calculate(this.done.GetHashCode(), this.undone.GetHashCode());
        }

        /// <summary>
        /// Returns a new <see cref="History"/> that contains the undone activities of this instance
        /// without any done activities.
        /// </summary>
        /// <returns>
        /// A <see cref="History"/> that is equivalent to this instance except that the done activities are cleared.
        /// </returns>
        public History ClearDone()
        {
            return new History(
                Timeline<IActivity>.Empty(),
                this.undone);
        }

        /// <summary>
        /// Returns a new <see cref="History"/> that contains the done activities of this instance
        /// without any undone activities.
        /// </summary>
        /// <returns>
        /// A <see cref="History"/> that is equivalent to this instance except that the undone activities are cleared.
        /// </returns>
        public History ClearUndone()
        {
            return new History(
                this.done,
                Timeline<IActivity>.Empty());
        }

        /// <summary>
        /// Copies the done activities in chronological order to the specified <paramref name="collection"/>.
        /// </summary>
        /// <param name="collection">
        /// The collection of activities to which the done activities are copied.
        /// </param>
        public void CopyDoneTo(ICollection<IActivity> collection)
        {
            this.done.CopyTo(collection);
        }

        /// <summary>
        /// Copies the undone activities in chronological order to the specified <paramref name="collection"/>.
        /// </summary>
        /// <param name="collection">
        /// The collection of activities to which the undone activities are copied.
        /// </param>
        public void CopyUndoneTo(ICollection<IActivity> collection)
        {
            this.undone.CopyTo(collection);
        }

        /// <summary>
        /// Returns a new <see cref="History"/> whose done activities
        /// are extended by the result of doing the specified <paramref name="activity"/>,
        /// without any activities to redo.
        /// </summary>
        /// <param name="activity">
        /// The activity to perform the <see cref="IActivity.Do"/> method whose result
        /// is the next activity to undo.
        /// </param>
        /// <returns>
        /// A new <see cref="History"/> whose done activities
        /// are extended by the result of performing the <see cref="IActivity.Do"/>
        /// method of the specified <paramref name="activity"/>,
        /// without any activities to redo.
        /// </returns>
        public History Do(IActivity activity)
        {
            return new History(
                Timeline<IActivity>.Create(activity.Do(), this.done),
                Timeline<IActivity>.Empty());
        }

        /// <summary>
        /// Returns a new <see cref="History"/> whose undone activities
        /// are extended by the result of undoing the recently done activity,
        /// having the done activities truncated by the recently done activity.
        /// </summary>
        /// <returns>
        /// Returns a new <see cref="History"/> whose undone activities
        /// are extended by the result of performing the <see cref="IActivity.Undo"/>
        /// method of the recently done activity,
        /// having the done activities truncated by the recently done activity.
        /// If this instance does not contain any done activities,
        /// the method returns a new <see cref="History"/> that is identical to this instance.
        /// </returns>
        public History Undo()
        {
            return new History(
                this.done.Next(),
                this.undone.Prepend(this.done.Apply(e => e.Undo())));
        }

        /// <summary>
        /// Returns a new <see cref="History"/> whose done activities
        /// are extended by the result of doing the recently undone activity,
        /// having the undone activities truncated by the recently undone activity.
        /// </summary>
        /// <returns>
        /// Returns a new <see cref="History"/> whose done activities
        /// are extended by the result of performing the <see cref="IActivity.Do"/>
        /// method of the recently undone activity,
        /// having the undone activities truncated by the recently undone activity.
        /// If this instance does not contain any undone activities,
        /// the method returns a new <see cref="History"/> that is identical to this instance.
        /// </returns>
        public History Redo()
        {
            return new History(
                this.done.Prepend(this.undone.Apply(e => e.Do())),
                this.undone.Next());
        }

        bool IContentEquatable<History>.ContentEquals(History other)
        {
            return object.Equals(this.done, other.done)
                   && object.Equals(this.undone, other.undone);
        }
    }
}
