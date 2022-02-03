using System;

namespace HallEventManager
{
    [Serializable]
    public class Employee
    {
        private string name;
        private string surname;
        private string position;

        public Employee(string name, string surname, string position)
        {
            this.name = name;
            this.surname = surname;
            this.position = position;
        }

        public override string ToString()
        {
            return $"{name} {surname} - {position}";
        }
    }
}