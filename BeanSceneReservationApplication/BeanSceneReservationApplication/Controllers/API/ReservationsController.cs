using BeanSceneReservationApplication.Data;
using BeanSceneReservationApplication.Models.ReservationListModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Data;

namespace BeanSceneReservationApplication.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReservationsController : Controller
    {
        private readonly ILogger<ReservationsController> _logger;
        private readonly ApplicationDbContext _context;

        public ReservationsController(ApplicationDbContext context, ILogger<ReservationsController> logger)
        {
            _logger = logger;
            _context = context;
        }

        [Route("get")]
        public List<DataModel> Get()
        {
            var RLmodel = _context.Reservations
                .Include(r => r.Person)
                .Include(r => r.Source)
                .Include(r=>r.AssignedTables)
                .Include(r=>r.Status)
                .Select(r => new DataModel()
                {
                    Id = r.Id,
                    Name = r.Person.Name,
                    Phone = r.Person.Phone,
                    Email = r.Person.Email,
                    Duration = r.Duration,
                    Guests = r.Guests,
                    Start = r.StartTime.ToString("yyyy-MM-dd hh:mm"),
                    Source = r.Source.Name,
                    Notes = r.Notes,
                    Status = r.Status.Name,
                    Tables = r.AssignedTables.Select(t => t.Name).ToArray()
                })
                .ToList();

            return RLmodel;
        }

        [Route("get/{sittingId}")]
        public List<DataModel> Get(int sittingId)
        {
            var sitting = _context.Sittings
                .Include(s => s.Reservations)
                    .ThenInclude(r => r.Person)
                .Include(s => s.Reservations)
                    .ThenInclude(r => r.Source)
                .Include(s => s.Reservations)
                    .ThenInclude(r => r.Status)
                .Include(s => s.Reservations)
                    .ThenInclude(r => r.Status)
                .Include(s => s.Reservations)
                    .ThenInclude(r => r.AssignedTables)
                .FirstOrDefault(s => s.Id == sittingId);

            var RLmodel = sitting
                .Reservations
                .Select(r => new DataModel()
                {
                    Id = r.Id,
                    Name = r.Person.Name,
                    Phone = r.Person.Phone,
                    Email = r.Person.Email,
                    Duration = r.Duration,
                    Guests = r.Guests,
                    Start = r.StartTime.ToString(),
                    Source = r.Source.Name,
                    Notes = r.Notes,
                    Status = r.Status.Name,
                    Tables = r.AssignedTables.Select(t => t.Name).ToArray()
                })
                .ToList();

            return RLmodel;
        }

        [Route("get/sitting/{sittingId}")]
        public SittingDataModel GetSitting(int sittingId)
        {
            var s = _context.Sittings
                .Include(s => s.Reservations)
                    .ThenInclude(r => r.Person)
                .Include(s => s.Reservations)
                    .ThenInclude(r => r.Source)
                .Include(s => s.Reservations)
                    .ThenInclude(r => r.Status)
                .FirstOrDefault(s => s.Id == sittingId);

            int currentCapacity = s.Capacity;
            foreach (var r in s.Reservations!)
            {
                currentCapacity -= r.Guests;
            }

            var sitting = _context.Sittings
                .Include(s => s.Reservations)
                    .ThenInclude(r => r.Person)
                .Include(s => s.Reservations)
                    .ThenInclude(r => r.Source)
                .Include(s => s.Reservations)
                    .ThenInclude(r => r.Status)
                .Select(s => new SittingDataModel()
                {
                    Id = s.Id,
                    Name = s.Name,
                    Type = s.SittingType.Name,
                    Start = s.StartTime.ToString("yyyy-MM-ddTHH:mm"),
                    End = s.EndTime.ToString("yyyy-MM-ddTHH:mm"),
                    Capacity = currentCapacity,
                    Active = s.Active,
                })
                .FirstOrDefault(s => s.Id == sittingId);

            return sitting;
        }
        
        [Route("get/source")]
        public List<ReservationSource> GetSource()
        {
            var source = _context.ReservationsSource.ToList();

            return source;
        }
        
        [Route("get/reservation/{reservationId}")]
        public CreateVM GetReservation(int reservationId)
        {
            var r = _context.Reservations
                .Include(r=>r.Person)
                .Include(r=>r.Source)
                .FirstOrDefault(r => r.Id == reservationId);
            var createVM = new CreateVM()
            {
                Name = r.Person.Name,
                Phone = r.Person.Phone,
                Email = r.Person.Email,
                Guests = r.Guests,
                Duration = r.Duration,
                StartTime = r.StartTime,
                Notes = r.Notes,
                SourceName = r.Source.Name,
            };

            return createVM;
        }
        
        [Route("{reservationId}/get/tables/{sittingId}")]
        public List<AreaVM> GetTables(int reservationId, int sittingId)
        {
            var reservations = _context.Sittings
                .Include(r=>r.Reservations)
                    .ThenInclude(r=>r.AssignedTables)
                .FirstOrDefault(s => s.Id == sittingId).Reservations;

            var rSelected = _context.Reservations
                .Include(r=>r.AssignedTables)
                .FirstOrDefault(r => r.Id == reservationId);

            var AreaVMs = _context.Areas
                .Select(a => new AreaVM()
                {
                    Name = a.Name,
                    TableVMs = a.RestaurantTables
                        .Select(t=> new TableVM()
                        {
                            Id = t.Id,
                            Name = t.Name,
                            Assigned = false
                        }).ToList()
                }).ToList();

            foreach (var areaVM in AreaVMs)
            {
                foreach (var r in reservations)
                {
                    foreach (var table in r.AssignedTables)
                    {
                        if (areaVM.TableVMs.Select(t => t.Id).Contains(table.Id))
                        {
                            areaVM.TableVMs.FirstOrDefault(t => t.Id == table.Id).Assigned = true;
                        };
                    };
                };

                foreach (var tSelected in rSelected.AssignedTables)
                {
                    if (areaVM.TableVMs.Select(t => t.Id).Contains(tSelected.Id))
                    {
                        areaVM.TableVMs.FirstOrDefault(t => t.Id == tSelected.Id).Assigned = false;
                    };
                };
            };

            return AreaVMs;
        }
        
        [Route("get/status/{sittingId}")]
        public List<ReservationStatus> GetStatus(int sittingId)
        {
            var s = _context.ReservationsStatus.ToList();

            return s;
        }

        [Route("{sittingId}/create")]
        [HttpPost]
        public async Task<IActionResult> Create(int sittingId, [FromBody] CreateVM c)
        {
            var person = new Person();

            if (_context.People.FirstOrDefault(p => p.Name == c.Name && p.Email == c.Email && p.Phone == c.Phone) != null)
            {
                person = _context.People.FirstOrDefault(p => p.Name == c.Name && p.Email == c.Email && p.Phone == c.Phone);
            }
            else
            {
                person = new Person()
                {
                    Name = c.Name,
                    Email = c.Email,
                    Phone = c.Phone
                };

                await _context.People.AddAsync(person);
            }

            var reservation = new Reservation()
            {
                Person = person,
                Guests = c.Guests,
                Duration = c.Duration,
                StartTime = c.StartTime,
                Source = _context.ReservationsSource.FirstOrDefault(s => s.Name == c.SourceName),
                Status = _context.ReservationsStatus.FirstOrDefault(s => s.Id == 1),
                Notes = c.Notes
            };

            await _context.Reservations.AddAsync(reservation);
            _context.Sittings.FirstOrDefault(s => s.Id == sittingId).Reservations.Add(reservation);

            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpPost]
        [Route("edit/{reservationId}")]
        public IActionResult Edit(int reservationId, [FromBody] CreateVM c)
        {
            var reservation = _context.Reservations.FirstOrDefault(r => r.Id == reservationId);

            var person = new Person();
            //if (_context.People.FirstOrDefault(p => p.Name == c.Name && p.Email == c.Email && p.Phone == c.Phone) != null)
            //{
            //    person = _context.People.First(p => p.Name == c.Name && p.Email == c.Email && p.Phone == c.Phone);
            //}
            //else
            //{
            //    person = new Person()
            //    {
            //        Name = c.Name,
            //        Email = c.Email,
            //        Phone = c.Phone
            //    };

            //    _context.People.Add(person);
            //}
            person = new Person()
            {
                Name = c.Name,
                Email = c.Email,
                Phone = c.Phone
            };

            reservation.Person = person;
            reservation.Guests = c.Guests;
            reservation.Duration = c.Duration;
            reservation.StartTime = c.StartTime;
            reservation.Notes = c.Notes;
            reservation.Source = _context.ReservationsSource.FirstOrDefault(s => s.Name == c.SourceName);

            _context.SaveChanges();

            return Ok();
        }

        [HttpPost]
        [Route("status/{reservationId}")]
        public IActionResult EditStatus(int reservationId, [FromBody] ReservationStatus Status)
        {
            var reservation = _context.Reservations
                .Include(r=>r.Status)
                .Include(r=>r.AssignedTables)
                .FirstOrDefault(r => r.Id == reservationId);
            reservation.Status = _context.ReservationsStatus.FirstOrDefault(s => s.Name == Status.Name);

            if (reservation.Status.Name != "Confirmed")
            {
                reservation.AssignedTables.Clear();
            };

            _context.SaveChanges();

            return Ok();
        }


        [Route("delete/{reservationId}")]
        public IActionResult Delete(int sittingId, int reservationId)
        {
            var reservation = _context.Reservations.FirstOrDefault(r => r.Id == reservationId);
            _context.Reservations.Remove(reservation);
            _context.SaveChanges();

            return Ok();
        }

        [Route("delete/all/{sittingId}")]
        public IActionResult DeleteAll(int sittingId)
        {
            var sitting = _context.Sittings
                .Include(s => s.Reservations)
                .FirstOrDefault(s => s.Id == sittingId);
            foreach (var r in sitting.Reservations)
            {
                _context.Reservations.Remove(r);
            }

            _context.SaveChanges();

            return Ok();
        }

        [Route("edit/table/{reservationId}")]
        public IActionResult EditTable(int reservationId, [FromBody] List<AreaVM> AreaVMs)
        {
            var r = _context.Reservations
                .Include(r=>r.AssignedTables)
                .FirstOrDefault(r => r.Id == reservationId);
            r.AssignedTables.Clear();
            foreach(var area in AreaVMs)
            {
                foreach (var table in area.TableVMs)
                {
                    if (table.Assigned == true)
                    {
                        var aTable = _context.RestaurantTables.FirstOrDefault(t => t.Id == table.Id);
                        r.AssignedTables.Add(aTable);
                    }
                }
            }
            r.Status = _context.ReservationsStatus.FirstOrDefault(s => s.Id == 2);

            _context.SaveChanges();

            return Ok();
        }
    }
}