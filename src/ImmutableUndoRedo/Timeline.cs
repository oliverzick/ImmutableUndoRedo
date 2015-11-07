#region Copyright and license
// // <copyright file="Timeline.cs" company="Oliver Zick">
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
    using System;
    using System.Collections.Generic;
    using System.Linq;

    internal sealed class Timeline<T> : IEquatable<Timeline<T>>, IContentEquatable<Timeline<T>>
    {
        private readonly INode node;

        private Timeline(INode node)
        {
            this.node = node;
        }

        public static Timeline<T> Create(T value, Timeline<T>  next)
        {
            return new Timeline<T>(new Node(value, next.node));
        }

        public static Timeline<T> Create(IEnumerable<T> values)
        {
            return values.Aggregate(Empty(), (current, value) => Create(value, current));
        }

        public static Timeline<T> Empty()
        {
            return new Timeline<T>(new NullNode());
        }

        public override bool Equals(object obj)
        {
            return this.Equals(obj as Timeline<T>);
        }

        public bool Equals(Timeline<T> other)
        {
            return ValueSemantics.Equals(this, other);
        }

        public override int GetHashCode()
        {
            return HashCode.Calculate(this.AllNodesReverse().Select(item => (HashCode)item.GetHashCode()));
        }

        public Timeline<T> Apply(Func<T, T> action)
        {
            return new Timeline<T>(this.node.Apply(action));
        }

        public Timeline<T> Concatenate(Timeline<T> timeline)
        {
            return new Timeline<T>(this.node.Concatenate(timeline.node));
        }

        public void CopyTo(ICollection<T> collection)
        {
            foreach (var item in this.AllNodes())
            {
                item.CopyTo(collection);
            }
        }

        public Timeline<T> Next()
        {
            return new Timeline<T>(this.node.Next());
        }

        public Timeline<T> Prepend(Timeline<T> timeline)
        {
            return timeline.Concatenate(this);
        }

        private IEnumerable<INode> AllNodes()
        {
            return this.AllNodesReverse().Reverse();
        }

        private IEnumerable<INode> AllNodesReverse()
        {
            var current = this.node;

            while (current.HasNext())
            {
                yield return current;

                current = current.Next();
            }

            yield return current;
        }

        bool IContentEquatable<Timeline<T>>.ContentEquals(Timeline<T> other)
        {
            return this.AllNodesReverse().SequenceEqual(other.AllNodesReverse());
        }

        private interface INode
        {
            INode Apply(Func<T, T> action);

            INode Concatenate(INode newNext);

            void CopyTo(ICollection<T> collection);

            bool HasNext();

            INode Next();
        }

        private sealed class Node : INode, IEquatable<Node>, IContentEquatable<Node>
        {
            private readonly T value;

            private readonly INode next;

            public Node(T value, INode next)
            {
                this.value = value;
                this.next = next;
            }

            public INode Apply(Func<T, T> action)
            {
                return new Node(action(this.value), this.next);
            }

            public INode Concatenate(INode newNext)
            {
                return new Node(this.value, newNext);
            }

            public void CopyTo(ICollection<T> collection)
            {
                collection.Add(this.value);
            }

            public bool HasNext()
            {
                return true;
            }

            public INode Next()
            {
                return this.next;
            }

            public override bool Equals(object obj)
            {
                return this.Equals(obj as Node);
            }

            public bool Equals(Node other)
            {
                return ValueSemantics.Equals(this, other);
            }

            public override int GetHashCode()
            {
                return this.value.GetHashCode();
            }

            bool IContentEquatable<Node>.ContentEquals(Node other)
            {
                return object.Equals(this.value, other.value);
            }
        }

        private sealed class NullNode : INode, IEquatable<NullNode>
        {
            public INode Apply(Func<T, T> action)
            {
                return this;
            }

            public INode Concatenate(INode newNext)
            {
                return newNext;
            }

            public void CopyTo(ICollection<T> collection)
            {
            }

            public bool HasNext()
            {
                return false;
            }

            public INode Next()
            {
                return this;
            }

            public override bool Equals(object obj)
            {
                return this.Equals(obj as NullNode);
            }

            public bool Equals(NullNode other)
            {
                return !object.ReferenceEquals(other, null);
            }

            public override int GetHashCode()
            {
                return 0;
            }
        }
    }
}
