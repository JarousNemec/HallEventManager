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

        public void RemoveEmployees(List<int> indexesToRemove)
        {
            var deletedEmployee = new Employee("", "", "");
            foreach (var index in indexesToRemove)
            {
                employees[index] = deletedEmployee;
            }

            for (int i = 0; i < indexesToRemove.Count; i++)
            {
                foreach (var employee in employees)
                {
                    if (employee == deletedEmployee)
                    {
                        employees.Remove(employee);
                        break;
                    }
                }
            }
        }
    }
}