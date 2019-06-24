using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using EventAPI.Models;

namespace EventAPI.Infrastructure
{
    public class EventDbContext : DbContext
    {
        public EventDbContext(DbContextOptions<EventDbContext> options) : base(options)
        {

        }
        public DbSet<EventData> Events { get; set; }
        public DbSet<EventUser> Users { get; set; }
    }
}
