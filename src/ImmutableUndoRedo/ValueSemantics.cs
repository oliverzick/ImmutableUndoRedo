﻿#region Copyright and license
// // <copyright file="ValueSemantics.cs" company="Oliver Zick">
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
    internal static class ValueSemantics
    {
        public static bool Equals<T>(T @this, T other)
            where T : class, IContentEquatable<T>
        {
            if (object.ReferenceEquals(other, null))
            {
                return false;
            }

            if (object.ReferenceEquals(@this, other))
            {
                return true;
            }

            return @this.ContentEquals(other);
        }
    }
}
