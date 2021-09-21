using DepartureBoard.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Net;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using JsonApiSerializer;
using DepartureBoard.ViewModels;

namespace DepartureBoard.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        { 
            using (WebClient wc = new WebClient())
            {
                // get list of all north station (place-north) child stops (these are separate tracks/platforms in the case of commuter rail)
                string northStationStopJson = wc.DownloadString("https://api-v3.mbta.com/stops/place-north?include=child_stops");
                
                var northStationStop = JsonConvert.DeserializeObject<Stop>(northStationStopJson, new JsonApiSerializerSettings());

                // filter the list of child stops to only commuter rail (vehicle type = 2) and aggregrate the stop ids
                string stops = String.Join(",", northStationStop.ChildStops.Where(s => s.VehicleType == "2").Select(s => s.Id));

                // check for schedule/prediction records starting 5 minutes ago, to capture departed/departing status
                DateTime fiveMinutesAgo = DateTime.Now.Add(new TimeSpan(0, -5, 0));
                string fiveMinutesAgoString = fiveMinutesAgo.ToString("HH:mm");

                // get list of all prediction records for north station commuter rail platform departures (direction_id = 0)
                string predictionJson = wc.DownloadString($"https://api-v3.mbta.com/predictions?include=schedule,stop,trip&filter[stop]={stops}&filter[direction_id]=0");

                var predictions = JsonConvert.DeserializeObject<Prediction[]>(predictionJson, new JsonApiSerializerSettings());

                // exclude trains that departed more than 5 minutes ago, unless the status is something other than "departed", e.g. late
                predictions = predictions.Where(p => p.Schedule?.DepartureTime >= fiveMinutesAgo || p.Status != "Departed").ToArray();

                List<DepartureBoardRecord> departureBoardRecords = predictions.Select(p => new DepartureBoardRecord() { ScheduleId = p.Schedule?.Id, Timestamp = (DateTime)p.Schedule?.DepartureTime, Time = p.Schedule?.DepartureTime.ToString("h:mm tt"), Destination = p.Trip?.Destination, Track = (p.Stop?.PlatformCode) ?? "TBD", Status = p.Status, Train = p.Trip?.Name}).ToList();
                
                // get list of all CR scheduled departures (direction_id = 0) from North Station, for the remainder of the day
                string scheduleJson = wc.DownloadString($"https://api-v3.mbta.com/schedules?sort=departure_time&include=prediction,stop,trip&filter[min_time]={fiveMinutesAgoString}&filter[stop]={stops}&filter[direction_id]=0");

                var schedules = JsonConvert.DeserializeObject<Schedule[]>(scheduleJson, new JsonApiSerializerSettings());

                List<DepartureBoardRecord> departureBoardRecords2 = schedules.Select(s => new DepartureBoardRecord() { ScheduleId = s.Id, Timestamp = s.DepartureTime, Time = s.DepartureTime.ToString("h:mm tt"), Destination = s.Trip?.Destination, Track = (s.Stop?.PlatformCode) ?? "TBD", Status = (s.Prediction?.Status) ?? "On time", Train = s.Trip?.Name}).ToList();

                // combine schedule records with predictions, since predictions only cover a fraction of departures; use union to prevent duplication; the schedule id is used for comparison
                departureBoardRecords = departureBoardRecords.Union(departureBoardRecords2, new DepartureRecordEqualityComparer()).OrderBy(r => r.Timestamp).ToList();                               

                return View(new IndexViewModel() {DepartureBoardRecords = departureBoardRecords });                
            }
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
