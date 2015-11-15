#region Copyright and license
// <copyright file="IActivity.cs" company="Oliver Zick">
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
    /// Defines methods that a type implements to represent an activity that supports doing and undoing.
    /// </summary>
    /// <typeparam name="T">
    /// The type that represents an activity to support doing and undoing.
    /// </typeparam>
    /// <remarks>
    /// This interface is intended to be implemented as immutable object with value semantics.
    /// </remarks>
    public interface IActivity<out T>
    {
        /// <summary>
        /// Returns a new activity that represents the result of doing the current activity.
        /// </summary>
        /// <returns>
        /// A new activity that represents the result of doing the current activity.
        /// </returns>
        T Do();

        /// <summary>
        /// Returns a new activity that represents the result of undoing the current activity.
        /// </summary>
        /// <returns>
        /// A new activity that represents the result of undoing the current activity.
        /// </returns>
        T Undo();
    }
}
