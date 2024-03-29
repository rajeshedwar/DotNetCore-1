﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MVCApp.Models;

namespace MVCApp.Services
{
    public interface IEventManager
    {
        IEnumerable<EventData> GetEvents();

        EventData GetEvent(int id);

        EventData Add( EventData data);

        void Delete(int id);
    }
}
