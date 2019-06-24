using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MVCApp.Services;
using MVCApp.Repositories;
using MVCApp.Models;

namespace MVCApp.Controllers
{
    [Route("events")]
    public class EventInfoController : Controller
    {
        //private IEventManager eventManager;
        //public EventInfoController(IEventManager em)
        //{
        //    this.eventManager = em;
        //}        

        //[HttpGet("",Name = "EventList")]
        //public IActionResult Index()
        //{
        //    var events = eventManager.GetEvents();
        //    return View(events);
        //}

        //[HttpGet("new", Name ="NewEvent")]
        //public IActionResult Create()
        //{
        //    return View();
        //}

        //[HttpPost("new",Name ="NewEvent")]
        //public IActionResult Create(EventData model)
        //{
        //    if(ModelState.IsValid)
        //    {
        //        eventManager.Add(model);
        //        return RedirectToRoute("EventList");
        //    }
        //    else
        //    {
        //        return View();
        //    }
        //}

        private IEventRepository<EventData> repo;
        private IEventRepository<EventUser> repo1;
        public EventInfoController(IEventRepository<EventData> repository, IEventRepository<EventUser> repository1)
        {
            this.repo = repository;
            this.repo1 = repository1;
        }

        [HttpGet("", Name = "EventList")]
        public IActionResult Index()
        {
            var events = repo.GetAll();
            return View(events);
        }

        [HttpGet("new", Name = "NewEvent")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost("new", Name = "NewEvent")]
        public async Task<IActionResult> Create(EventData model)
        {
            if (ModelState.IsValid)
            {
                await repo.Add(model);
                return RedirectToRoute("EventList");
            }
            else
            {
                return View();
            }
        }


        [HttpGet("newevent", Name = "NewEventRegister")]
        public IActionResult EventRegister([FromQuery] int id)
        {
            var item = repo.Get(id);
            ViewBag.EventID = item.Id;
            ViewBag.EventName = item.Title;
            return View();
        }

        [HttpPost("newevent", Name = "NewEventRegister")]
        public async Task<IActionResult> EventRegister(EventUser model)
        {
            if (ModelState.IsValid)
            {
                await repo1.Add(model);
                return RedirectToRoute("EventList");
            }
            else
            {
                return View();
            }
        }
    }
}