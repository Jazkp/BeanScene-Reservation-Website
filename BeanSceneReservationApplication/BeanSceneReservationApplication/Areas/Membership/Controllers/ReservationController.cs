using BeanSceneReservationApplication.Data;
using BeanSceneReservationApplication.Models.ReservationModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace BeanSceneReservationApplication.Areas.Membership.Controllers
{
    [Area("Membership")]
    public class ReservationController : Controller
    {
        private readonly ILogger<ReservationController> _logger;
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public ReservationController(ApplicationDbContext context, ILogger<ReservationController> logger, UserManager<IdentityUser> userManager)
        {
            _logger = logger;
            _context = context;
            _userManager = userManager;
        }

        [Route("Reservation")]
        public IActionResult CreateReservation()
        {
            return View();
        }

        [HttpPost]
        [Route("CreateReservation")]
        public async Task<IActionResult> CreateReservation(ReservationVM r)
        {
            if (r == null)
            {
                return BadRequest("No data recieved.");
            }

            //Fields intentionally not passed from form, this code prevents ModelState from being incorrectly invalid
            ModelState.Remove("Reservation.Status");
            ModelState.Remove("Reservation.Source");
            ModelState.Remove("Sitting.Name");

            //Establishes necessary variables
            var reservation = r.Reservation;
            var person = r.Reservation.Person;
            var checkPerson = _context.People.FirstOrDefault(p => p.Email == person.Email);
            var sitting = _context.Sittings
                                    .Include(s => s.SittingType)
                                    .Include(s => s.Reservations)
                                        .ThenInclude(r => r.Status)
                                    .FirstOrDefault(s => s.Id == r.Sitting.Id);
            int seatsRemaining = sitting.Capacity - sitting.Reservations.Where(r => r.Status.Id != 3).Sum(r => r.Guests); //Sum of pending and confirmed reservation guest numbers

            //Returns 404 if posted sitting Id does not match an existing sitting
            if (sitting == null)
            {
                return NotFound("The sitting was not found.");
            }

            //matches Date of StartTime DateTime field to match sitting Date
            reservation.StartTime = sitting.StartTime.Date + reservation.StartTime.TimeOfDay;

            //Validation
            if (reservation.StartTime < sitting.StartTime)
            {
                ModelState.AddModelError("Reservation.StartTime", "A reservation cannot begin before the sitting.");
            }

            var endTime = reservation.StartTime.AddMinutes(r.Reservation.Duration);
            if (endTime > sitting.EndTime)
            {
                ModelState.AddModelError("Reservation.StartTime", "A reservation must end by the time the sitting ends.");
            }

            if (reservation.Guests > seatsRemaining)
            {
                ModelState.AddModelError("Reservation.Guests", "No. of guests for a reservation cannot exceed the remaining capacity of seating for the sitting. It may be possible that capacity has reduced since the sitting was selected.");
            }

            if (reservation.Guests < 1)
            {
                ModelState.AddModelError("Reservation.Guests", "No. of guests for a reservation cannot be less than 1.");
            }

            if (reservation.Duration < 30)
            {
                ModelState.AddModelError("Reservation.Duration", "Reservations must go for at least 30 mins.");
            }

            //Perform CRUD if ModelState is valid, only runs if clientside and serverside validation is passed
            if (ModelState.IsValid)
            {
                //Checks to see if reservation has been made under this email before
                if (checkPerson == null)
                {
                    //Checks to see if a member with this email address exists
                    var member = await _userManager.FindByEmailAsync(person.Email);

                    //Links member account to person account by Id
                    if (member != null)
                    {
                        person.UserId = member.Id;
                    }

                    //Adds new person to database
                    reservation.Person = person;
                    _context.People.Add(person);
                }
                else
                {
                    //Existing person inserted into reservation Person field
                    reservation.Person = checkPerson;

                    //If a different name or phone number was given for an existing email, they are stored in an optional field
                    if (person.Name != checkPerson.Name)
                    {
                        reservation.PersonName = person.Name;
                    }
                    if (person.Phone != checkPerson.Phone)
                    {
                        reservation.PersonPhone = person.Phone;
                    }
                }

                //Status is Pending and source is Online
                var status = _context.ReservationsStatus.FirstOrDefault(s => s.Id == 1);
                var source = _context.ReservationsSource.FirstOrDefault(s => s.Id == 1);

                reservation.Status = status;
                reservation.Source = source;

                _context.Reservations.Add(reservation);
                sitting.Reservations.Add(reservation);

                await _context.SaveChangesAsync();

                r.Reservation = reservation;
                r.Sitting = sitting;

                TempData["ReservationVM"] = JsonConvert.SerializeObject(r);

                return RedirectToAction("Confirmation", "Reservation");
            }

            r.Sitting = sitting;

            ViewData["SeatsRemaining"] = seatsRemaining;

            return View(r);
        }

        public IActionResult Confirmation()
        {
            string? reservationVMJson = TempData["ReservationVM"] as string;
            ReservationVM? r = JsonConvert.DeserializeObject<ReservationVM>(reservationVMJson);

            return View(r);
        }
    }
}
