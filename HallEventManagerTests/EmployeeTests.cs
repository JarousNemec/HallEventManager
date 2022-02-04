using System;
using System.Collections.Generic;
using HallEventManager;
using NUnit.Framework;

namespace HallEventManagerTests
{
    public class EmployeeTests
    {
        private Employee employee;
        [SetUp]
        public void Setup()
        {
            employee = new Employee("Petr", "Horák", "Grafik");
        }

        [Test]
        public void ToStringTest()
        {
            string label = "Petr Horák - Grafik";
            Assert.AreEqual(label, employee.ToString());
        }
    }
}