using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PRN211_Project_Group_4.Models;

namespace G4.Controllers
{
    public class CommonController : Controller
    {
        private readonly PRN211Context _context;

        public CommonController(PRN211Context context)
        {
            _context = context;
        }


        // GET: CommonController
        [HttpGet]
        public ActionResult Index(int? tripId, int? trainId)
        {
            ViewData["tripId"] = tripId;
            ViewData["trainId"] = trainId;
            return View();
        }

        [HttpGet]
        public ActionResult BookSeat(int WagonId, int TripId, int TrainId)
        {
            var booking = _context.Bookings.Where(b => b.TripId == TripId).ToList();
            List<string> wagonSeats = new List<string>();
            foreach (var item in booking)
            {
                var wagon = item.SeatStatus[0] + "";
                if (item != null && wagon.Equals(Convert.ToString(WagonId)))
                {
                    wagonSeats.Add(item.SeatStatus.Substring(1, 48));
                }
            }
            string result = AddBinaryStrings(wagonSeats);
            ViewData["seatStatus"] = result;
            ViewData["WagonId"] = WagonId;
            ViewData["TripId"] = TripId;
            ViewData["TrainId"] = TrainId;
            return View();
        }

        string AddBinaryStrings(List<string> binaryStrings)
        {
            if (binaryStrings == null || binaryStrings.Count == 0)
            {
                return string.Empty;
            }

            string sum = binaryStrings[0];

            for (int i = 1; i < binaryStrings.Count; i++)
            {
                sum = AddBinaryStrings(sum, binaryStrings[i]);
            }

            return sum;
        }

        string AddBinaryStrings(string str1, string str2)
        {
            int carry = 0;
            char[] result = new char[Math.Max(str1.Length, str2.Length) + 1];
            int index = result.Length - 1;

            for (int i = str1.Length - 1, j = str2.Length - 1; i >= 0 || j >= 0 || carry > 0; i--, j--)
            {
                int bit1 = (i >= 0) ? str1[i] - '0' : 0;
                int bit2 = (j >= 0) ? str2[j] - '0' : 0;

                int sum = bit1 + bit2 + carry;
                carry = sum / 2;
                result[index--] = (char)((sum % 2) + '0');
            }

            return new string(result, index + 1, result.Length - index - 1);
        }

        [HttpPost]
        public ActionResult AddNewTicket(string selectedSeat, int tripId, int trainId)
        {
            try
            {
                Booking booking = new Booking();
                booking.SeatStatus = selectedSeat;
                booking.TripId = tripId;
                int count = 0;
                for (int i = 1; i < selectedSeat.Length; i++)
                {
                    if (selectedSeat[i].Equals('1'))
                    {
                        count++;
                    }
                }
                var trip = _context.Trips.FirstOrDefault(t => t.TripId == tripId);
                booking.Amount = Convert.ToDouble(trip.Price * ount);
                booking.AccountId = 2;
                _context.Bookings.Add(booking);
                _context.SaveChanges();

                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, error = ex.Message });
            }
        }

        [HttpGet]
        public ActionResult MyTicket()
        {
            var myTickets = _context.Bookings
                .Where(b => b.AccountId == 2)
                .ToList();
            List<Trip> trips = new List<Trip>();
            foreach (var item in myTickets)
            {
                var myTrip = _context.Trips
                    .Include(t => t.Train)
                    .Include(t => t.Route)
                    .FirstOrDefault(t => t.TripId == item.TripId);
                var routeTrain = _context.RouteTrains.Include(r => r.Start).Include(r => r.End).FirstOrDefault(r => r.RouteId == myTrip.Route.RouteId);
                var test = routeTrain;
                myTrip.Route = routeTrain;
                item.Trip = myTrip;
                trips.Add(myTrip);
            }
            ViewData["myTicket"] = myTickets;
            ViewData["myTrips"] = trips;
            return View();
        }

        [HttpPost]
        public ActionResult DeleteTrip(int tripId)
        {
            try
            {
                var trip = _context.Trips.Include(t => t.Bookings).FirstOrDefault(t => t.TripId == tripId);

                if (trip != null)
                {
                    _context.Bookings.RemoveRange(trip.Bookings);
                    _context.SaveChanges();
                }

                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, error = ex.Message });
            }
        }

        [HttpGet]
        public ActionResult TicketDetail(int? id)
        {
            var booking = _context.Bookings
                .Include(b => b.Trip)
                .Include(b => b.Trip.Train)
                .Include(b => b.Trip.Route)
                .Include(b => b.Trip.Route.Start)
                .Include(b => b.Trip.Route.End)
                .Include(b=>b.Trip.Train.Type)
                .Include(b=>b.Account)
                .FirstOrDefault(b => b.BookId == id);
            

            return View(booking);
        }


        // GET: CommonController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: CommonController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: CommonController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: CommonController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: CommonController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: CommonController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: CommonController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
