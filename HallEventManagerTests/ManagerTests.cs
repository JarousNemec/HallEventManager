using System;
using System.Collections.Generic;
using HallEventManager;
using NUnit.Framework;

namespace HallEventManagerTests
{
    public class ManagerTests
    {
        private Manager manager;

        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void AddAndGetEventTest()
        {
            manager = new Manager();
            Event @event = InitializeSampleEvent(1);
            manager.AddEvent(@event);
            List<Event> expectedEvents = new List<Event>() { @event };
            CollectionAssert.AreEqual(expectedEvents, manager.GetEvents());
        }

        [Test]
        public void RemoveEventsTest()
        {
            manager = new Manager();
            List<Event> expectedEvents = InitializeSampleEvents(5);
            FillManagerWithTestEvents(manager, expectedEvents);
            expectedEvents.RemoveAt(4);
            expectedEvents.RemoveAt(2);
            expectedEvents.RemoveAt(0);
            manager.RemoveEvents(new List<int> { 0, 4, 2 });
            CollectionAssert.AreEqual(expectedEvents, manager.GetEvents());
        }

        private void FillManagerWithTestEvents(Manager manager, List<Event> events)
        {
            foreach (var @event in events)
            {
                manager.AddEvent(@event);
            }
        }

        private List<Event> InitializeSampleEvents(int count)
        {
            List<Event> events = new List<Event>();
            for (int i = 0; i < count; i++)
            {
                events.Add(InitializeSampleEvent(i));
            }

            return events;
        }

        private Event InitializeSampleEvent(int id)
        {
            var eventName = "testEvent " + id;
            var eventDate = DateTime.Now;
            var employees = new List<Employee>()
            {
                new Employee("Pepa", "Novák", "uklízeč"), new Employee("Honza", "Zelinka", "programátor"),
                new Employee("František", "Mírný", "řidič"), new Employee("Václav", "Svatý", "účetní")
            };
            var eventDescription = "just test " + id;
            return new Event(eventName, eventDate, employees, eventDescription);
        }
    }
}