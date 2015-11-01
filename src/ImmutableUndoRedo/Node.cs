#region Copyright and license
// // <copyright file="Node.cs" company="Oliver Zick">
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

    internal sealed class Node<T> : IEquatable<Node<T>>, IContentEquatable<Node<T>>
    {
        private readonly IElement element;

        private Node(IElement element)
        {
            this.element = element;
        }

        public static Node<T> Create(T value, Node<T>  next)
        {
            return new Node<T>(new Element(value, next.element));
        }

        public static Node<T> Create(IEnumerable<T> values)
        {
            return values.Aggregate(None(), (current, value) => Create(value, current));
        }

        public static Node<T> None()
        {
            return new Node<T>(new NullElement());
        }

        public override bool Equals(object obj)
        {
            return this.Equals(obj as Node<T>);
        }

        public bool Equals(Node<T> other)
        {
            return ValueSemantics.Equals(this, other);
        }

        public override int GetHashCode()
        {
            return HashCode.Calculate(this.EnumerateElements().Select(item => (HashCode)item.GetHashCode()));
        }

        public Node<T> Apply(Func<T, T> action)
        {
            return new Node<T>(this.element.Apply(action));
        }

        public Node<T> Concatenate(Node<T> node)
        {
            return new Node<T>(this.element.Concatenate(node.element));
        } 

        public Node<T> Next()
        {
            return new Node<T>(this.element.Next());
        }

        public Node<T> Prepend(Node<T> node)
        {
            return node.Concatenate(this);
        }

        public void CopyTo(ICollection<T> collection)
        {
            foreach (var item in this.EnumerateElements().Reverse())
            {
                item.CopyTo(collection);
            }
        }

        private bool HasNext()
        {
            return this.element.HasNext();
        }

        private IEnumerable<Node<T>> Enumerate()
        {
            var current = this;

            while (current.HasNext())
            {
                yield return current;

                current = current.Next();
            }

            yield return current;
        }

        private IEnumerable<IElement> EnumerateElements()
        {
            return this.Enumerate().Select(node => node.element);
        }

        bool IContentEquatable<Node<T>>.ContentEquals(Node<T> other)
        {
            return this.EnumerateElements().SequenceEqual(other.EnumerateElements());
        }

        private interface IElement
        {
            IElement Apply(Func<T, T> action);

            IElement Concatenate(IElement newNext);

            void CopyTo(ICollection<T> collection);

            bool HasNext();

            IElement Next();
        }

        private sealed class Element : IElement, IEquatable<Element>, IContentEquatable<Element>
        {
            private readonly T value;

            private readonly IElement next;

            public Element(T value, IElement next)
            {
                this.value = value;
                this.next = next;
            }

            public IElement Apply(Func<T, T> action)
            {
                return new Element(action(this.value), this.next);
            }

            public IElement Concatenate(IElement newNext)
            {
                return new Element(this.value, newNext);
            }

            public void CopyTo(ICollection<T> collection)
            {
                collection.Add(this.value);
            }

            public bool HasNext()
            {
                return true;
            }

            public IElement Next()
            {
                return this.next;
            }

            public override bool Equals(object obj)
            {
                return this.Equals(obj as Element);
            }

            public bool Equals(Element other)
            {
                return ValueSemantics.Equals(this, other);
            }

            public override int GetHashCode()
            {
                return this.value.GetHashCode();
            }

            bool IContentEquatable<Element>.ContentEquals(Element other)
            {
                return object.Equals(this.value, other.value);
            }
        }

        private sealed class NullElement : IElement, IEquatable<NullElement>
        {
            public IElement Apply(Func<T, T> action)
            {
                return this;
            }

            public IElement Concatenate(IElement newNext)
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

            public IElement Next()
            {
                return this;
            }

            public override bool Equals(object obj)
            {
                return this.Equals(obj as NullElement);
            }

            public bool Equals(NullElement other)
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
