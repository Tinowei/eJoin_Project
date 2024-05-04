using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Admin.Models;
using Admin.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Admin.WebApi
{
    [Route("api/[controller]/")]
    [Authorize]
    [ApiController]
    public class EventController : ControllerBase
    {
        private readonly EventService _eventService;

        public EventController(EventService eventService)
        {
            _eventService = eventService;
        }
        
        
        [HttpGet]
        public async Task<IActionResult> GetEvents()
        {
            var test = _eventService.GetEvents();
            var result = test.Select(e => new EventsOutput
            {
                Id = e.Id,
                Title = e.Title,
                StartTime = e.StartTime,
                EndTime = e.EndTime,
                City = e.City,
                Status = GetStatusDescription.Description(e.Status),
                MemberId = e.MemberId,
                CreateTime = e.CreateTime,
                LastEditTime = e.LastEditTime
            });
            return Ok(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateEventAsync([FromBody] UpdateEventInput input)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            
            var affectedRow = await _eventService.UpdateEventAsync(input.Id, input.Status, input.Title);
            if(affectedRow is 0)
            {
                return NotFound();
            }
            
            return Ok(new {des =$"受影響的列:{affectedRow}"});
        }

        [HttpGet("DownloadEventsToJsonFile")]
        public async Task<IActionResult> DownloadEventsToJsonFile()
        {
            var result = _eventService.CreateJsonEvents();
            
            return Ok(result);
        }
    }
    
    public class EventsOutput
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string City { get; set; }
        public string Status { get; set; }
        public int MemberId { get; set; }
        public DateTime CreateTime  { get; set; }
        public DateTime? LastEditTime { get; set; }
    }

    public class UpdateEventInput
    {
        [Required]
        public int Id { get; set; }

        public string  Status { get; set; }

        public string? Title { get; set; }
        
    }
    
}


