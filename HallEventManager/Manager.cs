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
    }
}