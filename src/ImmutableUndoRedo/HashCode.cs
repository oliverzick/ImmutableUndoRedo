#region Copyright and license
// // <copyright file="HashCode.cs" company="Oliver Zick">
// //     Copyright (c) 2015 Oliver Zick. All rights reserved.
// // </copyright>
// // <author>Oliver Zick</author>
// // <license>
// //     Licensed under the Apache License, Version 2.0 (the "License");
// //     you may not use this file except in compliance with the License.
// //     You may obtain a copy of the License at
// // 
// //     http://www.apache.org/licenses/LICENSE-2.0
// // 
// //     Unless required by applicable law or agreed to in writing, software
// //     distributed under the License is distributed on an "AS IS" BASIS,
// //     WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// //     See the License for the specific language governing permissions and
// //     limitations under the License.
// // </license>
#endregion

namespace ImmutableUndoRedo
{
    using System.Collections.Generic;
    using System.Linq;

    internal struct HashCode
    {
        private readonly int value;

        public HashCode(int value)
        {
            this.value = value;
        }

        public static HashCode Calculate(IEnumerable<HashCode> hashCodes)
        {
            return hashCodes.Aggregate(new HashCode(0), (hashCode, item) => hashCode.Add(item));
        }

        public static HashCode Calculate(params HashCode[] hashCodes)
        {
            return Calculate(hashCodes.AsEnumerable());
        }

        public static implicit operator int(HashCode from)
        {
            return from.value;
        }

        public static implicit operator HashCode(int from)
        {
            return new HashCode(from);
        }

        public HashCode Add(HashCode hashCode)
        {
            unchecked
            {
                return new HashCode(this.value ^ (this.value << 5) + (this.value >> 2) + hashCode.value);
            }
        }
    }
}
