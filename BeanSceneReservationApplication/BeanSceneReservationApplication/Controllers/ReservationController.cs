using BeanSceneReservationApplication.Data;
using BeanSceneReservationApplication.Models.ReservationModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BeanSceneReservationApplication.Controllers
{
    public class ReservationController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;

        public ReservationController(ApplicationDbContext context, ILogger<HomeController> logger)
        {
            _logger = logger;
            _context = context;
        }

        /*Note: Which is better?
            After push Create Reservation buttn,    
            1. firstly select date, and display sitting at the date in the next page
            2. display all sitting
         */
        [Route("BeanScene/SelectSitting")]
        public IActionResult SelectSitting()
        {
            var s = _context.Sittings
                .Include(s => s.SittingType)
                .Include(s => s.Reservations)
                    .ThenInclude(r => r.Status);
            return View(s);
        }

        [Route("BeanScene/{sittingId}/Create")]
        public IActionResult Create(int sittingId)
        {
            var sitting = _context.Sittings
                .Include(s => s.Reservations)
                    .ThenInclude(r => r.Status)
                .FirstOrDefault(s => s.Id == sittingId);
            var r = new Reservation() { Person = new Person(), Source = new ReservationSource() };
            var rModel = new ReservationVM()
            {
                Reservation = r,
                Sitting = sitting
            };

            return View(rModel);
        }

        [HttpPost]
        public async Task<IActionResult> Check(ReservationVM r)
        {
            var reservation = r.Reservation;
            var person = r.Reservation.Person;
            var s = _context.ReservationsSource.FirstOrDefault(s => s.Id == r.Reservation.Source.Id);

            var existPerson = _context.People.FirstOrDefault(p=>p.Email==person.Email);

            _context.People.Add(person);
            
            reservation.Source = s;
            reservation.Status = _context.ReservationsStatus.FirstOrDefault(s => s.Id == 1);
            reservation.Person = person;

            _context.Reservations.Add(reservation);

            var sitting = _context.Sittings.FirstOrDefault(s => s.Id == r.Sitting.Id);
            sitting.Reservations.Add(reservation);

            await _context.SaveChangesAsync();

            return RedirectToAction("Confirmation", new { id = r.Reservation.Id });
        }

        public IActionResult Confirmation(int id)
        {
            var reservation = _context.Reservations
                .Include(r => r.Source)
                .Include(r => r.Status)
                .Include(r => r.Person)
                .FirstOrDefault(r => r.Id == id);

            return View(reservation);
        }

        //Editing reservation 
        [Route("BeanScene/Select")]
        public IActionResult Select()
        {
            var r = _context.Reservations
                .OrderBy(r => r.Status)
                    .ThenBy(r => r.StartTime)
                .Include(r => r.Status)
                .Include(r => r.Person);
            return View(r);
        }

        [Route("BeanScene/{reservationId}/SelectEditAction")]
        public IActionResult SelectEditAction(int reservationId)
        {
            var r = _context.Reservations
                .Include(r => r.Status)
                .Include(r => r.Source)
                .Include(r=>r.Person)
                .Include(r=>r.AssignedTables)
                .FirstOrDefault(r => r.Id == reservationId);

            return View(r);
        }

        //Assign Table
        [Route("BeanScene/{reservationId}/AssignTable")]
        public IActionResult AssignTable(int reservationId)
        {
            var reservation = _context.Reservations
                    .Include(r => r.Source)
                    .Include(r => r.Status)
                    .Include(r => r.Person)
                    .FirstOrDefault(r => r.Id == reservationId);

            var sitting = _context.Sittings
                    .Include(s => s.Reservations)
                        .ThenInclude(r => r.Status)
                    .Include(s => s.Reservations)
                        .ThenInclude(r => r.AssignedTables)
                    .Include(s => s.Reservations)
                        .ThenInclude(r => r.Source)
                    .FirstOrDefault(s => s.StartTime <= reservation.StartTime && s.EndTime >= reservation.StartTime);

            var e = new EditReservationVM()
            {
                Tables = _context.RestaurantTables.ToArray(),
                Reservation = reservation,

                TableInfos = _context.RestaurantTables
                    .Select(t => new ReservationTableInfo()
                    {
                        TableId = t.Id,
                        Selected = false
                    })
                    .ToArray(),

                ReservedTableIds = new List<int>()
            };

            foreach (var r in sitting.Reservations)
            {
                foreach (var table in r.AssignedTables)
                {
                    e.ReservedTableIds.Add(table.Id);
                }
            }

            return View(e);
        }

        [HttpPost]
        public async Task<IActionResult> CheckAssign(EditReservationVM e)
        {
            var reservation = _context.Reservations
                .Include(r => r.Status)
                .Include(r => r.Source)
                .Include(r => r.Person)
                .Include(r => r.AssignedTables)
                .FirstOrDefault(r => r.Id == e.Reservation.Id);
            
            foreach (var et in e.TableInfos)
            {
                if (et.Selected)
                {
                    var table = _context.RestaurantTables.FirstOrDefault(t => t.Id == et.TableId);
                    reservation.AssignedTables.Add(table);
                }
            }
            reservation.Status = _context.ReservationsStatus.FirstOrDefault(s => s.Id == 2);
            await _context.SaveChangesAsync();

            return RedirectToAction("EditConfirmation", new { id = e.Reservation.Id });
        }

        public IActionResult EditConfirmation(int id)
        {
            var reservation = _context.Reservations
                .Include(r => r.Status)
                .Include(r => r.Source)
                .Include(r => r.Person)
                .Include(r => r.AssignedTables)
                .FirstOrDefault(r => r.Id == id);

            return View(reservation);
        }

        //Cancel Reservation
        [Route("BeanScene/{reservationId}/Cancel")]
        public IActionResult Cancel(int reservationId)
        {
            var reservation = _context.Reservations
                .Include(r => r.Source)
                .Include(r => r.Status)
                .Include(r => r.Person)
                .Include(r => r.AssignedTables)
                .FirstOrDefault(r => r.Id == reservationId);
            return View(reservation);
        }

        [Route("BeanScene/{reservationId}/CancelConfirmation")]
        public IActionResult CancelConfirmation(int reservationId)
        {
            var reservation = _context.Reservations
                .Include(r => r.Source)
                .Include(r => r.Status)
                .Include(r => r.Person)
                .Include(r => r.AssignedTables)
                .FirstOrDefault(r => r.Id == reservationId);

            reservation.Status = _context.ReservationsStatus.FirstOrDefault(s => s.Id == 3);
            _context.SaveChanges();

            return View(reservation);
        }

        //Edit Reservation
        [Route("BeanScene/{reservationId}/Edit")]
        public IActionResult Edit(int reservationId)
        {
            var reservation = _context.Reservations
                .Include(r => r.Source)
                .Include(r => r.Status)
                .Include(r => r.Person)
                .Include(r => r.AssignedTables)
                .FirstOrDefault(r => r.Id == reservationId);

            var sitting = _context.Sittings
                    .Include(s => s.Reservations)
                        .ThenInclude(r => r.Status)
                    .Include(s => s.Reservations)
                        .ThenInclude(r => r.Source)
                    .FirstOrDefault(s => s.StartTime <= reservation.StartTime && s.EndTime >= reservation.StartTime);

            var r = new ReservationVM
            {
                Reservation = reservation,
                Sitting = sitting
            };
            return View(r);
        }

        [HttpPost]
        public async Task<IActionResult> CheckEdit(ReservationVM rVM)
        {
            var reservation = _context.Reservations
                .Include(r => r.Source)
                .Include(r => r.Status)
                .Include(r => r.Person)
                .Include(r => r.AssignedTables)
                .FirstOrDefault(r => r.Id == rVM.Reservation.Id);

            _context.Reservations.Remove(reservation);

            var EditReservation = new Reservation
            {
                Person = reservation.Person,
                StartTime = rVM.Reservation.StartTime,
                Duration = rVM.Reservation.Duration,
                Notes = rVM.Reservation.Notes,
                Guests = rVM.Reservation.Guests,
                Source = _context.ReservationsSource.FirstOrDefault(s => s.Id == rVM.Reservation.Source.Id),
                Status = _context.ReservationsStatus.FirstOrDefault(s => s.Id == rVM.Reservation.Status.Id),
                AssignedTables = new List<RestaurantTable>()
            };
            foreach (var item in reservation.AssignedTables)
            {
                var table = _context.RestaurantTables.FirstOrDefault(t => t.Id == item.Id);
                EditReservation.AssignedTables.Add(table);
            }
            _context.Sittings.FirstOrDefault(s => s.Id == rVM.Sitting.Id).Reservations.Add(EditReservation);

            await _context.SaveChangesAsync();

            return RedirectToAction("EditConfirmation", new { id = EditReservation.Id });
        }
    }
}
