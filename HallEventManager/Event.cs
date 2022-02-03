using System;
using System.Collections.Generic;
using System.Text;

namespace HallEventManager
{
    [Serializable]
    public class Event
    {
        private string name;
        private DateTime date;
        private string description;
        private List<Employee> employees;

        public Event(string name, DateTime date, List<Employee> employees, string description)
        {
            this.name = name;
            this.date = date;
            this.description = description;
            this.employees = employees;
        }

        public override string ToString()
        {
            return name;
        }

        public DateTime GetDate()
        {
            return date;
        }

        public string GetDescription()
        {
            return description;
        }

        public List<Employee> GetEmployeesOnEvent()
        {
            return employees;
        }
    }
}