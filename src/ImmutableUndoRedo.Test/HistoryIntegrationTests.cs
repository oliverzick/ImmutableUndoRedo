#region Copyright and license
// <copyright file="HistoryIntegrationTests.cs" company="Oliver Zick">
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
    public class HistoryIntegrationTests
    {
        [TestMethod]
        public void Do__With_Event__Should_Return_New_Instance_That_Represents_The_History_After_Performing_The_Do_Operation_Of_The_Event()
        {
            var done = new EventNode(
                new EventStub(11, 0, 0),
                new EventNode(
                    new EventStub(10, 0, 0),
                    new NullEventNode()));

            var undone = new EventNode(
                new EventStub(21, 0, 0),
                new EventNode(
                    new EventStub(20, 0, 0),
                    new NullEventNode()));

            var target = new History(done, undone);

            var @event = new EventStub(12, 0, 0);

            var expectedDone = new EventNode(
                new EventStub(12, 1, 0),
                done);

            var expectedUndone = new NullEventNode();

            var expected = new History(expectedDone, expectedUndone);

            var actual = target.Do(@event);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Undo__With_Empty_History__Should_Return_Empty_History()
        {
            var target = History.CreateEmpty();

            var actual = target.Undo();

            Assert.AreEqual(target, actual);
        }

        [TestMethod]
        public void Undo__Should_Return_New_Instance_That_Represents_The_History_After_Performing_The_Undo_Operation_Of_The_Next_Event_To_Undo()
        {
            var done = new EventNode(
                new EventStub(11, 0, 0),
                new EventNode(
                    new EventStub(10, 0, 0),
                    new NullEventNode()));

            var undone = new EventNode(
                new EventStub(21, 0, 0),
                new EventNode(
                    new EventStub(20, 0, 0),
                    new NullEventNode()));

            var target = new History(done, undone);

            var expectedDone = new EventNode(
                new EventStub(10, 0, 0),
                 new NullEventNode());

            var expectedUndone = new EventNode(
                new EventStub(11, 0, 1),
                new EventNode(
                    new EventStub(21, 0, 0),
                    new EventNode(
                        new EventStub(20, 0, 0),
                        new NullEventNode())));

            var expected = new History(expectedDone, expectedUndone);

            var actual = target.Undo();

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Redo__With_Empty_History__Should_Return_Empty_History()
        {
            var target = History.CreateEmpty();

            var actual = target.Redo();

            Assert.AreEqual(target, actual);
        }

        [TestMethod]
        public void Redo__Should_Return_New_Instance_That_Represents_The_History_After_Performing_The_Do_Operation_Of_The_Next_Event_To_Redo()
        {
            var done = new EventNode(
                new EventStub(11, 0, 0),
                new EventNode(
                    new EventStub(10, 0, 0),
                    new NullEventNode()));

            var undone = new EventNode(
                new EventStub(21, 0, 0),
                new EventNode(
                    new EventStub(20, 0, 0),
                    new NullEventNode()));

            var target = new History(done, undone);

            var expectedDone = new EventNode(
                new EventStub(21, 1, 0),
                new EventNode(
                    new EventStub(11, 0, 0),
                    new EventNode(
                        new EventStub(10, 0, 0),
                        new NullEventNode())));

            var expectedUndone = new EventNode(
                new EventStub(20, 0, 0),
                 new NullEventNode());

            var expected = new History(expectedDone, expectedUndone);

            var actual = target.Redo();

            Assert.AreEqual(expected, actual);
        }
    }
}
