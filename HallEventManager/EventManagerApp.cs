using System;
using System.Collections.Generic;
using System.Threading;

namespace HallEventManager
{
    public class EventManagerApp
    {
        private Manager manager;
        private Hall hall;
        
        private const string SAVE_PATH = "data.hallmanager";

        public EventManagerApp()
        {
            var save = Save.LoadSave(SAVE_PATH);
            if (save == null)
            {
                manager = new Manager();
                hall = new Hall();
            }
            else
            {
                manager = save.GetManager();
                hall = save.GetHall();
            }
        }

        private bool run = true;

        public void Start()
        {
            while (run)
            {
                MainMenu();
            }
        }

        private void MainMenu()
        {
            PrintInfoScreen();
            HandleInput();
        }

        private void PrintInfoScreen()
        {
            Console.Clear();
            if (manager.GetEvents().Count != 0)
            {
                Console.WriteLine("Události:");
                foreach (var @event in manager.GetEvents())
                {
                    Console.WriteLine(@event);
                }

                Console.WriteLine();
            }

            Console.WriteLine("Možnosti: ");
            Console.WriteLine("1) Přidat událost");
            Console.WriteLine("2) Přidat zaměstnance");
            if (manager.GetEvents().Count != 0)
            {
                Console.WriteLine("3) Zobrazit podrobnosti o události");
            }

            if (hall.GetEmployees().Count != 0)
            {
                Console.WriteLine("4) Zobrazit seznam zaměstnanců");
            }

            Console.WriteLine("5) Odejít");
        }

        private const string ADD_EVENT = "1";
        private const string ADD_EMPLOYEE = "2";
        private const string SHOW_EVENT_INFO = "3";
        private const string SHOW_EMPLOYEES = "4";
        private const string EXIT = "5";
        private void HandleInput()
        {
            switch (Console.ReadLine())
            {
                case ADD_EVENT:
                {
                    if (hall.GetEmployees().Count != 0)
                    {
                        EventAddForm();
                    }
                    else
                    {
                        NotEnoughEmployeesForEventError();
                    }
                }
                    break;
                case ADD_EMPLOYEE:
                {
                    EmployeeAddForm();
                }
                    break;
                case SHOW_EVENT_INFO:
                {
                    if (manager.GetEvents().Count != 0)
                    {
                        EventInfoMenu();
                    }
                }
                    break;
                case SHOW_EMPLOYEES:
                {
                    if (hall.GetEmployees().Count != 0)
                    {
                        PrintAllEmployes();
                    }
                }
                    break;
                case EXIT:
                {
                    run = false;
                    var save = new Save(manager,hall);
                    save.SaveData(SAVE_PATH);
                }
                    break;
            }
        }

        private void EmployeeAddForm()
        {
            Console.Clear();
            var name = GetImportantStringValue("Zadejte jméno: ");
            var surname = GetImportantStringValue("Zadejte příjmení: ");
            var position = GetImportantStringValue("Zadejte název pozice: ");
            Employee employee = new Employee(name, surname, position);
            hall.AddEmployee(employee);
        }

        private void NotEnoughEmployeesForEventError()
        {
            Console.Clear();
            Console.WriteLine("Systém neobsahuje žádné zaměstnance, a proto nelze vytvořit událost!");
            Console.WriteLine("Nejprve přidejte zaměstnance do systému.");
            Console.ReadKey();
        }

        private void EventAddForm()
        {
            Console.Clear();
            string name = GetImportantStringValue("Zadejte název události: ");
            Console.WriteLine("Zadejte popis události: ");
            string description = Console.ReadLine();
            DateTime date = GetDateWhenEventHappensFromUser();
            List<Employee> employees = SelectEmployeesForEvent();
            if (employees.Count == 0)
            {
                Console.Clear();
                Console.WriteLine("Vytvoření události se nezdařilo, nebyl vybrán žádný zaměstnanec!");
                Console.ReadKey();
                return;
            }

            Event @event = new Event(name, date, employees, description);
            manager.AddEvent(@event);
        }

