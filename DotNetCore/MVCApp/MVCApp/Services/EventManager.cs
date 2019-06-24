using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MVCApp.Models;
using MVCApp.Infrastructure;

namespace MVCApp.Services
{
    public class EventManager : IEventManager
    {
        EventDbContext db;
        public EventManager(EventDbContext db)
        {
            this.db = db;
        }

        private static List<EventData> events = new List<EventData>()
        {
            new EventData
            {
                Id=1,
                Title="Developing cloud apps using .NET Core",
                Location="Chennai",
                Speaker="Siva",
                Url="https://www.google.com/",
                StartDate=DateTime.Now.AddDays(2),
                EndDate=DateTime.Now.AddDays(5)
            },
            new EventData
            {
                Id=1,
                Title="Cloud architecting on Azure",
                Location="Pune",
                Speaker="Kumar",
                Url="https://www.google.com/",
                StartDate=DateTime.Now.AddDays(6),
                EndDate=DateTime.Now.AddDays(12)
            }
        };


        public EventData Add(EventData data)
        {
            //data.Id = events.Max(e => e.Id) + 1;
            //events.Add(data);
            //return data;            
            db.Events.Add(data);
            db.SaveChanges();
            return data;
        }

        public void Delete(int id)
        {
            throw new NotImplementedException();
        }

        public EventData GetEvent(int id)
        {
            //return events.Find(e => e.Id == id);
            return db.Events.Find(id);

        }

        public IEnumerable<EventData> GetEvents()
        {
            //return events;
            return db.Events.ToList();

        }
    }
}
