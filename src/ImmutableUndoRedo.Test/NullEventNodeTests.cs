#region Copyright and license
// <copyright file="NullEventNodeTests.cs" company="Oliver Zick">
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
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class NullEventNodeTests
    {
        [TestMethod]
        public void Concat__With_New_Next__Should_Return_New_Next()
        {
            IEventNode expected = new EventNodeDummy();
            var target = new Builder().Build();

            var actual = target.Concat(expected);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Do__Should_Return_Itself()
        {
            var target = new Builder().Build();

            var actual = target.Do();

            Assert.AreSame(target, actual);
        }

        [TestMethod]
        public void Next__Should_Return_Itself()
        {
            var target = new Builder().Build();

            var actual = target.Next();

            Assert.AreSame(target, actual);
        }

        [TestMethod]
        public void Undo__Should_Return_Itself()
        {
            var target = new Builder().Build();

            var actual = target.Undo();

            Assert.AreSame(target, actual);
        }

        #region Value semantics

        [TestMethod]
        public void Equals__With_Null__Should_Return_False()
        {
            const bool expected = false;
            var target = new Builder().Build();
            var other = (NullEventNode)null;

            var actual = target.Equals(other);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Equals__With_Same_Instance__Should_Return_True()
        {
            const bool expected = true;
            var target = new Builder().Build();
            var other = target;

            var actual = target.Equals(other);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Equals__With_Different_Instances__Should_Return_True()
        {
            const bool expected = true;
            var target = new Builder().Build();
            var other = new Builder().Build();

            var actual = target.Equals(other);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Equals_Object__With_Null__Should_Return_False()
        {
            const bool expected = false;
            var target = new Builder().Build();
            var other = (NullEventNode)null;

            var actual = target.Equals((object)other);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Equals_Object__With_Same_Instance__Should_Return_True()
        {
            const bool expected = true;
            var target = new Builder().Build();
            var other = target;

            var actual = target.Equals((object)other);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Equals_Object__With_Different_Instances__Should_Return_True()
        {
            const bool expected = true;
            var target = new Builder().Build();
            var other = new Builder().Build();

            var actual = target.Equals((object)other);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Op_Equality__With_Both_Null__Should_Return_True()
        {
            const bool expected = true;
            var left = (NullEventNode)null;
            var right = (NullEventNode)null;

            var actual = left == right;

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Op_Equality__With_Instance_For_Left_And_Null_For_Right__Should_Return_False()
        {
            const bool expected = false;
            var left = new Builder().Build();
            var right = (NullEventNode)null;

            var actual = left == right;

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Op_Equality__With_Null_For_Left_And_Instance_For_Right__Should_Return_False()
        {
            const bool expected = false;
            var left = (NullEventNode)null;
            var right = new Builder().Build();

            var actual = left == right;

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Op_Equality__With_Same_Instance__Should_Return_True()
        {
            const bool expected = true;
            var left = new Builder().Build();
            var right = left;

            var actual = left == right;

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Op_Equality__With_Different_Instances__Should_Return_True()
        {
            const bool expected = true;
            var left = new Builder().Build();
            var right = new Builder().Build();

            var actual = left == right;

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Op_Inequality__With_Both_Null__Should_Return_False()
        {
            const bool expected = false;
            var left = (NullEventNode)null;
            var right = (NullEventNode)null;

            var actual = left != right;

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Op_Inequality__With_Instance_For_Left_And_Null_For_Right__Should_Return_True()
        {
            const bool expected = true;
            var left = new Builder().Build();
            var right = (NullEventNode)null;

            var actual = left != right;

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Op_Inequality__With_Null_For_Left_And_Instance_For_Right__Should_Return_True()
        {
            const bool expected = true;
            var left = (NullEventNode)null;
            var right = new Builder().Build();

            var actual = left != right;

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Op_Inequality__With_Same_Instance__Should_Return_False()
        {
            const bool expected = false;
            var left = new Builder().Build();
            var right = left;

            var actual = left != right;

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Op_Inequality__With_Different_Instances__Should_Return_False()
        {
            const bool expected = false;
            var left = new Builder().Build();
            var right = new Builder().Build();

            var actual = left != right;

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void GetHashCode__Should_Return_0()
        {
            const int expected = 0;
            var target = new Builder().Build();

            var actual = target.GetHashCode();

            Assert.AreEqual(expected, actual);
        }

        #endregion

        private sealed class Builder
        {
            public NullEventNode Build()
            {
                return new NullEventNode();
            }
        }
    }
}
