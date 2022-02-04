using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace HallEventManager
{
    [Serializable]
    public class Manager
    {
        private List<Event> events;

        public Manager()
        {
            events = new List<Event>();
        }

        public void AddEvent(Event @event)
        {
            events.Add(@event);
        }

        public List<Event> GetEvents()
        {
            return events;
        }

        public void RemoveEvents(List<int> indexesToRemove)
        {
            var deletedEvent = new Event("", DateTime.Now, new List<Employee>(), "");
            foreach (var index in indexesToRemove)
            {
                events[index] = deletedEvent;
            }

            for (int i = 0; i < indexesToRemove.Count; i++)
            {
                foreach (var @event in events)
                {
                    if (@event == deletedEvent)
                    {
                        events.Remove(@event);
                        break;
                    }
                }
            }
        }
    }
}