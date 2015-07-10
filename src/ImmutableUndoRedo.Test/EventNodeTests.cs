#region Copyright and license
// <copyright file="EventNodeTests.cs" company="Oliver Zick">
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
    public class EventNodeTests
    {
        [TestMethod]
        public void Concat__With_Injected_Event_And_New_Next__Should_Return_New_Instance_With_Injected_Event_And_New_Next()
        {
            var @event = new EventDummy();
            var next = new EventNodeDummy();
            var target = new Builder().WithEvent(@event).WithNext(next).Build();

            var newNext = new EventNodeDummy(1);
            var expected = new Builder().WithEvent(@event).WithNext(newNext).Build();

            var actual = target.Concat(newNext);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Next__With_Injected_Next__Should_Return_Injected_Next()
        {
            var expected = new EventNodeDummy();
            var target = new Builder().WithNext(expected).Build();

            var actual = target.Next();

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Do__With_Injected_Event__Should_Return_New_Instance_With_Injected_Event_Whose_Do_Was_Called_Once()
        {
            var @event = new EventStub(1);
            var target = new Builder().WithEvent(@event).Build();

            var expectedEvent = new EventStub(1, 1, 0);
            var expected = new Builder().WithEvent(expectedEvent).Build();

            var actual = target.Do();

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Undo__With_Injected_Event__Should_Return_New_Instance_With_Injected_Event_Whose_Undo_Was_Called_Once()
        {
            var @event = new EventStub(1);
            var target = new Builder().WithEvent(@event).Build();

            var expectedEvent = new EventStub(1, 0, 1);
            var expected = new Builder().WithEvent(expectedEvent).Build();

            var actual = target.Undo();

            Assert.AreEqual(expected, actual);
        }

        #region Value semantics

        [TestMethod]
        public void Equals__With_Null__Should_Return_False()
        {
            const bool expected = false;
            var target = new Builder().Build();
            var other = (EventNode)null;

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
        public void Equals__With_Different_Event__Should_Return_False()
        {
            const bool expected = false;
            var @event = new EventDummy();
            var target = new Builder().WithEvent(@event).Build();
            var other = new Builder().WithEvent(null).Build();

            var actual = target.Equals(other);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Equals__With_Different_Next__Should_Return_False()
        {
            const bool expected = false;
            var next = new EventNodeDummy();
            var target = new Builder().WithNext(next).Build();
            var other = new Builder().WithNext(null).Build();

            var actual = target.Equals(other);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Equals__With_Same_Setup__Should_Return_True()
        {
            const bool expected = true;
            var @event = new EventDummy();
            var next = new EventNodeDummy();
            var target1 = new Builder().WithEvent(@event).WithNext(next).Build();
            var target2 = new Builder().WithEvent(@event).WithNext(next).Build();

            var actual = target1.Equals(target2);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Equals_Object__With_Null__Should_Return_False()
        {
            const bool expected = false;
            var target = new Builder().Build();
            var other = (EventNode)null;

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
        public void Equals_Object__With_Different_Event__Should_Return_False()
        {
            const bool expected = false;
            var @event = new EventDummy();
            var target = new Builder().WithEvent(@event).Build();
            var other = new Builder().WithEvent(null).Build();

            var actual = target.Equals((object)other);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Equals_Object__With_Different_Next__Should_Return_False()
        {
            const bool expected = false;
            var next = new EventNodeDummy();
            var target = new Builder().WithNext(next).Build();
            var other = new Builder().WithNext(null).Build();

            var actual = target.Equals((object)other);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Equals_Object__With_Same_Setup__Should_Return_True()
        {
            const bool expected = true;
            var @event = new EventDummy();
            var next = new EventNodeDummy();
            var target1 = new Builder().WithEvent(@event).WithNext(next).Build();
            var target2 = new Builder().WithEvent(@event).WithNext(next).Build();

            var actual = target1.Equals((object)target2);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Op_Equality__With_Both_Null__Should_Return_True()
        {
            const bool expected = true;
            var left = (EventNode)null;
            var right = (EventNode)null;

            var actual = left == right;

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Op_Equality__With_Instance_For_Left_And_Null_For_Right__Should_Return_False()
        {
            const bool expected = false;
            var left = new Builder().Build();
            var right = (EventNode)null;

            var actual = left == right;

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Op_Equality__With_Null_For_Left_And_Instance_For_Right__Should_Return_False()
        {
            const bool expected = false;
            var left = (EventNode)null;
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
        public void Op_Equality__With_Different_Event__Should_Return_False()
        {
            const bool expected = false;
            var @event = new EventDummy();
            var left = new Builder().WithEvent(@event).Build();
            var right = new Builder().WithEvent(null).Build();

            var actual = left == right;

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Op_Equality__With_Different_Next__Should_Return_False()
        {
            const bool expected = false;
            var next = new EventNodeDummy();
            var left = new Builder().WithNext(next).Build();
            var right = new Builder().WithNext(null).Build();

            var actual = left == right;

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Op_Equality__With_Same_Setup__Should_Return_True()
        {
            const bool expected = true;
            var @event = new EventDummy();
            var next = new EventNodeDummy();
            var target1 = new Builder().WithEvent(@event).WithNext(next).Build();
            var target2 = new Builder().WithEvent(@event).WithNext(next).Build();

            var actual = target1 == target2;

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Op_Inequality__With_Both_Null__Should_Return_False()
        {
            const bool expected = false;
            var left = (EventNode)null;
            var right = (EventNode)null;

            var actual = left != right;

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Op_Inequality__With_Instance_For_Left_And_Null_For_Right__Should_Return_True()
        {
            const bool expected = true;
            var left = new Builder().Build();
            var right = (EventNode)null;

            var actual = left != right;

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Op_Inequality__With_Null_For_Left_And_Instance_For_Right__Should_Return_True()
        {
            const bool expected = true;
            var left = (EventNode)null;
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
        public void Op_Inequality__With_Different_Event__Should_Return_True()
        {
            const bool expected = true;
            var @event = new EventDummy();
            var left = new Builder().WithEvent(@event).Build();
            var right = new Builder().WithEvent(null).Build();

            var actual = left != right;

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Op_Inequality__With_Different_Next__Should_Return_True()
        {
            const bool expected = true;
            var next = new EventNodeDummy();
            var left = new Builder().WithNext(next).Build();
            var right = new Builder().WithNext(null).Build();

            var actual = left != right;

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Op_Inequality__With_Same_Setup__Should_Return_True()
        {
            const bool expected = false;
            var @event = new EventDummy();
            var next = new EventNodeDummy();
            var target1 = new Builder().WithEvent(@event).WithNext(next).Build();
            var target2 = new Builder().WithEvent(@event).WithNext(next).Build();

            var actual = target1 != target2;

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void GetHashCode__With_Same_Setup__Should_Return_Same_Hash_Code()
        {
            var @event = new EventDummy();
            var next = new EventNodeDummy();
            var target1 = new Builder().WithEvent(@event).WithNext(next).Build();
            var target2 = new Builder().WithEvent(@event).WithNext(next).Build();

            Assert.AreEqual(target1.GetHashCode(), target2.GetHashCode());
        }

        #endregion

        private sealed class Builder
        {
            private readonly IEvent @event;

            private readonly IEventNode next;

            public Builder()
            {
            }

            private Builder(IEvent @event, IEventNode next)
            {
                this.@event = @event;
                this.next = next;
            }

            public EventNode Build()
            {
                return new EventNode(this.@event, this.next);
            }

            public Builder WithEvent(IEvent value)
            {
                return new Builder(value, this.next);
            }

            public Builder WithNext(IEventNode value)
            {
                return new Builder(this.@event, value);
            }
        }
    }
}
