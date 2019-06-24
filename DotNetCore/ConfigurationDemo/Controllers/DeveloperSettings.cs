using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ConfigurationDemo.Models;
using Microsoft.Extensions.Configuration;

namespace ConfigurationDemo.Controllers
{
    public class DeveloperSettings
    {
        public DeveloperSettings{
            this.Address = new Address();
        }
        public string Name { get; set; }
        public string Domain { get; set; }
        public string Experience { get; set; }
        public string Address { get; set; }
    }

    public class DeveloperSettings
    {
        public string No { get; set; }
        public string City { get; set; }
        public string State { get; set; }
    }
}