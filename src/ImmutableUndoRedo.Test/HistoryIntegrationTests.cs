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
    using System.Linq;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class HistoryIntegrationTests
    {
        [TestMethod]
        public void Do__With_Event__Should_Return_New_Instance_Whose_History_Of_Done_Events_Is_Extended_By_The_Result_Of_Doing_The_Specified_Event_Without_Any_Events_To_Redo()
        {
            var done = new IEvent[]
                       {
                           new EventStub(10, 0, 0),
                           new EventStub(11, 0, 0),
                       };

            var undone = new IEvent[]
                         {
                             new EventStub(20, 0, 0),
                             new EventStub(21, 0, 0),
                         };

            var target = History.Create(done, undone);

            var expectedDone = new IEvent[]
                               {
                                   new EventStub(10, 0, 0),
                                   new EventStub(11, 0, 0),
                                   new EventStub(12, 1, 0),
                               };

            var expectedUndone = Enumerable.Empty<IEvent>();

            var expected = History.Create(expectedDone, expectedUndone);

            var @event = new EventStub(12, 0, 0);

            var actual = target.Do(@event);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Undo__On_Empty_History__Should_Return_Empty_History()
        {
            var expected = History.Create();

            var target = History.Create();

            var actual = target.Undo();

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Undo__With_Given_Done_And_Undone_Events__Should_Return_New_Instance_Whose_History_Of_Undone_Events_Is_Extended_By_The_Result_Of_Undoing_The_Recently_Done_Event_Having_The_History_Of_Done_Events_Truncated_By_The_Recently_Done_Event()
        {
            var done = new IEvent[]
                       {
                           new EventStub(10, 0, 0),
                           new EventStub(11, 0, 0),
                       };

            var undone = new IEvent[]
                         {
                             new EventStub(20, 0, 0),
                             new EventStub(21, 0, 0),
                         };

            var target = History.Create(done, undone);

            var expectedDone = new IEvent[]
                               {
                                   new EventStub(10, 0, 0),
                               };

            var expectedUndone = new IEvent[]
                                 {
                                     new EventStub(20, 0, 0),
                                     new EventStub(21, 0, 0),
                                     new EventStub(11, 0, 1),
                                 };

            var expected = History.Create(expectedDone, expectedUndone);

            var actual = target.Undo();

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Undo__Without_Any_Done_Events__Should_Return_Identical_Instance()
        {
            var done = new IEvent[] { };

            var undone = new IEvent[]
                         {
                             new EventStub(20, 0, 0),
                             new EventStub(21, 0, 0),
                         };

            var target = History.Create(done, undone);

            var expected = History.Create(done, undone);

            var actual = target.Undo();

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Redo__On_Empty_History__Should_Return_Empty_History()
        {
            var expected = History.Create();

            var target = History.Create();

            var actual = target.Redo();

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Redo__With_Given_Done_And_Undone_Events__Should_Return_New_Instance_Whose_History_Of_Done_Events_Is_Extended_By_The_Result_Of_Doing_The_Recently_Undone_Event_Having_The_History_Of_Undone_Events_Truncated_By_The_Recently_Undone_Event()
        {
            var done = new IEvent[]
                       {
                           new EventStub(10, 0, 0),
                           new EventStub(11, 0, 0),
                       };

            var undone = new IEvent[]
                         {
                             new EventStub(20, 0, 0),
                             new EventStub(21, 0, 0),
                         };

            var target = History.Create(done, undone);

            var expectedDone = new IEvent[]
                               {
                                   new EventStub(10, 0, 0),
                                   new EventStub(11, 0, 0),
                                   new EventStub(21, 1, 0),
                               };

            var expectedUndone = new IEvent[]
                                 {
                                     new EventStub(20, 0, 0),
                                 };

            var expected = History.Create(expectedDone, expectedUndone);

            var actual = target.Redo();

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Redo__Without_Any_Undone_Events__Should_Return_Identical_Instance()
        {
            var done = new IEvent[]
                       {
                           new EventStub(10, 0, 0),
                           new EventStub(11, 0, 0),
                       };

            var undone = new IEvent[] { };

            var target = History.Create(done, undone);

            var expected = History.Create(done, undone);

            var actual = target.Redo();

            Assert.AreEqual(expected, actual);
        }
    }
}
