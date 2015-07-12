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
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class HistoryTests
    {
        [TestMethod]
        public void CreateEmpty__Should_Return_New_Instance_With_NullEventNode_Instances_For_Both_Done_And_Undone()
        {
            var expected = new Builder().WithDone(new NullEventNode()).WithUndone(new NullEventNode()).Build();
            var actual = History.CreateEmpty();

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ClearDone__With_Injected_Done_And_Injected_Undone__Should_Return_New_Instance_With_Empty_Done_And_Injected_Undone()
        {
            var doneEvent = new EventStub(1);
            var doneNext = new NullEventNode();
            var done = new EventNode(doneEvent, doneNext);

            var undoneEvent = new EventStub(2);
            var undoneNext = new NullEventNode();
            var undone = new EventNode(undoneEvent, undoneNext);

            var target = new Builder().WithDone(done).WithUndone(undone).Build();

            var expectedDone = new NullEventNode();
            var expectedUndone = undone;

            var expected = new Builder().WithDone(expectedDone).WithUndone(expectedUndone).Build();

            var actual = target.ClearDone();

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ClearUndone__With_Injected_Done_And_Injected_Undone__Should_Return_New_Instance_With_Injected_Done_And_Empty_Undone()
        {
            var doneEvent = new EventStub(1);
            var doneNext = new NullEventNode();
            var done = new EventNode(doneEvent, doneNext);

            var undoneEvent = new EventStub(2);
            var undoneNext = new NullEventNode();
            var undone = new EventNode(undoneEvent, undoneNext);

            var target = new Builder().WithDone(done).WithUndone(undone).Build();

            var expectedDone = done;
            var expectedUndone = new NullEventNode();

            var expected = new Builder().WithDone(expectedDone).WithUndone(expectedUndone).Build();

            var actual = target.ClearUndone();

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Do__With_Event__Should_Return_New_Instance_That_Represents_The_History_After_Performing_The_Do_Operation_Of_The_Event()
        {
            var done = new NullEventNode();
            var undone = new NullEventNode();
            var target = new Builder().WithDone(done).WithUndone(undone).Build();

            var expectedEvent = new EventStub(1, 1, 0);
            var expectedNext = done;
            var expectedDone = new EventNode(expectedEvent, expectedNext);
            var expectedUndone = new NullEventNode();

            var expected = new Builder().WithDone(expectedDone).WithUndone(expectedUndone).Build();

            var @event = new EventStub(1);
            var actual = target.Do(@event);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Undo__Should_Return_New_Instance_That_Represents_The_History_After_Performing_The_Undo_Operation_Of_The_Next_Event_To_Undo()
        {
            var doneEvent = new EventStub(1);
            var doneNext = new NullEventNode();
            var done = new EventNode(doneEvent, doneNext);

            var undoneEvent = new EventStub(2);
            var undoneNext = new NullEventNode();
            var undone = new EventNode(undoneEvent, undoneNext);

            var target = new Builder().WithDone(done).WithUndone(undone).Build();

            var expectedDone = new NullEventNode();

            var expectedUndoneEvent = new EventStub(1, 0, 1);
            var expectedUndoneNext = undone;
            var expectedUndone = new EventNode(expectedUndoneEvent, expectedUndoneNext);

            var expected = new Builder().WithDone(expectedDone).WithUndone(expectedUndone).Build();

            var actual = target.Undo();

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Redo__Should_Return_New_Instance_That_Represents_The_History_After_Performing_The_Do_Operation_Of_The_Next_Event_To_Redo()
        {
            var doneEvent = new EventStub(1);
            var doneNext = new NullEventNode();
            var done = new EventNode(doneEvent, doneNext);

            var undoneEvent = new EventStub(2);
            var undoneNext = new NullEventNode();
            var undone = new EventNode(undoneEvent, undoneNext);

            var target = new Builder().WithDone(done).WithUndone(undone).Build();

            var expectedDoneEvent = new EventStub(2, 1, 0);
            var expectedDoneNext = done;
            var expectedDone = new EventNode(expectedDoneEvent, expectedDoneNext);

            var expectedUndone = new NullEventNode();

            var expected = new Builder().WithDone(expectedDone).WithUndone(expectedUndone).Build();

            var actual = target.Redo();

            Assert.AreEqual(expected, actual);
        }

        #region Value semantics

        [TestMethod]
        public void Equals__With_Null__Should_Return_False()
        {
            const bool expected = false;
            var target = new Builder().Build();
            var other = (History)null;

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
        public void Equals__With_Different_Done__Should_Return_False()
        {
            const bool expected = false;
            var done = new EventNodeDummy();
            var target = new Builder().WithDone(done).Build();
            var other = new Builder().WithDone(null).Build();

            var actual = target.Equals(other);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Equals__With_Different_Undone__Should_Return_False()
        {
            const bool expected = false;
            var undone = new EventNodeDummy();
            var target = new Builder().WithUndone(undone).Build();
            var other = new Builder().WithUndone(null).Build();

            var actual = target.Equals(other);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Equals__With_Same_Setup__Should_Return_True()
        {
            const bool expected = true;
            var done = new EventNodeDummy(1);
            var undone = new EventNodeDummy(2);
            var target = new Builder().WithDone(done).WithUndone(undone).Build();
            var other = new Builder().WithDone(done).WithUndone(undone).Build();

            var actual = target.Equals(other);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Equals_Object__With_Null__Should_Return_False()
        {
            const bool expected = false;
            var target = new Builder().Build();
            var other = (History)null;

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
        public void Equals_Object__With_Different_Done__Should_Return_False()
        {
            const bool expected = false;
            var done = new EventNodeDummy();
            var target = new Builder().WithDone(done).Build();
            var other = new Builder().WithDone(null).Build();

            var actual = target.Equals((object)other);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Equals_Object__With_Different_Undone__Should_Return_False()
        {
            const bool expected = false;
            var undone = new EventNodeDummy();
            var target = new Builder().WithUndone(undone).Build();
            var other = new Builder().WithUndone(null).Build();

            var actual = target.Equals((object)other);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Equals_Object__With_Same_Setup__Should_Return_True()
        {
            const bool expected = true;
            var done = new EventNodeDummy(1);
            var undone = new EventNodeDummy(2);
            var target = new Builder().WithDone(done).WithUndone(undone).Build();
            var other = new Builder().WithDone(done).WithUndone(undone).Build();

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
            var left = new Builder().Build();
            var right = (History)null;

            var actual = left == right;

            Assert.AreEqual(expected, actual);
        }


        [TestMethod]
        public void Op_Equality__With_Null_For_Left_And_Instance_For_Right__Should_Return_False()
        {
            const bool expected = false;
            var left = (History)null;
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
        public void Op_Equality__With_Different_Done__Should_Return_False()
        {
            const bool expected = false;
            var done = new EventNodeDummy();
            var left = new Builder().WithDone(done).Build();
            var right = new Builder().WithDone(null).Build();

            var actual = left == right;

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Op_Equality__With_Different_Undone__Should_Return_False()
        {
            const bool expected = false;
            var undone = new EventNodeDummy();
            var left = new Builder().WithUndone(undone).Build();
            var right = new Builder().WithUndone(null).Build();

            var actual = left == right;

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Op_Equality__With_Same_Setup__Should_Return_True()
        {
            const bool expected = true;
            var done = new EventNodeDummy(1);
            var undone = new EventNodeDummy(2);
            var target1 = new Builder().WithDone(done).WithUndone(undone).Build();
            var target2 = new Builder().WithDone(done).WithUndone(undone).Build();

            var actual = target1 == target2;

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
            var left = new Builder().Build();
            var right = (History)null;

            var actual = left != right;

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Op_Inequality__With_Null_For_Left_And_Instance_For_Right__Should_Return_True()
        {
            const bool expected = true;
            var left = (History)null;
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
        public void Op_Inequality__With_Different_Done__Should_Return_True()
        {
            const bool expected = true;
            var done = new EventNodeDummy();
            var left = new Builder().WithDone(done).Build();
            var right = new Builder().WithDone(null).Build();

            var actual = left != right;

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Op_Inequality__With_Different_Undone__Should_Return_True()
        {
            const bool expected = true;
            var undone = new EventNodeDummy();
            var left = new Builder().WithUndone(undone).Build();
            var right = new Builder().WithUndone(null).Build();

            var actual = left != right;

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Op_Inequality__With_Same_Setup__Should_Return_False()
        {
            const bool expected = false;
            var done = new EventNodeDummy(1);
            var undone = new EventNodeDummy(2);
            var target1 = new Builder().WithDone(done).WithUndone(undone).Build();
            var target2 = new Builder().WithDone(done).WithUndone(undone).Build();

            var actual = target1 != target2;

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void GetHashCode__With_Same_Setup__Should_Return_Same_Hash_Code()
        {
            var done = new EventNodeDummy(1);
            var undone = new EventNodeDummy(2);
            var target1 = new Builder().WithDone(done).WithUndone(undone).Build();
            var target2 = new Builder().WithDone(done).WithUndone(undone).Build();

            Assert.AreEqual(target1.GetHashCode(), target2.GetHashCode());
        }

        #endregion

        private sealed class Builder
        {
            private readonly IEventNode done;

            private readonly IEventNode undone;

            public Builder()
            {
            }

            private Builder(IEventNode done, IEventNode undone)
            {
                this.done = done;
                this.undone = undone;
            }

            public History Build()
            {
                return new History(this.done, this.undone);
            }

            public Builder WithDone(IEventNode value)
            {
                return new Builder(value, this.undone);
            }

            public Builder WithUndone(IEventNode value)
            {
                return new Builder(this.done, value);
            }
        }
    }
}
