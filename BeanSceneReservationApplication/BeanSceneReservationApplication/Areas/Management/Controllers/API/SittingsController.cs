using BeanSceneReservationApplication.Data;
using BeanSceneReservationApplication.Models.CalendarModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MongoDB.Bson;
using MongoDB.Bson.IO;
using Newtonsoft.Json.Serialization;

namespace BeanSceneReservationApplication.Areas.Management.Controllers.API
{
    [Authorize(Roles = "Staff,Manager")]
    [Route("api/[controller]")]
    [ApiController]
    public class SittingsController : ControllerBase
    {

        private readonly ApplicationDbContext _context;

        public SittingsController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<List<EventViewModel>> GetAsync(DateTime? start)
        {
            int days = 42;
            start = start.HasValue ? start.Value : DateTime.Now;
            var end = start.Value.AddDays(days);
            var events = await _context.Sittings
                .Where(s => s.StartTime > start && s.StartTime < end)
                .Select(s => new EventViewModel
                {
                    Title = s.Name,
                    Start = s.StartTime,
                    End = s.EndTime,
                    Editable = !s.Active,
                    BackgroundColor = s.Active ? "CornflowerBlue" : "red",
                    ExtendedProps = new ExtendedProps()
                    {
                        Id = s.Id,
                        Capacity = s.Capacity,
                        Active = s.Active,
                        Reservations = s.Reservations!.Select(r => new { r.Id, r.Duration, r.Person.Name }).ToList<object>()
                    }
                }).ToListAsync();

            return events;

        }

        [HttpPost]
        [Route("{sittingId}")]
        public IActionResult Post(int sittingId, [FromBody] Sitting sittingVM)
        {
            var sitting = _context.Sittings.FirstOrDefault(s => s.Id == sittingId);

            sitting.Name = sittingVM.Name;
            sitting.StartTime = sittingVM.StartTime;
            sitting.EndTime = sittingVM.EndTime;
            sitting.Capacity = sittingVM.Capacity;
            sitting.Active = sittingVM.Active;
            
            _context.SaveChanges();
            
            return Ok();
        }
        
        [HttpPost]
        [Route("active/{sittingId}")]
        public IActionResult PostActive(int sittingId, [FromBody] bool active)
        {
            var sitting = _context.Sittings.FirstOrDefault(s => s.Id == sittingId);

            sitting.Active = active;
            
            _context.SaveChanges();

            return Ok();
        }

        [HttpGet,Route("getType")]
        public SittingType[] GetType()
        {
            var sittingType = _context.SittingTypes.ToArray();

            return sittingType;
        }

        [HttpPost,Route("create")]
        public async Task<IActionResult> Create([FromBody] CreateSittingsVM s)
        {
            //Validation
            if (s == null)
            {
                return BadRequest("No data was sent.");
            }

            DateTime startTime = DateTime.Parse(s.StartTime);
            DateTime endTime = DateTime.Parse(s.EndTime);

            if (s.BatchStartDate > s.BatchEndDate)
            {
                ModelState.AddModelError("Dates", "The batch end date cannot be earlier than the batch start date.");
            }

            if (startTime >= endTime)
            {
                ModelState.AddModelError("Times", "The sitting end time cannot be before or the same as the sitting start time");
            }

            if (s.DaysOfWeek.Length < 1)
            {
                ModelState.AddModelError("Weekdays", "You must select at least one weekday.");
            }

            var type = _context.SittingTypes.FirstOrDefault(t => t.Name == s.TypeName);
            if (type == null)
            {
                return NotFound(s.TypeName);
            }

            if (ModelState.IsValid)
            {
                var batchDate = s.BatchStartDate;
                List<DayOfWeek> daysOfWeek = new List<DayOfWeek>();

                //Converts weekday strings into DayOfWeek enums stored in a list
                foreach (var day in s.DaysOfWeek)
                {
                    if (Enum.TryParse(day, true, out DayOfWeek parsedDay))
                    {
                        daysOfWeek.Add(parsedDay);
                    }
                }

                //For the duration of the provided time period, a sitting is created on each day of the week selected by the manager
                while (batchDate <= s.BatchEndDate)
                {
                    foreach (var day in daysOfWeek)
                    {
                        if (batchDate.DayOfWeek == day)
                        {
                            var sitting = new Sitting()
                            {
                                Name = s.Name,
                                SittingType = type,
                                StartTime = batchDate.Date + startTime.TimeOfDay,
                                EndTime = batchDate.Date + endTime.TimeOfDay,
                                Capacity = s.Capacity,
                                Active = s.Active,                      
                            };

                            _context.Sittings.Add(sitting);

                            //ends foreach loop if sitting for that date is created
                            break;
                        }
                    }

                    batchDate = batchDate.AddDays(1);
                }

                await _context.SaveChangesAsync();

                return Ok();
            }    

            return BadRequest(ModelState);
        }
        
        [HttpPost,Route("createType")]
        public IActionResult CreateType([FromBody] SittingType s)
        {
            var sittingType = new SittingType()
            {
                Name = s.Name
            };

            if (!_context.SittingTypes.Contains(_context.SittingTypes.FirstOrDefault(t=>t.Name==s.Name)))
            {
                _context.SittingTypes.Add(sittingType);
                _context.SaveChanges();
            }

            return Ok();
        }
        
        [HttpPost,Route("delete/{sittingId}")]
        public IActionResult Delete(int sittingId)
        {
            var sitting = _context.Sittings
                .Include(t=>t.Reservations)
                .FirstOrDefault(t => t.Id == sittingId);

            _context.Sittings.Remove(sitting);
            _context.SaveChanges();

            return Ok();
        }
    }
}