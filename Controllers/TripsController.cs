using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PRN211_Project_Group_4.Models;

namespace PRN211_Project_Group_4.Controllers
{
    public class TripsController : Controller
    {
        private readonly PRN211Context _context;

        public TripsController(PRN211Context context)
        {
            _context = context;
        }

        // GET: Trips
        public async Task<IActionResult> Index()
        {
            var pRN211Context = _context.Trips.Include(t => t.Route.End).Include(t => t.Route.Start).Include(t => t.Route).Include(t => t.Train).Include(t=>t.Train.Type);
            ViewData["Train"] = new SelectList(_context.Trains, "TrainId", "Name");
            var routeList = _context.RouteTrains.Select(rt => new SelectListItem
            {
                Value = rt.RouteId.ToString(),
                Text = (rt.Start.Location1 + " -> " + rt.End.Location1)
            }).ToList();


            ViewData["Route"] = new SelectList(routeList, "Value", "Text");
            ViewData["numberOfTrips"] = pRN211Context.Count();
            return View(await pRN211Context.ToListAsync());
        }

        // GET: Trips/Details/5
        [HttpPost]
        public async Task<IActionResult> Index(DateTime selectedDate, int selectedTrain, int selectedRoute)
        {
            var list = _context.Trips
                .Include(s => s.Route.Start)
                .Include(s=> s.Route.End)
                .Include(s => s.Train)
                .Where(s => s.TrainId == selectedTrain
                && s.RouteId == selectedRoute
                && s.Date == selectedDate);
            ViewData["Train"] = new SelectList(_context.Trains, "TrainId", "Name");
            var routeList = _context.RouteTrains.Select(rt => new SelectListItem
            {
                Value = rt.RouteId.ToString(),
                Text = (rt.Start.Location1 + " -> " + rt.End.Location1)
            }).ToList();


            ViewData["Route"] = new SelectList(routeList, "Value", "Text");
            ViewData["numberOfTrips"] = list.Count();
            return View(await list.ToListAsync());
        }
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Trips == null)
            {
                return NotFound();
            }

            var trip = await _context.Trips
                .Include(t => t.Route.Start)
                .Include(t => t.Route.End)
                .Include(t => t.Train)
                .FirstOrDefaultAsync(m => m.TripId == id);
            if (trip == null)
            {
                return NotFound();
            }
            string route = trip.Route.Start.Location1 + " -> " + trip.Route.End.Location1;
          
            ViewBag.Route = route;
            return View(trip);
        }

        // GET: Trips/Create
        public IActionResult Create(int? trainId, int? routeId, DateTime? date) 
        {
            if (trainId == null || routeId == null || date == null)
            {
                trainId = _context.Wagons.FirstOrDefault().TrainId;
                routeId = _context.RouteTrains.FirstOrDefault().RouteId;
                date = date ?? DateTime.UtcNow;
            }
            var existingSlots = _context.Trips
            .Where(s => s.TrainId == trainId && s.RouteId == routeId)
            .Select(s => s.Slot)
            .Distinct()
            .ToList();
            var validSlots = Enumerable.Range(1, 12).Select(s => (int?)s).Except(existingSlots).ToList();
            var routeList = _context.RouteTrains.Select(rt => new SelectListItem
            {
                Value = rt.RouteId.ToString(),
                Text = (rt.Start.Location1 + " -> " + rt.End.Location1)
            }).ToList();


            ViewData["Route"] = new SelectList(routeList, "Value", "Text", routeId);

            ViewData["Slot"] = new SelectList(validSlots);
           
            ViewData["Train"] = new SelectList(_context.Trains, "TrainId", "Name",trainId);
            return View();
        }

        // POST: Trips/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("TripId,Slot,TrainId,Price,Date,RouteId,Status")] Trip trip)
        {
            if (ModelState.IsValid)
            {
                _context.Add(trip);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["RouteId"] = new SelectList(_context.RouteTrains, "RouteId", "RouteId", trip.RouteId);
            ViewData["TrainId"] = new SelectList(_context.Trains, "TrainId", "TrainId", trip.TrainId);
            return View(trip);
        }

        // GET: Trips/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Trips == null)
            {
                return NotFound();
            }

            var trip = await _context.Trips.FindAsync(id);
            if (trip == null)
            {
                return NotFound();
            }
            int? trainId = trip.TrainId;
            int? routeId = trip.RouteId;
            if (trainId == null || routeId == null)
            {

                trainId = _context.Wagons.FirstOrDefault().TrainId;
                routeId = _context.RouteTrains.FirstOrDefault().RouteId;
            }
            var existingSlots = _context.Trips
                .Where(s => s.TripId != id && s.TrainId == trainId && s.RouteId == routeId && s.Date == trip.Date)
                .Select(s => s.Slot)
                .Distinct()
                .ToList();

            var validSlots = Enumerable.Range(1, 12).Select(s => (int?)s).Except(existingSlots).ToList();

            ViewData["Slot"] = new SelectList(validSlots);
            ViewData["Train"] = new SelectList(_context.Wagons, "WagonId", "Name", trainId);
            ViewData["Route"] = new SelectList(_context.RouteTrains, "RouteId", "RouteId", routeId);

            return View(trip);
        }
        [HttpGet]
        public IActionResult GetAvailableSlots(int trainId, int routeId, DateTime date)
        {
            var existingSlots = _context.Trips
                .Where(s => s.TrainId == trainId && s.RouteId == routeId && s.Date == date)
                .Select(s => s.Slot)
                .Distinct()
                .ToList();
            var validSlots = Enumerable.Range(1, 12).Select(s => (int?)s).Except(existingSlots).ToList();

            return Json(validSlots);
        }
        // POST: Trips/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("TripId,Slot,TrainId,Price,Date,RouteId,Status")] Trip trip)
        {
            if (id != trip.TripId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var tripBefore = _context.Trips
                        .Include(t => t.Train)
                        .Include(t => t.Route)
                        .FirstOrDefault(s => s.TripId == id);
                    tripBefore.Slot = trip.Slot;
                    tripBefore.Price = trip.Price;
                    tripBefore.RouteId = trip.RouteId;

                    var routeTrain = _context.RouteTrains
                        .FirstOrDefault(r => r.RouteId == tripBefore.RouteId);
                    if (routeTrain != null)
                    {
                        tripBefore.Route = routeTrain;
                    }
                    _context.Update(tripBefore);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TripExists(trip.TripId))
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
            ViewData["RouteId"] = new SelectList(_context.RouteTrains, "RouteId", "RouteId", trip.RouteId);
            ViewData["TrainId"] = new SelectList(_context.Trains, "TrainId", "TrainId", trip.TrainId);
            return View(trip);
        }

        // GET: Trips/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Trips == null)
            {
                return NotFound();
            }

            var trip = await _context.Trips
                .Include(t => t.Route)
                .Include(t => t.Train)
                .FirstOrDefaultAsync(m => m.TripId == id);
            if (trip == null)
            {
                return NotFound();
            }

            return View(trip);
        }

        // POST: Trips/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Trips == null)
            {
                return Problem("Entity set 'PRN211Context.Trips'  is null.");
            }
            var trip = await _context.Trips.FindAsync(id);
            if (trip != null)
            {
                _context.Trips.Remove(trip);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TripExists(int id)
        {
          return (_context.Trips?.Any(e => e.TripId == id)).GetValueOrDefault();
        }
    }
}
