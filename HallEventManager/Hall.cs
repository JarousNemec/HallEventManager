using System;
using System.Collections.Generic;

namespace HallEventManager
{
    [Serializable]
    public class Hall
    {
        private List<Employee> employees;

        public Hall()
        {
            employees = new List<Employee>();
        }

        public void AddEmployee(Employee employee)
        {
            employees.Add(employee);
        }

        public List<Employee> GetEmployees()
        {
            return employees;
        }
    }
}