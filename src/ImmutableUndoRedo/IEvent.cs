#region Copyright and license
// <copyright file="IEvent.cs" company="Oliver Zick">
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
    /// <summary>
    /// Represents an event in the <see cref="History"/> that supports 'Do' and 'Undo' operations.
    /// </summary>
    /// <remarks>
    /// This interface is intended to be implemented as immutable object with value semantics.
    /// </remarks>
    public interface IEvent
    {
        /// <summary>
        /// Returns a new event after performing the 'Do' operation of this event.
        /// </summary>
        /// <returns>
        /// A new event that represents the event after performing the 'Do' operation.
        /// </returns>
        IEvent Do();

        /// <summary>
        /// Returns a new event after performing the 'Undo' operation of this event.
        /// </summary>
        /// <returns>
        /// A new event that represents the event after performing the 'Undo' operation.
        /// </returns>
        IEvent Undo();
    }
}
