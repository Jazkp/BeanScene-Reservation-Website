using BeanSceneReservationApplication.Data;
using BeanSceneReservationApplication.Models.ReservationModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace BeanSceneReservationApplication.Controllers
{
    [Authorize(Roles = "Manager")]
    public class SittingController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;

        public SittingController(ApplicationDbContext context, ILogger<HomeController> logger)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            var s = _context.Sittings
                .ToList();
            return View(s);
        }

        //AddSitting function
        public IActionResult Create()
        {
            var s = new CreateSittingVM
            {
                Sitting = new Sitting() { SittingType = new SittingType() },
                SittingType = _context.SittingTypes.ToList()
            };
            return View(s);
        }

        [HttpPost]
        public async Task<IActionResult> Check(CreateSittingVM sVM)
        {
            var sitting = sVM.Sitting;
            sitting.SittingType = _context.SittingTypes.FirstOrDefault(s => s.Id == sVM.Sitting.SittingType.Id);
            sitting.Active = true;

            _context.Sittings.Add(sitting);
            await _context.SaveChangesAsync();

            return RedirectToAction("Confirmation", new { id = sVM.Sitting.Id });
        }

        public IActionResult Confirmation(int id)
        {
            var s = _context.Sittings
                .Include(s => s.SittingType)
                .FirstOrDefault(s => s.Id == id);
            return View(s);
        }

        //Create Sitting Type
        public IActionResult CreateType()
        {
            var rType = new SittingType();

            return View(rType);
        }

        public async Task<IActionResult> SaveType(SittingType s)
        {
            var sittingType = s;

            bool covered = false;
            foreach (var type in _context.SittingTypes)
            {
                if (sittingType.Name == type.Name)
                {
                    covered = true;
                }
            }

            if (!covered)
            {
                _context.SittingTypes.Add(sittingType);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("Create");
        }

        //Edit Sitting
        public IActionResult SelectEdit()
        {
            var s = _context.Sittings
                .Include(s=>s.Reservations)
                    .ThenInclude(r=>r.Status)
                .Include(s => s.SittingType);

            return View(s);
        }

        [Route("/{sittingId}/Edit")]
        public IActionResult Edit(int sittingId)
        {
            var s = _context.Sittings
                .Include(s => s.SittingType)
                .Include(s => s.Reservations)
                .FirstOrDefault(s => s.Id == sittingId);

            var cs = new CreateSittingVM
            {
                Sitting = s,
                SittingType = _context.SittingTypes.ToList()
            };

            return View(cs);
        }

        [HttpPost]
        public async Task<IActionResult> CheckEdit(CreateSittingVM csvm)
        {
            var Sitting = _context.Sittings
                            .Include(s => s.SittingType)
                            .Include(s => s.Reservations)
                            .FirstOrDefault(s => s.Id == csvm.Sitting.Id);

            var reservations = Sitting.Reservations.ToList();
                            

            _context.Sittings.Remove(Sitting);

            var EditSitting = new Sitting
            {
                Name = csvm.Sitting.Name,
                SittingType = new SittingType(),
                StartTime = csvm.Sitting.StartTime,
                EndTime = csvm.Sitting.EndTime,
                Capacity = csvm.Sitting.Capacity,
                Reservations = new List<Reservation>(),
                Active = true
            };
            EditSitting.SittingType = _context.SittingTypes.FirstOrDefault(s => s.Id == csvm.Sitting.SittingType.Id);

            foreach (var reservation in reservations)
            {
                EditSitting.Reservations.Add(_context.Reservations.FirstOrDefault(r => r.Id == reservation.Id));
            }


            _context.Sittings.Add(EditSitting);

            await _context.SaveChangesAsync();

            return RedirectToAction("Confirmation", new { id = EditSitting.Id });
        }

        //Delete Sitting
        [Route("/{sittingId}/Delete")]
        public IActionResult Delete(int sittingId)
        {
            var sitting = _context.Sittings
                .Include(s=>s.SittingType)
                .FirstOrDefault(s => s.Id == sittingId);
            return View(sitting);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteTask(Sitting sitting)
        {
            var s = _context.Sittings
                .Include(s => s.SittingType)
                .Include(s => s.Reservations)
                .FirstOrDefault(s => s.Id == sitting.Id);

            var reservations = s.Reservations.ToList();

            foreach (var reservation in reservations)
            {
                _context.Reservations.Remove(reservation);
            }

            _context.Sittings.Remove(s);

            await _context.SaveChangesAsync();

            return RedirectToAction("DeleteConfirmation");
        }

        public IActionResult DeleteConfirmation()
        {
            return View();
        }
    }
}
