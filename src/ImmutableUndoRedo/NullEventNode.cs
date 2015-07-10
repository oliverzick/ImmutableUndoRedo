#region Copyright and license
// <copyright file="NullEventNode.cs" company="Oliver Zick">
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

    internal sealed class NullEventNode : IEventNode, IEquatable<NullEventNode>
    {
        private const int HashCode = 0;

        public static bool operator ==(NullEventNode left, NullEventNode right)
        {
            return object.Equals(left, right);
        }

        public static bool operator !=(NullEventNode left, NullEventNode right)
        {
            return !(left == right);
        }

        public override bool Equals(object obj)
        {
            return this.Equals(obj as NullEventNode);
        }

        public bool Equals(NullEventNode other)
        {
            return !object.ReferenceEquals(other, null);
        }

        public override int GetHashCode()
        {
            return HashCode;
        }

        public IEventNode Concat(IEventNode newNext)
        {
            return newNext;
        }

        public IEventNode Do()
        {
            return this;
        }

        public IEventNode Next()
        {
            return this;
        }

        public IEventNode Undo()
        {
            return this;
        }
    }
}
