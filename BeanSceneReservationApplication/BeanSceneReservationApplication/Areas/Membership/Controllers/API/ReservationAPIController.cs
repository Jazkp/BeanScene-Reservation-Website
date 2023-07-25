using BeanSceneReservationApplication.Data;
using BeanSceneReservationApplication.Models.CalendarModel;
using BeanSceneReservationApplication.Models.ReservationModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BeanSceneReservationApplication.Areas.Membership.Controllers.API
{
    [ApiController]
    [Route("api/reservation")]
    public class ReservationAPIController : Controller
    {

        private readonly ApplicationDbContext _context;

        public ReservationAPIController(ApplicationDbContext context)
        {
            _context = context;
        }


        [HttpGet]
        [Route("getsittings")]
        public async Task<List<ReservationEventVM>> GetSittingsAsync(DateTime? start)
        {
            int days = 42;
            start = start.HasValue ? start.Value : DateTime.Now;
            var end = start.Value.AddDays(days);
            var events = await _context.Sittings
                .Where(s => s.StartTime > start && s.StartTime < end)
                .Where(s => s.Active == true)
                .Select(s => new ReservationEventVM
                {
                    Title = s.Name,
                    Start = s.StartTime,
                    End = s.EndTime,
                    Editable = false,
                    BackgroundColor = s.Capacity > s.Reservations
                                                        .Where(r => r.Status.Id != 3)
                                                        .Sum(r => r.Guests) ? "CornflowerBlue" : "red",
                    ExtendedProps = new ReservationExtendedProps()
                    {
                        SittingId = s.Id,
                        SeatsRemaining = s.Capacity - s.Reservations.Where(r => r.Status.Id != 3).Sum(r => r.Guests)
                    }
                }).ToListAsync();
                

            return events;

        }
    }
}