        private List<Employee> SelectEmployeesForEvent()
        {
            var selectedEmployees = new List<Employee>();
            var selectedEmployeesIndexes = new List<int>();
            var choosing = true;
            var cursor = 0;
            while (choosing)
            {
                Console.Clear();
                Console.WriteLine("Mezerníkem vyberte zaměstnance a výběr potvrďte klávesou ENTER");
                for (int i = 0; i < hall.GetEmployees().Count; i++)
                {
                    if (i == cursor && selectedEmployeesIndexes.Contains(i))
                    {
                        Console.Write($"[■] ");
                    }
                    else if (i == cursor && !selectedEmployeesIndexes.Contains(i))
                    {
                        Console.Write($"[ ] ");
                    }
                    else if (i != cursor && selectedEmployeesIndexes.Contains(i))
                    {
                        Console.Write($" ■  ");
                    }
                    else if (i != cursor && !selectedEmployeesIndexes.Contains(i))
                    {
                        Console.Write($"    ");
                    }

                    Console.WriteLine(hall.GetEmployees()[i]);
                }

                switch (Console.ReadKey().Key)
                {
                    case ConsoleKey.UpArrow:
                    {
                        if (cursor - 1 >= 0)
                        {
                            cursor--;
                        }
                    }
                        break;
                    case ConsoleKey.DownArrow:
                    {
                        if (cursor + 1 < hall.GetEmployees().Count)
                        {
                            cursor++;
                        }
                    }
                        break;
                    case ConsoleKey.Spacebar:
                    {
                        if (selectedEmployeesIndexes.Contains(cursor))
                        {
                            selectedEmployeesIndexes.Remove(cursor);
                        }
                        else
                        {
                            selectedEmployeesIndexes.Add(cursor);
                        }
                    }
                        break;
                    case ConsoleKey.Enter:
                    {
                        choosing = false;
                        if (selectedEmployeesIndexes.Count != 0)
                        {
                            foreach (var index in selectedEmployeesIndexes)
                            {
                                selectedEmployees.Add(hall.GetEmployees()[index]);
                            }
                        }
                    }
                        break;
                }
            }

            return selectedEmployees;
        }

        private DateTime GetDateWhenEventHappensFromUser()
        {
            var year = GetImportantIntValue("Zadejte rok: ", 2005, 9999);
            var month = GetImportantIntValue("Zadejte měsíc: ", 1, 12);
            var day = GetImportantIntValue("Zadejte den: ", 1, 31);
            var hour = GetImportantIntValue("Zadejte hodinu: ", 0, 23);
            var minute = GetImportantIntValue("Zadejte minuty: ", 0, 59);
            return new DateTime(year, month, day, hour, minute, 0);
        }

        private int GetImportantIntValue(string label, int min, int max)
        {
            int value;
            bool parsed;
            bool isInRange = false;
            do
            {
                Console.Write(label);
                parsed = int.TryParse(Console.ReadLine(), out value);
                if (parsed)
                {
                    if (value <= max && value >= min)
                    {
                        isInRange = true;
                    }
                }
            } while (!(parsed && isInRange));

            return value;
        }

        private string GetImportantStringValue(string label)
        {
            string value;
            do
            {
                Console.Write(label);
                value = Console.ReadLine();
            } while (string.IsNullOrWhiteSpace(value) || string.IsNullOrEmpty(value));

            return value;
        }

        private void EventInfoMenu()
        {
            bool choosing = true;
            int cursor = 0;
            while (choosing)
            {
                Console.Clear();
                Console.WriteLine("Vyberte událost a stiskněte ENTER pro zobrazení podrobností");
                for (int i = 0; i < manager.GetEvents().Count; i++)
                {
                    Console.WriteLine(i == cursor ? $"[■] {manager.GetEvents()[i]}" : $"[ ] {manager.GetEvents()[i]}");
                }

                switch (Console.ReadKey().Key)
                {
                    case ConsoleKey.UpArrow:
                    {
                        if (cursor - 1 >= 0)
                        {
                            cursor--;
                        }
                    }
                        break;
                    case ConsoleKey.DownArrow:
                    {
                        if (cursor + 1 < manager.GetEvents().Count)
                        {
                            cursor++;
                        }
                    }
                        break;
                    case ConsoleKey.Enter:
                    {
                        PrintEventInfo(manager.GetEvents()[cursor]);
                        choosing = false;
                    }
                        break;
                }
            }

            Console.ReadKey();
        }

        private void PrintAllEmployes()
        {
            Console.Clear();
            Console.WriteLine("Zaměstnanci: ");
            foreach (var employee in hall.GetEmployees())
            {
                Console.WriteLine(employee);
            }

            Console.ReadKey();
        }

        private void PrintEventInfo(Event @event)
        {
            Console.Clear();
            Console.WriteLine($"Název: {@event}");
            Console.WriteLine($"Datum: {@event.GetDate()}");

            Console.WriteLine(
                string.IsNullOrEmpty(@event.GetDescription()) || string.IsNullOrWhiteSpace(@event.GetDescription())
                    ? "Bez popisu"
                    : $"Popis: {@event.GetDescription()}");
            Console.WriteLine("Pozvaní zaměstnanci: ");
            foreach (var employee in @event.GetEmployeesOnEvent())
            {
                Console.WriteLine(employee);
            }
        }
    }
}