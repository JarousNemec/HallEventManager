using System.Collections.Generic;
using HallEventManager;
using NUnit.Framework;

namespace HallEventManagerTests
{
    public class HallTests
    {
        private Hall hall;

        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void AddAndGetEmployeeTest()
        {
            hall = new Hall();
            Employee employee = InitializeSampleEmployee(1);
            hall.AddEmployee(employee);
            List<Employee> expectedEmployees = new List<Employee>() { employee };
            CollectionAssert.AreEqual(expectedEmployees, hall.GetEmployees());
        }

        [Test]
        public void RemoveEmployeesTest()
        {
            hall = new Hall();
            List<Employee> expectedEmployees = InitializeSampleEmployees(5);
            FillHallWithSampleEmployees(hall,expectedEmployees);
            expectedEmployees.RemoveAt(4);
            expectedEmployees.RemoveAt(3);
            expectedEmployees.RemoveAt(2);
            hall.RemoveEmployees(new List<int>(){2,3,4});
            CollectionAssert.AreEqual(expectedEmployees, hall.GetEmployees());
        }

        private void FillHallWithSampleEmployees(Hall hall, List<Employee> employees)
        {
            foreach (var employee in employees)
            {
                hall.AddEmployee(employee);
            }
        }
        private List<Employee> InitializeSampleEmployees(int count)
        {
            List<Employee> employees = new List<Employee>();
            for (int i = 0; i < count; i++)
            {
                employees.Add(InitializeSampleEmployee(i));
            }

            return employees;
        }

        private Employee InitializeSampleEmployee(int id)
        {
            return new Employee("Josef " + id, "Novák", "Dělník");
        }
    }
}