#nullable disable
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Reservation2.Models;

namespace Reservation2
{
    public class ReservationsController : Controller
    {
        private readonly DbContext _context;

        public ReservationsController(DbContext context)
        {
            _context = context;
        }

        public class ReservationClass
        {
            public string Start { get; set; }
            public string End { get; set; }
        }

        [HttpGet]
        public List<ReservationClass> GetReservationDateRangesJson()
        {

            var resList = new List<ReservationClass>();

            foreach (var reservation in _context.Reservation)
            {
                var start = reservation.StartOfReservation.AddDays(1).ToString("yyyy-MM-dd");
                var end = reservation.EndOfReservation.AddDays(-1).ToString("yyyy-MM-dd");

                var res = new ReservationClass
                {
                    Start = start,
                    End = end,
                };

                resList.Add(res);

            }
            return resList;
        }

        [HttpGet]
        public List<string> GetReservedDays()
        {

            var reserved_days = new List<string>();

            foreach (var reservation in _context.Reservation)
            {
                var start = reservation.StartOfReservation.AddDays(1);
                var end = reservation.EndOfReservation.AddDays(-1);

                var daysInRange = (end - start).TotalDays;

                if (daysInRange > 0)
                {
                    for (var dt = start; dt <= end; dt = dt.AddDays(1))
                    {
                        reserved_days.Add(dt.ToString("yyyy-MM-dd"));
                    }
                }
            }
            return reserved_days;

        }

        // GET: Reservations
        public async Task<IActionResult> Index()
        {
            return View(await _context.Reservation.ToListAsync());
        }

        // GET: Reservations/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reservation = await _context.Reservation
                .FirstOrDefaultAsync(m => m.Id == id);
            if (reservation == null)
            {
                return NotFound();
            }

            return View(reservation);
        }

        // GET: Reservations/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Reservations/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Surname,Phone,Email,StartOfReservation,EndOfReservation,Prepayment,Payment,IsPaid,AdditionalInfo,Notes")] ReservationModel reservation)
        {
            bool validationError = false;

            if (ModelState.IsValid)
            {

                var start = reservation.StartOfReservation;
                var end = reservation.EndOfReservation;

                //if(start.Date == end.Date)
                //{
                //    validationError = true;
                //    ModelState.AddModelError("StartOfReservation", "Start jest taki sam jak koniec.");
                //    ModelState.AddModelError("EndOfReservation", "Start jest taki sam jak koniec.");
                //}

                foreach (var reservedDay in GetReservedDays())
                {
                    var day = DateTime.Parse(reservedDay);
                    if (start < day && day < end)
                    {
                        validationError = true;
                        ModelState.AddModelError("StartOfReservation", "W podanym zakresie isnieje reserwacja.");
                        ModelState.AddModelError("EndOfReservation", "W podanym zakresie isnieje reserwacja.");
                    }
                }

                if (!validationError)
                {
                    _context.Add(reservation);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
            }
            return View(reservation);
        }

        // GET: Reservations/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reservation = await _context.Reservation.FindAsync(id);
            if (reservation == null)
            {
                return NotFound();
            }
            return View(reservation);
        }

        // POST: Reservations/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Surname,Phone,Email,StartOfReservation,EndOfReservation,Prepayment,Payment,IsPaid,AdditionalInfo,Notes")] ReservationModel reservation)
        {
            if (id != reservation.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(reservation);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ReservationExists(reservation.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(reservation);
        }

        // GET: Reservations/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reservation = await _context.Reservation
                .FirstOrDefaultAsync(m => m.Id == id);
            if (reservation == null)
            {
                return NotFound();
            }

            return View(reservation);
        }

        // POST: Reservations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var reservation = await _context.Reservation.FindAsync(id);
            _context.Reservation.Remove(reservation);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ReservationExists(int id)
        {
            return _context.Reservation.Any(e => e.Id == id);
        }
    }
}
