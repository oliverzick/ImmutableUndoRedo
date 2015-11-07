#region Copyright and license
// <copyright file="HistoryTests.cs" company="Oliver Zick">
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
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class HistoryTests
    {
        [TestMethod]
        public void Create__Should_Return_New_Instance_With_Neither_Done_Nor_Undone_Events()
        {
            var expectedDone = Enumerable.Empty<IEvent>();
            var expectedUndone = Enumerable.Empty<IEvent>();
            var expected = History.Create(expectedDone, expectedUndone);

            var actual = History.Create();

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void CreateEmpty__Should_Return_New_Instance_With_Neither_Done_Nor_Undone_Events()
        {
            var expectedDone = Enumerable.Empty<IEvent>();
            var expectedUndone = Enumerable.Empty<IEvent>();
            var expected = History.Create(expectedDone, expectedUndone);

            var actual = History.CreateEmpty();

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ClearDone__With_Given_Done_And_Undone_Events__Should_Return_New_Instance_With_No_Done_And_Given_Undone_Events()
        {
            var done = new IEvent[] { new EventStub(1) };
            var undone = new IEvent[] { new EventStub(2) };
            var target = History.Create(done, undone);
            var expectedDone = Enumerable.Empty<IEvent>();
            var expectedUndone = new IEvent[] { new EventStub(2) };
            var expected = History.Create(expectedDone, expectedUndone);

            var actual = target.ClearDone();

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ClearUndone__With_Given_Done_And_Undone_Events__Should_Return_New_Instance_With_Given_Done_And_No_Undone_Events()
        {
            var done = new IEvent[] { new EventStub(1) };
            var undone = new IEvent[] { new EventStub(2) };
            var target = History.Create(done, undone);
            var expectedDone = new IEvent[] { new EventStub(1) };
            var expectedUndone = Enumerable.Empty<IEvent>();

            var expected = History.Create(expectedDone, expectedUndone);

            var actual = target.ClearUndone();

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void CopyDoneTo__With_Collection__Should_Copy_The_Done_Events_In_Chronological_Order_To_The_Given_Collection()
        {
            var done = new IEvent[] { new EventStub(11), new EventStub(12), new EventStub(13) };
            var undone = new IEvent[] { new EventStub(21), new EventStub(22), new EventStub(23) };
            var target = History.Create(done, undone);
            var expected = new List<IEvent>(done);
            var actual = new List<IEvent>();

            target.CopyDoneTo(actual);

            CollectionAssert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void CopyUndoneTo__With_Collection__Should_Copy_The_Undone_Events_In_Chronological_Order_To_The_Given_Collection()
        {
            var done = new IEvent[] { new EventStub(11), new EventStub(12), new EventStub(13) };
            var undone = new IEvent[] { new EventStub(21), new EventStub(22), new EventStub(23) };
            var target = History.Create(done, undone);
            var expected = new List<IEvent>(undone);
            var actual = new List<IEvent>();

            target.CopyUndoneTo(actual);

            CollectionAssert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Do__With_Event__Should_Return_New_Instance_Whose_History_Of_Done_Events_Is_Extended_By_The_Result_Of_Doing_The_Specified_Event_Without_Any_Events_To_Redo()
        {
            var target = History.Create();
            var @event = new EventStub(1);
            var expectedDone = new IEvent[] { new EventStub(1, 1, 0) };
            var expectedUndone = Enumerable.Empty<IEvent>();
            var expected = History.Create(expectedDone, expectedUndone);

            var actual = target.Do(@event);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Undo__With_Given_Done_Events__Should_Return_New_Instance_Whose_History_Of_Undone_Events_Is_Extended_By_The_Result_Of_Undoing_The_Recently_Done_Event_Having_The_History_Of_Done_Events_Truncated_By_The_Recently_Done_Event()
        {
            var done = new IEvent[] { new EventStub(1) };
            var undone = Enumerable.Empty<IEvent>();
            var target = History.Create(done, undone);
            var expectedDone = Enumerable.Empty<IEvent>();
            var expectedUndone = new IEvent[] { new EventStub(1, 0, 1) };
            var expected = History.Create(expectedDone, expectedUndone);

            var actual = target.Undo();

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Redo__With_Given_Undone_Events__Should_Return_New_Instance_Whose_History_Of_Done_Events_Is_Extended_By_The_Result_Of_Doing_The_Recently_Undone_Event_Having_The_History_Of_Undone_Events_Truncated_By_The_Recently_Undone_Event()
        {
            var done = Enumerable.Empty<IEvent>();
            var undone = new IEvent[] { new EventStub(1) };
            var target = History.Create(done, undone);
            var expectedDone = new IEvent[] { new EventStub(1, 1, 0) };
            var expectedUndone = Enumerable.Empty<IEvent>();
            var expected = History.Create(expectedDone, expectedUndone);

            var actual = target.Redo();

            Assert.AreEqual(expected, actual);
        }

        #region Value semantics

        [TestMethod]
        public void Equals__With_Null__Should_Return_False()
        {
            const bool expected = false;
            var target = History.Create();
            var other = (History)null;

            var actual = target.Equals(other);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Equals__With_Same_Instance__Should_Return_True()
        {
            const bool expected = true;
            var target = History.Create();
            var other = target;

            var actual = target.Equals(other);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Equals__With_Different_Done__Should_Return_False()
        {
            const bool expected = false;
            var none = new IEvent[0];
            var done = new IEvent[] { new EventStub(1) };
            var target = History.Create(done, none);
            var other = History.Create(none, none);

            var actual = target.Equals(other);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Equals__With_Different_Undone__Should_Return_False()
        {
            const bool expected = false;
            var none = new IEvent[0];
            var undone = new IEvent[] { new EventStub(1) };
            var target = History.Create(none, undone);
            var other = History.Create(none, none);

            var actual = target.Equals(other);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Equals__With_Same_Setup__Should_Return_True()
        {
            const bool expected = true;
            var done = new IEvent[] { new EventStub(1) };
            var undone = new IEvent[] { new EventStub(2) };
            var target = History.Create(done, undone);
            var other = History.Create(done, undone);

            var actual = target.Equals(other);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Equals_Object__With_Null__Should_Return_False()
        {
            const bool expected = false;
            var target = History.Create();
            var other = (History)null;

            var actual = target.Equals((object)other);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Equals_Object__With_Same_Instance__Should_Return_True()
        {
            const bool expected = true;
            var target = History.Create();
            var other = target;

            var actual = target.Equals((object)other);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Equals_Object__With_Different_Done__Should_Return_False()
        {
            const bool expected = false;
            var none = new IEvent[0];
            var done = new IEvent[] { new EventStub(1) };
            var target = History.Create(done, none);
            var other = History.Create(none, none);

            var actual = target.Equals((object)other);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Equals_Object__With_Different_Undone__Should_Return_False()
        {
            const bool expected = false;
            var none = new IEvent[0];
            var undone = new IEvent[] { new EventStub(1) };
            var target = History.Create(none, undone);
            var other = History.Create(none, none);

            var actual = target.Equals((object)other);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Equals_Object__With_Same_Setup__Should_Return_True()
        {
            const bool expected = true;
            var done = new IEvent[] { new EventStub(1) };
            var undone = new IEvent[] { new EventStub(2) };
            var target = History.Create(done, undone);
            var other = History.Create(done, undone);

            var actual = target.Equals((object)other);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Op_Equality__With_Both_Null__Should_Return_True()
        {
            const bool expected = true;
            var left = (History)null;
            var right = (History)null;

            var actual = left == right;

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Op_Equality__With_Instance_For_Left_And_Null_For_Right__Should_Return_False()
        {
            const bool expected = false;
            var left = History.Create();
            var right = (History)null;

            var actual = left == right;

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Op_Equality__With_Null_For_Left_And_Instance_For_Right__Should_Return_False()
        {
            const bool expected = false;
            var left = (History)null;
            var right = History.Create();

            var actual = left == right;

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Op_Equality__With_Same_Instance__Should_Return_True()
        {
            const bool expected = true;
            var left = History.Create();
            var right = left;

            var actual = left == right;

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Op_Equality__With_Different_Done__Should_Return_False()
        {
            const bool expected = false;
            var none = new IEvent[0];
            var done = new IEvent[] { new EventStub(1) };
            var left = History.Create(done, none);
            var right = History.Create(none, none);

            var actual = left == right;

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Op_Equality__With_Different_Undone__Should_Return_False()
        {
            const bool expected = false;
            var none = new IEvent[0];
            var undone = new IEvent[] { new EventStub(1) };
            var left = History.Create(none, undone);
            var right = History.Create(none, none);

            var actual = left == right;

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Op_Equality__With_Same_Setup__Should_Return_True()
        {
            const bool expected = true;
            var done = new IEvent[] { new EventStub(1) };
            var undone = new IEvent[] { new EventStub(2) };
            var left = History.Create(done, undone);
            var right = History.Create(done, undone);

            var actual = left == right;

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Op_Inequality__With_Both_Null__Should_Return_False()
        {
            const bool expected = false;
            var left = (History)null;
            var right = (History)null;

            var actual = left != right;

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Op_Inequality__With_Instance_For_Left_And_Null_For_Right__Should_Return_True()
        {
            const bool expected = true;
            var left = History.Create();
            var right = (History)null;

            var actual = left != right;

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Op_Inequality__With_Null_For_Left_And_Instance_For_Right__Should_Return_True()
        {
            const bool expected = true;
            var left = (History)null;
            var right = History.Create();

            var actual = left != right;

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Op_Inequality__With_Same_Instance__Should_Return_False()
        {
            const bool expected = false;
            var left = History.Create();
            var right = left;

            var actual = left != right;

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Op_Inequality__With_Different_Done__Should_Return_True()
        {
            const bool expected = true;
            var none = new IEvent[0];
            var done = new IEvent[] { new EventStub(1) };
            var left = History.Create(done, none);
            var right = History.Create(none, none);

            var actual = left != right;

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Op_Inequality__With_Different_Undone__Should_Return_True()
        {
            const bool expected = true;
            var none = new IEvent[0];
            var undone = new IEvent[] { new EventStub(1) };
            var left = History.Create(none, undone);
            var right = History.Create(none, none);

            var actual = left != right;

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Op_Inequality__With_Same_Setup__Should_Return_False()
        {
            const bool expected = false;
            var done = new IEvent[] { new EventStub(1) };
            var undone = new IEvent[] { new EventStub(2) };
            var left = History.Create(done, undone);
            var right = History.Create(done, undone);

            var actual = left != right;

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void GetHashCode__With_Same_Setup__Should_Return_Same_Hash_Code()
        {
            var done = new IEvent[] { new EventStub(1) };
            var undone = new IEvent[] { new EventStub(2) };
            var target1 = History.Create(done, undone);
            var target2 = History.Create(done, undone);

            Assert.AreEqual(target1.GetHashCode(), target2.GetHashCode());
        }

        #endregion
    }
}
