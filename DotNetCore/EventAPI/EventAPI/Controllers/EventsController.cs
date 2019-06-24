using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using EventAPI.Models;
using EventAPI.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EventAPI.Controllers
{
    [Authorize]
    [FormatFilter]
    //[Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class EventsController : ControllerBase
    {
        private IEventRepository<EventData> eventRepo;

        public EventsController(IEventRepository<EventData> eventRepo)
        {
            this.eventRepo = eventRepo;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<IEnumerable<EventData>> GetEvents()
        {
            var events = eventRepo.GetAll();
            return events.ToList();
        }

        [HttpGet("{id}.{format?}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<EventData> GetById([FromRoute] int id)
        {
            var item = eventRepo.Get(id);
            //if (item == null)
            //{
            //    return NotFound();
            //}
            //else
            {
                return Ok(item);
            }
        }

        [Authorize(Roles ="admin")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]

        public async Task<ActionResult<EventData>> AddAsync([FromBody]EventData ev)
        {
            //try
            //{
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            //throw new InvalidOperationException("No vaild");
            var result = await eventRepo.AddAsync(ev);
            //return result; //200
            //return Created("", result); //201
            //return Created($"/api/events/{result.Id}", result);
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
            //}
            //catch (Exception ex)
            //{
            //    var error = new
            //    {
            //        Message = ex.Message
            //    };
            //    return StatusCode(StatusCodes.Status500InternalServerError, error);
            //}

        }
    }
}