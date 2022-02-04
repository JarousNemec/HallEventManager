using System;
using System.Collections.Generic;
using NUnit.Framework;
using NUnit.Framework.Internal.Execution;
using HallEventManager;

namespace HallEventManagerTests
{
    public class EventTests
    {
        private HallEventManager.Event @event;
        private List<Employee> employees;
        private string eventName;
        private DateTime eventDate;
        private string eventDesctiption;

        [SetUp]
        public void Setup()
        {
            eventName = "testEvent";
            eventDate = DateTime.Now;
            employees = new List<Employee>()
            {
                new Employee("test1", "1", "first"), new Employee("test2", "2", "second"),
                new Employee("test3", "3", "third"), new Employee("test4", "4", "fourth")
            };
            eventDesctiption = "just test";
            @event = new HallEventManager.Event(eventName, eventDate, employees, eventDesctiption);
        }

        [Test]
        public void ToStringTest()
        {
            Assert.AreEqual(eventName, @event.ToString());
        }

        [Test]
        public void GetDateTest()
        {
            Assert.AreEqual(eventDate, @event.GetDate());
        }

        [Test]
        public void GetDescriptionTest()
        {
            Assert.AreEqual(eventDesctiption, @event.GetDescription());
        }

        [Test]
        public void GetEmployeesOnEventTest()
        {
            CollectionAssert.AreEqual(employees, @event.GetEmployeesOnEvent());
        }
    }
}