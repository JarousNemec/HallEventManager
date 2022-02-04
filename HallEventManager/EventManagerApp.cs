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
                Console.WriteLine("4) Odebrat událost");
            }

            if (hall.GetEmployees().Count != 0)
            {
                Console.WriteLine("5) Zobrazit seznam zaměstnanců");
                Console.WriteLine("6) Odebrat zaměstnance");
            }

            Console.WriteLine("7) Odejít");
        }

        private const string ADD_EVENT = "1";
        private const string ADD_EMPLOYEE = "2";
        private const string SHOW_EVENT_INFO = "3";
        private const string SHOW_EMPLOYEES = "5";
        private const string REMOVE_EMPLOYEE = "6";
        private const string REMOVE_EVENT = "4";
        private const string EXIT = "7";

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
                case REMOVE_EMPLOYEE:
                {
                    RemoveEmployees();
                }
                    break;
                case REMOVE_EVENT:
                {
                    RemoveEvents();
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
                        PrintAllEmployees();
                    }
                }
                    break;
                case EXIT:
                {
                    run = false;
                    var save = new Save(manager, hall);
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
            var selectedEmployeesIndexes =
                GetSelectedItemsIndexes("Mezerníkem vyberte zaměstnance a výběr potvrďte klávesou ENTER",
                    hall.GetEmployees());
            if (selectedEmployeesIndexes.Count != 0)
            {
                foreach (var index in selectedEmployeesIndexes)
                {
                    selectedEmployees.Add(hall.GetEmployees()[index]);
                }
            }

            return selectedEmployees;
        }

        private void RemoveEmployees()
        {
            var selectedEmployeesIndexes =
                GetSelectedItemsIndexes(
                    "Mezerníkem vyberte zaměstnance, které chcete odstranit, a výběr potvrďte klávesou ENTER",
                    hall.GetEmployees());

            if (selectedEmployeesIndexes.Count != 0)
            {
                hall.RemoveEmployees(selectedEmployeesIndexes);
            }
        }

        private List<int> GetSelectedItemsIndexes<T>(string label, List<T> items)
        {
            var selectedItemsIndexes = new List<int>();
            var choosing = true;
            var cursor = 0;
            while (choosing)
            {
                Console.Clear();
                Console.WriteLine(label);
                PrintChossingMenu(cursor, selectedItemsIndexes, items);
                HandleChossingMenu(ref cursor, selectedItemsIndexes, ref choosing,
                    items.Count);
            }

            return selectedItemsIndexes;
        }

        private void RemoveEvents()
        {
            var selectedEventsIndexes = GetSelectedItemsIndexes(
                "Mezerníkem vyberte události, které chcete odstranit, a výběr potvrďte klávesou ENTER",
                manager.GetEvents());
            if (selectedEventsIndexes.Count != 0)
            {
                manager.RemoveEvents(selectedEventsIndexes);
            }
        }

        private void HandleChossingMenu(ref int cursor, List<int> selectedEmployeesIndexes, ref bool choosing,
            int maxIndex)
        {
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
                    if (cursor + 1 < maxIndex)
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
                }
                    break;
            }
        }

        private void PrintChossingMenu<T>(int cursor, List<int> selectedEmployeesIndexes, List<T> items)
        {
            for (var i = 0; i < items.Count; i++)
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

                Console.WriteLine(items[i]);
            }
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

        private void PrintAllEmployees()
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